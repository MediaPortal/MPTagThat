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
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Elegant.Ui;
using MPTagThat.Core;
using MPTagThat.Core.Burning;
using MPTagThat.Core.MediaChangeMonitor;
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

      /// Add the Ribbon Control to the Top Panel
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
      ApplicationCommands.Options.Executed += Options_Executed;
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

      buttonGetCoverArt.ScreenTip.Caption = localisation.ToString("screentip", "GetCoverArt");
      buttonGetCoverArt.ScreenTip.Text = localisation.ToString("screentip", "GetCoverArtText");

      buttonSaveAsThumb.ScreenTip.Caption = localisation.ToString("screentip", "SaveFolderThumb");
      buttonSaveAsThumb.ScreenTip.Text = localisation.ToString("screentip", "SaveFolderThumbText");

      buttonGetLyrics.ScreenTip.Caption = localisation.ToString("screentip", "GetLyrics");
      buttonGetLyrics.ScreenTip.Text = localisation.ToString("screentip", "GetLyricsText");

      buttonAutoNumber.ScreenTip.Caption = localisation.ToString("screentip", "AutoNumber");
      buttonAutoNumber.ScreenTip.Text = localisation.ToString("screentip", "AutoNumberText");

      buttonNumberOnClick.ScreenTip.Caption = localisation.ToString("screentip", "NumberOnClick");
      buttonNumberOnClick.ScreenTip.Text = localisation.ToString("screentip", "NumberOnClickText");

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
            Hide();
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
    ///   Show the Options Panel
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Options_Executed(object sender, CommandExecutedEventArgs e)
    {
      this.ribbon.BackstageViewVisible = false;
      Preferences.Preferences dlgPreferences = new Preferences.Preferences(this);
      ShowModalDialog(dlgPreferences);
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