#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion
#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using LyricsEngine;
using MPTagThat.Core;
using Queue = System.Collections.Queue;

#endregion

namespace MPTagThat.GridView
{
  public partial class LyricsSearch : ShapedForm, ILyricForm
  {
    #region Variables

    private const int m_NoOfCurrentSearchesAllowed = 6;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly string[] m_strippedPrefixStrings = {"the", "les"};

    private readonly List<TrackData> tracks;

    private BackgroundWorker bgWorkerLyrics;
    private GridViewColumnsLyrics gridColumns;
    private LyricsController lc;
    private NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private Queue lyricsQueue;
    private ManualResetEvent m_EventStopThread;
    private Thread m_LyricControllerThread;
    private List<string> sitesToSearch;

    #endregion

    #region Delegates

    public delegate void DelegateLyricFound(String s, String artist, String track, String site, int row);

    public delegate void DelegateLyricNotFound(String artist, String title, String message, String site, int row);

    public delegate void DelegateStringUpdate(String message, String site);

    public delegate void DelegateThreadException(String s);

    public delegate void DelegateThreadFinished(String artist, String title, String message, String site);

    #endregion

    public DelegateLyricFound m_DelegateLyricFound;
    public DelegateLyricNotFound m_DelegateLyricNotFound;
    public DelegateStringUpdate m_DelegateStringUpdate;

    public DelegateThreadException m_DelegateThreadException;
    public DelegateThreadFinished m_DelegateThreadFinished;

    #region Properties

    public DataGridView GridView
    {
      get { return dataGridViewLyrics; }
    }

    #endregion

    #region ctor

    public LyricsSearch(List<TrackData> tracks)
    {
      this.tracks = tracks;
      InitializeComponent();
    }

    #endregion

    #region Methods

    #region Form Load

    private void OnLoad(object sender, EventArgs e)
    {
      log.Trace(">>>");
      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      // Load the Settings
      gridColumns = new GridViewColumnsLyrics();

      // Setup Dataview Grid
      dataGridViewLyrics.AutoGenerateColumns = false;
      dataGridViewLyrics.DataSource = tracks;

      // Now Setup the columns, we want to display
      CreateColumns();

      Localisation();

      sitesToSearch = Options.MainSettings.LyricSites;

      // initialize delegates
      m_DelegateLyricFound = new DelegateLyricFound(lyricFound);
      m_DelegateLyricNotFound = new DelegateLyricNotFound(lyricNotFound);
      m_DelegateThreadFinished = new DelegateThreadFinished(threadFinished);
      m_DelegateThreadException = new DelegateThreadException(threadException);

      m_EventStopThread = new ManualResetEvent(false);

      bgWorkerLyrics = new BackgroundWorker();
      bgWorkerLyrics.DoWork += bgWorkerLyrics_DoWork;
      bgWorkerLyrics.ProgressChanged += bgWorkerLyrics_ProgressChanged;
      bgWorkerLyrics.RunWorkerCompleted += bgWorkerLyrics_RunWorkerCompleted;
      bgWorkerLyrics.WorkerSupportsCancellation = true;

      lbFinished.Visible = false;

      lyricsQueue = new Queue();

      dataGridViewLyrics.ReadOnly = true;

      foreach (TrackData track in tracks)
      {
        string[] lyricId = new string[2] {track.Artist, track.Title};
        lyricsQueue.Enqueue(lyricId);
      }
      bgWorkerLyrics.RunWorkerAsync();
      log.Trace("<<<");
    }

    private void Localisation()
    {
      Text = localisation.ToString("lyricssearch", "Heading");
    }

    #region Gridlayout

    /// <summary>
    ///   Create the Columns of the Grid based on the users setting
    /// </summary>
    private void CreateColumns()
    {
      // Now create the columns 
      foreach (GridViewColumn column in gridColumns.Settings.Columns)
      {
        dataGridViewLyrics.Columns.Add(Util.FormatGridColumn(column));
      }

      // Add a dummy column and set the property of the last column to fill
      DataGridViewColumn col = new DataGridViewTextBoxColumn();
      col.Name = "dummy";
      col.HeaderText = "";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 1;
      dataGridViewLyrics.Columns.Add(col);
      dataGridViewLyrics.Columns[dataGridViewLyrics.Columns.Count - 1].AutoSizeMode =
        DataGridViewAutoSizeColumnMode.Fill;
    }

    /// <summary>
    ///   Save the settings
    /// </summary>
    private void SaveSettings()
    {
      // Save the Width of the Columns
      int i = 0;
      foreach (DataGridViewColumn column in dataGridViewLyrics.Columns)
      {
        // Don't save the dummy column
        if (i == dataGridViewLyrics.Columns.Count - 1)
          break;

        gridColumns.SaveColumnSettings(column, i);
        i++;
      }
      gridColumns.SaveSettings();
    }

    #endregion

    #endregion

    #region Lyrics Worker Thread

