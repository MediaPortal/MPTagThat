using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using LyricsEngine;
using MPTagThat.Core;

namespace MPTagThat.TagEdit
{
  public partial class LyricsSearch : Telerik.WinControls.UI.ShapedForm, ILyricForm
  {
    #region Delegates
    public delegate void DelegateStringUpdate(String message, String site);
    public DelegateStringUpdate m_DelegateStringUpdate;
    public delegate void DelegateStatusUpdate(Int32 noOfLyricsToSearch, Int32 noOfLyricsSearched, Int32 noOfLyricsFound, Int32 noOfLyricsNotFound);
    public DelegateStatusUpdate m_DelegateStatusUpdate;
    public delegate void DelegateLyricFound(String s, String artist, String track, String site, int row);
    public DelegateLyricFound m_DelegateLyricFound;
    public delegate void DelegateLyricNotFound(String artist, String title, String message, String site, int row);
    public DelegateLyricNotFound m_DelegateLyricNotFound;
    public delegate void DelegateThreadFinished(String arist, String title, String message, String site);
    public DelegateThreadFinished m_DelegateThreadFinished;
    public delegate void DelegateThreadException(Object o);
    public DelegateThreadException m_DelegateThreadException;
    #endregion

    #region Variables
    LyricsController lc;

    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();

    // worker thread
    Thread m_LyricControllerThread;

    ManualResetEvent m_EventStopThread;

    string originalArtist;
    string originalTitle;
    int counter;
    bool m_automaticFetch = true;
    bool m_automaticUpdate = false;

    string[] m_strippedPrefixStrings = { "the", "les" };

    List<string> sitesToSearch;
    object parent = null;
    #endregion

    #region ctor
    public LyricsSearch(object parent, string artist, string title, bool automaticUpdate)
    {
      InitializeComponent();

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.parent = parent;
      m_automaticUpdate = automaticUpdate;

      // initialize delegates
      m_DelegateStringUpdate = new DelegateStringUpdate(this.updateStringMethod);
      m_DelegateStatusUpdate = new DelegateStatusUpdate(this.updateStatusMethod);
      m_DelegateLyricFound = new DelegateLyricFound(this.lyricFoundMethod);
      m_DelegateLyricNotFound = new DelegateLyricNotFound(this.lyricNotFoundMethod);
      m_DelegateThreadFinished = new DelegateThreadFinished(this.ThreadFinishedMethod);
      m_DelegateThreadException = new DelegateThreadException(this.ThreadExceptionMethod);

      // initialize events
      m_EventStopThread = new ManualResetEvent(false);

      tbArtist.Text = artist;
      tbTitle.Text = title;

      if (Options.MainSettings.SwitchArtist)
        btSwitchArtist_Click(this.btSwitchArtist, new EventArgs());

      originalArtist = tbArtist.Text;
      originalTitle = title;

      LocaliseScreen();

      BeginSearchIfPossible(artist, title);

      this.ShowDialog();
    }
    #endregion

    #region Methods
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Text = String.Format(localisation.ToString("LyricsSearch", "Header"), tbArtist.Text, tbTitle.Text);
      this.chSite.Text = localisation.ToString("LyricsSearch", "Site");
      this.chResult.Text = localisation.ToString("LyricsSearch", "Result");
      this.chLyric.Text = localisation.ToString("LyricsSearch", "Lyric");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    internal void BeginSearchIfPossible(string artist, string title)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      if (artist.Length != 0 && title.Length != 0)
      {
        if (m_automaticFetch)
        {
          lvSearchResults.Focus();
          fetchLyric(originalArtist, originalTitle, m_automaticUpdate);
        }
        else
        {
          btFind.Focus();
        }
      }
      else if (artist.Length != 0)
      {
        tbTitle.Focus();
      }
      else
      {
        tbArtist.Focus();
      }
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    private void lockGUI()
    {
      btFind.Enabled = false;
      btCancel.Enabled = true;
      btClose.Enabled = false;
    }

    private void openGUI()
    {
      btFind.Enabled = true;
      btCancel.Enabled = false;
      btClose.Enabled = true;
    }

    private void fetchLyric(string artist, string title, bool automaticUpdate)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      lockGUI();
      tbLyrics.Text = "";
      lvSearchResults.Items.Clear();

      counter = 0;

      sitesToSearch = new List<string>();

      if (Options.MainSettings.SearchLyricWiki)
        sitesToSearch.Add("LyricWiki");

      if (Options.MainSettings.SearchHotLyrics)
        sitesToSearch.Add("HotLyrics");

      if (Options.MainSettings.SearchLyrics007)
        sitesToSearch.Add("Lyrics007");

      if (Options.MainSettings.SearchLyricsOnDemand)
        sitesToSearch.Add("LyricsOnDemand");

      if (Options.MainSettings.SearchSeekLyrics)
        sitesToSearch.Add("SeekLyrics");

