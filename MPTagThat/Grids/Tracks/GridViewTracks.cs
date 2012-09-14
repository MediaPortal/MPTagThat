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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Elegant.Ui;
using FreeImageAPI;
using Microsoft.VisualBasic.FileIO;
using MPTagThat.Core;
using MPTagThat.Core.Amazon;
using MPTagThat.Core.MusicBrainz;
using MPTagThat.Dialogues;
using MPTagThat.Player;
using MPTagThat.TagEdit;
using TagLib;
using TagLib.Id3v2;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Control = System.Windows.Forms.Control;
using File = TagLib.File;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;
using Picture = MPTagThat.Core.Common.Picture;
using Tag = TagLib.Id3v1.Tag;
using TextBox = System.Windows.Forms.TextBox;

#endregion

namespace MPTagThat.GridView
{
  public partial class GridViewTracks : UserControl
  {
    #region Variables

    private const int RATINGCELLNUMBER = 11;

    private readonly Cursor _numberingCursor = Util.CreateCursorFromResource("CursorNumbering", 0, 0);
    private readonly GridViewColumns gridColumns;

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly IThemeManager theme = ServiceScope.Get<IThemeManager>();
    private bool _actionCopy;

    private Thread _asyncThread;
    private BackgroundWorker _bgWorker;
    private Rectangle _dragBoxFromMouseDown;

    private string[] _filterFileExtensions;
    private string _filterFileMask = "*.*";

    private FindResult _findResult;
    private bool _itemsChanged;
    private Main _main;
    private List<FileInfo> _nonMusicFiles;
    private bool _progressCancelled;
    private Point _screenOffset;
    private bool _waitCursorActive;

    // Get Properties to be able to sort on column heading 
    private readonly PropertyDescriptorCollection _propColl = TypeDescriptor.GetProperties(new TrackData());

    #region Nested type: ThreadSafeAddErrorDelegate

    private delegate void ThreadSafeAddErrorDelegate(string file, string message);

    #endregion

    #region Nested type: ThreadSafeAddTracksDelegate

    private delegate void ThreadSafeAddTracksDelegate(TrackData track);

    #endregion

    #region Nested type: ThreadSafeGridDelegate

    private delegate void ThreadSafeGridDelegate();

    #endregion

    #region Nested type: ThreadSafeGridDelegate1

    private delegate void ThreadSafeGridDelegate1(object sender, DoWorkEventArgs e);

    #endregion

    #region Imports

    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int InitGainAnalysis(long samplefreq);
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ResetSampleFrequency(long samplefreq);
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int AnalyzeSamples(double[] left_samples, double[] right_samples, UIntPtr num_samples, int num_channels);
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern float GetTitleGain();
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern float GetAlbumGain();

    #endregion

    #endregion

    #region Properties

    /// <summary>
    ///   Returns the GridView
    /// </summary>
    public DataGridView View
    {
      get { return tracksGrid; }
    }

    /// <summary>
    ///   Returns the Selected Track
    /// </summary>
    public TrackData SelectedTrack
    {
      get { return Options.Songlist[tracksGrid.CurrentRow.Index]; }
    }

    /// <summary>
    ///   Do we have any changes pending?
    /// </summary>
    public bool Changed
    {
      get { return _itemsChanged; }
      set { _itemsChanged = value; }
    }

    public FindResult ResultFind
    {
      set { _findResult = value; }
    }

    #endregion

    #region Constructor

    public GridViewTracks()
    {
      // Activates double buffering 
      this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();

      InitializeComponent();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      // Load the Settings
      gridColumns = new GridViewColumns();

      // Setup Dataview Grid
      tracksGrid.AutoGenerateColumns = false;
      tracksGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable; // Handle Copy 

      // Setup Event Handler
      tracksGrid.ColumnWidthChanged += tracksGrid_ColumnWidthChanged;
      tracksGrid.CurrentCellDirtyStateChanged += tracksGrid_CurrentCellDirtyStateChanged;
      tracksGrid.DataError += tracksGrid_DataError;
      tracksGrid.CellEndEdit += tracksGrid_CellEndEdit;
      tracksGrid.CellValueChanged += tracksGrid_CellValueChanged;
      tracksGrid.CellPainting += tracksGrid_CellPainting;
      tracksGrid.EditingControlShowing += tracksGrid_EditingControlShowing;
      tracksGrid.ColumnHeaderMouseClick += tracksGrid_ColumnHeaderMouseClick;
      tracksGrid.MouseDown += tracksGrid_MouseDown;
      tracksGrid.MouseUp += tracksGrid_MouseUp;
      tracksGrid.MouseMove += tracksGrid_MouseMove;
      tracksGrid.QueryContinueDrag += tracksGrid_QueryContinueDrag;
      tracksGrid.MouseEnter += tracksGrid_MouseEnter;

      // establish Event Handlers for Virtual Mode hangling of the grid
      tracksGrid.CellValueNeeded += tracksGrid_CellValueNeeded;
      tracksGrid.CellValuePushed += tracksGrid_CellValuePushed;

      // The Color for the Image Cell for the Rating is not handled correctly. so we need to handle it via an event
      tracksGrid.SelectionChanged += tracksGrid_SelectionChanged;

      // Now Setup the columns, we want to display
      CreateColumns();

      LocaliseScreen();
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Set Main Ref to Main
    ///   Can't do it in constructir due to problems with Designer
    /// </summary>
    /// <param name = "main"></param>
    public void SetMainRef(Main main)
    {
      _main = main;

      // Register for ProgressBar Events
      ApplicationCommands.ProgressCancel.Executed += ProgressCancel_Executed;
      _main.ProgressCancelHovering += new Main.ProgressCancelHover(ProgressCancel_Hover);
      _main.ProgressCancelLeaving += new Main.ProgressCancelLeave(ProgressCancel_Leave);
    }

    #region Status Column

    /// <summary>
    /// Clears the Status column
    /// </summary>
    /// <param name="row"></param>
    public void ClearStatusColumn(int rowIndex)
    {
      Options.Songlist[rowIndex].Status = -1;
    }

    #endregion

    #region Save

    /// <summary>
    ///   Save the Selected files only
    /// </summary>
    public void Save()
    {
      log.Trace(">>>");

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        
        ClearStatusColumn(row.Index);

        if (!row.Selected || (string)row.Tag != "Changed")
        {
          log.Trace("Save: Row {0} not selected or changed", row.Index);
          continue;
        }

        count++;
        try
        {
          Application.DoEvents();
          _main.progressBar1.Value += 1;
          if (_progressCancelled)
          {
            ResetProgressBar();
            return;
          }

          TrackData track = Options.Songlist[row.Index];
          SaveTrack(track, row.Index);
        }
        catch (Exception ex)
        {
          Options.Songlist[row.Index].Status = 2;
          AddErrorMessage(row, ex.Message);
        }
      }

      Util.SendProgress("");
      Options.ReadOnlyFileHandling = 2; //No
      ResetProgressBar();

      _itemsChanged = false;
      // check, if we still have changed items in the list
      foreach (TrackData track in Options.Songlist)
      {
        if (track.Changed)
          _itemsChanged = true;
      }

      log.Trace("<<<");
    }

    /// <summary>
    ///   Save All changed files, regardless, if they are selected or not
    /// </summary>
    public void SaveAll()
    {
      SaveAll(true);
    }

    /// <summary>
    ///   Save All changed files, regardless, if they are selected or not
    /// </summary>
    /// <param name = "showProgressDialog">Show / Hide the progress dialogue</param>
    public void SaveAll(bool showProgressDialog)
    {
      log.Trace(">>>");

      bool bErrors = false;

      if (showProgressDialog)
      {
        SetProgressBar(tracksGrid.Rows.Count);
      }

      int trackCount = Options.Songlist.Count;
      for (int i = 0; i < trackCount; i++)
      {
        Application.DoEvents();

        TrackData track = Options.Songlist[i];
        track.Status = -1;

        if (showProgressDialog)
        {
          _main.progressBar1.Value += 1;
          if (_progressCancelled)
          {
            ResetProgressBar();
            return;
          }
        }

        if (!SaveTrack(track, i))
          bErrors = true;
      }

      Util.SendProgress("");
      Options.ReadOnlyFileHandling = 2; //No
      if (showProgressDialog)
      {
        ResetProgressBar();
      }
      _itemsChanged = bErrors;

      log.Trace("<<<");
    }


    /// <summary>
    ///   Does the actual save of the track
    /// </summary>
    /// <param name = "track"></param>
    /// <returns></returns>
    private bool SaveTrack(TrackData track, int rowIndex)
    {
      try
      {
        if (track.Changed)
        {
          Util.SendProgress(string.Format("Saving file {0}", track.FullFileName));
          log.Debug("Save: Saving track: {0}", track.FullFileName);

          // The track to be saved, may be currently playing. If this is the case stop playnack to free the file
          if (track.FullFileName == _main.Player.CurrentSongPlaying)
          {
            log.Debug("Save: Song is played in Player. Stop playback to free the file");
            _main.Player.Stop();
          }

          if (Options.MainSettings.CopyArtist && track.AlbumArtist == "")
          {
            track.AlbumArtist = track.Artist;
          }

          if (Options.MainSettings.UseCaseConversion)
          {
            CaseConversion.CaseConversion convert = new CaseConversion.CaseConversion(_main, true);
            convert.CaseConvert(track, rowIndex);
            convert.Dispose();
          }

          // Save the file 
          string errorMessage = "";
          if (Track.SaveFile(track, ref errorMessage))
          {
            // If we are in Database mode, we should also update the MediaPortal Database
            if (_main.TreeView.DatabaseMode)
            {
              UpdateMusicDatabase(track);
            }

            if (RenameFile(track))
            {
              // rename was ok, so get the new file into the binding list
              string ext = Path.GetExtension(track.FileName);
              string newFileName = Path.Combine(Path.GetDirectoryName(track.FullFileName),
                                                String.Format("{0}{1}", Path.GetFileNameWithoutExtension(track.FileName),
                                                              ext));

              track = Track.Create(newFileName);
              Options.Songlist[rowIndex] = track;
            }

            // Check, if we need to create a folder.jpg
            if (!System.IO.File.Exists(Path.Combine(Path.GetDirectoryName(track.FullFileName), "folder.jpg")) &&
                Options.MainSettings.CreateFolderThumb)
            {
              SavePicture(track);
            }

            track.Status = 0;
            tracksGrid.Rows[rowIndex].Cells[0].ToolTipText = "";
            track.Changed = false;
            tracksGrid.Rows[rowIndex].Tag = "";
            Options.Songlist[rowIndex] = track;
            SetGridRowColors(rowIndex);
          }
          else
          {
            track.Status = 2;
            AddErrorMessage(tracksGrid.Rows[rowIndex], errorMessage);
          }
        }
      }
      catch (Exception ex)
      {
        Options.Songlist[rowIndex].Status = 2;
        AddErrorMessage(tracksGrid.Rows[rowIndex], ex.Message);
        log.Error("Save: Error Saving data for row {0}: {1} {2}", rowIndex, ex.Message, ex.StackTrace);
        return false;
      }
      return true;
    }

    /// <summary>
    ///   Rename the file if necessary
    ///   Called by Save and SaveAll
    /// </summary>
    /// <param name = "track"></param>
    private bool RenameFile(TrackData track)
    {
      string originalFileName = Path.GetFileName(track.FullFileName);
      if (originalFileName != track.FileName)
      {
        string ext = Path.GetExtension(track.FileName);
        string newFileName = Path.Combine(Path.GetDirectoryName(track.FullFileName),
                                          String.Format("{0}{1}", Path.GetFileNameWithoutExtension(track.FileName), ext));
        System.IO.File.Move(track.FullFileName, newFileName);
        log.Debug("Save: Renaming track: {0} Newname: {1}", track.FullFileName, newFileName);
        return true;
      }
      return false;
    }

