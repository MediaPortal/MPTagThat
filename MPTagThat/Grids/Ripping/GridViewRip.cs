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
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.AudioEncoder;
using MPTagThat.Core.Freedb;
using MPTagThat.Core.MediaChangeMonitor;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Cd;
using File = TagLib.File;

#endregion

namespace MPTagThat.GridView
{
  public partial class GridViewRip : UserControl
  {
    #region Variables

    private readonly Main _main;
    private readonly IAudioEncoder audioEncoder;

    private readonly List<SortableBindingList<CDTrackDetail>> bindingList =
      new List<SortableBindingList<CDTrackDetail>>();

    private readonly GridViewColumnsRip gridColumns;

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly IMediaChangeMonitor mediaChangeMonitor;
    private int _currentRow = -1;

    private string _musicDir;
    private string _outFile;
    private string _selectedCDRomDrive = "";
    private Thread threadRip;

    #region Nested type: ThreadSafeGridDelegate

    private delegate void ThreadSafeGridDelegate();

    #endregion

    #region Nested type: ThreadSafeMediaInsertedDelegate

    private delegate void ThreadSafeMediaInsertedDelegate(string DriveLetter);

    #endregion

    #region Nested type: ThreadSafeMediaRemovedDelegate

    private delegate void ThreadSafeMediaRemovedDelegate(string DriveLetter);

    #endregion

    #region Nested type: ThreadSafeRippingStatusDelegate

    private delegate void ThreadSafeRippingStatusDelegate(string text);

    #endregion

    #endregion

    #region Properties

    public Color BackGroundColor
    {
      set
      {
        BackColor = value;
        panelTop.BackColor = value;
      }
    }

    public string SelectedCDRomDrive
    {
      get { return _selectedCDRomDrive; }
      set
      {
        if (_selectedCDRomDrive != value)
        {
          _selectedCDRomDrive = value;
          // Change the Datasource of the grid to the correct bindinglist
          if (CurrentDriveID > -1)
          {
            // Activate the Rip Grid
            _main.Ribbon.CurrentTabPage = _main.TabRip;
            _main.BurnGridView.Hide();
            _main.TracksGridView.Hide();
            _main.RipGridView.Show();
            if (!_main.SplitterRight.IsCollapsed)
            {
              _main.SplitterRight.ToggleState();
            }
            QueryFreeDB(Convert.ToChar(_selectedCDRomDrive));
            dataGridViewRip.DataSource = bindingList[CurrentDriveID];
            if (dataGridViewRip.Rows.Count > 0)
            {
              dataGridViewRip.Rows[0].Selected = false;
            }
          }
        }
      }
    }

    public int CurrentDriveID
    {
      get { return Util.Drive2BassID(Convert.ToChar(_selectedCDRomDrive)); }
    }

    public bool Ripping
    {
      get
      {
        if (threadRip == null)
          return false;

        if (threadRip.ThreadState == ThreadState.Running)
          return true;
        else
          return false;
      }
    }

    public DataGridView View
    {
      get { return dataGridViewRip; }
    }

    #endregion

    #region ctor

