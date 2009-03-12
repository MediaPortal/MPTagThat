using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using TagLib;
using MPTagThat.GridView;
using MPTagThat.Core;
using MPTagThat.Core.MediaChangeMonitor;
using MPTagThat.Core.Burning;
using MPTagThat.Dialogues;
using MPTagThat.TagEdit;
using MPTagThat.FileNameToTag;

using Un4seen.Bass;

namespace MPTagThat
{
  public partial class Main : Form
  {
    #region Variables
    private RibbonBar ribbon;
    private bool _keyHandled = false;
    private bool _showForm = false;
    private object _dialog = null;
    private bool _rightPanelCollapsed = false;
    private bool _folderScanInProgress = false;
    private bool _treeViewFolderSelected = false;

    private TreeViewControl treeViewControl;

    // Grids: Can't have them in Designer, as it will fail loading
    private MPTagThat.GridView.GridViewTracks gridViewControl;
    private MPTagThat.GridView.GridViewBurn gridViewBurn;
    private MPTagThat.GridView.GridViewRip gridViewRip;
    private MPTagThat.GridView.GridViewConvert gridViewConvert;

    // Dialogues
    private Progress dlgScan;

    private string _selectedDirectory;          // The currently selcted Directory
    private bool _treeViewSelected = false;     // Has the user selected the Treeview
    private Point _formLocation;
    private Size _formSize;

    private TreeNode _oldSelectNode;            // Store selected node, for context menu

    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    private IThemeManager themeManager = ServiceScope.Get<IThemeManager>();

    private SplashScreen _splashScreen;
    #endregion

    #region Constructor
    public Main()
    {
      InitializeComponent();
    }
    #endregion

    #region Properties
    /// <summary>
    /// Returns the Gridview containing the Tracks
    /// </summary>
    public GridViewTracks TracksGridView
    {
      get { return this.gridViewControl; }
    }

    /// <summary>
    /// Returns the Burning Gridview
    /// </summary>
    public GridViewBurn BurnGridView
    {
      get { return this.gridViewBurn; }
    }

    /// <summary>
    /// Returns the Rip Gridview
    /// </summary>
    public GridViewRip RipGridView
    {
      get { return this.gridViewRip; }
    }

    /// <summary>
    /// Returns the Convert Gridview
    /// </summary>
    public GridViewConvert ConvertGridView
    {
      get { return this.gridViewConvert; }
    }


    /// <summary>
    /// Returns the Error Gridview
    /// </summary>
    public DataGridView ErrorGridView
    {
      get { return this.dataGridViewError; }
    }

    /// <summary>
    /// Is Burning Active?
    /// </summary>
    public bool Burning
    {
      get { return gridViewBurn.Burning; }
    }

    /// <summary>
    /// Is Ripping active?
    /// </summary>
    public bool Ripping
    {
      get { return gridViewRip.Ripping; }
    }

    /// <summary>
    /// Returns the Right Splitter
    /// </summary>
    public NJFLib.Controls.CollapsibleSplitter SplitterRight
    {
      get { return splitterRight; }
    }

    /// <summary>
    /// Returns the Status of the Right Splitter
    /// </summary>
    public bool RightSplitterStatus
    {
      get { return _rightPanelCollapsed; }
    }

    /// <summary>
    /// return the MAIN Ribbon
    /// </summary>
    public RibbonBar MainRibbon
    {
      get { return ribbon; }
    }

    /// <summary>
    /// Gets / Sets the Current Selected Directory in the Treeview
    /// </summary>
    public string CurrentDirectory
    {
      get { return _selectedDirectory; }
      set { _selectedDirectory = value; }
    }

    /// <summary>
    /// Returns the Player Control
    /// </summary>
    public MPTagThat.Player.PlayerControl Player
    {
      get { return playerControl; }
    }

    public bool TreeViewSelected
    {
      get { return _treeViewFolderSelected; }
      set { _treeViewFolderSelected = value; }

    }

