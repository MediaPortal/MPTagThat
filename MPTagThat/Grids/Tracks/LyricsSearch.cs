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

    private const int NoOfCurrentSearchesAllowed = 6;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly string[] _strippedPrefixStrings = {"the ", "les "};
    private readonly string[] _titleBrackets = {"{}", "[]", "()"};

    private readonly List<TrackData> _tracks;

    private BackgroundWorker _bgWorkerLyrics;
    private GridViewColumnsLyrics _gridColumns;
    private LyricsController _lc;
    
    private Queue _lyricsQueue;
    private ManualResetEvent _eventStopThread;
    private Thread _lyricControllerThread;
    private List<string> _sitesToSearch;

    #endregion

    #region Delegates

    public delegate void DelegateLyricFound(String s, String artist, String track, String site, int row);

    public delegate void DelegateLyricNotFound(String artist, String title, String message, String site, int row);

    public delegate void DelegateStringUpdate(String message, String site);

    public delegate void DelegateThreadException(String s);

    public delegate void DelegateThreadFinished(String artist, String title, String message, String site);

    public DelegateLyricFound _delegateLyricFound;
    public DelegateLyricNotFound _delegateLyricNotFound;
    public DelegateStringUpdate _delegateStringUpdate;
    public DelegateThreadException _delegateThreadException;
    public DelegateThreadFinished _delegateThreadFinished;

    #endregion

    #region Properties

    public DataGridView GridView => dataGridViewLyrics;

    #endregion

    #region ctor

    public LyricsSearch(List<TrackData> tracks)
    {
      _tracks = tracks;
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
      _gridColumns = new GridViewColumnsLyrics();

      // Setup Dataview Grid
      dataGridViewLyrics.AutoGenerateColumns = false;
      dataGridViewLyrics.DataSource = _tracks;

      // Now Setup the columns, we want to display
      CreateColumns();

      Localisation();

      _sitesToSearch = Options.MainSettings.LyricSites;

      // initialize delegates
      _delegateLyricFound = lyricFound;
      _delegateLyricNotFound = lyricNotFound;
      _delegateThreadFinished = threadFinished;
      _delegateThreadException = threadException;

      _eventStopThread = new ManualResetEvent(false);

      _bgWorkerLyrics = new BackgroundWorker();
      _bgWorkerLyrics.DoWork += bgWorkerLyrics_DoWork;
      _bgWorkerLyrics.ProgressChanged += bgWorkerLyrics_ProgressChanged;
      _bgWorkerLyrics.RunWorkerCompleted += bgWorkerLyrics_RunWorkerCompleted;
      _bgWorkerLyrics.WorkerSupportsCancellation = true;

      lbFinished.Visible = false;

      _lyricsQueue = new Queue();

      dataGridViewLyrics.ReadOnly = true;

      foreach (TrackData track in _tracks)
      {
        string[] lyricId = new string[] {track.Artist, track.Title};
        _lyricsQueue.Enqueue(lyricId);
      }
      _bgWorkerLyrics.RunWorkerAsync();
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
      foreach (GridViewColumn column in _gridColumns.Settings.Columns)
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

        _gridColumns.SaveColumnSettings(column, i);
        i++;
      }
      _gridColumns.SaveSettings();
    }

    #endregion

    #endregion

    #region Lyrics Worker Thread

    private void bgWorkerLyrics_DoWork(object sender, DoWorkEventArgs e)
    {
      if (_lyricsQueue.Count > 0)
      {
        // start running the lyricController
        _lc = new LyricsController(this, _eventStopThread, _sitesToSearch.ToArray(), false, false, "", "")
        {
          NoOfLyricsToSearch = _lyricsQueue.Count
        };

        ThreadStart runLyricController = delegate { _lc.Run(); };
        _lyricControllerThread = new Thread(runLyricController);
        _lyricControllerThread.Start();

        _lc.StopSearches = false;


        int row = 0;
        while (_lyricsQueue.Count != 0)
        {
          if (_lc == null)
            return;

          if (_lc.NoOfCurrentSearches < NoOfCurrentSearchesAllowed && _lc.StopSearches == false)
          {
            string[] lyricId = (string[])_lyricsQueue.Dequeue();

            if (Options.MainSettings.SwitchArtist)
              lyricId[0] = SwitchArtist(lyricId[0]);

            _lc.AddNewLyricSearch(lyricId[0], TrimTitle(lyricId[1]), GetStrippedPrefixArtist(lyricId[0], _strippedPrefixStrings),
                                 row);
            row++;
          }

          Thread.Sleep(100);
        }
      }
      else
      {
        ThreadFinished = new object[] {"", "", localisation.ToString("lyricssearch", "NothingToSearch"), ""};
      }
    }

    private string GetStrippedPrefixArtist(string artist, string[] strippedPrefixStringArray)
    {
      foreach (string s in strippedPrefixStringArray)
      {
        if (artist.Trim().ToLowerInvariant().StartsWith(s))
        {
          artist = artist.Substring(s.Length);
          break;
        }
      }
      return artist;
    }

    /// <summary>
    /// Switches the Artist, if it is separated with a "colon"
    /// </summary>
    /// <param name="artist"></param>
    /// <returns></returns>
    private string SwitchArtist(string artist)
    {
      int iPos = artist.IndexOf(',');
      if (iPos > 0)
      {
        artist = $"{artist.Substring(iPos + 2)} {artist.Substring(0, iPos)}";
      }
      return artist;
    }

    /// <summary>
    /// Cleans the title before submitting for Lyrics search
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    private string TrimTitle(string title)
    {
      foreach (string s in _titleBrackets)
      {
        if (title.Trim().EndsWith(s.Substring(1,1)))
        {
          var startPos = title.LastIndexOf(s.Substring(0, 1), StringComparison.Ordinal);
          if (startPos > 0)
          {
            title = title.Substring(0, startPos).Trim();
          }
          break;
        }
      }
      return title;
    }

    private void bgWorkerLyrics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {}

    private void bgWorkerLyrics_ProgressChanged(object sender, ProgressChangedEventArgs e) {}

    #endregion

    // Stop worker thread if it is running.
    // Called when user presses Stop button of form is closed.
    private void StopThread()
    {
      if (_lyricControllerThread != null && _lyricControllerThread.IsAlive) // thread is active
      {
        _eventStopThread.Set();

        // wait when thread  will stop or finish
        while (_lyricControllerThread.IsAlive)
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
      if (_lc != null)
      {
        _lc.StopSearches = true;
      }

      // Due to a bug, we first need the view to readonly, otherwise the checkbox in the first column is not displayed as checked.
      dataGridViewLyrics.ReadOnly = false;
      dataGridViewLyrics.Refresh();
      _bgWorkerLyrics.CancelAsync();
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
      if (_lc != null)
      {
        _lc.StopSearches = true;
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
          Invoke(_delegateStringUpdate, value);
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
            Invoke(_delegateLyricFound, value);
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
            Invoke(_delegateLyricNotFound, value);
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
            Invoke(_delegateThreadFinished, value);
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
            Invoke(_delegateThreadException);
          }
          catch (InvalidOperationException) {}
          ;
        }
      }
    }

    #endregion
  }
}