    public GridViewRip(Main main)
    {
      _main = main;

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      InitializeComponent();

      dataGridViewRip.CurrentCellDirtyStateChanged += dataGridViewRip_CurrentCellDirtyStateChanged;

      // Listen to Messages
      IMessageQueue queueMessageEncoding = ServiceScope.Get<IMessageBroker>().GetOrCreate("encoding");
      queueMessageEncoding.OnMessageReceive += OnMessageReceiveEncoding;

      audioEncoder = ServiceScope.Get<IAudioEncoder>();
      mediaChangeMonitor = ServiceScope.Get<IMediaChangeMonitor>();
      mediaChangeMonitor.MediaInserted += mediaChangeMonitor_MediaInserted;
      mediaChangeMonitor.MediaRemoved += mediaChangeMonitor_MediaRemoved;

      // Get number of CD Drives found and initialise a Bindinglist for every drove
      int driveCount = BassCd.BASS_CD_GetDriveCount();
      if (driveCount == 0)
        bindingList.Add(new SortableBindingList<CDTrackDetail>()); // In case of no CD, we want a Dummy List

      _main.RipButtonsEnabled = false;

      for (int i = 0; i < driveCount; i++)
      {
        bindingList.Add(new SortableBindingList<CDTrackDetail>());
        if (BassCd.BASS_CD_IsReady(i))
        {
          _main.RipButtonsEnabled = true;
        }
      }

      // Prepare the Gridview
      gridColumns = new GridViewColumnsRip();
      dataGridViewRip.AutoGenerateColumns = false;
      dataGridViewRip.DataSource = bindingList[0];

      // Now Setup the columns, we want to display
      CreateColumns();

      SetStatusLabel("");
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Rips the selected files in the Grid
    /// </summary>
    public void RipAudioCD()
    {
      SetStatusLabel("");

      if (threadRip == null)
      {
        threadRip = new Thread(RippingThread);
        threadRip.Name = "Ripping";
      }

      if (threadRip.ThreadState != ThreadState.Running)
      {
        threadRip = new Thread(RippingThread);
        threadRip.Start();
      }
    }

    /// <summary>
    ///   Cancel the Ripping Process
    /// </summary>
    public void RipAudioCDCancel()
    {
      if (threadRip != null)
      {
        SetStatusLabel("Rip aborted");
        threadRip.Abort();
        _currentRow = -1;
        BassCd.BASS_CD_Door(CurrentDriveID, BASSCDDoor.BASS_CD_DOOR_UNLOCK);
      }
    }

    #endregion

    #region Private Methods

    private void SetStatusLabel(string text)
    {
      if (lbRippingStatus.InvokeRequired)
      {
        ThreadSafeRippingStatusDelegate d = SetStatusLabel;
        lbRippingStatus.Invoke(d, new object[] {text});
      }
      lbRippingStatus.Text = text;
    }

    #region Localisation

    /// <summary>
    ///   Language Change event has been fired. Apply the new language
    /// </summary>
    /// <param name = "language"></param>
    private void LanguageChanged()
    {
      LocaliseScreen();
    }

    private void LocaliseScreen()
    {
      // Update the column Headings
      foreach (DataGridViewColumn col in dataGridViewRip.Columns)
      {
        col.HeaderText = localisation.ToString("column_header", col.Name);
      }
    }

    #endregion

    #region Ripping

    private void RippingThread()
    {
      if (_main.TreeView.InvokeRequired)
      {
        ThreadSafeGridDelegate d = RippingThread;
        _main.TreeView.Invoke(d, new object[] {});
        return;
      }

      if (_selectedCDRomDrive == "")
      {
        log.Info("No CD drive selected. Rip not started.");
        return;
      }

      log.Trace(">>>");
      string targetDir = "";
      string encoder = null;
      try
      {
        _musicDir = _main.RipOutputDirectory;

        if (_main.RipEncoderCombo.SelectedItem != null)
        {
          encoder = (string)(_main.RipEncoderCombo.SelectedItem as Item).Value;
          Options.MainSettings.RipEncoder = encoder;
        }
        else
        {
          encoder = Options.MainSettings.RipEncoder;
        }

        if (encoder == null)
          return;

        log.Debug("Rip: Using Encoder: {0}", encoder);

        try
        {
          if (!Directory.Exists(_musicDir) && !string.IsNullOrEmpty(_musicDir))
            Directory.CreateDirectory(_musicDir);
        }
        catch (Exception ex)
        {
          SetStatusLabel(localisation.ToString("Conversion", "ErrorDirectory"));
          log.Error("Error creating Ripping directory: {0}. {1}", _musicDir, ex.Message);
          return;
        }

        // Build the Target Directory
        string artistDir = tbAlbumArtist.Text == string.Empty ? "Artist" : tbAlbumArtist.Text;
        string albumDir = tbAlbum.Text == string.Empty ? "Album" : tbAlbum.Text;

        string outFileFormat = Options.MainSettings.RipFileNameFormat;
        int index = outFileFormat.LastIndexOf('\\');
        if (index > -1)
        {
          targetDir = outFileFormat.Substring(0, index);
          targetDir = targetDir.Replace("<A>", artistDir);
          targetDir = targetDir.Replace("<O>", artistDir);
          targetDir = targetDir.Replace("<B>", albumDir);
          targetDir = targetDir.Replace("<G>", tbGenre.Text);
          targetDir = targetDir.Replace("<Y>", tbYear.Text);
          outFileFormat = outFileFormat.Substring(index + 1);
        }
        else
        {
          targetDir = string.Format(@"{0}\{1}", artistDir, albumDir);
        }

        targetDir = string.Format(@"{0}\{1}", _musicDir, targetDir);

        log.Debug("Rip: Using Target Folder: {0}", targetDir);

        try
        {
          if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);
        }
        catch (Exception ex)
        {
          SetStatusLabel(localisation.ToString("Conversion", "ErrorDirectory"));
          log.Error("Error creating Ripping directory: {0}. {1}", targetDir, ex.Message);
          return;
        }

        log.Debug("Rip: Selected CD Drive: {0}", _selectedCDRomDrive);

        int selectedDriveID = 0;
        try
        {
          // User may change to a different drive while ripping 
          selectedDriveID = CurrentDriveID;
          log.Debug("Rip: Selected drive id: {0}", selectedDriveID);
        }
        catch (Exception)
        {
          log.Debug("Rip: Error setting the drive id. Fallback to drive #0");
          selectedDriveID = 0;
        }

        // Lock the Door
        BassCd.BASS_CD_Door(CurrentDriveID, BASSCDDoor.BASS_CD_DOOR_LOCK);

        foreach (DataGridViewRow row in dataGridViewRip.Rows)
        {
          // Reset the Status field to 0
          row.Cells[1].Value = 0;
        }

        _currentRow = -1;
        foreach (DataGridViewRow row in dataGridViewRip.Rows)
        {
          // when checking and unchecking a row, we have the DBNull value
          if (row.Cells[0].Value == null || row.Cells[0].Value == DBNull.Value)
            continue;

          if ((int)row.Cells[0].Value == 0)
            continue;

          SetStatusLabel(localisation.ToString("Conversion", "Ripping"));

          _currentRow = row.Index;

          CDTrackDetail track = bindingList[selectedDriveID][_currentRow];

          int stream = BassCd.BASS_CD_StreamCreate(selectedDriveID, row.Index, BASSFlag.BASS_STREAM_DECODE);
          if (stream == 0)
          {
            log.Error("Error creating stream for Audio Track {0}. Error: {1}", _currentRow,
                      Enum.GetName(typeof (BASSError), Bass.BASS_ErrorGetCode()));
            continue;
          }

          log.Info("Ripping Audio CD Track{0} - {1}", row.Index + 1, track.Title);
          _outFile = outFileFormat;
          _outFile = _outFile.Replace("<O>", artistDir);
          _outFile = _outFile.Replace("<B>", albumDir);
          _outFile = _outFile.Replace("<G>", tbGenre.Text);
          _outFile = _outFile.Replace("<Y>", tbYear.Text);
          _outFile = _outFile.Replace("<A>", track.Artist);
          _outFile = _outFile.Replace("<K>", track.Track.ToString().PadLeft(Options.MainSettings.NumberTrackDigits, '0'));
          _outFile = _outFile.Replace("<T>", track.Title);
          _outFile = Util.MakeValidFileName(_outFile);

          _outFile = string.Format(@"{0}\{1}", targetDir, _outFile);

          _outFile = audioEncoder.SetEncoder(encoder, _outFile);

          if (audioEncoder.StartEncoding(stream) != BASSError.BASS_OK)
          {
            log.Error("Error starting Encoder for Audio Track {0}. Error: {1}", _currentRow,
                      Enum.GetName(typeof (BASSError), Bass.BASS_ErrorGetCode()));
          }

          dataGridViewRip.Rows[_currentRow].Cells[1].Value = 100;

          Bass.BASS_StreamFree(stream);
          log.Info("Finished Ripping Audio CD Track{0}", _currentRow + 1);

          try
          {
            // Now Tag the encoded File
            File file = File.Create(_outFile);
            file.Tag.AlbumArtists = new[] {tbAlbumArtist.Text};
            file.Tag.Album = tbAlbum.Text;
            file.Tag.Genres = new[] {tbGenre.Text};
            file.Tag.Year = tbYear.Text == string.Empty ? 0 : Convert.ToUInt32(tbYear.Text);
            file.Tag.Performers = new[] {track.Artist};
            file.Tag.Track = (uint)track.Track;
            file.Tag.Title = track.Title;
            file = Util.FormatID3Tag(file);
            file.Save();
          }
          catch (Exception ex)
          {
            log.Error("Error tagging encoded file {0}. Error: {1}", _outFile, ex.Message);
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("Rip: Exception: {0} {1}", ex.Message, ex.StackTrace);
      }

      _currentRow = -1;
      SetStatusLabel(localisation.ToString("Conversion", "RippingFinished"));
      Options.MainSettings.RipTargetFolder = _musicDir;
      Options.MainSettings.RipEncoder = encoder;

      // Unlock the Drive and open the door, if selected
      BassCd.BASS_CD_Door(CurrentDriveID, BASSCDDoor.BASS_CD_DOOR_UNLOCK);
      if (Options.MainSettings.RipEjectCD)
      {
        BassCd.BASS_CD_Door(CurrentDriveID, BASSCDDoor.BASS_CD_DOOR_OPEN);
      }

      // Activate the target Folder, if selected
      if (Options.MainSettings.RipActivateTargetFolder)
      {
        _main.CurrentDirectory = targetDir;
        _main.TreeView.RefreshFolders();
        _main.Ribbon.CurrentTabPage = _main.TabTag;
        _main.TracksGridView.Show();
        if (_main.SplitterRight.IsCollapsed && !Options.MainSettings.RightPanelCollapsed)
        {
          _main.SplitterRight.ToggleState();
        }
        _main.RefreshTrackList();
      }

      log.Trace("<<<");
    }

    #endregion

    #region FreedDB

    /// <summary>
    ///   Queries FreeDB for the Audio CD inserted
    /// </summary>
    /// <param name = "drive"></param>
    private void QueryFreeDB(char drive)
    {
      log.Trace(">>>");
      SetStatusLabel(localisation.ToString("Conversion", "FreeDBAccess"));
      string discId = string.Empty;
      CDInfoDetail MusicCD = new CDInfoDetail();
      int driveID = Util.Drive2BassID(drive);
      if (driveID < 0)
      {
        SetStatusLabel("");
        return;
      }
      log.Info("Starting FreeDB Lookup");
      _main.Cursor = Cursors.WaitCursor;
      bindingList[driveID].Clear();
      try
      {
        FreeDBQuery freedb = new FreeDBQuery();
        freedb.Connect();
        CDInfo[] cds = freedb.GetDiscInfo(drive);
        if (cds != null)
        {
          log.Debug("FreeDB: Found {0} matching discs.", cds.Length);
          if (cds.Length == 1)
          {
            MusicCD = freedb.GetDiscDetails(cds[0].Category, cds[0].DiscId);
          }
          else
          {
            FreeDBMultiCDSelect multiCDSelect = new FreeDBMultiCDSelect();
            multiCDSelect.CDList.ValueMember = "DiscId";
            multiCDSelect.CDList.DisplayMember = "Title";
            multiCDSelect.CDList.DataSource = cds;

            if (multiCDSelect.ShowDialog() == DialogResult.OK)
            {
              MusicCD = freedb.GetDiscDetails(cds[0].Category, multiCDSelect.DiscID);
            }
            else
            {
              MusicCD = null;
            }
          }
        }
        else
        {
          log.Debug("FreeDB: Disc could not be located in FreeDB.");
          MusicCD = null;
        }

        freedb.Disconnect();
      }
      catch (System.Net.WebException webEx)
      {
        if (webEx.Status == WebExceptionStatus.Timeout)
        {
          log.Info("FreeDB: Timeout querying FreeDB. No Data returned for CD");
          MusicCD = null;
        }
        else
        {
          log.Error("FreeDB: Exception querying Disc. {0} {1}", webEx.Message, webEx.StackTrace);
          MusicCD = null;
        }
      }
      catch (Exception ex)
      {
        log.Error("FreeDB: Exception querying Disc. {0} {1}", ex.Message, ex.StackTrace);
        MusicCD = null;
      }
      log.Info("Finished FreeDB Lookup");
      _main.Cursor = Cursors.Default;

      if (MusicCD != null)
      {
        tbAlbumArtist.Text = MusicCD.Artist;
        tbAlbum.Text = MusicCD.Title;
        tbGenre.Text = MusicCD.Genre;
        tbYear.Text = MusicCD.Year.ToString();

        foreach (CDTrackDetail trackDetail in MusicCD.Tracks)
        {
          if (trackDetail.Artist == null)
            trackDetail.Artist = MusicCD.Artist;

          bindingList[driveID].Add(trackDetail);
        }
      }
      else
      {
        int numTracks = BassCd.BASS_CD_GetTracks(driveID);
        for (int i = 0; i < numTracks; i++)
        {
          CDTrackDetail trackDetail = new CDTrackDetail();
          trackDetail.Track = i + 1;
          trackDetail.Title = string.Format("Track{0}", (i + 1).ToString().PadLeft(2, '0'));
          bindingList[driveID].Add(trackDetail);
        }
      }
      CheckRows(true);
      (dataGridViewRip.Columns[0].HeaderCell as DatagridViewCheckBoxHeaderCell).Checked = true;
      SetStatusLabel("");

      log.Trace("<<<");
    }

    #endregion

    #region Gridlayout

    /// <summary>
    ///   Create the Columns of the Grid based on the users setting
    /// </summary>
    private void CreateColumns()
    {
      // Now create the columns 
      foreach (GridViewColumn column in gridColumns.Settings.Columns)
      {
        dataGridViewRip.Columns.Add(Util.FormatGridColumn(column));
      }

      // Set the Header of the Checkbox Column
      DatagridViewCheckBoxHeaderCell chkBoxHdr = new DatagridViewCheckBoxHeaderCell();
      chkBoxHdr.OnCheckBoxClicked += chkBoxHdr_OnCheckBoxClicked;
      dataGridViewRip.Columns[0].HeaderCell = chkBoxHdr;

      // Add a dummy column and set the property of the last column to fill
      DataGridViewColumn col = new DataGridViewTextBoxColumn();
      col.Name = "dummy";
      col.HeaderText = "";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 5;
      dataGridViewRip.Columns.Add(col);
      dataGridViewRip.Columns[dataGridViewRip.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    /// <summary>
    ///   Save the settings
    /// </summary>
    private void SaveSettings()
    {
      // Save the Width of the Columns
      int i = 0;
      foreach (DataGridViewColumn column in dataGridViewRip.Columns)
      {
        // Don't save the dummy column
        if (i == dataGridViewRip.Columns.Count - 1)
          break;

        gridColumns.SaveColumnSettings(column, i);
        i++;
      }
      gridColumns.SaveSettings();
    }

    /// <summary>
    ///   Checks or unchecks the checkbox columns
    /// </summary>
    /// <param name = "check"></param>
    private void CheckRows(bool check)
    {
      for (int i = 0; i < dataGridViewRip.Rows.Count - 1; i++)
      {
        dataGridViewRip.Rows[i].Cells[0].Value = check ? 1 : 0;
      }
      /*
        foreach (DataGridViewRow row in dataGridViewRip.Rows)
        {
          row.Cells[0].Value = check ? 1 : 0;
        }
      */
    }

    #endregion

    #endregion

    #region Event Handler

    /// <summary>
    ///   For combo box and check box cells, commit any value change as soon
    ///   as it is made rather than waiting for the focus to leave the cell.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void dataGridViewRip_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!dataGridViewRip.CurrentCell.OwningColumn.GetType().Equals(typeof (DataGridViewTextBoxColumn)))
      {
        dataGridViewRip.CommitEdit(DataGridViewDataErrorContexts.Commit);
      }
    }

    /// <summary>
    ///   The Checkbox Header has been checked. Set the state of all rows
    /// </summary>
    /// <param name = "state"></param>
    private void chkBoxHdr_OnCheckBoxClicked(bool state)
    {
      CheckRows(state);
    }

    private void mediaChangeMonitor_MediaRemoved(string eDriveLetter)
    {
      if (dataGridViewRip.InvokeRequired)
      {
        ThreadSafeMediaRemovedDelegate d = mediaChangeMonitor_MediaRemoved;
        dataGridViewRip.Invoke(d, new object[] {eDriveLetter});
        return;
      }

      _main.RipButtonsEnabled = false;

      // Clear the Header fields
      tbAlbumArtist.Text = "";
      tbAlbum.Text = "";
      tbGenre.Text = "";
      tbYear.Text = "";

      // Clear the Bindinglist
      string driveLetter = eDriveLetter.Substring(0, 1);
      int driveID = Util.Drive2BassID(Convert.ToChar(driveLetter));
      if (driveID > -1)
        bindingList[driveID].Clear();

      (dataGridViewRip.Columns[0].HeaderCell as DatagridViewCheckBoxHeaderCell).Checked = false;

      SetStatusLabel("");
    }

    private void mediaChangeMonitor_MediaInserted(string eDriveLetter)
    {
      if (dataGridViewRip.InvokeRequired)
      {
        ThreadSafeMediaInsertedDelegate d = mediaChangeMonitor_MediaInserted;
        dataGridViewRip.Invoke(d, new object[] {eDriveLetter});
        return;
      }

      // Query FreeDB for the inserted CD
      string driveLetter = eDriveLetter.Substring(0, 1);
      int driveID = Util.Drive2BassID(Convert.ToChar(driveLetter));
      if (driveID > -1)
        bindingList[driveID].Clear();

      SelectedCDRomDrive = driveLetter;

      _main.RipButtonsEnabled = true;
    }

    /// <summary>
    ///   Handle Key input on the Grid
    /// </summary>
    /// <param name = "msg"></param>
    /// <param name = "keyData"></param>
    /// <returns></returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      switch (keyData)
      {
        case Keys.Control | Keys.A:
          dataGridViewRip.SelectAll();
          return true;
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    ///   Handle Messages from the Audio Encoder
    /// </summary>
    /// <param name = "message"></param>
    private void OnMessageReceiveEncoding(QueueMessage message)
    {
      if (_currentRow < 0)
        return;

      double percentComplete = (double)message.MessageData["progress"];
      dataGridViewRip.Rows[_currentRow].Cells[1].Value = (int)percentComplete;
      dataGridViewRip.Update();
      Application.DoEvents();
    }

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
            dataGridViewRip.BackgroundColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            break;
          }
      }
    }

    #endregion
  }
}