    public bool FolderScanning
    {
      get { return _folderScanInProgress; }
    }

    public TreeViewControl TreeView
    {
      get { return treeViewControl; }
    }
    #endregion

    #region Form Open / Close
    /// <summary>
    /// The form is loaded. Do some init work.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Main_Load(object sender, EventArgs e)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      _splashScreen = new SplashScreen();
      _splashScreen.Run();
      _splashScreen.SetInformation("Starting up ...");
      log.Debug("Main: Loading Main form");

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      /// Add the Ribbon Control to the Top Panel
      _splashScreen.SetInformation("Adding Ribbon ...");
      ribbon = new RibbonBar(this);
      ribbon.Dock = DockStyle.Fill;
      this.panelTop.Controls.Add(ribbon);

      #region Setup Grids
      log.Debug("Main: Setup Grid");
      _splashScreen.SetInformation("Setting up Grids ...");
      // Add the Grids to the Main Form
      gridViewControl = new MPTagThat.GridView.GridViewTracks();
      gridViewBurn = new MPTagThat.GridView.GridViewBurn(this);
      gridViewRip = new MPTagThat.GridView.GridViewRip(this);
      gridViewConvert = new GridViewConvert(this);

      // 
      // gridViewControl
      // 
      this.gridViewControl.AutoScroll = true;
      this.gridViewControl.Changed = false;
      this.gridViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridViewControl.Location = new System.Drawing.Point(0, 0);
      this.gridViewControl.Name = "gridViewControl";
      this.gridViewControl.Size = new System.Drawing.Size(676, 470);
      this.gridViewControl.TabIndex = 8;
      // 
      // gridViewBurn
      // 
      this.gridViewBurn.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridViewBurn.Location = new System.Drawing.Point(0, 0);
      this.gridViewBurn.Name = "gridViewBurn";
      this.gridViewBurn.Size = new System.Drawing.Size(676, 470);
      this.gridViewBurn.TabIndex = 9;
      this.gridViewBurn.Visible = false;
      //
      // gridViewRip
      // 
      this.gridViewRip.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridViewRip.Location = new System.Drawing.Point(0, 0);
      this.gridViewRip.Name = "gridViewRip";
      this.gridViewRip.Size = new System.Drawing.Size(676, 470);
      this.gridViewRip.TabIndex = 9;
      this.gridViewRip.Visible = false;
      //
      // gridViewConvert
      // 
      this.gridViewConvert.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridViewConvert.Location = new System.Drawing.Point(0, 0);
      this.gridViewConvert.Name = "gridViewConvert";
      this.gridViewConvert.Size = new System.Drawing.Size(676, 470);
      this.gridViewConvert.TabIndex = 9;
      this.gridViewConvert.Visible = false;

      this.panelFileList.Controls.Add(this.gridViewControl);
      this.panelFileList.Controls.Add(this.gridViewBurn);
      this.panelFileList.Controls.Add(this.gridViewRip);
      this.panelFileList.Controls.Add(this.gridViewConvert);

      // Set reference to Main, so that we may use the ErrorGrid
      gridViewControl.SetMainRef(this);
      #endregion

      // Setup Treeview
      treeViewControl = new TreeViewControl(this);
      treeViewControl.Dock = DockStyle.Fill;
      this.panelLeftTop.Controls.Add(treeViewControl);

      // Start Listening for Media Changes
      ServiceScope.Get<IMediaChangeMonitor>().StartListening(this.Handle);

      // Load BASS
      log.Debug("Main: Loading Bass");
      _splashScreen.SetInformation("Loading Bass ...");
      LoadBass();

      // Load the Settings
      log.Debug("Main: Loading Settings");
      _splashScreen.SetInformation("Loading Settings ...");
      LoadSettings();

      // Localise the Screens
      log.Debug("Main: Localisation");
      _splashScreen.SetInformation("Localisation ...");
      LocaliseScreen();

