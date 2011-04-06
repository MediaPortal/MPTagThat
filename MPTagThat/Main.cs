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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Elegant.Ui;
using MPTagThat.Core;
using MPTagThat.Core.MediaChangeMonitor;
using MPTagThat.Dialogues;
using MPTagThat.GridView;
using MPTagThat.Organise;
using MPTagThat.Player;
using MPTagThat.TagEdit;
using NJFLib.Controls;
using Raccoom.Windows.Forms;
using Un4seen.Bass;
using Label = Elegant.Ui.Label;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;
using Action = MPTagThat.Core.Action;

#endregion

namespace MPTagThat
{
  public partial class Main : Form
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly IThemeManager themeManager = ServiceScope.Get<IThemeManager>();
    private object _dialog;
    private Point _formLocation;
    private Size _formSize;
    private bool _keyHandled;
    private MusicDatabaseBuild _musicDatabaseBuild;
    private bool _rightPanelCollapsed;
    private bool _bottomPanelCollapsed;
    private string _selectedDirectory = ""; // The currently selcted Directory
    private bool _showForm;
    private SplashScreen _splashScreen;
    private bool _treeViewFolderSelected;
    private bool _treeViewSelected; // Has the user selected the Treeview

    private DatabaseSearchControl databaseSearchControl;

    // Grids: Can't have them in Designer, as it will fail loading
    private GridViewBurn gridViewBurn;
    private GridViewTracks gridViewControl;
    private GridViewConvert gridViewConvert;
    private GridViewRip gridViewRip;
    private MiscInfoControl miscInfoControl;
    private PlayerControl playerControl;
    private RibbonControl ribbonControl;
    private TreeViewControl treeViewControl;
    private TagEditControl tagEditControl;

    private delegate void ThreadSafeFolderScan();

    #endregion

    #region Constructor

