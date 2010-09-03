using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MPTagThat.Core;
using LyricsEngine;

namespace MPTagThat.GridView
{
  public partial class LyricsSearch : ShapedForm, ILyricForm
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();

    private List<TrackData> tracks;
    private GridViewColumnsLyrics gridColumns;

    private BackgroundWorker bgWorkerLyrics;
    private System.Collections.Queue lyricsQueue;
    private LyricsController lc = null;
    private List<string> sitesToSearch;

    Thread m_LyricControllerThread;
    ManualResetEvent m_EventStopThread;
    const int m_NoOfCurrentSearchesAllowed = 6;

    string[] m_strippedPrefixStrings = { "the", "les" };
    #endregion

    #region Delegates
    public delegate void DelegateStringUpdate(String message, String site);
    public DelegateStringUpdate m_DelegateStringUpdate;
    public delegate void DelegateLyricFound(String s, String artist, String track, String site, int row);
    public DelegateLyricFound m_DelegateLyricFound;
    public delegate void DelegateLyricNotFound(String artist, String title, String message, String site, int row);
    public DelegateLyricNotFound m_DelegateLyricNotFound;
    public delegate void DelegateThreadFinished(String artist, String title, String message, String site);
    public DelegateThreadFinished m_DelegateThreadFinished;
    public delegate void DelegateThreadException(String s);
    public DelegateThreadException m_DelegateThreadException;
    #endregion

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
      Util.EnterMethod(Util.GetCallingMethod());
      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      // Load the Settings
      gridColumns = new GridViewColumnsLyrics();

      // Setup Dataview Grid
      dataGridViewLyrics.AutoGenerateColumns = false;
      dataGridViewLyrics.DataSource = tracks;

      // Now Setup the columns, we want to display
      CreateColumns();

      Localisation();

      sitesToSearch = new List<string>();

      if (Options.MainSettings.SearchLyricWiki)
        sitesToSearch.Add("LyricWiki");

      if (Options.MainSettings.SearchHotLyrics)
        sitesToSearch.Add("HotLyrics");

      if (Options.MainSettings.SearchLyrics007)
        sitesToSearch.Add("Lyrics007");

      if (Options.MainSettings.SearchLyricsOnDemand)
        sitesToSearch.Add("LyricsOnDemand");

      if (Options.MainSettings.SearchLyricsPlugin)
        sitesToSearch.Add("LyricsPluginSite");

      if (Options.MainSettings.SearchActionext)
        sitesToSearch.Add("Actionext");

      if (Options.MainSettings.SearchLyrDB)
        sitesToSearch.Add("LyrDB");

      if (Options.MainSettings.SearchLRCFinder)
        sitesToSearch.Add("LrcFinder");

      // initialize delegates
      m_DelegateLyricFound = new DelegateLyricFound(this.lyricFound);
      m_DelegateLyricNotFound = new DelegateLyricNotFound(this.lyricNotFound);
      m_DelegateThreadFinished = new DelegateThreadFinished(this.threadFinished);
      m_DelegateThreadException = new DelegateThreadException(this.threadException);

      m_EventStopThread = new ManualResetEvent(false);

      bgWorkerLyrics = new BackgroundWorker();
      bgWorkerLyrics.DoWork += new DoWorkEventHandler(bgWorkerLyrics_DoWork);
      bgWorkerLyrics.ProgressChanged += new ProgressChangedEventHandler(bgWorkerLyrics_ProgressChanged);
      bgWorkerLyrics.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerLyrics_RunWorkerCompleted);

      lbFinished.Visible = false;

      lyricsQueue = new System.Collections.Queue();

      dataGridViewLyrics.ReadOnly = true;

      foreach (TrackData track in tracks)
      {
        string[] lyricId = new string[2] { track.Artist, track.Title };
        lyricsQueue.Enqueue(lyricId);
      }
      bgWorkerLyrics.RunWorkerAsync();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    #region Gridlayout
    /// <summary>
    /// Create the Columns of the Grid based on the users setting
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
      col.Width = 5;
      dataGridViewLyrics.Columns.Add(col);
      dataGridViewLyrics.Columns[dataGridViewLyrics.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    /// <summary>
    /// Save the settings
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

    private void Localisation()
    {
      this.Text = localisation.ToString("lyricssearch", "Heading");
    }
    #endregion

    #region Lyrics Worker Thread
    void bgWorkerLyrics_DoWork(object sender, DoWorkEventArgs e)
    {
      if (lyricsQueue.Count > 0)
      {
        // start running the lyricController
        lc = new LyricsController(this, m_EventStopThread, (string[])sitesToSearch.ToArray(), false, false, "", "");

        lc.NoOfLyricsToSearch = lyricsQueue.Count;
        ThreadStart runLyricController = delegate
        {
          lc.Run();
        };
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

            lc.AddNewLyricSearch(lyricID[0], lyricID[1], GetStrippedPrefixArtist(lyricID[0], m_strippedPrefixStrings), row);
            row++;
          }

          Thread.Sleep(100);
        }
      }
      else
      {
        ThreadFinished = new string[] { "", "", localisation.ToString("lyricssearch", "NothingToSearch"), "" };
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

    void bgWorkerLyrics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
    }

    void bgWorkerLyrics_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
    }
    #endregion

    // Stop worker thread if it is running.
    // Called when user presses Stop button of form is closed.
    private void StopThread()
    {
      if (m_LyricControllerThread != null && m_LyricControllerThread.IsAlive)  // thread is active
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

    private void threadException(Object o)
    {
    }
    #endregion

    #region Event Handler
    private void btUpdate_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      lc.StopSearches = true;
      StopThread();
      this.Close();
    }
    #endregion

    #region ILyricForm implementation
    public Object[] UpdateString
    {
      set
      {
        if (this.IsDisposed == false)
        {
          this.Invoke(m_DelegateStringUpdate, value);
        }
      }
    }
    public Object[] UpdateStatus
    {
      set
      {
      }
    }
    public Object[] LyricFound
    {
      set
      {
        if (this.IsDisposed == false)
        {
          try
          {
            this.Invoke(m_DelegateLyricFound, value);
          }
          catch (System.InvalidOperationException) { };
        }
      }
    }
    public Object[] LyricNotFound
    {
      set
      {
        if (this.IsDisposed == false)
        {
          try
          {
            this.Invoke(m_DelegateLyricNotFound, value);
          }
          catch (System.InvalidOperationException) { };
        }
      }
    }
    public Object[] ThreadFinished
    {
      set
      {
        if (this.IsDisposed == false)
        {
          try
          {
            this.Invoke(m_DelegateThreadFinished, value);
          }
          catch (System.InvalidOperationException) { };
        }
      }
    }

    public string ThreadException
    {
      set
      {
        if (this.IsDisposed == false)
        {
          try
          {
            this.Invoke(m_DelegateThreadException);
          }
          catch (System.InvalidOperationException) { };
        }
      }
    }
    #endregion
  }
}