    #endregion

    #region Identify File

    public void IdentifyFiles()
    {
      if (_bgWorker == null)
      {
        _bgWorker = new BackgroundWorker();
        _bgWorker.DoWork += IdentifyFilesThread;
      }

      if (!_bgWorker.IsBusy)
      {
        _bgWorker.RunWorkerAsync();
      }
    }

    /// <summary>
    ///   Tag the the Selected files from Internet
    /// </summary>
    private void IdentifyFilesThread(object sender, DoWorkEventArgs e)
    {
      log.Trace(">>>");
      //Make calls to Tracksgrid Threadsafe
      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeGridDelegate1 d = IdentifyFilesThread;
        tracksGrid.Invoke(d, new[] { sender, e });
        return;
      }

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      MusicBrainzAlbum musicBrainzAlbum = new MusicBrainzAlbum();

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        ClearStatusColumn(row.Index);

        if (!row.Selected)
        {
          continue;
        }

        count++;
        try
        {
          Application.DoEvents();
          _main.progressBar1.Value += 1;
          if (_progressCancelled)
          {
            ResetProgressBar();
            return;
          }
          TrackData track = Options.Songlist[row.Index];

          using (MusicBrainzTrackInfo trackinfo = new MusicBrainzTrackInfo())
          {
            Util.SendProgress(string.Format("Identifying file {0}", track.FileName));
            log.Debug("Identify: Processing file: {0}", track.FullFileName);
            List<MusicBrainzTrack> musicBrainzTracks = trackinfo.GetMusicBrainzTrack(track.FullFileName);

            if (musicBrainzTracks == null)
            {
              log.Debug("Identify: Couldn't identify file");
              continue;
            }

            if (musicBrainzTracks.Count > 0)
            {
              MusicBrainzTrack musicBrainzTrack = null;
              if (musicBrainzTracks.Count == 1)
                musicBrainzTrack = musicBrainzTracks[0];
              else
              {
                // Skip the Album selection, if the album been selected already for a previous track
                bool albumFound = false;
                foreach (MusicBrainzTrack mbtrack in musicBrainzTracks)
                {
                  if (mbtrack.AlbumID == musicBrainzAlbum.Id)
                  {
                    albumFound = true;
                    musicBrainzTrack = mbtrack;
                    break;
                  }
                }

                if (!albumFound)
                {
                  MusicBrainzAlbumResults dlgAlbumResults = new MusicBrainzAlbumResults(musicBrainzTracks);
                  dlgAlbumResults.Owner = _main;
                  if (_main.ShowModalDialog(dlgAlbumResults) == DialogResult.OK)
                  {
                    if (dlgAlbumResults.SelectedListItem > -1)
                      musicBrainzTrack = musicBrainzTracks[dlgAlbumResults.SelectedListItem];
                    else
                      musicBrainzTrack = musicBrainzTracks[0];
                  }
                  dlgAlbumResults.Dispose();
                }
              }

              // We didn't get a track
              if (musicBrainzTrack == null)
              {
                log.Debug("Identify: No information returned from Musicbrainz");
                continue;
              }

              // Are we still at the same album?
              // if not, get the album, so that we have the release date
              if (musicBrainzAlbum.Id != musicBrainzTrack.AlbumID)
              {
                using (MusicBrainzAlbumInfo albumInfo = new MusicBrainzAlbumInfo())
                {
                  Application.DoEvents();
                  if (_progressCancelled)
                  {
                    ResetProgressBar();
                    return;
                  }
                  musicBrainzAlbum = albumInfo.GetMusicBrainzAlbumById(musicBrainzTrack.AlbumID.ToString());
                }
              }

              track.Title = musicBrainzTrack.Title;
              track.Artist = musicBrainzTrack.Artist;
              track.Album = musicBrainzTrack.Album;
              track.Track = musicBrainzTrack.Number.ToString();

              if (musicBrainzAlbum.Year != null && musicBrainzAlbum.Year.Length >= 4)
                track.Year = Convert.ToInt32(musicBrainzAlbum.Year.Substring(0, 4));

              // Do we have a valid Amazon Album?
              if (musicBrainzAlbum.Amazon != null)
              {
                // Only write a picture if we don't have a picture OR Overwrite Pictures is set
                if (track.Pictures.Count == 0 || Options.MainSettings.OverwriteExistingCovers)
                {
                  ByteVector vector = musicBrainzAlbum.Amazon.AlbumImage;
                  if (vector != null)
                  {
                    MPTagThat.Core.Common.Picture pic = new MPTagThat.Core.Common.Picture();
                    pic.MimeType = "image/jpg";
                    pic.Description = "";
                    pic.Type = PictureType.FrontCover;
                    pic.Data = vector.Data;
                    track.Pictures.Add(pic);
                  }
                }
              }

              SetBackgroundColorChanged(row.Index);
              track.Changed = true;
              Options.Songlist[row.Index] = track;
              _itemsChanged = true;
            }
          }
        }
        catch (Exception ex)
        {
          Options.Songlist[row.Index].Status = 2;
          AddErrorMessage(row, ex.Message);
        }
      }

      Util.SendProgress("");
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      ResetProgressBar();

