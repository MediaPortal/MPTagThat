using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

using TagLib;
using MPTagThat.Core;
using MPTagThat.Core.Amazon;
using MPTagThat.Core.MusicBrainz;
using MPTagThat.Dialogues;
using MPTagThat.Player;

namespace MPTagThat.GridView
{
  public partial class GridViewTracks : UserControl
  {
    #region Variables
    private delegate void ThreadSafeGridDelegate();
    private delegate void ThreadSafeGridDelegate1(object sender, DoWorkEventArgs e);
    private delegate void ThreadSafeAddTracksDelegate(TrackData track);
    private delegate void ThreadSafeAddErrorDelegate(string file, string message);
    private GridViewColumns gridColumns;

    private Main _main;

    private bool _actionCopy = false;

    private bool _itemsChanged;
    private List<string> _lyrics = new List<string>();
    private List<string> _stdOutList = new List<string>();

    private SortableBindingList<TrackData> bindingList = new SortableBindingList<TrackData>();

    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();

    private Progress dlgProgress;

    private Rectangle _dragBoxFromMouseDown;
    private Point _screenOffset;

    private Thread _asyncThread = null;
    private BackgroundWorker _bgWorker = null;

    private Cursor _numberingCursor = Util.CreateCursorFromResource("CursorNumbering", 0, 0);
    private Cursor _defaultCursor = Cursors.Default;
    #endregion

    #region Properties
    /// <summary>
    /// Returns the GridView
    /// </summary>
    public System.Windows.Forms.DataGridView View
    {
      get { return tracksGrid; }
    }

    /// <summary>
    /// Returns the Selected Track
    /// </summary>
    public TrackData SelectedTrack
    {
      get { return bindingList[tracksGrid.CurrentRow.Index]; }
    }

    /// <summary>
    /// Do we have any changes pending?
    /// </summary>
    public bool Changed
    {
      get { return _itemsChanged; }
      set { _itemsChanged = value; }
    }

    /// <summary>
    /// Returns the Bindinglist with all the Rows
    /// </summary>
    public SortableBindingList<TrackData> TrackList
    {
      get { return bindingList; }
    }
    #endregion

    #region Constructor
    public GridViewTracks()
    {
      InitializeComponent();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      // Load the Settings
      gridColumns = new GridViewColumns();

      // Setup Dataview Grid
      tracksGrid.AutoGenerateColumns = false;
      tracksGrid.DataSource = bindingList;
      tracksGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable; // Handle Copy 

      // Setup Event Handler
      tracksGrid.ColumnWidthChanged += new DataGridViewColumnEventHandler(tracksGrid_ColumnWidthChanged);
      tracksGrid.CurrentCellDirtyStateChanged += new EventHandler(tracksGrid_CurrentCellDirtyStateChanged);
      tracksGrid.DataError += new DataGridViewDataErrorEventHandler(tracksGrid_DataError);
      tracksGrid.CellEndEdit += new DataGridViewCellEventHandler(tracksGrid_CellEndEdit);
      tracksGrid.CellValueChanged += new DataGridViewCellEventHandler(tracksGrid_CellValueChanged);
      tracksGrid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(tracksGrid_EditingControlShowing);
      tracksGrid.Sorted += new EventHandler(tracksGrid_Sorted);
      tracksGrid.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(tracksGrid_ColumnHeaderMouseClick);
      tracksGrid.MouseDown += new MouseEventHandler(tracksGrid_MouseDown);
      tracksGrid.MouseUp += new MouseEventHandler(tracksGrid_MouseUp);
      tracksGrid.MouseMove += new MouseEventHandler(tracksGrid_MouseMove);
      tracksGrid.QueryContinueDrag += new QueryContinueDragEventHandler(tracksGrid_QueryContinueDrag);
      tracksGrid.MouseEnter += new EventHandler(tracksGrid_MouseEnter);

      // The Color for the Image Cell for the Rating is not handled correctly. so we need to handle it via an event
      tracksGrid.SelectionChanged += new EventHandler(tracksGrid_SelectionChanged);

      // Now Setup the columns, we want to display
      CreateColumns();

      LocaliseScreen();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Set Main Ref to Main
    /// Can't do it in constructir due to problems with Designer
    /// </summary>
    /// <param name="main"></param>
    public void SetMainRef(Main main)
    {
      _main = main;
    }

    #region Save
    /// <summary>
    /// Save the Selected files only
    /// </summary>
    public void Save()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      dlgProgress = new Progress();
      dlgProgress.Owner = _main;
      dlgProgress.Header = localisation.ToString("progress", "SavingHeader");
      ShowForm(dlgProgress);

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        row.Cells[1].Value = "";

        if (!row.Selected)
        {
          continue;
        }

        count++;
        try
        {
          Application.DoEvents();
          dlgProgress.UpdateProgress(ProgressBarStyle.Blocks, string.Format(localisation.ToString("progress", "Saving"), count, trackCount), count, trackCount, true);
          if (dlgProgress.IsCancelled)
          {
            dlgProgress.Close();
            return;
          }
          TrackData track = bindingList[row.Index];
          SaveTrack(track, row.Index);

        }
        catch (Exception ex)
        {
          row.Cells[1].Value = localisation.ToString("message", "Error");
          AddErrorMessage(bindingList[row.Index].File.Name, ex.Message);
        }
      }

      dlgProgress.Close();

      _itemsChanged = false;
      // check, if we still have changed items in the list
      foreach (TrackData track in bindingList)
      {
        if (track.Changed)
          _itemsChanged = true;
      }

      Util.LeaveMethod(Util.GetCallingMethod());

    }

    /// <summary>
    /// Save All changed files, regardless, if they are selected or not
    /// </summary>
    public void SaveAll()
    {
      SaveAll(true);
    }

    /// <summary>
    /// Save All changed files, regardless, if they are selected or not
    /// </summary>
    /// <param name="showProgressDialog">Show / Hide the progress dialogue</param>
    public void SaveAll(bool showProgressDialog)
    {
      Util.EnterMethod(Util.GetCallingMethod());

      bool bErrors = false;
      int i = 0;

      if (showProgressDialog)
      {
        dlgProgress = new Progress();
        dlgProgress.Owner = _main;
        dlgProgress.Header = localisation.ToString("progress", "SavingHeader");
        ShowForm(dlgProgress);
      }

      int trackCount = bindingList.Count;
      foreach (TrackData track in bindingList)
      {
        Application.DoEvents();

        if (showProgressDialog)
        {
          dlgProgress.UpdateProgress(ProgressBarStyle.Blocks, string.Format(localisation.ToString("progress", "Saving"), i + 1, trackCount), i + 1, trackCount, true);
          if (dlgProgress.IsCancelled)
          {
            dlgProgress.Close();
            return;
          }
        }

        if (!SaveTrack(track, i))
          bErrors = true;

        i++;
      }

      if (showProgressDialog)
        dlgProgress.Close();
      _itemsChanged = bErrors;

      Util.LeaveMethod(Util.GetCallingMethod());
    }


