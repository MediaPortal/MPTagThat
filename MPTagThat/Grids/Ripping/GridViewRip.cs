using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MPTagThat.Core;
using MPTagThat.Core.AudioEncoder;
using MPTagThat.Core.Freedb;
using MPTagThat.Core.MediaChangeMonitor;

using Un4seen.Bass;
using Un4seen.Bass.AddOn.Cd;

using TagLib;

namespace MPTagThat.GridView
{
  public partial class GridViewRip : UserControl
  {
    #region Variables
    private delegate void ThreadSafeRippingStatusDelegate(string text);
    private delegate void ThreadSafeMediaInsertedDelegate(string DriveLetter);
    private delegate void ThreadSafeMediaRemovedDelegate(string DriveLetter);

    private Main _main;
    private Thread threadRip = null;
    private IAudioEncoder audioEncoder;

    private List<SortableBindingList<CDTrackDetail>> bindingList = new List<SortableBindingList<CDTrackDetail>>();
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log;
    private IMediaChangeMonitor mediaChangeMonitor;

    private GridViewColumnsRip gridColumns;
    private string _selectedCDRomDrive = "";

    private string _musicDir;
    private string _outFile;

    private int _currentRow = -1;
    #endregion

    #region Properties
    public Color BackGroundColor
    {
      set
      {
        this.BackColor = value;
        this.panelTop.BackColor = value;
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
            QueryFreeDB(Convert.ToChar(_selectedCDRomDrive));
            dataGridViewRip.DataSource = bindingList[CurrentDriveID];
            CheckRows(false);
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
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      InitializeComponent();

      dataGridViewRip.CurrentCellDirtyStateChanged += new EventHandler(dataGridViewRip_CurrentCellDirtyStateChanged);

      // Listen to Messages
      IMessageQueue queueMessageEncoding = ServiceScope.Get<IMessageBroker>().GetOrCreate("encoding");
      queueMessageEncoding.OnMessageReceive += new MessageReceivedHandler(OnMessageReceiveEncoding);

      log = ServiceScope.Get<ILogger>();
      audioEncoder = ServiceScope.Get<IAudioEncoder>();
      mediaChangeMonitor = ServiceScope.Get<IMediaChangeMonitor>();
      mediaChangeMonitor.MediaInserted += new MediaInsertedEvent(mediaChangeMonitor_MediaInserted);
      mediaChangeMonitor.MediaRemoved += new MediaRemovedEvent(mediaChangeMonitor_MediaRemoved);

      // Get number of CD Drives found and initialise a Bindinglist for every drove
      int driveCount = BassCd.BASS_CD_GetDriveCount();
      if (driveCount == 0)
        bindingList.Add(new SortableBindingList<CDTrackDetail>());  // In case of no CD, we want a Dummy List

      for (int i = 0; i < driveCount; i++)
      {
        bindingList.Add(new SortableBindingList<CDTrackDetail>());
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
    /// Rips the selected files in the Grid
    /// </summary>
    public void RipAudioCD()
    {
      SetStatusLabel("");

      if (threadRip == null)
      {
        threadRip = new Thread(new ThreadStart(RippingThread));
        threadRip.Name = "Ripping";
      }

      if (threadRip.ThreadState != ThreadState.Running)
      {
        threadRip = new Thread(new ThreadStart(RippingThread));
        threadRip.Start();
      }
    }

    /// <summary>
    /// Cancel the Ripping Process
    /// </summary>
    public void RipAudioCDCancel()
    {
      if (threadRip != null)
      {
        SetStatusLabel("Rip aborted");
        threadRip.Abort();
        _currentRow = -1;
      }
    }
    #endregion

    #region Private Methods
    private void SetStatusLabel(string text)
    {
      if (lbRippingStatus.InvokeRequired)
      {
        ThreadSafeRippingStatusDelegate d = new ThreadSafeRippingStatusDelegate(SetStatusLabel);
        lbRippingStatus.Invoke(d, new object[] { text });
      }
      lbRippingStatus.Text = text;
    }

    #region Localisation
    /// <summary>
    /// Language Change event has been fired. Apply the new language
    /// </summary>
    /// <param name="language"></param>
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
      Util.EnterMethod(Util.GetCallingMethod());
      try
      {
        _musicDir = _main.MainRibbon.RipOutputDirectory;

        string encoder = null;
        List<Item> encoders = (List<Item>)_main.MainRibbon.RipEncoderCombo.DataSource;
        if (_main.MainRibbon.RipEncoderCombo.SelectedItem != null)
        {
          encoder = encoders[_main.MainRibbon.RipEncoderCombo.SelectedIndex].Value;
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
          if (!System.IO.Directory.Exists(_musicDir) && !string.IsNullOrEmpty(_musicDir))
            System.IO.Directory.CreateDirectory(_musicDir);
        }
        catch (Exception ex)
        {
          SetStatusLabel(localisation.ToString("Conversion", "ErrorDirectory"));
          log.Error("Error creating Ripping directory: {0}. {1}", _musicDir, ex.Message);
          return;
        }

        // Build the Target Directory
        string targetDir = "";
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
          targetDir = string.Format(@"{1}\{2}", artistDir, albumDir);

        targetDir = string.Format(@"{0}\{1}", _musicDir, targetDir);

        log.Debug("Rip: Using Target Folder: {0}", targetDir);

        try
        {
          if (!System.IO.Directory.Exists(targetDir))
            System.IO.Directory.CreateDirectory(targetDir);
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

        foreach (DataGridViewRow row in dataGridViewRip.Rows)
        {
          // Reset the Status field to 0
          row.Cells[1].Value = 0;
        }

        _currentRow = -1;
        foreach (DataGridViewRow row in dataGridViewRip.Rows)
        {
          // when checking and unchecking a row, we have the DBNull value
          if (row.Cells[0].Value == System.DBNull.Value)
            continue;

          if ((int)row.Cells[0].Value == 0)
            continue;

          SetStatusLabel(localisation.ToString("Conversion", "Ripping"));

          _currentRow = row.Index;

          CDTrackDetail track = bindingList[selectedDriveID][_currentRow];

          int stream = BassCd.BASS_CD_StreamCreate(selectedDriveID, row.Index, BASSFlag.BASS_STREAM_DECODE);
          if (stream == 0)
          {
            log.Error("Error creating stream for Audio Track {0}. Error: {1}", _currentRow, Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode()));
            continue;
          }

          log.Info("Decoding Audio CD Track{0}", row.Index);
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
            log.Error("Error starting Encoder for Audio Track {0}. Error: {1}", _currentRow, Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode()));
          }

          dataGridViewRip.Rows[_currentRow].Cells[1].Value = 100;

          Bass.BASS_StreamFree(stream);
          log.Info("Finished Decoding Audio CD Track{0}", _currentRow);

          try
          {
            // Now Tag the encoded File
            File file = File.Create(_outFile);
            file.Tag.AlbumArtists = new string[] { tbAlbumArtist.Text };
            file.Tag.Album = tbAlbum.Text;
            file.Tag.Genres = new string[] { tbGenre.Text };
            file.Tag.Year = tbYear.Text == string.Empty ? 0 : Convert.ToUInt32(tbYear.Text);
            file.Tag.Performers = new string[] { track.Artist };
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
      SetStatusLabel(localisation.ToString("Conversion","RippingFinished"));
      Options.MainSettings.RipTargetFolder = _musicDir;

      Util.LeaveMethod(Util.GetCallingMethod());

    }
    #endregion

    #region FreedDB
    /// <summary>
    /// Queries FreeDB for the Audio CD inserted
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="items"></param>
    private void QueryFreeDB(char drive)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      SetStatusLabel(localisation.ToString("Conversion","FreeDBAccess"));
      string discId = string.Empty;
      CDInfoDetail MusicCD = new CDInfoDetail();
      int driveID = Util.Drive2BassID(drive);
      if (driveID < 0)
      {
        SetStatusLabel("");
        return;
      }
      bindingList[driveID].Clear();
      try
      {
        FreeDBQuery freedb = new FreeDBQuery();
        freedb.Connect();
        CDInfo[] cds = freedb.GetDiscInfo(drive);
        if (cds != null)
        {
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
          MusicCD = null;

        freedb.Disconnect();
      }
      catch (Exception)
      {
        MusicCD = null;
      }

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

      Util.LeaveMethod(Util.GetCallingMethod());

    }
    #endregion

    #region Gridlayout
    /// <summary>
    /// Create the Columns of the Grid based on the users setting
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
      chkBoxHdr.OnCheckBoxClicked += new CheckBoxClickedHandler(chkBoxHdr_OnCheckBoxClicked);
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
    /// Save the settings
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
    /// Checks or unchecks the checkbox columns
    /// </summary>
    /// <param name="check"></param>
    private void CheckRows(bool check)
    {
      foreach (DataGridViewRow row in dataGridViewRip.Rows)
      {
        row.Cells[0].Value = check == true ? 1 : 0;
      }
    }
    #endregion
    #endregion

    #region Event Handler
    /// <summary>
    /// For combo box and check box cells, commit any value change as soon
    /// as it is made rather than waiting for the focus to leave the cell.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void dataGridViewRip_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!dataGridViewRip.CurrentCell.OwningColumn.GetType().Equals(typeof(DataGridViewTextBoxColumn)))
      {
        dataGridViewRip.CommitEdit(DataGridViewDataErrorContexts.Commit);

      }
    }
    /// <summary>
    /// The Checkbox Header has been checked. Set the state of all rows
    /// </summary>
    /// <param name="state"></param>
    void chkBoxHdr_OnCheckBoxClicked(bool state)
    {
      CheckRows(state);
    }

    void mediaChangeMonitor_MediaRemoved(string eDriveLetter)
    {
      if (dataGridViewRip.InvokeRequired)
      {
        ThreadSafeMediaRemovedDelegate d = new ThreadSafeMediaRemovedDelegate(mediaChangeMonitor_MediaRemoved);
        dataGridViewRip.Invoke(d, new object[] { eDriveLetter });
        return;
      }

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

    void mediaChangeMonitor_MediaInserted(string eDriveLetter)
    {
      if (dataGridViewRip.InvokeRequired)
      {
        ThreadSafeMediaInsertedDelegate d = new ThreadSafeMediaInsertedDelegate(mediaChangeMonitor_MediaInserted);
        dataGridViewRip.Invoke(d, new object[] { eDriveLetter });
        return;
      }

      // Query FreeDB for the inserted CD
      string driveLetter = eDriveLetter.Substring(0, 1);
      int driveID = Util.Drive2BassID(Convert.ToChar(driveLetter));
      if (driveID > -1)
        bindingList[driveID].Clear();

      SelectedCDRomDrive = driveLetter;
    }

    /// <summary>
    /// Handle Key input on the Grid
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="keyData"></param>
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
    /// Handle Messages from the Audio Encoder
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceiveEncoding(QueueMessage message)
    {
      if (_currentRow < 0)
        return;

      double percentComplete = (double)message.MessageData["progress"];
      dataGridViewRip.Rows[_currentRow].Cells[1].Value = (int)percentComplete;
    }

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
    #endregion
  }
}