    public Main()
    {
      SkinManager.LoadEmbeddedTheme(EmbeddedTheme.Office2007Silver, Product.Common);
      SkinManager.LoadEmbeddedTheme(EmbeddedTheme.Office2007Silver, Product.Ribbon);
      InitializeComponent();
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Returns the Gridview containing the Tracks
    /// </summary>
    public GridViewTracks TracksGridView
    {
      get { return gridViewControl; }
    }

    /// <summary>
    ///   Returns the Burning Gridview
    /// </summary>
    public GridViewBurn BurnGridView
    {
      get { return gridViewBurn; }
    }

    /// <summary>
    ///   Returns the Rip Gridview
    /// </summary>
    public GridViewRip RipGridView
    {
      get { return gridViewRip; }
    }

    /// <summary>
    ///   Returns the Convert Gridview
    /// </summary>
    public GridViewConvert ConvertGridView
    {
      get { return gridViewConvert; }
    }

    /// <summary>
    ///   Is Burning Active?
    /// </summary>
    public bool Burning
    {
      get
      {
        if (gridViewBurn == null)
        {
          return false;
        }
        return gridViewBurn.Burning;
      }
    }

    /// <summary>
    ///   Is Ripping active?
    /// </summary>
    public bool Ripping
    {
      get
      {
        if (gridViewRip == null)
        {
          return false;
        }
        return gridViewRip.Ripping;
      }
    }

    /// <summary>
    ///   Returns the Top Splitter
    /// </summary>
    public CollapsibleSplitter SplitterTop
    {
      get { return splitterTop; }
    }

    /// <summary>
    ///   Returns the Right Splitter
    /// </summary>
    public CollapsibleSplitter SplitterRight
    {
      get { return splitterRight; }
    }

    /// <summary>
    ///   Returns the Status of the Right Splitter
    /// </summary>
    public bool RightSplitterStatus
    {
      get { return _rightPanelCollapsed; }
    }

    /// <summary>
    ///   return the MAIN Ribbon
    /// </summary>
    public RibbonControl MainRibbon
    {
      get { return ribbonControl; }
    }

    /// <summary>
    ///   Gets / Sets the Current Selected Directory in the Treeview
    /// </summary>
    public string CurrentDirectory
    {
      get { return _selectedDirectory; }
      set { _selectedDirectory = value; }
    }

    /// <summary>
    ///   Returns the Player Control
    /// </summary>
    public PlayerControl Player
    {
      get { return playerControl; }
    }

    public bool TreeViewSelected
    {
      get { return _treeViewFolderSelected; }
      set { _treeViewFolderSelected = value; }
    }

    public bool FolderScanning { get; set; }

    public TreeViewControl TreeView
    {
      get { return treeViewControl; }
    }

    public Label ToolStripStatusFiles
    {
      get { return toolStripStatusLabelFiles; }
    }

    public Label ToolStripStatusFilter
    {
      get { return toolStripStatusLabelFilter; }
    }

    public Label ToolStripStatusScan
    {
      get { return toolStripStatusLabelScanProgress; }
    }

    public MiscInfoControl MiscInfoPanel
    {
      get { return miscInfoControl; }
    }

    public TagEditControl TagEditForm
    {
      get { return tagEditControl; }
    }

    #endregion

    #region Form Open / Close

    /// <summary>
    ///   The form is loaded. Do some init work.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Main_Load(object sender, EventArgs e)
    {
      log.Trace(">>>");

      //FindRibbonWin();

      _splashScreen = new SplashScreen();
      _splashScreen.Run();
      _splashScreen.SetInformation(localisation.ToString("splash", "Startup"));
      log.Info("Main: Loading Main form");

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      /// Add the Ribbon Control to the Top Panel
      _splashScreen.SetInformation(localisation.ToString("splash", "AddRibbon"));

      ribbonControl = new RibbonControl(this);
      ribbonControl.Dock = DockStyle.Fill;
      ribbonControl.ribbon.CustomTitleBarEnabled = true; // Indicating we have the ribbon placed in a user container
      panelTop.AutoSize = true;
      panelTop.Controls.Add(ribbonControl);

      ribbonControl.Initialising = true;

      #region Setup Grids

      log.Debug("Main: Setup Grid");
      _splashScreen.SetInformation(localisation.ToString("splash", "AddGrids"));
      // Add the Grids to the Main Form
      gridViewControl = new GridViewTracks();
      gridViewBurn = new GridViewBurn(this);
      gridViewRip = new GridViewRip(this);
      gridViewConvert = new GridViewConvert(this);
      playerControl = new PlayerControl();

      // 
      // gridViewControl
      // 
      gridViewControl.AutoScroll = true;
      gridViewControl.Changed = false;
      gridViewControl.Dock = DockStyle.Fill;
      gridViewControl.Location = new Point(0, 0);
      gridViewControl.Name = "gridViewControl";
      gridViewControl.Size = new Size(676, 470);
      gridViewControl.TabIndex = 8;
      // 
      // gridViewBurn
      // 
      gridViewBurn.Dock = DockStyle.Fill;
      gridViewBurn.Location = new Point(0, 0);
      gridViewBurn.Name = "gridViewBurn";
      gridViewBurn.Size = new Size(676, 470);
      gridViewBurn.TabIndex = 9;
      gridViewBurn.Visible = false;
      //
      // gridViewRip
      // 
      gridViewRip.Dock = DockStyle.Fill;
      gridViewRip.Location = new Point(0, 0);
      gridViewRip.Name = "gridViewRip";
      gridViewRip.Size = new Size(676, 470);
      gridViewRip.TabIndex = 9;
      gridViewRip.Visible = false;
      //
      // gridViewConvert
      // 
      gridViewConvert.Dock = DockStyle.Fill;
      gridViewConvert.Location = new Point(0, 0);
      gridViewConvert.Name = "gridViewConvert";
      gridViewConvert.Size = new Size(676, 470);
      gridViewConvert.TabIndex = 9;
      gridViewConvert.Visible = false;
      // 
      // playerControl
      // 
      playerControl.Dock = DockStyle.Fill;
      playerControl.Location = new Point(0, 0);
      playerControl.Name = "playerControl";
      playerControl.Size = new Size(1008, 68);
      playerControl.TabIndex = 0;

      // Look where to place the Track LIst Panel
      if (Options.MainSettings.TrackListLocation == 0)
      {
        panelFileList.Controls.Add(gridViewControl);
        panelFileList.Controls.Add(gridViewBurn);
        panelFileList.Controls.Add(gridViewRip);
        panelFileList.Controls.Add(gridViewConvert);
      }
      else
      {
        panelMiddleBottom.Controls.Add(gridViewControl);
        panelMiddleBottom.Controls.Add(gridViewBurn);
        panelMiddleBottom.Controls.Add(gridViewRip);
        panelMiddleBottom.Controls.Add(gridViewConvert);        
      }

      playerPanel.Controls.Add(playerControl);

      // Set reference to Main, so that we may use the ErrorGrid
      gridViewControl.SetMainRef(this);

      #endregion

      // Hide the DB Search Panel
      splitterTop.ToggleState();

      // Setup Treeview
      treeViewControl = new TreeViewControl(this);
      treeViewControl.Dock = DockStyle.Fill;
      panelLeftTop.Controls.Add(treeViewControl);

      // Setup Database Search Control
      databaseSearchControl = new DatabaseSearchControl(this);
      databaseSearchControl.Dock = DockStyle.Fill;
      panelMiddleDBSearch.Controls.Add(databaseSearchControl);

      // Setup Misc Info Control
      miscInfoControl = new MiscInfoControl();
      miscInfoControl.Dock = DockStyle.Fill;
      panelRight.Controls.Add(miscInfoControl);

      // Setup TagEdit Control
      tagEditControl = new TagEditControl(this);
      tagEditControl.Dock = DockStyle.Fill;

      if (Options.MainSettings.TrackListLocation == 0)
      {
        panelMiddleBottom.Controls.Add(tagEditControl);
      }
      else
      {
        panelFileList.Controls.Add(tagEditControl);
      }

      // Start Listening for Media Changes
      ServiceScope.Get<IMediaChangeMonitor>().StartListening(Handle);

      // Load BASS
      LoadBass();

      // Load the Settings
      _splashScreen.SetInformation(localisation.ToString("splash", "LoadSettings"));
      LoadSettings();

      // Localise the Screens
      log.Info("Main: Localisation");
      _splashScreen.SetInformation(localisation.ToString("splash", "Localisation"));
      LocaliseScreen();

      // Populate the Treeview with the directories found
      treeViewControl.Init();
      treeViewControl.TreeView.Populate();
      treeViewControl.TreeView.Nodes[0].Expand();
      treeViewControl.TreeView.ShowFolder(_selectedDirectory);

      _splashScreen.Stop();

      ribbonControl.Theme = Options.Themes[Options.MainSettings.Theme];

      // Display the files in the last selected Directory
      if (_selectedDirectory != String.Empty && !TreeView.DatabaseMode)
      {
        toolStripStatusLabelFolder.Text = _selectedDirectory;

        ThreadStart ts = FolderScanAsync;
        Thread FolderScanAsyncThread = new Thread(ts);
        FolderScanAsyncThread.Name = "FolderScanAsyncThread";
        FolderScanAsyncThread.Start();
      }

      // setup various Event Handler needed
      gridViewControl.View.SelectionChanged += DataGridView_SelectionChanged;

      ribbonControl.Initialising = false;

      // Activate the form, will be hidden because of the size change
      TopMost = true;
      Focus();
      BringToFront();
      TopMost = false;
      log.Info("Finished loading Main Form");
      log.Trace("<<<");
    }

    /// <summary>
    ///   Thread to populate the Treeview async during startup
    /// </summary>
    private void FolderScanAsync()
    {
      if (gridViewControl.InvokeRequired)
      {
        ThreadSafeFolderScan d = FolderScanAsync;
        gridViewControl.Invoke(d);
        return;
      }
      
      gridViewControl.FolderScan();
    }

    /// <summary>
    ///   The form gets closed. Do cleanup
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Main_Close(object sender, FormClosingEventArgs e)
    {
      log.Trace(">>>");
      log.Info("Main: Closing Main form");
      if (_musicDatabaseBuild != null && _musicDatabaseBuild.ScanActive)
      {
        if (
          MessageBox.Show(localisation.ToString("Settings", "DBSCanActive"),
                          localisation.ToString("Settings", "DBScanTitle"), MessageBoxButtons.YesNo,
                          MessageBoxIcon.Question) == DialogResult.No)
        {
          e.Cancel = true;
          return;
        }
        _musicDatabaseBuild.AbortScan = true;
      }
      ServiceScope.Get<IMediaChangeMonitor>().StopListening();
      gridViewControl.CheckForChanges();

      Options.MainSettings.LastFolderUsed = _selectedDirectory;
      Options.MainSettings.ScanSubFolders = treeViewControl.ScanFolderRecursive;
      Options.MainSettings.FormLocation = Location;
      Options.MainSettings.FormSize = ClientSize;
      Options.MainSettings.FormIsMaximized = WindowState == FormWindowState.Maximized ? true : false;
      Options.MainSettings.LeftPanelSize = panelLeft.Width;
      Options.MainSettings.RightPanelSize = panelRight.Width;
      Options.MainSettings.RightPanelCollapsed = _rightPanelCollapsed;
      Options.MainSettings.BottomPanelSize = panelMiddleBottom.Height;
      Options.MainSettings.BottomPanelCollapsed = _bottomPanelCollapsed;
      Options.MainSettings.PlayerPanelCollapsed = splitterPlayer.IsCollapsed;
      Options.MainSettings.ActiveScript = ribbonControl.ScriptsCombo.Text;
      Options.SaveAllSettings();
      log.Info("Main: Finished closing Main form");
      log.Trace("<<<");
    }

    #endregion

    #region Ribbon Win Close

    private const int WM_SYSCOMMAND = 0x0112;
    private const int SC_CLOSE = 0xF060;

    private void FindRibbonWin()
    {
      ThreadStart ts = FindRibbonWinAsync;
      Thread FindRibbonWinThread = new Thread(ts);
      FindRibbonWinThread.Name = "FindRibbonWin";
      FindRibbonWinThread.Start();
    }

    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

    private void FindRibbonWinAsync()
    {
      IntPtr hWnd = FindWindowByCaption(IntPtr.Zero, "Elegant UI");
      while (hWnd == IntPtr.Zero)
      {
        Thread.Sleep(100);
        hWnd = FindWindowByCaption(IntPtr.Zero, "Elegant UI");
      }
      SendMessage((int)hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
    }

    #endregion

    #region BASS

    private void LoadBass()
    {
      ThreadStart ts = LoadBassAsync;
      Thread BassAsyncLoadThread = new Thread(ts);
      BassAsyncLoadThread.Name = "BassAudio";
      BassAsyncLoadThread.Start();
    }

    private void LoadBassAsync()
    {
      log.Trace(">>>");
      log.Debug("Main: Initialising Bass Libraries");
      if (!Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
      {
        int error = (int)Bass.BASS_ErrorGetCode();
        log.Error("Error Init Bass: {0}", Enum.GetName(typeof (BASSError), error));
        return;
      }

      log.Debug("BASS: Loading audio decoder add-ins...");

      string appPath = Application.StartupPath;
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
      log.Debug("Main: Finished Initialising Bass Libraries");
      log.Trace("<<<");
    }

    #endregion

    #region Localisation

    /// <summary>
    ///   Language Change event has been fired. Apply the new language
    /// </summary>
    /// <param name = "language"></param>
    private void LanguageChanged()
    {
      LocaliseScreen();
    }

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      log.Trace(">>>");
      Text = localisation.ToString("system", "ApplicationName");
      log.Trace("<<<");
    }

    #endregion

    #region Settings / Layout

    /// <summary>
    ///   Load the Settings
    /// </summary>
    private void LoadSettings()
    {
      log.Trace(">>>");
      log.Info("Main: Loading Settings");

      // We might have received a folder via startup argument
      if (_selectedDirectory == string.Empty)
      {
        _selectedDirectory = Options.MainSettings.LastFolderUsed;
      }
      treeViewControl.ScanFolderRecursive = Options.MainSettings.ScanSubFolders;
      treeViewControl.DatabaseMode = Options.MainSettings.DataProvider == 3;

      _formLocation = Options.MainSettings.FormLocation;
      // Check, if we are out of screen bounds
      if (_formLocation.X < 0 || _formLocation.Y < 0 ||
          _formLocation.X > Screen.PrimaryScreen.Bounds.Width || _formLocation.Y > Screen.PrimaryScreen.Bounds.Height)
      {
        _formLocation.X = 10;
        _formLocation.Y = 10;
      }
      Location = _formLocation;

      _formSize = Options.MainSettings.FormSize;
      if (_formSize.Width < 0 || _formSize.Height < 0 ||
          _formSize.Width > Screen.PrimaryScreen.Bounds.Width || _formSize.Height > Screen.PrimaryScreen.Bounds.Height)
      {
        _formSize.Width = 1024;
        _formSize.Height = 768;
      }
      Size = _formSize;

      if (Options.MainSettings.FormIsMaximized)
        WindowState = FormWindowState.Maximized;

      if (Options.MainSettings.LeftPanelSize > -1)
        panelLeft.Width = Options.MainSettings.LeftPanelSize;

      if (Options.MainSettings.PlayerPanelCollapsed)
        splitterPlayer.ToggleState();

      if (Options.MainSettings.RightPanelSize > -1)
        panelRight.Width = Options.MainSettings.RightPanelSize;

      _rightPanelCollapsed = Options.MainSettings.RightPanelCollapsed;
      if (Options.MainSettings.RightPanelCollapsed)
        splitterRight.ToggleState();

      if (Options.MainSettings.BottomPanelSize > -1 && Options.MainSettings.BottomPanelSize < 600) 
        panelMiddleBottom.Height = Options.MainSettings.BottomPanelSize;

      _bottomPanelCollapsed = Options.MainSettings.BottomPanelCollapsed;
      if (Options.MainSettings.BottomPanelCollapsed)
        splitterBottom.ToggleState();

      log.Info("Main: Finished Loading Settings");
      log.Trace("<<<");
    }

    /// <summary>
    ///   Set the Color of the Elements, based on the selected Theme Background Color
    /// </summary>
    private void SetRibbonColorBase()
    {
      playerPanel.BackColor = themeManager.CurrentTheme.BackColor;
      playerControl.BackColor = themeManager.CurrentTheme.BackColor;
      databaseSearchControl.BackColor = themeManager.CurrentTheme.BackColor;

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
    }

    #endregion

    #region Misc Public Methods

    /// <summary>
    ///   Shows a Modal Dialogue
    /// </summary>
    /// <param name = "dlg"></param>
    public DialogResult ShowModalDialog(object dlg)
    {
      Form f = (Form)dlg;
      int x = (ClientSize.Width / 2) - (f.Width / 2);
      int y = (ClientSize.Height / 2) - (f.Height / 2);
      Point clientLocation = Location;
      x += clientLocation.X;
      y += clientLocation.Y;

      f.Location = new Point(x, y);
      return f.ShowDialog();
    }

    /// <summary>
    ///   Shows a Form Centered
    /// </summary>
    /// <param name = "form"></param>
    public void ShowCenteredForm(object form)
    {
      Form f = (Form)form;
      int x = (ClientSize.Width / 2) - (f.Width / 2);
      int y = (ClientSize.Height / 2) - (f.Height / 2);
      Point clientLocation = Location;
      x += clientLocation.X;
      y += clientLocation.Y;

      f.Location = new Point(x, y);
      f.Show();
    }

    /// <summary>
    ///   Refreshes the Track List
    /// </summary>
    public void RefreshTrackList()
    {
      log.Trace(">>>");
      gridViewControl.CheckForChanges();
      if (_selectedDirectory != String.Empty)
      {
        tagEditControl.ClearForm();
        ribbonControl.ClearGallery();
        gridViewControl.View.Rows.Clear();
        toolStripStatusLabelFolder.Text = _selectedDirectory;
        if (TreeView.DatabaseMode)
        {
          gridViewControl.DatabaseScan();
        }
        else
        {
          gridViewControl.FolderScan();
        }
      }
      log.Trace("<<<");
    }

    /// <summary>
    ///   Creates a Music Database
    /// </summary>
    public void CreateMusicDatabase(string databaseName)
    {
      if (_musicDatabaseBuild == null)
      {
        _musicDatabaseBuild = new MusicDatabaseBuild();
      }
      _musicDatabaseBuild.CreateMusicDatabase(databaseName);
    }

    /// <summary>
    ///   Starts scanning of the selected folder and fills the Music Database
    /// </summary>
    /// <param name = "folder"></param>
    public void FillMusicDatabase(string folder, string databaseName)
    {
      if (_musicDatabaseBuild == null)
      {
        _musicDatabaseBuild = new MusicDatabaseBuild();
      }
      _musicDatabaseBuild.FillMusicDatabase(folder, databaseName);
    }

    /// <summary>
    ///   Returns the Status of the Database Scan
    /// </summary>
    /// <returns></returns>
    public string DatabaseScanStatus()
    {
      if (_musicDatabaseBuild == null)
      {
        _musicDatabaseBuild = new MusicDatabaseBuild();
      }
      return _musicDatabaseBuild.DatabaseScanStatus();
    }

    /// <summary>
    /// Toogles the display of the detail panel, which we should hide / deactivate when not in TRacks View
    /// </summary>
    /// <param name="show"></param>
    public void ToggleDetailPanel(bool show)
    {
      if (show)
      {
        if (Options.MainSettings.TrackListLocation == 0)
        {
          // Enable Splitter and show the bar
          if (splitterBottom.IsCollapsed)
          {
            splitterBottom.ToggleState();
          }
        }
        else
        {
          panelMiddleTop.Show();
          panelMiddleBottom.Dock = DockStyle.Bottom;
        }
        splitterBottom.Enabled = true;
      }
      else
      {
        if (Options.MainSettings.TrackListLocation == 0)
        {
          // Disable splitter
          if (!splitterBottom.IsCollapsed)
          {
            splitterBottom.ToggleState();
          }
        }
        else
        {
          panelMiddleTop.Hide();
          panelMiddleBottom.Dock = DockStyle.Fill;
        }
        splitterBottom.Enabled = false;
      }
    }

    /// <summary>
    /// Displays / Hides the Detail Panel
    /// </summary>
    /// <param name="show"></param>
    public void ShowTagEditPanel(bool show)
    {
      if (show)
      {
        tagEditControl.Show();
      }
      else
      {
        tagEditControl.Hide();
      }
    }

    /// <summary>
    ///   Shows the Dialogue in the Detail Panel
    /// </summary>
    /// <param name = "dlg"></param>
    public void ShowDialogInDetailPanel(object dlg)
    {
      UserControl control = (UserControl) dlg;
      if (Options.MainSettings.TrackListLocation == 0)
      {
        panelMiddleBottom.Controls.Add(control);
      }
      else
      {
        panelFileList.Controls.Add(control);
      }
      
      ShowTagEditPanel(false);
      control.Show();
    }

    #endregion

    #region Event Handler

    #region Key Events

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      _keyHandled = false;
      Action newaction = new Action();
      if (ServiceScope.Get<IActionHandler>().GetAction(0, keyData, ref newaction))
      {
        if (OnAction(newaction))
        {
          _keyHandled = true;

          if (_showForm && _dialog != null)
            ShowModalDialog(_dialog);

          _showForm = false;
          _dialog = null;
        }
      }
      return _keyHandled;
    }

    private bool OnAction(Action action)
    {
      if (action == null)
        return false;

      _dialog = null;
      _showForm = false;
      bool handled = true;
      switch (action.ID)
      {
        case Action.ActionType.ACTION_HELP:
          Process.Start(Options.HelpLocation);
          break;

        case Action.ActionType.ACTION_EXIT:
          Application.Exit();
          break;

        case Action.ActionType.ACTION_SAVE:
          gridViewControl.Save();
          break;

        case Action.ActionType.ACTION_SAVEALL:
          gridViewControl.SaveAll();
          break;

        case Action.ActionType.ACTION_FILENAME2TAG:
          if (!gridViewControl.CheckSelections(true))
            break;

          FileNameToTag.FileNameToTag dialog = new FileNameToTag.FileNameToTag(this);
          ShowDialogInDetailPanel(dialog);
          _showForm = false;
          break;

        case Action.ActionType.ACTION_TAG2FILENAME:
          if (!gridViewControl.CheckSelections(true))
            break;

          _dialog = new TagToFileName.TagToFileName(this);
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

          InternetLookup.InternetLookup lookup = new InternetLookup.InternetLookup(this);
          lookup.SearchForAlbumInformation();
          break;

        case Action.ActionType.ACTION_ORGANISE:
          if (!gridViewControl.CheckSelections(true))
            break;

          _dialog = new OrganiseFiles(this);
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
          _dialog = new Preferences.Preferences(this);
          ShowModalDialog(_dialog);
          break;

        case Action.ActionType.ACTION_SELECTALL:
          TreeNodePath node = treeViewControl.TreeView.SelectedNode as TreeNodePath;
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
          ApplicationCommands.ScriptExecute.Execute();
          //ribbon.ribbonButtonScriptExecute_Click(this, new EventArgs());
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

          _dialog = new CaseConversion.CaseConversion(this);
          ShowModalDialog(_dialog);
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
            MessageBox.Show(localisation.ToString("message", "DeleteFolders"), "", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
            break;
          }

          // Don't handle the delete key, if the view is not focused
          // So that delete may be used on other controls as well
          if (!gridViewControl.View.Focused)
          {
            handled = false;
            break;
          }

          gridViewControl.DeleteTracks();
          break;

        case Action.ActionType.ACTION_TOGGLESTREEVIEWSPLITTER:
          splitterLeft.ToggleState();
          break;

        case Action.ActionType.ACTION_TOGGLEDATABASESPLITTER:
          splitterTop.ToggleState();
          break;

        case Action.ActionType.ACTION_TOGGLEQUICKEDIT:
          splitterBottom.ToggleState();
          break;

        case Action.ActionType.ACTION_TOGGLEMISCFILES:
          splitterRight.ToggleState();
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

        case Action.ActionType.ACTION_VALIDATEMP3:
          if (!gridViewControl.CheckSelections(true))
            break;
          gridViewControl.ValidateMP3File();
          gridViewControl.View.ClearSelection();
          break;

        case Action.ActionType.ACTION_FIXMP3:
          if (!gridViewControl.CheckSelections(true))
            break;
          gridViewControl.FixMP3File();
          break;

        case Action.ActionType.ACTION_FIND:
          _dialog = new FindReplace(this);
          (_dialog as FindReplace).Replace = false;
          ShowCenteredForm(_dialog);
          _showForm = false; // Don't show the dialog in the Keypress event
          break;

        case Action.ActionType.ACTION_REPLACE:
          _dialog = new FindReplace(this);
          (_dialog as FindReplace).Replace = true;
          ShowCenteredForm(_dialog);
          _showForm = false; // Don't show the dialog in the Keypress event
          break;
      }

      return handled;
    }

    #endregion

    #region GridView events

    /// <summary>
    ///   A new Row has been selected in the Grid. Fill the Quickedit Panel and update the picture in the gallery
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void DataGridView_SelectionChanged(object sender, EventArgs e)
    {
      if (gridViewControl.View.CurrentRow != null)
      {
        tagEditControl.FillForm();
        ribbonControl.SetGalleryItem();
      }

      // Update Status Information
      try
      {
        toolStripStatusLabelFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"),
                                                       gridViewControl.View.Rows.Count,
                                                       gridViewControl.View.SelectedRows.Count);
      }
      catch (InvalidOperationException) // we might get a Cross-thread Exception on startup
      {}
    }