      // Populate the Treeview with the directories found
      treeViewControl.Init();
      treeViewControl.TreeView.Populate();
      treeViewControl.TreeView.ShowFolder(_selectedDirectory);

      // Setup File Info Listeview
      SetupFileInfo();

      // Build the Context Menu for the Error Grid
      MenuItem[] rmitems = new MenuItem[1];
      rmitems[0] = new MenuItem();
      rmitems[0].Text = "Clear List";
      rmitems[0].Click += new System.EventHandler(dataGridViewError_ClearList);
      rmitems[0].DefaultItem = true;
      this.dataGridViewError.ContextMenu = new ContextMenu(rmitems);

      _splashScreen.Stop();

      // Display the files in the last selected Directory
      if (_selectedDirectory != String.Empty)
        FolderScan();

      // setup various Event Handler needed
      gridViewControl.View.SelectionChanged += new EventHandler(DataGridView_SelectionChanged);

      themeManager.ChangeTheme(Options.Themes[Options.MainSettings.Theme]);
      ribbon.MainRibbon.ThemeName = Options.Themes[Options.MainSettings.Theme];

      // Activate the form, will be hidden because of the size change
      this.Activate();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// The form gets closed. Do cleanup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Main_Close(object sender, FormClosingEventArgs e)
    {
      log.Debug("Main: Closing Main form");
      ServiceScope.Get<IMediaChangeMonitor>().StopListening();
      CheckForChanges();
      Options.MainSettings.LastFolderUsed = _selectedDirectory;
      Options.MainSettings.ScanSubFolders = treeViewControl.ScanFolderRecursive;
      Options.MainSettings.FormLocation = this.Location;
      Options.MainSettings.FormSize = this.ClientSize;
      Options.MainSettings.LeftPanelSize = this.panelLeft.Width;
      Options.MainSettings.RightPanelSize = this.panelRight.Width;
      Options.MainSettings.RightPanelCollapsed = _rightPanelCollapsed;
      Options.MainSettings.ErrorPanelCollapsed = this.splitterBottom.IsCollapsed;
      Options.MainSettings.ActiveScript = ribbon.ScriptsCombo.Text;
      Options.SaveAllSettings();
    }
    #endregion

    #region BASS
    private void LoadBass()
    {
      System.Threading.ThreadStart ts = new System.Threading.ThreadStart(LoadBassAsync);
      System.Threading.Thread BassAsyncLoadThread = new System.Threading.Thread(ts);
      BassAsyncLoadThread.Name = "BassAudio";
      BassAsyncLoadThread.Start();
    }

