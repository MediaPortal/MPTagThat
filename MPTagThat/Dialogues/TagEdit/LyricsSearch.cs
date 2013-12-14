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
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using LyricsEngine;
using MPTagThat.Core;

#endregion

namespace MPTagThat.TagEdit
{
  public partial class LyricsSearch : ShapedForm, ILyricForm
  {
    #region Delegates

    public delegate void DelegateLyricFound(String s, String artist, String track, String site, int row);

    public delegate void DelegateLyricNotFound(String artist, String title, String message, String site, int row);

    public delegate void DelegateStatusUpdate(
      Int32 noOfLyricsToSearch, Int32 noOfLyricsSearched, Int32 noOfLyricsFound, Int32 noOfLyricsNotFound);

    public delegate void DelegateStringUpdate(String message, String site);

    public delegate void DelegateThreadException(Object o);

    public delegate void DelegateThreadFinished(String arist, String title, String message, String site);

    #endregion

    public DelegateLyricFound m_DelegateLyricFound;
    public DelegateLyricNotFound m_DelegateLyricNotFound;
    public DelegateStatusUpdate m_DelegateStatusUpdate;
    public DelegateStringUpdate m_DelegateStringUpdate;

    public DelegateThreadException m_DelegateThreadException;
    public DelegateThreadFinished m_DelegateThreadFinished;

    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

    // worker thread

    private readonly ManualResetEvent m_EventStopThread;
    private readonly bool m_automaticUpdate;

    private readonly string[] m_strippedPrefixStrings = {"the", "les"};

    private readonly string originalArtist;
    private readonly string originalTitle;
    private readonly object parent;
    private int counter;
    private LyricsController lc;
    private Thread m_LyricControllerThread;
    private bool m_automaticFetch = true;

    private List<string> sitesToSearch;

    #endregion

    #region ctor

    public LyricsSearch(object parent, string artist, string title, bool automaticUpdate)
    {
      InitializeComponent();

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.parent = parent;
      m_automaticUpdate = automaticUpdate;

      // initialize delegates
      m_DelegateStringUpdate = new DelegateStringUpdate(updateStringMethod);
      m_DelegateStatusUpdate = new DelegateStatusUpdate(updateStatusMethod);
      m_DelegateLyricFound = new DelegateLyricFound(lyricFoundMethod);
      m_DelegateLyricNotFound = new DelegateLyricNotFound(lyricNotFoundMethod);
      m_DelegateThreadFinished = new DelegateThreadFinished(ThreadFinishedMethod);
      m_DelegateThreadException = new DelegateThreadException(ThreadExceptionMethod);

      // initialize events
      m_EventStopThread = new ManualResetEvent(false);

      tbArtist.Text = artist;
      tbTitle.Text = title;

      if (Options.MainSettings.SwitchArtist)
        btSwitchArtist_Click(btSwitchArtist, new EventArgs());

      originalArtist = tbArtist.Text;
      originalTitle = title;

      LocaliseScreen();

      BeginSearchIfPossible(artist, title);

      this.CenterToScreen();
      ShowDialog();
    }

    #endregion

    #region Methods

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      log.Trace(">>>");
      Text = String.Format(localisation.ToString("LyricsSearch", "Header"), tbArtist.Text, tbTitle.Text);
      chSite.Text = localisation.ToString("LyricsSearch", "Site");
      chResult.Text = localisation.ToString("LyricsSearch", "Result");
      chLyric.Text = localisation.ToString("LyricsSearch", "Lyric");
      log.Trace("<<<");
    }

    #endregion

    internal void BeginSearchIfPossible(string artist, string title)
    {
      log.Trace(">>>");
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
      log.Trace("<<<");
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
      log.Trace(">>>");
      lockGUI();
      tbLyrics.Text = "";
      lvSearchResults.Items.Clear();

      counter = 0;

      sitesToSearch = Options.MainSettings.LyricSites;

 // If automaticUpdate is set then return after the first positive search
      lc = new LyricsController(this, m_EventStopThread, sitesToSearch.ToArray(), true, automaticUpdate, "", "");

      ThreadStart job = delegate { lc.Run(); };

      m_LyricControllerThread = new Thread(job);
      m_LyricControllerThread.Name = "lyricSearch Thread"; // looks nice in Output window
      m_LyricControllerThread.Start();

      lc.AddNewLyricSearch(artist, title, GetStrippedPrefixArtist(artist, m_strippedPrefixStrings));
      log.Trace("<<<");
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
      Close();
    }

    private void lvSearchResults_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lvSearchResults.SelectedItems.Count > 0)
      {
        tbLyrics.Text = LyricUtil.ReturnEnvironmentNewLine(lvSearchResults.SelectedItems[0].SubItems[2].Text);
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
      Close();
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
    private void updateStringMethod(String message, String site) {}

    // Called from worker thread using delegate and Control.Invoke
    private void updateStatusMethod(Int32 noOfLyricsToSearch, Int32 noOfLyricsSearched, Int32 noOfLyricsFound,
                                    Int32 noOfLyricsNotFound) {}

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
        Close();
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
        if (parent.GetType() == typeof (TagEditControl))
        {
          (parent as TagEditControl).LyricsText =
            LyricUtil.ReturnEnvironmentNewLine(lvSearchResults.SelectedItems[0].SubItems[2].Text);
        }
      }
    }


    // Set initial state of controls.
    // Called from worker thread using delegate and Control.Invoke
    private void ThreadFinishedMethod(string artist, string title, string message, string site) {}

    private void ThreadExceptionMethod(Object o) {}

    #endregion

    #region DelegateCalls

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