    #endregion

    #region General Message Handling

    /// <summary>
    ///   Handle Messages
    /// </summary>
    /// <param name = "message"></param>
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
            Refresh();
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

    private void splitterBottom_Click(object sender, EventArgs e)
    {
      _bottomPanelCollapsed = splitterBottom.IsCollapsed;
    }

    #endregion

    #region Form Resize / Move events

    /// <summary>
    ///   The Form is resized, if Playlist is docked, move it as well
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Main_Resize(object sender, EventArgs e)
    {
      if (playerControl != null)
      {
        playerControl.MovePlayList();
      }
    }

    /// <summary>
    ///   The Form is resized, if Playlist is docked, move it as well
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Main_Move(object sender, EventArgs e)
    {
      if (playerControl != null)
      {
        playerControl.MovePlayList();
      }
    }

    #endregion

    #region Statusbar Events

    /// <summary>
    ///   We're hovering over the Cancel Button
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonProgressCancel_MouseEnter(object sender, EventArgs e)
    {
      TracksGridView.ProgressCancel_Hover();
    }

    /// <summary>
    ///   We're leaving the Cancel Button
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonProgressCancel_MouseLeave(object sender, EventArgs e)
    {
      TracksGridView.ProgressCancel_Leave();
    }

    #endregion

    #endregion
  }
}