      // If automaticUpdate is set then return after the first positive search
      lc = new LyricsController(this, m_EventStopThread, (string[])sitesToSearch.ToArray(), true, automaticUpdate, "", "");

      ThreadStart job = delegate
      {
        lc.Run();
      };

      m_LyricControllerThread = new Thread(job);
      m_LyricControllerThread.Name = "lyricSearch Thread";	// looks nice in Output window
      m_LyricControllerThread.Start();

      lc.AddNewLyricSearch(artist, title, GetStrippedPrefixArtist(artist, m_strippedPrefixStrings));
      Util.LeaveMethod(Util.GetCallingMethod());
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

    private void stopSearch()
    {
      Monitor.Enter(this);
      try
      {
        if (lc != null)
        {
          lc.FinishThread(originalArtist, originalTitle, "", "");
          lc.Dispose();
          lc = null;
        }
        else
        {
          m_EventStopThread.Set();
          ThreadFinishedMethod(originalArtist, originalTitle, "", "");
        }

        m_LyricControllerThread = null;
      }
      finally
      {
        Monitor.Exit(this);
      }
    }
    #endregion

    #region Events
    private void btFind_Click(object sender, EventArgs e)
    {
      string artist = tbArtist.Text.Trim();
      string title = tbTitle.Text.Trim();

      if (artist.Length != 0 && title.Length != 0)
      {
        fetchLyric(artist, title, m_automaticUpdate);
      }
      else if (artist.Length == 0)
      {
        tbArtist.Focus();
      }
      else
      {
        tbTitle.Focus();
      }
    }

    private void btClose_Click(object sender, EventArgs e)
    {
      stopSearch();
      this.Close();
    }

    private void lvSearchResults_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lvSearchResults.SelectedItems.Count > 0)
      {
        tbLyrics.Text = LyricsEngine.LyricUtil.ReturnEnvironmentNewLine(lvSearchResults.SelectedItems[0].SubItems[2].Text);
        if (tbLyrics.Text.Length != 0)
        {
          btUpdate.Enabled = true;
        }
        else
        {
          btUpdate.Enabled = false;
        }
      }
      else
      {
        btUpdate.Enabled = false;
      }
    }

    private void btUpdate_Click(object sender, EventArgs e)
    {
      UpdateSong();
      this.Close();
    }

    private void lvSearchResults_DoubleClick(object sender, EventArgs e)
    {
      btUpdate.PerformClick();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      stopSearch();
      openGUI();
    }


    private void btSwitchArtist_Click(object sender, EventArgs e)
    {
      string artist = tbArtist.Text;
      int iPos = artist.IndexOf(',');
      if (iPos > 0)
      {
        tbArtist.Text = String.Format("{0} {1}", artist.Substring(iPos + 2), artist.Substring(0, iPos));
      }
    }
    #endregion

    #region delegate called methods
    // Called from worker thread using delegate and Control.Invoke
    private void updateStringMethod(String message, String site)
    {
    }

    // Called from worker thread using delegate and Control.Invoke
    private void updateStatusMethod(Int32 noOfLyricsToSearch, Int32 noOfLyricsSearched, Int32 noOfLyricsFound, Int32 noOfLyricsNotFound)
    {
    }

    private void lyricFoundMethod(String lyricStrings, String artist, String title, String site, int row)
    {
      ListViewItem item = new ListViewItem(site);
      item.SubItems.Add("yes");
      item.SubItems.Add(lyricStrings);
      lvSearchResults.Items.Add(item);
      lvSearchResults.Items[lvSearchResults.Items.Count - 1].Selected = true;

      if (m_automaticUpdate)
      {
        stopSearch();
        UpdateSong();
        this.Close();
      }
      else if (++counter == sitesToSearch.Count)
      {
        stopSearch();
        openGUI();
      }
    }

    private void lyricNotFoundMethod(String artist, String title, String message, String site, int row)
    {
      ListViewItem item = new ListViewItem(site);
      item.SubItems.Add("no");
      item.SubItems.Add("");
      lvSearchResults.Items.Add(item);

      if (++counter == sitesToSearch.Count)
      {
        stopSearch();
        openGUI();
        btClose.Focus();
      }
    }

    private void UpdateSong()
    {
      if (lvSearchResults.SelectedItems.Count > 0)
      {
        if (parent.GetType() == typeof(MPTagThat.TagEdit.SingleTagEdit))
        {
          (parent as SingleTagEdit).LyricsText = LyricsEngine.LyricUtil.ReturnEnvironmentNewLine(lvSearchResults.SelectedItems[0].SubItems[2].Text);
        }
      }
    }


    // Set initial state of controls.
    // Called from worker thread using delegate and Control.Invoke
    private void ThreadFinishedMethod(string artist, string title, string message, string site)
    {
    }

    private void ThreadExceptionMethod(Object o)
    {
    }

    #endregion

    #region DelegateCalls
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
        //if (this.IsDisposed == false)
        //{
        //    this.Invoke(m_DelegateStatusUpdate, value);
        //}
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