      log.Trace("<<<");
    }

    #endregion

    #region Cover Art

    public void GetCoverArt()
    {
      if (_asyncThread == null)
      {
        _asyncThread = new Thread(GetCoverArtThread);
        _asyncThread.Name = "GetCoverArt";
      }

      if (_asyncThread.ThreadState != ThreadState.Running)
      {
        _asyncThread = new Thread(GetCoverArtThread);
        _asyncThread.Start();
      }
    }

    /// <summary>
    ///   Get Cover Art via Amazon Webservice
    /// </summary>
    private void GetCoverArtThread()
    {
      log.Trace(">>>");
      //Make calls to Tracksgrid Threadsafe
      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeGridDelegate d = GetCoverArtThread;
        tracksGrid.Invoke(d, new object[] { });
        return;
      }

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      AmazonAlbum amazonAlbum = null;

      bool isMultipleArtistAlbum = false;
      string savedArtist = "";
      string savedAlbum = "";
      string savedFolder = "";

      Core.Common.Picture folderThumb = null;

      // Find out, if we deal with a multiple artist album and submit only the album name
      // If we have different artists, then it is a multiple artist album.
      // BUT: if the album is different, we don't have a multiple artist album and should submit the artist as well
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        ClearStatusColumn(row.Index);

        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];
        if (savedArtist == "")
        {
          savedArtist = track.Artist;
          savedAlbum = track.Album;
        }
        if (savedArtist != track.Artist)
        {
          isMultipleArtistAlbum = true;
        }
        if (savedAlbum != track.Album)
        {
          isMultipleArtistAlbum = false;
          break;
        }
      }

      if (isMultipleArtistAlbum)
      {
        log.Debug("CoverArt: Album contains Multiple Artists, just search for the album");
      }

      Dictionary<string, AmazonAlbum> savedCoverCash = new Dictionary<string, AmazonAlbum>();

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        ClearStatusColumn(row.Index);

        if (!row.Selected)
        {
          continue;
        }

        count++;
        try
        {
          Application.DoEvents();
          _main.progressBar1.Value += 1;
          if (_progressCancelled)
          {
            ResetProgressBar();
            return;
          }
          TrackData track = Options.Songlist[row.Index];

          Util.SendProgress(string.Format("Search coverart for {0}", track.FileName));
          log.Debug("CoverArt: Retrieving coverart for: {0} - {1}", track.Artist, track.Album);
          // Should we take an existing folder.jpg instead of searching the web
          if (Options.MainSettings.EmbedFolderThumb && !Options.MainSettings.OnlySaveFolderThumb)
          {
            if (folderThumb == null || Path.GetDirectoryName(track.FullFileName) != savedFolder)
            {
              savedFolder = Path.GetDirectoryName(track.FullFileName);
              folderThumb = GetFolderThumb(savedFolder);
            }

            if (folderThumb != null)
            {
              // Only write a picture if we don't have a picture OR Overwrite Pictures is set
              if (track.Pictures.Count == 0 || Options.MainSettings.OverwriteExistingCovers)
              {
                if (Options.MainSettings.ChangeCoverSize && Picture.ImageFromData(folderThumb.Data).Width > Options.MainSettings.MaxCoverWidth)
                {
                  folderThumb.Resize(Options.MainSettings.MaxCoverWidth);
                }

                log.Debug("CoverArt: Using existing folder.jpg");
                // First Clear all the existingPictures
                track.Pictures.Clear();
                track.Pictures.Add(folderThumb);
                SetBackgroundColorChanged(row.Index);
                track.Changed = true;
                Options.Songlist[row.Index] = track;
                _itemsChanged = true;
                _main.SetGalleryItem();
              }
              continue;
            }
          }

          // If we don't have an Album don't do any query
          if (track.Album == "")
            continue;

          string coverSearchString = track.Artist + track.Album;
          if (isMultipleArtistAlbum)
          {
            coverSearchString = track.Album;
          }

          bool foundInCash = savedCoverCash.ContainsKey(coverSearchString);
          if (foundInCash)
          {
            amazonAlbum = savedCoverCash[coverSearchString];
          }
          else
          {
            amazonAlbum = null;
          }

          // Only retrieve the Cover Art, if we don't have it yet)
          if (!foundInCash || amazonAlbum == null)
          {
            CoverSearch dlgAlbumResults = new CoverSearch();
            dlgAlbumResults.Artist = isMultipleArtistAlbum ? "" : track.Artist;
            dlgAlbumResults.Album = track.Album;
            dlgAlbumResults.FileDetails = track.FullFileName;
            dlgAlbumResults.Owner = _main;

            amazonAlbum = null;
            DialogResult dlgResult = _main.ShowModalDialog(dlgAlbumResults);
            if (dlgResult == DialogResult.OK)
            {
              if (dlgAlbumResults.SelectedAlbum != null)
              {
                amazonAlbum = dlgAlbumResults.SelectedAlbum;
              }
            }
            else if (dlgResult == DialogResult.Abort)
            {
              log.Debug("CoverArt: Search for all albums cancelled");
              break;
            }
            else
            {
              log.Debug("CoverArt: Album Selection cancelled");
              continue;
            }
            dlgAlbumResults.Dispose();
          }


          // Now update the Cover Art
          if (amazonAlbum != null)
          {
            if (!savedCoverCash.ContainsKey(coverSearchString))
            {
              savedCoverCash.Add(coverSearchString, amazonAlbum);
            }

            // Only write a picture if we don't have a picture OR Overwrite Pictures is set);
            if ((track.Pictures.Count == 0 || Options.MainSettings.OverwriteExistingCovers) && !Options.MainSettings.OnlySaveFolderThumb)
            {
              track.Pictures.Clear();

              ByteVector vector = amazonAlbum.AlbumImage;
              if (vector != null)
              {
                MPTagThat.Core.Common.Picture pic = new MPTagThat.Core.Common.Picture();
                pic.MimeType = "image/jpg";
                pic.Description = "Front Cover";
                pic.Type = PictureType.FrontCover;
                pic.Data = vector.Data;

                if (Options.MainSettings.ChangeCoverSize && Picture.ImageFromData(pic.Data).Width > Options.MainSettings.MaxCoverWidth)
                {
                  pic.Resize(Options.MainSettings.MaxCoverWidth);
                }

                track.Pictures.Add(pic);
              }

              // And also set the Year from the Release Date delivered by Amazon
              // only if not present in Track
              if (amazonAlbum.Year != null)
              {
                string strYear = amazonAlbum.Year;
                if (strYear.Length > 4)
                  strYear = strYear.Substring(0, 4);

                int year = 0;
                try
                {
                  year = Convert.ToInt32(strYear);
                }
                catch (Exception) { }
                if (year > 0 && track.Year == 0)
                  track.Year = year;
              }

              SetBackgroundColorChanged(row.Index);
              track.Changed = true;
              Options.Songlist[row.Index] = track;
              _itemsChanged = true;
              _main.SetGalleryItem();
            }
          }

          // If the user has selected to store only the folder thumb, without touching the file 
          if (amazonAlbum != null && Options.MainSettings.OnlySaveFolderThumb)
          {
            ByteVector vector = amazonAlbum.AlbumImage;
            if (vector != null)
            {
              string fileName = Path.Combine(Path.GetDirectoryName(track.FullFileName), "folder.jpg");
              try
              {
                MPTagThat.Core.Common.Picture pic = new MPTagThat.Core.Common.Picture();
                if (Options.MainSettings.ChangeCoverSize && Picture.ImageFromData(pic.Data).Width > Options.MainSettings.MaxCoverWidth)
                {
                  pic.Resize(Options.MainSettings.MaxCoverWidth);
                }

                Image img = Picture.ImageFromData(vector.Data);

                // Need to make a copy, otherwise we have a GDI+ Error
                Bitmap bmp = new Bitmap(img);
                bmp.Save(fileName, ImageFormat.Jpeg);

                FileInfo fi = new FileInfo(fileName);
                _nonMusicFiles.RemoveAll(f => f.Name == fi.Name);
                _nonMusicFiles.Add(fi);
                _main.MiscInfoPanel.AddNonMusicFiles(_nonMusicFiles);
              }
              catch (Exception ex)
              {
                log.Error("Exception Saving picture: {0} {1}", fileName, ex.Message);
              }
              break;
            }
          }
        }
        catch (Exception ex)
        {
          Options.Songlist[row.Index].Status = 2;
          AddErrorMessage(row, ex.Message);
        }
      }

      Util.SendProgress("");
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      ResetProgressBar();

      log.Trace("<<<");
    }

    /// <summary>
    ///   Return the folder.jpg as a Taglib.Picture
    /// </summary>
    /// <param name = "folder"></param>
    /// <returns></returns>
    public Core.Common.Picture GetFolderThumb(string folder)
    {
      string thumb = Path.Combine(folder, "folder.jpg");
      if (!System.IO.File.Exists(thumb))
      {
        return null;
      }

      try
      {
        Core.Common.Picture pic = new Core.Common.Picture(thumb);
        pic.Description = "Front Cover";
        pic.Type = PictureType.FrontCover;
        return pic;
      }
      catch (Exception ex)
      {
        log.Error("Exception loading thumb file: {0} {1}", thumb, ex.Message);
        return null;
      }
    }

    /// <summary>
    ///   Save the Picture of the track as folder.jpg
    /// </summary>
    /// <param name = "track"></param>
    public void SavePicture(TrackData track)
    {
      if (track.NumPics > 0)
      {
        string fileName = Path.Combine(Path.GetDirectoryName(track.FullFileName), "folder.jpg");
        try
        {
          Image img = Picture.ImageFromData(track.Pictures[0].Data);
          // Need to make a copy, otherwise we have a GDI+ Error
          Bitmap bCopy = new Bitmap(img);
          bCopy.Save(fileName, ImageFormat.Jpeg);
        }
        catch (Exception ex)
        {
          log.Error("Exception Saving picture: {0} {1}", fileName, ex.Message);
        }
      }
    }

    #endregion

    #region Cover Art Drop

    /// <summary>
    /// A Picture file has been dropped on the Ribbon Gallery. 
    /// Set the picture for all selected rows
    /// </summary>
    /// <param name="fileName"></param>
    public void CoverArtDrop(string fileName)
    {
      SetWaitCursor();
      Util.SendProgress(string.Format("Downloading picture from {0}", fileName));
      Picture pic = null;
      if (fileName.ToLower().StartsWith("http"))
      {
        pic = GetCoverArtFromUrl(fileName);
      }
      else
      {
        pic = new Picture(fileName);
      }
      if (pic == null || pic.Data == null)
      {
        ResetWaitCursor();
        return;
      }

      if (Options.MainSettings.ChangeCoverSize && Picture.ImageFromData(pic.Data).Width > Options.MainSettings.MaxCoverWidth)
      {
        pic.Resize(Options.MainSettings.MaxCoverWidth);
      }

      pic.MimeType = "image/jpg";
      pic.Description = "Front Cover";
      pic.Type = PictureType.FrontCover;


      foreach (DataGridViewRow row in tracksGrid.SelectedRows)
      {
        ClearStatusColumn(row.Index);

        TrackData track = Options.Songlist[row.Index];
        track.Pictures.Clear();
        track.Pictures.Add(pic);
        SetBackgroundColorChanged(row.Index);
        track.Changed = true;
        Options.Songlist[row.Index] = track;
        _itemsChanged = true;
      }

      Util.SendProgress("");
      _main.SetGalleryItem();
      ResetWaitCursor();
    }

    /// <summary>
    /// Get Cover Art from a given Url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private Picture GetCoverArtFromUrl(string url)
    {
      Picture pic = null;
      try
      {
        // When dragging from Google images, we have a imgurl. extract the right url.
        int imgurlIndex = url.IndexOf("imgurl=");
        if (imgurlIndex > -1)
        {
          url = url.Substring(imgurlIndex + 7);
          url = url.Substring(0, url.IndexOf("&"));
        }

        log.Info("Retrieving Coverart from: {0}", url);
        WebRequest req = WebRequest.Create(url);
        req.Proxy = new WebProxy();
        req.Proxy.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = req.GetResponse();
        if (response == null)
        {
          return null;
        }
        Stream stream = response.GetResponseStream();
        if (stream == null)
        {
          return null;
        }
        Image img = Image.FromStream(stream);
        stream.Close();

        pic = new Picture { Data = Picture.ImageToByte((Image)img.Clone()) };

        if (Options.MainSettings.ChangeCoverSize && img.Width > Options.MainSettings.MaxCoverWidth)
        {
          pic.Resize(Options.MainSettings.MaxCoverWidth);
        }

        img.Dispose();
        return pic;
      }
      catch (Exception ex)
      {
        log.Error("Error retrieving Image from Url: {0} Error: {1}", url, ex.Message);
      }
      return null;
    }

    #endregion

    #region Lyrics

    public void GetLyrics()
    {
      if (_asyncThread == null)
      {
        _asyncThread = new Thread(GetLyricsThread);
        _asyncThread.Name = "GetLyrics";
      }

      if (_asyncThread.ThreadState != ThreadState.Running)
      {
        _asyncThread = new Thread(GetLyricsThread);
        _asyncThread.Start();
      }
    }

    /// <summary>
    ///   Get Lyrics for selected Rows
    /// </summary>
    private void GetLyricsThread()
    {
      log.Trace(">>>");
      //Make calls to Tracksgrid Threadsafe
      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeGridDelegate d = GetLyricsThread;
        tracksGrid.Invoke(d, new object[] { });
        return;
      }

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      List<TrackData> tracks = new List<TrackData>();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        count++;
        Application.DoEvents();
        _main.progressBar1.Value += 1;
        if (_progressCancelled)
        {
          ResetProgressBar();
          return;
        }
        TrackData track = Options.Songlist[row.Index];
        if (track.Lyrics == null || Options.MainSettings.OverwriteExistingLyrics)
        {
          tracks.Add(track);
        }
      }

      ResetProgressBar();

      if (tracks.Count > 0)
      {
        try
        {
          LyricsSearch lyricssearch = new LyricsSearch(tracks);
          lyricssearch.Owner = _main;
          if (_main.ShowModalDialog(lyricssearch) == DialogResult.OK)
          {
            DataGridView lyricsResult = lyricssearch.GridView;
            foreach (DataGridViewRow lyricsRow in lyricsResult.Rows)
            {
              if (lyricsRow.Cells[0].Value == DBNull.Value || lyricsRow.Cells[0].Value == null)
                continue;

              if ((bool)lyricsRow.Cells[0].Value != true)
                continue;

              foreach (DataGridViewRow row in tracksGrid.Rows)
              {
                TrackData lyricsTrack = tracks[lyricsRow.Index];
                TrackData track = Options.Songlist[row.Index];
                if (lyricsTrack.FullFileName == track.FullFileName)
                {
                  track.Lyrics = (string)lyricsRow.Cells[5].Value;
                  SetBackgroundColorChanged(row.Index);
                  track.Changed = true;
                  Options.Songlist[row.Index] = track;
                  _itemsChanged = true;
                  break;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          log.Error("Error in Lyricssearch: {0}", ex.Message);
        }
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      log.Trace("<<<");
    }

    #endregion

    #region Numbering

    public void AutoNumber()
    {
      log.Trace(">>>");
      int numberValue = _main.AutoNumber;
      if (numberValue == -1)
        return;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];

        // Get the Number of tracks, so that we don't loose it
        string[] tracks = track.Track.Split('/');
        string numTracks = "0";
        if (tracks.Length > 1)
        {
          numTracks = tracks[1];
        }

        track.Track = string.Format("{0}/{1}", numberValue, numTracks);
        SetBackgroundColorChanged(row.Index);
        track.Changed = true;
        Options.Songlist[row.Index] = track;
        _itemsChanged = true;
        numberValue++;
      }
      _main.AutoNumber = numberValue;
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      log.Trace("<<<");
    }

    #endregion

    #region Delete Tags / Delete Files

    /// <summary>
    ///   Remove the Tags
    /// </summary>
    /// <param name = "type"></param>
    public void DeleteTags(TagTypes type)
    {
      log.Trace(">>>");
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];
        track.TagsRemoved.Add(type);
        track = Track.ClearTag(track);

        SetBackgroundColorChanged(row.Index);
        track.Changed = true;
        Options.Songlist[row.Index] = track;
        _itemsChanged = true;
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      log.Trace("<<<");
    }

    /// <summary>
    ///   The Del key has been pressed. Send the selected files to the recycle bin
    /// </summary>
    public void DeleteTracks()
    {
      log.Trace(">>>");

      if (tracksGrid.SelectedRows.Count > 0)
      {
        DialogResult result = MessageBox.Show(localisation.ToString("message", "DeleteConfirm"),
                                              localisation.ToString("message", "DeleteConfirmHeader"),
                                              MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        if (result == DialogResult.Cancel)
        {
          log.Trace("<<<");
          return;
        }
      }

      for (int i = tracksGrid.Rows.Count - 1; i > 0; i--)
      {
        DataGridViewRow row = tracksGrid.Rows[i];
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];
        try
        {
          FileSystem.DeleteFile(track.FullFileName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin,
                                UICancelOption.ThrowException);

          // Remove the file from the binding list
          Options.Songlist.RemoveAt(row.Index);
        }
        catch (OperationCanceledException) // User pressed No on delete. Do nothing
        { }
        catch (Exception ex)
        {
          log.Error("Error deleting file: {0} Exception: {1}", track.FullFileName, ex.Message);
          Options.Songlist[row.Index].Status = 2;
          AddErrorMessage(row, ex.Message);
        }
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      log.Trace("<<<");
    }

    #endregion

    #region Remove Comments / Remove Pictures

    /// <summary>
    ///   Remove all Comments in te selected tracks
    /// </summary>
    public void RemoveComments()
    {
      log.Trace(">>>");
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];

        if (track.Comment != "")
        {
          track.ID3Comments.Clear();
          SetBackgroundColorChanged(row.Index);
          track.Changed = true;
          Options.Songlist[row.Index] = track;
          _itemsChanged = true;
        }
      }
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();
      log.Trace("<<<");
    }

    /// <summary>
    ///   Remove all Picture data for the selected tracks
    /// </summary>
    public void RemovePictures()
    {
      log.Trace(">>>");
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];

        if (track.NumPics > 0)
        {
          track.Pictures.Clear();
          SetBackgroundColorChanged(row.Index);
          track.Changed = true;
          Options.Songlist[row.Index] = track;
          _itemsChanged = true;
        }
      }
      _main.SetGalleryItem();
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();
      log.Trace("<<<");
    }

    #endregion

    #region Validate / Fix MP3 Files

    /// <summary>
    ///   Validates an MP3 File using mp3val
    /// </summary>
    public void ValidateMP3File()
    {
      log.Trace(">>>");

      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        ClearStatusColumn(row.Index);

        if (!row.Selected)
        {
          continue;
        }

        Application.DoEvents();
        _main.progressBar1.Value += 1;
        if (_progressCancelled)
        {
          ResetProgressBar();
          return;
        }

        TrackData track = Options.Songlist[row.Index];

        if (track.IsMp3)
        {
          Util.SendProgress(string.Format("Validating file {0}", track.FileName));
          string strError = "";
          track.MP3ValidationError = MP3Val.ValidateMp3File(track.FullFileName, out strError);
          if (track.MP3ValidationError != Util.MP3Error.NoError)
          {
            SetColorMP3Errors(row.Index, track.MP3ValidationError);
            track.Status = 3;
            tracksGrid.Rows[row.Index].Cells[0].ToolTipText = strError;
          }
          else
          {
            tracksGrid.Rows[row.Index].Cells[0].ToolTipText = "";
          }
        }
      }
      Util.SendProgress("");
      ResetProgressBar();
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();
      log.Trace("<<<");
    }

    /// <summary>
    ///   Fixes errors in an MP3 file using mp3val
    /// </summary>
    public void FixMP3File()
    {
      log.Trace(">>>");

      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        Application.DoEvents();
        _main.progressBar1.Value += 1;
        if (_progressCancelled)
        {
          ResetProgressBar();
          return;
        }

        TrackData track = Options.Songlist[row.Index];
        if (track.IsMp3)
        {
          Util.SendProgress(string.Format("Fixing file {0}", track.FileName));
          string strError = "";
          track.MP3ValidationError = MP3Val.FixMp3File(track.FullFileName, out strError);
          if (track.MP3ValidationError == Util.MP3Error.Fixed)
          {
            SetGridRowColors(row.Index);
            track.Status = 4;
            tracksGrid.Rows[row.Index].Cells[0].ToolTipText = "";
          }
          else
          {
            SetColorMP3Errors(row.Index, track.MP3ValidationError);
            track.Status = 3;
            tracksGrid.Rows[row.Index].Cells[0].ToolTipText = strError;
          }
        }
      }
      Util.SendProgress("");
      ResetProgressBar();
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();
      log.Trace("<<<");
    }

    #endregion

    #region Replay Gain

    /// <summary>
    ///   Analyses selected files and calculates the ReplayGain
    /// </summary>
    public void ReplayGain()
    {
      log.Trace(">>>");

      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      bool albumGain = false;
      bool gainInitialised = false;
      int usedFrequency = -1;

      // Check, if all rows have been selected and provide the option to invoke Album Gain analysis
      if (tracksGrid.Rows.Count == tracksGrid.SelectedRows.Count)
      {
        if (MessageBox.Show(localisation.ToString("albumgain", "Explanation"),
                 localisation.ToString("albumgain", "Header"), MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          albumGain = true;
        }
      }

      float maxPeak = 0.0f;
      
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        ClearStatusColumn(row.Index);

        if (!row.Selected)
        {
          continue;
        }

        Application.DoEvents();
        _main.progressBar1.Value += 1;
        if (_progressCancelled)
        {
          ResetProgressBar();
          return;
        }

        TrackData track = Options.Songlist[row.Index];

        int stream = Bass.BASS_StreamCreateFile(track.FullFileName, 0, 0, BASSFlag.BASS_STREAM_DECODE);
        if (stream == 0)
        {
          log.Error("ReplayGain: Could not create stream for {0}. {1}", track.FullFileName, Bass.BASS_ErrorGetCode());
          continue;
        }

        BASS_CHANNELINFO chInfo = Bass.BASS_ChannelGetInfo(stream);
        if (chInfo == null)
        {
          log.Error("ReplayGain: Could not get channel info for {0}. {1}", track.FullFileName, Bass.BASS_ErrorGetCode());
          continue;
        }
        
        Util.SendProgress(string.Format("Analysing gain for {0}", track.FileName));
        log.Info("ReplayGain: Start gain analysis for: {0}", track.FullFileName);

        if (!albumGain || !gainInitialised)
        {
          InitGainAnalysis(chInfo.freq);
          gainInitialised = true;
          usedFrequency = chInfo.freq;
        }
        else
        {
          if (usedFrequency != chInfo.freq)
          {
            ResetSampleFrequency(chInfo.freq);
            usedFrequency = chInfo.freq;
          }
        }
        
        ReplayAnalyze(stream);
        float titleGain = GetTitleGain();
        
        // Calculating the peak level
        float peak = 0;
        Bass.BASS_ChannelSetPosition(stream, 0);

        float[] level = new float[chInfo.chans];  // allocate for all Channels of the stream
        while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
        { // not reached the end yet
          if(Bass.BASS_ChannelGetLevel(stream, level))
          {
            for (int i = 0; i < chInfo.chans; i++ )
            {
              if (peak < level[i]) peak = level[i];
            }
          }
        }

        if (albumGain && maxPeak < peak)
        {
          maxPeak = peak;
        }

        Bass.BASS_StreamFree(stream);

        log.Info("ReplayGain: Finished analysis. Gain: {0} Peak level: {1}", titleGain.ToString(CultureInfo.InvariantCulture), peak.ToString(CultureInfo.InvariantCulture));
        track.ReplayGainTrack = titleGain.ToString(CultureInfo.InvariantCulture);
        track.ReplayGainTrackPeak = peak.ToString(CultureInfo.InvariantCulture);

        SetBackgroundColorChanged(row.Index);
        track.Changed = true;
        Options.Songlist[row.Index] = track;
        _itemsChanged = true;
      }

      // Should we also get Album Gain
      if (albumGain)
      {
        float albumGainValue = GetAlbumGain();
        string albumGainValueStr = albumGainValue.ToString(CultureInfo.InvariantCulture);
        string albumPeakValueStr = maxPeak.ToString(CultureInfo.InvariantCulture);

        foreach (DataGridViewRow row in tracksGrid.Rows)
        {
          if (!row.Selected)
          {
            continue;
          }

          TrackData track = Options.Songlist[row.Index];
          track.ReplayGainAlbum = albumGainValueStr;
          track.ReplayGainAlbumPeak = albumPeakValueStr;
        }
      }

      Util.SendProgress("");
      ResetProgressBar();
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      log.Trace("<<<");
    }

    /// <summary>
    /// Do a Replaygain and Peak analyses
    /// </summary>
    /// <param name="channel"></param>
    void ReplayAnalyze(int channel)
    {
      int result;
      int peak = 0;

      int[] chanmap = new int[2];
      chanmap[0] = 0; // left channel
      chanmap[1] = -1;
      int stream1 = BassMix.BASS_Split_StreamCreate(channel, BASSFlag.BASS_STREAM_DECODE, chanmap);
      chanmap[0] = 1; // right channel
      int stream2 = BassMix.BASS_Split_StreamCreate(channel, BASSFlag.BASS_STREAM_DECODE, chanmap);

      int length = (int)Bass.BASS_ChannelSeconds2Bytes(channel, 0.02); // 20ms window

      Int16[] buffer = new Int16[length / 2];
      double[] leftSamples = new double[length / 2];
      double[] rightSamples = new double[length / 2];

      Bass.BASS_ChannelSetPosition(stream1, 0); // make sure to start from the beginning
      Bass.BASS_ChannelSetPosition(stream2, 0); // make sure to start from the beginning
      while (Bass.BASS_ChannelIsActive(stream1) == BASSActive.BASS_ACTIVE_PLAYING
        || Bass.BASS_ChannelIsActive(stream2) == BASSActive.BASS_ACTIVE_PLAYING)
      {
        result = Bass.BASS_ChannelGetData(stream1, buffer, length);

        int l4 = result / 2;
        for (int a = 0; a < l4; a++)
        {
          leftSamples[a] = Convert.ToDouble(buffer[a]);
        }

        result = Bass.BASS_ChannelGetData(stream2, buffer, length);
        l4 = result / 2;
        for (int a = 0; a < l4; a++)
        {
          rightSamples[a] = Convert.ToDouble(buffer[a]);
        }

        AnalyzeSamples(leftSamples, rightSamples, new UIntPtr((uint)l4 / 2), 2);
        leftSamples = new double[length / 2];
        rightSamples = new double[length / 2];
      }
    }

    #endregion

    #region Misc Methods

    /// <summary>
    ///   Discards any changes
    /// </summary>
    public void DiscardChanges()
    {
      _itemsChanged = false;
    }

    /// <summary>
    ///   Checks for Pending Changes
    /// </summary>
    public void CheckForChanges()
    {
      if (Changed)
      {
        DialogResult result = MessageBox.Show(localisation.ToString("message", "Save_Changes"),
                                              localisation.ToString("message", "Save_Changes_Title"),
                                              MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
          SaveAll();
        else
          DiscardChanges();
      }
    }

    /// <summary>
    ///   Checks, if we have something selected
    /// </summary>
    /// <returns></returns>
    public bool CheckSelections(bool selectAll)
    {
      if (tracksGrid.SelectedRows.Count == 0)
      {
        // If no Rows are selected, select ALL of them and do the necessary action
        if (selectAll)
          tracksGrid.SelectAll();
        else
        {
          MessageBox.Show(localisation.ToString("message", "NoSelection"),
                          localisation.ToString("message", "NoSelectionHeader"), MessageBoxButtons.OK,
                          MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    /// <summary>
    ///   Sets the Color for changed Items
    /// </summary>
    /// <param name = "index"></param>
    public void SetBackgroundColorChanged(int index)
    {
      tracksGrid.Rows[index].Tag = "Changed";

      tracksGrid.Rows[index].DefaultCellStyle.BackColor =
        ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedBackColor;
      tracksGrid.Rows[index].DefaultCellStyle.ForeColor =
        ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedForeColor;

      // Rating cell needs special processing
      if (tracksGrid.Rows[index].Selected)
      {
        tracksGrid.Rows[index].Cells[RATINGCELLNUMBER].Style.BackColor =
             ServiceScope.Get<IThemeManager>().CurrentTheme.SelectionBackColor;
      }
      else
      {
        tracksGrid.Rows[index].Cells[RATINGCELLNUMBER].Style.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedBackColor;
      }
    }

    /// <summary>
    /// Sets the Grid Color based on the Line
    /// </summary>
    /// <param name="index"></param>
    public void SetGridRowColors(int index)
    {
      if (index % 2 == 0)
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
        // Rating cell needs special processing
        if (tracksGrid.Rows[index].Selected)
        {
          tracksGrid.Rows[index].Cells[RATINGCELLNUMBER].Style.BackColor =
               ServiceScope.Get<IThemeManager>().CurrentTheme.SelectionBackColor;
        }
        else
        {
          tracksGrid.Rows[index].Cells[RATINGCELLNUMBER].Style.BackColor =
            ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
        }
      }
      else
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.AlternatingRowBackColor;
        // Rating cell needs special processing
        if (tracksGrid.Rows[index].Selected)
        {
          tracksGrid.Rows[index].Cells[RATINGCELLNUMBER].Style.BackColor =
               ServiceScope.Get<IThemeManager>().CurrentTheme.SelectionBackColor;
        }
        else
        {
          tracksGrid.Rows[index].Cells[RATINGCELLNUMBER].Style.BackColor =
            ServiceScope.Get<IThemeManager>().CurrentTheme.AlternatingRowBackColor;
        }
      }
    }

    /// <summary>
    ///   Sets the Color for Tracks, that contain errors found by mp3val
    /// </summary>
    /// <param name = "index"></param>
    public void SetColorMP3Errors(int index, Util.MP3Error error)
    {
      if (error == Util.MP3Error.Fixable || error == Util.MP3Error.Fixed)
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.FixableErrorBackColor;
        tracksGrid.Rows[index].DefaultCellStyle.ForeColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.FixableErrorForeColor;
      }
      else if (error == Util.MP3Error.NonFixable)
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.NonFixableErrorBackColor;
        tracksGrid.Rows[index].DefaultCellStyle.ForeColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.NonFixableErrorForeColor;
      }

      if (error == Util.MP3Error.Fixed)
      {
        Options.Songlist[index].Status = 4;
      }
    }

    /// <summary>
    ///   Adds an Error Message to the Status Column
    /// </summary>
    /// <param name = "row"></param>
    /// <param name = "message"></param>
    public void AddErrorMessage(DataGridViewRow row, string message)
    {
      row.Cells[0].ToolTipText = message;
    }

    #endregion

    #region Script Handling

    /// <summary>
    ///   Executes a script on all selected rows
    /// </summary>
    /// <param name = "scriptFile"></param>
    public void ExecuteScript(string scriptFile)
    {
      log.Trace(">>>");
      Assembly assembly = ServiceScope.Get<IScriptManager>().Load(scriptFile);

      try
      {
        if (assembly != null)
        {
          
          int trackCount = tracksGrid.SelectedRows.Count;
          SetProgressBar(trackCount);

          IScript script = (IScript)assembly.CreateInstance("Script");

          foreach (DataGridViewRow row in tracksGrid.Rows)
          {
            if (!row.Selected)
            {
              continue;
            }

            Application.DoEvents();
            _main.progressBar1.Value += 1;
            if (_progressCancelled)
            {
              ResetProgressBar();
              Util.SendProgress("");
              return;
            }

            TrackData track = Options.Songlist[row.Index];
            Util.SendProgress(string.Format("Running script on {0}", track.FullFileName));
            script.Invoke(track);

            if (track.Changed)
            {
              Options.Songlist[row.Index] = track;
              SetBackgroundColorChanged(row.Index);
              _itemsChanged = true;
            }
          }
          ResetProgressBar();
          Util.SendProgress("");
        }
      }
      catch (Exception ex)
      {
        log.Error("Script Execution failed: {0}", ex.Message);
        MessageBox.Show(localisation.ToString("message", "Script_Compile_Failed"),
                        localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      }
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      log.Trace("<<<");
    }

    #endregion

    #region Folder Scanning

    public void FolderScan()
    {
      log.Trace(">>>");
      tracksGrid.Rows.Clear();
      Options.Songlist.Clear();
      _nonMusicFiles = new List<FileInfo>();
      _main.MiscInfoPanel.ClearNonMusicFiles();
      _main.MiscInfoPanel.ActivateNonMusicTab();
      GC.Collect();

      string selectedFolder = _main.CurrentDirectory;
      if (!Directory.Exists(selectedFolder))
        return;

      _main.FolderScanning = true;

      // Get File Filter Settings
      _filterFileExtensions = _main.TreeView.ActiveFilter.FileFilter.Split('|');
      _filterFileMask = _main.TreeView.ActiveFilter.FileMask.Trim() == ""
                          ? "*"
                          : _main.TreeView.ActiveFilter.FileMask.Trim();

      SetProgressBar(1);

      // Change the style to Marquee, since we really don't know how much files we will get
      _main.progressBar1.Style = ProgressBarStyle.Marquee;
      _main.progressBar1.MarqueeAnimationSpeed = 10;

      int count = 1;
      int nonMusicCount = 0;

      tracksGrid.SuspendLayout();
      tracksGrid.Hide();
      try
      {
        foreach (FileInfo fi in GetFiles(new DirectoryInfo(selectedFolder), _main.TreeView.ScanFolderRecursive))
        {
          Application.DoEvents();

          if (_progressCancelled)
          {
            break;
          }
          try
          {
            if (Util.IsAudio(fi.FullName))
            {
              Util.SendProgress(string.Format("Reading file {0}", fi.FullName));
              log.Trace("Retrieving file: {0}", fi.FullName);
              // Read the Tag
              TrackData track = Track.Create(fi.FullName);
              if (track != null)
              {
                if (ApplyTagFilter(track))
                {
                  AddTrack(track);
                  tracksGrid.Rows.Add(); // Add a row to the grid. Virtualmode will handle the filling of cells
                  count++;
                }
              }
            }
            else
            {
              _nonMusicFiles.Add(fi);
              nonMusicCount++;
            }
          }
          catch (PathTooLongException)
          {
            log.Warn("FolderScan: Ignoring track {0} - path too long!", fi.FullName);
            continue;
          }
          catch (System.UnauthorizedAccessException exUna)
          {
            log.Warn("Could not access file or folder: {0}. {1}", exUna.Message, fi.FullName);
          }
          catch (Exception ex)
          {
            log.Error("Caugth error processing files: {0} {1}", ex.Message, fi.FullName);
          }

          if (count > 20)
          {
            tracksGrid.Show();
            tracksGrid.ResumeLayout();
          }

          _main.ToolStripStatusScan.Text = string.Format(localisation.ToString("main", "toolStripLabelScan"), count);
        }
      }
      catch (OutOfMemoryException)
      {
        GC.Collect();
        MessageBox.Show(localisation.ToString("message", "OutOfMemory"), localisation.ToString("message", "Error_Title"),
                        MessageBoxButtons.OK);
        log.Error("Folderscan: Running out of memory. Scanning aborted.");
      }
      finally
      {
        tracksGrid.Show();
        tracksGrid.ResumeLayout();
      }

      Util.SendProgress("");
      log.Info("FolderScan: Scanned {0} files. Found {1} audio files", nonMusicCount + count, count);

      _main.MiscInfoPanel.AddNonMusicFiles(_nonMusicFiles);

      _main.ToolStripStatusScan.Text = "";
      

      ResetProgressBar();
      _main.progressBar1.Style = ProgressBarStyle.Continuous;

      // Display Status Information
      try
      {
        _main.ToolStripStatusFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"), count, 0);
      }
      catch (InvalidOperationException) { }

      // unselect the first row, which would be selected automatically by the grid
      // And set the background color of the rating cell, as it isn't reset by the grid
      try
      {
        if (tracksGrid.Rows.Count > 0)
        {
          _main.TagEditForm.ClearForm();
          tracksGrid.Rows[0].Selected = false;
          tracksGrid.Rows[0].Cells[RATINGCELLNUMBER].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
        }
      }
      catch (ArgumentOutOfRangeException) { }

      // If MP3 Validation is turned on, set the color
      if (Options.MainSettings.MP3Validate)
      {
        ChangeErrorRowColor();
      }

      _main.FolderScanning = false;

      log.Trace("<<<");
    }

    /// <summary>
    ///   Read a Folder and return the files
    /// </summary>
    /// <param name = "folder"></param>
    /// <param name = "foundFiles"></param>
    private IEnumerable<FileInfo> GetFiles(DirectoryInfo dirInfo, bool recursive)
    {
      Queue<DirectoryInfo> directories = new Queue<DirectoryInfo>();
      directories.Enqueue(dirInfo);
      Queue<FileInfo> files = new Queue<FileInfo>();
      while (files.Count > 0 || directories.Count > 0)
      {
        if (files.Count > 0)
        {
          yield return files.Dequeue();
        }
        try
        {
          if (directories.Count > 0)
          {
            DirectoryInfo dir = directories.Dequeue();

            if (recursive)
            {
              DirectoryInfo[] newDirectories = dir.GetDirectories();
              foreach (DirectoryInfo di in newDirectories)
              {
                directories.Enqueue(di);
              }
            }

            foreach (string extension in _filterFileExtensions)
            {
              string searchFilter = string.Format("{0}.{1}", _filterFileMask, extension);
              FileInfo[] newFiles = dir.GetFiles(searchFilter);
              foreach (FileInfo file in newFiles)
              {
                files.Enqueue(file);
              }
            }
          }
        }
        catch (UnauthorizedAccessException ex)
        {
          log.ErrorException(ex.Message, ex);
        }
      }
    }

    #endregion

    #region Database Scanning / Update

    public void DatabaseScan()
    {
      if (_main.CurrentDirectory == null)
      {
        return;
      }

      string[] searchString = _main.CurrentDirectory.Split('\\');

      // Get out, if we are on the Top Level only
      if (searchString.GetLength(0) == 1)
      {
        return;
      }

      List<string> songs = new List<string>();
      string sql = FormatSQL(searchString);

      string connection = string.Format(@"Data Source={0}", Options.MainSettings.MediaPortalDatabase);
      try
      {
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = CommandType.Text;
          cmd.CommandText = sql;
          log.Debug("Database Scan: Executing sql: {0}", sql);
          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              songs.Add(reader.GetString(0));
            }
          }
        }
        conn.Close();
      }
      catch (Exception ex)
      {
        log.Error("Database Scan: Error executing sql: {0}", ex.Message);
      }

      log.Debug("Database Scan: Query returned {0} songs", songs.Count);
      AddDatabaseSongsToGrid(songs);
    }

    public void AddDatabaseSongsToGrid(List<string> songs)
    {
      // Clear the list and free up resources
      tracksGrid.Rows.Clear();
      Options.Songlist.Clear();
      GC.Collect();

      SetProgressBar(songs.Count);

      // Get File Filter Settings
      _filterFileExtensions = _main.TreeView.ActiveFilter.FileFilter.Split('|');
      _filterFileMask = _main.TreeView.ActiveFilter.FileMask.Trim() == ""
                          ? "*"
                          : _main.TreeView.ActiveFilter.FileMask.Trim();

      int count = 1;
      foreach (string song in songs)
      {
        Application.DoEvents();
        _main.progressBar1.Value += 1;
        if (_progressCancelled)
        {
          break;
        }

        if (ApplyFileFilter(song))
        {
          TrackData track = Track.Create(song);
          if (ApplyTagFilter(track))
          {
            AddTrack(track);
            tracksGrid.Rows.Add(); // Add a row to the grid. Virtualmode will handle the filling of cells
          }
        }
        count++;
      }

      _main.FolderScanning = false;
      ResetProgressBar();

      // Display Status Information
      try
      {
        _main.ToolStripStatusFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"), count, 0);
      }
      catch (InvalidOperationException) { }

      // unselect the first row, which would be selected automatically by the grid
      // And set the background color of the rating cell, as it isn't reset by the grid
      try
      {
        if (tracksGrid.Rows.Count > 0)
        {
          _main.TagEditForm.ClearForm();
          tracksGrid.Rows[0].Selected = false;
          tracksGrid.Rows[0].Cells[RATINGCELLNUMBER].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
        }
      }
      catch (ArgumentOutOfRangeException) { }


      // If MP3 Validation is turned on, set the color
      if (Options.MainSettings.MP3Validate)
      {
        ChangeErrorRowColor();
      }
    }

    private string FormatSQL(string[] searchString)
    {
      string sql = "select strPath from tracks where {0} order by {1}";

      string whereClause = "";
      string orderByClause = "";
      switch (searchString[0])
      {
        case "artist":
          whereClause = string.Format("strArtist like '%| {0}%'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "strAlbum, iTrack";
          if (searchString.GetLength(0) > 2)
          {
            whereClause += string.Format(" AND strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[2]));
            orderByClause = "iTrack";
          }
          break;

        case "albumartist":
          whereClause = string.Format("strAlbumArtist like '%| {0}%'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "strAlbum, iTrack";
          if (searchString.GetLength(0) > 2)
          {
            whereClause += string.Format(" AND strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[2]));
            orderByClause = "iTrack";
          }
          break;

        case "album":
          whereClause = string.Format("strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "iTrack";
          break;

        case "genre":
          whereClause = string.Format("strGenre like '%| {0}%'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "strArtist, strAlbum, iTrack";
          if (searchString.GetLength(0) > 2)
          {
            whereClause += string.Format(" AND strArtist like '%{0}%'", Util.RemoveInvalidChars(searchString[2]));
            orderByClause = "strAlbum, iTrack";
          }
          if (searchString.GetLength(0) > 3)
          {
            whereClause += string.Format(" AND strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[3]));
            orderByClause = "iTrack";
          }
          break;
      }

      sql = string.Format(sql, whereClause, orderByClause);

      return sql;
    }

    private void UpdateMusicDatabase(TrackData track)
    {
      string db = Options.MainSettings.MediaPortalDatabase;

      if (!System.IO.File.Exists(db))
      {
        return;
      }

      string[] tracks = track.Track.Split('/');
      int trackNumber = 0;
      if (tracks[0] != "")
      {
        trackNumber = Convert.ToInt32(tracks[0]);
      }
      int trackTotal = 0;
      if (tracks.Length > 1)
      {
        trackTotal = Convert.ToInt32(tracks[1]);
      }

      string[] discs = track.Disc.Split('/');
      int discNumber = 0;
      if (discs[0] != "")
      {
        discNumber = Convert.ToInt32(discs[0]);
      }
      int discTotal = 0;
      if (discs.Length > 1)
      {
        discTotal = Convert.ToInt32(discs[1]);
      }

      string originalFileName = Path.GetFileName(track.FullFileName);
      string newFileName = "";
      newFileName = track.FullFileName;

      /*
      if (originalFileName != track.FileName)
      {
        string ext = Path.GetExtension(track.File.Name);
        newFileName = Path.Combine(Path.GetDirectoryName(track.File.Name),
                                   String.Format("{0}{1}", Path.GetFileNameWithoutExtension(track.FileName), ext));
      }
      else
      {
        newFileName = track.FullFileName;
      }
       */


      string sql = String.Format(
        @"update tracks 
              set strArtist = '{0}', strAlbumArtist = '{1}', strAlbum = '{2}', 
              strGenre = '{3}', strTitle = '{4}', iTrack = {5}, iNumTracks = {6}, 
              iYear = {7}, iRating = {8}, iDisc = {9}, iNumDisc = {10}, strLyrics = '{11}', strPath = '{12}'
            where strPath = '{13}'",
        Util.RemoveInvalidChars(FormatMultipleEntry(track.Artist)),
        Util.RemoveInvalidChars(FormatMultipleEntry(track.AlbumArtist)), Util.RemoveInvalidChars(track.Album),
        Util.RemoveInvalidChars(FormatMultipleEntry(track.Genre)), Util.RemoveInvalidChars(track.Title), trackNumber,
        trackTotal,
        track.Year, track.Rating, discNumber, discTotal,
        Util.RemoveInvalidChars(track.Lyrics), Util.RemoveInvalidChars(newFileName),
        Util.RemoveInvalidChars(track.FullFileName)
        );


      string connection = string.Format(@"Data Source={0}", db);
      try
      {
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = CommandType.Text;
          cmd.CommandText = sql;
          int result = cmd.ExecuteNonQuery();
        }
        conn.Close();
      }
      catch (Exception ex)
      {
        log.Error("Database Update: Error executing sql: {0}", ex.Message);
      }
    }

    /// <summary>
    ///   Multiple Entry fields need to be formatted to contain a | at the end to be able to search correct
    /// </summary>
    /// <param name = "str"></param>
    /// <returns>Formatted string</returns>
    private string FormatMultipleEntry(string str)
    {
      string[] strSplit = str.Split(new[] { ';', '|' });
      // Can't use a simple String.Join as i need to trim all the elements 
      string strJoin = "| ";
      foreach (string strTmp in strSplit)
      {
        string s = strTmp.Trim();
        strJoin += String.Format("{0} | ", s.Trim());
      }
      return strJoin;
    }

    #endregion

    #endregion

    #region Private Methods

    #region Localisation

    /// <summary>
    ///   Language Change event has been fired. Apply the new language
    /// </summary>
    private void LanguageChanged()
    {
      LocaliseScreen();
    }

    private void LocaliseScreen()
    {
      // Update the column Headings
      foreach (DataGridViewColumn col in tracksGrid.Columns)
      {
        col.HeaderText = localisation.ToString("column_header", col.Name);
      }
      contextMenu.Items[0].Text = localisation.ToString("contextmenu", "Copy");
      contextMenu.Items[1].Text = localisation.ToString("contextmenu", "Cut");
      contextMenu.Items[2].Text = localisation.ToString("contextmenu", "Paste");
      contextMenu.Items[3].Text = localisation.ToString("contextmenu", "Delete");
      contextMenu.Items[5].Text = localisation.ToString("contextmenu", "SelectAll");
      contextMenu.Items[7].Text = localisation.ToString("contextmenu", "CoverLookup");
      contextMenu.Items[8].Text = localisation.ToString("contextmenu", "AddBurner");
      contextMenu.Items[9].Text = localisation.ToString("contextmenu", "AddConverter");
      contextMenu.Items[10].Text = localisation.ToString("contextmenu", "AddPlaylist");
      contextMenu.Items[12].Text = localisation.ToString("contextmenu", "SavePlaylist");
      contextMenu.Items[13].Text = localisation.ToString("contextmenu", "CreateFolderThumb");
    }

    #endregion

    #region Miscellaneous Methods

    /// <summary>
    ///   Create the Columns of the Grid based on the users setting
    /// </summary>
    private void CreateColumns()
    {
      log.Trace(">>>");

      // Now create the columns 
      foreach (GridViewColumn column in gridColumns.Settings.Columns)
      {
        tracksGrid.Columns.Add(Util.FormatGridColumn(column));
      }

      // Add a dummy column and set the property of the last column to fill
      DataGridViewColumn col = new DataGridViewTextBoxColumn();
      col.Name = "dummy";
      col.HeaderText = "";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 5;
      tracksGrid.Columns.Add(col);
      tracksGrid.Columns[tracksGrid.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

      log.Trace("<<<");
    }

    /// <summary>
    ///   Save the settings
    /// </summary>
    private void SaveSettings()
    {
      // Save the Width of the Columns
      int i = 0;
      foreach (DataGridViewColumn column in tracksGrid.Columns)
      {
        // Don't save the dummy column
        if (i == tracksGrid.Columns.Count - 1)
          break;

        gridColumns.SaveColumnSettings(column, i);
        i++;
      }
      gridColumns.SaveSettings();
    }

    /// <summary>
    ///   Adds a Track to the data grid
    /// </summary>
    /// <param name = "track"></param>
    private void AddTrack(TrackData track)
    {
      if (track == null)
        return;


      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeAddTracksDelegate d = AddTrack;
        tracksGrid.Invoke(d, new object[] { track });
        return;
      }

      // Validate the MP3 File
      if (Options.MainSettings.MP3Validate && track.IsMp3)
      {
        string strError = "";
        track.MP3ValidationError = MP3Val.ValidateMp3File(track.FullFileName, out strError);
        track.MP3ValidationErrorText = strError;
      }

      Options.Songlist.Add(track);
    }

    private void ShowForm(Form f)
    {
      int x = _main.ClientSize.Width / 2 - f.Width / 2;
      int y = _main.ClientSize.Height / 2 - f.Height / 2;
      Point clientLocation = _main.Location;
      x += clientLocation.X;
      y += clientLocation.Y;
      f.Location = new Point(x, y);
      f.Show();
    }

    private void ChangeErrorRowColor()
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        TrackData track = Options.Songlist[row.Index];

        if (track.MP3ValidationError == Util.MP3Error.NoError)
        {
          continue;
        }

        SetColorMP3Errors(row.Index, track.MP3ValidationError);
        tracksGrid.Rows[row.Index].Cells[0].ToolTipText = track.MP3ValidationErrorText;
        track.Status = 3;
      }
    }

    /// <summary>
    ///   Sets the maximum value of Progressbar
    /// </summary>
    /// <param name = "maxCount"></param>
    private void SetProgressBar(int maxCount)
    {
      _main.progressBar1.Maximum = maxCount == 0 ? 100 : maxCount;
      _main.progressBar1.Value = 0;
      _progressCancelled = false;
      SetWaitCursor();
    }

    /// <summary>
    ///   Reset the Progressbar to Initiaövfalue
    /// </summary>
    private void ResetProgressBar()
    {
      _main.progressBar1.Value = 0;
      _progressCancelled = false;
      ResetWaitCursor();
    }

    /// <summary>
    ///   Sets the WaitCursor during various operations
    /// </summary>
    private void SetWaitCursor()
    {
      _main.Cursor = Cursors.WaitCursor;
      tracksGrid.Cursor = Cursors.WaitCursor;
      _waitCursorActive = true;
    }

    /// <summary>
    ///   Resets the WaitCursor to the default
    /// </summary>
    private void ResetWaitCursor()
    {
      _main.Cursor = Cursors.Default;
      tracksGrid.Cursor = Cursors.Default;
      _waitCursorActive = false;
    }

    #endregion

    #region Filter

    private bool ApplyFileFilter(string fileName)
    {
      foreach (string extension in _filterFileExtensions)
      {
        if (extension != "*.*" && Path.GetExtension(fileName) != extension)
          continue;

        if (Regex.IsMatch(Path.GetFileNameWithoutExtension(fileName), MakeRegexp(_filterFileMask)))
        {
          return true;
        }
      }
      return false;
    }

    private bool ApplyTagFilter(TrackData track)
    {
      if (!_main.TreeView.ActiveFilter.UseTagFilter)
      {
        return true;
      }

      List<TreeViewTagFilter> filter = _main.TreeView.ActiveFilter.TagFilter;
      List<bool> results = new List<bool>(filter.Count);


      bool numericCompare = false; // For year, bpm, etc.
      int index = 0;
      int searchNumber = 0;
      string searchstring = "";
      foreach (TreeViewTagFilter tagfilter in filter)
      {
        switch (tagfilter.Field)
        {
          case "artist":
            searchstring = track.Artist;
            break;

          case "albumartist":
            searchstring = track.AlbumArtist;
            break;

          case "album":
            searchstring = track.Album;
            break;

          case "title":
            searchstring = track.Title;
            break;

          case "genre":
            searchstring = track.Genre;
            break;

          case "comment":
            searchstring = track.Comment;
            break;

          case "composer":
            searchstring = track.Composer;
            break;

          case "conductor":
            searchstring = track.Conductor;
            break;

          // Tags with are checked for existence or non-existence
          case "picture":
            searchstring = track.Pictures.Count > 0 ? "True" : "False";
            break;

          case "lyrics":
            searchstring = track.Lyrics != null ? "True" : "False";
            break;

          // Numeric Tags
          case "track":
            numericCompare = true;
            searchNumber = (int)track.TrackNumber;
            break;

          case "numtracks":
            numericCompare = true;
            searchNumber = (int)track.TrackCount;
            break;

          case "disc":
            numericCompare = true;
            searchNumber = (int)track.DiscNumber;
            break;

          case "numdiscs":
            numericCompare = true;
            searchNumber = (int)track.DiscCount;
            break;

          case "year":
            numericCompare = true;
            searchNumber = track.Year;
            break;

          case "rating":
            numericCompare = true;
            searchNumber = track.Rating;
            break;

          case "bpm":
            numericCompare = true;
            searchNumber = track.BPM;
            break;

          case "bitrate":
            numericCompare = true;
            searchNumber = Convert.ToInt16(track.BitRate);
            break;

          case "samplerate":
            numericCompare = true;
            searchNumber = Convert.ToInt16(track.SampleRate);
            break;

          case "channels":
            numericCompare = true;
            searchNumber = Convert.ToInt16(track.Channels);
            break;
        }

        // Now check the filter value, if we have a negation
        string searchValue = tagfilter.FilterValue;
        bool negation = false;
        if (searchValue.Length > 0 && searchValue[0] == '!')
        {
          searchValue = searchValue.Substring(1);
          negation = true;
        }

        bool match = false;
        if (numericCompare)
        {
          try
          {
            searchValue = searchValue.Trim();
            if (searchValue == "")
              searchValue = "0";

            if (searchValue.StartsWith("<="))
            {
              searchValue = searchValue.Substring(2);
              match = searchNumber <= Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith(">="))
            {
              searchValue = searchValue.Substring(2);
              match = searchNumber >= Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith("<"))
            {
              searchValue = searchValue.Substring(1);
              match = searchNumber < Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith(">"))
            {
              searchValue = searchValue.Substring(1);
              match = searchNumber > Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith("="))
            {
              searchValue = searchValue.Substring(1);
              match = searchNumber == Int32.Parse(searchValue);
            }
            else
            {
              match = searchNumber == Int32.Parse(searchValue);
            }
          }
          catch (FormatException)
          {
            match = false;
          }
        }
        else
        {
          match = Regex.IsMatch(searchstring, MakeRegexp(searchValue));
        }

        if (negation)
        {
          match = !match;
        }

        results.Insert(index, match);
        index++;
      }


      index = 0;
      bool ok = false;
      foreach (bool result in results)
      {
        if (index + 1 == results.Count)
        {
          if (index == 0)
          {
            ok = result;
          }
          break;
        }

        bool nextresult = results[index + 1];

        if (filter[index].FilterOperator == "and")
        {
          ok = result && nextresult;
        }
        else
        {
          ok = result || nextresult;
        }

        if (!ok)
        {
          break;
        }
      }

      return ok;
    }

    private string MakeRegexp(string searchValue)
    {
      if (searchValue == "")
        return "^$";

      // Now replace furter meta chars with their backslashed version
      searchValue = searchValue.Replace(@"\", @"\\").Replace("|", @"\|").Replace("(", @"\(").Replace(")", @"\)");
      searchValue = searchValue.Replace(@"{", @"\{").Replace("[", @"\[").Replace("^", @"\^").Replace("$", @"\$");
      searchValue = searchValue.Replace(@"+", @"\+").Replace("*", @"\*").Replace(".", @"\.");
      searchValue = searchValue.Replace(@"<", @"\<").Replace(">", @"\<");

      if (searchValue.Substring(0, 2) == @"\*")
      {
        searchValue = ".*" + searchValue.Substring(2);
      }
      else
      {
        searchValue = "^" + searchValue;
      }

      if (searchValue.Substring(searchValue.Length - 2) == @"\*")
      {
        searchValue = searchValue.Substring(0, searchValue.Length - 2) + ".*";
      }

      // Replace any wildcards that the user inserted by the correct reg exp syntax
      searchValue = searchValue.Replace("?", ".");

      return searchValue;
    }

    #endregion

    #endregion

    #region EventHandler

    /// <summary>
    ///   Handle Messages
    /// </summary>
    /// <param name = "message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "languagechanged":
          LanguageChanged();
          Refresh();
          break;

        case "themechanged":
          {
            tracksGrid.BackgroundColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            break;
          }
      }
    }

    /// <summary>
    ///   Handles changes in the column width
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      // On startup we get sometimes an exception
      try
      {
        if (tracksGrid.Rows.Count > 0)
          tracksGrid.InvalidateRow(e.Column.Index);
      }
      catch (Exception) { }
    }

    /// <summary>
    ///   Handles editing of data columns
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      // For combo box and check box cells, commit any value change as soon
      // as it is made rather than waiting for the focus to leave the cell.
      if (!tracksGrid.CurrentCell.OwningColumn.GetType().Equals(typeof(DataGridViewTextBoxColumn)))
      {
        tracksGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
      }
    }

    /// <summary>
    ///   Only allow valid values to be entered.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      if (e.Exception == null) return;

      // If the user-specified value is invalid, cancel the change 
      // and display the error icon in the row header.
      if ((e.Context & DataGridViewDataErrorContexts.Commit) != 0 &&
          (typeof(FormatException).IsAssignableFrom(e.Exception.GetType()) ||
           typeof(ArgumentException).IsAssignableFrom(e.Exception.GetType())))
      {
        tracksGrid.Rows[e.RowIndex].ErrorText = localisation.ToString("message", "DataEntryError");
        e.Cancel = true;
      }
      else
      {
        // Rethrow any exceptions that aren't related to the user input.
        e.ThrowException = true;
      }
    }

    /// <summary>
    ///   We're leaving a Cell after edit
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      // Ensure that the error icon in the row header is hidden.
      tracksGrid.Rows[e.RowIndex].ErrorText = "";
    }

    /// <summary>
    ///   Value of Cell has changed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      // When changing the Status or the Header Text, ignore the Cell Changed event
      if (e.ColumnIndex == 0 || e.RowIndex == -1)
        return;

      _itemsChanged = true;
      SetBackgroundColorChanged(e.RowIndex);
      TrackData track = Options.Songlist[e.RowIndex];
      _main.TagEditForm.FillForm();
      Options.Songlist[e.RowIndex] = track;
      track.Changed = true;
    }

    /// <summary>
    ///   Handle Right Mouse Click to show Column Config Dialogue
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      // On right click on Header show config dialogue
      if (e.Button == MouseButtons.Right)
      {
        ColumnSelect dialog = new ColumnSelect(this);
        dialog.ShowDialog();
        return;
      }

      if (e.ColumnIndex == 0) // Don't Sort the Status Column
      {
        return;
      }

      // We clicked on the column, so let's sort it
      DataGridViewColumn newColumn = tracksGrid.Columns[e.ColumnIndex];
      ListSortDirection direction;

      // We store the Sortoder in the Columns Tag
      object currentSortOrder = newColumn.Tag;
      if (currentSortOrder != null)
      {
        // Sort the same column again, reversing the SortOrder.
        if ((int)currentSortOrder == 0)
        {
          direction = ListSortDirection.Descending;
        }
        else
        {
          direction = ListSortDirection.Ascending;
        }
      }
      else
      {
        direction = ListSortDirection.Ascending;
        // Clear the Glyph for all 
        foreach (DataGridViewColumn col in tracksGrid.Columns)
        {
          col.Tag = null;
          col.HeaderCell.SortGlyphDirection = SortOrder.None;
        }
      }

      // Sort the selected column.

      PropertyDescriptor prop = _propColl.Find(newColumn.Name, false);
      Options.Songlist.Sort(prop, direction);

      newColumn.HeaderCell.SortGlyphDirection =
          direction == ListSortDirection.Ascending ?
          SortOrder.Ascending : SortOrder.Descending;
      newColumn.Tag = (int)direction;

      tracksGrid_Sorted();
      tracksGrid.Update();
      tracksGrid.Refresh();
    }

    /// <summary>
    ///   THe Grid has been sorted. We need to change the Background Color of the changed Rows
    /// </summary>
    private void tracksGrid_Sorted()
    {
      int i = 0;
      // Set the Color for changed rows again
      foreach (TrackData track in Options.Songlist)
      {
        if (track.Changed)
        {
          SetBackgroundColorChanged(i);
        }
        else
        {
          SetGridRowColors(i);
        }
        i++;
      }
    }

    /// <summary>
    ///   We want to get Control, when editing a Cell, so that we can control the input
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      DataGridView view = sender as DataGridView;
      string colName = view.CurrentCell.OwningColumn.Name;
      if (colName == "Track" || colName == "Disc")
      {
        TextBox txtbox = e.Control as TextBox;
        if (txtbox != null)
        {
          txtbox.KeyPress += txtbox_KeyPress;
        }
      }
    }

    /// <summary>
    ///   Allow only Digits and the slash when enetering data for Track or Disc
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void txtbox_KeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;

      if (!Char.IsDigit(keyChar) // 0 - 9
          &&
          keyChar != 8 // backspace
          &&
          keyChar != 13 // enter
          &&
          keyChar != '/'
        )
      {
        //  Do not display the keystroke
        e.Handled = true;
      }
    }

    /// <summary>
    ///   Handle Left Mouse Click for Numbering on Click
    ///   Handle Right Mouse Click to open the context Menu in the Grid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        Point mouse = tracksGrid.PointToClient(Cursor.Position);
        DataGridView.HitTestInfo selectedRow = tracksGrid.HitTest(mouse.X, mouse.Y);

        if (selectedRow.Type != DataGridViewHitTestType.ColumnHeader)
          contextMenu.Show(tracksGrid, new Point(e.X, e.Y));
      }
      else
      {
        // Handle Numbering on Click
        if (_main.NumberingOnClick)
        {
          Point mouse = tracksGrid.PointToClient(Cursor.Position);
          DataGridView.HitTestInfo selectedRow = tracksGrid.HitTest(mouse.X, mouse.Y);

          if (selectedRow.Type != DataGridViewHitTestType.ColumnHeader)
          {
            int numberValue = _main.AutoNumber;
            if (numberValue == -1)
              return;

            if (selectedRow.RowIndex == -1)
              return;

            TrackData track = Options.Songlist[selectedRow.RowIndex];

            // Get the Number of tracks, so that we don't loose it
            string[] tracks = track.Track.Split('/');
            string numTracks = "0";
            if (tracks.Length > 1)
            {
              numTracks = tracks[1];
            }

            track.Track = string.Format("{0}/{1}", numberValue, numTracks);
            SetBackgroundColorChanged(selectedRow.RowIndex);
            track.Changed = true;
            Options.Songlist[selectedRow.RowIndex] = track;
            _itemsChanged = true;
            _main.AutoNumber = numberValue + 1;
            tracksGrid.Refresh();
            tracksGrid.Parent.Refresh();
          }
        }
      }
    }

    /// <summary>
    ///   Mouse is over Trackgrid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseEnter(object sender, EventArgs e)
    {
      // Numbering on Click enabled
      if (_main.NumberingOnClick)
        tracksGrid.Cursor = _numberingCursor;
      else
      {
        if (_waitCursorActive)
        {
          tracksGrid.Cursor = Cursors.WaitCursor;
        }
        else
        {
          tracksGrid.Cursor = Cursors.Default;
        }
      }
    }

    /// <summary>
    ///   Handle the Background Color for the Rating Image Cell
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_SelectionChanged(object sender, EventArgs e)
    {
      if (_main.FolderScanning)
      {
        return;
      }
      
      for (int i = 0; i < tracksGrid.Rows.Count; i++)
      {
        if (tracksGrid.Rows[i].Selected)
        {
          tracksGrid.Rows[i].Cells[RATINGCELLNUMBER].Style.BackColor =
            ServiceScope.Get<IThemeManager>().CurrentTheme.SelectionBackColor;

          tracksGrid.Rows[i].Cells[RATINGCELLNUMBER].Style.BackColor =
              ServiceScope.Get<IThemeManager>().CurrentTheme.SelectionBackColor;
        }
        else
        {
          if ((string)tracksGrid.Rows[i].Tag == "Changed")
          {
            tracksGrid.Rows[i].Cells[RATINGCELLNUMBER].Style.BackColor =
              ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedBackColor;
          }
          else
          {
            if (i % 2 == 0)
              tracksGrid.Rows[i].Cells[RATINGCELLNUMBER].Style.BackColor =
                ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
            else
              tracksGrid.Rows[i].Cells[RATINGCELLNUMBER].Style.BackColor =
                ServiceScope.Get<IThemeManager>().CurrentTheme.AlternatingRowBackColor;
          }
        }
      }
    }

    private void tracksGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (_findResult == null)
      {
        e.Handled = false;
        return;
      }

      if ((e.ColumnIndex == _findResult.Column) && (e.RowIndex == _findResult.Row))
      {
        // Draw Merged Cell
        Graphics g = e.Graphics;
        bool selected = ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);
        Color fcolor = (selected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor);
        Color bcolor = (selected ? e.CellStyle.SelectionBackColor : e.CellStyle.BackColor);
        Font font = e.CellStyle.Font;

        // Split up the cell content into 3 pieces
        string cellContent = e.Value == null ? "" : e.Value.ToString();
        ;
        string[] results = new string[3];
        results[0] = cellContent.Substring(0, _findResult.StartPos);
        results[1] = cellContent.Substring(_findResult.StartPos, _findResult.Length);
        results[2] = cellContent.Substring(_findResult.StartPos + _findResult.Length);

        int x = e.CellBounds.Left + e.CellStyle.Padding.Left;
        int y = e.CellBounds.Top + e.CellStyle.Padding.Top + 4;

        for (int i = 0; i < 3; i++)
        {
          Size proposedSize = new Size(int.MaxValue, int.MaxValue);

          int width;
          Size size;
          Color color;
          Color backColor;
          TextFormatFlags textFlags;
          if (i == 0)
          {
            size = TextRenderer.MeasureText(e.Graphics, results[i], font, proposedSize);
            width = size.Width - e.CellStyle.Padding.Left;
            color = fcolor;
            backColor = bcolor;
            textFlags = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.EndEllipsis;
          }
          else if (i == 1)
          {
            size = TextRenderer.MeasureText(e.Graphics, results[i], font, proposedSize, TextFormatFlags.NoPadding);
            width = size.Width;
            color = theme.CurrentTheme.FindReplaceForeColor;
            backColor = theme.CurrentTheme.FindReplaceBackColor;
            textFlags = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding;
          }
          else
          {
            size = TextRenderer.MeasureText(e.Graphics, results[i], font, proposedSize);
            width = size.Width - e.CellStyle.Padding.Right;
            color = fcolor;
            backColor = bcolor;
            textFlags = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.EndEllipsis;
          }

          int height = size.Height + (e.CellStyle.Padding.Top + e.CellStyle.Padding.Bottom);
          Rectangle rect = new Rectangle(x, y - 4, width, e.CellBounds.Height - 2);
          g.FillRectangle(new SolidBrush(backColor), rect);
          TextRenderer.DrawText(e.Graphics, results[i], font, new Rectangle(x, y, width, height), color,
                                textFlags);
          x += width;
        }

        e.Handled = true;
        return;
      }
      e.Handled = false;
    }

    /// <summary>
    ///   The Progress Cancel has been fired from the Statusbar Button
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void ProgressCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      _progressCancelled = true;
      ResetWaitCursor();
    }

    /// <summary>
    ///   We're hovering over the Progress Cancel button.
    ///   If the Wait Cursor is active, change it to Default
    /// </summary>
    private void ProgressCancel_Hover(object sender, EventArgs e)
    {
      if (_waitCursorActive)
      {
        _main.Cursor = Cursors.Default;
      }
    }

    /// <summary>
    ///   We are leaving the Button again. If WaitCursor is active, we should set it back again
    /// </summary>
    private void ProgressCancel_Leave(object sender, EventArgs e)
    {
      if (_waitCursorActive)
      {
        _main.Cursor = Cursors.WaitCursor;
      }
    }

    #region Virtual Mode Handling

    /// <summary>
    /// This method is invoked, whenever the Grid decides that it needs a cell value.
    /// We get the required row from from the Songlist and retrieve the cell value using reflection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      // Don't handle empty folders
      if (Options.Songlist.Count == 0)
      {
        return;
      }

      if (e.RowIndex == Options.Songlist.Count)
      {
        return;
      }

      TrackData track = Options.Songlist[e.RowIndex];

      // Handle the status column
      if (e.ColumnIndex == 0)
      {
        if (track.Changed)
        {
          e.Value = Properties.Resources.Warning;
          return;
        }

        switch (track.Status)
        {
          case -1:
            return;

          case 0:
            e.Value = Properties.Resources.Complete_OK;
            break;

          case 1:
            e.Value = Properties.Resources.Warning;
            break;

          case 2:
            e.Value = Properties.Resources.CriticalError;
            break;

          case 3:
            e.Value = Properties.Resources.ribbon_BrokenSong_16x;
            break;

          case 4:
            e.Value = Properties.Resources.ribbon_FixedSong_16x;
            break;
        }

        return;
      }

      string colName = tracksGrid.Columns[e.ColumnIndex].Name;
      if (colName == "dummy")
      {
        return;
      }

      // Get the row from the bindinglist and set the required cell
      e.Value = track.GetType().InvokeMember(colName, BindingFlags.GetField | BindingFlags.GetProperty, null, track,
                                                 new object[] { });
    }
    
    /// <summary>
    /// A cell value has changed. Push it back into the Songlist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      // Don't handle the Status column
      if (e.ColumnIndex == 0)
      {
        return;
      }

      Options.Songlist[e.RowIndex].GetType().InvokeMember(tracksGrid.Columns[e.ColumnIndex].Name,
                                      BindingFlags.SetProperty | BindingFlags.SetField, null, Options.Songlist[e.RowIndex],
                                      new object[] { e.Value });
    }

    #endregion

    #region Drag & Drop

    /// <summary>
    ///   Handle Drag and Drop Operation
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left && tracksGrid.SelectedRows.Count > 0)
      {
        // Remember the point where the mouse down occurred. The DragSize indicates
        // the size that the mouse can move before a drag event should be started.                
        Size dragSize = SystemInformation.DragSize;

        // Create a rectangle using the DragSize, with the mouse position being
        // at the center of the rectangle.
        _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)),
                                              dragSize);
      }
      else
        _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    ///   The mouse moves. Do a Drag & Drop if necessary
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        // If the mouse moves outside the rectangle, start the drag.
        if (_dragBoxFromMouseDown != Rectangle.Empty &&
            !_dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          // The screenOffset is used to account for any desktop bands 
          // that may be at the top or left side of the screen when 
          // determining when to cancel the drag drop operation.
          _screenOffset = SystemInformation.WorkingArea.Location;

          List<TrackData> selectedRows = new List<TrackData>();
          foreach (DataGridViewRow row in tracksGrid.Rows)
          {
            if (!row.Selected)
              continue;

            TrackData track = Options.Songlist[row.Index];
            selectedRows.Add(track);
          }
          tracksGrid.DoDragDrop(selectedRows, DragDropEffects.Copy | DragDropEffects.Move);
        }
      }
    }

    /// <summary>
    ///   The Mouse has been released
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    ///   Determines, if Drag and drop should continue
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      DataGridView dg = sender as DataGridView;

      if (dg != null)
      {
        Form f = dg.FindForm();
        // Cancel the drag if the mouse moves off the form. The screenOffset
        // takes into account any desktop bands that may be at the top or left
        // side of the screen.
        if (((MousePosition.X - _screenOffset.X) < f.DesktopBounds.Left) ||
            ((MousePosition.X - _screenOffset.X) > f.DesktopBounds.Right + _main.Player.PlayListForm.Width) ||
            ((MousePosition.Y - _screenOffset.Y) < f.DesktopBounds.Top) ||
            ((MousePosition.Y - _screenOffset.Y) > f.DesktopBounds.Bottom))
        {
          e.Action = DragAction.Cancel;
        }
      }
    }

    #endregion

    #region Context Menu Handler

    /// <summary>
    ///   Add to Burner
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_AddToBurner(object o, EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        _main.BurnGridView.AddToBurner(track);
      }
    }

    /// <summary>
    ///   Add to Converter Grid
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_AddToConvert(object o, EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        _main.ConvertGridView.AddToConvert(track);
      }
    }

    /// <summary>
    ///   Add to Playlist
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_AddToPlayList(object o, EventArgs e)
    {
      // get current playlist count
      int playlistCount = _main.Player.PlayList.Count;
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        PlayListData playListItem = new PlayListData();
        playListItem.FileName = track.FullFileName;
        playListItem.Title = track.Title;
        playListItem.Artist = track.Artist;
        playListItem.Album = track.Album;
        playListItem.Duration = track.Duration.Substring(3, 5); // Just get Minutes and seconds
        _main.Player.PlayList.Add(playListItem);
      }

      // Start playing the songs just added, if player is idle
      if (!_main.Player.IsPlaying && _main.Player.PlayList.Count > playlistCount)
      {
        int index = playlistCount;
        if (index == -1)
        {
          index = 0;
        }
        _main.Player.Play(index);
      }
    }

    /// <summary>
    ///   Save as Playlist
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_SaveAsPlayList(object o, EventArgs e)
    {
      SortableBindingList<PlayListData> playList = new SortableBindingList<PlayListData>();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        PlayListData playListItem = new PlayListData();
        playListItem.FileName = track.FullFileName;
        playListItem.Title = track.Title;
        playListItem.Artist = track.Artist;
        playListItem.Album = track.Album;
        playListItem.Duration = track.Duration.Substring(3, 5); // Just get Minutes and seconds
        playList.Add(playListItem);
      }

      SaveFileDialog sFD = new SaveFileDialog();
      sFD.InitialDirectory = _main.CurrentDirectory;
      sFD.Filter = "M3U Format (*.m3u)|*.m3u|PLS Format (*.pls)|*.pls";
      if (sFD.ShowDialog() == DialogResult.OK)
      {
        IPlayListIO saver = PlayListFactory.CreateIO(sFD.FileName);
        saver.Save(playList, sFD.FileName, true);
      }
    }

    /// <summary>
    ///   Create a Folder Thumb out of the selected Track Picture
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_CreateFolderThumb(object o, EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];
        SavePicture(track);
      }
    }

    public void tracksGrid_Copy(object o, EventArgs e)
    {
      contextMenu.Items[2].Enabled = true; // Enable the Paste Menu item
      Options.CopyPasteBuffer.Clear();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        Options.CopyPasteBuffer.Add(track);
      }
      _actionCopy = true;
    }

    public void tracksGrid_Cut(object o, EventArgs e)
    {
      contextMenu.Items[2].Enabled = true; // Enable the Paste Menu item
      Options.CopyPasteBuffer.Clear();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        Options.CopyPasteBuffer.Add(track);
      }
      _actionCopy = false;
    }

    public void tracksGrid_Paste(object o, EventArgs e)
    {
      contextMenu.Items[2].Enabled = false; // Disable the Paste Menu item
      if (Options.CopyPasteBuffer.Count == 0)
      {
        log.Debug("Copy Paste: No files in Copy Buffer");
        return;
      }


      foreach (TrackData track in Options.CopyPasteBuffer)
      {
        string targetFile = Path.Combine(_main.CurrentDirectory, track.FileName);
        try
        {
          if (_actionCopy)
          {
            log.Debug("TracksGrid: Copying file {0} to {1}", track.FullFileName, targetFile);
            FileSystem.CopyFile(track.FullFileName, targetFile, UIOption.AllDialogs, UICancelOption.DoNothing);
          }
          else
          {
            log.Debug("TracksGrid: Moving file {0} to {1}", track.FullFileName, targetFile);
            FileSystem.MoveFile(track.FullFileName, targetFile, UIOption.AllDialogs, UICancelOption.DoNothing);
          }
        }
        catch (Exception ex)
        {
          log.Error("TracksGrid: Error copying / moving file {0}. {1}", track.FullFileName, ex.Message);
        }
      }
      _main.RefreshTrackList();
    }


    private void tracksGrid_Delete(object sender, EventArgs e)
    {
      DeleteTracks();
    }

    private void tracksGrid_SelectAll(object sender, EventArgs e)
    {
      tracksGrid.SelectAll();
    }


    private void lookupTitlleOnGoogleImagesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (tracksGrid.SelectedRows.Count == 0)
      {
        return;
      }

      TrackData track = Options.Songlist[tracksGrid.SelectedRows[0].Index];
      string songString = track.Artist + " " + track.Title;
      songString = songString.Replace(" ", "+");

      string url = "https://www.google.com/search?tbm=isch&q=" + songString;
      System.Diagnostics.Process.Start(url);
    }

    #endregion

    #endregion
  }
}