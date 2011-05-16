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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Elegant.Ui;
using MPTagThat.Core;
using MPTagThat.Core.Burning;
using MPTagThat.Core.MediaChangeMonitor;
using MPTagThat.Core.WinControls;
using MPTagThat.Dialogues;
using MPTagThat.GridView;
using MPTagThat.Organise;
using MPTagThat.Player;
using MPTagThat.Properties;
using MPTagThat.TagEdit;
using NJFLib.Controls;
using Raccoom.Windows.Forms;
using TagLib;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Wma;
using Label = Elegant.Ui.Label;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;
using Action = MPTagThat.Core.Action;
using ComboBox = Elegant.Ui.ComboBox;
using ScrollEventArgs = System.Windows.Forms.ScrollEventArgs;

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
    private UserControl _currentControlshown = null;

    private DatabaseSearchControl databaseSearchControl;

    // Grids: Can't have them in Designer, as it will fail loading
    private GridViewBurn gridViewBurn;
    private GridViewTracks gridViewControl;
    private GridViewConvert gridViewConvert;
    private GridViewRip gridViewRip;
    private MiscInfoControl miscInfoControl;
    private PlayerControl playerControl;
    private TreeViewControl treeViewControl;
    private TagEditControl tagEditControl;

    private delegate void ThreadSafeFolderScan();

    // Ribbon Related Vars
    private readonly IActionHandler actionhandler = ServiceScope.Get<IActionHandler>();
    private readonly List<Item> encodersRip = new List<Item>();
    private readonly List<Item> encodersConvert = new List<Item>();
    private bool _initialising = true;
    private bool _numberingOnClick;
    private PictureControl picControl;
    private GalleryItem _displayedGalleryItem;

    // Settings related Variables
    private readonly List<Item> amazonSites = new List<Item>();
    private readonly List<ActionWindow> mapWindows = new List<ActionWindow>();

    private int _defaultBitRateIndex;
    private bool _keysChanged;
    private TreeNode _selectedNode;
    private Theme prevTheme;
    private TreeNode windowsNode;

    #endregion

    #region Constructor

    public Main()
    {
      // Activates double buffering 
      this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();

      SkinManager.LoadEmbeddedTheme(EmbeddedTheme.Office2007Silver, Product.Common);
      SkinManager.LoadEmbeddedTheme(EmbeddedTheme.Office2007Silver, Product.Ribbon);
      InitializeComponent();
    }

    #endregion

    #region Properties

    #region Main
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
    ///   return the Ribbon
    /// </summary>
    public Elegant.Ui.Ribbon Ribbon
    {
      get { return ribbon; }
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

    #region Ribbon Properties

    /// <summary>
    ///   Set the Theme of the Ribbon
    /// </summary>
    public string Theme
    {
      set
      {
        EmbeddedTheme theme = EmbeddedTheme.Office2007Blue;
        switch (value)
        {
          case "ControlDefault":
            theme = EmbeddedTheme.Office2007Blue;
            break;

          case "Office2007Silver":
            theme = EmbeddedTheme.Office2007Silver;
            break;

          case "Office2007Black":
            theme = EmbeddedTheme.Office2007Black;
            break;

          case "Office2010Blue":
            theme = EmbeddedTheme.Office2010Blue;
            break;

          case "Office2010Silver":
            theme = EmbeddedTheme.Office2010Silver;
            break;

          case "Office2010Black":
            theme = EmbeddedTheme.Office2010Black;
            break;
        }

        SkinManager.LoadEmbeddedTheme(theme, Product.Ribbon);
        ServiceScope.Get<IThemeManager>().ChangeTheme(value);

        // On a "1020" Theme, remove the Application Button Image
        if (value.Contains("2010"))
        {
          ribbon.ApplicationButtonImages.SetImage(RibbonApplicationButtonState.Normal, null);
          ribbon.ApplicationButtonStyle = RibbonApplicationButtonStyle.Office2010Azure;
        }
        else
        {
          Image img = Image.FromFile(string.Format("{0}\\Themes\\ribbon_StartButton.png", Application.StartupPath));
          if (img != null)
          {
            ribbon.ApplicationButtonImages.SetImage(RibbonApplicationButtonState.Normal, img);
          }
        }
      }
    }

    /// <summary>
    ///   Returns the Tag Tab
    /// </summary>
    public RibbonTabPage TabTag
    {
      get { return ribbonTabPageTag; }
    }

    /// <summary>
    ///   Returns the Burn Tab
    /// </summary>
    public RibbonTabPage TabBurn
    {
      get { return ribbonTabPageBurn; }
    }

    /// <summary>
    ///   Returns the Rip Tab
    /// </summary>
    public RibbonTabPage TabRip
    {
      get { return ribbonTabPageRip; }
    }

    /// <summary>
    ///   Returns the Convert Tab
    /// </summary>
    public RibbonTabPage TabConvert
    {
      get { return ribbonTabPageConvert; }
    }

    /// <summary>
    ///   Returns the Scripts Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox ScriptsCombo
    {
      get { return comboBoxScripts; }
    }

    /// <summary>
    ///   Returns the Conversion Encoder Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox EncoderCombo
    {
      get { return comboBoxConvertEncoder; }
    }

    /// <summary>
    ///   Returns the selected output Directory
    /// </summary>
    public string EncoderOutputDirectory
    {
      get { return textBoxConvertOutputFolder.Text; }
    }

    /// <summary>
    ///   Returns the Rip Encoder Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox RipEncoderCombo
    {
      get { return comboBoxRipEncoder; }
    }

    /// <summary>
    ///   Returns the selected output Directory
    /// </summary>
    public string RipOutputDirectory
    {
      get { return textBoxRipOutputFolder.Text; }
    }

    /// <summary>
    ///   Returns the Burner Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox BurnerCombo
    {
      get { return comboBoxBurner; }
    }

    /// <summary>
    ///   Get / Set Auto Numbering
    /// </summary>
    public int AutoNumber
    {
      get
      {
        try
        {
          return Convert.ToInt32(textBoxNumber.Text);
        }
        catch (Exception)
        {
          return -1;
        }
      }

      set { textBoxNumber.Text = value.ToString(); }
    }

    /// <summary>
    ///   Return Numbering On Click
    /// </summary>
    public bool NumberingOnClick
    {
      get { return _numberingOnClick; }
    }

    /// <summary>
    ///   Returns the Picture Gallery
    /// </summary>
    public Gallery PictureGallery
    {
      get { return galleryPicture; }
    }

    /// <summary>
    /// Enable or disable the RIP Buttons, when a media is inserted / removed
    /// </summary>
    public bool RipButtonsEnabled
    {
      set
      {
        if (value)
        {
          buttonRipStart.Enabled = true;
          buttonRipCancel.Enabled = true;
        }
        else
        {
          buttonRipStart.Enabled = false;
          buttonRipCancel.Enabled = false;
        }
      }
    }

    /// <summary>
    /// Enable or disable the Burn Buttons, when a media is inserted / removed
    /// </summary>
    public bool BurnButtonsEnabled
    {
      set
      {
        if (value)
        {
          buttonBurnStart.Enabled = true;
          buttonBurnCancel.Enabled = true;
        }
        else
        {
          buttonBurnStart.Enabled = false;
          buttonBurnCancel.Enabled = false;
        }
      }
    }

    #endregion

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

      // Add the Ribbon Control to the Top Panel
      _splashScreen.SetInformation(localisation.ToString("splash", "AddRibbon"));

      _initialising = true;

      #region Setup Ribbon

      log.Info("Initialising Ribbon");

      // Register the Ribbon Button Events
      RegisterCommands();

      // Register Ribbon KeyTips
      RegisterKeyTips();

      // Load Recent Folders
      List<PinItem> recentPlacesPinItems = new List<PinItem>();

      foreach (string folderItem in Options.MainSettings.RecentFolders)
      {
        string directoryName = Path.GetDirectoryName(folderItem);

        string folderName = Path.GetFileName(directoryName);
        if (string.IsNullOrEmpty(folderName))
          folderName = directoryName;

        PinItem pinItem = new PinItem(
        folderName,
        directoryName,
        Resources.RecentFolder_Large,
        false,
        directoryName);

        recentPlacesPinItems.Add(pinItem);
      }

      pinListRecentFolders.BeginInit();
      pinListRecentFolders.Items.AddRange(recentPlacesPinItems.ToArray());
      pinListRecentFolders.EndInit();

      // Load the available Scripts
      PopulateScriptsCombo();

      encodersRip.Add(new Item("MP3 Encoder", "mp3", ""));
      encodersRip.Add(new Item("OGG Encoder", "ogg", ""));
      encodersRip.Add(new Item("FLAC Encoder", "flac", ""));
      encodersRip.Add(new Item("AAC Encoder", "m4a", ""));
      encodersRip.Add(new Item("WMA Encoder", "wma", ""));
      encodersRip.Add(new Item("WAV Encoder", "wav", ""));
      encodersRip.Add(new Item("MusePack Encoder", "mpc", ""));
      encodersRip.Add(new Item("WavPack Encoder", "wv", ""));
      comboBoxRipEncoder.DisplayMember = "Name";
      comboBoxRipEncoder.ValueMember = "Value";
      comboBoxRipEncoder.DataSource = encodersRip;

      encodersConvert.Add(new Item("MP3 Encoder", "mp3", ""));
      encodersConvert.Add(new Item("OGG Encoder", "ogg", ""));
      encodersConvert.Add(new Item("FLAC Encoder", "flac", ""));
      encodersConvert.Add(new Item("AAC Encoder", "m4a", ""));
      encodersConvert.Add(new Item("WMA Encoder", "wma", ""));
      encodersConvert.Add(new Item("WAV Encoder", "wav", ""));
      encodersConvert.Add(new Item("MusePack Encoder", "mpc", ""));
      encodersConvert.Add(new Item("WavPack Encoder", "wv", ""));
      comboBoxConvertEncoder.DisplayMember = "Name";
      comboBoxConvertEncoder.ValueMember = "Value";
      comboBoxConvertEncoder.DataSource = encodersConvert;

      int i = 0;
      foreach (Item item in encodersRip)
      {
        if ((string)item.Value == Options.MainSettings.LastConversionEncoderUsed)
        {
          comboBoxConvertEncoder.SelectedIndex = i;
        }

        if ((string)item.Value == Options.MainSettings.RipEncoder)
        {
          comboBoxRipEncoder.SelectedIndex = i;
        }
        i++;
      }

      textBoxRipOutputFolder.Text = Options.MainSettings.RipTargetFolder;
      ribbon.CurrentTabPage = ribbonTabPageTag;
      ribbon.CustomTitleBarEnabled = true;
      log.Info("Finished Initialising Ribbon");

      #endregion

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

      // Now position the Tracklist and Tagedit Panel
      PositionTrackList();

      // Start Listening for Media Changes
      ServiceScope.Get<IMediaChangeMonitor>().StartListening(Handle);

      // Load BASS
      LoadBass();

      // Localise the Screens
      log.Info("Main: Localisation");
      _splashScreen.SetInformation(localisation.ToString("splash", "Localisation"));
      LocaliseScreen();

      // Load the Settings
      _splashScreen.SetInformation(localisation.ToString("splash", "LoadSettings"));
      LoadSettings();

      // Populate the Treeview with the directories found
      treeViewControl.Init();
      treeViewControl.TreeView.Populate();
      treeViewControl.TreeView.Nodes[0].Expand();
      treeViewControl.TreeView.ShowFolder(_selectedDirectory);

      _splashScreen.Stop();

      Theme = Options.Themes[Options.MainSettings.Theme];

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

      _initialising = false;

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
      Options.MainSettings.ActiveScript = ScriptsCombo.Text;
      Options.SaveAllSettings();
      log.Info("Main: Finished closing Main form");
      log.Trace("<<<");
    }

    /// <summary>
    /// Position the Tracklist and Tagedit Details based on the selected option
    /// </summary>
    private void PositionTrackList()
    {
      // Remove controls firs, if they already exist
      if (panelMiddleBottom.Controls.Contains(tagEditControl))
      {
        panelMiddleBottom.Controls.Remove(tagEditControl);
        panelFileList.Controls.Remove(gridViewControl);
        panelFileList.Controls.Remove(gridViewBurn);
        panelFileList.Controls.Remove(gridViewRip);
        panelFileList.Controls.Remove(gridViewConvert);
      }
      else if (panelFileList.Controls.Contains(tagEditControl))
      {
        panelFileList.Controls.Remove(tagEditControl);
        panelMiddleBottom.Controls.Remove(gridViewControl);
        panelMiddleBottom.Controls.Remove(gridViewBurn);
        panelMiddleBottom.Controls.Remove(gridViewRip);
        panelMiddleBottom.Controls.Remove(gridViewConvert);
      }

      if (Options.MainSettings.TrackListLocation == 0)
      {
        // Tag Edit Details goes bottom
        panelMiddleBottom.Controls.Add(tagEditControl);

        // Tracklist goes Top
        panelFileList.Controls.Add(gridViewControl);
        panelFileList.Controls.Add(gridViewBurn);
        panelFileList.Controls.Add(gridViewRip);
        panelFileList.Controls.Add(gridViewConvert);
      }
      else
      {
        // Tag Edit Details goes Top
        panelFileList.Controls.Add(tagEditControl);

        // Tracklist goes bottom
        panelMiddleBottom.Controls.Add(gridViewControl);
        panelMiddleBottom.Controls.Add(gridViewBurn);
        panelMiddleBottom.Controls.Add(gridViewRip);
        panelMiddleBottom.Controls.Add(gridViewConvert);
      }
    }
    #endregion

    #region Ribbon Related Methods

    /// <summary>
    ///   Register Ribbon Commands to be executed when a button is pressed.
    /// </summary>
    private void RegisterCommands()
    {
      log.Trace(">>>");
      ApplicationCommands.AddToBurner.Executed += TagsTabButton_Executed;
      ApplicationCommands.AddToConversion.Executed += TagsTabButton_Executed;
      ApplicationCommands.AddToPlaylist.Executed += TagsTabButton_Executed;
      ApplicationCommands.AutoNumber.Executed += TagsTabButton_Executed;
      ApplicationCommands.BurnCancel.Executed += BurnCancel_Executed;
      ApplicationCommands.BurnStart.Executed += BurnStart_Executed;
      ApplicationCommands.CaseConversion.Executed += TagsTabButton_Executed;
      ApplicationCommands.CaseConversionOptions.Executed += TagsTabButton_Executed;
      ApplicationCommands.ChangeDisplayColumns.Executed += ChangeDisplayColumns_Executed;
      ApplicationCommands.ConvertCancel.Executed += ConvertCancel_Executed;
      ApplicationCommands.ConvertStart.Executed += ConvertStart_Executed;
      ApplicationCommands.DeleteAllTags.Executed += TagsTabButton_Executed;
      ApplicationCommands.DeleteID3v1.Executed += TagsTabButton_Executed;
      ApplicationCommands.DeleteID3v2.Executed += TagsTabButton_Executed;
      ApplicationCommands.Exit.Executed += Exit_Executed;
      ApplicationCommands.FileNameToTag.Executed += TagsTabButton_Executed;
      ApplicationCommands.FolderSelect.Executed += FolderSelect_Executed;
      ApplicationCommands.GetCoverArt.Executed += TagsTabButton_Executed;
      ApplicationCommands.GetLyrics.Executed += TagsTabButton_Executed;
      ApplicationCommands.Help.Executed += Help_Executed;
      ApplicationCommands.IdentifyFiles.Executed += TagsTabButton_Executed;
      ApplicationCommands.OrganiseFiles.Executed += TagsTabButton_Executed;
      ApplicationCommands.Refresh.Executed += Refresh_Executed;
      ApplicationCommands.RemoveComment.Executed += TagsTabButton_Executed;
      ApplicationCommands.RemoveCoverArt.Executed += TagsTabButton_Executed;
      ApplicationCommands.RenameFileOptions.Executed += TagsTabButton_Executed;
      ApplicationCommands.RenameFiles.Executed += TagsTabButton_Executed;
      ApplicationCommands.RipCancel.Executed += RipCancel_Executed;
      ApplicationCommands.RipStart.Executed += RipStart_Executed;
      ApplicationCommands.Save.Executed += Save_Executed;
      ApplicationCommands.ScriptExecute.Executed += TagsTabButton_Executed;
      ApplicationCommands.TagFromInternet.Executed += TagsTabButton_Executed;
      ApplicationCommands.SaveAsThumb.Executed += SaveAsThumb_Executed;

      ApplicationCommands.SaveAsThumb.Enabled = false; // Disable button initally
      log.Trace("<<<");
    }

    /// <summary>
    ///   Register the Keytips to be displayed in the menu, when pressing Alt or F10
    /// </summary>
    private void RegisterKeyTips()
    {
      log.Trace(">>>");
      // Start Menu
      backstageViewButtonSave.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_SAVE);
      backstageViewButtonRefresh.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_REFRESH);
      backstageViewPageOptions.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_OPTIONS);
      backstageViewButtonExit.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_EXIT);

      // Tags Tab
      buttonTagFromFile.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_FILENAME2TAG);
      buttonTagIdentifyFiles.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_IDENTIFYFILE);
      buttonTagFromInternet.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_TAGFROMINTERNET);

      buttonGetCoverArt.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_GETCOVERART);
      buttonGetLyrics.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_GETLYRICS);
      buttonRemoveComment.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_REMOVECOMMENT);
      buttonCaseConversion.ButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_CASECONVERSION);
      buttonScriptExecute.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_SCRIPTEXECUTE);

      buttonRenameFiles.ButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_TAG2FILENAME);
      buttonOrganiseFiles.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_ORGANISE);
      log.Trace("<<<");
    }

    /// <summary>
    ///   Fill the Script Combo  Box with all scripts found
    /// </summary>
    private void PopulateScriptsCombo()
    {
      comboBoxScripts.Items.Clear();
      ArrayList scripts = null;

      if (Options.MainSettings.ActiveScript == "")
      {
        Options.MainSettings.ActiveScript = "Switch Artist";
      }

      scripts = ServiceScope.Get<IScriptManager>().GetScripts();
      int i = 0;
      foreach (string[] item in scripts)
      {
        comboBoxScripts.Items.Add(new Item(item[1], item[0], item[2]));
        if (item[1] == Options.MainSettings.ActiveScript)
        {
          comboBoxScripts.SelectedIndex = i;
        }
        i++;
      }
    }

    /// <summary>
    ///   Get the Coverart out of the selected TRack item and fill the Ribbon Gallery
    /// </summary>
    public void SetGalleryItem()
    {
      Image img = null;
      ClearGallery();
      try
      {
        TrackData track = TracksGridView.SelectedTrack;
        ApplicationCommands.SaveAsThumb.Enabled = false;
        if (track.Pictures.Count > 0)
        {
          img = track.Pictures[0].Data;
          if (img != null)
          {
            GalleryItem galleryItem = new GalleryItem(img, "", "");
            galleryPicture.Items.Add(galleryItem);
            ApplicationCommands.SaveAsThumb.Enabled = true;
          }
        }
      }
      catch (Exception) { }
    }

    /// <summary>
    ///   Clear the Ribbon Gallery
    /// </summary>
    public void ClearGallery()
    {
      galleryPicture.Items.Clear();
      galleryPicture.Invalidate();
    }

    /// <summary>
    ///   Set the Recent Folder, when a new folder has been selected
    /// </summary>
    /// <param name = "newFolder"></param>
    public void SetRecentFolder(string newFolder)
    {
      string folderName = Path.GetFileName(newFolder);
      if (string.IsNullOrEmpty(folderName))
        folderName = newFolder;

      PinItem pinItem = new PinItem(
          folderName,
          newFolder,
          Resources.RecentFolder_Large,
          false,
          newFolder);

      try
      {
        for (int i = 0; i < pinListRecentFolders.Items.Count; i++)
        {
          if (pinListRecentFolders.Items[i].DescriptionText == newFolder)
          {
            pinListRecentFolders.Items.RemoveAt(i);
            break;
          }
        }
      }
      catch (ArgumentException) { }

      pinListRecentFolders.Items.Insert(0, pinItem);

      Options.MainSettings.RecentFolders.Insert(0, newFolder);
      if (Options.MainSettings.RecentFolders.Count > 20)
      {
        Options.MainSettings.RecentFolders.RemoveAt(20);
      }
    }

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
        log.Error("Error Init Bass: {0}", Enum.GetName(typeof(BASSError), error));
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

      // Ribbon
      log.Debug("Localise Ribbon");
      // BackstageView
      backstageViewButtonSave.Text = localisation.ToString("ribbon", "Save");
      backstageViewButtonSave.ScreenTip.Caption = localisation.ToString("screentip", "Save");
      backstageViewButtonSave.ScreenTip.Text = localisation.ToString("screentip", "SaveText");

      backstageViewButtonRefresh.Text = localisation.ToString("ribbon", "Refresh");
      backstageViewButtonRefresh.ScreenTip.Caption = localisation.ToString("screentip", "Refresh");
      backstageViewButtonRefresh.ScreenTip.Text = localisation.ToString("screentip", "RefreshText");

      backstageViewButtonChangeColumns.Text = localisation.ToString("ribbon", "ColumnsSelect");
      backstageViewButtonChangeColumns.ScreenTip.Caption = localisation.ToString("screentip", "ColumnsSelect");
      backstageViewButtonChangeColumns.ScreenTip.Text = localisation.ToString("screentip", "ColumnsSelectText");

      backstageViewButtonExit.Text = localisation.ToString("ribbon", "Exit");

      backstageViewPageRecentFolders.Text = localisation.ToString("ribbon", "RecentFolders");
      separatorRecentFolders.Text = localisation.ToString("ribbon", "RecentFolders");

      backstageViewPageOptions.Text = localisation.ToString("ribbon", "Settings");

      // Tags Tab
      ribbonTabPageTag.Text = localisation.ToString("ribbon", "TagTab");

      buttonTagFromFile.Text = localisation.ToString("ribbon", "TagFromFile");
      buttonTagFromFile.ScreenTip.Caption = localisation.ToString("screentip", "TagFromFile");
      buttonTagFromFile.ScreenTip.Text = localisation.ToString("screentip", "TagFromFileText");

      buttonTagIdentifyFiles.Text = localisation.ToString("ribbon", "IdentifyFile");
      buttonTagIdentifyFiles.ScreenTip.Caption = localisation.ToString("screentip", "IdentifyFile");
      buttonTagIdentifyFiles.ScreenTip.Text = localisation.ToString("screentip", "IdentifyFileText");

      buttonTagFromInternet.Text = localisation.ToString("ribbon", "TagFromInternet");
      buttonTagFromInternet.ScreenTip.Caption = localisation.ToString("screentip", "TagFromInternet");
      buttonTagFromInternet.ScreenTip.Text = localisation.ToString("screentip", "TagFromInternetText");

      ribbonGroupTagsRetrieve.Text = localisation.ToString("ribbon", "RetrieveTags");

      buttonGetCoverArt.Text = localisation.ToString("ribbon", "GetCoverArt");
      buttonGetCoverArt.ScreenTip.Caption = localisation.ToString("screentip", "GetCoverArt");
      buttonGetCoverArt.ScreenTip.Text = localisation.ToString("screentip", "GetCoverArtText");

      buttonSaveAsThumb.ScreenTip.Caption = localisation.ToString("screentip", "SaveFolderThumb");
      buttonSaveAsThumb.ScreenTip.Text = localisation.ToString("screentip", "SaveFolderThumbText");

      buttonGetLyrics.Text = localisation.ToString("ribbon", "GetLyrics");
      buttonGetLyrics.ScreenTip.Caption = localisation.ToString("screentip", "GetLyrics");
      buttonGetLyrics.ScreenTip.Text = localisation.ToString("screentip", "GetLyricsText");

      buttonAutoNumber.ScreenTip.Caption = localisation.ToString("screentip", "AutoNumber");
      buttonAutoNumber.ScreenTip.Text = localisation.ToString("screentip", "AutoNumberText");

      buttonNumberOnClick.ScreenTip.Caption = localisation.ToString("screentip", "NumberOnClick");
      buttonNumberOnClick.ScreenTip.Text = localisation.ToString("screentip", "NumberOnClickText");

      buttonRemoveComment.Text = localisation.ToString("ribbon", "RemoveComments");
      buttonRemoveComment.ScreenTip.Caption = localisation.ToString("screentip", "RemoveComments");
      buttonRemoveComment.ScreenTip.Text = localisation.ToString("screentip", "RemoveCommentsText");

      buttonRemoveCoverArt.ScreenTip.Caption = localisation.ToString("screentip", "RemovePictures");
      buttonRemoveCoverArt.ScreenTip.Text = localisation.ToString("screentip", "RemovePicturesText");

      buttonCaseConversion.Text = localisation.ToString("ribbon", "CaseConversion");
      buttonCaseConversion.ButtonScreenTip.Caption = localisation.ToString("screentip", "CaseConversion");
      buttonCaseConversion.ButtonScreenTip.Text = localisation.ToString("screentip", "CaseConversionText");
      buttonCaseConversionOptions.Text = localisation.ToString("ribbon", "CaseConversionOption");

      buttonDeleteTag.Text = localisation.ToString("ribbon", "DeleteTags");
      buttonDeleteTag.ButtonScreenTip.Caption = localisation.ToString("screentip", "DeleteTags");
      buttonDeleteTag.ButtonScreenTip.Text = localisation.ToString("screentip", "DeleteTagsText");
      buttonDeleteAllTags.Text = localisation.ToString("ribbon", "DeleteAllTags");
      buttonDeleteID3v1.Text = localisation.ToString("ribbon", "DeleteID3V1Tags");
      buttonDeleteID3v2.Text = localisation.ToString("ribbon", "DeleteID3V2Tags");
      ribbonGroupTagsEdit.Text = localisation.ToString("ribbon", "EditTags");

      buttonRenameFiles.Text = localisation.ToString("ribbon", "RenameFile");
      buttonRenameFiles.ButtonScreenTip.Caption = localisation.ToString("screentip", "RenameFile");
      buttonRenameFiles.ButtonScreenTip.Text = localisation.ToString("screentip", "RenameFileText");
      buttonRenameFilesOptions.Text = localisation.ToString("ribbon", "RenameFileOptions");

      buttonOrganiseFiles.Text = localisation.ToString("ribbon", "Organise");
      buttonOrganiseFiles.ScreenTip.Caption = localisation.ToString("screentip", "Organise");
      buttonOrganiseFiles.ScreenTip.Text = localisation.ToString("screentip", "OrganiseText");

      ribbonGroupOrganise.Text = localisation.ToString("ribbon", "OrganiseFiles");

      buttonScriptExecute.Text = localisation.ToString("ribbon", "ExecuteScript");
      buttonScriptExecute.ScreenTip.Caption = localisation.ToString("screentip", "ExecuteScript");
      buttonScriptExecute.ScreenTip.Text = localisation.ToString("screentip", "ExecuteScriptText");

      ribbonGroupPicture.Text = localisation.ToString("ribbon", "Picture");

      buttonAddToBurner.Text = localisation.ToString("ribbon", "AddBurner");
      buttonAddToBurner.ScreenTip.Caption = localisation.ToString("screentip", "AddBurner");
      buttonAddToBurner.ScreenTip.Text = localisation.ToString("screentip", "AddBurnerText");

      buttonAddToConversion.Text = localisation.ToString("ribbon", "AddConvert");
      buttonAddToConversion.ScreenTip.Caption = localisation.ToString("screentip", "AddConvert");
      buttonAddToConversion.ScreenTip.Text = localisation.ToString("screentip", "AddConvertText");

      buttonAddToPlaylist.Text = localisation.ToString("ribbon", "AddPlaylist");
      buttonAddToPlaylist.ScreenTip.Caption = localisation.ToString("screentip", "AddPlaylist");
      buttonAddToPlaylist.ScreenTip.Text = localisation.ToString("screentip", "AddPlaylistText");

      ribbonGroupOther.Text = localisation.ToString("ribbon", "Other");

      // Rip Tab
      ribbonTabPageRip.Text = localisation.ToString("ribbon", "RipTab");
      buttonRipStart.Text = localisation.ToString("ribbon", "RipButton");
      buttonRipStart.ScreenTip.Caption = localisation.ToString("screentip", "RipButton");
      buttonRipStart.ScreenTip.Text = localisation.ToString("screentip", "RipButtonText");
      comboBoxRipEncoder.LabelText = localisation.ToString("ribbon", "RipEncoder");
      textBoxRipOutputFolder.LabelText = localisation.ToString("ribbon", "RipFolder");
      buttonRipCancel.Text = localisation.ToString("ribbon", "RipCancel");
      buttonRipCancel.ScreenTip.Caption = localisation.ToString("screentip", "RipCancel");
      buttonRipCancel.ScreenTip.Text = localisation.ToString("screentip", "RipCancelText");
      ribbonGroupRipOptions.Text = localisation.ToString("ribbon", "RipOptions");

      // Convert Tab
      ribbonTabPageConvert.Text = localisation.ToString("ribbon", "ConvertTab");
      buttonConvertStart.Text = localisation.ToString("ribbon", "ConvertButton");
      buttonConvertStart.ScreenTip.Caption = localisation.ToString("screentip", "ConvertButton");
      buttonConvertStart.ScreenTip.Text = localisation.ToString("screentip", "ConvertButtonText");
      comboBoxConvertEncoder.LabelText = localisation.ToString("ribbon", "ConvertEncoder");
      textBoxConvertOutputFolder.LabelText = localisation.ToString("ribbon", "ConvertFolder");
      buttonConvertCancel.Text = localisation.ToString("ribbon", "ConvertCancel");
      buttonConvertCancel.ScreenTip.Caption = localisation.ToString("screentip", "ConvertCancel");
      buttonConvertCancel.ScreenTip.Text = localisation.ToString("screentip", "ConvertCancelText");
      ribbonGroupConvertOptions.Text = localisation.ToString("ribbon", "ConvertOptions");

      // Burn Tab
      ribbonTabPageBurn.Text = localisation.ToString("ribbon", "BurnTab");
      buttonBurnStart.Text = localisation.ToString("ribbon", "Burn");
      buttonBurnStart.ScreenTip.Caption = localisation.ToString("screentip", "Burn");
      buttonBurnStart.ScreenTip.Text = localisation.ToString("screentip", "BurnText");
      buttonBurnCancel.Text = localisation.ToString("ribbon", "BurnCancel");
      buttonBurnCancel.ScreenTip.Caption = localisation.ToString("screentip", "BurnCancel");
      buttonBurnCancel.ScreenTip.Text = localisation.ToString("screentip", "BurnCancelText");
      ribbonGroupBurnOptions.Text = localisation.ToString("ribbon", "BurnOptions");
      comboBoxBurner.LabelText = localisation.ToString("ribbon", "Burner");
      comboBoxBurnerSpeed.LabelText = localisation.ToString("ribbon", "BurnerSPeed");

      log.Debug("Finished localising Ribbon");


      log.Trace("<<<");
    }

    #endregion

    #region Load Settings / Layout

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

      if (Options.MainSettings.BottomPanelSize > -1 && Options.MainSettings.BottomPanelSize < 1024)
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

      // We want to have our own color and Font for the various Grids
      gridViewControl.View.Font = themeManager.CurrentTheme.LabelFont;
      gridViewControl.View.EnableHeadersVisualStyles = false;
      gridViewControl.View.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      gridViewControl.View.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      gridViewControl.View.DefaultCellStyle.BackColor = themeManager.CurrentTheme.DefaultBackColor;
      gridViewControl.View.DefaultCellStyle.SelectionBackColor = themeManager.CurrentTheme.SelectionBackColor;
      gridViewControl.View.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      gridViewControl.View.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;
      
      gridViewBurn.View.Font = themeManager.CurrentTheme.LabelFont;
      gridViewBurn.View.EnableHeadersVisualStyles = false;
      gridViewBurn.View.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      gridViewBurn.View.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      gridViewBurn.BackGroundColor = themeManager.CurrentTheme.BackColor;
      gridViewBurn.View.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      gridViewBurn.View.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;
      
      gridViewRip.View.Font = themeManager.CurrentTheme.LabelFont;
      gridViewRip.View.EnableHeadersVisualStyles = false;
      gridViewRip.View.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      gridViewRip.View.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      gridViewRip.BackGroundColor = themeManager.CurrentTheme.BackColor;
      gridViewRip.View.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      gridViewRip.View.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;
      
      gridViewConvert.View.Font = themeManager.CurrentTheme.LabelFont;
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
        ClearGallery();
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
      AutoNumber = 1; // Reset the number on Folder Change
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
      UserControl control = (UserControl)dlg;

      // REmove current control first, before adding anothe one
      if (_currentControlshown != null)
      {
        _currentControlshown.Dispose();
      }

      _currentControlshown = control;

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

    #region Settings / Preferences

    /// <summary>
    /// Initialises Preferences
    /// </summary>
    private void InitPreferences()
    {
      log.Trace(">>>");

      #region TabPage General

      comboBoxLanguage.Items.Clear();
      // Get available languages
      CultureInfo[] availLanguages = localisation.AvailableLanguages();
      foreach (CultureInfo info in availLanguages)
      {
        comboBoxLanguage.Items.Add(new Item(info.DisplayName, info.Name, ""));
      }

      foreach (Item item in comboBoxLanguage.Items)
      {
        if ((string)item.Value == localisation.CurrentCulture.Name)
        {
          comboBoxLanguage.SelectedItem = item;
          break;
        }
      }

      // Themes
      comboBoxThemes.Items.Clear();
      comboBoxThemes.Items.Add(new Item(localisation.ToString("Settings", "Blue"), "ControlDefault", ""));
      comboBoxThemes.Items.Add(new Item(localisation.ToString("Settings", "Silver"), "Office2007Silver", ""));
      comboBoxThemes.Items.Add(new Item(localisation.ToString("Settings", "Black"), "Office2007Black", ""));
      comboBoxThemes.Items.Add(new Item(string.Format("{0} (2010)", localisation.ToString("Settings", "Blue")), "Office2010Blue", ""));
      comboBoxThemes.Items.Add(new Item(string.Format("{0} (2010)", localisation.ToString("Settings", "Silver")), "Office2010Silver", ""));
      comboBoxThemes.Items.Add(new Item(string.Format("{0} (2010)", localisation.ToString("Settings", "Black")), "Office2010Black", ""));

      // Set the Selectedinde handler after we have filled the box to prevent changing the theme
      comboBoxThemes.SelectedIndex = Options.MainSettings.Theme;
      comboBoxThemes.SelectedIndexChanged += new EventHandler(comboBoxThemes_SelectedIndexChanged);
      

      // Save the currently used theme, in case the user presses Cancel.
      prevTheme = ServiceScope.Get<IThemeManager>().CurrentTheme;

      // Debug Level
      comboBoxDebugLevel.Items.Clear();
      comboBoxDebugLevel.Items.Add("Off");
      comboBoxDebugLevel.Items.Add("Fatal");
      comboBoxDebugLevel.Items.Add("Error");
      comboBoxDebugLevel.Items.Add("Warn");
      comboBoxDebugLevel.Items.Add("Info");
      comboBoxDebugLevel.Items.Add("Debug");
      comboBoxDebugLevel.Items.Add("Trace");
      comboBoxDebugLevel.Text = Options.MainSettings.DebugLevel;

      if (Options.MainSettings.TrackListLocation == 0)
      {
        pictureBoxTrackListTop.Image = Resources.TrackList_top_selected;
        pictureBoxTrackListBottom.Image = Resources.TrackList_bottom;
      }
      else
      {
        pictureBoxTrackListTop.Image = Resources.TrackList_top;
        pictureBoxTrackListBottom.Image = Resources.TrackList_bottom_selected;
      }


      // Load the keymap file into the treeview
      if (LoadKeyMap())
      {
        PopulateKeyTreeView();
        treeViewKeys.ExpandAll();
      }

      #endregion

      #region TabPage Ripping

      #region General

      tbTargetFolder.Text = Options.MainSettings.RipTargetFolder == ""
                              ? Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
                              : Options.MainSettings.RipTargetFolder;
      ckRipEjectCD.Checked = Options.MainSettings.RipEjectCD;
      ckActivateTargetFolder.Checked = Options.MainSettings.RipActivateTargetFolder;

      comboBoxEncoder.Items.Clear();
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderMP3"), "mp3", ""));
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderOgg"), "ogg", ""));
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderFlac"), "flac", ""));
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderAAC"), "m4a", ""));
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderWMA"), "wma", ""));
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderWAV"), "wav", ""));
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderMPC"), "mpc", ""));
      comboBoxEncoder.Items.Add(new Item(localisation.ToString("Settings", "EncoderWV"), "wv", ""));

      foreach (Item item in comboBoxEncoder.Items)
      {
        if ((string)item.Value == Options.MainSettings.RipEncoder)
        {
          comboBoxEncoder.SelectedItem = item;
          break;
        }
      }

      #endregion

      #region AAC

      comboBoxAACBitrates.Items.Clear();
      comboBoxAACBitrates.Items.AddRange(Options.BitRatesLCAAC);
      SetBitRate();

      #endregion

      #region WMA

      comboBoxWMAEncoderFormat.Items.Clear();
      comboBoxWMAEncoderFormat.Items.Add(new Item(localisation.ToString("Settings", "EncoderWmaStandard"), "wma", ""));
      comboBoxWMAEncoderFormat.Items.Add(new Item(localisation.ToString("Settings", "EncoderWmaPro"), "wmapro", ""));
      comboBoxWMAEncoderFormat.Items.Add(new Item(localisation.ToString("Settings", "EncoderWmaLossless"), "wmalossless", ""));

      foreach (Item item in comboBoxWMAEncoderFormat.Items)
      {
        if ((string)item.Value == Options.MainSettings.RipEncoderWMA)
        {
          comboBoxWMAEncoderFormat.SelectedItem = item;
        }
      }

      #endregion

      #region MPC

      comboBoxMPCPresets.Items.Clear();
      comboBoxMPCPresets.Items.Add(new Item(localisation.ToString("Settings", "EncoderMPCStandard"), "standard", ""));
      comboBoxMPCPresets.Items.Add(new Item(localisation.ToString("Settings", "EncoderMPCxtreme"), "xtreme", ""));
      comboBoxMPCPresets.Items.Add(new Item(localisation.ToString("Settings", "EncoderMPCinsane"), "insane", ""));
      comboBoxMPCPresets.Items.Add(new Item(localisation.ToString("Settings", "EncoderMPCbraindead"), "braindead", ""));

      foreach (Item item in comboBoxMPCPresets.Items)
      {
        if ((string)item.Value == Options.MainSettings.RipEncoderMPCPreset)
        {
          comboBoxMPCPresets.SelectedItem = item;
          break;
        }
      }
      textBoxMPCParms.Text = Options.MainSettings.RipEncoderMPCExpert;

      #endregion

      #region WV

      comboBoxWVPresets.Items.Clear();
      comboBoxWVPresets.Items.Add(new Item(localisation.ToString("Settings", "EncoderWVFast"), "-f", ""));
      comboBoxWVPresets.Items.Add(new Item(localisation.ToString("Settings", "EncoderWVHigh"), "-h", ""));

      foreach (Item item in comboBoxWVPresets.Items)
      {
        if ((string)item.Value == Options.MainSettings.RipEncoderWVPreset)
        {
          comboBoxWVPresets.SelectedItem = item;
          break;
        }
      }
      textBoxWVParms.Text = Options.MainSettings.RipEncoderWVExpert;

      #endregion

      #region MP3

      textBoxRippingFilenameFormat.Text = Options.MainSettings.RipFileNameFormat;
      comboBoxLamePresets.SelectedIndex = Options.MainSettings.RipLamePreset;
      textBoxABRBitrate.Text = Options.MainSettings.RipLameABRBitRate == 0
                                 ? ""
                                 : Options.MainSettings.RipLameABRBitRate.ToString();
      textBoxLameParms.Text = Options.MainSettings.RipLameExpert;

      #endregion

      #region Ogg

      hScrollBarOggEncodingQuality.Value = Options.MainSettings.RipOggQuality;
      lbOggQualitySelected.Text = hScrollBarOggEncodingQuality.Value.ToString();
      textBoxOggParms.Text = Options.MainSettings.RipOggExpert;

      #endregion

      #region FLAC

      hScrollBarFlacEncodingQuality.Value = Options.MainSettings.RipFlacQuality;
      lbFlacQualitySelected.Text = hScrollBarFlacEncodingQuality.Value.ToString();
      textBoxFlacParms.Text = Options.MainSettings.RipFlacExpert;

      #endregion

      #endregion

      #region TabPage Tags

      comboBoxCharacterEncoding.SelectedIndex = Options.MainSettings.CharacterEncoding;

      ckCopyArtistToAlbumArtist.Checked = Options.MainSettings.CopyArtist;
      ckAutoFillNumberOfTracks.Checked = Options.MainSettings.AutoFillNumberOfTracks;
      ckUseCaseConversionWhenSaving.Checked = Options.MainSettings.UseCaseConversion;
      ckChangeReadonlyAttributte.Checked = Options.MainSettings.ChangeReadOnlyAttributte;
      ckCreateMissingFolderThumb.Checked = Options.MainSettings.CreateFolderThumb;
      ckUseExistinbgThumb.Checked = Options.MainSettings.EmbedFolderThumb;
      ckOverwriteExistingCovers.Checked = Options.MainSettings.OverwriteExistingCovers;
      ckOverwriteExistingLyrics.Checked = Options.MainSettings.OverwriteExistingLyrics;
      ckOnlySaveFolderThumb.Checked = Options.MainSettings.OnlySaveFolderThumb;

      ckUseMediaPortalDatabase.Checked = Options.MainSettings.UseMediaPortalDatabase;
      tbMediaPortalDatabase.Text = Options.MainSettings.MediaPortalDatabase;

      ckValidateMP3.Checked = Options.MainSettings.MP3Validate;
      ckAutoFixMp3.Checked = Options.MainSettings.MP3AutoFix;

      switch (Options.MainSettings.ID3V2Version)
      {
        case 0: // APE Tags embedded in mp3
          radioButtonUseApe.Checked = true;
          break;

        case 3:
          radioButtonUseV3.Checked = true;
          break;

        case 4:
          radioButtonUseV4.Checked = true;
          break;
      }

      switch (Options.MainSettings.ID3Version)
      {
        case 1:
          radioButtonID3V1.Checked = true;
          break;

        case 2:
          radioButtonID3V2.Checked = true;
          break;

        case 3:
          radioButtonID3Both.Checked = true;
          break;
      }

      checkBoxRemoveID3V1.Checked = Options.MainSettings.RemoveID3V1;
      checkBoxRemoveID3V2.Checked = Options.MainSettings.RemoveID3V2;

      ckHotLyrics.Checked = Options.MainSettings.SearchHotLyrics;
      ckLyrics007.Checked = Options.MainSettings.SearchLyrics007;
      ckLyricsOnDemand.Checked = Options.MainSettings.SearchLyricsOnDemand;
      ckLyricWiki.Checked = Options.MainSettings.SearchLyricWiki;
      ckLyricsPlugin.Checked = Options.MainSettings.SearchLyricsPlugin;
      ckActionext.Checked = Options.MainSettings.SearchActionext;
      ckLyrDB.Checked = Options.MainSettings.SearchLyrDB;
      ckLRCFinder.Checked = Options.MainSettings.SearchLRCFinder;
      ckSwitchArtist.Checked = Options.MainSettings.SwitchArtist;

      comboBoxAmazonSite.Items.Clear();
      comboBoxAmazonSite.Items.Add(new Item("United States (US)", "com", ""));
      comboBoxAmazonSite.Items.Add(new Item("Deutschland (DE)", "de", ""));
      comboBoxAmazonSite.Items.Add(new Item("United Kingdom (UK)", "co.uk", ""));
      comboBoxAmazonSite.Items.Add(new Item("Nippon (JP)", "jp", ""));
      comboBoxAmazonSite.Items.Add(new Item("France (FR)", "fr", ""));
      comboBoxAmazonSite.Items.Add(new Item("Canada (CA)", "ca", ""));

      foreach (Item item in comboBoxAmazonSite.Items)
      {
        if ((string)item.Value == Options.MainSettings.AmazonSite)
        {
          comboBoxAmazonSite.SelectedItem = item;
          break;
        }
      }

      #endregion

      log.Trace("<<<");
    }

    #region Keys

    private void PopulateKeyTreeView()
    {
      log.Trace(">>>");
      windowsNode = new TreeNode("Windows");
      treeViewKeys.Nodes.Add(windowsNode);

      foreach (ActionWindow map in mapWindows)
      {
        TreeNode windownode = new TreeNode(map.Description + "[" + map.Window + "]");

        foreach (KeyAction action in map.Buttons)
        {
          string modifier = action.Modifiers;
          if (modifier == null)
            modifier = "";

          string nodeText = string.Format("{0} ({1}) = {2}{3}", action.Description, action.ActionType, modifier,
                                          action.KeyCode);
          TreeNode actionNode = new TreeNode(nodeText);
          actionNode.Tag = action;
          windownode.Nodes.Add(actionNode);
        }
        windowsNode.Nodes.Add(windownode);
      }

      treeViewKeys.Sort();
      log.Trace("<<<");
    }

    private bool SaveKeyMap()
    {
      log.Trace(">>>");
      if (!_keysChanged)
        return false;

      string strFilename = String.Format(@"{0}\{1}", Options.ConfigDir, "keymap.xml");

      try
      {
        using (FileStream fileStream = new FileStream(strFilename, FileMode.Create))
        {
          using (XmlTextWriter writer = new XmlTextWriter(fileStream, Encoding.UTF8))
          {
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();

            writer.WriteStartElement("keymap");

            foreach (ActionWindow window in mapWindows)
            {
              writer.WriteStartElement("window");

              writer.WriteStartElement("id");
              writer.WriteString(window.Window.ToString());
              writer.WriteEndElement();

              writer.WriteStartElement("description");
              writer.WriteString(window.Description);
              writer.WriteEndElement();

              foreach (KeyAction action in window.Buttons)
              {
                writer.WriteStartElement("action");

                writer.WriteStartElement("id");
                int id = (int)action.ActionType;
                writer.WriteString(id.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("key");
                writer.WriteString(action.Modifiers + action.KeyCode);
                writer.WriteEndElement();

                writer.WriteStartElement("ribbon");
                writer.WriteString(action.RibbonKeyCode);
                writer.WriteEndElement();

                writer.WriteStartElement("description");
                writer.WriteString(action.Description);
                writer.WriteEndElement();

                writer.WriteEndElement();
              }
              writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("Error writing Keymap.xml - {0}", ex.Message);
        return false;
      }
      log.Trace("<<<");
      return true;
    }


    /// <summary>
    ///   Loads the keymap file and creates the mapping.
    /// </summary>
    /// <returns>True if the load was successfull, false if it failed.</returns>
    private bool LoadKeyMap()
    {
      mapWindows.Clear();
      string strFilename = String.Format(@"{0}\{1}", Options.ConfigDir, "keymap.xml");
      if (!System.IO.File.Exists(strFilename))
        strFilename = String.Format(@"{0}\bin\{1}", Application.StartupPath, "keymap.xml");
      log.Info("Load key mapping from {0}", strFilename);
      try
      {
        // Load the XML file
        XmlDocument doc = new XmlDocument();
        doc.Load(strFilename);
        // Check if it is a keymap
        if (doc.DocumentElement == null) return false;
        string strRoot = doc.DocumentElement.Name;
        if (strRoot != "keymap") return false;

        // For each window
        XmlNodeList listWindows = doc.DocumentElement.SelectNodes("/keymap/window");
        foreach (XmlNode nodeWindow in listWindows)
        {
          XmlNode nodeWindowId = nodeWindow.SelectSingleNode("id");
          XmlNode nodeWindowDesc = nodeWindow.SelectSingleNode("description");
          if (null != nodeWindowId)
          {
            ActionWindow map = new ActionWindow();
            map.Window = Int32.Parse(nodeWindowId.InnerText);
            map.Description = nodeWindowDesc.InnerText;
            XmlNodeList listNodes = nodeWindow.SelectNodes("action");
            // Create a list of key/actiontype mappings
            foreach (XmlNode node in listNodes)
            {
              XmlNode nodeId = node.SelectSingleNode("id");
              XmlNode nodeKey = node.SelectSingleNode("key");
              XmlNode nodeRibbonKey = node.SelectSingleNode("ribbon");
              XmlNode nodeDesc = node.SelectSingleNode("description");
              MapAction(ref map, nodeId, nodeKey, nodeRibbonKey, nodeDesc);
            }
            if (map.Buttons.Count > 0)
            {
              mapWindows.Add(map);
            }
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("exception loading keymap {0} err:{1} stack:{2}", strFilename, ex.Message, ex.StackTrace);
        return false;
      }
      return true;
    }

    private void MapAction(ref ActionWindow map, XmlNode nodeId, XmlNode nodeKey, XmlNode nodeRibbonKey,
                           XmlNode nodeDesc)
    {
      if (null == nodeId) return;
      KeyAction but = new KeyAction();
      but.ActionType = (Action.ActionType)Int32.Parse(nodeId.InnerText);

      if (nodeDesc != null)
        but.Description = nodeDesc.InnerText;

      if (nodeKey != null)
      {
        string[] buttons = nodeKey.InnerText.Split('-');
        for (int i = 0; i < buttons.Length - 1; i++)
        {
          if (buttons[i] == "Alt")
            but.Modifiers += "Alt-";
          else if (buttons[i] == "Ctrl")
            but.Modifiers += "Ctrl-";
          else if (buttons[i] == "Shift")
            but.Modifiers += "Shift-";
        }
        but.KeyCode = buttons[buttons.Length - 1];
      }

      if (nodeRibbonKey != null)
      {
        but.RibbonKeyCode = nodeRibbonKey.InnerText;
      }

      map.Buttons.Add(but);
    }


    private void treeViewKeys_AfterSelect(object sender, TreeViewEventArgs e)
    {
      _selectedNode = (sender as TreeView).SelectedNode;
      object tag = _selectedNode.Tag;
      if (tag != null)
      {
        ckCtrl.Checked = false;
        ckAlt.Checked = false;
        ckShift.Checked = false;
        KeyAction action = (KeyAction)tag;
        tbAction.Text = Enum.GetName(typeof(Action.ActionType), action.ActionType);
        tbKeyDescription.Text = action.Description;
        tbRibbonKeyValue.Text = action.RibbonKeyCode;
        tbKeyValue.Text = action.KeyCode;
        if (action.Modifiers != null)
        {
          if (action.Modifiers.Contains("Ctrl"))
            ckCtrl.Checked = true;

          if (action.Modifiers.Contains("Alt"))
            ckAlt.Checked = true;

          if (action.Modifiers.Contains("Shift"))
            ckShift.Checked = true;
        }
      }
    }

    /// <summary>
    ///   Handle entry into Textbox for Ribbon Keys
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tbRibbonKeyValue_KeyPress(object sender, KeyPressEventArgs e)
    {
      const char Delete = (char)8;
      e.Handled = !Char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != Delete;
      if (!e.Handled)
      {
        tbRibbonKeyValue.Text = Convert.ToChar(e.KeyChar).ToString().ToUpperInvariant();
      }
    }

    private void buttonChangeKey_Click(object sender, EventArgs e)
    {
      if (tbAction.Text != "")
      {
        _keysChanged = true;
        KeyAction action = (KeyAction)_selectedNode.Tag;
        action.Modifiers = "";
        if (ckAlt.Checked)
          action.Modifiers += "Alt-";
        if (ckCtrl.Checked)
          action.Modifiers += "Ctrl-";
        if (ckShift.Checked)
          action.Modifiers += "Shift-";

        action.KeyCode = tbKeyValue.Text;
        action.RibbonKeyCode = tbRibbonKeyValue.Text;
        action.Description = tbKeyDescription.Text;
        _selectedNode.Text = string.Format("{0} ({1}) = {2}{3}", action.Description, action.ActionType, action.Modifiers,
                                           action.KeyCode);
      }
    }

    #region Nested type: ActionWindow

    private class ActionWindow
    {
      #region Variales

      private readonly List<KeyAction> mapButtons = new List<KeyAction>();

      #endregion

      #region Properties

      public int Window { get; set; }

      public List<KeyAction> Buttons
      {
        get { return mapButtons; }
      }

      public string Description { get; set; }

      #endregion
    }

    #endregion

    #region Nested type: KeyAction

    private class KeyAction
    {
      #region Variables

      #endregion

      #region Properties

      public string Modifiers { get; set; }

      public string KeyCode { get; set; }

      public string RibbonKeyCode { get; set; }

      public Action.ActionType ActionType { get; set; }

      public string Description { get; set; }

      #endregion
    }

    #endregion

    #endregion

    #region Preferences Event Handler

    #region General

    /// <summary>
    ///   A Theme has been changed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxThemes_SelectedIndexChanged(object sender, EventArgs e)
    {
      Item item = (Item)comboBoxThemes.Items[comboBoxThemes.SelectedIndex];
      Theme = (string)item.Value;
    }

    /// <summary>
    /// The Tracklist should be displayed top
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pictureBoxTrackListTop_Click(object sender, EventArgs e)
    {
      if (pictureBoxTrackListTop.Image != Resources.TrackList_top_selected)
      {
        pictureBoxTrackListTop.Image = Resources.TrackList_top_selected;
        pictureBoxTrackListBottom.Image = Resources.TrackList_bottom;
        Options.MainSettings.TrackListLocation = 0;
        PositionTrackList();
      }
    }

    /// <summary>
    /// The Tracklist should be displayed bottom
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pictureBoxTrackListBottom_Click(object sender, EventArgs e)
    {
      if (pictureBoxTrackListBottom.Image != Resources.TrackList_bottom_selected)
      {
        pictureBoxTrackListTop.Image = Resources.TrackList_top;
        pictureBoxTrackListBottom.Image = Resources.TrackList_bottom_selected;
        Options.MainSettings.TrackListLocation = 1;
        PositionTrackList();
      }
    }

    /// <summary>
    ///   Apply the Changes to the Options
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonSettingsApply_Click(object sender, EventArgs e)
    {
      log.Trace(">>>");
      bool bErrors = false;
      string message = "";

      #region General

      string currentLanguage = ServiceScope.Get<ILocalisation>().CurrentCulture.Name;
      string selectedLanguage = (string)(comboBoxLanguage.SelectedItem as Item).Value;

      // Notify about language change
      if (currentLanguage != selectedLanguage)
        ServiceScope.Get<ILocalisation>().ChangeLanguage(selectedLanguage);

      Options.MainSettings.Theme = comboBoxThemes.SelectedIndex;
      Options.MainSettings.DebugLevel = comboBoxDebugLevel.Text;
      ServiceScope.Get<ILogger>().Level = NLog.LogLevel.FromString(Options.MainSettings.DebugLevel);

      if (SaveKeyMap())
        ServiceScope.Get<IActionHandler>().LoadKeyMap();

      #endregion

      #region Ripping

      Options.MainSettings.RipEncoder = (string)(comboBoxEncoder.SelectedItem as Item).Value;
      Options.MainSettings.RipTargetFolder = tbTargetFolder.Text;
      Options.MainSettings.RipEjectCD = ckRipEjectCD.Checked;
      Options.MainSettings.RipActivateTargetFolder = ckActivateTargetFolder.Checked;

      if (!Util.CheckParameterFormat(textBoxRippingFilenameFormat.Text, Options.ParameterFormat.RipFileName))
      {
        message += localisation.ToString("settings", "InvalidParm") + "\r\n";
        bErrors = true;
      }
      else
        Options.MainSettings.RipFileNameFormat = textBoxRippingFilenameFormat.Text;

      // MP3 Encoder Settings
      Options.MainSettings.RipLamePreset = comboBoxLamePresets.SelectedIndex;
      try
      {
        Options.MainSettings.RipLameABRBitRate = comboBoxLamePresets.SelectedIndex == 4
                                                   ? Convert.ToInt32(textBoxABRBitrate.Text)
                                                   : 0;
      }
      catch (FormatException) { }
      Options.MainSettings.RipLameExpert = textBoxLameParms.Text.Trim();

      // Ogg Encoder Settings
      Options.MainSettings.RipOggQuality = hScrollBarOggEncodingQuality.Value;
      Options.MainSettings.RipOggExpert = textBoxOggParms.Text.Trim();

      // Flac Encoder Settings
      Options.MainSettings.RipFlacQuality = hScrollBarFlacEncodingQuality.Value;
      Options.MainSettings.RipFlacExpert = textBoxFlacParms.Text.Trim();

      // AAC Encoder Settings
      Options.MainSettings.RipEncoderAACBitRate = comboBoxAACBitrates.SelectedItem != null
                                                    ? comboBoxAACBitrates.SelectedItem.ToString()
                                                    : "";
      // WMA Encoder Settings
      Options.MainSettings.RipEncoderWMA = (string)(comboBoxWMAEncoderFormat.SelectedItem as Item).Value;
      Options.MainSettings.RipEncoderWMASample = (string)(comboBoxWMASampleFormat.SelectedItem as Item).Value;
      Options.MainSettings.RipEncoderWMACbrVbr = (string)(comboBoxWMACbrVbr.SelectedItem as Item).Value;
      Options.MainSettings.RipEncoderWMABitRate = comboBoxWMABitRate.SelectedItem.ToString();

      // MPC Encoder Settings
      Options.MainSettings.RipEncoderMPCPreset = (string)(comboBoxMPCPresets.SelectedItem as Item).Value;
      Options.MainSettings.RipEncoderMPCExpert = textBoxMPCParms.Text.Trim();

      // WV Encoder Settings
      Options.MainSettings.RipEncoderWVPreset = (string)(comboBoxWVPresets.SelectedItem as Item).Value;
      Options.MainSettings.RipEncoderWVExpert = textBoxWVParms.Text.Trim();

      #endregion

      #region Tags

      Options.MainSettings.CharacterEncoding = comboBoxCharacterEncoding.SelectedIndex;
      Options.MainSettings.CopyArtist = ckCopyArtistToAlbumArtist.Checked;
      Options.MainSettings.AutoFillNumberOfTracks = ckAutoFillNumberOfTracks.Checked;
      Options.MainSettings.UseCaseConversion = ckUseCaseConversionWhenSaving.Checked;
      Options.MainSettings.ChangeReadOnlyAttributte = ckChangeReadonlyAttributte.Checked;
      Options.MainSettings.CreateFolderThumb = ckCreateMissingFolderThumb.Checked;
      Options.MainSettings.EmbedFolderThumb = ckUseExistinbgThumb.Checked;
      Options.MainSettings.OverwriteExistingCovers = ckOverwriteExistingCovers.Checked;
      Options.MainSettings.OnlySaveFolderThumb = ckOnlySaveFolderThumb.Checked;
      Options.MainSettings.OverwriteExistingLyrics = ckOverwriteExistingLyrics.Checked;
      Options.MainSettings.MP3Validate = ckValidateMP3.Checked;
      Options.MainSettings.MP3AutoFix = ckAutoFixMp3.Checked;

      if (ckUseMediaPortalDatabase.Checked && System.IO.File.Exists(tbMediaPortalDatabase.Text))
      {
        Options.MainSettings.UseMediaPortalDatabase = true;
        Options.MainSettings.MediaPortalDatabase = tbMediaPortalDatabase.Text;
      }
      else
      {
        Options.MainSettings.UseMediaPortalDatabase = false;
        Options.MainSettings.MediaPortalDatabase = "";
      }

      if (radioButtonUseV3.Checked)
        Options.MainSettings.ID3V2Version = 3;
      else if (radioButtonUseV4.Checked)
        Options.MainSettings.ID3V2Version = 4;
      else
        Options.MainSettings.ID3V2Version = 0; // APE Support

      if (radioButtonID3V1.Checked)
        Options.MainSettings.ID3Version = 1;
      else if (radioButtonID3V2.Checked)
        Options.MainSettings.ID3Version = 2;
      else
        Options.MainSettings.ID3Version = 3;

      Options.MainSettings.RemoveID3V1 = checkBoxRemoveID3V1.Checked;
      Options.MainSettings.RemoveID3V2 = checkBoxRemoveID3V2.Checked;

      Options.MainSettings.SearchHotLyrics = ckHotLyrics.Checked;
      Options.MainSettings.SearchLyrics007 = ckLyrics007.Checked;
      Options.MainSettings.SearchLyricsOnDemand = ckLyricsOnDemand.Checked;
      Options.MainSettings.SearchLyricWiki = ckLyricWiki.Checked;
      Options.MainSettings.SearchLyricsPlugin = ckLyricsPlugin.Checked;
      Options.MainSettings.SearchLyrDB = ckLyrDB.Checked;
      Options.MainSettings.SearchLRCFinder = ckLRCFinder.Checked;
      Options.MainSettings.SearchActionext = ckActionext.Checked;
      Options.MainSettings.SwitchArtist = ckSwitchArtist.Checked;
      Options.MainSettings.AmazonSite = (string)(comboBoxAmazonSite.SelectedItem as Item).Value;

      #endregion

      if (bErrors)
      {
        MessageBox.Show(message, localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      }
      else
      {
        Options.SaveAllSettings();
        this.backstageView.Visible = false;
      }
      log.Trace("<<<");
    }

    /// <summary>
    ///   Cancel changes
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonSettingsCancel_Click(object sender, EventArgs e)
    {
      if (prevTheme.ThemeName != ServiceScope.Get<IThemeManager>().CurrentTheme.ThemeName)
      {
        Theme = prevTheme.ThemeName;
      }
      this.ribbon.BackstageViewVisible = false;
    }

    #endregion

    #region TabPage Ripping

    /// <summary>
    ///   Show directory browser
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonTargetFolderBrowse_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog oFD = new FolderBrowserDialog();
      oFD.RootFolder = Environment.SpecialFolder.MyDocuments;
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        tbTargetFolder.Text = oFD.SelectedPath;
      }
    }

    /// <summary>
    ///   User clicked on a parameter label. Update combo box with value.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void lblParm_Click(object sender, EventArgs e)
    {
      MPTLabel label = (MPTLabel)sender;
      int cursorPos = textBoxRippingFilenameFormat.SelectionStart;
      string text = textBoxRippingFilenameFormat.Text;

      string parameter = Util.LabelToParameter(label.Name);

      if (parameter != String.Empty)
      {
        text = text.Insert(cursorPos, parameter);
        textBoxRippingFilenameFormat.Text = text;
        if (label.Name == "lblParmFolder")
          cursorPos += 1;
        else
          cursorPos += 3;

        textBoxRippingFilenameFormat.SelectionStart = cursorPos;
      }
    }

    #region MP3

    /// <summary>
    ///   A Preset has been selected from the Combo Box. Set the narrative
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxPreset_SelectedIndexChanged(object sender, EventArgs e)
    {
      textBoxABRBitrate.Enabled = false;
      switch (comboBoxLamePresets.SelectedIndex)
      {
        case 0:
          textBoxPresetDesc.Text = localisation.ToString("settings", "DescPrefMedium");
          break;

        case 1:
          textBoxPresetDesc.Text = localisation.ToString("settings", "DescPrefStandard");
          break;

        case 2:
          textBoxPresetDesc.Text = localisation.ToString("settings", "DescPrefExtreme");
          break;

        case 3:
          textBoxPresetDesc.Text = localisation.ToString("settings", "DescPrefInsane");
          break;

        case 4:
          textBoxPresetDesc.Text = localisation.ToString("settings", "DescPrefABR");
          textBoxABRBitrate.Enabled = true;
          break;
      }
    }

    #endregion

    #region Ogg

    private void hScrollBarOggEncodingQuality_Scroll(object sender, ScrollEventArgs e)
    {
      lbOggQualitySelected.Text = hScrollBarOggEncodingQuality.Value.ToString();
    }

    #endregion

    #region Flac

    private void hScrollBarFlacEncodingQuality_Scroll(object sender, ScrollEventArgs e)
    {
      lbFlacQualitySelected.Text = hScrollBarFlacEncodingQuality.Value.ToString();
    }

    #endregion

    #region AAC

    /// <summary>
    ///   Sets the Bitrate according to the Settings / Selection
    /// </summary>
    private void SetBitRate()
    {
      foreach (string item in comboBoxAACBitrates.Items)
      {
        if (item.Contains(Options.MainSettings.RipEncoderAACBitRate))
        {
          comboBoxAACBitrates.SelectedItem = item;
          break;
        }
      }
    }

    #endregion

    #region WMA

    /// <summary>
    ///   Encoder Format has changed set the available Sample Format
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxWMAEncoderFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
      comboBoxWMACbrVbr.Items.Clear();
      switch (comboBoxWMAEncoderFormat.SelectedIndex)
      {
        case 0: // WMA Standard
          comboBoxWMACbrVbr.Items.AddRange(new[]
                                             {
                                               new Item(localisation.ToString("settings", "Cbr"), "Cbr", ""),
                                               new Item(localisation.ToString("settings", "Vbr"), "Vbr", "")
                                             });
          SetWMACbrVbr();
          break;

        case 1: // WMA Pro
          comboBoxWMACbrVbr.Items.AddRange(new[]
                                             {
                                               new Item(localisation.ToString("settings", "Cbr"), "Cbr", ""),
                                               new Item(localisation.ToString("settings", "Vbr"), "Vbr", "")
                                             });
          SetWMACbrVbr();
          break;

        case 2: // WMA LossLess
          comboBoxWMACbrVbr.Items.AddRange(new[] { new Item(localisation.ToString("settings", "Vbr"), "Vbr", "") });
          comboBoxWMACbrVbr.SelectedIndex = 0;
          break;
      }
    }

    /// <summary>
    ///   Sets the Mode according to the Settings / Selection
    /// </summary>
    private void SetWMACbrVbr()
    {
      foreach (Item item in comboBoxWMACbrVbr.Items)
      {
        if ((item.Value as string).StartsWith(Options.MainSettings.RipEncoderWMACbrVbr))
        {
          comboBoxWMACbrVbr.SelectedItem = item;
          break;
        }
      }
    }

    /// <summary>
    ///   Get the available Bitrates
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxWMACbrVbr_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetWMASampleCombo();
    }


    /// <summary>
    ///   The Sample Format has been changed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxWMASampleFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetWMABitRateCombo();
    }


    /// <summary>
    ///   Fills the Sample Format Combo box
    /// </summary>
    private void SetWMASampleCombo()
    {
      Item[] modeTab = null;
      int defaultValue = 0;
      string vbrcbr = (string)(comboBoxWMACbrVbr.SelectedItem as Item).Value;
      string encoder = (string)(comboBoxWMAEncoderFormat.SelectedItem as Item).Value;

      if (encoder == "wma")
      {
        if (vbrcbr == "Cbr")
        {
          modeTab = Options.WmaStandardSampleCBR;
          defaultValue = 10;
          _defaultBitRateIndex = 4;
        }
        else
        {
          modeTab = Options.WmaStandardSampleVBR;
          defaultValue = 0;
          _defaultBitRateIndex = 4;
        }
      }
      else if (encoder == "wmapro")
      {
        if (vbrcbr == "Cbr")
        {
          modeTab = Options.WmaProSampleCBR;
          defaultValue = 1;
          _defaultBitRateIndex = 4;
        }
        else
        {
          modeTab = Options.WmaProSampleVBR;
          defaultValue = 0;
          _defaultBitRateIndex = 4;
        }
      }
      else
      {
        // Lossless
        modeTab = Options.WmaLosslessSampleVBR;
        defaultValue = 0;
        _defaultBitRateIndex = 0;
      }
      comboBoxWMASampleFormat.Items.Clear();
      comboBoxWMASampleFormat.Items.AddRange(modeTab);

      bool found = false;
      foreach (Item item in comboBoxWMASampleFormat.Items)
      {
        if ((string)item.Value == Options.MainSettings.RipEncoderWMASample)
        {
          comboBoxWMASampleFormat.SelectedItem = item;
          found = true;
          break;
        }
      }

      if (!found)
        comboBoxWMASampleFormat.SelectedIndex = defaultValue;
    }

    /// <summary>
    ///   Fillls the Bitrate combo, according to the selection in the Sample and CbrVbr Combo
    /// </summary>
    private void SetWMABitRateCombo()
    {
      string vbrcbr;
      if (comboBoxWMACbrVbr.SelectedItem == null)
        vbrcbr = Options.MainSettings.RipEncoderWMACbrVbr;
      else
        vbrcbr = (string)(comboBoxWMACbrVbr.SelectedItem as Item).Value;

      string[] sampleFormat;
      if (comboBoxWMASampleFormat.SelectedItem == null)
        sampleFormat = Options.MainSettings.RipEncoderWMASample.Split(',');
      else
        sampleFormat = ((comboBoxWMASampleFormat.SelectedItem as Item).Value as string).Split(',');

      BASSWMAEncode encodeFlags = BASSWMAEncode.BASS_WMA_ENCODE_DEFAULT;

      string encoder = (string)(comboBoxWMAEncoderFormat.SelectedItem as Item).Value;
      if (encoder == "wmapro" || encoder == "wmalossless")
        encodeFlags = encodeFlags | BASSWMAEncode.BASS_WMA_ENCODE_PRO;
      else
        encodeFlags = encodeFlags | BASSWMAEncode.BASS_WMA_ENCODE_STANDARD;

      if (vbrcbr == "Cbr")
        encodeFlags = encodeFlags | BASSWMAEncode.BASS_WMA_ENCODE_RATES_CBR;
      else
        encodeFlags = encodeFlags | BASSWMAEncode.BASS_WMA_ENCODE_RATES_VBR;

      if (sampleFormat[0] == "24")
        encodeFlags = encodeFlags | BASSWMAEncode.BASS_WMA_ENCODE_24BIT;

      comboBoxWMABitRate.Items.Clear();
      if (encoder == "wmalossless")
      {
        comboBoxWMABitRate.Items.Add(100);
        comboBoxWMABitRate.SelectedIndex = 0;
      }
      else
      {
        int[] bitRates = BassWma.BASS_WMA_EncodeGetRates(Convert.ToInt32(sampleFormat[2]),
                                                         Convert.ToInt32(sampleFormat[1]), encodeFlags);
        for (int i = 0; i < bitRates.Length; i++)
          comboBoxWMABitRate.Items.Add(bitRates[i]);

        comboBoxWMABitRate.SelectedItem = Convert.ToInt32(Options.MainSettings.RipEncoderWMABitRate);
        if (comboBoxWMABitRate.SelectedItem == null)
        {
          if (comboBoxWMABitRate.Items.Count - 1 < _defaultBitRateIndex)
            comboBoxWMABitRate.SelectedIndex = 0;
          else
            comboBoxWMABitRate.SelectedIndex = _defaultBitRateIndex;
        }
      }
    }

    #endregion

    #endregion

    #region TabPage Tags

    /// <summary>
    ///   Offers a File Selection Duialogue to select the MediaPortal Music Database
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonMusicDatabaseBrowse_Click(object sender, EventArgs e)
    {
      OpenFileDialog oFD = new OpenFileDialog();
      oFD.Multiselect = false;
      oFD.ValidateNames = true;
      oFD.CheckFileExists = false;
      oFD.CheckPathExists = true;
      oFD.Filter = "MediaPortal DB | *.db3";
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        tbMediaPortalDatabase.Text = oFD.FileName;
      }

      if (!System.IO.File.Exists(oFD.FileName))
      {
        // THe selected dababase does not exist. Ask if we create it.
        if (MessageBox.Show(localisation.ToString("Settings", "DBNotExists"), "", MessageBoxButtons.YesNo) ==
            DialogResult.Yes)
        {
          CreateMusicDatabase(oFD.FileName);
        }
      }
    }

    private void buttonStartDatabaseScan_Click(object sender, EventArgs e)
    {
      if (checkBoxClearDatabase.Checked)
      {
        if (System.IO.File.Exists(tbMediaPortalDatabase.Text))
        {
          System.IO.File.Delete(tbMediaPortalDatabase.Text);
        }
      }

      if (!System.IO.File.Exists(tbMediaPortalDatabase.Text))
      {
        CreateMusicDatabase(tbMediaPortalDatabase.Text);
      }
      FolderBrowserDialog oFB = new FolderBrowserDialog();
      oFB.Description = localisation.ToString("Settings", "SelectMusicFolder");
      if (oFB.ShowDialog() == DialogResult.OK)
      {
        FillMusicDatabase(oFB.SelectedPath, tbMediaPortalDatabase.Text);
      }
    }

    private void buttonDBScanStatus_Click(object sender, EventArgs e)
    {
      lbDBScanStatus.Text = DatabaseScanStatus();
      lbDBScanStatus.Update();
    }

    private void radioButtonID3Both_CheckedChanged(object sender, EventArgs e)
    {
      checkBoxRemoveID3V1.Checked = false;
      checkBoxRemoveID3V1.Enabled = false;
      checkBoxRemoveID3V2.Checked = false;
      checkBoxRemoveID3V2.Enabled = false;
    }

    private void radioButtonID3V1_CheckedChanged(object sender, EventArgs e)
    {
      checkBoxRemoveID3V1.Checked = false;
      checkBoxRemoveID3V2.Checked = false;
      checkBoxRemoveID3V1.Enabled = false;
      checkBoxRemoveID3V2.Enabled = true;
    }

    private void radioButtonID3V2_CheckedChanged(object sender, EventArgs e)
    {
      checkBoxRemoveID3V1.Checked = false;
      checkBoxRemoveID3V2.Checked = false;
      checkBoxRemoveID3V1.Enabled = true;
      checkBoxRemoveID3V2.Enabled = false;
    }

    #endregion

    #endregion
    #endregion

    #region Event Handler

    #region Key Events

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      // Handle the key press, if the keyvalue field is focused in Preferences
      if (tbKeyValue.Focused)
      {
        tbKeyValue.Text = Enum.GetName(typeof(Keys), keyData);
        return true;
      }

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

      object dialog = null;
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

          dialog = new FileNameToTag.FileNameToTag(this);
          ShowDialogInDetailPanel(dialog);
          _showForm = false;
          break;

        case Action.ActionType.ACTION_TAG2FILENAME:
          if (!gridViewControl.CheckSelections(true))
            break;

          dialog = new TagToFileName.TagToFileName(this);
          ShowDialogInDetailPanel(dialog);
          _showForm = false;
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

          dialog = new OrganiseFiles(this);
          ShowDialogInDetailPanel(dialog);
          _showForm = false;
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
          this.backstageView.Visible = true;
          this.backstageView.CurrentPage = backstageViewPageOptions;
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

          dialog = new CaseConversion.CaseConversion(this);
          ShowDialogInDetailPanel(dialog);
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
        SetGalleryItem();
      }

      // Update Status Information
      try
      {
        toolStripStatusLabelFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"),
                                                       gridViewControl.View.Rows.Count,
                                                       gridViewControl.View.SelectedRows.Count);
      }
      catch (InvalidOperationException) // we might get a Cross-thread Exception on startup
      { }
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
            textBoxPresetDesc.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            textBoxPresetDesc.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
            treeViewKeys.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            treeViewKeys.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
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

    #region Ribbon Events

    #region General Events

    /// <summary>
    ///   Display a Folder Select Dialog and update the Textbox, based on the button being pressed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void FolderSelect_Executed(object sender, CommandExecutedEventArgs e)
    {
      FolderBrowserDialog oFD = new FolderBrowserDialog();
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        if (e.Invoker == buttonRipFolderSelect)
        {
          textBoxRipOutputFolder.Text = oFD.SelectedPath;
        }
        else if (e.Invoker == buttonConvertFolderSelect)
        {
          textBoxConvertOutputFolder.Text = oFD.SelectedPath;
        }
      }
    }

    #endregion

    #region TabPage Clicks

    /// <summary>
    ///   Tab Page has changed. Show the correct grid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void ribbon_CurrentTabPageChanged(object sender, EventArgs e)
    {
      //Don't do anything, if we are just initialising
      if (_initialising)
      {
        return;
      }

      if (sender == null)
      {
        return;
      }

      if ((sender as Ribbon).CurrentTabPage == null)
      {
        return;
      }

      string tabPage = (sender as Ribbon).CurrentTabPage.Tag as string;

      switch (tabPage)
      {
        case "Rip":
          {
            // Don't allow navigation, while Burning
            if (Burning)
            {
              return;
            }

            ToggleDetailPanel(false);
            TracksGridView.Hide();
            BurnGridView.Hide();
            ConvertGridView.Hide();
            RipGridView.Show();
            if (!SplitterRight.IsCollapsed)
            {
              SplitterRight.ToggleState();
            }
            break;
          }

        case "Burn":
          {
            // Don't allow navigation, while Ripping
            if (Ripping)
            {
              return;
            }

            ToggleDetailPanel(false);
            TracksGridView.Hide();
            ConvertGridView.Hide();
            RipGridView.Hide();
            BurnGridView.SetMediaInfo();
            BurnGridView.Show();
            if (!SplitterRight.IsCollapsed)
            {
              SplitterRight.ToggleState();
            }

            break;
          }

        case "Convert":
          {
            // Don't allow navigation, while Ripping or Burning
            if (Burning || Ripping)
            {
              return;
            }

            textBoxConvertOutputFolder.Text = Options.MainSettings.RipTargetFolder;

            ToggleDetailPanel(false);
            TracksGridView.Hide();
            RipGridView.Hide();
            BurnGridView.Hide();
            ConvertGridView.Show();
            if (!SplitterRight.IsCollapsed)
            {
              SplitterRight.ToggleState();
            }

            break;
          }

        default:
          {
            // Don't allow navigation, while Ripping or Burning
            if (Burning || Ripping)
            {
              return;
            }

            ToggleDetailPanel(true);
            BurnGridView.Hide();
            RipGridView.Hide();
            TracksGridView.Show();
            if (SplitterRight.IsCollapsed && !RightSplitterStatus)
            {
              SplitterRight.ToggleState();
            }

            break;
          }
      }
    }

    #endregion

    #region Application Menu & Quick Access Bar Clicks

    /// <summary>
    ///   Exit the Application
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Exit_Executed(object sender, CommandExecutedEventArgs e)
    {
      this.ribbon.BackstageViewVisible = false;
      Application.Exit();
    }

    /// <summary>
    ///   The Save button was pressed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Save_Executed(object sender, CommandExecutedEventArgs e)
    {
      this.ribbon.BackstageViewVisible = false;
      TracksGridView.Save();
    }

    /// <summary>
    ///   Refresh the Tracksgrid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Refresh_Executed(object sender, CommandExecutedEventArgs e)
    {
      this.ribbon.BackstageViewVisible = false;
      RefreshTrackList();
    }

    /// <summary>
    ///   Change Display Columns
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void ChangeDisplayColumns_Executed(object sender, CommandExecutedEventArgs e)
    {
      this.ribbon.BackstageViewVisible = false;
      Form dlg = new ColumnSelect(TracksGridView);
      ShowModalDialog(dlg);
    }

    /// <summary>
    ///   Help Button has been pressed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Help_Executed(object sender, CommandExecutedEventArgs e)
    {
      About dlgAbout = new About();
      ShowModalDialog(dlgAbout);
    }

    /// <summary>
    /// An Item has been selected in the Recent Folder List
    /// Change to that folder.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pinListRecentFolders_ItemClick(object sender, PinItemEventArgs e)
    {
      string folder = e.PinItem.DescriptionText;
      if (!Directory.Exists(folder))
      {
        pinListRecentFolders.Items.Remove(e.PinItem);
        return;
      }

      this.ribbon.BackstageViewVisible = false;
      CurrentDirectory = folder;
      TreeView.TreeView.ShowFolder(folder);
      SetRecentFolder(folder);
      RefreshTrackList();
    }

    /// <summary>
    /// An item in the navigation bar has been selected. Activate the respective Tab Page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void navigatonBarItem_Click(object sender, EventArgs e)
    {
      NavigationBarItem item = (NavigationBarItem) sender;
      if (item == navigationBarItemGeneral)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsGeneral;
      }
      else if (item == navigationBarItemKeys)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsKeys;
      }
      else if (item == navigationBarItemTagsGeneral)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsTagsGeneral;
      }
      else if (item == navigationBarItemTagsId3)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsTagsId3;
      }
      else if (item == navigationBarItemTagsLyricsCover)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsLyricsCover;
      }
      else if (item == navigationBarItemTagsDatabase)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsDatabase;
      }
      else if (item == navigationBarItemRipGeneral)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipGeneral;
      }
      else if (item == navigationBarItemRipMp3)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipMp3;
      }
      else if (item == navigationBarItemRipOgg)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipOgg;
      }
      else if (item == navigationBarItemFlac)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipFlac;
      }
      else if (item == navigationBarItemRipAAC)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipAAC;
      }
      else if (item == navigationBarItemRipWMA)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipWMA;
      }
      else if (item == navigationBarItemMPC)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipMPC;
      }
      else if (item == navigationBarItemRipWV)
      {
        tabControlSettings.SelectedTabPage = tabPageSettingsRipWV;
      }
    }

    /// <summary>
    /// The Backstage View is displayed. Make sure the preferences are set.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void backstageView_VisibleChanged(object sender, EventArgs e)
    {
      if (backstageView.Visible)
      {
        backstageView.CurrentPage = backstageViewPageRecentFolders;
        InitPreferences();
        tabControlSettings.SelectFirstTab();
      }
    }

    #endregion

    #region Tags Tab

    /// <summary>
    ///   Handle various button click events
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void TagsTabButton_Executed(object sender, CommandExecutedEventArgs e)
    {
      // If no Rows are selected, select ALL of them and do the necessary action
      if (!TracksGridView.CheckSelections(true))
      {
        return;
      }

      switch (e.Command.Name)
      {
        case "FileNameToTag":
          FileNameToTag.FileNameToTag dlgFileNameToTag = new FileNameToTag.FileNameToTag(this);
          ShowDialogInDetailPanel(dlgFileNameToTag);
          break;

        case "IdentifyFiles":
          TracksGridView.IdentifyFiles();
          break;

        case "TagFromInternet":
          InternetLookup.InternetLookup lookup = new InternetLookup.InternetLookup(this);
          lookup.SearchForAlbumInformation();
          break;

        case "GetCoverArt":
          TracksGridView.GetCoverArt();
          break;

        case "GetLyrics":
          TracksGridView.GetLyrics();
          break;

        case "AutoNumber":
          TracksGridView.AutoNumber();
          break;

        case "RemoveComment":
          TracksGridView.RemoveComments();
          break;

        case "RemoveCoverArt":
          TracksGridView.RemovePictures();
          break;

        case "OrganiseFiles":
          OrganiseFiles dlgOrganise = new OrganiseFiles(this);
          ShowDialogInDetailPanel(dlgOrganise);
          break;

        case "RenameFiles":
          TagToFileName.TagToFileName dlgTagToFile = new TagToFileName.TagToFileName(this, true);
          break;

        case "RenameFileOptions":
          TagToFileName.TagToFileName dlgTagToFileOptions = new TagToFileName.TagToFileName(this, false);
          ShowDialogInDetailPanel(dlgTagToFileOptions);
          break;

        case "CaseConversion":
          CaseConversion.CaseConversion dlgCaseConversion = new CaseConversion.CaseConversion(this, true);
          dlgCaseConversion.CaseConvertSelectedTracks();
          break;

        case "CaseConversionOptions":
          CaseConversion.CaseConversion dlgCaseConversionOptions = new CaseConversion.CaseConversion(this);
          ShowDialogInDetailPanel(dlgCaseConversionOptions);
          break;

        case "DeleteAllTags":
          TracksGridView.DeleteTags(TagTypes.AllTags);
          break;

        case "DeleteID3v1":
          TracksGridView.DeleteTags(TagTypes.Id3v1);
          break;

        case "DeleteID3v2":
          TracksGridView.DeleteTags(TagTypes.Id3v2);
          break;

        case "ScriptExecute":
          if (comboBoxScripts.SelectedIndex < 0)
            return;

          Item tag = (Item)comboBoxScripts.SelectedItem;
          TracksGridView.ExecuteScript((string)tag.Value);
          break;

        case "AddToBurner":
          TracksGridView.tracksGrid_AddToBurner(sender, new EventArgs());
          break;

        case "AddToConversion":
          TracksGridView.tracksGrid_AddToConvert(sender, new EventArgs());
          break;

        case "AddToPlaylist":
          TracksGridView.tracksGrid_AddToPlayList(sender, new EventArgs());
          break;
      }
    }

    /// <summary>
    ///   The NumberOnClick ToggleButton has been pressed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonNumberOnClick_PressedChanged(object sender, EventArgs e)
    {
      if (buttonNumberOnClick.Pressed)
      {
        _numberingOnClick = true;
      }
      else
      {
        _numberingOnClick = false;
      }
    }

    /// <summary>
    ///   Save the selected Picture as Folder THumb
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void SaveAsThumb_Executed(object sender, CommandExecutedEventArgs e)
    {
      TrackData track = TracksGridView.SelectedTrack;
      TracksGridView.SavePicture(track);
    }

    #endregion

    #region Rip Tab

    /// <summary>
    ///   Start Ripping
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void RipStart_Executed(object sender, CommandExecutedEventArgs e)
    {
      RipGridView.RipAudioCD();
    }

    private void RipCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      RipGridView.RipAudioCDCancel();
    }

    #endregion

    #region Burn Tab

    /// <summary>
    ///   Burn the selected Tracks to CD
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void BurnStart_Executed(object sender, CommandExecutedEventArgs e)
    {
      Burner burner = null;
      if (comboBoxBurner.SelectedIndex < 0)
        burner = (Burner)(comboBoxBurner.Items[0] as Item).Value;
      else
        burner = (Burner)(comboBoxBurner.SelectedItem as Item).Value;

      BurnGridView.SetActiveBurner(burner);
      BurnGridView.BurnAudioCD();
    }

    /// <summary>
    ///   Cancel burning
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void BurnCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      BurnGridView.BurnAudioCDCancel();
    }


    /// <summary>
    ///   A Burner has been selected. Set it as active Burner
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxBurner_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxBurner.Items.Count == 0)
      {
        return;
      }

      Burner burner = (Burner)(comboBoxBurner.SelectedItem as Item).Value;
      // Set Speed Table
      comboBoxBurnerSpeed.Items.Clear();
      List<string> speeds = burner.SupportedDriveSpeed;
      if (speeds.Count > 0)
      {
        Item item = new Item("Maximum", Convert.ToInt32(speeds[0].Trim(new[] { 'x' })));
        comboBoxBurnerSpeed.Items.Add(item);
        burner.SelectedWriteSpeed = (int)item.Value;
        foreach (string speed in speeds)
        {
          item = new Item(speed, Convert.ToInt32(speed.Trim(new[] { 'x' })));
          comboBoxBurnerSpeed.Items.Add(item);
        }
      }
      if (comboBoxBurnerSpeed.Items.Count > 0)
      {
        comboBoxBurnerSpeed.SelectedIndex = 0;
      }

      BurnGridView.SetActiveBurner(burner);
    }

    /// <summary>
    ///   The Speed has been selected. Set it for the active Burner
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxBurnerSpeed_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxBurner.Items.Count == 0 || comboBoxBurnerSpeed.Items.Count == 0)
      {
        return;
      }

      Burner burner = (Burner)(comboBoxBurner.SelectedItem as Item).Value;
      burner.SelectedWriteSpeed = (int)(comboBoxBurnerSpeed.SelectedItem as Item).Value;
    }

    #endregion

    #region Convert Tab

    private void ConvertStart_Executed(object sender, CommandExecutedEventArgs e)
    {
      ConvertGridView.ConvertFiles();
    }

    private void ConvertCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      ConvertGridView.ConvertFilesCancel();
    }

    #endregion

    #region Gallery

    private void galleryPicture_HoveredItemChanged(object sender, GalleryHoveredItemChangedEventArgs e)
    {
      if (sender == null)
      {
        return;
      }

      if (e.NewValue != null)
      {
        _displayedGalleryItem = (GalleryItem)e.NewValue;
        if (picControl != null && picControl.Text != "")
        {
          picControl.Close();
        }
        picControl = new PictureControl((GalleryItem)e.NewValue, (sender as Gallery).PopupOwnerBounds.Location);
      }
      else
      {
        if (e.OldValue != null && e.OldValue == _displayedGalleryItem)
        {
          return;
        }
        if (picControl != null)
        {
          picControl.Close();
        }
      }
    }

    #endregion   

    #endregion

    #endregion
  }
}