    private void bgWorkerLyrics_DoWork(object sender, DoWorkEventArgs e)
    {
      if (lyricsQueue.Count > 0)
      {
        // start running the lyricController
        lc = new LyricsController(this, m_EventStopThread, sitesToSearch.ToArray(), false, false, "", "");

        lc.NoOfLyricsToSearch = lyricsQueue.Count;
        ThreadStart runLyricController = delegate { lc.Run(); };
        m_LyricControllerThread = new Thread(runLyricController);
        m_LyricControllerThread.Start();

        lc.StopSearches = false;


        int row = 0;
        while (lyricsQueue.Count != 0)
        {
          if (lc == null)
            return;

          if (lc.NoOfCurrentSearches < m_NoOfCurrentSearchesAllowed && lc.StopSearches == false)
          {
            string[] lyricID = (string[])lyricsQueue.Dequeue();

            if (Options.MainSettings.SwitchArtist)
              lyricID[0] = SwitchArtist(lyricID[0]);

            lc.AddNewLyricSearch(lyricID[0], lyricID[1], GetStrippedPrefixArtist(lyricID[0], m_strippedPrefixStrings),
                                 row);
            row++;
          }

          Thread.Sleep(100);
        }
      }
      else
      {
        ThreadFinished = new[] {"", "", localisation.ToString("lyricssearch", "NothingToSearch"), ""};
      }
    }

    private string GetStrippedPrefixArtist(string artist, string[] strippedPrefixStringArray)
    {
      foreach (string s in strippedPrefixStringArray)
      {
        int index = artist.IndexOf(s);
        if (index != -1)
        {
          string prefix = artist.Substring(index + 2);
          artist = prefix + " " + artist.Replace(s, "");
          break;
        }
      }
      return artist;
    }

    private void bgWorkerLyrics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {}

    private void bgWorkerLyrics_ProgressChanged(object sender, ProgressChangedEventArgs e) {}

    #endregion

    // Stop worker thread if it is running.
    // Called when user presses Stop button of form is closed.
    private void StopThread()
    {
      if (m_LyricControllerThread != null && m_LyricControllerThread.IsAlive) // thread is active
      {
        m_EventStopThread.Set();

        // wait when thread  will stop or finish
        while (m_LyricControllerThread.IsAlive)
        {
          // We cannot use here infinite wait because our thread
          // makes syncronous calls to main form, this will cause deadlock.
          // Instead of this we wait for event some appropriate time
          // (and by the way give time to worker thread) and
          // process events. These events may contain Invoke calls.
          Application.DoEvents();
          break;
        }
      }
    }

    private string SwitchArtist(string artist)
    {
      int iPos = artist.IndexOf(',');
      if (iPos > 0)
      {
        artist = String.Format("{0} {1}", artist.Substring(iPos + 2), artist.Substring(0, iPos));
      }
      return artist;
    }

    #endregion

    #region Delegate Called Methods

    private void lyricFound(String lyricStrings, String artist, String title, String site, int row)
    {
      dataGridViewLyrics.Rows[row].Cells[0].Value = true;
      dataGridViewLyrics.Rows[row].Cells[1].Value = localisation.ToString("lyricssearch", "FoundLyrics");
      dataGridViewLyrics.Rows[row].Cells[5].Value = lyricStrings;
    }

    private void lyricNotFound(String artist, String title, String message, String site, int row)
    {
      dataGridViewLyrics.Rows[row].Cells[1].Value = localisation.ToString("lyricssearch", "NoLyricsFound");
    }

    private void threadFinished(string artist, string title, string message, string site)
    {
      lbStatus.Visible = false;
      lbFinished.Visible = true;
      if (lc != null)
      {
        lc.StopSearches = true;
      }

      // Due to a bug, we first need the view to readonly, otherwise the checkbox in the first column is not displayed as checked.
      dataGridViewLyrics.ReadOnly = false;
      dataGridViewLyrics.Refresh();
      bgWorkerLyrics.CancelAsync();
      StopThread();
    }

    private void threadException(Object o) {}

    #endregion

    #region Event Handler

    private void btUpdate_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      if (lc != null)
      {
        lc.StopSearches = true;
      }
      StopThread();
      Close();
    }

    #endregion

    #region ILyricForm implementation

    public Object[] UpdateString
    {
      set
      {
        if (IsDisposed == false)
        {
          Invoke(m_DelegateStringUpdate, value);
        }
      }
    }

    public Object[] UpdateStatus
    {
      set { }
    }

    public Object[] LyricFound
    {
      set
      {
        if (IsDisposed == false)
        {
          try
          {
            Invoke(m_DelegateLyricFound, value);
          }
          catch (InvalidOperationException) {}
          ;
        }
      }
    }

    public Object[] LyricNotFound
    {
      set
      {
        if (IsDisposed == false)
        {
          try
          {
            Invoke(m_DelegateLyricNotFound, value);
          }
          catch (InvalidOperationException) {}
          ;
        }
      }
    }

    public Object[] ThreadFinished
    {
      set
      {
        if (IsDisposed == false)
        {
          try
          {
            Invoke(m_DelegateThreadFinished, value);
          }
          catch (InvalidOperationException) {}
          ;
        }
      }
    }

    public string ThreadException
    {
      set
      {
        if (IsDisposed == false)
        {
          try
          {
            Invoke(m_DelegateThreadException);
          }
          catch (InvalidOperationException) {}
          ;
        }
      }
    }

    #endregion
  }
}