    private void LoadBassAsync()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero, null))
      {
        int error = (int)Bass.BASS_ErrorGetCode();
        log.Error("Error Init Bass: {0}", Enum.GetName(typeof(BASSError), error));
        return;
      }

      log.Debug("BASS: Loading audio decoder add-ins...");

      string appPath = System.Windows.Forms.Application.StartupPath;
      string decoderFolderPath = Path.Combine(appPath, @"bin\Bass");

      if (!Directory.Exists(decoderFolderPath))
      {
        log.Error(@"BASS: Unable to find decoders folder.");
        return;
      }

      DirectoryInfo dirInfo = new DirectoryInfo(decoderFolderPath);
      FileInfo[] decoders = dirInfo.GetFiles();

      int pluginHandle = 0;

      foreach (FileInfo file in decoders)
      {
        if (Path.GetExtension(file.Name).ToLower() != ".dll")
          continue;

        pluginHandle = Bass.BASS_PluginLoad(file.FullName);
      }
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Localisation
    /// <summary>
    /// Language Change event has been fired. Apply the new language
    /// </summary>
    /// <param name="language"></param>
    private void LanguageChanged()
    {
      LocaliseScreen();
    }

    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Text = localisation.ToString("system", "ApplicationName");

      // Extended Panels. Doing it via TTExtendedPanel doesn't work for some reason
      this.picturePanel.CaptionText = localisation.ToString("main", "PicturePanel");
      this.fileInfoPanel.CaptionText = localisation.ToString("main", "InformationPanel");
      this.dataGridViewError.Columns[0].HeaderText = localisation.ToString("main", "ErrorHeaderFile");
      this.dataGridViewError.Columns[1].HeaderText = localisation.ToString("main", "ErrorHeaderMessage");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Settings / Layout
    /// <summary>
    /// Load the Settings
    /// </summary>
    private void LoadSettings()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      _selectedDirectory = Options.MainSettings.LastFolderUsed;
      treeViewControl.ScanFolderRecursive = Options.MainSettings.ScanSubFolders;
      _formLocation = Options.MainSettings.FormLocation;
      _formSize = Options.MainSettings.FormSize;


      this.Location = _formLocation;

      if (_formSize.Width > 0)
        this.Size = _formSize;

      if (Options.MainSettings.LeftPanelSize > -1)
        this.panelLeft.Width = Options.MainSettings.LeftPanelSize;

      if (Options.MainSettings.ErrorPanelCollapsed)
        splitterBottom.ToggleState();

      if (Options.MainSettings.RightPanelSize > -1)
        this.panelRight.Width = Options.MainSettings.RightPanelSize;

      _rightPanelCollapsed = Options.MainSettings.RightPanelCollapsed;
      if (Options.MainSettings.RightPanelCollapsed)
        splitterRight.ToggleState();

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Set the Color of the Elements, based on the selected Theme Background Color
    /// </summary>
    private void SetRibbonColorBase()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      statusStrip.BackColor = themeManager.CurrentTheme.BackColor;
      statusStrip.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      pictureBoxAlbumArt.BackColor = themeManager.CurrentTheme.BackColor;
      playerPanel.BackColor = themeManager.CurrentTheme.BackColor;
      playerControl.BackColor = themeManager.CurrentTheme.BackColor;
      listViewFileInfo.BackColor = themeManager.CurrentTheme.BackColor;
      listViewFileInfo.ForeColor = themeManager.CurrentTheme.LabelForeColor;

      // We want to have our own header color
      gridViewControl.View.EnableHeadersVisualStyles = false;
      gridViewControl.View.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      gridViewControl.View.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      gridViewControl.View.DefaultCellStyle.BackColor = themeManager.CurrentTheme.DefaultBackColor;
      gridViewControl.View.DefaultCellStyle.SelectionBackColor = themeManager.CurrentTheme.SelectionBackColor;
      gridViewControl.View.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      gridViewControl.View.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;
      // We want to have our own header color
      gridViewBurn.View.EnableHeadersVisualStyles = false;
      gridViewBurn.View.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      gridViewBurn.View.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      gridViewBurn.BackGroundColor = themeManager.CurrentTheme.BackColor;
      gridViewBurn.View.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      gridViewBurn.View.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;
      // We want to have our own header color
      gridViewRip.View.EnableHeadersVisualStyles = false;
      gridViewRip.View.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      gridViewRip.View.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      gridViewRip.BackGroundColor = themeManager.CurrentTheme.BackColor;
      gridViewRip.View.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      gridViewRip.View.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;
      // We want to have our own header color
      gridViewConvert.View.EnableHeadersVisualStyles = false;
      gridViewConvert.View.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      gridViewConvert.View.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      gridViewConvert.BackGroundColor = themeManager.CurrentTheme.BackColor;
      gridViewConvert.View.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      gridViewConvert.View.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;
      // We want to have our own header color
      dataGridViewError.EnableHeadersVisualStyles = false;
      dataGridViewError.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      dataGridViewError.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Remove the Windows Caption from the form.
    /// We need to do it here, otherwise no name would appear in the Task Bar
    /// </summary>
    protected override CreateParams CreateParams
    {
      get
      {
        int WS_BORDER = 0x00800000; //window with border
        int WS_DLGFRAME = 0x00400000; //window with double border but no title
        int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar 

        System.Windows.Forms.CreateParams showText = base.CreateParams;
        showText.Style = showText.Style & ~WS_CAPTION;
        return showText;
      }
    }
    #endregion

    #region Folder Scanning
    private void FolderScan()
    {
        try
        {
            System.Threading.Thread t = new System.Threading.Thread(ScanFoldersAsync);
            t.Name = "FolderScanner";
            t.IsBackground = true;
            t.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Could not launch thread {0}", ex.Message);
        }
    }

    private void ScanFoldersAsync()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      TagLib.File file = null;
      DirectoryInfo dirInfo = null;
      List<FileInfo> files = new List<FileInfo>();

      if (!Directory.Exists(_selectedDirectory))
        return;

      _folderScanInProgress = true;
      toolStripStatusLabelFolder.Text = _selectedDirectory;

      try // just in case we are lacking sufficent permissions
      {
        dirInfo = new DirectoryInfo(_selectedDirectory);
        log.Debug("Main: Retrieving files from. {0}", _selectedDirectory);
        GetFiles(_selectedDirectory, ref files, treeViewControl.ScanFolderRecursive);
      }
      catch (Exception)
      {
      }

      dlgScan = new Progress();
      dlgScan.Header = localisation.ToString("progress", "ScanningHeader");
      int x = ClientSize.Width / 2 - dlgScan.Width / 2;
      int y = ClientSize.Height / 2 - dlgScan.Height / 2;
      Point clientLocation = this.Location;
      x += clientLocation.X;
      y += clientLocation.Y;
      dlgScan.Location = new Point(x, y);
      dlgScan.Show();

      // The Folder scan should stay on Top
      dlgScan.TopMost = true;

      string dlgMessage = localisation.ToString("progress", "Scanning");

      int count = 1;
      int trackCount = files.Count;
      log.Debug("Main: Found {0} files", trackCount);
      foreach (FileInfo fi in files)
      {
        Application.DoEvents();
        dlgScan.UpdateProgress(ProgressBarStyle.Blocks, string.Format(dlgMessage, count, trackCount), count, trackCount, true);
        if (dlgScan.IsCancelled)
        {
          _folderScanInProgress = false;
          dlgScan.Close();
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
              gridViewControl.CreateTracksItem(fi.FullName, file);
            }
            catch (CorruptFileException)
            {
              log.Warn("Main: Ignoring track {0} - Corrupt File!", fi.FullName);
            }
            catch (UnsupportedFormatException)
            {
              log.Warn("Main: Ignoring track {0} - Unsupported format!", fi.FullName);
            }
          }
        }
        catch (PathTooLongException)
        {
          log.Warn("Main: Ignoring track {0} - path too long!", fi.FullName);
          continue;
        }
        count++;
      }

      _folderScanInProgress = false;
      dlgScan.Close();

      // Display Status Information
      try
      {
        toolStripStatusLabelFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"), count, 0);
      }
      catch (InvalidOperationException) { }

      // unselect the first row, which would be selected automatically by the grid
      // And set the background color of the rating cell, as it isn#t reset by the grid
      try
      {
          if (gridViewControl.View.Rows.Count > 0)
          {
              gridViewControl.View.Rows[0].Selected = false;
              gridViewControl.View.Rows[0].Cells[10].Style.BackColor = themeManager.CurrentTheme.DefaultBackColor;
          }
      }
      catch (ArgumentOutOfRangeException) { }

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

    /// <summary>
    /// Shows a Modal Dialogue
    /// </summary>
    /// <param name="dlg"></param>
    public DialogResult ShowForm(object dlg)
    {
      Form f = (Form)dlg;
      int x = (ClientSize.Width / 2) - (f.Width / 2);
      int y = (ClientSize.Height / 2) - (f.Height / 2);
      Point clientLocation = this.Location;
      x += clientLocation.X;
      y += clientLocation.Y;

      f.Location = new Point(x, y);
      return f.ShowDialog();
    }

    /// <summary>
    /// Refreshes the Track List
    /// </summary>
    public void RefreshTrackList()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      CheckForChanges();
      if (_selectedDirectory != String.Empty)
      {
        ClearFileInfoPanel();
        gridViewControl.View.Rows.Clear();
        FolderScan();
        toolStripStatusLabelFolder.Text = _selectedDirectory;
      }
      dataGridViewError.Rows.Clear();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    #region File Info Panel
    /// <summary>
    /// Sets up the File Info Panel
    /// </summary>
    private void SetupFileInfo()
    {
      listViewFileInfo.Columns.Add("", 70);
      listViewFileInfo.Columns.Add("", 200);
    }

    /// <summary>
    /// Fills the File Info Panel
    /// </summary>
    public void FillInfoPanel()
    {
      Image img = null;
      try
      {
        listViewFileInfo.Items.Clear();
        TrackData track = gridViewControl.SelectedTrack;
        // Duration
        TimeSpan ts = track.File.Properties.Duration;
        DateTime dt = new DateTime(ts.Ticks);
        string duration = String.Format("{0:HH:mm:ss.fff}", dt);

        // File Length
        FileInfo fi = new FileInfo(track.File.Name);
        int fileLength = (int)(fi.Length / 1024);

        AddItemToInfoPanel("Duration", duration);
        AddItemToInfoPanel("FileSize", fileLength.ToString());
        AddItemToInfoPanel("Bitrate", track.File.Properties.AudioBitrate.ToString());
        AddItemToInfoPanel("Samplerate", track.File.Properties.AudioSampleRate.ToString());
        AddItemToInfoPanel("Channels", track.File.Properties.AudioChannels.ToString());
        AddItemToInfoPanel("Version", track.File.Properties.Description);
        AddItemToInfoPanel("Created", String.Format("{0:yyyy-MM-dd HH:mm:ss}", fi.CreationTime));
        AddItemToInfoPanel("Changed", String.Format("{0:yyyy-MM-dd HH:mm:ss}", fi.LastWriteTime));

        // Set the Picture
        pictureBoxAlbumArt.Image = null;
        //if (picturePanel.State == Stepi.UI.ExtendedPanelState.Expanded)
        //  picturePanel.Collapse();
        IPicture[] pics = new IPicture[] { };
        pics = track.File.Tag.Pictures;
        btnSaveFolderThumb.Enabled = false;
        if (pics.Length > 0)
        {
          using (MemoryStream ms = new MemoryStream(pics[0].Data.Data))
          {
            img = Image.FromStream(ms);
            if (img != null)
            {
              pictureBoxAlbumArt.Image = img;
              btnSaveFolderThumb.Enabled = true;
            }
          }
        }
      }
      catch (Exception)
      { }
      finally
      {
        //if (img != null)
        //  img.Dispose();
      }
    }

    /// <summary>
    /// Adds an Item to the File Info PAnel
    /// </summary>
    /// <param name="label"></param>
    /// <param name="text"></param>
    private void AddItemToInfoPanel(string label, string text)
    {
      string[] items = new string[2];
      items[0] = localisation.ToString("file_info", label);
      items[1] = text;
      ListViewItem lvi = new ListViewItem(items);
      listViewFileInfo.Items.Add(lvi);
    }

    /// <summary>
    /// Clears the information in the FileInfo Panel
    /// </summary>
    public void ClearFileInfoPanel()
    {
      pictureBoxAlbumArt.Image = null;
      listViewFileInfo.Items.Clear();
    }

    /// <summary>
    /// Save the currently selected picture as folder.jpg
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSaveFolderThumb_Click(object sender, EventArgs e)
    {
      TrackData track = gridViewControl.SelectedTrack;
      TracksGridView.SavePicture(track);
    }
    #endregion

    /// <summary>
    /// Checks for Pending Changes
    /// </summary>
    private void CheckForChanges()
    {
      if (gridViewControl.Changed)
      {
        DialogResult result = MessageBox.Show(localisation.ToString("message", "Save_Changes"), localisation.ToString("message", "Save_Changes_Title"), MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
          gridViewControl.SaveAll();
        else
          gridViewControl.DiscardChanges();
      }
    }

    #region Event Handler
    #region Key Events
    /// <summary>
    /// Handle the OnKeypress, otherwise we get the "Bell", when using shortcuts, with the Treeview active
    /// </summary>
    /// <param name="e"></param>
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      e.Handled = _keyHandled;
      base.OnKeyPress(e);

      // We can show the dialog from the OnKeydown only here. oterwise we hear the annoying bell.
      if (_showForm && _dialog != null)
        ShowForm(_dialog);

      _showForm = false;
      _dialog = null;
    }


    /// <summary>
    /// A Key has been pressed
    /// </summary>
    /// <param name="e"></param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      _keyHandled = false;
      Action newaction = new Action();
      if (ServiceScope.Get<IActionHandler>().GetAction(0, e.KeyData, ref newaction))
      {
        if (OnAction(newaction))
          e.Handled = _keyHandled = true;

        newaction = null;
        return;
      }
      newaction = null;
      base.OnKeyDown(e);
    }

    bool OnAction(Action action)
    {
      if (action == null)
        return false;

      _dialog = null;
      _showForm = false;
      bool handled = true;
      switch (action.ID)
      {
        case Action.ActionType.ACTION_HELP:
          System.Diagnostics.Process.Start(Options.HelpLocation);
          break;

        case Action.ActionType.ACTION_EXIT:
          Application.Exit();
          break;

        case Action.ActionType.ACTION_SAVE:
          gridViewControl.Save();
          break;

        case Action.ActionType.ACTION_MULTI_EDIT:
          if (!gridViewControl.CheckSelections(true))
            break;

          _dialog = new MPTagThat.TagEdit.MultiTagEdit(this);
          _showForm = true;
          break;

        case Action.ActionType.ACTION_EDIT:
          _dialog = new MPTagThat.TagEdit.SingleTagEdit(this);
          _showForm = true;
          break;

        case Action.ActionType.ACTION_FILENAME2TAG:
          if (!gridViewControl.CheckSelections(true))
            break;

          _dialog = new MPTagThat.FileNameToTag.FileNameToTag(this);
          _showForm = true;
          break;

        case Action.ActionType.ACTION_TAG2FILENAME:
          if (!gridViewControl.CheckSelections(true))
            break;

          _dialog = new MPTagThat.TagToFileName.TagToFileName(this);
          _showForm = true;
          break;

        case Action.ActionType.ACTION_IDENTIFYFILE:
          if (!gridViewControl.CheckSelections(true))
            break;

          gridViewControl.IdentifyFiles();
          break;

        case Action.ActionType.ACTION_TAGFROMINTERNET:
          if (!gridViewControl.CheckSelections(true))
            break;

          MPTagThat.InternetLookup.InternetLookup lookup = new MPTagThat.InternetLookup.InternetLookup(this);
          lookup.SearchForAlbumInformation();
          break;

        case Action.ActionType.ACTION_ORGANISE:
          if (!gridViewControl.CheckSelections(true))
            break;

          _dialog = new MPTagThat.Organise.OrganiseFiles(this);
          _showForm = true;
          break;

        case Action.ActionType.ACTION_GETCOVERART:
          if (gridViewControl.CheckSelections(true))
            gridViewControl.GetCoverArt();
          break;

        case Action.ActionType.ACTION_GETLYRICS:
          if (gridViewControl.CheckSelections(true))
            gridViewControl.GetLyrics();
          break;

        case Action.ActionType.ACTION_OPTIONS:
          _dialog = new MPTagThat.Preferences.Preferences(this);
          ShowForm(_dialog);
          break;

        case Action.ActionType.ACTION_SELECTALL:
          Raccoom.Windows.Forms.TreeNodePath node = treeViewControl.TreeView.SelectedNode as Raccoom.Windows.Forms.TreeNodePath;
          if (node != null)
            gridViewControl.View.SelectAll();
          break;

        case Action.ActionType.ACTION_COPY:
          if (gridViewControl.View.CurrentCell != null)
          {
            Clipboard.SetDataObject(gridViewControl.View.CurrentCell.Value);
          }
          break;

        case Action.ActionType.ACTION_SCRIPTEXECUTE:
          ribbon.ribbonButtonScriptExecute_Click(this, new EventArgs());
          break;

        case Action.ActionType.ACTION_TREEREFRESH:
          treeViewControl.RefreshFolders();
          break;

        case Action.ActionType.ACTION_FOLDERDELETE:
          if (!_treeViewSelected)
            break;

          treeViewControl.DeleteFolder();
          break;

        case Action.ActionType.ACTION_CASECONVERSION:
          if (!gridViewControl.CheckSelections(true))
            break;

          _dialog = new MPTagThat.CaseConversion.CaseConversion(this);
          ShowForm(_dialog);
          break;

        case Action.ActionType.ACTION_REFRESH:
          RefreshTrackList();
          break;

        case Action.ActionType.ACTION_DELETE:
          // Don't handle the Delete key, if the user edits directly the cell
          // let the grid control handle it
          if (gridViewControl.View.IsCurrentCellInEditMode)
          {
            handled = false;
            break;
          }

          if (_treeViewFolderSelected)
          {
            MessageBox.Show(localisation.ToString("message", "DeleteFolders"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }

          // When the Tracks grid is not visible, don't handle the delete key
          if (!gridViewControl.Visible)
            break;

          gridViewControl.DeleteTracks();
          break;

        case Action.ActionType.ACTION_TOGGLESPLITTER:
          this.splitterLeft.ToggleState();
          break;

        case Action.ActionType.ACTION_REMOVECOMMENT:
          if (!gridViewControl.CheckSelections(true))
            break;
          gridViewControl.RemoveComments();
          break;

        case Action.ActionType.ACTION_REMOVEPICTURE:
          if (!gridViewControl.CheckSelections(true))
            break;
          gridViewControl.RemovePictures();
          break;
      } 

      return handled;
    }
    #endregion

    #region GridView events
    /// <summary>
    /// A new Row has been selected in the Grid. Fill the Info Panel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DataGridView_SelectionChanged(object sender, EventArgs e)
    {
      if (gridViewControl.View.CurrentRow != null)
        FillInfoPanel();

      // Update Status Information
      try
      {
        toolStripStatusLabelFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"), gridViewControl.View.Rows.Count, gridViewControl.View.SelectedRows.Count);
      }
      catch (InvalidOperationException)  // we might get a Cross-thread Exception on startup
      { }
    }

    /// <summary>
    /// Handle Right Mouse Click to open the context Menu in the Error DataGrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void datagridViewError_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        this.dataGridViewError.ContextMenu.Show(dataGridViewError, new Point(e.X, e.Y));
    }

    /// <summary>
    /// Context Menu entry has been selected
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void dataGridViewError_ClearList(object o, System.EventArgs e)
    {
      dataGridViewError.Rows.Clear();
    }
    #endregion

    #region General Message Handling
    /// <summary>
    /// Handle Messages
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        // Message sent, when a Theme is changing
        case "themechanged":
          {
            SetRibbonColorBase();
            break;
          }

        case "languagechanged":
          {
            LanguageChanged();
            this.Refresh();
            break;
          }
      }
    }
    #endregion

    #region Splitter Events
    private void splitterRight_Click(object sender, EventArgs e)
    {
      _rightPanelCollapsed = splitterRight.IsCollapsed;
    }
    #endregion

    #region Form Resize / Move events
    /// <summary>
    /// The Form is resized, if Playlist is docked, move it as well
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Main_Resize(object sender, EventArgs e)
    {
      playerControl.MovePlayList();
    }

    /// <summary>
    /// The Form is resized, if Playlist is docked, move it as well
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Main_Move(object sender, EventArgs e)
    {
      playerControl.MovePlayList();
    }
    #endregion
    #endregion
  }
}