    /// <summary>
    /// Does the actual save of the track
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    private bool SaveTrack(TrackData track, int rowIndex)
    {
      try
      {
        tracksGrid.Rows[rowIndex].Cells[1].Value = "";
        if (track.Changed)
        {
          log.Debug("Save: Saving track: {0}", track.FullFileName);

          // The track to be saved, may be currently playing. If this is the case stop playnack to free the file
          if (track.FullFileName == _main.Player.CurrentSongPlaying)
          {
            log.Debug("Save: Song is played in Player. Stop playback to free the file");
            _main.Player.Stop();
          }

          if (Options.MainSettings.CopyArtist && track.AlbumArtist == "")
            track.AlbumArtist = track.Artist;

          if (Options.MainSettings.UseCaseConversion)
          {
            CaseConversion.CaseConversion convert = new MPTagThat.CaseConversion.CaseConversion(_main, true);
            convert.CaseConvert(track, rowIndex);
            convert.Dispose();
          }

          // Save the file 
          track.File = Util.FormatID3Tag(track.File);
          track.File.Save();


          // If we are in Database mode, we should also update the MediaPortal Database
          if (_main.TreeView.DatabaseMode)
          {
            UpdateMusicDatabase(track);
          }

          if (RenameFile(track))
          {
            // rename was ok, so get the new file into the binding list
            string ext = System.IO.Path.GetExtension(track.File.Name);
            string newFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(track.File.Name), String.Format("{0}{1}", System.IO.Path.GetFileNameWithoutExtension(track.FileName), ext));
            TagLib.ByteVector.UseBrokenLatin1Behavior = true;
            TagLib.File file = TagLib.File.Create(newFileName);
            track.File = file;
          }

          // Check, if we need to create a folder.jpg
          if (!System.IO.File.Exists(Path.Combine(Path.GetDirectoryName(track.FullFileName), "folder.jpg")) && Options.MainSettings.CreateFolderThumb)
          {
            SavePicture(track);
          }

          tracksGrid.Rows[rowIndex].Cells[1].Value = localisation.ToString("message", "Ok");
          if (rowIndex % 2 == 0)
            tracksGrid.Rows[rowIndex].DefaultCellStyle.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
          else
            tracksGrid.Rows[rowIndex].DefaultCellStyle.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.AlternatingRowBackColor;

          track.Changed = false;
        }
      }
      catch (Exception ex)
      {
        tracksGrid.Rows[rowIndex].Cells[1].Value = localisation.ToString("message", "Error");
        AddErrorMessage(track.File.Name, ex.Message);
        return false;
      }
      return true;
    }

    /// <summary>
    /// Rename the file if necessary
    /// Called by Save and SaveAll
    /// </summary>
    /// <param name="track"></param>
    private bool RenameFile(TrackData track)
    {
      string originalFileName = System.IO.Path.GetFileName(track.File.Name);
      if (originalFileName != track.FileName)
      {
        string ext = System.IO.Path.GetExtension(track.File.Name);
        string newFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(track.File.Name), String.Format("{0}{1}", System.IO.Path.GetFileNameWithoutExtension(track.FileName), ext));
        System.IO.File.Move(track.File.Name, newFileName);
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
        _bgWorker.DoWork += new DoWorkEventHandler(IdentifyFilesThread);
      }

      if (!_bgWorker.IsBusy)
      {
        _bgWorker.RunWorkerAsync();
      }
    }
    /// <summary>
    /// Tag the the Selected files from Internet
    /// </summary>
    private void IdentifyFilesThread(object sender, DoWorkEventArgs e)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      //Make calls to Tracksgrid Threadsafe
      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeGridDelegate1 d = new ThreadSafeGridDelegate1(IdentifyFilesThread);
        tracksGrid.Invoke(d, new object[] { sender, e });
        return;
      }

      dlgProgress = new Progress();
      dlgProgress.Owner = _main;
      dlgProgress.Header = localisation.ToString("progress", "InternetHeader");
      ShowForm(dlgProgress);

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      MusicBrainzAlbum musicBrainzAlbum = new MusicBrainzAlbum();

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        row.Cells[1].Value = "";

        if (!row.Selected)
        {
          continue;
        }

        count++;
        try
        {
          Application.DoEvents();
          dlgProgress.UpdateProgress(ProgressBarStyle.Blocks, string.Format(localisation.ToString("progress", "Internet"), count, trackCount), count, trackCount, true);
          if (dlgProgress.IsCancelled)
          {
            dlgProgress.Close();
            return;
          }
          TrackData track = bindingList[row.Index];

          using (MusicBrainzTrackInfo trackinfo = new MusicBrainzTrackInfo())
          {
            log.Debug("Identify: Processing file: {0}", track.FullFileName);
            dlgProgress.StatusLabel2 = localisation.ToString("progress", "InternetMusicBrainz");
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
                  if (_main.ShowForm(dlgAlbumResults) == DialogResult.OK)
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
                  dlgProgress.StatusLabel2 = localisation.ToString("progress", "InternetAlbum");
                  Application.DoEvents();
                  if (dlgProgress.IsCancelled)
                  {
                    dlgProgress.Close();
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
                if (track.Pictures.Length == 0 || Options.MainSettings.OverwriteExistingCovers)
                {
                  ByteVector vector = musicBrainzAlbum.Amazon.AlbumImage;
                  if (vector != null)
                  {
                    Picture pic = new Picture(vector);
                    pic.MimeType = "image/jpg";
                    pic.Description = "";
                    pic.Type = PictureType.FrontCover;
                    track.Pictures = new TagLib.IPicture[] { pic };
                  }
                }
              }

              SetBackgroundColorChanged(row.Index);
              track.Changed = true;
              _itemsChanged = true;
              row.Cells[1].Value = localisation.ToString("message", "Ok");
            }
          }
        }
        catch (Exception ex)
        {
          row.Cells[1].Value = localisation.ToString("message", "Error");
          AddErrorMessage(bindingList[row.Index].File.Name, ex.Message);
        }
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      dlgProgress.Close();

      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Cover Art
    public void GetCoverArt()
    {
      if (_asyncThread == null)
      {
        _asyncThread = new Thread(new ThreadStart(GetCoverArtThread));
        _asyncThread.Name = "GetCoverArt";
      }

      if (_asyncThread.ThreadState != ThreadState.Running)
      {
        _asyncThread = new Thread(new ThreadStart(GetCoverArtThread));
        _asyncThread.Start();
      }
    }

    /// <summary>
    /// Get Cover Art via Amazon Webservice
    /// </summary>
    private void GetCoverArtThread()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      //Make calls to Tracksgrid Threadsafe
      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeGridDelegate d = new ThreadSafeGridDelegate(GetCoverArtThread);
        tracksGrid.Invoke(d, new object[] { });
        return;
      }

      dlgProgress = new Progress();
      dlgProgress.Owner = _main;
      dlgProgress.Header = localisation.ToString("progress", "CoverArtHeader");
      ShowForm(dlgProgress);

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      AmazonAlbum amazonAlbum = null;

      bool isMultipleArtistAlbum = false;
      string savedArtist = "";
      string savedAlbum = "";
      string savedFolder = "";

      Picture folderThumb = null;

      // Find out, if we deal with a multiple artist album and submit only the album name
      // If we have different artists, then it is a multiple artist album.
      // BUT: if the album is different, we don't have a multiple artist album and should submit the artist as well
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        row.Cells[1].Value = "";

        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];
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

      savedAlbum = "";
      savedArtist = "";

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        row.Cells[1].Value = "";

        if (!row.Selected)
        {
          continue;
        }

        count++;
        try
        {
          Application.DoEvents();
          dlgProgress.UpdateProgress(ProgressBarStyle.Blocks, string.Format(localisation.ToString("progress", "CoverArt"), count, trackCount), count, trackCount, true);
          if (dlgProgress.IsCancelled)
          {
            dlgProgress.Close();
            return;
          }
          TrackData track = bindingList[row.Index];

          log.Debug("CoverArt: Retrieving coverart for: {0} - {1}", track.Artist, track.Album);
          // Should we take an existing folder.jpg instead of searching the web
          if (Options.MainSettings.EmbedFolderThumb)
          {
            if (folderThumb == null || Path.GetDirectoryName(track.FullFileName) != savedFolder)
            {
              savedFolder = Path.GetDirectoryName(track.FullFileName);
              folderThumb = GetFolderThumb(savedFolder);
            }

            if (folderThumb != null)
            {
              // Only write a picture if we don't have a picture OR Overwrite Pictures is set
              if (track.Pictures.Length == 0 || Options.MainSettings.OverwriteExistingCovers)
              {
                log.Debug("CoverArt: Using existing folder.jpg");
                // Prepare Picture Array and then add the cover retrieved
                List<IPicture> pics = new List<IPicture>();
                pics.Add(folderThumb);
                track.Pictures = pics.ToArray();

                SetBackgroundColorChanged(row.Index);
                track.Changed = true;
                _itemsChanged = true;
                row.Cells[1].Value = localisation.ToString("message", "Ok");
                _main.FileInfoPanel.FillPanel();
              }
              continue;
            }
          }

          // If we don't have an Album don't do any query
          if (track.Album == "")
            continue;

          // Only retrieve the Cover Art, if we don't have it yet
          if (track.Album != savedAlbum || (track.Artist != savedArtist && !isMultipleArtistAlbum) || amazonAlbum == null)
          {
            savedArtist = track.Artist;
            savedAlbum = track.Album;

            List<AmazonAlbum> albums = new List<AmazonAlbum>();
            using (AmazonAlbumInfo amazonInfo = new AmazonAlbumInfo())
            {
              string searchArtist = isMultipleArtistAlbum ? "" : track.Artist;
              albums = amazonInfo.AmazonAlbumSearch(searchArtist, track.Album);
            }

            amazonAlbum = null;
            if (albums.Count > 0)
            {
              if (albums.Count == 1)
              {
                amazonAlbum = albums[0];
              }
              else
              {
                AmazonAlbumSearchResults dlgAlbumResults = new AmazonAlbumSearchResults(albums);
                dlgAlbumResults.Artist = track.Artist;
                dlgAlbumResults.Album = track.Album;
                dlgAlbumResults.FileDetails = track.FullFileName;

                dlgAlbumResults.Owner = _main;
                if (_main.ShowForm(dlgAlbumResults) == DialogResult.OK)
                {
                  if (dlgAlbumResults.SelectedListItem > -1)
                    amazonAlbum = albums[dlgAlbumResults.SelectedListItem];
                  else
                    amazonAlbum = albums[0];
                }
                dlgAlbumResults.Dispose();
              }

              if (amazonAlbum == null)
              {
                log.Debug("CoverArt: Album Selection cancelled");
                continue;
              }
            }
            else
            {
              log.Debug("CoverArt: No coverart found");
            }
          }

          // Now update the Cover Art
          if (amazonAlbum != null)
          {
            // Only write a picture if we don't have a picture OR Overwrite Pictures is set
            if (track.Pictures.Length == 0 || Options.MainSettings.OverwriteExistingCovers)
            {
              ByteVector vector = amazonAlbum.AlbumImage;
              if (vector != null)
              {
                // Prepare Picture Array and then add the cover retrieved
                List<IPicture> pics = new List<IPicture>();
                Picture pic = new Picture(vector);
                pics.Add(pic);

                track.Pictures = pics.ToArray();
              }
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
              catch (Exception)
              { }
              if (year > 0 && track.Year == 0)
                track.Year = year;
            }

            SetBackgroundColorChanged(row.Index);
            track.Changed = true;
            _itemsChanged = true;
            row.Cells[1].Value = localisation.ToString("message", "Ok");
            _main.FileInfoPanel.FillPanel();
          }

        }
        catch (Exception ex)
        {
          row.Cells[1].Value = localisation.ToString("message", "Error");
          AddErrorMessage(bindingList[row.Index].File.Name, ex.Message);
        }
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      dlgProgress.Close();

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Return the folder.jpg as a Taglib.Picture
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    public Picture GetFolderThumb(string folder)
    {
      string thumb = Path.Combine(folder, "folder.jpg");
      if (!System.IO.File.Exists(thumb))
      {
        return null;
      }

      try
      {

        Picture pic = new Picture(thumb);
        return pic;
      }
      catch (Exception ex)
      {
        log.Error("Exception loading thumb file: {0} {1}", thumb, ex.Message);
        return null;
      }
    }

    /// <summary>
    /// Save the Picture of the track as folder.jpg
    /// </summary>
    /// <param name="track"></param>
    public void SavePicture(TrackData track)
    {
      if (track.NumPics > 0)
      {
        string fileName = Path.Combine(Path.GetDirectoryName(track.FullFileName), "folder.jpg");
        try
        {
          using (System.IO.MemoryStream ms = new System.IO.MemoryStream(track.Pictures[0].Data.Data))
          {
            Image img = Image.FromStream(ms);
            if (img != null)
            {
              img.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
              System.IO.File.SetAttributes(fileName, FileAttributes.Hidden);
            }
          }
        }
        catch (Exception ex)
        {
          log.Error("Exception Saving picture: {0} {1}", fileName, ex.Message);
        }
      }
    }
    #endregion

    #region Lyrics
    public void GetLyrics()
    {
      if (_asyncThread == null)
      {
        _asyncThread = new Thread(new ThreadStart(GetLyricsThread));
        _asyncThread.Name = "GetLyrics";
      }

      if (_asyncThread.ThreadState != ThreadState.Running)
      {
        _asyncThread = new Thread(new ThreadStart(GetLyricsThread));
        _asyncThread.Start();
      }
    }

    /// <summary>
    /// Get Lyrics for selected Rows
    /// </summary>
    private void GetLyricsThread()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      //Make calls to Tracksgrid Threadsafe
      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeGridDelegate d = new ThreadSafeGridDelegate(GetLyricsThread);
        tracksGrid.Invoke(d, new object[] { });
        return;
      }

      dlgProgress = new Progress();
      dlgProgress.Owner = _main;
      dlgProgress.Header = localisation.ToString("progress", "LyricsHeader");
      ShowForm(dlgProgress);

      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;

      List<TrackData> tracks = new List<TrackData>();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        count++;
        Application.DoEvents();
        dlgProgress.UpdateProgress(ProgressBarStyle.Blocks, string.Format(localisation.ToString("progress", "Lyrics"), count, trackCount), count, trackCount, true);
        if (dlgProgress.IsCancelled)
        {
          dlgProgress.Close();
          return;
        }
        TrackData track = bindingList[row.Index];
        if (track.Lyrics == null || Options.MainSettings.OverwriteExistingLyrics)
        {
          tracks.Add(track);
        }
      }

      dlgProgress.Close();

      if (tracks.Count > 0)
      {
        try
        {
          LyricsSearch lyricssearch = new LyricsSearch(tracks);
          lyricssearch.Owner = _main;
          if (_main.ShowForm(lyricssearch) == DialogResult.OK)
          {
            DataGridView lyricsResult = lyricssearch.GridView;
            foreach (DataGridViewRow lyricsRow in lyricsResult.Rows)
            {
              if (lyricsRow.Cells[0].Value == System.DBNull.Value || lyricsRow.Cells[0].Value == null)
                continue;

              if ((bool)lyricsRow.Cells[0].Value != true)
                continue;

              foreach (DataGridViewRow row in tracksGrid.Rows)
              {
                TrackData lyricsTrack = tracks[lyricsRow.Index];
                TrackData track = bindingList[row.Index];
                if (lyricsTrack.FullFileName == track.FullFileName)
                {
                  track.Lyrics = (string)lyricsRow.Cells[5].Value;
                  SetBackgroundColorChanged(row.Index);
                  track.Changed = true;
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

      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Numbering
    public void AutoNumber()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      int numberValue = _main.MainRibbon.AutoNumber;
      if (numberValue == -1)
        return;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];

        // Get the Number of tracks, so that we don't loose it
        string[] tracks = track.Track.Split('/');
        string numTracks = "0";
        if (tracks.Length > 1)
        {
          numTracks = tracks[1];
        }

        track.Track = string.Format("{0}/{1}", numberValue.ToString(), numTracks);
        SetBackgroundColorChanged(row.Index);
        track.Changed = true;
        _itemsChanged = true;
        numberValue++;
      }
      _main.MainRibbon.AutoNumber = numberValue;
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Delete Tags / Delete Files
    /// <summary>
    /// Remove the Tags
    /// </summary>
    /// <param name="type"></param>
    public void DeleteTags(TagTypes type)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];
        try
        {
          track.File.RemoveTags(type);

          SetBackgroundColorChanged(row.Index);
          track.Changed = true;
          _itemsChanged = true;
        }
        catch (Exception ex)
        {
          log.Error("Error while Removing Tags: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[1].Value = localisation.ToString("message", "Error");
          AddErrorMessage(track.File.Name, ex.Message);

        }
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// The Del key has been pressed. Send the selected files to the recycle bin
    /// </summary>
    public void DeleteTracks()
    {
      Util.EnterMethod(Util.GetCallingMethod());

      if (tracksGrid.SelectedRows.Count > 0)
      {
        DialogResult result = MessageBox.Show(localisation.ToString("message", "DeleteConfirm"), localisation.ToString("message", "DeleteConfirmHeader"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        if (result == DialogResult.Cancel)
        {
          Util.LeaveMethod(Util.GetCallingMethod());
          return;
        }
      }

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];
        try
        {
          Util.SHFILEOPSTRUCT shf = new Util.SHFILEOPSTRUCT();
          shf.wFunc = Util.FO_DELETE;
          shf.fFlags = Util.FOF_ALLOWUNDO | Util.FOF_NOCONFIRMATION;
          shf.pFrom = track.File.Name;
          Util.SHFileOperation(ref shf);

          // Remove the file from the binding list
          bindingList.RemoveAt(row.Index);
        }
        catch (Exception ex)
        {
          log.Error("Error applying changes from MultiTagedit: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[1].Value = localisation.ToString("message", "Error");
          AddErrorMessage(track.File.Name, ex.Message);
        }
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Remove Comments / Remove Pictures
    /// <summary>
    /// Remove all Comments in te selected tracks
    /// </summary>
    public void RemoveComments()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];

        bool commentRemoved = false;
        if (track.TagType.ToLower() == "mp3")
        {
          TagLib.Id3v1.Tag id3v1tag = track.File.GetTag(TagTypes.Id3v1, true) as TagLib.Id3v1.Tag;
          TagLib.Id3v2.Tag id3v2tag = track.File.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

          if (id3v1tag.Comment != null)
          {
            track.Comment = "";
            id3v1tag.Comment = null;
            commentRemoved = true;
          }
          IEnumerator<TagLib.Id3v2.CommentsFrame> id3v2comments = id3v2tag.GetFrames<TagLib.Id3v2.CommentsFrame>().GetEnumerator();
          if (id3v2comments.MoveNext())
          {
            track.Comment = "";
            id3v2tag.RemoveFrames("COMM");
            commentRemoved = true;
          }
        }
        else
        {
          if (track.Comment != "")
          {
            commentRemoved = true;
            track.Comment = "";
          }
        }

        if (commentRemoved)
        {
          SetBackgroundColorChanged(row.Index);
          track.Changed = true;
          _itemsChanged = true;
        }
      }
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Remove all Picture data for the selected tracks
    /// </summary>
    public void RemovePictures()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];

        bool pictureRemoved = false;
        if (track.NumPics > 0)
        {
          track.Pictures = null;
          pictureRemoved = true;
        }

        if (pictureRemoved)
        {
          SetBackgroundColorChanged(row.Index);
          track.Changed = true;
          _itemsChanged = true;
        }
      }
      _main.FileInfoPanel.FillPanel();
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Validate / Fix MP3 Files
    /// <summary>
    /// Validates an MP3 File using mp3val
    /// </summary>
    public void ValidateMP3File()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];

        if (track.TagType.ToLower() == "mp3")
        {
          track.MP3ValidationError = MP3Val.ValidateMp3File(track.FullFileName);
          if (track.MP3ValidationError != TrackData.MP3Error.NoError)
          {
            SetColorMP3Errors(row.Index, track.MP3ValidationError);
          }
        }
      }
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Fixes errors in an MP3 file using mp3val
    /// </summary>
    public void FixMP3File()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];
        if (track.TagType.ToLower() == "mp3")
        {
          track.MP3ValidationError = MP3Val.FixMp3File(track.FullFileName);
          if (track.MP3ValidationError != TrackData.MP3Error.NoError)
          {
            SetColorMP3Errors(row.Index, track.MP3ValidationError);
          }
        }
      }
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Misc Methods
    /// <summary>
    /// Discards any changes
    /// </summary>
    public void DiscardChanges()
    {
      _itemsChanged = false;
    }

    /// <summary>
    /// Checks for Pending Changes
    /// </summary>
    public void CheckForChanges()
    {
      if (Changed)
      {
        DialogResult result = MessageBox.Show(localisation.ToString("message", "Save_Changes"), localisation.ToString("message", "Save_Changes_Title"), MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
          SaveAll();
        else
          DiscardChanges();
      }
    }

    /// <summary>
    /// Checks, if we have something selected
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
          MessageBox.Show(localisation.ToString("message", "NoSelection"), localisation.ToString("message", "NoSelectionHeader"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Sets the Color for changed Items
    /// </summary>
    /// <param name="index"></param>
    public void SetBackgroundColorChanged(int index)
    {
      tracksGrid.Rows[index].DefaultCellStyle.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedBackColor;
      tracksGrid.Rows[index].DefaultCellStyle.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedForeColor;
    }

    /// <summary>
    /// Sets the Color for Tracks, that contain errors found by mp3val
    /// </summary>
    /// <param name="index"></param>
    public void SetColorMP3Errors(int index, TrackData.MP3Error error)
    {
      if (error == TrackData.MP3Error.Fixable || error == TrackData.MP3Error.Fixed) 
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FixableErrorBackColor;
        tracksGrid.Rows[index].DefaultCellStyle.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FixableErrorForeColor;
      }
      else if (error == TrackData.MP3Error.NonFixable) 
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.NonFixableErrorBackColor;
        tracksGrid.Rows[index].DefaultCellStyle.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.NonFixableErrorForeColor;
      }

      if (error == TrackData.MP3Error.Fixed)
      {
        tracksGrid.Rows[index].Cells[1].Value = localisation.ToString("message", "Fixed");
      }
    }

    /// <summary>
    /// Adds an Error Message to the Message Grid
    /// </summary>
    /// <param name="file"></param>
    /// <param name="message"></param>
    public void AddErrorMessage(string file, string message)
    {
      if (_main.ErrorGridView.InvokeRequired)
      {
        ThreadSafeAddErrorDelegate d = new ThreadSafeAddErrorDelegate(AddErrorMessage);
        _main.ErrorGridView.Invoke(d, new object[] { file, message });
        return;
      }

      _main.ErrorGridView.Rows.Add(file, message);
    }
    #endregion

    #region Script Handling
    /// <summary>
    /// Executes a script on all selected rows
    /// </summary>
    /// <param name="scriptFile"></param>
    public void ExecuteScript(string scriptFile)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      Assembly assembly = ServiceScope.Get<IScriptManager>().Load(scriptFile);

      try
      {
        if (assembly != null)
        {
          IScript script = (IScript)assembly.CreateInstance("Script");

          List<TrackData> tracks = new List<TrackData>();
          foreach (DataGridViewRow row in tracksGrid.Rows)
          {
            if (!row.Selected)
            {
              continue;
            }

            tracks.Add(bindingList[row.Index]);
          }
          script.Invoke(tracks);
          foreach (DataGridViewRow row in tracksGrid.Rows)
          {
            if (!row.Selected)
            {
              continue;
            }

            if (bindingList[row.Index].Changed)
            {
              SetBackgroundColorChanged(row.Index);
              _itemsChanged = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("Script Execution failed: {0}", ex.Message);
        MessageBox.Show(localisation.ToString("message", "Script_Compile_Failed"), localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      }
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Folder Scanning
    public void FolderScan()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      bindingList = new SortableBindingList<TrackData>();
      tracksGrid.DataSource = bindingList;
      GC.Collect();

      TagLib.File file = null;
      DirectoryInfo dirInfo = null;
      List<FileInfo> files = new List<FileInfo>();

      string selectedFolder = _main.CurrentDirectory;

      if (!Directory.Exists(selectedFolder))
        return;

      _main.FolderScanning = true;

      try // just in case we are lacking sufficent permissions
      {
        dirInfo = new DirectoryInfo(selectedFolder);
        log.Debug("FolderScan: Retrieving files from. {0}", selectedFolder);
        GetFiles(selectedFolder, ref files, _main.TreeView.ScanFolderRecursive);
      }
      catch (Exception)
      {
      }

      dlgProgress = new Progress();
      dlgProgress.Header = localisation.ToString("progress", "ScanningHeader");
      int x = _main.ClientSize.Width / 2 - dlgProgress.Width / 2;
      int y = _main.ClientSize.Height / 2 - dlgProgress.Height / 2;
      Point clientLocation = _main.Location;
      x += clientLocation.X;
      y += clientLocation.Y;
      dlgProgress.Location = new Point(x, y);
      dlgProgress.Show();

      // The Folder scan should stay on Top
      dlgProgress.TopMost = true;

      string dlgMessage = localisation.ToString("progress", "Scanning");

      int count = 1;
      int trackCount = files.Count;
      log.Debug("FolderScan: Found {0} files", trackCount);
      foreach (FileInfo fi in files)
      {
        Application.DoEvents();
        dlgProgress.UpdateProgress(ProgressBarStyle.Blocks, string.Format(dlgMessage, count, trackCount), count, trackCount, true);
        if (dlgProgress.IsCancelled)
        {
          _main.FolderScanning = false;
          dlgProgress.Close();
          return;
        }
        try
        {
          if (Util.IsAudio(fi.FullName))
          {
            // Read the Tag
            try
            {
              TagLib.ByteVector.UseBrokenLatin1Behavior = true;
              file = TagLib.File.Create(fi.FullName);
              AddTrack(new TrackData(file));
            }
            catch (CorruptFileException)
            {
              log.Warn("FolderScan: Ignoring track {0} - Corrupt File!", fi.FullName);
            }
            catch (UnsupportedFormatException)
            {
              log.Warn("FolderScan: Ignoring track {0} - Unsupported format!", fi.FullName);
            }
          }
        }
        catch (PathTooLongException)
        {
          log.Warn("FolderScan: Ignoring track {0} - path too long!", fi.FullName);
          continue;
        }
        count++;
      }

      _main.FolderScanning = false;
      dlgProgress.Close();

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
          tracksGrid.Rows[0].Selected = false;
          tracksGrid.Rows[0].Cells[10].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
        }
      }
      catch (ArgumentOutOfRangeException) { }

      // If MP3 Validation is turned on, set the color
      if (Options.MainSettings.MP3Validate)
      {
        ChangeErrorRowColor();
      }

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Read a Folder and return the files
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="foundFiles"></param>
    void GetFiles(string folder, ref List<FileInfo> foundFiles, bool recursive)
    {
      try
      {
        if (recursive)
        {
          string[] subFolders = Directory.GetDirectories(folder);
          for (int i = 0; i < subFolders.Length; ++i)
          {
            GetFiles(subFolders[i], ref foundFiles, recursive);
          }
        }

        FileInfo[] files = new DirectoryInfo(folder).GetFiles();
        foundFiles.AddRange(files);
      }
      catch (Exception ex)
      {
        ServiceScope.Get<ILogger>().Error(ex);
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

      string connection = string.Format(@"Data Source={0}", MPTagThat.Core.Options.MainSettings.MediaPortalDatabase);
      try
      {
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = System.Data.CommandType.Text;
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
      bindingList = new SortableBindingList<TrackData>();
      tracksGrid.DataSource = bindingList;
      GC.Collect();

      dlgProgress = new Progress();
      dlgProgress.Header = localisation.ToString("progress", "ScanningHeader");
      int x = _main.ClientSize.Width / 2 - dlgProgress.Width / 2;
      int y = _main.ClientSize.Height / 2 - dlgProgress.Height / 2;
      Point clientLocation = _main.Location;
      x += clientLocation.X;
      y += clientLocation.Y;
      dlgProgress.Location = new Point(x, y);
      dlgProgress.Show();

      // The Folder scan should stay on Top
      dlgProgress.TopMost = true;

      string dlgMessage = localisation.ToString("progress", "Scanning");

      TagLib.File file = null;
      int count = 1;
      foreach (string song in songs)
      {
        Application.DoEvents();
        dlgProgress.UpdateProgress(ProgressBarStyle.Blocks, string.Format(dlgMessage, count, songs.Count), count, songs.Count, true);
        if (dlgProgress.IsCancelled)
        {
          _main.FolderScanning = false;
          dlgProgress.Close();
          return;
        }

        try
        {
          TagLib.ByteVector.UseBrokenLatin1Behavior = true;
          file = TagLib.File.Create(song);
          AddTrack(new TrackData(file));
        }
        catch (CorruptFileException)
        {
          log.Warn("FolderScan: Ignoring track {0} - Corrupt File!", song);
        }
        catch (UnsupportedFormatException)
        {
          log.Warn("FolderScan: Ignoring track {0} - Unsupported format!", song);
        }
        count++;
      }

      dlgProgress.Close();

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
          tracksGrid.Rows[0].Selected = false;
          tracksGrid.Rows[0].Cells[10].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
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
      string db = MPTagThat.Core.Options.MainSettings.MediaPortalDatabase;

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

      string originalFileName = System.IO.Path.GetFileName(track.File.Name);
      string newFileName = "";
      if (originalFileName != track.FileName)
      {
        string ext = System.IO.Path.GetExtension(track.File.Name);
        newFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(track.File.Name), String.Format("{0}{1}", System.IO.Path.GetFileNameWithoutExtension(track.FileName), ext));
      }
      else
      {
        newFileName = track.FullFileName;
      }


      string sql = String.Format(
          @"update tracks 
              set strArtist = '{0}', strAlbumArtist = '{1}', strAlbum = '{2}', 
              strGenre = '{3}', strTitle = '{4}', iTrack = {5}, iNumTracks = {6}, 
              iYear = {7}, iRating = {8}, iDisc = {9}, iNumDisc = {10}, strLyrics = '{11}', strPath = '{12}'
            where strPath = '{13}'",
        Util.RemoveInvalidChars(track.Artist), Util.RemoveInvalidChars(track.AlbumArtist), Util.RemoveInvalidChars(track.Album),
        Util.RemoveInvalidChars(track.Genre), Util.RemoveInvalidChars(track.Title), trackNumber, trackTotal,
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
          cmd.CommandType = System.Data.CommandType.Text;
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
    #endregion

    #endregion

    #region Private Methods
    #region Localisation
    /// <summary>
    /// Language Change event has been fired. Apply the new language
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
      contextMenu.Items[7].Text = localisation.ToString("contextmenu", "AddBurner");
      contextMenu.Items[8].Text = localisation.ToString("contextmenu", "AddConverter");
      contextMenu.Items[9].Text = localisation.ToString("contextmenu", "AddPlaylist");
      contextMenu.Items[10].Text = localisation.ToString("contextmenu", "SavePlaylist");
      contextMenu.Items[12].Text = localisation.ToString("contextmenu", "CreateFolderThumb");
    }
    #endregion

    /// <summary>
    /// Create the Columns of the Grid based on the users setting
    /// </summary>
    private void CreateColumns()
    {
      Util.EnterMethod(Util.GetCallingMethod());

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

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Save the settings
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
    /// Adds a Track to the data grid
    /// </summary>
    /// <param name="track"></param>
    private void AddTrack(TrackData track)
    {
      if (track == null)
        return;


      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeAddTracksDelegate d = new ThreadSafeAddTracksDelegate(AddTrack);
        tracksGrid.Invoke(d, new object[] { track });
        return;
      }

      // Validate the MP3 File
      if (Options.MainSettings.MP3Validate && track.TagType.ToLower() == "mp3")
      {
        track.MP3ValidationError = MP3Val.ValidateMp3File(track.FullFileName);
      }

      bindingList.Add(track);
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
        TrackData track = bindingList[row.Index];
        
        if (track.MP3ValidationError == TrackData.MP3Error.NoError)
        {
          continue;
        }

        SetColorMP3Errors(row.Index, track.MP3ValidationError);
      }
    }
    #endregion

    #region EventHandler
    /// <summary>
    /// Handle Messages
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "languagechanged":
          LanguageChanged();
          this.Refresh();
          break;
      }
    }

    /// <summary>
    /// Handles changes in the column width
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tracksGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      // On startup we get sometimes an exception
      try
      {
        if (tracksGrid.Rows.Count > 0)
          tracksGrid.InvalidateRow(e.Column.Index);
      }
      catch (Exception)
      { }
    }

    /// <summary>
    /// Handles editing of data columns
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Only allow valid values to be entered.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// We're leaving a Cell after edit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tracksGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      // Ensure that the error icon in the row header is hidden.
      tracksGrid.Rows[e.RowIndex].ErrorText = "";
    }

    /// <summary>
    /// Value of Cell has changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tracksGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      // When changing the Status or the Header Text, ignore the Cell Changed event
      if (e.ColumnIndex == 1 || e.RowIndex == -1)
        return;

      _itemsChanged = true;
      SetBackgroundColorChanged(e.RowIndex);
      TrackData track = (TrackData)bindingList[e.RowIndex];
      track.Changed = true;
    }

    /// <summary>
    /// Clicking on the Column Header sorts by this column
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_Sorted(object sender, EventArgs e)
    {
      int i = 0;
      // Set the Color for changed rows again
      foreach (TrackData track in bindingList)
      {
        if (track.Changed)
        {
          SetBackgroundColorChanged(i);
        }
        i++;
      }
    }

    /// <summary>
    /// Handle Right Mouse Click to show Column Config Dialogue
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      // only hndle right click on Header to show config dialogue
      if (e.Button == MouseButtons.Right)
      {
        MPTagThat.Dialogues.ColumnSelect dialog = new MPTagThat.Dialogues.ColumnSelect(this);
        dialog.ShowDialog();
      }
    }

    /// <summary>
    /// We want to get Control, when editing a Cell, so that we can control the input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      DataGridView view = sender as DataGridView;
      string colName = view.CurrentCell.OwningColumn.Name;
      if (colName == "Track" || colName == "Disc")
      {
        TextBox txtbox = e.Control as TextBox;
        if (txtbox != null)
        {
          txtbox.KeyPress += new KeyPressEventHandler(txtbox_KeyPress);
        }
      }
    }

    /// <summary>
    /// Allow only Digits and the slash when enetering data for Track or Disc
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void txtbox_KeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;

      if (!Char.IsDigit(keyChar)      // 0 - 9
         &&
         keyChar != 8               // backspace
         &&
         keyChar != 13              // enter
         &&
         keyChar != '/'
         )
      {
        //  Do not display the keystroke
        e.Handled = true;
      }
    }

    /// <summary>
    /// Handle Left Mouse Click for Numbering on Click
    /// Handle Right Mouse Click to open the context Menu in the Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tracksGrid_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        Point mouse = tracksGrid.PointToClient(Cursor.Position);
        System.Windows.Forms.DataGridView.HitTestInfo selectedRow = tracksGrid.HitTest(mouse.X, mouse.Y);

        if (selectedRow.Type != DataGridViewHitTestType.ColumnHeader)
          this.contextMenu.Show(tracksGrid, new Point(e.X, e.Y));
      }
      else
      {
        // Handle Numbering on Click
        if (_main.MainRibbon.NumberingOnClick)
        {
          Point mouse = tracksGrid.PointToClient(Cursor.Position);
          System.Windows.Forms.DataGridView.HitTestInfo selectedRow = tracksGrid.HitTest(mouse.X, mouse.Y);

          if (selectedRow.Type != DataGridViewHitTestType.ColumnHeader)
          {
            int numberValue = _main.MainRibbon.AutoNumber;
            if (numberValue == -1)
              return;

            if (selectedRow.RowIndex == -1)
              return;

            TrackData track = bindingList[selectedRow.RowIndex];

            // Get the Number of tracks, so that we don't loose it
            string[] tracks = track.Track.Split('/');
            string numTracks = "0";
            if (tracks.Length > 1)
            {
              numTracks = tracks[1];
            }

            track.Track = string.Format("{0}/{1}", numberValue.ToString(), numTracks);
            SetBackgroundColorChanged(selectedRow.RowIndex);
            track.Changed = true;
            _itemsChanged = true;
            _main.MainRibbon.AutoNumber = numberValue + 1;
            tracksGrid.Refresh();
            tracksGrid.Parent.Refresh();
          }
        }
      }
    }

    /// <summary>
    /// Double Click on a Row.
    /// Start Single Edit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tracksGrid_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      MPTagThat.TagEdit.SingleTagEdit dlgSingleTagedit = new MPTagThat.TagEdit.SingleTagEdit(_main);
      Form f = (Form)dlgSingleTagedit;
      int x = (_main.ClientSize.Width / 2) - (f.Width / 2);
      int y = (_main.ClientSize.Height / 2) - (f.Height / 2);
      Point clientLocation = _main.Location;
      x += clientLocation.X;
      y += clientLocation.Y;

      f.Location = new Point(x, y);
      f.ShowDialog();
    }

    /// <summary>
    /// Mouse is over Trackgrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_MouseEnter(object sender, EventArgs e)
    {
      // Numbering on Click enabled
      if (_main.MainRibbon.NumberingOnClick)
        this.tracksGrid.Cursor = _numberingCursor;
      else
        this.tracksGrid.Cursor = _defaultCursor;
    }

    #region Context Menu Handler
    /// <summary>
    /// Add to Burner
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public void tracksGrid_AddToBurner(object o, System.EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = bindingList[row.Index];
        _main.BurnGridView.AddToBurner(track);
      }
    }

    /// <summary>
    /// Add to Converter Grid
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public void tracksGrid_AddToConvert(object o, System.EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = bindingList[row.Index];
        _main.ConvertGridView.AddToConvert(track);
      }
    }

    /// <summary>
    /// Add to Playlist
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public void tracksGrid_AddToPlayList(object o, System.EventArgs e)
    {
      // get current playlist count
      int playlistCount = _main.Player.PlayList.Count;
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = bindingList[row.Index];
        PlayListData playListItem = new PlayListData();
        playListItem.FileName = track.FullFileName;
        playListItem.Title = track.Title;
        playListItem.Artist = track.Artist;
        playListItem.Album = track.Album;
        playListItem.Duration = track.Duration.Substring(3, 5);  // Just get Minutes and seconds
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
    /// Save as Playlist
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public void tracksGrid_SaveAsPlayList(object o, System.EventArgs e)
    {
      SortableBindingList<PlayListData> playList = new SortableBindingList<PlayListData>();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = bindingList[row.Index];
        PlayListData playListItem = new PlayListData();
        playListItem.FileName = track.FullFileName;
        playListItem.Title = track.Title;
        playListItem.Artist = track.Artist;
        playListItem.Album = track.Album;
        playListItem.Duration = track.Duration.Substring(3, 5);  // Just get Minutes and seconds
        playList.Add(playListItem);
      }

      SaveFileDialog sFD = new SaveFileDialog();
      sFD.InitialDirectory = _main.CurrentDirectory;
      sFD.Filter = "M3U Format (*.m3u)|*.m3u|PLS Format (*.pls)|*.pls";
      if (sFD.ShowDialog() == DialogResult.OK)
      {
        IPlayListIO saver = PlayListFactory.CreateIO(sFD.FileName);
        saver.Save(playList, sFD.FileName);
      }
    }

    /// <summary>
    /// Create a Folder Thumb out of the selected Track Picture
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public void tracksGrid_CreateFolderThumb(object o, System.EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = bindingList[row.Index];
        SavePicture(track);
      }
    }

    public void tracksGrid_Copy(object o, System.EventArgs e)
    {
      contextMenu.Items[2].Enabled = true;  // Enable the Paste Menu item
      Options.CopyPasteBuffer.Clear();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = bindingList[row.Index];
        Options.CopyPasteBuffer.Add(track);
      }
      _actionCopy = true;
    }

    public void tracksGrid_Cut(object o, System.EventArgs e)
    {
      contextMenu.Items[2].Enabled = true;  // Enable the Paste Menu item
      Options.CopyPasteBuffer.Clear();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = bindingList[row.Index];
        Options.CopyPasteBuffer.Add(track);
      }
      _actionCopy = false;
    }

    public void tracksGrid_Paste(object o, System.EventArgs e)
    {
      contextMenu.Items[2].Enabled = false;  // Disable the Paste Menu item
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
            log.Debug("^TracksGrid: Copying file {0} to {1}", track.FullFileName, targetFile);
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
    #endregion

    /// <summary>
    /// Handle the Background Color for the Rating Image Cell
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_SelectionChanged(object sender, EventArgs e)
    {
      if (bindingList.Count == 0)
      {
        return;
      }

      for (int i = 0; i < tracksGrid.Rows.Count; i++)
      {
        if (tracksGrid.Rows[i].Selected)
          tracksGrid.Rows[i].Cells[10].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.SelectionBackColor;
        else
        {
          if (bindingList[i].Changed)
          {
            tracksGrid.Rows[i].Cells[10].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedBackColor;
          }
          else
          {
            if (i % 2 == 0)
              tracksGrid.Rows[i].Cells[10].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
            else
              tracksGrid.Rows[i].Cells[10].Style.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.AlternatingRowBackColor;
          }
        }
      }
    }

    #region Drag & Drop
    /// <summary>
    /// Handle Drag and Drop Operation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left && tracksGrid.SelectedRows.Count > 0)
      {
        // Remember the point where the mouse down occurred. The DragSize indicates
        // the size that the mouse can move before a drag event should be started.                
        Size dragSize = SystemInformation.DragSize;

        // Create a rectangle using the DragSize, with the mouse position being
        // at the center of the rectangle.
        _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
      }
      else
        _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    /// The mouse moves. Do a Drag & Drop if necessary
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_MouseMove(object sender, MouseEventArgs e)
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

            TrackData track = bindingList[row.Index];
            selectedRows.Add(track);
          }
          tracksGrid.DoDragDrop(selectedRows, DragDropEffects.Copy | DragDropEffects.Move);
        }
      }
    }

    /// <summary>
    /// The Mouse has been released
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_MouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    /// Determines, if Drag and drop should continue
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      DataGridView dg = sender as DataGridView;

      if (dg != null)
      {
        Form f = dg.FindForm();
        // Cancel the drag if the mouse moves off the form. The screenOffset
        // takes into account any desktop bands that may be at the top or left
        // side of the screen.
        if (((Control.MousePosition.X - _screenOffset.X) < f.DesktopBounds.Left) ||
            ((Control.MousePosition.X - _screenOffset.X) > f.DesktopBounds.Right + _main.Player.PlayListForm.Width) ||
            ((Control.MousePosition.Y - _screenOffset.Y) < f.DesktopBounds.Top) ||
            ((Control.MousePosition.Y - _screenOffset.Y) > f.DesktopBounds.Bottom))
        {

          e.Action = DragAction.Cancel;
        }
      }
    }
    #endregion
    #endregion
  }
}
