using System.Windows.Forms;

namespace MPTagThat
{
  partial class Main
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      Elegant.Ui.ThemeSelector themeSelector;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.panelBottom = new System.Windows.Forms.Panel();
      this.playerPanel = new MPTagThat.Core.WinControls.MPTPanel();
      this.panelMiddle = new MPTagThat.Core.WinControls.MPTPanel();
      this.panelMiddleTop = new MPTagThat.Core.WinControls.MPTPanel();
      this.panelFileList = new MPTagThat.Core.WinControls.MPTPanel();
      this.splitterTop = new NJFLib.Controls.CollapsibleSplitter();
      this.panelMiddleDBSearch = new MPTagThat.Core.WinControls.MPTPanel();
      this.splitterLeft = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelLeft = new MPTagThat.Core.WinControls.MPTPanel();
      this.panelLeftTop = new MPTagThat.Core.WinControls.MPTPanel();
      this.splitterRight = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelRight = new MPTagThat.Core.WinControls.MPTPanel();
      this.splitterBottom = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelMiddleBottom = new MPTagThat.Core.WinControls.MPTPanel();
      this.splitterPlayer = new NJFLib.Controls.CollapsibleSplitter();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.formFrameSkinner = new Elegant.Ui.FormFrameSkinner();
      this.statusBar = new Elegant.Ui.StatusBar();
      this.statusBarNotificationsArea1 = new Elegant.Ui.StatusBarNotificationsArea();
      this.statusBarPane2 = new Elegant.Ui.StatusBarPane();
      this.toolStripStatusLabelFiles = new Elegant.Ui.Label();
      this.toolStripStatusLabelFilter = new Elegant.Ui.Label();
      this.statusBarPane3 = new Elegant.Ui.StatusBarPane();
      this.toolStripStatusLabelFolder = new Elegant.Ui.Label();
      this.statusBarControlsArea1 = new Elegant.Ui.StatusBarControlsArea();
      this.statusBarPane4 = new Elegant.Ui.StatusBarPane();
      this.toolStripStatusLabelScanProgress = new Elegant.Ui.Label();
      this.statusBarPane1 = new Elegant.Ui.StatusBarPane();
      this.progressBar1 = new Elegant.Ui.ProgressBar();
      this.buttonProgressCancel = new Elegant.Ui.Button();
      this.ribbon = new Elegant.Ui.Ribbon();
      this.backstageView = new Elegant.Ui.BackstageView();
      this.backstageViewPageOptions = new Elegant.Ui.BackstageViewPage();
      this.panel2 = new Elegant.Ui.Panel();
      this.tabControlSettings = new Elegant.Ui.TabControl();
      this.tabPageSettingsGeneral = new Elegant.Ui.TabPage();
      this.groupBoxGeneral = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbTracklistLocation = new MPTagThat.Core.WinControls.MPTLabel();
      this.pictureBoxTrackListTop = new System.Windows.Forms.PictureBox();
      this.pictureBoxTrackListBottom = new System.Windows.Forms.PictureBox();
      this.comboBoxDebugLevel = new MPTagThat.Core.WinControls.MPTComboBox();
      this.lbDebugLevel = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbTheme = new MPTagThat.Core.WinControls.MPTLabel();
      this.comboBoxThemes = new MPTagThat.Core.WinControls.MPTComboBox();
      this.comboBoxLanguage = new MPTagThat.Core.WinControls.MPTComboBox();
      this.lbLanguage = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsKeys = new Elegant.Ui.TabPage();
      this.groupBoxKeys = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.tbRibbonKeyValue = new System.Windows.Forms.TextBox();
      this.lblRibbonShortCut = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblKeyShortCut = new MPTagThat.Core.WinControls.MPTLabel();
      this.buttonChangeKey = new MPTagThat.Core.WinControls.MPTButton();
      this.tbKeyValue = new System.Windows.Forms.TextBox();
      this.ttLabel1 = new MPTagThat.Core.WinControls.MPTLabel();
      this.ckShift = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckCtrl = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckAlt = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.tbKeyDescription = new System.Windows.Forms.TextBox();
      this.lbKeyDescription = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbAction = new System.Windows.Forms.TextBox();
      this.lbKeyAction = new MPTagThat.Core.WinControls.MPTLabel();
      this.treeViewKeys = new System.Windows.Forms.TreeView();
      this.tabPageSettingsTagsGeneral = new Elegant.Ui.TabPage();
      this.groupBoxTagsGeneral = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.ckChangeReadonlyAttributte = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckAutoFillNumberOfTracks = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckUseCaseConversionWhenSaving = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckCopyArtistToAlbumArtist = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.tabPageSettingsTagsId3 = new Elegant.Ui.TabPage();
      this.groupBoxTagValidate = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.ckAutoFixMp3 = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckValidateMP3 = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.groupBoxTagsID3 = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.comboBoxCharacterEncoding = new Elegant.Ui.ComboBox();
      this.radioButtonUseApe = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.lbCharacterEncoding = new MPTagThat.Core.WinControls.MPTLabel();
      this.radioButtonUseV4 = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.radioButtonUseV3 = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.groupBoxID3Update = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.checkBoxRemoveID3V1 = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxRemoveID3V2 = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.radioButtonID3Both = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.radioButtonID3V2 = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.radioButtonID3V1 = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.tabPageSettingsLyricsCover = new Elegant.Ui.TabPage();
      this.groupBoxPictures = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.ckOnlySaveFolderThumb = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.comboBoxAmazonSite = new MPTagThat.Core.WinControls.MPTComboBox();
      this.lbAmazonSearchSite = new MPTagThat.Core.WinControls.MPTLabel();
      this.ckUseExistinbgThumb = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckOverwriteExistingCovers = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckCreateMissingFolderThumb = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.groupBoxLyrics = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.ckOverwriteExistingLyrics = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckSwitchArtist = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.groupBoxLyricsSites = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.ckLRCFinder = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckLyrDB = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckActionext = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckLyricsPlugin = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckLyricsOnDemand = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckLyrics007 = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckHotLyrics = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckLyricWiki = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.tabPageSettingsDatabase = new Elegant.Ui.TabPage();
      this.groupBoxDatabaseBuild = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.checkBoxClearDatabase = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.lbDBScanStatus = new MPTagThat.Core.WinControls.MPTLabel();
      this.buttonDBScanStatus = new MPTagThat.Core.WinControls.MPTButton();
      this.buttonStartDatabaseScan = new MPTagThat.Core.WinControls.MPTButton();
      this.lbDatabaseNote = new MPTagThat.Core.WinControls.MPTLabel();
      this.groubBoxTagsDatabase = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.buttonMusicDatabaseBrowse = new MPTagThat.Core.WinControls.MPTButton();
      this.tbMediaPortalDatabase = new System.Windows.Forms.TextBox();
      this.ckUseMediaPortalDatabase = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.tabPageSettingsRipGeneral = new Elegant.Ui.TabPage();
      this.groupBoxEncoding = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.comboBoxEncoder = new MPTagThat.Core.WinControls.MPTComboBox();
      this.lbEncodingFormat = new MPTagThat.Core.WinControls.MPTLabel();
      this.groupBoxRippingOptions = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.ckActivateTargetFolder = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.ckRipEjectCD = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.groupBoxCustomPath = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.groupBoxRippingFormatOptions = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lblParmFolder = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblAlbumArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmGenre = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmTrack = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmYear = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.textBoxRippingFilenameFormat = new System.Windows.Forms.TextBox();
      this.lbFormat = new MPTagThat.Core.WinControls.MPTLabel();
      this.buttonTargetFolderBrowse = new MPTagThat.Core.WinControls.MPTButton();
      this.tbTargetFolder = new System.Windows.Forms.TextBox();
      this.lbTargetFolder = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsRipMp3 = new Elegant.Ui.TabPage();
      this.groupBoxPresets = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.textBoxPresetDesc = new System.Windows.Forms.TextBox();
      this.textBoxABRBitrate = new System.Windows.Forms.TextBox();
      this.lbABRBitrate = new MPTagThat.Core.WinControls.MPTLabel();
      this.comboBoxLamePresets = new MPTagThat.Core.WinControls.MPTComboBox();
      this.lbPreset = new MPTagThat.Core.WinControls.MPTLabel();
      this.groupBoxMp3Experts = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbLameExpertsWarning = new MPTagThat.Core.WinControls.MPTLabel();
      this.textBoxLameParms = new System.Windows.Forms.TextBox();
      this.lbLameExpertOptions = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsRipOgg = new Elegant.Ui.TabPage();
      this.groupBoxOggEncoding = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbOggQualitySelected = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbOggQuality = new MPTagThat.Core.WinControls.MPTLabel();
      this.hScrollBarOggEncodingQuality = new System.Windows.Forms.HScrollBar();
      this.groupBoxOggExpert = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbOggExpertWarning = new MPTagThat.Core.WinControls.MPTLabel();
      this.textBoxOggParms = new System.Windows.Forms.TextBox();
      this.lbOggExpert = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsRipFlac = new Elegant.Ui.TabPage();
      this.groupBoxFlacEncoding = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbFlacQualitySelected = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbFlacQuality = new MPTagThat.Core.WinControls.MPTLabel();
      this.hScrollBarFlacEncodingQuality = new System.Windows.Forms.HScrollBar();
      this.groupBoxFlacSettings = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbFlacExpertsWarning = new MPTagThat.Core.WinControls.MPTLabel();
      this.textBoxFlacParms = new System.Windows.Forms.TextBox();
      this.lbFlacExperts = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsRipAAC = new Elegant.Ui.TabPage();
      this.groupBoxAACEncoding = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.comboBoxAACBitrates = new MPTagThat.Core.WinControls.MPTComboBox();
      this.labelAACBitrate = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsRipWMA = new Elegant.Ui.TabPage();
      this.groupBoxWMAEncoding = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.comboBoxWMABitRate = new MPTagThat.Core.WinControls.MPTComboBox();
      this.comboBoxWMACbrVbr = new MPTagThat.Core.WinControls.MPTComboBox();
      this.labelWMAQuality = new MPTagThat.Core.WinControls.MPTLabel();
      this.comboBoxWMASampleFormat = new MPTagThat.Core.WinControls.MPTComboBox();
      this.labelWMASampleFormat = new MPTagThat.Core.WinControls.MPTLabel();
      this.comboBoxWMAEncoderFormat = new MPTagThat.Core.WinControls.MPTComboBox();
      this.labelWMAEncoderFormat = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsRipMPC = new Elegant.Ui.TabPage();
      this.groupBoxMPCExpert = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbMPCExpertsWarning = new MPTagThat.Core.WinControls.MPTLabel();
      this.textBoxMPCParms = new System.Windows.Forms.TextBox();
      this.lbMPCExperts = new MPTagThat.Core.WinControls.MPTLabel();
      this.groupBoxMPCPresets = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.comboBoxMPCPresets = new MPTagThat.Core.WinControls.MPTComboBox();
      this.lbMPCPresets = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabPageSettingsRipWV = new Elegant.Ui.TabPage();
      this.groupBoxWVExpertSettings = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbWVExpertWarning = new MPTagThat.Core.WinControls.MPTLabel();
      this.textBoxWVParms = new System.Windows.Forms.TextBox();
      this.lbWVExpert = new MPTagThat.Core.WinControls.MPTLabel();
      this.groupBoxWVPresets = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.comboBoxWVPresets = new MPTagThat.Core.WinControls.MPTComboBox();
      this.lbWVPreset = new MPTagThat.Core.WinControls.MPTLabel();
      this.panel3 = new Elegant.Ui.Panel();
      this.buttonSettingsCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.buttonSettingsApply = new MPTagThat.Core.WinControls.MPTButton();
      this.backstageViewPanel1 = new Elegant.Ui.BackstageViewPanel();
      this.groupedNavigationBar1 = new Elegant.Ui.GroupedNavigationBar();
      this.navigationBarGroupGeneral = new Elegant.Ui.NavigationBarGroup();
      this.navigationBarGroupItemsContainer1 = new Elegant.Ui.NavigationBarGroupItemsContainer();
      this.navigationBarItemGeneral = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemKeys = new Elegant.Ui.NavigationBarItem();
      this.navigationBarGroupTags = new Elegant.Ui.NavigationBarGroup();
      this.navigationBarGroupItemsContainer2 = new Elegant.Ui.NavigationBarGroupItemsContainer();
      this.navigationBarItemTagsGeneral = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemTagsId3 = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemTagsLyricsCover = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemTagsDatabase = new Elegant.Ui.NavigationBarItem();
      this.navigationBarGroupRipConvert = new Elegant.Ui.NavigationBarGroup();
      this.navigationBarGroupItemsContainer3 = new Elegant.Ui.NavigationBarGroupItemsContainer();
      this.navigationBarItemRipGeneral = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemRipMp3 = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemRipOgg = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemFlac = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemRipAAC = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemRipWMA = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemMPC = new Elegant.Ui.NavigationBarItem();
      this.navigationBarItemRipWV = new Elegant.Ui.NavigationBarItem();
      this.backstageViewButtonSave = new Elegant.Ui.BackstageViewButton();
      this.backstageViewButtonRefresh = new Elegant.Ui.BackstageViewButton();
      this.backstageViewPageRecentFolders = new Elegant.Ui.BackstageViewPage();
      this.panel1 = new Elegant.Ui.Panel();
      this.separatorRecentFolders = new Elegant.Ui.Separator();
      this.pinListRecentFolders = new Elegant.Ui.PinList();
      this.backstageViewSeparator1 = new Elegant.Ui.BackstageViewSeparator();
      this.backstageViewButtonChangeColumns = new Elegant.Ui.BackstageViewButton();
      this.backstageViewSeparator2 = new Elegant.Ui.BackstageViewSeparator();
      this.backstageViewButtonExit = new Elegant.Ui.BackstageViewButton();
      this.ribbonTabPageTag = new Elegant.Ui.RibbonTabPage();
      this.ribbonGroupTagsRetrieve = new Elegant.Ui.RibbonGroup();
      this.buttonTagFromFile = new Elegant.Ui.Button();
      this.buttonTagIdentifyFiles = new Elegant.Ui.Button();
      this.buttonTagFromInternet = new Elegant.Ui.Button();
      this.ribbonGroupTagsEdit = new Elegant.Ui.RibbonGroup();
      this.buttonCaseConversion = new Elegant.Ui.SplitButton();
      this.popupMenu3 = new Elegant.Ui.PopupMenu(this.components);
      this.buttonCaseConversionOptions = new Elegant.Ui.Button();
      this.buttonGetLyrics = new Elegant.Ui.Button();
      this.separator2 = new Elegant.Ui.Separator();
      this.buttonDeleteTag = new Elegant.Ui.SplitButton();
      this.popupMenu2 = new Elegant.Ui.PopupMenu(this.components);
      this.buttonDeleteAllTags = new Elegant.Ui.Button();
      this.buttonDeleteID3v1 = new Elegant.Ui.Button();
      this.buttonDeleteID3v2 = new Elegant.Ui.Button();
      this.buttonRemoveComment = new Elegant.Ui.Button();
      this.separator3 = new Elegant.Ui.Separator();
      this.buttonGroup1 = new Elegant.Ui.ButtonGroup();
      this.comboBoxScripts = new Elegant.Ui.ComboBox();
      this.buttonScriptExecute = new Elegant.Ui.Button();
      this.buttonGroup3 = new Elegant.Ui.ButtonGroup();
      this.buttonNumberOnClick = new Elegant.Ui.ToggleButton();
      this.buttonAutoNumber = new Elegant.Ui.Button();
      this.textBoxNumber = new Elegant.Ui.TextBox();
      this.ribbonGroupPicture = new Elegant.Ui.RibbonGroup();
      this.galleryPicture = new Elegant.Ui.Gallery();
      this.buttonGetCoverArt = new Elegant.Ui.Button();
      this.buttonRemoveCoverArt = new Elegant.Ui.Button();
      this.buttonSaveAsThumb = new Elegant.Ui.Button();
      this.ribbonGroupOrganise = new Elegant.Ui.RibbonGroup();
      this.buttonRenameFiles = new Elegant.Ui.SplitButton();
      this.popupMenu1 = new Elegant.Ui.PopupMenu(this.components);
      this.buttonRenameFilesOptions = new Elegant.Ui.Button();
      this.buttonOrganiseFiles = new Elegant.Ui.Button();
      this.ribbonGroupOther = new Elegant.Ui.RibbonGroup();
      this.buttonAddToBurner = new Elegant.Ui.Button();
      this.buttonAddToConversion = new Elegant.Ui.Button();
      this.buttonAddToPlaylist = new Elegant.Ui.Button();
      this.startMenuSave = new Elegant.Ui.Button();
      this.startMenuRefresh = new Elegant.Ui.Button();
      this.ribbonTabPageRip = new Elegant.Ui.RibbonTabPage();
      this.ribbonGroupRip = new Elegant.Ui.RibbonGroup();
      this.buttonRipStart = new Elegant.Ui.Button();
      this.buttonRipCancel = new Elegant.Ui.Button();
      this.ribbonGroupRipOptions = new Elegant.Ui.RibbonGroup();
      this.textBoxRipOutputFolder = new Elegant.Ui.TextBox();
      this.comboBoxRipEncoder = new Elegant.Ui.ComboBox();
      this.buttonRipFolderSelect = new Elegant.Ui.Button();
      this.ribbonTabPageConvert = new Elegant.Ui.RibbonTabPage();
      this.ribbonGroupConvert = new Elegant.Ui.RibbonGroup();
      this.buttonConvertStart = new Elegant.Ui.Button();
      this.buttonConvertCancel = new Elegant.Ui.Button();
      this.ribbonGroupConvertOptions = new Elegant.Ui.RibbonGroup();
      this.textBoxConvertOutputFolder = new Elegant.Ui.TextBox();
      this.comboBoxConvertEncoder = new Elegant.Ui.ComboBox();
      this.buttonConvertFolderSelect = new Elegant.Ui.Button();
      this.ribbonTabPageBurn = new Elegant.Ui.RibbonTabPage();
      this.ribbonGroupBurn = new Elegant.Ui.RibbonGroup();
      this.buttonBurnStart = new Elegant.Ui.Button();
      this.buttonBurnCancel = new Elegant.Ui.Button();
      this.ribbonGroupBurnOptions = new Elegant.Ui.RibbonGroup();
      this.comboBoxBurner = new Elegant.Ui.ComboBox();
      this.comboBoxBurnerSpeed = new Elegant.Ui.ComboBox();
      themeSelector = new Elegant.Ui.ThemeSelector(this.components);
      this.panelBottom.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panelMiddleTop.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.statusBar.SuspendLayout();
      this.statusBarNotificationsArea1.SuspendLayout();
      this.statusBarPane2.SuspendLayout();
      this.statusBarPane3.SuspendLayout();
      this.statusBarControlsArea1.SuspendLayout();
      this.statusBarPane4.SuspendLayout();
      this.statusBarPane1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.backstageView)).BeginInit();
      this.backstageViewPageOptions.SuspendLayout();
      this.panel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tabControlSettings)).BeginInit();
      this.tabPageSettingsGeneral.SuspendLayout();
      this.groupBoxGeneral.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrackListTop)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrackListBottom)).BeginInit();
      this.tabPageSettingsKeys.SuspendLayout();
      this.groupBoxKeys.SuspendLayout();
      this.tabPageSettingsTagsGeneral.SuspendLayout();
      this.groupBoxTagsGeneral.SuspendLayout();
      this.tabPageSettingsTagsId3.SuspendLayout();
      this.groupBoxTagValidate.SuspendLayout();
      this.groupBoxTagsID3.SuspendLayout();
      this.groupBoxID3Update.SuspendLayout();
      this.tabPageSettingsLyricsCover.SuspendLayout();
      this.groupBoxPictures.SuspendLayout();
      this.groupBoxLyrics.SuspendLayout();
      this.groupBoxLyricsSites.SuspendLayout();
      this.tabPageSettingsDatabase.SuspendLayout();
      this.groupBoxDatabaseBuild.SuspendLayout();
      this.groubBoxTagsDatabase.SuspendLayout();
      this.tabPageSettingsRipGeneral.SuspendLayout();
      this.groupBoxEncoding.SuspendLayout();
      this.groupBoxRippingOptions.SuspendLayout();
      this.groupBoxCustomPath.SuspendLayout();
      this.groupBoxRippingFormatOptions.SuspendLayout();
      this.tabPageSettingsRipMp3.SuspendLayout();
      this.groupBoxPresets.SuspendLayout();
      this.groupBoxMp3Experts.SuspendLayout();
      this.tabPageSettingsRipOgg.SuspendLayout();
      this.groupBoxOggEncoding.SuspendLayout();
      this.groupBoxOggExpert.SuspendLayout();
      this.tabPageSettingsRipFlac.SuspendLayout();
      this.groupBoxFlacEncoding.SuspendLayout();
      this.groupBoxFlacSettings.SuspendLayout();
      this.tabPageSettingsRipAAC.SuspendLayout();
      this.groupBoxAACEncoding.SuspendLayout();
      this.tabPageSettingsRipWMA.SuspendLayout();
      this.groupBoxWMAEncoding.SuspendLayout();
      this.tabPageSettingsRipMPC.SuspendLayout();
      this.groupBoxMPCExpert.SuspendLayout();
      this.groupBoxMPCPresets.SuspendLayout();
      this.tabPageSettingsRipWV.SuspendLayout();
      this.groupBoxWVExpertSettings.SuspendLayout();
      this.groupBoxWVPresets.SuspendLayout();
      this.panel3.SuspendLayout();
      this.backstageViewPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.groupedNavigationBar1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.navigationBarGroupItemsContainer1)).BeginInit();
      this.navigationBarGroupItemsContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.navigationBarGroupItemsContainer2)).BeginInit();
      this.navigationBarGroupItemsContainer2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.navigationBarGroupItemsContainer3)).BeginInit();
      this.navigationBarGroupItemsContainer3.SuspendLayout();
      this.backstageViewPageRecentFolders.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pinListRecentFolders)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageTag)).BeginInit();
      this.ribbonTabPageTag.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupTagsRetrieve)).BeginInit();
      this.ribbonGroupTagsRetrieve.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupTagsEdit)).BeginInit();
      this.ribbonGroupTagsEdit.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
      this.buttonGroup1.SuspendLayout();
      this.buttonGroup3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupPicture)).BeginInit();
      this.ribbonGroupPicture.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.galleryPicture)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupOrganise)).BeginInit();
      this.ribbonGroupOrganise.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupOther)).BeginInit();
      this.ribbonGroupOther.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageRip)).BeginInit();
      this.ribbonTabPageRip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupRip)).BeginInit();
      this.ribbonGroupRip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupRipOptions)).BeginInit();
      this.ribbonGroupRipOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageConvert)).BeginInit();
      this.ribbonTabPageConvert.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupConvert)).BeginInit();
      this.ribbonGroupConvert.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupConvertOptions)).BeginInit();
      this.ribbonGroupConvertOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageBurn)).BeginInit();
      this.ribbonTabPageBurn.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupBurn)).BeginInit();
      this.ribbonGroupBurn.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupBurnOptions)).BeginInit();
      this.ribbonGroupBurnOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelBottom
      // 
      this.panelBottom.Controls.Add(this.playerPanel);
      this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelBottom.Location = new System.Drawing.Point(0, 1077);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new System.Drawing.Size(1008, 90);
      this.panelBottom.TabIndex = 12;
      // 
      // playerPanel
      // 
      this.playerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.playerPanel.Location = new System.Drawing.Point(0, 0);
      this.playerPanel.Name = "playerPanel";
      this.playerPanel.Size = new System.Drawing.Size(1008, 90);
      this.playerPanel.TabIndex = 11;
      // 
      // panelMiddle
      // 
      this.panelMiddle.Controls.Add(this.panelMiddleTop);
      this.panelMiddle.Controls.Add(this.splitterLeft);
      this.panelMiddle.Controls.Add(this.splitterRight);
      this.panelMiddle.Controls.Add(this.splitterBottom);
      this.panelMiddle.Controls.Add(this.panelMiddleBottom);
      this.panelMiddle.Controls.Add(this.panelLeft);
      this.panelMiddle.Controls.Add(this.panelRight);
      this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddle.Location = new System.Drawing.Point(0, 153);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(1008, 921);
      this.panelMiddle.TabIndex = 10;
      // 
      // panelMiddleTop
      // 
      this.panelMiddleTop.Controls.Add(this.panelFileList);
      this.panelMiddleTop.Controls.Add(this.splitterTop);
      this.panelMiddleTop.Controls.Add(this.panelMiddleDBSearch);
      this.panelMiddleTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddleTop.Location = new System.Drawing.Point(158, 0);
      this.panelMiddleTop.Name = "panelMiddleTop";
      this.panelMiddleTop.Size = new System.Drawing.Size(676, 649);
      this.panelMiddleTop.TabIndex = 11;
      // 
      // panelFileList
      // 
      this.panelFileList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelFileList.Location = new System.Drawing.Point(0, 88);
      this.panelFileList.Name = "panelFileList";
      this.panelFileList.Size = new System.Drawing.Size(676, 561);
      this.panelFileList.TabIndex = 9;
      // 
      // splitterTop
      // 
      this.splitterTop.AnimationDelay = 20;
      this.splitterTop.AnimationStep = 20;
      this.splitterTop.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterTop.ControlToHide = this.panelMiddleDBSearch;
      this.splitterTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.splitterTop.ExpandParentForm = false;
      this.splitterTop.Location = new System.Drawing.Point(0, 80);
      this.splitterTop.Name = "splitterTop";
      this.splitterTop.TabIndex = 11;
      this.splitterTop.TabStop = false;
      this.splitterTop.UseAnimations = true;
      this.splitterTop.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      // 
      // panelMiddleDBSearch
      // 
      this.panelMiddleDBSearch.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelMiddleDBSearch.Location = new System.Drawing.Point(0, 0);
      this.panelMiddleDBSearch.Name = "panelMiddleDBSearch";
      this.panelMiddleDBSearch.Size = new System.Drawing.Size(676, 80);
      this.panelMiddleDBSearch.TabIndex = 10;
      // 
      // splitterLeft
      // 
      this.splitterLeft.AnimationDelay = 20;
      this.splitterLeft.AnimationStep = 20;
      this.splitterLeft.BackColor = System.Drawing.SystemColors.Control;
      this.splitterLeft.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterLeft.ControlToHide = this.panelLeft;
      this.splitterLeft.ExpandParentForm = false;
      this.splitterLeft.Localisation = "splitterLeft";
      this.splitterLeft.LocalisationContext = "Main";
      this.splitterLeft.Location = new System.Drawing.Point(150, 0);
      this.splitterLeft.Name = "splitterLeft";
      this.splitterLeft.TabIndex = 2;
      this.splitterLeft.TabStop = false;
      this.splitterLeft.UseAnimations = false;
      this.splitterLeft.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      // 
      // panelLeft
      // 
      this.panelLeft.Controls.Add(this.panelLeftTop);
      this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.panelLeft.Location = new System.Drawing.Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new System.Drawing.Size(150, 921);
      this.panelLeft.TabIndex = 1;
      // 
      // panelLeftTop
      // 
      this.panelLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelLeftTop.Location = new System.Drawing.Point(0, 0);
      this.panelLeftTop.Name = "panelLeftTop";
      this.panelLeftTop.Size = new System.Drawing.Size(150, 921);
      this.panelLeftTop.TabIndex = 4;
      // 
      // splitterRight
      // 
      this.splitterRight.AnimationDelay = 20;
      this.splitterRight.AnimationStep = 20;
      this.splitterRight.BackColor = System.Drawing.SystemColors.Control;
      this.splitterRight.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterRight.ControlToHide = this.panelRight;
      this.splitterRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.splitterRight.ExpandParentForm = false;
      this.splitterRight.Localisation = "splitterRight";
      this.splitterRight.LocalisationContext = "Main";
      this.splitterRight.Location = new System.Drawing.Point(834, 0);
      this.splitterRight.Name = "splitterRight";
      this.splitterRight.TabIndex = 4;
      this.splitterRight.TabStop = false;
      this.splitterRight.UseAnimations = false;
      this.splitterRight.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      this.splitterRight.Click += new System.EventHandler(this.splitterRight_Click);
      // 
      // panelRight
      // 
      this.panelRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panelRight.BackColor = System.Drawing.SystemColors.Control;
      this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelRight.Location = new System.Drawing.Point(842, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new System.Drawing.Size(166, 921);
      this.panelRight.TabIndex = 3;
      // 
      // splitterBottom
      // 
      this.splitterBottom.AnimationDelay = 20;
      this.splitterBottom.AnimationStep = 20;
      this.splitterBottom.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterBottom.ControlToHide = this.panelMiddleBottom;
      this.splitterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.splitterBottom.ExpandParentForm = false;
      this.splitterBottom.Localisation = "collapsibleSplitter1";
      this.splitterBottom.LocalisationContext = "Main";
      this.splitterBottom.Location = new System.Drawing.Point(150, 649);
      this.splitterBottom.Name = "collapsibleSplitter1";
      this.splitterBottom.TabIndex = 6;
      this.splitterBottom.TabStop = false;
      this.splitterBottom.UseAnimations = false;
      this.splitterBottom.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      this.splitterBottom.Click += new System.EventHandler(this.splitterBottom_Click);
      // 
      // panelMiddleBottom
      // 
      this.panelMiddleBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelMiddleBottom.Location = new System.Drawing.Point(150, 657);
      this.panelMiddleBottom.Name = "panelMiddleBottom";
      this.panelMiddleBottom.Size = new System.Drawing.Size(692, 264);
      this.panelMiddleBottom.TabIndex = 12;
      // 
      // splitterPlayer
      // 
      this.splitterPlayer.AnimationDelay = 1;
      this.splitterPlayer.AnimationStep = 50;
      this.splitterPlayer.BorderStyle3D = System.Windows.Forms.Border3DStyle.RaisedOuter;
      this.splitterPlayer.ControlToHide = this.panelBottom;
      this.splitterPlayer.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.splitterPlayer.ExpandParentForm = false;
      this.splitterPlayer.Location = new System.Drawing.Point(0, 1074);
      this.splitterPlayer.Name = "splitterPlayer";
      this.splitterPlayer.TabIndex = 13;
      this.splitterPlayer.TabStop = false;
      this.splitterPlayer.UseAnimations = true;
      this.splitterPlayer.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.HeaderText = "File";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Width = 300;
      // 
      // dataGridViewTextBoxColumn2
      // 
      this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.HeaderText = "Message";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      // 
      // formFrameSkinner
      // 
      this.formFrameSkinner.Form = this;
      this.formFrameSkinner.TitleFont = new System.Drawing.Font("Tahoma", 9F);
      // 
      // statusBar
      // 
      this.statusBar.Controls.Add(this.statusBarNotificationsArea1);
      this.statusBar.Controls.Add(this.statusBarControlsArea1);
      this.statusBar.ControlsArea = this.statusBarControlsArea1;
      this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.statusBar.Location = new System.Drawing.Point(0, 1167);
      this.statusBar.Name = "statusBar";
      this.statusBar.NotificationsArea = this.statusBarNotificationsArea1;
      this.statusBar.Size = new System.Drawing.Size(1008, 22);
      this.statusBar.TabIndex = 16;
      this.statusBar.Text = "statusBar1";
      // 
      // statusBarNotificationsArea1
      // 
      this.statusBarNotificationsArea1.Controls.Add(this.statusBarPane2);
      this.statusBarNotificationsArea1.Controls.Add(this.statusBarPane3);
      this.statusBarNotificationsArea1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.statusBarNotificationsArea1.Location = new System.Drawing.Point(0, 0);
      this.statusBarNotificationsArea1.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarNotificationsArea1.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarNotificationsArea1.Name = "statusBarNotificationsArea1";
      this.statusBarNotificationsArea1.Size = new System.Drawing.Size(689, 22);
      this.statusBarNotificationsArea1.TabIndex = 1;
      // 
      // statusBarPane2
      // 
      this.statusBarPane2.Controls.Add(this.toolStripStatusLabelFiles);
      this.statusBarPane2.Controls.Add(this.toolStripStatusLabelFilter);
      this.statusBarPane2.Location = new System.Drawing.Point(0, 0);
      this.statusBarPane2.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane2.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane2.Name = "statusBarPane2";
      this.statusBarPane2.Size = new System.Drawing.Size(149, 22);
      this.statusBarPane2.TabIndex = 0;
      // 
      // toolStripStatusLabelFiles
      // 
      this.toolStripStatusLabelFiles.Location = new System.Drawing.Point(5, 5);
      this.toolStripStatusLabelFiles.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.toolStripStatusLabelFiles.Name = "toolStripStatusLabelFiles";
      this.toolStripStatusLabelFiles.Size = new System.Drawing.Size(57, 13);
      this.toolStripStatusLabelFiles.TabIndex = 0;
      this.toolStripStatusLabelFiles.Text = "                   ";
      // 
      // toolStripStatusLabelFilter
      // 
      this.toolStripStatusLabelFilter.Location = new System.Drawing.Point(68, 5);
      this.toolStripStatusLabelFilter.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.toolStripStatusLabelFilter.Name = "toolStripStatusLabelFilter";
      this.toolStripStatusLabelFilter.Size = new System.Drawing.Size(48, 13);
      this.toolStripStatusLabelFilter.TabIndex = 1;
      this.toolStripStatusLabelFilter.Text = "                ";
      // 
      // statusBarPane3
      // 
      this.statusBarPane3.Controls.Add(this.toolStripStatusLabelFolder);
      this.statusBarPane3.Location = new System.Drawing.Point(149, 0);
      this.statusBarPane3.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane3.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane3.Name = "statusBarPane3";
      this.statusBarPane3.Padding = new System.Windows.Forms.Padding(10, 2, 2, 1);
      this.statusBarPane3.Size = new System.Drawing.Size(117, 22);
      this.statusBarPane3.TabIndex = 1;
      // 
      // toolStripStatusLabelFolder
      // 
      this.toolStripStatusLabelFolder.Location = new System.Drawing.Point(13, 5);
      this.toolStripStatusLabelFolder.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.toolStripStatusLabelFolder.Name = "toolStripStatusLabelFolder";
      this.toolStripStatusLabelFolder.Size = new System.Drawing.Size(63, 13);
      this.toolStripStatusLabelFolder.TabIndex = 0;
      this.toolStripStatusLabelFolder.Text = "                     ";
      // 
      // statusBarControlsArea1
      // 
      this.statusBarControlsArea1.Controls.Add(this.statusBarPane4);
      this.statusBarControlsArea1.Controls.Add(this.statusBarPane1);
      this.statusBarControlsArea1.Dock = System.Windows.Forms.DockStyle.Right;
      this.statusBarControlsArea1.Location = new System.Drawing.Point(689, 0);
      this.statusBarControlsArea1.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarControlsArea1.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarControlsArea1.Name = "statusBarControlsArea1";
      this.statusBarControlsArea1.Size = new System.Drawing.Size(319, 22);
      this.statusBarControlsArea1.TabIndex = 0;
      // 
      // statusBarPane4
      // 
      this.statusBarPane4.Controls.Add(this.toolStripStatusLabelScanProgress);
      this.statusBarPane4.Dock = System.Windows.Forms.DockStyle.Right;
      this.statusBarPane4.Location = new System.Drawing.Point(0, 0);
      this.statusBarPane4.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane4.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane4.Name = "statusBarPane4";
      this.statusBarPane4.Size = new System.Drawing.Size(34, 22);
      this.statusBarPane4.TabIndex = 2;
      // 
      // toolStripStatusLabelScanProgress
      // 
      this.toolStripStatusLabelScanProgress.Location = new System.Drawing.Point(5, 11);
      this.toolStripStatusLabelScanProgress.Name = "toolStripStatusLabelScanProgress";
      this.toolStripStatusLabelScanProgress.Size = new System.Drawing.Size(0, 0);
      this.toolStripStatusLabelScanProgress.TabIndex = 0;
      // 
      // statusBarPane1
      // 
      this.statusBarPane1.Controls.Add(this.progressBar1);
      this.statusBarPane1.Controls.Add(this.buttonProgressCancel);
      this.statusBarPane1.Location = new System.Drawing.Point(34, 0);
      this.statusBarPane1.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane1.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane1.Name = "statusBarPane1";
      this.statusBarPane1.Size = new System.Drawing.Size(233, 22);
      this.statusBarPane1.TabIndex = 0;
      // 
      // progressBar1
      // 
      this.progressBar1.DesiredWidth = 175;
      this.progressBar1.Id = "a3fc702d-0b4f-4160-9fff-0baba8d7430e";
      this.progressBar1.Location = new System.Drawing.Point(3, 5);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(175, 12);
      this.progressBar1.TabIndex = 0;
      this.progressBar1.Text = "progressBar1";
      // 
      // buttonProgressCancel
      // 
      this.buttonProgressCancel.AutoSize = true;
      this.buttonProgressCancel.CommandName = "ProgressCancel";
      this.buttonProgressCancel.Id = "72fee6d7-03bc-42a8-8e13-35309ec722ad";
      this.buttonProgressCancel.Location = new System.Drawing.Point(180, 2);
      this.buttonProgressCancel.Name = "buttonProgressCancel";
      this.buttonProgressCancel.Size = new System.Drawing.Size(26, 19);
      this.buttonProgressCancel.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonProgressCancel.SmallImages.Images"))))});
      this.buttonProgressCancel.TabIndex = 1;
      this.buttonProgressCancel.MouseEnter += new System.EventHandler(this.buttonProgressCancel_MouseEnter);
      this.buttonProgressCancel.MouseLeave += new System.EventHandler(this.buttonProgressCancel_MouseLeave);
      // 
      // ribbon
      // 
      this.ribbon.ApplicationButtonImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("ribbon.ApplicationButtonImages.Images"))))});
      this.ribbon.ApplicationButtonStyle = Elegant.Ui.RibbonApplicationButtonStyle.Default;
      this.ribbon.BackstageView = this.backstageView;
      this.ribbon.CurrentTabPage = this.ribbonTabPageTag;
      this.ribbon.CustomTitleBarEnabled = false;
      this.ribbon.Dock = System.Windows.Forms.DockStyle.Top;
      this.ribbon.HelpButtonCommandName = "Help";
      this.ribbon.HelpButtonImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::MPTagThat.Properties.Resources.ribbon_help)});
      this.ribbon.HelpButtonVisible = true;
      this.ribbon.Location = new System.Drawing.Point(0, 0);
      this.ribbon.Name = "ribbon";
      this.ribbon.QuickAccessToolbarControls.AddRange(new Elegant.Ui.Control[] {
            this.startMenuSave,
            this.startMenuRefresh});
      this.ribbon.QuickAccessToolbarCustomizationDialogEnabled = false;
      this.ribbon.QuickAccessToolbarCustomizationEnabled = false;
      this.ribbon.QuickAccessToolbarPlacementMode = Elegant.Ui.QuickAccessToolbarPlacementMode.AboveRibbon;
      this.ribbon.Size = new System.Drawing.Size(1008, 153);
      this.ribbon.TabIndex = 15;
      this.ribbon.TabPages.AddRange(new Elegant.Ui.RibbonTabPage[] {
            this.ribbonTabPageTag,
            this.ribbonTabPageRip,
            this.ribbonTabPageConvert,
            this.ribbonTabPageBurn});
      this.ribbon.Text = "ribbon1";
      this.ribbon.CurrentTabPageChanged += new System.EventHandler(this.ribbon_CurrentTabPageChanged);
      // 
      // backstageView
      // 
      this.backstageView.CurrentPage = this.backstageViewPageOptions;
      this.backstageView.Id = "7072530e-3b26-4337-af20-69a8f1207f12";
      this.backstageView.Items.AddRange(new System.Windows.Forms.Control[] {
            this.backstageViewButtonSave,
            this.backstageViewButtonRefresh,
            this.backstageViewPageRecentFolders,
            this.backstageViewPageOptions,
            this.backstageViewSeparator1,
            this.backstageViewButtonChangeColumns,
            this.backstageViewSeparator2,
            this.backstageViewButtonExit});
      this.backstageView.Location = new System.Drawing.Point(0, 52);
      this.backstageView.Name = "backstageView";
      this.backstageView.Size = new System.Drawing.Size(1008, 1115);
      this.backstageView.TabIndex = 17;
      this.backstageView.VisibleChanged += new System.EventHandler(this.backstageView_VisibleChanged);
      // 
      // backstageViewPageOptions
      // 
      this.backstageViewPageOptions.CommandName = "Options";
      this.backstageViewPageOptions.Controls.Add(this.panel2);
      this.backstageViewPageOptions.Controls.Add(this.panel3);
      this.backstageViewPageOptions.Controls.Add(this.backstageViewPanel1);
      this.backstageViewPageOptions.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::MPTagThat.Properties.Resources.QuickAccessMenuOptions)});
      this.backstageViewPageOptions.Location = new System.Drawing.Point(0, 5);
      this.backstageViewPageOptions.Name = "backstageViewPageOptions";
      this.backstageViewPageOptions.Padding = new System.Windows.Forms.Padding(12);
      this.backstageViewPageOptions.Size = new System.Drawing.Size(910, 1105);
      this.backstageViewPageOptions.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::MPTagThat.Properties.Resources.QuickAccessMenuOptions)});
      this.backstageViewPageOptions.TabIndex = 0;
      this.backstageViewPageOptions.Text = "Options";
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.tabControlSettings);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(222, 68);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(676, 1025);
      this.panel2.TabIndex = 1;
      this.panel2.Text = "panel2";
      // 
      // tabControlSettings
      // 
      this.tabControlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlSettings.Location = new System.Drawing.Point(0, 0);
      this.tabControlSettings.Name = "tabControlSettings";
      this.tabControlSettings.SelectedTabPage = null;
      this.tabControlSettings.Size = new System.Drawing.Size(676, 1025);
      this.tabControlSettings.TabIndex = 0;
      this.tabControlSettings.TabPages.AddRange(new Elegant.Ui.TabPage[] {
            this.tabPageSettingsGeneral,
            this.tabPageSettingsKeys,
            this.tabPageSettingsTagsGeneral,
            this.tabPageSettingsTagsId3,
            this.tabPageSettingsLyricsCover,
            this.tabPageSettingsDatabase,
            this.tabPageSettingsRipGeneral,
            this.tabPageSettingsRipMp3,
            this.tabPageSettingsRipOgg,
            this.tabPageSettingsRipFlac,
            this.tabPageSettingsRipAAC,
            this.tabPageSettingsRipWMA,
            this.tabPageSettingsRipMPC,
            this.tabPageSettingsRipWV});
      // 
      // tabPageSettingsGeneral
      // 
      this.tabPageSettingsGeneral.ActiveControl = null;
      this.tabPageSettingsGeneral.Controls.Add(this.groupBoxGeneral);
      this.tabPageSettingsGeneral.KeyTip = null;
      this.tabPageSettingsGeneral.Name = "tabPageSettingsGeneral";
      this.tabPageSettingsGeneral.Size = new System.Drawing.Size(674, 1010);
      this.tabPageSettingsGeneral.TabIndex = 0;
      // 
      // groupBoxGeneral
      // 
      this.groupBoxGeneral.Controls.Add(this.lbTracklistLocation);
      this.groupBoxGeneral.Controls.Add(this.pictureBoxTrackListTop);
      this.groupBoxGeneral.Controls.Add(this.pictureBoxTrackListBottom);
      this.groupBoxGeneral.Controls.Add(this.comboBoxDebugLevel);
      this.groupBoxGeneral.Controls.Add(this.lbDebugLevel);
      this.groupBoxGeneral.Controls.Add(this.lbTheme);
      this.groupBoxGeneral.Controls.Add(this.comboBoxThemes);
      this.groupBoxGeneral.Controls.Add(this.comboBoxLanguage);
      this.groupBoxGeneral.Controls.Add(this.lbLanguage);
      this.groupBoxGeneral.Id = "c2880c2d-dfe9-47a1-8adb-fa4a03aa933b";
      this.groupBoxGeneral.Localisation = "GroupBoxGeneral";
      this.groupBoxGeneral.LocalisationContext = "Settings";
      this.groupBoxGeneral.Location = new System.Drawing.Point(6, 25);
      this.groupBoxGeneral.Name = "groupBoxGeneral";
      this.groupBoxGeneral.Size = new System.Drawing.Size(587, 207);
      this.groupBoxGeneral.TabIndex = 1;
      this.groupBoxGeneral.Text = "General settings";
      // 
      // lbTracklistLocation
      // 
      this.lbTracklistLocation.Localisation = "Settings";
      this.lbTracklistLocation.LocalisationContext = "TrackListLocation";
      this.lbTracklistLocation.Location = new System.Drawing.Point(16, 144);
      this.lbTracklistLocation.Name = "lbTracklistLocation";
      this.lbTracklistLocation.Size = new System.Drawing.Size(100, 23);
      this.lbTracklistLocation.TabIndex = 2;
      this.lbTracklistLocation.Text = "Location of Tracklist:";
      // 
      // pictureBoxTrackListTop
      // 
      this.pictureBoxTrackListTop.BackColor = System.Drawing.SystemColors.Control;
      this.pictureBoxTrackListTop.Image = global::MPTagThat.Properties.Resources.TrackList_top;
      this.pictureBoxTrackListTop.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxTrackListTop.InitialImage")));
      this.pictureBoxTrackListTop.Location = new System.Drawing.Point(215, 122);
      this.pictureBoxTrackListTop.Name = "pictureBoxTrackListTop";
      this.pictureBoxTrackListTop.Size = new System.Drawing.Size(97, 57);
      this.pictureBoxTrackListTop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxTrackListTop.TabIndex = 2;
      this.pictureBoxTrackListTop.TabStop = false;
      this.pictureBoxTrackListTop.Text = "pictureBox1";
      this.pictureBoxTrackListTop.Click += new System.EventHandler(this.pictureBoxTrackListTop_Click);
      // 
      // pictureBoxTrackListBottom
      // 
      this.pictureBoxTrackListBottom.Image = global::MPTagThat.Properties.Resources.TrackList_bottom;
      this.pictureBoxTrackListBottom.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxTrackListBottom.InitialImage")));
      this.pictureBoxTrackListBottom.Location = new System.Drawing.Point(333, 122);
      this.pictureBoxTrackListBottom.Name = "pictureBoxTrackListBottom";
      this.pictureBoxTrackListBottom.Size = new System.Drawing.Size(97, 57);
      this.pictureBoxTrackListBottom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxTrackListBottom.TabIndex = 6;
      this.pictureBoxTrackListBottom.TabStop = false;
      this.pictureBoxTrackListBottom.Text = "pictureBox2";
      this.pictureBoxTrackListBottom.Click += new System.EventHandler(this.pictureBoxTrackListBottom_Click);
      // 
      // comboBoxDebugLevel
      // 
      this.comboBoxDebugLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxDebugLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxDebugLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxDebugLevel.Editable = false;
      this.comboBoxDebugLevel.FormattingEnabled = true;
      this.comboBoxDebugLevel.Id = "6b30288d-a831-4af5-b3b0-9049e7ca8e36";
      this.comboBoxDebugLevel.Location = new System.Drawing.Point(218, 80);
      this.comboBoxDebugLevel.Name = "comboBoxDebugLevel";
      this.comboBoxDebugLevel.Size = new System.Drawing.Size(320, 21);
      this.comboBoxDebugLevel.TabIndex = 2;
      this.comboBoxDebugLevel.TextEditorWidth = 301;
      // 
      // lbDebugLevel
      // 
      this.lbDebugLevel.Localisation = "DebugLevel";
      this.lbDebugLevel.LocalisationContext = "Settings";
      this.lbDebugLevel.Location = new System.Drawing.Point(16, 83);
      this.lbDebugLevel.Name = "lbDebugLevel";
      this.lbDebugLevel.Size = new System.Drawing.Size(67, 13);
      this.lbDebugLevel.TabIndex = 5;
      this.lbDebugLevel.Text = "Debug level:";
      // 
      // lbTheme
      // 
      this.lbTheme.Localisation = "Theme";
      this.lbTheme.LocalisationContext = "Settings";
      this.lbTheme.Location = new System.Drawing.Point(16, 52);
      this.lbTheme.Name = "lbTheme";
      this.lbTheme.Size = new System.Drawing.Size(43, 13);
      this.lbTheme.TabIndex = 4;
      this.lbTheme.Text = "Theme:";
      // 
      // comboBoxThemes
      // 
      this.comboBoxThemes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxThemes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxThemes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxThemes.Editable = false;
      this.comboBoxThemes.FormattingEnabled = true;
      this.comboBoxThemes.Id = "9accc7a3-4b86-4931-b228-8b00d8d049b6";
      this.comboBoxThemes.Location = new System.Drawing.Point(218, 49);
      this.comboBoxThemes.Name = "comboBoxThemes";
      this.comboBoxThemes.Size = new System.Drawing.Size(320, 21);
      this.comboBoxThemes.TabIndex = 1;
      this.comboBoxThemes.TextEditorWidth = 301;
      this.comboBoxThemes.SelectedIndexChanged += new System.EventHandler(this.comboBoxThemes_SelectedIndexChanged);
      // 
      // comboBoxLanguage
      // 
      this.comboBoxLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxLanguage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxLanguage.Editable = false;
      this.comboBoxLanguage.FormattingEnabled = true;
      this.comboBoxLanguage.Id = "1795cc6e-129d-4902-9edc-657a97c2c276";
      this.comboBoxLanguage.Location = new System.Drawing.Point(218, 17);
      this.comboBoxLanguage.Name = "comboBoxLanguage";
      this.comboBoxLanguage.Size = new System.Drawing.Size(320, 21);
      this.comboBoxLanguage.TabIndex = 0;
      this.comboBoxLanguage.TextEditorWidth = 301;
      // 
      // lbLanguage
      // 
      this.lbLanguage.Localisation = "Language";
      this.lbLanguage.LocalisationContext = "Settings";
      this.lbLanguage.Location = new System.Drawing.Point(16, 20);
      this.lbLanguage.Name = "lbLanguage";
      this.lbLanguage.Size = new System.Drawing.Size(58, 13);
      this.lbLanguage.TabIndex = 0;
      this.lbLanguage.Text = "Language:";
      // 
      // tabPageSettingsKeys
      // 
      this.tabPageSettingsKeys.ActiveControl = null;
      this.tabPageSettingsKeys.Controls.Add(this.groupBoxKeys);
      this.tabPageSettingsKeys.KeyTip = null;
      this.tabPageSettingsKeys.Name = "tabPageSettingsKeys";
      this.tabPageSettingsKeys.Size = new System.Drawing.Size(604, 1009);
      this.tabPageSettingsKeys.TabIndex = 1;
      // 
      // groupBoxKeys
      // 
      this.groupBoxKeys.Controls.Add(this.tbRibbonKeyValue);
      this.groupBoxKeys.Controls.Add(this.lblRibbonShortCut);
      this.groupBoxKeys.Controls.Add(this.lblKeyShortCut);
      this.groupBoxKeys.Controls.Add(this.buttonChangeKey);
      this.groupBoxKeys.Controls.Add(this.tbKeyValue);
      this.groupBoxKeys.Controls.Add(this.ttLabel1);
      this.groupBoxKeys.Controls.Add(this.ckShift);
      this.groupBoxKeys.Controls.Add(this.ckCtrl);
      this.groupBoxKeys.Controls.Add(this.ckAlt);
      this.groupBoxKeys.Controls.Add(this.tbKeyDescription);
      this.groupBoxKeys.Controls.Add(this.lbKeyDescription);
      this.groupBoxKeys.Controls.Add(this.tbAction);
      this.groupBoxKeys.Controls.Add(this.lbKeyAction);
      this.groupBoxKeys.Controls.Add(this.treeViewKeys);
      this.groupBoxKeys.Id = "7ee90950-d8f0-48ec-988f-d887da43ecac";
      this.groupBoxKeys.Localisation = "GroupBoxKeys";
      this.groupBoxKeys.LocalisationContext = "Settings";
      this.groupBoxKeys.Location = new System.Drawing.Point(9, 21);
      this.groupBoxKeys.Name = "groupBoxKeys";
      this.groupBoxKeys.Size = new System.Drawing.Size(581, 471);
      this.groupBoxKeys.TabIndex = 2;
      this.groupBoxKeys.Text = "Keyboard Shortcuts";
      // 
      // tbRibbonKeyValue
      // 
      this.tbRibbonKeyValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbRibbonKeyValue.Location = new System.Drawing.Point(196, 432);
      this.tbRibbonKeyValue.MaxLength = 1;
      this.tbRibbonKeyValue.Name = "tbRibbonKeyValue";
      this.tbRibbonKeyValue.Size = new System.Drawing.Size(20, 20);
      this.tbRibbonKeyValue.TabIndex = 13;
      this.tbRibbonKeyValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRibbonKeyValue_KeyPress);
      // 
      // lblRibbonShortCut
      // 
      this.lblRibbonShortCut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblRibbonShortCut.Localisation = "RibbonShortCut";
      this.lblRibbonShortCut.LocalisationContext = "Settings";
      this.lblRibbonShortCut.Location = new System.Drawing.Point(22, 435);
      this.lblRibbonShortCut.Name = "lblRibbonShortCut";
      this.lblRibbonShortCut.Size = new System.Drawing.Size(87, 13);
      this.lblRibbonShortCut.TabIndex = 12;
      this.lblRibbonShortCut.Text = "Ribbon Shortcut:";
      // 
      // lblKeyShortCut
      // 
      this.lblKeyShortCut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblKeyShortCut.Localisation = "KeyShortCut";
      this.lblKeyShortCut.LocalisationContext = "Settings";
      this.lblKeyShortCut.Location = new System.Drawing.Point(22, 406);
      this.lblKeyShortCut.Name = "lblKeyShortCut";
      this.lblKeyShortCut.Size = new System.Drawing.Size(71, 13);
      this.lblKeyShortCut.TabIndex = 11;
      this.lblKeyShortCut.Text = "Key Shortcut:";
      // 
      // buttonChangeKey
      // 
      this.buttonChangeKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonChangeKey.Id = "d9904459-a565-4b6e-8ccd-0587e7d0463e";
      this.buttonChangeKey.Localisation = "ChangeKey";
      this.buttonChangeKey.LocalisationContext = "Settings";
      this.buttonChangeKey.Location = new System.Drawing.Point(480, 425);
      this.buttonChangeKey.Name = "buttonChangeKey";
      this.buttonChangeKey.Size = new System.Drawing.Size(89, 23);
      this.buttonChangeKey.TabIndex = 10;
      this.buttonChangeKey.Text = "Change";
      this.buttonChangeKey.UseVisualStyleBackColor = true;
      this.buttonChangeKey.Click += new System.EventHandler(this.buttonChangeKey_Click);
      // 
      // tbKeyValue
      // 
      this.tbKeyValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbKeyValue.Location = new System.Drawing.Point(416, 393);
      this.tbKeyValue.Name = "tbKeyValue";
      this.tbKeyValue.Size = new System.Drawing.Size(153, 20);
      this.tbKeyValue.TabIndex = 9;
      // 
      // ttLabel1
      // 
      this.ttLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ttLabel1.Localisation = "ttLabel1";
      this.ttLabel1.LocalisationContext = "Settings";
      this.ttLabel1.Location = new System.Drawing.Point(397, 397);
      this.ttLabel1.Name = "ttLabel1";
      this.ttLabel1.Size = new System.Drawing.Size(13, 13);
      this.ttLabel1.TabIndex = 8;
      this.ttLabel1.Text = "+";
      // 
      // ckShift
      // 
      this.ckShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ckShift.Id = "31312ed6-2c18-4d8d-be30-0f03b5e155ab";
      this.ckShift.Localisation = "Shift";
      this.ckShift.LocalisationContext = "Settings";
      this.ckShift.Location = new System.Drawing.Point(305, 393);
      this.ckShift.Name = "ckShift";
      this.ckShift.Size = new System.Drawing.Size(47, 26);
      this.ckShift.TabIndex = 7;
      this.ckShift.Text = "Shift";
      // 
      // ckCtrl
      // 
      this.ckCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ckCtrl.Id = "2552c4f6-671e-43ad-9043-0a7f808e12d1";
      this.ckCtrl.Localisation = "Ctrl";
      this.ckCtrl.LocalisationContext = "Settings";
      this.ckCtrl.Location = new System.Drawing.Point(250, 393);
      this.ckCtrl.Name = "ckCtrl";
      this.ckCtrl.Size = new System.Drawing.Size(41, 26);
      this.ckCtrl.TabIndex = 6;
      this.ckCtrl.Text = "Ctrl";
      // 
      // ckAlt
      // 
      this.ckAlt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ckAlt.Id = "61e608bf-f29b-4b86-bca3-cd1b27e98927";
      this.ckAlt.Localisation = "Alt";
      this.ckAlt.LocalisationContext = "Settings";
      this.ckAlt.Location = new System.Drawing.Point(196, 393);
      this.ckAlt.Name = "ckAlt";
      this.ckAlt.Size = new System.Drawing.Size(38, 26);
      this.ckAlt.TabIndex = 5;
      this.ckAlt.Text = "Alt";
      // 
      // tbKeyDescription
      // 
      this.tbKeyDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbKeyDescription.Location = new System.Drawing.Point(194, 357);
      this.tbKeyDescription.Name = "tbKeyDescription";
      this.tbKeyDescription.Size = new System.Drawing.Size(375, 20);
      this.tbKeyDescription.TabIndex = 1;
      // 
      // lbKeyDescription
      // 
      this.lbKeyDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbKeyDescription.Localisation = "KeyDescription";
      this.lbKeyDescription.LocalisationContext = "Settings";
      this.lbKeyDescription.Location = new System.Drawing.Point(22, 357);
      this.lbKeyDescription.Name = "lbKeyDescription";
      this.lbKeyDescription.Size = new System.Drawing.Size(63, 13);
      this.lbKeyDescription.TabIndex = 3;
      this.lbKeyDescription.Text = "Description:";
      // 
      // tbAction
      // 
      this.tbAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbAction.Enabled = false;
      this.tbAction.Location = new System.Drawing.Point(194, 328);
      this.tbAction.Name = "tbAction";
      this.tbAction.ReadOnly = true;
      this.tbAction.Size = new System.Drawing.Size(375, 20);
      this.tbAction.TabIndex = 0;
      // 
      // lbKeyAction
      // 
      this.lbKeyAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbKeyAction.Localisation = "KeyAction";
      this.lbKeyAction.LocalisationContext = "Settings";
      this.lbKeyAction.Location = new System.Drawing.Point(22, 331);
      this.lbKeyAction.Name = "lbKeyAction";
      this.lbKeyAction.Size = new System.Drawing.Size(40, 13);
      this.lbKeyAction.TabIndex = 1;
      this.lbKeyAction.Text = "Action:";
      // 
      // treeViewKeys
      // 
      this.treeViewKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.treeViewKeys.Location = new System.Drawing.Point(19, 31);
      this.treeViewKeys.Name = "treeViewKeys";
      this.treeViewKeys.Size = new System.Drawing.Size(550, 284);
      this.treeViewKeys.TabIndex = 0;
      this.treeViewKeys.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewKeys_AfterSelect);
      // 
      // tabPageSettingsTagsGeneral
      // 
      this.tabPageSettingsTagsGeneral.ActiveControl = null;
      this.tabPageSettingsTagsGeneral.Controls.Add(this.groupBoxTagsGeneral);
      this.tabPageSettingsTagsGeneral.KeyTip = null;
      this.tabPageSettingsTagsGeneral.Name = "tabPageSettingsTagsGeneral";
      this.tabPageSettingsTagsGeneral.Size = new System.Drawing.Size(604, 1010);
      this.tabPageSettingsTagsGeneral.TabIndex = 2;
      // 
      // groupBoxTagsGeneral
      // 
      this.groupBoxTagsGeneral.Controls.Add(this.ckChangeReadonlyAttributte);
      this.groupBoxTagsGeneral.Controls.Add(this.ckAutoFillNumberOfTracks);
      this.groupBoxTagsGeneral.Controls.Add(this.ckUseCaseConversionWhenSaving);
      this.groupBoxTagsGeneral.Controls.Add(this.ckCopyArtistToAlbumArtist);
      this.groupBoxTagsGeneral.Id = "110f1ef1-e61d-4eaa-bea8-de95a3982eb0";
      this.groupBoxTagsGeneral.Localisation = "GroupBoxTagsGeneral";
      this.groupBoxTagsGeneral.LocalisationContext = "Settings";
      this.groupBoxTagsGeneral.Location = new System.Drawing.Point(6, 25);
      this.groupBoxTagsGeneral.Name = "groupBoxTagsGeneral";
      this.groupBoxTagsGeneral.Size = new System.Drawing.Size(580, 153);
      this.groupBoxTagsGeneral.TabIndex = 4;
      this.groupBoxTagsGeneral.Text = "General";
      // 
      // ckChangeReadonlyAttributte
      // 
      this.ckChangeReadonlyAttributte.Id = "0f774b0f-f698-4696-9d81-8675a25017b4";
      this.ckChangeReadonlyAttributte.Localisation = "ChangeReadonly";
      this.ckChangeReadonlyAttributte.LocalisationContext = "Settings";
      this.ckChangeReadonlyAttributte.Location = new System.Drawing.Point(14, 99);
      this.ckChangeReadonlyAttributte.Name = "ckChangeReadonlyAttributte";
      this.ckChangeReadonlyAttributte.Size = new System.Drawing.Size(544, 26);
      this.ckChangeReadonlyAttributte.TabIndex = 6;
      this.ckChangeReadonlyAttributte.Text = "Change Readonly Attributte on Save";
      // 
      // ckAutoFillNumberOfTracks
      // 
      this.ckAutoFillNumberOfTracks.Id = "a7e3a966-0c21-44d1-8d40-0a582b23955e";
      this.ckAutoFillNumberOfTracks.Localisation = "AutoFillNumberTracks";
      this.ckAutoFillNumberOfTracks.LocalisationContext = "Settings";
      this.ckAutoFillNumberOfTracks.Location = new System.Drawing.Point(14, 48);
      this.ckAutoFillNumberOfTracks.Name = "ckAutoFillNumberOfTracks";
      this.ckAutoFillNumberOfTracks.Size = new System.Drawing.Size(544, 26);
      this.ckAutoFillNumberOfTracks.TabIndex = 5;
      this.ckAutoFillNumberOfTracks.Text = "Auto Fill \"Number of Tracks\" on Multi Tag Edit";
      // 
      // ckUseCaseConversionWhenSaving
      // 
      this.ckUseCaseConversionWhenSaving.Id = "26179666-1da5-4f4f-957c-1428254efa78";
      this.ckUseCaseConversionWhenSaving.Localisation = "CaseConversion";
      this.ckUseCaseConversionWhenSaving.LocalisationContext = "Settings";
      this.ckUseCaseConversionWhenSaving.Location = new System.Drawing.Point(14, 73);
      this.ckUseCaseConversionWhenSaving.Name = "ckUseCaseConversionWhenSaving";
      this.ckUseCaseConversionWhenSaving.Size = new System.Drawing.Size(544, 26);
      this.ckUseCaseConversionWhenSaving.TabIndex = 1;
      this.ckUseCaseConversionWhenSaving.Text = "Use case conversion settings on Save";
      // 
      // ckCopyArtistToAlbumArtist
      // 
      this.ckCopyArtistToAlbumArtist.Id = "373e4d93-6dd3-41eb-a6e7-69c7e230717e";
      this.ckCopyArtistToAlbumArtist.Localisation = "CopyArtist";
      this.ckCopyArtistToAlbumArtist.LocalisationContext = "Settings";
      this.ckCopyArtistToAlbumArtist.Location = new System.Drawing.Point(15, 25);
      this.ckCopyArtistToAlbumArtist.Name = "ckCopyArtistToAlbumArtist";
      this.ckCopyArtistToAlbumArtist.Size = new System.Drawing.Size(543, 26);
      this.ckCopyArtistToAlbumArtist.TabIndex = 0;
      this.ckCopyArtistToAlbumArtist.Text = "Copy Artist to AlbumArtist when empty";
      // 
      // tabPageSettingsTagsId3
      // 
      this.tabPageSettingsTagsId3.ActiveControl = null;
      this.tabPageSettingsTagsId3.Controls.Add(this.groupBoxTagValidate);
      this.tabPageSettingsTagsId3.Controls.Add(this.groupBoxTagsID3);
      this.tabPageSettingsTagsId3.KeyTip = null;
      this.tabPageSettingsTagsId3.Name = "tabPageSettingsTagsId3";
      this.tabPageSettingsTagsId3.Size = new System.Drawing.Size(604, 1010);
      this.tabPageSettingsTagsId3.TabIndex = 3;
      // 
      // groupBoxTagValidate
      // 
      this.groupBoxTagValidate.Controls.Add(this.ckAutoFixMp3);
      this.groupBoxTagValidate.Controls.Add(this.ckValidateMP3);
      this.groupBoxTagValidate.Id = "7717c908-7971-4500-92c4-3eab7b09be59";
      this.groupBoxTagValidate.Localisation = "GroupBoxTagsValidate";
      this.groupBoxTagValidate.LocalisationContext = "Settings";
      this.groupBoxTagValidate.Location = new System.Drawing.Point(6, 292);
      this.groupBoxTagValidate.Name = "groupBoxTagValidate";
      this.groupBoxTagValidate.Size = new System.Drawing.Size(586, 87);
      this.groupBoxTagValidate.TabIndex = 4;
      this.groupBoxTagValidate.Text = "MP3 File Validation";
      // 
      // ckAutoFixMp3
      // 
      this.ckAutoFixMp3.Id = "82c36c1d-61ef-4839-9709-f0bb89ebaa02";
      this.ckAutoFixMp3.Localisation = "AutoFixMp3";
      this.ckAutoFixMp3.LocalisationContext = "Settings";
      this.ckAutoFixMp3.Location = new System.Drawing.Point(12, 52);
      this.ckAutoFixMp3.Name = "ckAutoFixMp3";
      this.ckAutoFixMp3.Size = new System.Drawing.Size(559, 26);
      this.ckAutoFixMp3.TabIndex = 1;
      this.ckAutoFixMp3.Text = "Automatically fix errorneous Mp3 Files";
      // 
      // ckValidateMP3
      // 
      this.ckValidateMP3.Id = "47cd7155-1711-46cf-9168-881505ad7e87";
      this.ckValidateMP3.Localisation = "ValidateMP3";
      this.ckValidateMP3.LocalisationContext = "Settings";
      this.ckValidateMP3.Location = new System.Drawing.Point(12, 29);
      this.ckValidateMP3.Name = "ckValidateMP3";
      this.ckValidateMP3.Size = new System.Drawing.Size(559, 26);
      this.ckValidateMP3.TabIndex = 0;
      this.ckValidateMP3.Text = "Validate MP3 Files while scanning the folder";
      // 
      // groupBoxTagsID3
      // 
      this.groupBoxTagsID3.Controls.Add(this.comboBoxCharacterEncoding);
      this.groupBoxTagsID3.Controls.Add(this.radioButtonUseApe);
      this.groupBoxTagsID3.Controls.Add(this.lbCharacterEncoding);
      this.groupBoxTagsID3.Controls.Add(this.radioButtonUseV4);
      this.groupBoxTagsID3.Controls.Add(this.radioButtonUseV3);
      this.groupBoxTagsID3.Controls.Add(this.groupBoxID3Update);
      this.groupBoxTagsID3.Id = "1fbe4009-e90a-475d-80d4-5ae18af00579";
      this.groupBoxTagsID3.Localisation = "GroupBoxTagsID3";
      this.groupBoxTagsID3.LocalisationContext = "Settings";
      this.groupBoxTagsID3.Location = new System.Drawing.Point(6, 24);
      this.groupBoxTagsID3.Name = "groupBoxTagsID3";
      this.groupBoxTagsID3.Size = new System.Drawing.Size(586, 250);
      this.groupBoxTagsID3.TabIndex = 3;
      this.groupBoxTagsID3.Text = "ID3";
      // 
      // comboBoxCharacterEncoding
      // 
      this.comboBoxCharacterEncoding.FormattingEnabled = false;
      this.comboBoxCharacterEncoding.Id = "fc50a15a-5f0f-488c-af4e-5eaceb744e36";
      this.comboBoxCharacterEncoding.Items.AddRange(new object[] {
            "Latin1",
            "UTF-16",
            "UTF16-BE",
            "UTF-8",
            "UTF-16LE"});
      this.comboBoxCharacterEncoding.Location = new System.Drawing.Point(295, 18);
      this.comboBoxCharacterEncoding.Name = "comboBoxCharacterEncoding";
      this.comboBoxCharacterEncoding.Size = new System.Drawing.Size(171, 21);
      this.comboBoxCharacterEncoding.TabIndex = 7;
      this.comboBoxCharacterEncoding.TextEditorWidth = 152;
      // 
      // radioButtonUseApe
      // 
      this.radioButtonUseApe.Id = "01a48f9a-423e-4b38-b64d-98a9f19feb00";
      this.radioButtonUseApe.Localisation = "UseAPE";
      this.radioButtonUseApe.LocalisationContext = "Settings";
      this.radioButtonUseApe.Location = new System.Drawing.Point(12, 92);
      this.radioButtonUseApe.Name = "radioButtonUseApe";
      this.radioButtonUseApe.Size = new System.Drawing.Size(152, 26);
      this.radioButtonUseApe.TabIndex = 2;
      this.radioButtonUseApe.Text = "Use APE and ID3 V1 Tags";
      // 
      // lbCharacterEncoding
      // 
      this.lbCharacterEncoding.Localisation = "Encoding";
      this.lbCharacterEncoding.LocalisationContext = "Settings";
      this.lbCharacterEncoding.Location = new System.Drawing.Point(13, 23);
      this.lbCharacterEncoding.Name = "lbCharacterEncoding";
      this.lbCharacterEncoding.Size = new System.Drawing.Size(150, 23);
      this.lbCharacterEncoding.TabIndex = 6;
      this.lbCharacterEncoding.Text = "Encoding used for saving Tags:";
      // 
      // radioButtonUseV4
      // 
      this.radioButtonUseV4.Id = "f99c4ccf-7421-479d-bb55-fcf10f7b8096";
      this.radioButtonUseV4.Localisation = "UseV4";
      this.radioButtonUseV4.LocalisationContext = "Settings";
      this.radioButtonUseV4.Location = new System.Drawing.Point(12, 69);
      this.radioButtonUseV4.Name = "radioButtonUseV4";
      this.radioButtonUseV4.Size = new System.Drawing.Size(178, 26);
      this.radioButtonUseV4.TabIndex = 1;
      this.radioButtonUseV4.Text = "Use Version 2.4 for ID3 V2 Tags";
      // 
      // radioButtonUseV3
      // 
      this.radioButtonUseV3.Checked = true;
      this.radioButtonUseV3.Id = "fa8acd7c-132c-4fe5-aecc-1aad2b0b149d";
      this.radioButtonUseV3.Localisation = "UseV3";
      this.radioButtonUseV3.LocalisationContext = "Settings";
      this.radioButtonUseV3.Location = new System.Drawing.Point(12, 46);
      this.radioButtonUseV3.Name = "radioButtonUseV3";
      this.radioButtonUseV3.Size = new System.Drawing.Size(178, 26);
      this.radioButtonUseV3.TabIndex = 0;
      this.radioButtonUseV3.Text = "Use Version 2.3 for ID3 V2 Tags";
      // 
      // groupBoxID3Update
      // 
      this.groupBoxID3Update.Controls.Add(this.checkBoxRemoveID3V1);
      this.groupBoxID3Update.Controls.Add(this.checkBoxRemoveID3V2);
      this.groupBoxID3Update.Controls.Add(this.radioButtonID3Both);
      this.groupBoxID3Update.Controls.Add(this.radioButtonID3V2);
      this.groupBoxID3Update.Controls.Add(this.radioButtonID3V1);
      this.groupBoxID3Update.Id = "baf8fb33-b3c0-4b00-b12f-4bed4121d388";
      this.groupBoxID3Update.Localisation = "GroupBoxID3Update";
      this.groupBoxID3Update.LocalisationContext = "Settings";
      this.groupBoxID3Update.Location = new System.Drawing.Point(12, 142);
      this.groupBoxID3Update.Name = "groupBoxID3Update";
      this.groupBoxID3Update.Size = new System.Drawing.Size(560, 99);
      this.groupBoxID3Update.TabIndex = 1;
      this.groupBoxID3Update.Text = "Update";
      // 
      // checkBoxRemoveID3V1
      // 
      this.checkBoxRemoveID3V1.Id = "84a37ab2-3021-4116-a9a4-d1443f38de13";
      this.checkBoxRemoveID3V1.Localisation = "RemoveID3V1";
      this.checkBoxRemoveID3V1.LocalisationContext = "Settings";
      this.checkBoxRemoveID3V1.Location = new System.Drawing.Point(278, 34);
      this.checkBoxRemoveID3V1.Name = "checkBoxRemoveID3V1";
      this.checkBoxRemoveID3V1.Size = new System.Drawing.Size(255, 26);
      this.checkBoxRemoveID3V1.TabIndex = 4;
      this.checkBoxRemoveID3V1.Text = "Remove ID3V1";
      // 
      // checkBoxRemoveID3V2
      // 
      this.checkBoxRemoveID3V2.Id = "e0cb86ac-a7ea-40aa-8359-9b3715e7fa05";
      this.checkBoxRemoveID3V2.Localisation = "RemoveID3V2";
      this.checkBoxRemoveID3V2.LocalisationContext = "Settings";
      this.checkBoxRemoveID3V2.Location = new System.Drawing.Point(278, 11);
      this.checkBoxRemoveID3V2.Name = "checkBoxRemoveID3V2";
      this.checkBoxRemoveID3V2.Size = new System.Drawing.Size(255, 26);
      this.checkBoxRemoveID3V2.TabIndex = 3;
      this.checkBoxRemoveID3V2.Text = "Remove ID3V2";
      // 
      // radioButtonID3Both
      // 
      this.radioButtonID3Both.Checked = true;
      this.radioButtonID3Both.Id = "c22286e6-87f6-4a44-8668-1df84c7a73d6";
      this.radioButtonID3Both.Localisation = "ID3Both";
      this.radioButtonID3Both.LocalisationContext = "Settings";
      this.radioButtonID3Both.Location = new System.Drawing.Point(15, 66);
      this.radioButtonID3Both.Name = "radioButtonID3Both";
      this.radioButtonID3Both.Size = new System.Drawing.Size(109, 26);
      this.radioButtonID3Both.TabIndex = 2;
      this.radioButtonID3Both.Text = "ID3V1 and ID3V2";
      this.radioButtonID3Both.CheckedChanged += new System.EventHandler(this.radioButtonID3Both_CheckedChanged);
      // 
      // radioButtonID3V2
      // 
      this.radioButtonID3V2.Id = "0db1a591-cb74-4ede-8bb7-dfb6f136d66f";
      this.radioButtonID3V2.Localisation = "ID3V2";
      this.radioButtonID3V2.LocalisationContext = "Settings";
      this.radioButtonID3V2.Location = new System.Drawing.Point(15, 43);
      this.radioButtonID3V2.Name = "radioButtonID3V2";
      this.radioButtonID3V2.Size = new System.Drawing.Size(55, 26);
      this.radioButtonID3V2.TabIndex = 1;
      this.radioButtonID3V2.Text = "ID3V2";
      this.radioButtonID3V2.CheckedChanged += new System.EventHandler(this.radioButtonID3V2_CheckedChanged);
      // 
      // radioButtonID3V1
      // 
      this.radioButtonID3V1.Id = "242b64b1-fc1e-493d-a55f-6d5034b9f0fb";
      this.radioButtonID3V1.Localisation = "ID3V1";
      this.radioButtonID3V1.LocalisationContext = "Settings";
      this.radioButtonID3V1.Location = new System.Drawing.Point(15, 20);
      this.radioButtonID3V1.Name = "radioButtonID3V1";
      this.radioButtonID3V1.Size = new System.Drawing.Size(55, 26);
      this.radioButtonID3V1.TabIndex = 0;
      this.radioButtonID3V1.Text = "ID3V1";
      this.radioButtonID3V1.CheckedChanged += new System.EventHandler(this.radioButtonID3V1_CheckedChanged);
      // 
      // tabPageSettingsLyricsCover
      // 
      this.tabPageSettingsLyricsCover.ActiveControl = null;
      this.tabPageSettingsLyricsCover.Controls.Add(this.groupBoxPictures);
      this.tabPageSettingsLyricsCover.Controls.Add(this.groupBoxLyrics);
      this.tabPageSettingsLyricsCover.KeyTip = null;
      this.tabPageSettingsLyricsCover.Name = "tabPageSettingsLyricsCover";
      this.tabPageSettingsLyricsCover.Size = new System.Drawing.Size(604, 1010);
      this.tabPageSettingsLyricsCover.TabIndex = 4;
      // 
      // groupBoxPictures
      // 
      this.groupBoxPictures.Controls.Add(this.ckOnlySaveFolderThumb);
      this.groupBoxPictures.Controls.Add(this.comboBoxAmazonSite);
      this.groupBoxPictures.Controls.Add(this.lbAmazonSearchSite);
      this.groupBoxPictures.Controls.Add(this.ckUseExistinbgThumb);
      this.groupBoxPictures.Controls.Add(this.ckOverwriteExistingCovers);
      this.groupBoxPictures.Controls.Add(this.ckCreateMissingFolderThumb);
      this.groupBoxPictures.Id = "bc9f8805-f201-4e8e-9ff9-8d04ec79ba1d";
      this.groupBoxPictures.Localisation = "GroupBoxPictures";
      this.groupBoxPictures.LocalisationContext = "Settings";
      this.groupBoxPictures.Location = new System.Drawing.Point(6, 25);
      this.groupBoxPictures.Name = "groupBoxPictures";
      this.groupBoxPictures.Size = new System.Drawing.Size(590, 196);
      this.groupBoxPictures.TabIndex = 5;
      this.groupBoxPictures.Text = "Pictures";
      // 
      // ckOnlySaveFolderThumb
      // 
      this.ckOnlySaveFolderThumb.Id = "50a2621b-c4e3-439e-9d77-9137e14ac18d";
      this.ckOnlySaveFolderThumb.Localisation = "OnlySaveFolderThumb";
      this.ckOnlySaveFolderThumb.LocalisationContext = "Settings";
      this.ckOnlySaveFolderThumb.Location = new System.Drawing.Point(27, 106);
      this.ckOnlySaveFolderThumb.Name = "ckOnlySaveFolderThumb";
      this.ckOnlySaveFolderThumb.Size = new System.Drawing.Size(531, 26);
      this.ckOnlySaveFolderThumb.TabIndex = 11;
      this.ckOnlySaveFolderThumb.Text = "Save Cover Art only to folder thumb. Don\'t touch the music file";
      // 
      // comboBoxAmazonSite
      // 
      this.comboBoxAmazonSite.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxAmazonSite.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxAmazonSite.Editable = false;
      this.comboBoxAmazonSite.FormattingEnabled = true;
      this.comboBoxAmazonSite.Id = "00255aaf-7dc5-4aad-bbef-25bcdf98f8a0";
      this.comboBoxAmazonSite.Location = new System.Drawing.Point(27, 161);
      this.comboBoxAmazonSite.Name = "comboBoxAmazonSite";
      this.comboBoxAmazonSite.Size = new System.Drawing.Size(197, 21);
      this.comboBoxAmazonSite.TabIndex = 10;
      // 
      // lbAmazonSearchSite
      // 
      this.lbAmazonSearchSite.Localisation = "AmazonSite";
      this.lbAmazonSearchSite.LocalisationContext = "Settings";
      this.lbAmazonSearchSite.Location = new System.Drawing.Point(27, 140);
      this.lbAmazonSearchSite.Name = "lbAmazonSearchSite";
      this.lbAmazonSearchSite.Size = new System.Drawing.Size(106, 13);
      this.lbAmazonSearchSite.TabIndex = 9;
      this.lbAmazonSearchSite.Text = "Amazon Search Site:";
      // 
      // ckUseExistinbgThumb
      // 
      this.ckUseExistinbgThumb.Id = "a8929ca2-5329-419f-a075-2cc413e5294c";
      this.ckUseExistinbgThumb.Localisation = "EmbedExistingThumb";
      this.ckUseExistinbgThumb.LocalisationContext = "Settings";
      this.ckUseExistinbgThumb.Location = new System.Drawing.Point(27, 52);
      this.ckUseExistinbgThumb.Name = "ckUseExistinbgThumb";
      this.ckUseExistinbgThumb.Size = new System.Drawing.Size(545, 26);
      this.ckUseExistinbgThumb.TabIndex = 8;
      this.ckUseExistinbgThumb.Text = "Embed existing folder thumb (folder.jpg)  on Cover search";
      // 
      // ckOverwriteExistingCovers
      // 
      this.ckOverwriteExistingCovers.Id = "51379b1b-ef8f-40e5-810f-a764197edd23";
      this.ckOverwriteExistingCovers.Localisation = "OverwriteExistingCover";
      this.ckOverwriteExistingCovers.LocalisationContext = "Settings";
      this.ckOverwriteExistingCovers.Location = new System.Drawing.Point(27, 78);
      this.ckOverwriteExistingCovers.Name = "ckOverwriteExistingCovers";
      this.ckOverwriteExistingCovers.Size = new System.Drawing.Size(531, 26);
      this.ckOverwriteExistingCovers.TabIndex = 7;
      this.ckOverwriteExistingCovers.Text = "Overwrite existing covers on automatic tagging";
      // 
      // ckCreateMissingFolderThumb
      // 
      this.ckCreateMissingFolderThumb.Id = "51c174e0-1555-40eb-afc2-d31b47ed3740";
      this.ckCreateMissingFolderThumb.Localisation = "CreateMissingFolderThumb";
      this.ckCreateMissingFolderThumb.LocalisationContext = "Settings";
      this.ckCreateMissingFolderThumb.Location = new System.Drawing.Point(27, 26);
      this.ckCreateMissingFolderThumb.Name = "ckCreateMissingFolderThumb";
      this.ckCreateMissingFolderThumb.Size = new System.Drawing.Size(545, 26);
      this.ckCreateMissingFolderThumb.TabIndex = 6;
      this.ckCreateMissingFolderThumb.Text = "Create missing folder thumb (folder.jpg) on save";
      // 
      // groupBoxLyrics
      // 
      this.groupBoxLyrics.Controls.Add(this.ckOverwriteExistingLyrics);
      this.groupBoxLyrics.Controls.Add(this.ckSwitchArtist);
      this.groupBoxLyrics.Controls.Add(this.groupBoxLyricsSites);
      this.groupBoxLyrics.Id = "455cf3ea-34c2-46c3-9024-547257e6e4c5";
      this.groupBoxLyrics.Localisation = "GroupBoxLyrics";
      this.groupBoxLyrics.LocalisationContext = "Settings";
      this.groupBoxLyrics.Location = new System.Drawing.Point(6, 235);
      this.groupBoxLyrics.Name = "groupBoxLyrics";
      this.groupBoxLyrics.Size = new System.Drawing.Size(590, 229);
      this.groupBoxLyrics.TabIndex = 4;
      this.groupBoxLyrics.Text = "Lyrics";
      // 
      // ckOverwriteExistingLyrics
      // 
      this.ckOverwriteExistingLyrics.Id = "9faefec4-e120-46f7-a0b5-0342b1086154";
      this.ckOverwriteExistingLyrics.Localisation = "OverwriteExistingLyrics";
      this.ckOverwriteExistingLyrics.LocalisationContext = "Settings";
      this.ckOverwriteExistingLyrics.Location = new System.Drawing.Point(27, 57);
      this.ckOverwriteExistingLyrics.Name = "ckOverwriteExistingLyrics";
      this.ckOverwriteExistingLyrics.Size = new System.Drawing.Size(475, 26);
      this.ckOverwriteExistingLyrics.TabIndex = 9;
      this.ckOverwriteExistingLyrics.Text = "Overwrite existing lyrics on automatic tagging";
      // 
      // ckSwitchArtist
      // 
      this.ckSwitchArtist.Id = "530ac569-1285-4a23-8e9c-082f5636223b";
      this.ckSwitchArtist.Localisation = "SwitchArtist";
      this.ckSwitchArtist.LocalisationContext = "Settings";
      this.ckSwitchArtist.Location = new System.Drawing.Point(27, 33);
      this.ckSwitchArtist.MaximumSize = new System.Drawing.Size(400, 0);
      this.ckSwitchArtist.Name = "ckSwitchArtist";
      this.ckSwitchArtist.Size = new System.Drawing.Size(400, 26);
      this.ckSwitchArtist.TabIndex = 1;
      this.ckSwitchArtist.Text = "Switch Artist before submit";
      // 
      // groupBoxLyricsSites
      // 
      this.groupBoxLyricsSites.Controls.Add(this.ckLRCFinder);
      this.groupBoxLyricsSites.Controls.Add(this.ckLyrDB);
      this.groupBoxLyricsSites.Controls.Add(this.ckActionext);
      this.groupBoxLyricsSites.Controls.Add(this.ckLyricsPlugin);
      this.groupBoxLyricsSites.Controls.Add(this.ckLyricsOnDemand);
      this.groupBoxLyricsSites.Controls.Add(this.ckLyrics007);
      this.groupBoxLyricsSites.Controls.Add(this.ckHotLyrics);
      this.groupBoxLyricsSites.Controls.Add(this.ckLyricWiki);
      this.groupBoxLyricsSites.Id = "d3c679e8-f772-4aff-a216-8804db1516e9";
      this.groupBoxLyricsSites.Localisation = "GroupBoxLyricsSites";
      this.groupBoxLyricsSites.LocalisationContext = "Settings";
      this.groupBoxLyricsSites.Location = new System.Drawing.Point(15, 96);
      this.groupBoxLyricsSites.Name = "groupBoxLyricsSites";
      this.groupBoxLyricsSites.Size = new System.Drawing.Size(557, 106);
      this.groupBoxLyricsSites.TabIndex = 0;
      this.groupBoxLyricsSites.Text = "Sites to Search";
      // 
      // ckLRCFinder
      // 
      this.ckLRCFinder.Checked = true;
      this.ckLRCFinder.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckLRCFinder.Id = "db5f5e4c-7b7b-4507-b455-747acb017eaf";
      this.ckLRCFinder.Localisation = "LrcFimder";
      this.ckLRCFinder.LocalisationContext = "Settings";
      this.ckLRCFinder.Location = new System.Drawing.Point(432, 60);
      this.ckLRCFinder.Name = "ckLRCFinder";
      this.ckLRCFinder.Size = new System.Drawing.Size(76, 26);
      this.ckLRCFinder.TabIndex = 7;
      this.ckLRCFinder.Text = "LRCFinder";
      // 
      // ckLyrDB
      // 
      this.ckLyrDB.Checked = true;
      this.ckLyrDB.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckLyrDB.Id = "14e3c95b-c46b-4b04-bb02-e4c4d8e549c0";
      this.ckLyrDB.Localisation = "LyrDB";
      this.ckLyrDB.LocalisationContext = "Settings";
      this.ckLyrDB.Location = new System.Drawing.Point(432, 28);
      this.ckLyrDB.Name = "ckLyrDB";
      this.ckLyrDB.Size = new System.Drawing.Size(55, 26);
      this.ckLyrDB.TabIndex = 6;
      this.ckLyrDB.Text = "LyrDB";
      // 
      // ckActionext
      // 
      this.ckActionext.Checked = true;
      this.ckActionext.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckActionext.Id = "b9632e78-1a2f-4599-bf3b-fa0c0f5f5f9a";
      this.ckActionext.Localisation = "Actionext";
      this.ckActionext.LocalisationContext = "Settings";
      this.ckActionext.Location = new System.Drawing.Point(288, 60);
      this.ckActionext.Name = "ckActionext";
      this.ckActionext.Size = new System.Drawing.Size(70, 26);
      this.ckActionext.TabIndex = 5;
      this.ckActionext.Text = "Actionext";
      // 
      // ckLyricsPlugin
      // 
      this.ckLyricsPlugin.Checked = true;
      this.ckLyricsPlugin.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckLyricsPlugin.Id = "47a9cf89-a178-4e59-8ecd-e253994f7176";
      this.ckLyricsPlugin.Localisation = "LyricsPlugin";
      this.ckLyricsPlugin.LocalisationContext = "Settings";
      this.ckLyricsPlugin.Location = new System.Drawing.Point(145, 60);
      this.ckLyricsPlugin.Name = "ckLyricsPlugin";
      this.ckLyricsPlugin.Size = new System.Drawing.Size(82, 26);
      this.ckLyricsPlugin.TabIndex = 4;
      this.ckLyricsPlugin.Text = "LyricsPlugin";
      // 
      // ckLyricsOnDemand
      // 
      this.ckLyricsOnDemand.Checked = true;
      this.ckLyricsOnDemand.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckLyricsOnDemand.Id = "fef42135-2e92-4470-ac77-16f7a6791775";
      this.ckLyricsOnDemand.Localisation = "LyricsOnDemand";
      this.ckLyricsOnDemand.LocalisationContext = "Settings";
      this.ckLyricsOnDemand.Location = new System.Drawing.Point(15, 60);
      this.ckLyricsOnDemand.Name = "ckLyricsOnDemand";
      this.ckLyricsOnDemand.Size = new System.Drawing.Size(113, 26);
      this.ckLyricsOnDemand.TabIndex = 3;
      this.ckLyricsOnDemand.Text = "Lyrics On Demand";
      // 
      // ckLyrics007
      // 
      this.ckLyrics007.Checked = true;
      this.ckLyrics007.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckLyrics007.Id = "1c0e07f0-fd1d-43b3-8b7d-ea61a61a80ca";
      this.ckLyrics007.Localisation = "Lyrics007";
      this.ckLyrics007.LocalisationContext = "Settings";
      this.ckLyrics007.Location = new System.Drawing.Point(288, 28);
      this.ckLyrics007.Name = "ckLyrics007";
      this.ckLyrics007.Size = new System.Drawing.Size(71, 26);
      this.ckLyrics007.TabIndex = 2;
      this.ckLyrics007.Text = "Lyrics007";
      // 
      // ckHotLyrics
      // 
      this.ckHotLyrics.Checked = true;
      this.ckHotLyrics.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckHotLyrics.Id = "de7e3b04-77b7-4162-b9b0-caaa90c52201";
      this.ckHotLyrics.Localisation = "HotLyrics";
      this.ckHotLyrics.LocalisationContext = "Settings";
      this.ckHotLyrics.Location = new System.Drawing.Point(145, 28);
      this.ckHotLyrics.Name = "ckHotLyrics";
      this.ckHotLyrics.Size = new System.Drawing.Size(73, 26);
      this.ckHotLyrics.TabIndex = 1;
      this.ckHotLyrics.Text = "Hot Lyrics";
      // 
      // ckLyricWiki
      // 
      this.ckLyricWiki.Checked = true;
      this.ckLyricWiki.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckLyricWiki.Id = "c4fe6501-7176-4604-b7d3-42bfb998edbf";
      this.ckLyricWiki.Localisation = "LyricWiki";
      this.ckLyricWiki.LocalisationContext = "Settings";
      this.ckLyricWiki.Location = new System.Drawing.Point(15, 28);
      this.ckLyricWiki.Name = "ckLyricWiki";
      this.ckLyricWiki.Size = new System.Drawing.Size(72, 26);
      this.ckLyricWiki.TabIndex = 0;
      this.ckLyricWiki.Text = "Lyric Wiki";
      // 
      // tabPageSettingsDatabase
      // 
      this.tabPageSettingsDatabase.ActiveControl = null;
      this.tabPageSettingsDatabase.Controls.Add(this.groupBoxDatabaseBuild);
      this.tabPageSettingsDatabase.Controls.Add(this.groubBoxTagsDatabase);
      this.tabPageSettingsDatabase.KeyTip = null;
      this.tabPageSettingsDatabase.Name = "tabPageSettingsDatabase";
      this.tabPageSettingsDatabase.Size = new System.Drawing.Size(604, 1010);
      this.tabPageSettingsDatabase.TabIndex = 5;
      // 
      // groupBoxDatabaseBuild
      // 
      this.groupBoxDatabaseBuild.Controls.Add(this.checkBoxClearDatabase);
      this.groupBoxDatabaseBuild.Controls.Add(this.lbDBScanStatus);
      this.groupBoxDatabaseBuild.Controls.Add(this.buttonDBScanStatus);
      this.groupBoxDatabaseBuild.Controls.Add(this.buttonStartDatabaseScan);
      this.groupBoxDatabaseBuild.Controls.Add(this.lbDatabaseNote);
      this.groupBoxDatabaseBuild.Id = "dd99fa5f-cc46-451d-a914-b3d588dd4e31";
      this.groupBoxDatabaseBuild.Localisation = "GroupBoxDatabaseBuild";
      this.groupBoxDatabaseBuild.LocalisationContext = "Settings";
      this.groupBoxDatabaseBuild.Location = new System.Drawing.Point(10, 142);
      this.groupBoxDatabaseBuild.Name = "groupBoxDatabaseBuild";
      this.groupBoxDatabaseBuild.Size = new System.Drawing.Size(572, 189);
      this.groupBoxDatabaseBuild.TabIndex = 3;
      this.groupBoxDatabaseBuild.Text = "Music Database Build";
      // 
      // checkBoxClearDatabase
      // 
      this.checkBoxClearDatabase.Id = "6a6d9dfe-3904-4efa-b455-748c76114022";
      this.checkBoxClearDatabase.Localisation = "ClearDatabase";
      this.checkBoxClearDatabase.LocalisationContext = "Settings";
      this.checkBoxClearDatabase.Location = new System.Drawing.Point(13, 65);
      this.checkBoxClearDatabase.Name = "checkBoxClearDatabase";
      this.checkBoxClearDatabase.Size = new System.Drawing.Size(215, 26);
      this.checkBoxClearDatabase.TabIndex = 4;
      this.checkBoxClearDatabase.Text = "Clear database content before scanning";
      // 
      // lbDBScanStatus
      // 
      this.lbDBScanStatus.Localisation = "mptLabel1";
      this.lbDBScanStatus.LocalisationContext = "groupBoxDatabaseBuild";
      this.lbDBScanStatus.Location = new System.Drawing.Point(10, 150);
      this.lbDBScanStatus.Name = "lbDBScanStatus";
      this.lbDBScanStatus.Size = new System.Drawing.Size(10, 13);
      this.lbDBScanStatus.TabIndex = 3;
      this.lbDBScanStatus.Text = " ";
      // 
      // buttonDBScanStatus
      // 
      this.buttonDBScanStatus.Id = "c1bfa8b6-71d8-42f5-b72b-6f4857d77148";
      this.buttonDBScanStatus.Localisation = "DBScanStatus";
      this.buttonDBScanStatus.LocalisationContext = "Settings";
      this.buttonDBScanStatus.Location = new System.Drawing.Point(298, 95);
      this.buttonDBScanStatus.Name = "buttonDBScanStatus";
      this.buttonDBScanStatus.Size = new System.Drawing.Size(170, 36);
      this.buttonDBScanStatus.TabIndex = 2;
      this.buttonDBScanStatus.Text = "Scan Status";
      this.buttonDBScanStatus.UseVisualStyleBackColor = true;
      this.buttonDBScanStatus.Click += new System.EventHandler(this.buttonDBScanStatus_Click);
      // 
      // buttonStartDatabaseScan
      // 
      this.buttonStartDatabaseScan.Id = "e8f89fc7-7be1-4cac-aca9-1296b92ffc6e";
      this.buttonStartDatabaseScan.Localisation = "StartDBScan";
      this.buttonStartDatabaseScan.LocalisationContext = "Settings";
      this.buttonStartDatabaseScan.Location = new System.Drawing.Point(77, 95);
      this.buttonStartDatabaseScan.Name = "buttonStartDatabaseScan";
      this.buttonStartDatabaseScan.Size = new System.Drawing.Size(170, 36);
      this.buttonStartDatabaseScan.TabIndex = 1;
      this.buttonStartDatabaseScan.Text = "Start Scan";
      this.buttonStartDatabaseScan.UseVisualStyleBackColor = true;
      this.buttonStartDatabaseScan.Click += new System.EventHandler(this.buttonStartDatabaseScan_Click);
      // 
      // lbDatabaseNote
      // 
      this.lbDatabaseNote.Localisation = "DatabaseScanNote";
      this.lbDatabaseNote.LocalisationContext = "Settings";
      this.lbDatabaseNote.Location = new System.Drawing.Point(7, 25);
      this.lbDatabaseNote.Name = "lbDatabaseNote";
      this.lbDatabaseNote.Size = new System.Drawing.Size(515, 26);
      this.lbDatabaseNote.TabIndex = 0;
      this.lbDatabaseNote.Text = "If no Music Database is available, a database may be created, by entering a file " +
    "name in the above text box.\r\nThen select a folder and start the scan.";
      // 
      // groubBoxTagsDatabase
      // 
      this.groubBoxTagsDatabase.Controls.Add(this.buttonMusicDatabaseBrowse);
      this.groubBoxTagsDatabase.Controls.Add(this.tbMediaPortalDatabase);
      this.groubBoxTagsDatabase.Controls.Add(this.ckUseMediaPortalDatabase);
      this.groubBoxTagsDatabase.Id = "33322965-7324-45e2-9f89-815c5dfa6f34";
      this.groubBoxTagsDatabase.Localisation = "TabTagsDatabase";
      this.groubBoxTagsDatabase.LocalisationContext = "Settings";
      this.groubBoxTagsDatabase.Location = new System.Drawing.Point(10, 20);
      this.groubBoxTagsDatabase.Name = "groubBoxTagsDatabase";
      this.groubBoxTagsDatabase.Size = new System.Drawing.Size(572, 100);
      this.groubBoxTagsDatabase.TabIndex = 2;
      this.groubBoxTagsDatabase.Text = "Database";
      // 
      // buttonMusicDatabaseBrowse
      // 
      this.buttonMusicDatabaseBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonMusicDatabaseBrowse.Id = "992ffee8-c2ff-4351-a3c3-31ca4ef5da92";
      this.buttonMusicDatabaseBrowse.Localisation = "TargetFolderBrowse";
      this.buttonMusicDatabaseBrowse.LocalisationContext = "Settings";
      this.buttonMusicDatabaseBrowse.Location = new System.Drawing.Point(507, 52);
      this.buttonMusicDatabaseBrowse.Name = "buttonMusicDatabaseBrowse";
      this.buttonMusicDatabaseBrowse.Size = new System.Drawing.Size(46, 23);
      this.buttonMusicDatabaseBrowse.TabIndex = 7;
      this.buttonMusicDatabaseBrowse.Text = "...";
      this.buttonMusicDatabaseBrowse.UseVisualStyleBackColor = true;
      this.buttonMusicDatabaseBrowse.Click += new System.EventHandler(this.buttonMusicDatabaseBrowse_Click);
      // 
      // tbMediaPortalDatabase
      // 
      this.tbMediaPortalDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbMediaPortalDatabase.Location = new System.Drawing.Point(10, 54);
      this.tbMediaPortalDatabase.Name = "tbMediaPortalDatabase";
      this.tbMediaPortalDatabase.Size = new System.Drawing.Size(491, 20);
      this.tbMediaPortalDatabase.TabIndex = 6;
      // 
      // ckUseMediaPortalDatabase
      // 
      this.ckUseMediaPortalDatabase.Id = "f8b2a241-1fc6-4398-9678-4ffce9b388b9";
      this.ckUseMediaPortalDatabase.Localisation = "AutoCompletion";
      this.ckUseMediaPortalDatabase.LocalisationContext = "Settings";
      this.ckUseMediaPortalDatabase.Location = new System.Drawing.Point(10, 31);
      this.ckUseMediaPortalDatabase.Name = "ckUseMediaPortalDatabase";
      this.ckUseMediaPortalDatabase.Size = new System.Drawing.Size(307, 26);
      this.ckUseMediaPortalDatabase.TabIndex = 5;
      this.ckUseMediaPortalDatabase.Text = "Use MediaPortal\'s music database for Artist auto completion";
      // 
      // tabPageSettingsRipGeneral
      // 
      this.tabPageSettingsRipGeneral.ActiveControl = null;
      this.tabPageSettingsRipGeneral.Controls.Add(this.groupBoxEncoding);
      this.tabPageSettingsRipGeneral.Controls.Add(this.groupBoxRippingOptions);
      this.tabPageSettingsRipGeneral.Controls.Add(this.groupBoxCustomPath);
      this.tabPageSettingsRipGeneral.KeyTip = null;
      this.tabPageSettingsRipGeneral.Name = "tabPageSettingsRipGeneral";
      this.tabPageSettingsRipGeneral.Size = new System.Drawing.Size(604, 1010);
      this.tabPageSettingsRipGeneral.TabIndex = 6;
      // 
      // groupBoxEncoding
      // 
      this.groupBoxEncoding.Controls.Add(this.comboBoxEncoder);
      this.groupBoxEncoding.Controls.Add(this.lbEncodingFormat);
      this.groupBoxEncoding.Id = "87ce55be-db04-4477-bfe6-1da64cf31f9e";
      this.groupBoxEncoding.Localisation = "GroupBoxEncoding";
      this.groupBoxEncoding.LocalisationContext = "Settings";
      this.groupBoxEncoding.Location = new System.Drawing.Point(5, 30);
      this.groupBoxEncoding.Name = "groupBoxEncoding";
      this.groupBoxEncoding.Size = new System.Drawing.Size(577, 90);
      this.groupBoxEncoding.TabIndex = 6;
      this.groupBoxEncoding.Text = "Encoding Format";
      // 
      // comboBoxEncoder
      // 
      this.comboBoxEncoder.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxEncoder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxEncoder.Editable = false;
      this.comboBoxEncoder.FormattingEnabled = true;
      this.comboBoxEncoder.Id = "9830c835-569a-43c5-a8e1-e20db82afee2";
      this.comboBoxEncoder.Location = new System.Drawing.Point(9, 41);
      this.comboBoxEncoder.Name = "comboBoxEncoder";
      this.comboBoxEncoder.Size = new System.Drawing.Size(275, 21);
      this.comboBoxEncoder.TabIndex = 1;
      // 
      // lbEncodingFormat
      // 
      this.lbEncodingFormat.Localisation = "EncodingFormat";
      this.lbEncodingFormat.LocalisationContext = "Settings";
      this.lbEncodingFormat.Location = new System.Drawing.Point(6, 25);
      this.lbEncodingFormat.Name = "lbEncodingFormat";
      this.lbEncodingFormat.Size = new System.Drawing.Size(197, 13);
      this.lbEncodingFormat.TabIndex = 0;
      this.lbEncodingFormat.Text = "Select the format you wish to encode to:";
      // 
      // groupBoxRippingOptions
      // 
      this.groupBoxRippingOptions.Controls.Add(this.ckActivateTargetFolder);
      this.groupBoxRippingOptions.Controls.Add(this.ckRipEjectCD);
      this.groupBoxRippingOptions.Id = "b80c6f86-2f03-40eb-bb9f-ad1f14789275";
      this.groupBoxRippingOptions.Localisation = "RipOptions";
      this.groupBoxRippingOptions.LocalisationContext = "Settings";
      this.groupBoxRippingOptions.Location = new System.Drawing.Point(5, 376);
      this.groupBoxRippingOptions.Name = "groupBoxRippingOptions";
      this.groupBoxRippingOptions.Size = new System.Drawing.Size(577, 90);
      this.groupBoxRippingOptions.TabIndex = 7;
      this.groupBoxRippingOptions.Text = "Ripping OPtions";
      // 
      // ckActivateTargetFolder
      // 
      this.ckActivateTargetFolder.Id = "012a12b5-e98d-4b82-900d-6a00daafc8e0";
      this.ckActivateTargetFolder.Localisation = "ActivateTargetFolder";
      this.ckActivateTargetFolder.LocalisationContext = "Settings";
      this.ckActivateTargetFolder.Location = new System.Drawing.Point(19, 47);
      this.ckActivateTargetFolder.Name = "ckActivateTargetFolder";
      this.ckActivateTargetFolder.Size = new System.Drawing.Size(194, 26);
      this.ckActivateTargetFolder.TabIndex = 1;
      this.ckActivateTargetFolder.Text = "Activate Target Folder after Ripping";
      // 
      // ckRipEjectCD
      // 
      this.ckRipEjectCD.Checked = true;
      this.ckRipEjectCD.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckRipEjectCD.Id = "686954a4-6de6-4859-b5dc-4baab5df7f82";
      this.ckRipEjectCD.Localisation = "EjectCD";
      this.ckRipEjectCD.LocalisationContext = "Settings";
      this.ckRipEjectCD.Location = new System.Drawing.Point(19, 23);
      this.ckRipEjectCD.Name = "ckRipEjectCD";
      this.ckRipEjectCD.Size = new System.Drawing.Size(131, 26);
      this.ckRipEjectCD.TabIndex = 0;
      this.ckRipEjectCD.Text = "Eject CD after Ripping";
      // 
      // groupBoxCustomPath
      // 
      this.groupBoxCustomPath.Controls.Add(this.groupBoxRippingFormatOptions);
      this.groupBoxCustomPath.Controls.Add(this.textBoxRippingFilenameFormat);
      this.groupBoxCustomPath.Controls.Add(this.lbFormat);
      this.groupBoxCustomPath.Controls.Add(this.buttonTargetFolderBrowse);
      this.groupBoxCustomPath.Controls.Add(this.tbTargetFolder);
      this.groupBoxCustomPath.Controls.Add(this.lbTargetFolder);
      this.groupBoxCustomPath.Id = "6c66a6ed-17e9-4b1b-9cba-cf72061c884f";
      this.groupBoxCustomPath.Localisation = "CustomPath";
      this.groupBoxCustomPath.LocalisationContext = "Settings";
      this.groupBoxCustomPath.Location = new System.Drawing.Point(5, 134);
      this.groupBoxCustomPath.Name = "groupBoxCustomPath";
      this.groupBoxCustomPath.Size = new System.Drawing.Size(577, 235);
      this.groupBoxCustomPath.TabIndex = 5;
      this.groupBoxCustomPath.Text = "Custom Paths and Filenames";
      // 
      // groupBoxRippingFormatOptions
      // 
      this.groupBoxRippingFormatOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblParmFolder);
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblAlbumArtist);
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblParmGenre);
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblParmTrack);
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblParmYear);
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblParmAlbum);
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblParmTitle);
      this.groupBoxRippingFormatOptions.Controls.Add(this.lblParmArtist);
      this.groupBoxRippingFormatOptions.Id = "54c7bfaf-e87b-4767-89ba-47f308fafa13";
      this.groupBoxRippingFormatOptions.Localisation = "GroupBoxParm";
      this.groupBoxRippingFormatOptions.LocalisationContext = "TagAndRename";
      this.groupBoxRippingFormatOptions.Location = new System.Drawing.Point(9, 108);
      this.groupBoxRippingFormatOptions.Name = "groupBoxRippingFormatOptions";
      this.groupBoxRippingFormatOptions.Size = new System.Drawing.Size(551, 109);
      this.groupBoxRippingFormatOptions.TabIndex = 5;
      this.groupBoxRippingFormatOptions.Text = "Format Options (Click to insert)";
      // 
      // lblParmFolder
      // 
      this.lblParmFolder.Localisation = "Folder";
      this.lblParmFolder.LocalisationContext = "TagAndRename";
      this.lblParmFolder.Location = new System.Drawing.Point(11, 78);
      this.lblParmFolder.Name = "lblParmFolder";
      this.lblParmFolder.Size = new System.Drawing.Size(240, 13);
      this.lblParmFolder.TabIndex = 17;
      this.lblParmFolder.Text = "\\ = Folder: Create folder based on entered Format";
      this.lblParmFolder.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblAlbumArtist
      // 
      this.lblAlbumArtist.Localisation = "TagAndRename";
      this.lblAlbumArtist.LocalisationContext = "Settings";
      this.lblAlbumArtist.Location = new System.Drawing.Point(7, 55);
      this.lblAlbumArtist.Name = "lblAlbumArtist";
      this.lblAlbumArtist.Size = new System.Drawing.Size(151, 13);
      this.lblAlbumArtist.TabIndex = 16;
      this.lblAlbumArtist.Text = "<O> = Orchestra / Album Artist";
      this.lblAlbumArtist.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmGenre
      // 
      this.lblParmGenre.Localisation = "Genre";
      this.lblParmGenre.LocalisationContext = "TagAndRename";
      this.lblParmGenre.Location = new System.Drawing.Point(167, 37);
      this.lblParmGenre.Name = "lblParmGenre";
      this.lblParmGenre.Size = new System.Drawing.Size(68, 13);
      this.lblParmGenre.TabIndex = 15;
      this.lblParmGenre.Text = "<G> = Genre";
      this.lblParmGenre.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmTrack
      // 
      this.lblParmTrack.Localisation = "Track";
      this.lblParmTrack.LocalisationContext = "TagAndRename";
      this.lblParmTrack.Location = new System.Drawing.Point(326, 37);
      this.lblParmTrack.Name = "lblParmTrack";
      this.lblParmTrack.Size = new System.Drawing.Size(106, 13);
      this.lblParmTrack.TabIndex = 14;
      this.lblParmTrack.Text = "<K> = Track Number";
      this.lblParmTrack.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmYear
      // 
      this.lblParmYear.Localisation = "Year";
      this.lblParmYear.LocalisationContext = "TagAndRename";
      this.lblParmYear.Location = new System.Drawing.Point(7, 37);
      this.lblParmYear.Name = "lblParmYear";
      this.lblParmYear.Size = new System.Drawing.Size(60, 13);
      this.lblParmYear.TabIndex = 13;
      this.lblParmYear.Text = "<Y> = Year";
      this.lblParmYear.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmAlbum
      // 
      this.lblParmAlbum.Localisation = "Album";
      this.lblParmAlbum.LocalisationContext = "TagAndRename";
      this.lblParmAlbum.Location = new System.Drawing.Point(326, 20);
      this.lblParmAlbum.Name = "lblParmAlbum";
      this.lblParmAlbum.Size = new System.Drawing.Size(67, 13);
      this.lblParmAlbum.TabIndex = 12;
      this.lblParmAlbum.Text = "<B> = Album";
      this.lblParmAlbum.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmTitle
      // 
      this.lblParmTitle.Localisation = "Title";
      this.lblParmTitle.LocalisationContext = "TagAndRename";
      this.lblParmTitle.Location = new System.Drawing.Point(167, 20);
      this.lblParmTitle.Name = "lblParmTitle";
      this.lblParmTitle.Size = new System.Drawing.Size(58, 13);
      this.lblParmTitle.TabIndex = 11;
      this.lblParmTitle.Text = "<T> = Title";
      this.lblParmTitle.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmArtist
      // 
      this.lblParmArtist.Localisation = "Artist";
      this.lblParmArtist.LocalisationContext = "TagAndRename";
      this.lblParmArtist.Location = new System.Drawing.Point(5, 20);
      this.lblParmArtist.Name = "lblParmArtist";
      this.lblParmArtist.Size = new System.Drawing.Size(61, 13);
      this.lblParmArtist.TabIndex = 10;
      this.lblParmArtist.Text = "<A> = Artist";
      this.lblParmArtist.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // textBoxRippingFilenameFormat
      // 
      this.textBoxRippingFilenameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxRippingFilenameFormat.Location = new System.Drawing.Point(85, 71);
      this.textBoxRippingFilenameFormat.Name = "textBoxRippingFilenameFormat";
      this.textBoxRippingFilenameFormat.Size = new System.Drawing.Size(421, 20);
      this.textBoxRippingFilenameFormat.TabIndex = 4;
      this.textBoxRippingFilenameFormat.Text = "<A>\\<B>\\<K> - <T>";
      // 
      // lbFormat
      // 
      this.lbFormat.Localisation = "Format";
      this.lbFormat.LocalisationContext = "Settings";
      this.lbFormat.Location = new System.Drawing.Point(6, 74);
      this.lbFormat.Name = "lbFormat";
      this.lbFormat.Size = new System.Drawing.Size(42, 13);
      this.lbFormat.TabIndex = 3;
      this.lbFormat.Text = "Format:";
      // 
      // buttonTargetFolderBrowse
      // 
      this.buttonTargetFolderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonTargetFolderBrowse.Id = "8b8ae639-4e2c-4af4-9c50-13c18b2e89a0";
      this.buttonTargetFolderBrowse.Localisation = "TargetFolderBrowse";
      this.buttonTargetFolderBrowse.LocalisationContext = "Settings";
      this.buttonTargetFolderBrowse.Location = new System.Drawing.Point(514, 23);
      this.buttonTargetFolderBrowse.Name = "buttonTargetFolderBrowse";
      this.buttonTargetFolderBrowse.Size = new System.Drawing.Size(46, 23);
      this.buttonTargetFolderBrowse.TabIndex = 2;
      this.buttonTargetFolderBrowse.Text = "...";
      this.buttonTargetFolderBrowse.UseVisualStyleBackColor = true;
      this.buttonTargetFolderBrowse.Click += new System.EventHandler(this.buttonTargetFolderBrowse_Click);
      // 
      // tbTargetFolder
      // 
      this.tbTargetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbTargetFolder.Location = new System.Drawing.Point(85, 25);
      this.tbTargetFolder.Name = "tbTargetFolder";
      this.tbTargetFolder.Size = new System.Drawing.Size(421, 20);
      this.tbTargetFolder.TabIndex = 1;
      // 
      // lbTargetFolder
      // 
      this.lbTargetFolder.Localisation = "TargetFolder";
      this.lbTargetFolder.LocalisationContext = "Settings";
      this.lbTargetFolder.Location = new System.Drawing.Point(6, 29);
      this.lbTargetFolder.Name = "lbTargetFolder";
      this.lbTargetFolder.Size = new System.Drawing.Size(73, 13);
      this.lbTargetFolder.TabIndex = 0;
      this.lbTargetFolder.Text = "Target Folder:";
      // 
      // tabPageSettingsRipMp3
      // 
      this.tabPageSettingsRipMp3.ActiveControl = null;
      this.tabPageSettingsRipMp3.Controls.Add(this.groupBoxPresets);
      this.tabPageSettingsRipMp3.Controls.Add(this.groupBoxMp3Experts);
      this.tabPageSettingsRipMp3.KeyTip = null;
      this.tabPageSettingsRipMp3.Name = "tabPageSettingsRipMp3";
      this.tabPageSettingsRipMp3.Size = new System.Drawing.Size(604, 1010);
      this.tabPageSettingsRipMp3.TabIndex = 7;
      // 
      // groupBoxPresets
      // 
      this.groupBoxPresets.Controls.Add(this.textBoxPresetDesc);
      this.groupBoxPresets.Controls.Add(this.textBoxABRBitrate);
      this.groupBoxPresets.Controls.Add(this.lbABRBitrate);
      this.groupBoxPresets.Controls.Add(this.comboBoxLamePresets);
      this.groupBoxPresets.Controls.Add(this.lbPreset);
      this.groupBoxPresets.Id = "b7e210b8-851a-40c1-b015-f0bc6a7009cb";
      this.groupBoxPresets.Localisation = "GroupBoxPresets";
      this.groupBoxPresets.LocalisationContext = "Settings";
      this.groupBoxPresets.Location = new System.Drawing.Point(7, 28);
      this.groupBoxPresets.Name = "groupBoxPresets";
      this.groupBoxPresets.Size = new System.Drawing.Size(588, 271);
      this.groupBoxPresets.TabIndex = 2;
      this.groupBoxPresets.Text = "MP3 encoder presets";
      // 
      // textBoxPresetDesc
      // 
      this.textBoxPresetDesc.BackColor = System.Drawing.Color.LightSteelBlue;
      this.textBoxPresetDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBoxPresetDesc.Location = new System.Drawing.Point(10, 89);
      this.textBoxPresetDesc.Multiline = true;
      this.textBoxPresetDesc.Name = "textBoxPresetDesc";
      this.textBoxPresetDesc.ReadOnly = true;
      this.textBoxPresetDesc.Size = new System.Drawing.Size(558, 159);
      this.textBoxPresetDesc.TabIndex = 4;
      // 
      // textBoxABRBitrate
      // 
      this.textBoxABRBitrate.Location = new System.Drawing.Point(207, 50);
      this.textBoxABRBitrate.Name = "textBoxABRBitrate";
      this.textBoxABRBitrate.Size = new System.Drawing.Size(60, 20);
      this.textBoxABRBitrate.TabIndex = 3;
      // 
      // lbABRBitrate
      // 
      this.lbABRBitrate.Localisation = "ABRBitrate";
      this.lbABRBitrate.LocalisationContext = "Settings";
      this.lbABRBitrate.Location = new System.Drawing.Point(207, 30);
      this.lbABRBitrate.Name = "lbABRBitrate";
      this.lbABRBitrate.Size = new System.Drawing.Size(65, 13);
      this.lbABRBitrate.TabIndex = 2;
      this.lbABRBitrate.Text = "ABR Bitrate:";
      // 
      // comboBoxLamePresets
      // 
      this.comboBoxLamePresets.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxLamePresets.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxLamePresets.Editable = false;
      this.comboBoxLamePresets.FormattingEnabled = true;
      this.comboBoxLamePresets.Id = "238e6b37-036a-4251-a08c-959c21013d87";
      this.comboBoxLamePresets.Items.AddRange(new object[] {
            "Medium",
            "Standard",
            "Extreme",
            "Insane",
            "ABR Mode"});
      this.comboBoxLamePresets.Location = new System.Drawing.Point(9, 49);
      this.comboBoxLamePresets.Name = "comboBoxLamePresets";
      this.comboBoxLamePresets.Size = new System.Drawing.Size(154, 21);
      this.comboBoxLamePresets.TabIndex = 1;
      this.comboBoxLamePresets.SelectedIndexChanged += new System.EventHandler(this.comboBoxPreset_SelectedIndexChanged);
      // 
      // lbPreset
      // 
      this.lbPreset.Localisation = "Preset";
      this.lbPreset.LocalisationContext = "Settings";
      this.lbPreset.Location = new System.Drawing.Point(10, 31);
      this.lbPreset.Name = "lbPreset";
      this.lbPreset.Size = new System.Drawing.Size(40, 13);
      this.lbPreset.TabIndex = 0;
      this.lbPreset.Text = "Preset:";
      // 
      // groupBoxMp3Experts
      // 
      this.groupBoxMp3Experts.Controls.Add(this.lbLameExpertsWarning);
      this.groupBoxMp3Experts.Controls.Add(this.textBoxLameParms);
      this.groupBoxMp3Experts.Controls.Add(this.lbLameExpertOptions);
      this.groupBoxMp3Experts.Id = "e4d242e1-d3f5-44c8-b18c-b3cf1b95b3f4";
      this.groupBoxMp3Experts.Localisation = "GroupBoxMp3Experts";
      this.groupBoxMp3Experts.LocalisationContext = "Settings";
      this.groupBoxMp3Experts.Location = new System.Drawing.Point(6, 318);
      this.groupBoxMp3Experts.Name = "groupBoxMp3Experts";
      this.groupBoxMp3Experts.Size = new System.Drawing.Size(588, 112);
      this.groupBoxMp3Experts.TabIndex = 3;
      this.groupBoxMp3Experts.Text = "Expert settings";
      // 
      // lbLameExpertsWarning
      // 
      this.lbLameExpertsWarning.Localisation = "LameExpertsWarning";
      this.lbLameExpertsWarning.LocalisationContext = "Settings";
      this.lbLameExpertsWarning.Location = new System.Drawing.Point(13, 80);
      this.lbLameExpertsWarning.Name = "lbLameExpertsWarning";
      this.lbLameExpertsWarning.Size = new System.Drawing.Size(364, 13);
      this.lbLameExpertsWarning.TabIndex = 2;
      this.lbLameExpertsWarning.Text = "Warning: Preset settings will not be used, when specifying Lame parameters";
      // 
      // textBoxLameParms
      // 
      this.textBoxLameParms.Location = new System.Drawing.Point(13, 37);
      this.textBoxLameParms.Name = "textBoxLameParms";
      this.textBoxLameParms.Size = new System.Drawing.Size(565, 20);
      this.textBoxLameParms.TabIndex = 1;
      // 
      // lbLameExpertOptions
      // 
      this.lbLameExpertOptions.Localisation = "LameExpertOptions";
      this.lbLameExpertOptions.LocalisationContext = "Settings";
      this.lbLameExpertOptions.Location = new System.Drawing.Point(10, 20);
      this.lbLameExpertOptions.Name = "lbLameExpertOptions";
      this.lbLameExpertOptions.Size = new System.Drawing.Size(230, 13);
      this.lbLameExpertOptions.TabIndex = 0;
      this.lbLameExpertOptions.Text = "Enter LAME encoder parameters (Experts only):";
      // 
      // tabPageSettingsRipOgg
      // 
      this.tabPageSettingsRipOgg.ActiveControl = null;
      this.tabPageSettingsRipOgg.Controls.Add(this.groupBoxOggEncoding);
      this.tabPageSettingsRipOgg.Controls.Add(this.groupBoxOggExpert);
      this.tabPageSettingsRipOgg.KeyTip = null;
      this.tabPageSettingsRipOgg.Name = "tabPageSettingsRipOgg";
      this.tabPageSettingsRipOgg.Size = new System.Drawing.Size(604, 1002);
      this.tabPageSettingsRipOgg.TabIndex = 8;
      // 
      // groupBoxOggEncoding
      // 
      this.groupBoxOggEncoding.Controls.Add(this.lbOggQualitySelected);
      this.groupBoxOggEncoding.Controls.Add(this.lbOggQuality);
      this.groupBoxOggEncoding.Controls.Add(this.hScrollBarOggEncodingQuality);
      this.groupBoxOggEncoding.Id = "384d1e29-b6d6-4999-94a5-371b2132eb69";
      this.groupBoxOggEncoding.Localisation = "GroupBoxOggEncoding";
      this.groupBoxOggEncoding.LocalisationContext = "Settings";
      this.groupBoxOggEncoding.Location = new System.Drawing.Point(9, 28);
      this.groupBoxOggEncoding.Name = "groupBoxOggEncoding";
      this.groupBoxOggEncoding.Size = new System.Drawing.Size(583, 99);
      this.groupBoxOggEncoding.TabIndex = 3;
      this.groupBoxOggEncoding.Text = "OGG encoding settings";
      // 
      // lbOggQualitySelected
      // 
      this.lbOggQualitySelected.Localisation = "OggQualitySelected";
      this.lbOggQualitySelected.LocalisationContext = "Settings";
      this.lbOggQualitySelected.Location = new System.Drawing.Point(566, 48);
      this.lbOggQualitySelected.Name = "lbOggQualitySelected";
      this.lbOggQualitySelected.Size = new System.Drawing.Size(13, 13);
      this.lbOggQualitySelected.TabIndex = 2;
      this.lbOggQualitySelected.Text = "3";
      // 
      // lbOggQuality
      // 
      this.lbOggQuality.Localisation = "OggQuality";
      this.lbOggQuality.LocalisationContext = "Settings";
      this.lbOggQuality.Location = new System.Drawing.Point(16, 48);
      this.lbOggQuality.Name = "lbOggQuality";
      this.lbOggQuality.Size = new System.Drawing.Size(42, 13);
      this.lbOggQuality.TabIndex = 1;
      this.lbOggQuality.Text = "Quality:";
      // 
      // hScrollBarOggEncodingQuality
      // 
      this.hScrollBarOggEncodingQuality.LargeChange = 1;
      this.hScrollBarOggEncodingQuality.Location = new System.Drawing.Point(124, 40);
      this.hScrollBarOggEncodingQuality.Maximum = 10;
      this.hScrollBarOggEncodingQuality.Minimum = -2;
      this.hScrollBarOggEncodingQuality.Name = "hScrollBarOggEncodingQuality";
      this.hScrollBarOggEncodingQuality.Size = new System.Drawing.Size(426, 25);
      this.hScrollBarOggEncodingQuality.TabIndex = 0;
      this.hScrollBarOggEncodingQuality.Value = 3;
      this.hScrollBarOggEncodingQuality.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarOggEncodingQuality_Scroll);
      // 
      // groupBoxOggExpert
      // 
      this.groupBoxOggExpert.Controls.Add(this.lbOggExpertWarning);
      this.groupBoxOggExpert.Controls.Add(this.textBoxOggParms);
      this.groupBoxOggExpert.Controls.Add(this.lbOggExpert);
      this.groupBoxOggExpert.Id = "cc9a8983-6810-46e4-9124-ebab3736e645";
      this.groupBoxOggExpert.Localisation = "GroupBoxOggExpert";
      this.groupBoxOggExpert.LocalisationContext = "Settings";
      this.groupBoxOggExpert.Location = new System.Drawing.Point(7, 142);
      this.groupBoxOggExpert.Name = "groupBoxOggExpert";
      this.groupBoxOggExpert.Size = new System.Drawing.Size(585, 94);
      this.groupBoxOggExpert.TabIndex = 4;
      this.groupBoxOggExpert.Text = "Expert settings";
      // 
      // lbOggExpertWarning
      // 
      this.lbOggExpertWarning.Localisation = "OggExpertWarning";
      this.lbOggExpertWarning.LocalisationContext = "Settings";
      this.lbOggExpertWarning.Location = new System.Drawing.Point(10, 70);
      this.lbOggExpertWarning.Name = "lbOggExpertWarning";
      this.lbOggExpertWarning.Size = new System.Drawing.Size(413, 13);
      this.lbOggExpertWarning.TabIndex = 2;
      this.lbOggExpertWarning.Text = "Warning: The settings above will not be used, when specifying Ogg Expert paramete" +
    "rs";
      // 
      // textBoxOggParms
      // 
      this.textBoxOggParms.Location = new System.Drawing.Point(13, 37);
      this.textBoxOggParms.Name = "textBoxOggParms";
      this.textBoxOggParms.Size = new System.Drawing.Size(539, 20);
      this.textBoxOggParms.TabIndex = 1;
      // 
      // lbOggExpert
      // 
      this.lbOggExpert.Localisation = "OggExpert";
      this.lbOggExpert.LocalisationContext = "Settings";
      this.lbOggExpert.Location = new System.Drawing.Point(10, 20);
      this.lbOggExpert.Name = "lbOggExpert";
      this.lbOggExpert.Size = new System.Drawing.Size(221, 13);
      this.lbOggExpert.TabIndex = 0;
      this.lbOggExpert.Text = "Enter Ogg encoder parameters (Experts only):";
      // 
      // tabPageSettingsRipFlac
      // 
      this.tabPageSettingsRipFlac.ActiveControl = null;
      this.tabPageSettingsRipFlac.Controls.Add(this.groupBoxFlacEncoding);
      this.tabPageSettingsRipFlac.Controls.Add(this.groupBoxFlacSettings);
      this.tabPageSettingsRipFlac.KeyTip = null;
      this.tabPageSettingsRipFlac.Name = "tabPageSettingsRipFlac";
      this.tabPageSettingsRipFlac.Size = new System.Drawing.Size(604, 1002);
      this.tabPageSettingsRipFlac.TabIndex = 9;
      // 
      // groupBoxFlacEncoding
      // 
      this.groupBoxFlacEncoding.Controls.Add(this.lbFlacQualitySelected);
      this.groupBoxFlacEncoding.Controls.Add(this.lbFlacQuality);
      this.groupBoxFlacEncoding.Controls.Add(this.hScrollBarFlacEncodingQuality);
      this.groupBoxFlacEncoding.Id = "d264b8c5-79d7-444b-959b-576e347151e6";
      this.groupBoxFlacEncoding.Localisation = "GroupBoxFlacEncoding";
      this.groupBoxFlacEncoding.LocalisationContext = "Settings";
      this.groupBoxFlacEncoding.Location = new System.Drawing.Point(7, 28);
      this.groupBoxFlacEncoding.Name = "groupBoxFlacEncoding";
      this.groupBoxFlacEncoding.Size = new System.Drawing.Size(577, 99);
      this.groupBoxFlacEncoding.TabIndex = 5;
      this.groupBoxFlacEncoding.Text = "FLAC encoding settings";
      // 
      // lbFlacQualitySelected
      // 
      this.lbFlacQualitySelected.Localisation = "FlacQualitySelected";
      this.lbFlacQualitySelected.LocalisationContext = "Settings";
      this.lbFlacQualitySelected.Location = new System.Drawing.Point(567, 48);
      this.lbFlacQualitySelected.Name = "lbFlacQualitySelected";
      this.lbFlacQualitySelected.Size = new System.Drawing.Size(13, 13);
      this.lbFlacQualitySelected.TabIndex = 2;
      this.lbFlacQualitySelected.Text = "4";
      // 
      // lbFlacQuality
      // 
      this.lbFlacQuality.Localisation = "FlacQuality";
      this.lbFlacQuality.LocalisationContext = "Settings";
      this.lbFlacQuality.Location = new System.Drawing.Point(16, 48);
      this.lbFlacQuality.Name = "lbFlacQuality";
      this.lbFlacQuality.Size = new System.Drawing.Size(42, 13);
      this.lbFlacQuality.TabIndex = 1;
      this.lbFlacQuality.Text = "Quality:";
      // 
      // hScrollBarFlacEncodingQuality
      // 
      this.hScrollBarFlacEncodingQuality.LargeChange = 1;
      this.hScrollBarFlacEncodingQuality.Location = new System.Drawing.Point(126, 40);
      this.hScrollBarFlacEncodingQuality.Maximum = 8;
      this.hScrollBarFlacEncodingQuality.Name = "hScrollBarFlacEncodingQuality";
      this.hScrollBarFlacEncodingQuality.Size = new System.Drawing.Size(424, 25);
      this.hScrollBarFlacEncodingQuality.TabIndex = 0;
      this.hScrollBarFlacEncodingQuality.Value = 4;
      this.hScrollBarFlacEncodingQuality.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarFlacEncodingQuality_Scroll);
      // 
      // groupBoxFlacSettings
      // 
      this.groupBoxFlacSettings.Controls.Add(this.lbFlacExpertsWarning);
      this.groupBoxFlacSettings.Controls.Add(this.textBoxFlacParms);
      this.groupBoxFlacSettings.Controls.Add(this.lbFlacExperts);
      this.groupBoxFlacSettings.Id = "9c934373-9dee-462f-9484-0a1abb4b4bb3";
      this.groupBoxFlacSettings.Localisation = "GroupBoxFlacSettings";
      this.groupBoxFlacSettings.LocalisationContext = "Settings";
      this.groupBoxFlacSettings.Location = new System.Drawing.Point(5, 142);
      this.groupBoxFlacSettings.Name = "groupBoxFlacSettings";
      this.groupBoxFlacSettings.Size = new System.Drawing.Size(579, 99);
      this.groupBoxFlacSettings.TabIndex = 6;
      this.groupBoxFlacSettings.Text = "Expert settings";
      // 
      // lbFlacExpertsWarning
      // 
      this.lbFlacExpertsWarning.Localisation = "FlacExpertsWarning";
      this.lbFlacExpertsWarning.LocalisationContext = "Settings";
      this.lbFlacExpertsWarning.Location = new System.Drawing.Point(10, 70);
      this.lbFlacExpertsWarning.Name = "lbFlacExpertsWarning";
      this.lbFlacExpertsWarning.Size = new System.Drawing.Size(416, 13);
      this.lbFlacExpertsWarning.TabIndex = 2;
      this.lbFlacExpertsWarning.Text = "Warning: The settings above will not be used when specifying FLAC Expert paramete" +
    "rs";
      // 
      // textBoxFlacParms
      // 
      this.textBoxFlacParms.Location = new System.Drawing.Point(13, 37);
      this.textBoxFlacParms.Name = "textBoxFlacParms";
      this.textBoxFlacParms.Size = new System.Drawing.Size(539, 20);
      this.textBoxFlacParms.TabIndex = 1;
      // 
      // lbFlacExperts
      // 
      this.lbFlacExperts.Localisation = "FlacExperts";
      this.lbFlacExperts.LocalisationContext = "Settings";
      this.lbFlacExperts.Location = new System.Drawing.Point(10, 20);
      this.lbFlacExperts.Name = "lbFlacExperts";
      this.lbFlacExperts.Size = new System.Drawing.Size(227, 13);
      this.lbFlacExperts.TabIndex = 0;
      this.lbFlacExperts.Text = "Enter FLAC encoder parameters (Experts only):";
      // 
      // tabPageSettingsRipAAC
      // 
      this.tabPageSettingsRipAAC.ActiveControl = null;
      this.tabPageSettingsRipAAC.Controls.Add(this.groupBoxAACEncoding);
      this.tabPageSettingsRipAAC.KeyTip = null;
      this.tabPageSettingsRipAAC.Name = "tabPageSettingsRipAAC";
      this.tabPageSettingsRipAAC.Size = new System.Drawing.Size(604, 1002);
      this.tabPageSettingsRipAAC.TabIndex = 10;
      // 
      // groupBoxAACEncoding
      // 
      this.groupBoxAACEncoding.Controls.Add(this.comboBoxAACBitrates);
      this.groupBoxAACEncoding.Controls.Add(this.labelAACBitrate);
      this.groupBoxAACEncoding.Id = "bb2b7add-c549-4328-a473-f4f7c6824e8c";
      this.groupBoxAACEncoding.Localisation = "GroupBoxAACEncoding";
      this.groupBoxAACEncoding.LocalisationContext = "Settings";
      this.groupBoxAACEncoding.Location = new System.Drawing.Point(8, 25);
      this.groupBoxAACEncoding.Name = "groupBoxAACEncoding";
      this.groupBoxAACEncoding.Size = new System.Drawing.Size(587, 117);
      this.groupBoxAACEncoding.TabIndex = 1;
      this.groupBoxAACEncoding.Text = "AAC Encoding Settings";
      // 
      // comboBoxAACBitrates
      // 
      this.comboBoxAACBitrates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxAACBitrates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxAACBitrates.Editable = false;
      this.comboBoxAACBitrates.FormattingEnabled = true;
      this.comboBoxAACBitrates.Id = "387ea074-5ea4-4a46-bcad-06873c44d1f6";
      this.comboBoxAACBitrates.Location = new System.Drawing.Point(155, 38);
      this.comboBoxAACBitrates.Name = "comboBoxAACBitrates";
      this.comboBoxAACBitrates.Size = new System.Drawing.Size(97, 21);
      this.comboBoxAACBitrates.TabIndex = 3;
      // 
      // labelAACBitrate
      // 
      this.labelAACBitrate.Localisation = "AACBitRate";
      this.labelAACBitrate.LocalisationContext = "Settings";
      this.labelAACBitrate.Location = new System.Drawing.Point(8, 41);
      this.labelAACBitrate.Name = "labelAACBitrate";
      this.labelAACBitrate.Size = new System.Drawing.Size(40, 13);
      this.labelAACBitrate.TabIndex = 2;
      this.labelAACBitrate.Text = "Bitrate:";
      // 
      // tabPageSettingsRipWMA
      // 
      this.tabPageSettingsRipWMA.ActiveControl = null;
      this.tabPageSettingsRipWMA.Controls.Add(this.groupBoxWMAEncoding);
      this.tabPageSettingsRipWMA.KeyTip = null;
      this.tabPageSettingsRipWMA.Name = "tabPageSettingsRipWMA";
      this.tabPageSettingsRipWMA.Size = new System.Drawing.Size(604, 1002);
      this.tabPageSettingsRipWMA.TabIndex = 11;
      // 
      // groupBoxWMAEncoding
      // 
      this.groupBoxWMAEncoding.Controls.Add(this.comboBoxWMABitRate);
      this.groupBoxWMAEncoding.Controls.Add(this.comboBoxWMACbrVbr);
      this.groupBoxWMAEncoding.Controls.Add(this.labelWMAQuality);
      this.groupBoxWMAEncoding.Controls.Add(this.comboBoxWMASampleFormat);
      this.groupBoxWMAEncoding.Controls.Add(this.labelWMASampleFormat);
      this.groupBoxWMAEncoding.Controls.Add(this.comboBoxWMAEncoderFormat);
      this.groupBoxWMAEncoding.Controls.Add(this.labelWMAEncoderFormat);
      this.groupBoxWMAEncoding.Id = "897e37c5-e94e-4b5c-ad18-104e1b346826";
      this.groupBoxWMAEncoding.Localisation = "GroupBoxWMASettings";
      this.groupBoxWMAEncoding.LocalisationContext = "Settings";
      this.groupBoxWMAEncoding.Location = new System.Drawing.Point(8, 26);
      this.groupBoxWMAEncoding.Name = "groupBoxWMAEncoding";
      this.groupBoxWMAEncoding.Size = new System.Drawing.Size(577, 163);
      this.groupBoxWMAEncoding.TabIndex = 2;
      this.groupBoxWMAEncoding.Text = "WMA Encoding Settings";
      // 
      // comboBoxWMABitRate
      // 
      this.comboBoxWMABitRate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxWMABitRate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxWMABitRate.Editable = false;
      this.comboBoxWMABitRate.FormattingEnabled = true;
      this.comboBoxWMABitRate.Id = "7eb2489e-c8fe-43e2-94dd-48835a3eac48";
      this.comboBoxWMABitRate.Location = new System.Drawing.Point(294, 112);
      this.comboBoxWMABitRate.Name = "comboBoxWMABitRate";
      this.comboBoxWMABitRate.Size = new System.Drawing.Size(108, 21);
      this.comboBoxWMABitRate.TabIndex = 6;
      // 
      // comboBoxWMACbrVbr
      // 
      this.comboBoxWMACbrVbr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxWMACbrVbr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxWMACbrVbr.Editable = false;
      this.comboBoxWMACbrVbr.FormattingEnabled = true;
      this.comboBoxWMACbrVbr.Id = "6a4b342e-38b5-4133-a93b-21c074988af1";
      this.comboBoxWMACbrVbr.Location = new System.Drawing.Point(156, 112);
      this.comboBoxWMACbrVbr.Name = "comboBoxWMACbrVbr";
      this.comboBoxWMACbrVbr.Size = new System.Drawing.Size(110, 21);
      this.comboBoxWMACbrVbr.TabIndex = 5;
      this.comboBoxWMACbrVbr.SelectedIndexChanged += new System.EventHandler(this.comboBoxWMACbrVbr_SelectedIndexChanged);
      // 
      // labelWMAQuality
      // 
      this.labelWMAQuality.Localisation = "WMAQuality";
      this.labelWMAQuality.LocalisationContext = "Settings";
      this.labelWMAQuality.Location = new System.Drawing.Point(9, 115);
      this.labelWMAQuality.Name = "labelWMAQuality";
      this.labelWMAQuality.Size = new System.Drawing.Size(42, 13);
      this.labelWMAQuality.TabIndex = 4;
      this.labelWMAQuality.Text = "Quality:";
      // 
      // comboBoxWMASampleFormat
      // 
      this.comboBoxWMASampleFormat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxWMASampleFormat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxWMASampleFormat.Editable = false;
      this.comboBoxWMASampleFormat.FormattingEnabled = true;
      this.comboBoxWMASampleFormat.Id = "1606a2bd-80a4-43d2-b1ef-b1187f321539";
      this.comboBoxWMASampleFormat.Location = new System.Drawing.Point(156, 72);
      this.comboBoxWMASampleFormat.Name = "comboBoxWMASampleFormat";
      this.comboBoxWMASampleFormat.Size = new System.Drawing.Size(246, 21);
      this.comboBoxWMASampleFormat.TabIndex = 3;
      this.comboBoxWMASampleFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxWMASampleFormat_SelectedIndexChanged);
      // 
      // labelWMASampleFormat
      // 
      this.labelWMASampleFormat.Localisation = "WMASample";
      this.labelWMASampleFormat.LocalisationContext = "Settings";
      this.labelWMASampleFormat.Location = new System.Drawing.Point(9, 75);
      this.labelWMASampleFormat.Name = "labelWMASampleFormat";
      this.labelWMASampleFormat.Size = new System.Drawing.Size(80, 13);
      this.labelWMASampleFormat.TabIndex = 2;
      this.labelWMASampleFormat.Text = "Sample Format:";
      // 
      // comboBoxWMAEncoderFormat
      // 
      this.comboBoxWMAEncoderFormat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxWMAEncoderFormat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxWMAEncoderFormat.Editable = false;
      this.comboBoxWMAEncoderFormat.FormattingEnabled = true;
      this.comboBoxWMAEncoderFormat.Id = "62b4831a-7a8f-4604-a89e-447ce0198095";
      this.comboBoxWMAEncoderFormat.Location = new System.Drawing.Point(156, 32);
      this.comboBoxWMAEncoderFormat.Name = "comboBoxWMAEncoderFormat";
      this.comboBoxWMAEncoderFormat.Size = new System.Drawing.Size(246, 21);
      this.comboBoxWMAEncoderFormat.TabIndex = 1;
      this.comboBoxWMAEncoderFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxWMAEncoderFormat_SelectedIndexChanged);
      // 
      // labelWMAEncoderFormat
      // 
      this.labelWMAEncoderFormat.Localisation = "WMAEncoder";
      this.labelWMAEncoderFormat.LocalisationContext = "Settings";
      this.labelWMAEncoderFormat.Location = new System.Drawing.Point(9, 35);
      this.labelWMAEncoderFormat.Name = "labelWMAEncoderFormat";
      this.labelWMAEncoderFormat.Size = new System.Drawing.Size(85, 13);
      this.labelWMAEncoderFormat.TabIndex = 0;
      this.labelWMAEncoderFormat.Text = "Encoder Format:";
      // 
      // tabPageSettingsRipMPC
      // 
      this.tabPageSettingsRipMPC.ActiveControl = null;
      this.tabPageSettingsRipMPC.Controls.Add(this.groupBoxMPCExpert);
      this.tabPageSettingsRipMPC.Controls.Add(this.groupBoxMPCPresets);
      this.tabPageSettingsRipMPC.KeyTip = null;
      this.tabPageSettingsRipMPC.Name = "tabPageSettingsRipMPC";
      this.tabPageSettingsRipMPC.Size = new System.Drawing.Size(604, 1002);
      this.tabPageSettingsRipMPC.TabIndex = 12;
      // 
      // groupBoxMPCExpert
      // 
      this.groupBoxMPCExpert.Controls.Add(this.lbMPCExpertsWarning);
      this.groupBoxMPCExpert.Controls.Add(this.textBoxMPCParms);
      this.groupBoxMPCExpert.Controls.Add(this.lbMPCExperts);
      this.groupBoxMPCExpert.Id = "29973064-97c7-4e4e-a0e2-7f19a7af691c";
      this.groupBoxMPCExpert.Localisation = "GroupBoxMpcExperts";
      this.groupBoxMPCExpert.LocalisationContext = "Settings";
      this.groupBoxMPCExpert.Location = new System.Drawing.Point(6, 108);
      this.groupBoxMPCExpert.Name = "groupBoxMPCExpert";
      this.groupBoxMPCExpert.Size = new System.Drawing.Size(583, 112);
      this.groupBoxMPCExpert.TabIndex = 5;
      this.groupBoxMPCExpert.Text = "Expert settings";
      // 
      // lbMPCExpertsWarning
      // 
      this.lbMPCExpertsWarning.Localisation = "MpcExpertsWarning";
      this.lbMPCExpertsWarning.LocalisationContext = "Settings";
      this.lbMPCExpertsWarning.Location = new System.Drawing.Point(13, 80);
      this.lbMPCExpertsWarning.Name = "lbMPCExpertsWarning";
      this.lbMPCExpertsWarning.Size = new System.Drawing.Size(384, 13);
      this.lbMPCExpertsWarning.TabIndex = 2;
      this.lbMPCExpertsWarning.Text = "Warning: preset settings will not be used when specifying Musepack parameters";
      // 
      // textBoxMPCParms
      // 
      this.textBoxMPCParms.Location = new System.Drawing.Point(13, 37);
      this.textBoxMPCParms.Name = "textBoxMPCParms";
      this.textBoxMPCParms.Size = new System.Drawing.Size(527, 20);
      this.textBoxMPCParms.TabIndex = 1;
      // 
      // lbMPCExperts
      // 
      this.lbMPCExperts.Localisation = "MPCExpertOptions";
      this.lbMPCExperts.LocalisationContext = "Settings";
      this.lbMPCExperts.Location = new System.Drawing.Point(10, 20);
      this.lbMPCExperts.Name = "lbMPCExperts";
      this.lbMPCExperts.Size = new System.Drawing.Size(252, 13);
      this.lbMPCExperts.TabIndex = 0;
      this.lbMPCExperts.Text = "Enter Musepack Encoder parameters (Experts only):";
      // 
      // groupBoxMPCPresets
      // 
      this.groupBoxMPCPresets.Controls.Add(this.comboBoxMPCPresets);
      this.groupBoxMPCPresets.Controls.Add(this.lbMPCPresets);
      this.groupBoxMPCPresets.Id = "8bf4ea44-eb5e-4e4d-adfb-a9dc1fc7fadb";
      this.groupBoxMPCPresets.Localisation = "GroupBoxMPCPresets";
      this.groupBoxMPCPresets.LocalisationContext = "Settings";
      this.groupBoxMPCPresets.Location = new System.Drawing.Point(6, 30);
      this.groupBoxMPCPresets.Name = "groupBoxMPCPresets";
      this.groupBoxMPCPresets.Size = new System.Drawing.Size(583, 63);
      this.groupBoxMPCPresets.TabIndex = 4;
      this.groupBoxMPCPresets.Text = "Musepack encoder presets";
      // 
      // comboBoxMPCPresets
      // 
      this.comboBoxMPCPresets.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxMPCPresets.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxMPCPresets.Editable = false;
      this.comboBoxMPCPresets.FormattingEnabled = true;
      this.comboBoxMPCPresets.Id = "4dd36d8e-a57f-4c84-9504-d502595f1603";
      this.comboBoxMPCPresets.Location = new System.Drawing.Point(157, 23);
      this.comboBoxMPCPresets.Name = "comboBoxMPCPresets";
      this.comboBoxMPCPresets.Size = new System.Drawing.Size(421, 21);
      this.comboBoxMPCPresets.TabIndex = 1;
      this.comboBoxMPCPresets.TextEditorWidth = 402;
      // 
      // lbMPCPresets
      // 
      this.lbMPCPresets.Localisation = "MPCPreset";
      this.lbMPCPresets.LocalisationContext = "Settings";
      this.lbMPCPresets.Location = new System.Drawing.Point(7, 26);
      this.lbMPCPresets.Name = "lbMPCPresets";
      this.lbMPCPresets.Size = new System.Drawing.Size(40, 13);
      this.lbMPCPresets.TabIndex = 0;
      this.lbMPCPresets.Text = "Preset:";
      // 
      // tabPageSettingsRipWV
      // 
      this.tabPageSettingsRipWV.ActiveControl = null;
      this.tabPageSettingsRipWV.Controls.Add(this.groupBoxWVExpertSettings);
      this.tabPageSettingsRipWV.Controls.Add(this.groupBoxWVPresets);
      this.tabPageSettingsRipWV.KeyTip = null;
      this.tabPageSettingsRipWV.Name = "tabPageSettingsRipWV";
      this.tabPageSettingsRipWV.Size = new System.Drawing.Size(604, 1002);
      this.tabPageSettingsRipWV.TabIndex = 13;
      // 
      // groupBoxWVExpertSettings
      // 
      this.groupBoxWVExpertSettings.Controls.Add(this.lbWVExpertWarning);
      this.groupBoxWVExpertSettings.Controls.Add(this.textBoxWVParms);
      this.groupBoxWVExpertSettings.Controls.Add(this.lbWVExpert);
      this.groupBoxWVExpertSettings.Id = "4b5fcd74-444b-42d4-bc1f-c3ac02650784";
      this.groupBoxWVExpertSettings.Localisation = "groupBoxWvExperts";
      this.groupBoxWVExpertSettings.LocalisationContext = "Settings";
      this.groupBoxWVExpertSettings.Location = new System.Drawing.Point(6, 105);
      this.groupBoxWVExpertSettings.Name = "groupBoxWVExpertSettings";
      this.groupBoxWVExpertSettings.Size = new System.Drawing.Size(586, 97);
      this.groupBoxWVExpertSettings.TabIndex = 7;
      this.groupBoxWVExpertSettings.Text = "Expert Settings";
      // 
      // lbWVExpertWarning
      // 
      this.lbWVExpertWarning.Localisation = "WvExpertsWarning";
      this.lbWVExpertWarning.LocalisationContext = "Settings";
      this.lbWVExpertWarning.Location = new System.Drawing.Point(10, 69);
      this.lbWVExpertWarning.Name = "lbWVExpertWarning";
      this.lbWVExpertWarning.Size = new System.Drawing.Size(383, 13);
      this.lbWVExpertWarning.TabIndex = 2;
      this.lbWVExpertWarning.Text = "Warning: Preset settings will not be used, when specifying WavPackparameters";
      // 
      // textBoxWVParms
      // 
      this.textBoxWVParms.Location = new System.Drawing.Point(13, 37);
      this.textBoxWVParms.Name = "textBoxWVParms";
      this.textBoxWVParms.Size = new System.Drawing.Size(567, 20);
      this.textBoxWVParms.TabIndex = 1;
      // 
      // lbWVExpert
      // 
      this.lbWVExpert.Localisation = "WvExpertOptions";
      this.lbWVExpert.LocalisationContext = "Settings";
      this.lbWVExpert.Location = new System.Drawing.Point(10, 20);
      this.lbWVExpert.Name = "lbWVExpert";
      this.lbWVExpert.Size = new System.Drawing.Size(250, 13);
      this.lbWVExpert.TabIndex = 0;
      this.lbWVExpert.Text = "Enter WavPack Encoder parameters (Experts only):";
      // 
      // groupBoxWVPresets
      // 
      this.groupBoxWVPresets.Controls.Add(this.comboBoxWVPresets);
      this.groupBoxWVPresets.Controls.Add(this.lbWVPreset);
      this.groupBoxWVPresets.Id = "581e2c77-240e-4cb2-a60d-7139cbd5be8d";
      this.groupBoxWVPresets.Localisation = "GroupBoxWvPresets";
      this.groupBoxWVPresets.LocalisationContext = "Settings";
      this.groupBoxWVPresets.Location = new System.Drawing.Point(6, 27);
      this.groupBoxWVPresets.Name = "groupBoxWVPresets";
      this.groupBoxWVPresets.Size = new System.Drawing.Size(586, 63);
      this.groupBoxWVPresets.TabIndex = 6;
      this.groupBoxWVPresets.Text = "WavPack Encoder Presets";
      // 
      // comboBoxWVPresets
      // 
      this.comboBoxWVPresets.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxWVPresets.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxWVPresets.Editable = false;
      this.comboBoxWVPresets.FormattingEnabled = true;
      this.comboBoxWVPresets.Id = "bf2c5a91-adcc-4bab-a67a-75861b0749fa";
      this.comboBoxWVPresets.Location = new System.Drawing.Point(144, 23);
      this.comboBoxWVPresets.Name = "comboBoxWVPresets";
      this.comboBoxWVPresets.Size = new System.Drawing.Size(436, 21);
      this.comboBoxWVPresets.TabIndex = 1;
      this.comboBoxWVPresets.TextEditorWidth = 417;
      // 
      // lbWVPreset
      // 
      this.lbWVPreset.Localisation = "WvPreset";
      this.lbWVPreset.LocalisationContext = "Settings";
      this.lbWVPreset.Location = new System.Drawing.Point(7, 26);
      this.lbWVPreset.Name = "lbWVPreset";
      this.lbWVPreset.Size = new System.Drawing.Size(40, 13);
      this.lbWVPreset.TabIndex = 0;
      this.lbWVPreset.Text = "Preset:";
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.buttonSettingsCancel);
      this.panel3.Controls.Add(this.buttonSettingsApply);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel3.Location = new System.Drawing.Point(222, 12);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(676, 56);
      this.panel3.TabIndex = 2;
      this.panel3.Text = "panel3";
      // 
      // buttonSettingsCancel
      // 
      this.buttonSettingsCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonSettingsCancel.Id = "697af1c8-e67d-4f4a-849b-ca538b45a398";
      this.buttonSettingsCancel.Localisation = "Cancel";
      this.buttonSettingsCancel.LocalisationContext = "Settings";
      this.buttonSettingsCancel.Location = new System.Drawing.Point(134, 16);
      this.buttonSettingsCancel.Name = "buttonSettingsCancel";
      this.buttonSettingsCancel.Size = new System.Drawing.Size(100, 23);
      this.buttonSettingsCancel.TabIndex = 3;
      this.buttonSettingsCancel.Text = "Cancel";
      this.buttonSettingsCancel.UseVisualStyleBackColor = true;
      this.buttonSettingsCancel.Click += new System.EventHandler(this.buttonSettingsCancel_Click);
      // 
      // buttonSettingsApply
      // 
      this.buttonSettingsApply.Id = "d50361cd-8def-4428-9a29-2578d93c4536";
      this.buttonSettingsApply.Localisation = "Apply";
      this.buttonSettingsApply.LocalisationContext = "Settings";
      this.buttonSettingsApply.Location = new System.Drawing.Point(23, 16);
      this.buttonSettingsApply.Name = "buttonSettingsApply";
      this.buttonSettingsApply.Size = new System.Drawing.Size(100, 23);
      this.buttonSettingsApply.TabIndex = 2;
      this.buttonSettingsApply.Text = "Apply";
      this.buttonSettingsApply.UseVisualStyleBackColor = true;
      this.buttonSettingsApply.Click += new System.EventHandler(this.buttonSettingsApply_Click);
      // 
      // backstageViewPanel1
      // 
      this.backstageViewPanel1.Controls.Add(this.groupedNavigationBar1);
      this.backstageViewPanel1.Dock = System.Windows.Forms.DockStyle.Left;
      this.backstageViewPanel1.Location = new System.Drawing.Point(12, 12);
      this.backstageViewPanel1.Name = "backstageViewPanel1";
      this.backstageViewPanel1.Size = new System.Drawing.Size(210, 1081);
      this.backstageViewPanel1.TabIndex = 0;
      // 
      // groupedNavigationBar1
      // 
      this.groupedNavigationBar1.BehaviorMode = Elegant.Ui.GroupedNavigationBarBehaviorMode.Normal;
      this.groupedNavigationBar1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupedNavigationBar1.GroupHeaderImageMode = Elegant.Ui.GroupedNavigationBarGroupHeaderImageMode.SmallImages;
      this.groupedNavigationBar1.Groups.AddRange(new Elegant.Ui.NavigationBarGroup[] {
            this.navigationBarGroupGeneral,
            this.navigationBarGroupTags,
            this.navigationBarGroupRipConvert});
      this.groupedNavigationBar1.Id = "0e02f3a9-3b46-4372-b562-76ec70fa44da";
      this.groupedNavigationBar1.ItemActionMode = Elegant.Ui.GroupedNavigationBarItemActionMode.RadioSelection;
      this.groupedNavigationBar1.ItemDisplayMode = Elegant.Ui.GroupedNavigationBarItemDisplayMode.SmallImageTextHorizontal;
      this.groupedNavigationBar1.Location = new System.Drawing.Point(0, 0);
      this.groupedNavigationBar1.MinimizeButtonDirection = Elegant.Ui.NavigationBarCaptionMinimizeButtonDirection.Left;
      this.groupedNavigationBar1.Name = "groupedNavigationBar1";
      this.groupedNavigationBar1.Size = new System.Drawing.Size(210, 1081);
      this.groupedNavigationBar1.TabIndex = 0;
      this.groupedNavigationBar1.Text = "Settings";
      this.groupedNavigationBar1.VisibleOutlookGroupHeadersCount = 0;
      // 
      // navigationBarGroupGeneral
      // 
      this.navigationBarGroupGeneral.ContentControl = this.navigationBarGroupItemsContainer1;
      this.navigationBarGroupGeneral.ContentSizeMode = Elegant.Ui.NavigationBarGroupContentSizeMode.Auto;
      this.navigationBarGroupGeneral.Title = "General Settings";
      // 
      // navigationBarGroupItemsContainer1
      // 
      this.navigationBarGroupItemsContainer1.Id = "79a5b8aa-1158-4a8b-b651-9042291af4ff";
      this.navigationBarGroupItemsContainer1.ItemActionMode = Elegant.Ui.GroupedNavigationBarItemActionMode.RadioSelection;
      this.navigationBarGroupItemsContainer1.ItemDisplayMode = Elegant.Ui.GroupedNavigationBarItemDisplayMode.SmallImageTextHorizontal;
      this.navigationBarGroupItemsContainer1.Items.AddRange(new Elegant.Ui.NavigationBarItem[] {
            this.navigationBarItemGeneral,
            this.navigationBarItemKeys});
      this.navigationBarGroupItemsContainer1.Location = new System.Drawing.Point(0, 21);
      this.navigationBarGroupItemsContainer1.Name = "navigationBarGroupItemsContainer1";
      this.navigationBarGroupItemsContainer1.Size = new System.Drawing.Size(208, 10);
      this.navigationBarGroupItemsContainer1.TabIndex = 2;
      // 
      // navigationBarItemGeneral
      // 
      this.navigationBarItemGeneral.Id = "4db69fb3-e8aa-4ea2-a37e-19b60609bd39";
      this.navigationBarItemGeneral.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemGeneral.Name = "navigationBarItemGeneral";
      this.navigationBarItemGeneral.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemGeneral.TabIndex = 1;
      this.navigationBarItemGeneral.Text = "General";
      this.navigationBarItemGeneral.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemKeys
      // 
      this.navigationBarItemKeys.Id = "00b369ff-5784-4a70-b131-59a883bcdcc6";
      this.navigationBarItemKeys.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemKeys.Name = "navigationBarItemKeys";
      this.navigationBarItemKeys.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemKeys.TabIndex = 2;
      this.navigationBarItemKeys.Text = "Keys";
      this.navigationBarItemKeys.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarGroupTags
      // 
      this.navigationBarGroupTags.ContentControl = this.navigationBarGroupItemsContainer2;
      this.navigationBarGroupTags.ContentSizeMode = Elegant.Ui.NavigationBarGroupContentSizeMode.Auto;
      this.navigationBarGroupTags.Title = "Tags";
      // 
      // navigationBarGroupItemsContainer2
      // 
      this.navigationBarGroupItemsContainer2.Id = "189ea0e2-df4a-40b6-9b98-3fa2be8ae8ad";
      this.navigationBarGroupItemsContainer2.ItemActionMode = Elegant.Ui.GroupedNavigationBarItemActionMode.RadioSelection;
      this.navigationBarGroupItemsContainer2.ItemDisplayMode = Elegant.Ui.GroupedNavigationBarItemDisplayMode.SmallImageTextHorizontal;
      this.navigationBarGroupItemsContainer2.Items.AddRange(new Elegant.Ui.NavigationBarItem[] {
            this.navigationBarItemTagsGeneral,
            this.navigationBarItemTagsId3,
            this.navigationBarItemTagsLyricsCover,
            this.navigationBarItemTagsDatabase});
      this.navigationBarGroupItemsContainer2.Location = new System.Drawing.Point(0, 52);
      this.navigationBarGroupItemsContainer2.Name = "navigationBarGroupItemsContainer2";
      this.navigationBarGroupItemsContainer2.Size = new System.Drawing.Size(208, 10);
      this.navigationBarGroupItemsContainer2.TabIndex = 5;
      // 
      // navigationBarItemTagsGeneral
      // 
      this.navigationBarItemTagsGeneral.Id = "b3698914-5def-4d5b-9eb3-575e3de603a4";
      this.navigationBarItemTagsGeneral.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemTagsGeneral.Name = "navigationBarItemTagsGeneral";
      this.navigationBarItemTagsGeneral.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemTagsGeneral.TabIndex = 1;
      this.navigationBarItemTagsGeneral.Text = "General";
      this.navigationBarItemTagsGeneral.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemTagsId3
      // 
      this.navigationBarItemTagsId3.Id = "e5db1b69-dfd6-4c8f-b3e4-89191c88381e";
      this.navigationBarItemTagsId3.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemTagsId3.Name = "navigationBarItemTagsId3";
      this.navigationBarItemTagsId3.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemTagsId3.TabIndex = 2;
      this.navigationBarItemTagsId3.Text = "ID3";
      this.navigationBarItemTagsId3.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemTagsLyricsCover
      // 
      this.navigationBarItemTagsLyricsCover.Id = "8aca463b-fb85-407e-98db-f779d493d24f";
      this.navigationBarItemTagsLyricsCover.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemTagsLyricsCover.Name = "navigationBarItemTagsLyricsCover";
      this.navigationBarItemTagsLyricsCover.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemTagsLyricsCover.TabIndex = 3;
      this.navigationBarItemTagsLyricsCover.Text = "Lyrics / Cover";
      this.navigationBarItemTagsLyricsCover.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemTagsDatabase
      // 
      this.navigationBarItemTagsDatabase.Id = "88cfd696-ed66-40cf-8fc8-4b57612a86b1";
      this.navigationBarItemTagsDatabase.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemTagsDatabase.Name = "navigationBarItemTagsDatabase";
      this.navigationBarItemTagsDatabase.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemTagsDatabase.TabIndex = 4;
      this.navigationBarItemTagsDatabase.Text = "Database";
      this.navigationBarItemTagsDatabase.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarGroupRipConvert
      // 
      this.navigationBarGroupRipConvert.ContentControl = this.navigationBarGroupItemsContainer3;
      this.navigationBarGroupRipConvert.ContentSizeMode = Elegant.Ui.NavigationBarGroupContentSizeMode.Auto;
      this.navigationBarGroupRipConvert.Title = "Ripping / Conversion";
      // 
      // navigationBarGroupItemsContainer3
      // 
      this.navigationBarGroupItemsContainer3.Id = "3509e4a1-cfc9-44f8-8b22-7fc4b0b9dd24";
      this.navigationBarGroupItemsContainer3.ItemActionMode = Elegant.Ui.GroupedNavigationBarItemActionMode.RadioSelection;
      this.navigationBarGroupItemsContainer3.ItemDisplayMode = Elegant.Ui.GroupedNavigationBarItemDisplayMode.SmallImageTextHorizontal;
      this.navigationBarGroupItemsContainer3.Items.AddRange(new Elegant.Ui.NavigationBarItem[] {
            this.navigationBarItemRipGeneral,
            this.navigationBarItemRipMp3,
            this.navigationBarItemRipOgg,
            this.navigationBarItemFlac,
            this.navigationBarItemRipAAC,
            this.navigationBarItemRipWMA,
            this.navigationBarItemMPC,
            this.navigationBarItemRipWV});
      this.navigationBarGroupItemsContainer3.Location = new System.Drawing.Point(0, 83);
      this.navigationBarGroupItemsContainer3.Name = "navigationBarGroupItemsContainer3";
      this.navigationBarGroupItemsContainer3.Size = new System.Drawing.Size(208, 10);
      this.navigationBarGroupItemsContainer3.TabIndex = 8;
      // 
      // navigationBarItemRipGeneral
      // 
      this.navigationBarItemRipGeneral.Id = "f32def35-f20f-4311-a0f2-1c8355043d68";
      this.navigationBarItemRipGeneral.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemRipGeneral.Name = "navigationBarItemRipGeneral";
      this.navigationBarItemRipGeneral.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemRipGeneral.TabIndex = 1;
      this.navigationBarItemRipGeneral.Text = "General";
      this.navigationBarItemRipGeneral.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemRipMp3
      // 
      this.navigationBarItemRipMp3.Id = "9d8979f0-7c74-47e7-8197-4792f75f513f";
      this.navigationBarItemRipMp3.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemRipMp3.Name = "navigationBarItemRipMp3";
      this.navigationBarItemRipMp3.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemRipMp3.TabIndex = 2;
      this.navigationBarItemRipMp3.Text = "MP3";
      this.navigationBarItemRipMp3.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemRipOgg
      // 
      this.navigationBarItemRipOgg.Id = "6cd82a8c-1a93-4ea2-87e6-c226163612b2";
      this.navigationBarItemRipOgg.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemRipOgg.Name = "navigationBarItemRipOgg";
      this.navigationBarItemRipOgg.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemRipOgg.TabIndex = 3;
      this.navigationBarItemRipOgg.Text = "OGG";
      this.navigationBarItemRipOgg.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemFlac
      // 
      this.navigationBarItemFlac.Id = "92a49143-104e-4885-a538-db9557075803";
      this.navigationBarItemFlac.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemFlac.Name = "navigationBarItemFlac";
      this.navigationBarItemFlac.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemFlac.TabIndex = 4;
      this.navigationBarItemFlac.Text = "FLAC";
      this.navigationBarItemFlac.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemRipAAC
      // 
      this.navigationBarItemRipAAC.Id = "062f0637-9556-4f29-a6b4-ccd7c41f76be";
      this.navigationBarItemRipAAC.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemRipAAC.Name = "navigationBarItemRipAAC";
      this.navigationBarItemRipAAC.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemRipAAC.TabIndex = 5;
      this.navigationBarItemRipAAC.Text = "AAC";
      this.navigationBarItemRipAAC.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemRipWMA
      // 
      this.navigationBarItemRipWMA.Id = "16c82fc1-67af-4336-8771-0ba81707e707";
      this.navigationBarItemRipWMA.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemRipWMA.Name = "navigationBarItemRipWMA";
      this.navigationBarItemRipWMA.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemRipWMA.TabIndex = 6;
      this.navigationBarItemRipWMA.Text = "WMA";
      this.navigationBarItemRipWMA.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemMPC
      // 
      this.navigationBarItemMPC.Id = "0187f926-0f64-4f0f-aaa8-f834549b0c63";
      this.navigationBarItemMPC.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemMPC.Name = "navigationBarItemMPC";
      this.navigationBarItemMPC.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemMPC.TabIndex = 7;
      this.navigationBarItemMPC.Text = "MPC";
      this.navigationBarItemMPC.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // navigationBarItemRipWV
      // 
      this.navigationBarItemRipWV.Id = "c628d04e-f8e7-4091-be19-4a288c7176a0";
      this.navigationBarItemRipWV.Location = new System.Drawing.Point(0, 0);
      this.navigationBarItemRipWV.Name = "navigationBarItemRipWV";
      this.navigationBarItemRipWV.Size = new System.Drawing.Size(0, 0);
      this.navigationBarItemRipWV.TabIndex = 8;
      this.navigationBarItemRipWV.Text = "WV";
      this.navigationBarItemRipWV.Click += new System.EventHandler(this.navigatonBarItem_Click);
      // 
      // backstageViewButtonSave
      // 
      this.backstageViewButtonSave.CommandName = "Save";
      this.backstageViewButtonSave.Id = "32efd3ca-28d1-4652-86df-5fc3b5ec04e5";
      this.backstageViewButtonSave.ImageToTextSpace = 5;
      this.backstageViewButtonSave.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Default", ((System.Drawing.Image)(resources.GetObject("backstageViewButtonSave.LargeImages.Images")))),
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("backstageViewButtonSave.LargeImages.Images1"))))});
      this.backstageViewButtonSave.Location = new System.Drawing.Point(5, 22);
      this.backstageViewButtonSave.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.backstageViewButtonSave.Name = "backstageViewButtonSave";
      this.backstageViewButtonSave.Padding = new System.Windows.Forms.Padding(15, 9, 25, 9);
      this.backstageViewButtonSave.Size = new System.Drawing.Size(150, 34);
      this.backstageViewButtonSave.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Default", ((System.Drawing.Image)(resources.GetObject("backstageViewButtonSave.SmallImages.Images")))),
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("backstageViewButtonSave.SmallImages.Images1"))))});
      this.backstageViewButtonSave.TabIndex = 2;
      this.backstageViewButtonSave.Text = "Save";
      // 
      // backstageViewButtonRefresh
      // 
      this.backstageViewButtonRefresh.CommandName = "Refresh";
      this.backstageViewButtonRefresh.Id = "e76de925-453d-41f3-aa98-02e009a23f90";
      this.backstageViewButtonRefresh.ImageToTextSpace = 5;
      this.backstageViewButtonRefresh.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("backstageViewButtonRefresh.LargeImages.Images"))))});
      this.backstageViewButtonRefresh.Location = new System.Drawing.Point(5, 56);
      this.backstageViewButtonRefresh.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.backstageViewButtonRefresh.Name = "backstageViewButtonRefresh";
      this.backstageViewButtonRefresh.Padding = new System.Windows.Forms.Padding(15, 9, 25, 9);
      this.backstageViewButtonRefresh.Size = new System.Drawing.Size(150, 34);
      this.backstageViewButtonRefresh.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("backstageViewButtonRefresh.SmallImages.Images"))))});
      this.backstageViewButtonRefresh.TabIndex = 4;
      this.backstageViewButtonRefresh.Text = "Refresh";
      // 
      // backstageViewPageRecentFolders
      // 
      this.backstageViewPageRecentFolders.Controls.Add(this.panel1);
      this.backstageViewPageRecentFolders.Location = new System.Drawing.Point(0, 5);
      this.backstageViewPageRecentFolders.Name = "backstageViewPageRecentFolders";
      this.backstageViewPageRecentFolders.Padding = new System.Windows.Forms.Padding(12);
      this.backstageViewPageRecentFolders.Size = new System.Drawing.Size(840, 1020);
      this.backstageViewPageRecentFolders.TabIndex = 0;
      this.backstageViewPageRecentFolders.Text = "Recent Folders";
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.separatorRecentFolders);
      this.panel1.Controls.Add(this.pinListRecentFolders);
      this.panel1.Location = new System.Drawing.Point(33, 36);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(779, 913);
      this.panel1.TabIndex = 0;
      this.panel1.Text = "panel1";
      // 
      // separatorRecentFolders
      // 
      this.separatorRecentFolders.Id = "dc2159b0-fc8b-48bb-9a49-96fc2c9c61e9";
      this.separatorRecentFolders.Location = new System.Drawing.Point(16, 19);
      this.separatorRecentFolders.Name = "separatorRecentFolders";
      this.separatorRecentFolders.Size = new System.Drawing.Size(112, 20);
      this.separatorRecentFolders.TabIndex = 2;
      this.separatorRecentFolders.Text = "Recent Folders";
      // 
      // pinListRecentFolders
      // 
      this.pinListRecentFolders.Id = "085c0e4d-df88-45cf-b68e-c8dea1b7af05";
      this.pinListRecentFolders.Location = new System.Drawing.Point(16, 56);
      this.pinListRecentFolders.Name = "pinListRecentFolders";
      this.pinListRecentFolders.PinnedItemsSeparatorMargin = new System.Windows.Forms.Padding(0, 5, 0, 5);
      this.pinListRecentFolders.Size = new System.Drawing.Size(747, 837);
      this.pinListRecentFolders.TabIndex = 1;
      this.pinListRecentFolders.ItemClick += new Elegant.Ui.PinItemEventHandler(this.pinListRecentFolders_ItemClick);
      // 
      // backstageViewSeparator1
      // 
      this.backstageViewSeparator1.Id = "369d25d6-d37c-4006-867c-9c1e69f63586";
      this.backstageViewSeparator1.Location = new System.Drawing.Point(0, 184);
      this.backstageViewSeparator1.Name = "backstageViewSeparator1";
      this.backstageViewSeparator1.Size = new System.Drawing.Size(155, 2);
      this.backstageViewSeparator1.TabIndex = 6;
      this.backstageViewSeparator1.Text = "backstageViewSeparator1";
      // 
      // backstageViewButtonChangeColumns
      // 
      this.backstageViewButtonChangeColumns.CommandName = "ChangeDisplayColumns";
      this.backstageViewButtonChangeColumns.Id = "86ec7f12-cb60-44d1-8593-5d4c8b9a22ce";
      this.backstageViewButtonChangeColumns.ImageToTextSpace = 5;
      this.backstageViewButtonChangeColumns.Location = new System.Drawing.Point(5, 186);
      this.backstageViewButtonChangeColumns.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.backstageViewButtonChangeColumns.Name = "backstageViewButtonChangeColumns";
      this.backstageViewButtonChangeColumns.Padding = new System.Windows.Forms.Padding(15, 9, 25, 9);
      this.backstageViewButtonChangeColumns.Size = new System.Drawing.Size(150, 31);
      this.backstageViewButtonChangeColumns.TabIndex = 5;
      this.backstageViewButtonChangeColumns.Text = "Change Display Columns";
      // 
      // backstageViewSeparator2
      // 
      this.backstageViewSeparator2.Id = "1f44213c-f649-4525-8a34-f85c80e2607f";
      this.backstageViewSeparator2.Location = new System.Drawing.Point(0, 217);
      this.backstageViewSeparator2.Name = "backstageViewSeparator2";
      this.backstageViewSeparator2.Size = new System.Drawing.Size(155, 2);
      this.backstageViewSeparator2.TabIndex = 8;
      this.backstageViewSeparator2.Text = "backstageViewSeparator2";
      // 
      // backstageViewButtonExit
      // 
      this.backstageViewButtonExit.CommandName = "Exit";
      this.backstageViewButtonExit.Id = "c79011c7-3dcd-44b7-8414-e1276b7e331f";
      this.backstageViewButtonExit.ImageToTextSpace = 5;
      this.backstageViewButtonExit.Location = new System.Drawing.Point(5, 219);
      this.backstageViewButtonExit.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.backstageViewButtonExit.Name = "backstageViewButtonExit";
      this.backstageViewButtonExit.Padding = new System.Windows.Forms.Padding(15, 9, 25, 9);
      this.backstageViewButtonExit.Size = new System.Drawing.Size(150, 31);
      this.backstageViewButtonExit.TabIndex = 7;
      this.backstageViewButtonExit.Text = "Exit";
      // 
      // ribbonTabPageTag
      // 
      this.ribbonTabPageTag.Controls.Add(this.ribbonGroupTagsRetrieve);
      this.ribbonTabPageTag.Controls.Add(this.ribbonGroupTagsEdit);
      this.ribbonTabPageTag.Controls.Add(this.ribbonGroupPicture);
      this.ribbonTabPageTag.Controls.Add(this.ribbonGroupOrganise);
      this.ribbonTabPageTag.Controls.Add(this.ribbonGroupOther);
      this.ribbonTabPageTag.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ribbonTabPageTag.KeyTip = null;
      this.ribbonTabPageTag.Location = new System.Drawing.Point(0, 0);
      this.ribbonTabPageTag.Name = "ribbonTabPageTag";
      this.ribbonTabPageTag.Size = new System.Drawing.Size(1008, 101);
      this.ribbonTabPageTag.TabIndex = 0;
      this.ribbonTabPageTag.Tag = "Tags";
      this.ribbonTabPageTag.Text = "Tags";
      // 
      // ribbonGroupTagsRetrieve
      // 
      this.ribbonGroupTagsRetrieve.Controls.Add(this.buttonTagFromFile);
      this.ribbonGroupTagsRetrieve.Controls.Add(this.buttonTagIdentifyFiles);
      this.ribbonGroupTagsRetrieve.Controls.Add(this.buttonTagFromInternet);
      this.ribbonGroupTagsRetrieve.Location = new System.Drawing.Point(4, 3);
      this.ribbonGroupTagsRetrieve.Name = "ribbonGroupTagsRetrieve";
      this.ribbonGroupTagsRetrieve.Size = new System.Drawing.Size(162, 94);
      this.ribbonGroupTagsRetrieve.TabIndex = 1;
      this.ribbonGroupTagsRetrieve.Text = "Retrieve Tags";
      // 
      // buttonTagFromFile
      // 
      this.buttonTagFromFile.CommandName = "FileNameToTag";
      this.buttonTagFromFile.Id = "8ef0b3fa-4639-4da8-a32d-5b2811fb92e1";
      this.buttonTagFromFile.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonTagFromFile.LargeImages.Images"))))});
      this.buttonTagFromFile.Location = new System.Drawing.Point(4, 2);
      this.buttonTagFromFile.Name = "buttonTagFromFile";
      this.buttonTagFromFile.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonTagFromFile.ScreenTip.Image")));
      this.ribbonGroupTagsRetrieve.SetShortcutKeys(this.buttonTagFromFile, System.Windows.Forms.Keys.None);
      this.buttonTagFromFile.Size = new System.Drawing.Size(52, 72);
      this.buttonTagFromFile.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonTagFromFile.SmallImages.Images"))))});
      this.buttonTagFromFile.TabIndex = 0;
      this.buttonTagFromFile.Text = "Filename to Tag";
      this.buttonTagFromFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
      // 
      // buttonTagIdentifyFiles
      // 
      this.buttonTagIdentifyFiles.CommandName = "IdentifyFiles";
      this.buttonTagIdentifyFiles.Id = "ed6737da-d174-45ea-914a-07d738a04fa9";
      this.buttonTagIdentifyFiles.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonTagIdentifyFiles.LargeImages.Images"))))});
      this.buttonTagIdentifyFiles.Location = new System.Drawing.Point(58, 2);
      this.buttonTagIdentifyFiles.Name = "buttonTagIdentifyFiles";
      this.buttonTagIdentifyFiles.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonTagIdentifyFiles.ScreenTip.Image")));
      this.ribbonGroupTagsRetrieve.SetShortcutKeys(this.buttonTagIdentifyFiles, System.Windows.Forms.Keys.None);
      this.buttonTagIdentifyFiles.Size = new System.Drawing.Size(44, 72);
      this.buttonTagIdentifyFiles.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonTagIdentifyFiles.SmallImages.Images"))))});
      this.buttonTagIdentifyFiles.TabIndex = 1;
      this.buttonTagIdentifyFiles.Text = "Identify Files";
      // 
      // buttonTagFromInternet
      // 
      this.buttonTagFromInternet.CommandName = "TagFromInternet";
      this.buttonTagFromInternet.Id = "ba319a36-7b4b-4a50-b424-2a08ebe21a3b";
      this.buttonTagFromInternet.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonTagFromInternet.LargeImages.Images"))))});
      this.buttonTagFromInternet.Location = new System.Drawing.Point(104, 2);
      this.buttonTagFromInternet.Name = "buttonTagFromInternet";
      this.buttonTagFromInternet.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonTagFromInternet.ScreenTip.Image")));
      this.ribbonGroupTagsRetrieve.SetShortcutKeys(this.buttonTagFromInternet, System.Windows.Forms.Keys.None);
      this.buttonTagFromInternet.Size = new System.Drawing.Size(53, 72);
      this.buttonTagFromInternet.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonTagFromInternet.SmallImages.Images"))))});
      this.buttonTagFromInternet.TabIndex = 2;
      this.buttonTagFromInternet.Text = "Tag from Internet";
      // 
      // ribbonGroupTagsEdit
      // 
      this.ribbonGroupTagsEdit.Controls.Add(this.buttonCaseConversion);
      this.ribbonGroupTagsEdit.Controls.Add(this.buttonGetLyrics);
      this.ribbonGroupTagsEdit.Controls.Add(this.separator2);
      this.ribbonGroupTagsEdit.Controls.Add(this.buttonDeleteTag);
      this.ribbonGroupTagsEdit.Controls.Add(this.buttonRemoveComment);
      this.ribbonGroupTagsEdit.Controls.Add(this.separator3);
      this.ribbonGroupTagsEdit.Controls.Add(this.buttonGroup1);
      this.ribbonGroupTagsEdit.Controls.Add(this.buttonGroup3);
      this.ribbonGroupTagsEdit.Location = new System.Drawing.Point(168, 3);
      this.ribbonGroupTagsEdit.Name = "ribbonGroupTagsEdit";
      this.ribbonGroupTagsEdit.Size = new System.Drawing.Size(347, 94);
      this.ribbonGroupTagsEdit.TabIndex = 2;
      this.ribbonGroupTagsEdit.Text = "Edit Tags";
      // 
      // buttonCaseConversion
      // 
      this.buttonCaseConversion.ButtonScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonCaseConversion.ButtonScreenTip.Image")));
      this.buttonCaseConversion.CommandName = "CaseConversion";
      this.buttonCaseConversion.DescriptionText = null;
      this.buttonCaseConversion.Id = "000a4131-395d-420d-9e14-40811c6e525c";
      this.buttonCaseConversion.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonCaseConversion.LargeImages.Images"))))});
      this.buttonCaseConversion.Location = new System.Drawing.Point(4, 2);
      this.buttonCaseConversion.Name = "buttonCaseConversion";
      this.buttonCaseConversion.Popup = this.popupMenu3;
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonCaseConversion, System.Windows.Forms.Keys.None);
      this.buttonCaseConversion.Size = new System.Drawing.Size(74, 72);
      this.buttonCaseConversion.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonCaseConversion.SmallImages.Images"))))});
      this.buttonCaseConversion.TabIndex = 5;
      this.buttonCaseConversion.Text = "Case Conversion";
      // 
      // popupMenu3
      // 
      this.popupMenu3.Items.AddRange(new System.Windows.Forms.Control[] {
            this.buttonCaseConversionOptions});
      this.popupMenu3.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
      this.popupMenu3.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
      this.popupMenu3.Size = new System.Drawing.Size(100, 100);
      // 
      // buttonCaseConversionOptions
      // 
      this.buttonCaseConversionOptions.CommandName = "CaseConversionOptions";
      this.buttonCaseConversionOptions.Id = "a82e8bf2-5344-4ae5-bb46-5161f294f60f";
      this.buttonCaseConversionOptions.Location = new System.Drawing.Point(2, 2);
      this.buttonCaseConversionOptions.Name = "buttonCaseConversionOptions";
      this.buttonCaseConversionOptions.Size = new System.Drawing.Size(192, 23);
      this.buttonCaseConversionOptions.TabIndex = 2;
      this.buttonCaseConversionOptions.Text = "Case Conversion Options";
      // 
      // buttonGetLyrics
      // 
      this.buttonGetLyrics.CommandName = "GetLyrics";
      this.buttonGetLyrics.Id = "dd174cc5-5be9-45b7-9cf9-26c6eb813b52";
      this.buttonGetLyrics.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonGetLyrics.LargeImages.Images"))))});
      this.buttonGetLyrics.Location = new System.Drawing.Point(80, 2);
      this.buttonGetLyrics.Name = "buttonGetLyrics";
      this.buttonGetLyrics.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonGetLyrics.ScreenTip.Image")));
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonGetLyrics, System.Windows.Forms.Keys.None);
      this.buttonGetLyrics.Size = new System.Drawing.Size(42, 72);
      this.buttonGetLyrics.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonGetLyrics.SmallImages.Images"))))});
      this.buttonGetLyrics.TabIndex = 0;
      this.buttonGetLyrics.Text = "Get Lyrics";
      // 
      // separator2
      // 
      this.separator2.Id = "d7a68ab0-052d-457e-8ab3-95e30ecb244b";
      this.separator2.Location = new System.Drawing.Point(126, 7);
      this.separator2.Name = "separator2";
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.separator2, System.Windows.Forms.Keys.None);
      this.separator2.Size = new System.Drawing.Size(2, 60);
      this.separator2.TabIndex = 8;
      this.separator2.Text = "separator1";
      // 
      // buttonDeleteTag
      // 
      this.buttonDeleteTag.ButtonScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonDeleteTag.ButtonScreenTip.Image")));
      this.buttonDeleteTag.CommandName = "DeleteAllTags";
      this.buttonDeleteTag.DescriptionText = null;
      this.ribbonGroupTagsEdit.SetFlowBreak(this.buttonDeleteTag, true);
      this.buttonDeleteTag.Id = "3356cb8b-8ef8-4134-b8e4-8290dd07b0f0";
      this.buttonDeleteTag.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonDeleteTag.LargeImages.Images"))))});
      this.buttonDeleteTag.Location = new System.Drawing.Point(132, 2);
      this.buttonDeleteTag.Name = "buttonDeleteTag";
      this.buttonDeleteTag.Popup = this.popupMenu2;
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonDeleteTag, System.Windows.Forms.Keys.None);
      this.buttonDeleteTag.Size = new System.Drawing.Size(42, 72);
      this.buttonDeleteTag.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonDeleteTag.SmallImages.Images"))))});
      this.buttonDeleteTag.TabIndex = 1;
      this.buttonDeleteTag.Text = "Delete Tags";
      // 
      // popupMenu2
      // 
      this.popupMenu2.Items.AddRange(new System.Windows.Forms.Control[] {
            this.buttonDeleteAllTags,
            this.buttonDeleteID3v1,
            this.buttonDeleteID3v2});
      this.popupMenu2.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
      this.popupMenu2.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
      this.popupMenu2.Size = new System.Drawing.Size(100, 100);
      // 
      // buttonDeleteAllTags
      // 
      this.buttonDeleteAllTags.CommandName = "DeleteAllTags";
      this.buttonDeleteAllTags.Id = "52f93e54-be6f-4749-8fa8-8f201868c5c5";
      this.buttonDeleteAllTags.Location = new System.Drawing.Point(2, 2);
      this.buttonDeleteAllTags.Name = "buttonDeleteAllTags";
      this.buttonDeleteAllTags.Size = new System.Drawing.Size(156, 23);
      this.buttonDeleteAllTags.TabIndex = 2;
      this.buttonDeleteAllTags.Text = "Delete All Tags";
      // 
      // buttonDeleteID3v1
      // 
      this.buttonDeleteID3v1.CommandName = "DeleteID3v1";
      this.buttonDeleteID3v1.Id = "f5f872b6-0211-40c6-877d-474252a98a09";
      this.buttonDeleteID3v1.Location = new System.Drawing.Point(2, 25);
      this.buttonDeleteID3v1.Name = "buttonDeleteID3v1";
      this.buttonDeleteID3v1.Size = new System.Drawing.Size(156, 23);
      this.buttonDeleteID3v1.TabIndex = 3;
      this.buttonDeleteID3v1.Text = "Delete ID3 V1 Only";
      // 
      // buttonDeleteID3v2
      // 
      this.buttonDeleteID3v2.CommandName = "DeleteID3v2";
      this.buttonDeleteID3v2.Id = "269f63f1-5f47-4f71-9f4f-2964d45ee55f";
      this.buttonDeleteID3v2.Location = new System.Drawing.Point(2, 48);
      this.buttonDeleteID3v2.Name = "buttonDeleteID3v2";
      this.buttonDeleteID3v2.Size = new System.Drawing.Size(156, 23);
      this.buttonDeleteID3v2.TabIndex = 4;
      this.buttonDeleteID3v2.Text = "Delete ID3 V2 only";
      // 
      // buttonRemoveComment
      // 
      this.buttonRemoveComment.CommandName = "RemoveComment";
      this.buttonRemoveComment.Id = "cbb994ed-acd4-4eeb-92d2-692a3e92d5bb";
      this.buttonRemoveComment.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRemoveComment.LargeImages.Images"))))});
      this.buttonRemoveComment.Location = new System.Drawing.Point(176, 2);
      this.buttonRemoveComment.Name = "buttonRemoveComment";
      this.buttonRemoveComment.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonRemoveComment.ScreenTip.Image")));
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonRemoveComment, System.Windows.Forms.Keys.None);
      this.buttonRemoveComment.Size = new System.Drawing.Size(58, 72);
      this.buttonRemoveComment.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRemoveComment.SmallImages.Images"))))});
      this.buttonRemoveComment.TabIndex = 1;
      this.buttonRemoveComment.Text = "Remove Comment";
      // 
      // separator3
      // 
      this.separator3.Id = "6d913a4c-a584-4ec3-87a9-bae2f1d79f15";
      this.separator3.Location = new System.Drawing.Point(238, 7);
      this.separator3.Name = "separator3";
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.separator3, System.Windows.Forms.Keys.None);
      this.separator3.Size = new System.Drawing.Size(2, 60);
      this.separator3.TabIndex = 9;
      this.separator3.Text = "separator1";
      // 
      // buttonGroup1
      // 
      this.buttonGroup1.Controls.Add(this.comboBoxScripts);
      this.buttonGroup1.Controls.Add(this.buttonScriptExecute);
      this.buttonGroup1.Location = new System.Drawing.Point(243, 2);
      this.buttonGroup1.Name = "buttonGroup1";
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonGroup1, System.Windows.Forms.Keys.None);
      this.buttonGroup1.Size = new System.Drawing.Size(57, 24);
      this.buttonGroup1.TabIndex = 7;
      // 
      // comboBoxScripts
      // 
      this.comboBoxScripts.Editable = false;
      this.comboBoxScripts.FormattingEnabled = false;
      this.comboBoxScripts.Id = "517fc9cb-14b0-4fff-b350-cd5b098498c2";
      this.comboBoxScripts.LabelAreaWidthTemplate = "";
      this.comboBoxScripts.Location = new System.Drawing.Point(1, 0);
      this.comboBoxScripts.Name = "comboBoxScripts";
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.comboBoxScripts, System.Windows.Forms.Keys.None);
      this.comboBoxScripts.Size = new System.Drawing.Size(19, 23);
      this.comboBoxScripts.TabIndex = 0;
      this.comboBoxScripts.TextEditorWidth = 0;
      // 
      // buttonScriptExecute
      // 
      this.buttonScriptExecute.CommandName = "ScriptExecute";
      this.buttonScriptExecute.Id = "980e2572-f8a5-4c19-9f23-296539fb858b";
      this.buttonScriptExecute.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImage";
      this.buttonScriptExecute.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonScriptExecute.LargeImages.Images"))))});
      this.buttonScriptExecute.Location = new System.Drawing.Point(21, 0);
      this.buttonScriptExecute.Name = "buttonScriptExecute";
      this.buttonScriptExecute.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonScriptExecute.ScreenTip.Image")));
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonScriptExecute, System.Windows.Forms.Keys.None);
      this.buttonScriptExecute.Size = new System.Drawing.Size(26, 24);
      this.buttonScriptExecute.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonScriptExecute.SmallImages.Images"))))});
      this.buttonScriptExecute.TabIndex = 1;
      this.buttonScriptExecute.Text = "Execute Script";
      // 
      // buttonGroup3
      // 
      this.buttonGroup3.Controls.Add(this.buttonNumberOnClick);
      this.buttonGroup3.Controls.Add(this.buttonAutoNumber);
      this.buttonGroup3.Controls.Add(this.textBoxNumber);
      this.buttonGroup3.Location = new System.Drawing.Point(243, 26);
      this.buttonGroup3.Name = "buttonGroup3";
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonGroup3, System.Windows.Forms.Keys.None);
      this.buttonGroup3.Size = new System.Drawing.Size(100, 24);
      this.buttonGroup3.TabIndex = 6;
      // 
      // buttonNumberOnClick
      // 
      this.buttonNumberOnClick.CommandName = "";
      this.buttonNumberOnClick.Id = "4dc861f9-7417-4727-ad76-bcafca928b70";
      this.buttonNumberOnClick.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonNumberOnClick.LargeImages.Images"))))});
      this.buttonNumberOnClick.Location = new System.Drawing.Point(0, 0);
      this.buttonNumberOnClick.Name = "buttonNumberOnClick";
      this.buttonNumberOnClick.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonNumberOnClick.ScreenTip.Image")));
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonNumberOnClick, System.Windows.Forms.Keys.None);
      this.buttonNumberOnClick.Size = new System.Drawing.Size(26, 24);
      this.buttonNumberOnClick.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonNumberOnClick.SmallImages.Images"))))});
      this.buttonNumberOnClick.TabIndex = 2;
      this.buttonNumberOnClick.Text = "NumberOnClick";
      this.buttonNumberOnClick.PressedChanged += new System.EventHandler(this.buttonNumberOnClick_PressedChanged);
      // 
      // buttonAutoNumber
      // 
      this.buttonAutoNumber.CommandName = "AutoNumber";
      this.buttonAutoNumber.Id = "2cc1eb5a-16cc-4259-8d08-8827191cbe96";
      this.buttonAutoNumber.Location = new System.Drawing.Point(26, 0);
      this.buttonAutoNumber.Name = "buttonAutoNumber";
      this.buttonAutoNumber.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonAutoNumber.ScreenTip.Image")));
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.buttonAutoNumber, System.Windows.Forms.Keys.None);
      this.buttonAutoNumber.Size = new System.Drawing.Size(26, 24);
      this.buttonAutoNumber.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonAutoNumber.SmallImages.Images"))))});
      this.buttonAutoNumber.TabIndex = 0;
      this.buttonAutoNumber.Text = "button1";
      // 
      // textBoxNumber
      // 
      this.textBoxNumber.Id = "103acbe1-2662-4c90-b050-8dc9c604b5a0";
      this.textBoxNumber.Location = new System.Drawing.Point(53, 0);
      this.textBoxNumber.Margin = new System.Windows.Forms.Padding(1, 0, 10, 0);
      this.textBoxNumber.MaxLength = 4;
      this.textBoxNumber.MinimumSize = new System.Drawing.Size(60, 0);
      this.textBoxNumber.Name = "textBoxNumber";
      this.textBoxNumber.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
      this.ribbonGroupTagsEdit.SetShortcutKeys(this.textBoxNumber, System.Windows.Forms.Keys.None);
      this.textBoxNumber.Size = new System.Drawing.Size(60, 23);
      this.textBoxNumber.TabIndex = 2;
      this.textBoxNumber.Text = "1";
      this.textBoxNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.textBoxNumber.TextEditorWidth = 54;
      // 
      // ribbonGroupPicture
      // 
      this.ribbonGroupPicture.Controls.Add(this.galleryPicture);
      this.ribbonGroupPicture.Controls.Add(this.buttonGetCoverArt);
      this.ribbonGroupPicture.Controls.Add(this.buttonRemoveCoverArt);
      this.ribbonGroupPicture.Controls.Add(this.buttonSaveAsThumb);
      this.ribbonGroupPicture.Location = new System.Drawing.Point(517, 3);
      this.ribbonGroupPicture.Name = "ribbonGroupPicture";
      this.ribbonGroupPicture.Size = new System.Drawing.Size(118, 94);
      this.ribbonGroupPicture.TabIndex = 6;
      this.ribbonGroupPicture.Text = "Picture";
      // 
      // galleryPicture
      // 
      this.galleryPicture.AutoScrollMinSize = new System.Drawing.Size(0, 0);
      this.galleryPicture.Id = "dfef072e-e209-42c2-8a2e-be1fc6f59025";
      this.galleryPicture.ItemWidth = 62;
      this.galleryPicture.Location = new System.Drawing.Point(4, 5);
      this.galleryPicture.MinimumItemsInRowCount = 1;
      this.galleryPicture.Name = "galleryPicture";
      // 
      // 
      // 
      this.galleryPicture.Popup.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.galleryPicture.Popup.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
      this.galleryPicture.Popup.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
      this.galleryPicture.Popup.Size = new System.Drawing.Size(100, 100);
      this.ribbonGroupPicture.SetShortcutKeys(this.galleryPicture, System.Windows.Forms.Keys.None);
      this.galleryPicture.Size = new System.Drawing.Size(81, 66);
      this.galleryPicture.TabIndex = 0;
      this.galleryPicture.Text = "gallery1";
      this.galleryPicture.HoveredItemChanged += new System.EventHandler<Elegant.Ui.GalleryHoveredItemChangedEventArgs>(this.galleryPicture_HoveredItemChanged);
      // 
      // buttonGetCoverArt
      // 
      this.buttonGetCoverArt.CommandName = "GetCoverArt";
      this.buttonGetCoverArt.Id = "f1dc7735-ff15-4363-b440-cc9e920f258b";
      this.buttonGetCoverArt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.buttonGetCoverArt.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImage";
      this.buttonGetCoverArt.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonGetCoverArt.LargeImages.Images"))))});
      this.buttonGetCoverArt.Location = new System.Drawing.Point(87, 2);
      this.buttonGetCoverArt.Name = "buttonGetCoverArt";
      this.buttonGetCoverArt.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonGetCoverArt.ScreenTip.Image")));
      this.ribbonGroupPicture.SetShortcutKeys(this.buttonGetCoverArt, System.Windows.Forms.Keys.None);
      this.buttonGetCoverArt.Size = new System.Drawing.Size(26, 24);
      this.buttonGetCoverArt.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonGetCoverArt.SmallImages.Images"))))});
      this.buttonGetCoverArt.TabIndex = 2;
      this.buttonGetCoverArt.Text = "Get Cover Art";
      // 
      // buttonRemoveCoverArt
      // 
      this.buttonRemoveCoverArt.CommandName = "RemoveCoverArt";
      this.buttonRemoveCoverArt.Id = "a9bf3abe-795d-4f3a-8e9f-d4efb9b3c887";
      this.buttonRemoveCoverArt.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImage";
      this.buttonRemoveCoverArt.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRemoveCoverArt.LargeImages.Images"))))});
      this.buttonRemoveCoverArt.Location = new System.Drawing.Point(87, 26);
      this.buttonRemoveCoverArt.Name = "buttonRemoveCoverArt";
      this.buttonRemoveCoverArt.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonRemoveCoverArt.ScreenTip.Image")));
      this.ribbonGroupPicture.SetShortcutKeys(this.buttonRemoveCoverArt, System.Windows.Forms.Keys.None);
      this.buttonRemoveCoverArt.Size = new System.Drawing.Size(26, 24);
      this.buttonRemoveCoverArt.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRemoveCoverArt.SmallImages.Images"))))});
      this.buttonRemoveCoverArt.TabIndex = 3;
      this.buttonRemoveCoverArt.Text = "button3";
      // 
      // buttonSaveAsThumb
      // 
      this.buttonSaveAsThumb.CommandName = "SaveAsThumb";
      this.buttonSaveAsThumb.Id = "770274cc-208e-4e61-a8e1-f192edc88a1b";
      this.buttonSaveAsThumb.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImage";
      this.buttonSaveAsThumb.Location = new System.Drawing.Point(87, 50);
      this.buttonSaveAsThumb.Name = "buttonSaveAsThumb";
      this.buttonSaveAsThumb.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonSaveAsThumb.ScreenTip.Image")));
      this.ribbonGroupPicture.SetShortcutKeys(this.buttonSaveAsThumb, System.Windows.Forms.Keys.None);
      this.buttonSaveAsThumb.Size = new System.Drawing.Size(26, 24);
      this.buttonSaveAsThumb.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonSaveAsThumb.SmallImages.Images"))))});
      this.buttonSaveAsThumb.TabIndex = 4;
      this.buttonSaveAsThumb.Text = "button1";
      // 
      // ribbonGroupOrganise
      // 
      this.ribbonGroupOrganise.Controls.Add(this.buttonRenameFiles);
      this.ribbonGroupOrganise.Controls.Add(this.buttonOrganiseFiles);
      this.ribbonGroupOrganise.Location = new System.Drawing.Point(637, 3);
      this.ribbonGroupOrganise.Name = "ribbonGroupOrganise";
      this.ribbonGroupOrganise.Size = new System.Drawing.Size(111, 94);
      this.ribbonGroupOrganise.TabIndex = 3;
      this.ribbonGroupOrganise.Text = "Organise";
      // 
      // buttonRenameFiles
      // 
      this.buttonRenameFiles.ButtonScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonRenameFiles.ButtonScreenTip.Image")));
      this.buttonRenameFiles.CommandName = "RenameFiles";
      this.buttonRenameFiles.DescriptionText = null;
      this.buttonRenameFiles.Id = "6458a3fe-966e-4350-84a2-a2feea18c99d";
      this.buttonRenameFiles.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRenameFiles.LargeImages.Images"))))});
      this.buttonRenameFiles.Location = new System.Drawing.Point(4, 2);
      this.buttonRenameFiles.Name = "buttonRenameFiles";
      this.buttonRenameFiles.Popup = this.popupMenu1;
      this.ribbonGroupOrganise.SetShortcutKeys(this.buttonRenameFiles, System.Windows.Forms.Keys.None);
      this.buttonRenameFiles.Size = new System.Drawing.Size(49, 72);
      this.buttonRenameFiles.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRenameFiles.SmallImages.Images"))))});
      this.buttonRenameFiles.TabIndex = 0;
      this.buttonRenameFiles.Text = "Rename Files";
      // 
      // popupMenu1
      // 
      this.popupMenu1.Items.AddRange(new System.Windows.Forms.Control[] {
            this.buttonRenameFilesOptions});
      this.popupMenu1.KeepPopupsWithOffsetPlacementWithinPlacementArea = false;
      this.popupMenu1.PlacementMode = Elegant.Ui.PopupPlacementMode.Bottom;
      this.popupMenu1.Size = new System.Drawing.Size(100, 100);
      // 
      // buttonRenameFilesOptions
      // 
      this.buttonRenameFilesOptions.CommandName = "RenameFileOptions";
      this.buttonRenameFilesOptions.Id = "823a9d9f-69dd-45b7-b024-77fc09f23742";
      this.buttonRenameFilesOptions.Location = new System.Drawing.Point(2, 2);
      this.buttonRenameFilesOptions.Name = "buttonRenameFilesOptions";
      this.buttonRenameFilesOptions.Size = new System.Drawing.Size(173, 23);
      this.buttonRenameFilesOptions.TabIndex = 2;
      this.buttonRenameFilesOptions.Text = "Rename Files Options";
      // 
      // buttonOrganiseFiles
      // 
      this.buttonOrganiseFiles.CommandName = "OrganiseFiles";
      this.buttonOrganiseFiles.Id = "c936611c-9924-417d-b0dc-9219e83aa5a0";
      this.buttonOrganiseFiles.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonOrganiseFiles.LargeImages.Images"))))});
      this.buttonOrganiseFiles.Location = new System.Drawing.Point(55, 2);
      this.buttonOrganiseFiles.Name = "buttonOrganiseFiles";
      this.buttonOrganiseFiles.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonOrganiseFiles.ScreenTip.Image")));
      this.ribbonGroupOrganise.SetShortcutKeys(this.buttonOrganiseFiles, System.Windows.Forms.Keys.None);
      this.buttonOrganiseFiles.Size = new System.Drawing.Size(51, 72);
      this.buttonOrganiseFiles.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonOrganiseFiles.SmallImages.Images"))))});
      this.buttonOrganiseFiles.TabIndex = 1;
      this.buttonOrganiseFiles.Text = "Organise Files";
      // 
      // ribbonGroupOther
      // 
      this.ribbonGroupOther.Controls.Add(this.buttonAddToBurner);
      this.ribbonGroupOther.Controls.Add(this.buttonAddToConversion);
      this.ribbonGroupOther.Controls.Add(this.buttonAddToPlaylist);
      this.ribbonGroupOther.Location = new System.Drawing.Point(750, 3);
      this.ribbonGroupOther.Name = "ribbonGroupOther";
      this.ribbonGroupOther.Size = new System.Drawing.Size(133, 94);
      this.ribbonGroupOther.TabIndex = 5;
      this.ribbonGroupOther.Text = "Other";
      // 
      // buttonAddToBurner
      // 
      this.buttonAddToBurner.CommandName = "AddToBurner";
      this.buttonAddToBurner.Id = "33e48a90-67a6-4307-9fe6-5bcc8729a288";
      this.buttonAddToBurner.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImageWithText";
      this.buttonAddToBurner.Location = new System.Drawing.Point(4, 2);
      this.buttonAddToBurner.Name = "buttonAddToBurner";
      this.buttonAddToBurner.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddToBurner.ScreenTip.Image")));
      this.ribbonGroupOther.SetShortcutKeys(this.buttonAddToBurner, System.Windows.Forms.Keys.None);
      this.buttonAddToBurner.Size = new System.Drawing.Size(99, 24);
      this.buttonAddToBurner.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonAddToBurner.SmallImages.Images"))))});
      this.buttonAddToBurner.TabIndex = 0;
      this.buttonAddToBurner.Text = "Add to Burner";
      // 
      // buttonAddToConversion
      // 
      this.buttonAddToConversion.CommandName = "AddToConversion";
      this.buttonAddToConversion.Id = "9960458f-576c-4ef4-a763-4365a98433c8";
      this.buttonAddToConversion.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImageWithText";
      this.buttonAddToConversion.Location = new System.Drawing.Point(4, 26);
      this.buttonAddToConversion.Name = "buttonAddToConversion";
      this.buttonAddToConversion.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddToConversion.ScreenTip.Image")));
      this.ribbonGroupOther.SetShortcutKeys(this.buttonAddToConversion, System.Windows.Forms.Keys.None);
      this.buttonAddToConversion.Size = new System.Drawing.Size(124, 24);
      this.buttonAddToConversion.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonAddToConversion.SmallImages.Images"))))});
      this.buttonAddToConversion.TabIndex = 1;
      this.buttonAddToConversion.Text = "Add to Conversion";
      // 
      // buttonAddToPlaylist
      // 
      this.buttonAddToPlaylist.CommandName = "AddToPlaylist";
      this.buttonAddToPlaylist.Id = "869b0a70-3051-4248-a96a-88284fdeb54c";
      this.buttonAddToPlaylist.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImageWithText";
      this.buttonAddToPlaylist.Location = new System.Drawing.Point(4, 50);
      this.buttonAddToPlaylist.Name = "buttonAddToPlaylist";
      this.buttonAddToPlaylist.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddToPlaylist.ScreenTip.Image")));
      this.ribbonGroupOther.SetShortcutKeys(this.buttonAddToPlaylist, System.Windows.Forms.Keys.None);
      this.buttonAddToPlaylist.Size = new System.Drawing.Size(101, 24);
      this.buttonAddToPlaylist.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonAddToPlaylist.SmallImages.Images"))))});
      this.buttonAddToPlaylist.TabIndex = 2;
      this.buttonAddToPlaylist.Text = "Add to Playlist";
      // 
      // startMenuSave
      // 
      this.startMenuSave.CommandName = "Save";
      this.startMenuSave.Id = "38bad7fd-3fad-4393-8d19-7cad0ef3854b";
      this.startMenuSave.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("startMenuSave.LargeImages.Images"))))});
      this.startMenuSave.Location = new System.Drawing.Point(0, 0);
      this.startMenuSave.Name = "startMenuSave";
      this.startMenuSave.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("startMenuSave.ScreenTip.Image")));
      this.startMenuSave.Size = new System.Drawing.Size(195, 23);
      this.startMenuSave.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("startMenuSave.SmallImages.Images"))))});
      this.startMenuSave.TabIndex = 2;
      this.startMenuSave.Text = "Save";
      // 
      // startMenuRefresh
      // 
      this.startMenuRefresh.CommandName = "Refresh";
      this.startMenuRefresh.Id = "870d4a3f-624a-4d16-93e4-0a91f196bc2b";
      this.startMenuRefresh.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("startMenuRefresh.LargeImages.Images")))),
            new Elegant.Ui.ControlImage("Default", null)});
      this.startMenuRefresh.Location = new System.Drawing.Point(0, 23);
      this.startMenuRefresh.Name = "startMenuRefresh";
      this.startMenuRefresh.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("startMenuRefresh.ScreenTip.Image")));
      this.startMenuRefresh.Size = new System.Drawing.Size(195, 23);
      this.startMenuRefresh.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("startMenuRefresh.SmallImages.Images"))))});
      this.startMenuRefresh.TabIndex = 4;
      this.startMenuRefresh.Text = "Refresh";
      // 
      // ribbonTabPageRip
      // 
      this.ribbonTabPageRip.Controls.Add(this.ribbonGroupRip);
      this.ribbonTabPageRip.Controls.Add(this.ribbonGroupRipOptions);
      this.ribbonTabPageRip.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ribbonTabPageRip.KeyTip = null;
      this.ribbonTabPageRip.Location = new System.Drawing.Point(0, 0);
      this.ribbonTabPageRip.Name = "ribbonTabPageRip";
      this.ribbonTabPageRip.Size = new System.Drawing.Size(1166, 101);
      this.ribbonTabPageRip.TabIndex = 0;
      this.ribbonTabPageRip.Tag = "Rip";
      this.ribbonTabPageRip.Text = "Rip";
      // 
      // ribbonGroupRip
      // 
      this.ribbonGroupRip.Controls.Add(this.buttonRipStart);
      this.ribbonGroupRip.Controls.Add(this.buttonRipCancel);
      this.ribbonGroupRip.Location = new System.Drawing.Point(4, 3);
      this.ribbonGroupRip.Name = "ribbonGroupRip";
      this.ribbonGroupRip.Size = new System.Drawing.Size(95, 0);
      this.ribbonGroupRip.TabIndex = 1;
      // 
      // buttonRipStart
      // 
      this.buttonRipStart.CommandName = "RipStart";
      this.buttonRipStart.Id = "d001a4be-1aa7-4920-93da-c0dc3ec51e31";
      this.buttonRipStart.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRipStart.LargeImages.Images"))))});
      this.buttonRipStart.Location = new System.Drawing.Point(16, 2);
      this.buttonRipStart.Name = "buttonRipStart";
      this.buttonRipStart.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonRipStart.ScreenTip.Image")));
      this.ribbonGroupRip.SetShortcutKeys(this.buttonRipStart, System.Windows.Forms.Keys.None);
      this.buttonRipStart.Size = new System.Drawing.Size(25, 0);
      this.buttonRipStart.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRipStart.SmallImages.Images"))))});
      this.buttonRipStart.TabIndex = 4;
      this.buttonRipStart.Text = "Rip";
      // 
      // buttonRipCancel
      // 
      this.buttonRipCancel.CommandName = "RipCancel";
      this.buttonRipCancel.Id = "dd9e5847-c68c-44a1-9a44-246f6ae71cd5";
      this.buttonRipCancel.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRipCancel.LargeImages.Images"))))});
      this.buttonRipCancel.Location = new System.Drawing.Point(16, 2);
      this.buttonRipCancel.Name = "buttonRipCancel";
      this.buttonRipCancel.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonRipCancel.ScreenTip.Image")));
      this.ribbonGroupRip.SetShortcutKeys(this.buttonRipCancel, System.Windows.Forms.Keys.None);
      this.buttonRipCancel.Size = new System.Drawing.Size(61, 0);
      this.buttonRipCancel.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRipCancel.SmallImages.Images"))))});
      this.buttonRipCancel.TabIndex = 5;
      this.buttonRipCancel.Text = "Rip Cancel";
      // 
      // ribbonGroupRipOptions
      // 
      this.ribbonGroupRipOptions.Controls.Add(this.textBoxRipOutputFolder);
      this.ribbonGroupRipOptions.Controls.Add(this.comboBoxRipEncoder);
      this.ribbonGroupRipOptions.Controls.Add(this.buttonRipFolderSelect);
      this.ribbonGroupRipOptions.Location = new System.Drawing.Point(101, 3);
      this.ribbonGroupRipOptions.Name = "ribbonGroupRipOptions";
      this.ribbonGroupRipOptions.Size = new System.Drawing.Size(514, 0);
      this.ribbonGroupRipOptions.TabIndex = 0;
      this.ribbonGroupRipOptions.Text = "Rip Options";
      // 
      // textBoxRipOutputFolder
      // 
      this.textBoxRipOutputFolder.Id = "232a5cbf-0ce0-4aff-b9bf-1c7640ed1c28";
      this.textBoxRipOutputFolder.LabelAreaWidthTemplate = "Select Encoder:                      ";
      this.textBoxRipOutputFolder.LabelText = "Output Folder:";
      this.textBoxRipOutputFolder.Location = new System.Drawing.Point(4, 2);
      this.textBoxRipOutputFolder.Name = "textBoxRipOutputFolder";
      this.ribbonGroupRipOptions.SetShortcutKeys(this.textBoxRipOutputFolder, System.Windows.Forms.Keys.None);
      this.textBoxRipOutputFolder.Size = new System.Drawing.Size(473, 19);
      this.textBoxRipOutputFolder.TabIndex = 3;
      this.textBoxRipOutputFolder.TextEditorWidth = 322;
      // 
      // comboBoxRipEncoder
      // 
      this.comboBoxRipEncoder.Editable = false;
      this.ribbonGroupRipOptions.SetFlowBreak(this.comboBoxRipEncoder, true);
      this.comboBoxRipEncoder.FormattingEnabled = false;
      this.comboBoxRipEncoder.Id = "c8d5a621-31b2-4a30-81c4-ff37571165dc";
      this.comboBoxRipEncoder.LabelAreaWidthTemplate = "Select Encoder:                      ";
      this.comboBoxRipEncoder.LabelText = "Select Encoder:";
      this.comboBoxRipEncoder.Location = new System.Drawing.Point(479, 2);
      this.comboBoxRipEncoder.Name = "comboBoxRipEncoder";
      this.ribbonGroupRipOptions.SetShortcutKeys(this.comboBoxRipEncoder, System.Windows.Forms.Keys.None);
      this.comboBoxRipEncoder.Size = new System.Drawing.Size(473, 19);
      this.comboBoxRipEncoder.TabIndex = 2;
      this.comboBoxRipEncoder.TextEditorWidth = 309;
      // 
      // buttonRipFolderSelect
      // 
      this.buttonRipFolderSelect.CommandName = "FolderSelect";
      this.buttonRipFolderSelect.Id = "ea53cc78-57ab-4d19-91e8-afe6a1d99e7a";
      this.buttonRipFolderSelect.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImage";
      this.buttonRipFolderSelect.Location = new System.Drawing.Point(954, 2);
      this.buttonRipFolderSelect.Name = "buttonRipFolderSelect";
      this.ribbonGroupRipOptions.SetShortcutKeys(this.buttonRipFolderSelect, System.Windows.Forms.Keys.None);
      this.buttonRipFolderSelect.Size = new System.Drawing.Size(6, 0);
      this.buttonRipFolderSelect.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonRipFolderSelect.SmallImages.Images"))))});
      this.buttonRipFolderSelect.TabIndex = 4;
      // 
      // ribbonTabPageConvert
      // 
      this.ribbonTabPageConvert.Controls.Add(this.ribbonGroupConvert);
      this.ribbonTabPageConvert.Controls.Add(this.ribbonGroupConvertOptions);
      this.ribbonTabPageConvert.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ribbonTabPageConvert.KeyTip = null;
      this.ribbonTabPageConvert.Location = new System.Drawing.Point(0, 0);
      this.ribbonTabPageConvert.Name = "ribbonTabPageConvert";
      this.ribbonTabPageConvert.Size = new System.Drawing.Size(1166, 101);
      this.ribbonTabPageConvert.TabIndex = 0;
      this.ribbonTabPageConvert.Tag = "Convert";
      this.ribbonTabPageConvert.Text = "Convert";
      // 
      // ribbonGroupConvert
      // 
      this.ribbonGroupConvert.Controls.Add(this.buttonConvertStart);
      this.ribbonGroupConvert.Controls.Add(this.buttonConvertCancel);
      this.ribbonGroupConvert.Location = new System.Drawing.Point(4, 3);
      this.ribbonGroupConvert.Name = "ribbonGroupConvert";
      this.ribbonGroupConvert.Size = new System.Drawing.Size(155, 0);
      this.ribbonGroupConvert.TabIndex = 0;
      // 
      // buttonConvertStart
      // 
      this.buttonConvertStart.CommandName = "ConvertStart";
      this.buttonConvertStart.Id = "44c3e83e-aab8-4d1f-a1af-8986db19a11c";
      this.buttonConvertStart.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonConvertStart.LargeImages.Images"))))});
      this.buttonConvertStart.Location = new System.Drawing.Point(28, 2);
      this.buttonConvertStart.Name = "buttonConvertStart";
      this.buttonConvertStart.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonConvertStart.ScreenTip.Image")));
      this.ribbonGroupConvert.SetShortcutKeys(this.buttonConvertStart, System.Windows.Forms.Keys.None);
      this.buttonConvertStart.Size = new System.Drawing.Size(87, 0);
      this.buttonConvertStart.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonConvertStart.SmallImages.Images"))))});
      this.buttonConvertStart.TabIndex = 0;
      this.buttonConvertStart.Text = "Conversion Start";
      // 
      // buttonConvertCancel
      // 
      this.buttonConvertCancel.CommandName = "ConvertCancel";
      this.buttonConvertCancel.Id = "45c66776-33b0-4832-b771-490f19f60493";
      this.buttonConvertCancel.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonConvertCancel.LargeImages.Images"))))});
      this.buttonConvertCancel.Location = new System.Drawing.Point(28, 2);
      this.buttonConvertCancel.Name = "buttonConvertCancel";
      this.buttonConvertCancel.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonConvertCancel.ScreenTip.Image")));
      this.ribbonGroupConvert.SetShortcutKeys(this.buttonConvertCancel, System.Windows.Forms.Keys.None);
      this.buttonConvertCancel.Size = new System.Drawing.Size(98, 0);
      this.buttonConvertCancel.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonConvertCancel.SmallImages.Images"))))});
      this.buttonConvertCancel.TabIndex = 1;
      this.buttonConvertCancel.Text = "Conversion Cancel";
      // 
      // ribbonGroupConvertOptions
      // 
      this.ribbonGroupConvertOptions.Controls.Add(this.textBoxConvertOutputFolder);
      this.ribbonGroupConvertOptions.Controls.Add(this.comboBoxConvertEncoder);
      this.ribbonGroupConvertOptions.Controls.Add(this.buttonConvertFolderSelect);
      this.ribbonGroupConvertOptions.Location = new System.Drawing.Point(161, 3);
      this.ribbonGroupConvertOptions.Name = "ribbonGroupConvertOptions";
      this.ribbonGroupConvertOptions.Size = new System.Drawing.Size(535, 0);
      this.ribbonGroupConvertOptions.TabIndex = 1;
      this.ribbonGroupConvertOptions.Text = "Conversion Options";
      // 
      // textBoxConvertOutputFolder
      // 
      this.textBoxConvertOutputFolder.Id = "73b761e2-6141-479c-b192-9e0bb04c70d1";
      this.textBoxConvertOutputFolder.LabelAreaWidthTemplate = "Select Encoder:                      ";
      this.textBoxConvertOutputFolder.LabelText = "Output Folder:";
      this.textBoxConvertOutputFolder.Location = new System.Drawing.Point(4, 2);
      this.textBoxConvertOutputFolder.Name = "textBoxConvertOutputFolder";
      this.ribbonGroupConvertOptions.SetShortcutKeys(this.textBoxConvertOutputFolder, System.Windows.Forms.Keys.None);
      this.textBoxConvertOutputFolder.Size = new System.Drawing.Size(472, 19);
      this.textBoxConvertOutputFolder.TabIndex = 4;
      this.textBoxConvertOutputFolder.TextEditorWidth = 321;
      // 
      // comboBoxConvertEncoder
      // 
      this.comboBoxConvertEncoder.Editable = false;
      this.ribbonGroupConvertOptions.SetFlowBreak(this.comboBoxConvertEncoder, true);
      this.comboBoxConvertEncoder.FormattingEnabled = false;
      this.comboBoxConvertEncoder.Id = "c76d2510-fde1-4b0d-b3b6-23f77f4a44af";
      this.comboBoxConvertEncoder.LabelAreaWidthTemplate = "Select Encoder:                      ";
      this.comboBoxConvertEncoder.LabelText = "Select Encoder:";
      this.comboBoxConvertEncoder.Location = new System.Drawing.Point(478, 2);
      this.comboBoxConvertEncoder.Name = "comboBoxConvertEncoder";
      this.ribbonGroupConvertOptions.SetShortcutKeys(this.comboBoxConvertEncoder, System.Windows.Forms.Keys.None);
      this.comboBoxConvertEncoder.Size = new System.Drawing.Size(472, 19);
      this.comboBoxConvertEncoder.TabIndex = 5;
      this.comboBoxConvertEncoder.TextEditorWidth = 308;
      // 
      // buttonConvertFolderSelect
      // 
      this.buttonConvertFolderSelect.CommandName = "FolderSelect";
      this.buttonConvertFolderSelect.Id = "afb2c267-778d-4d4a-9b2e-37116c167693";
      this.buttonConvertFolderSelect.InformativenessMaximumLevel = "Elegant.Ui.RibbonGroupButtonInformativenessLevel:SmallImage";
      this.buttonConvertFolderSelect.Location = new System.Drawing.Point(952, 2);
      this.buttonConvertFolderSelect.Name = "buttonConvertFolderSelect";
      this.ribbonGroupConvertOptions.SetShortcutKeys(this.buttonConvertFolderSelect, System.Windows.Forms.Keys.None);
      this.buttonConvertFolderSelect.Size = new System.Drawing.Size(6, 0);
      this.buttonConvertFolderSelect.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonConvertFolderSelect.SmallImages.Images"))))});
      this.buttonConvertFolderSelect.TabIndex = 6;
      // 
      // ribbonTabPageBurn
      // 
      this.ribbonTabPageBurn.Controls.Add(this.ribbonGroupBurn);
      this.ribbonTabPageBurn.Controls.Add(this.ribbonGroupBurnOptions);
      this.ribbonTabPageBurn.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ribbonTabPageBurn.KeyTip = null;
      this.ribbonTabPageBurn.Location = new System.Drawing.Point(0, 0);
      this.ribbonTabPageBurn.Name = "ribbonTabPageBurn";
      this.ribbonTabPageBurn.Size = new System.Drawing.Size(1166, 101);
      this.ribbonTabPageBurn.TabIndex = 0;
      this.ribbonTabPageBurn.Tag = "Burn";
      this.ribbonTabPageBurn.Text = "Burn";
      // 
      // ribbonGroupBurn
      // 
      this.ribbonGroupBurn.Controls.Add(this.buttonBurnStart);
      this.ribbonGroupBurn.Controls.Add(this.buttonBurnCancel);
      this.ribbonGroupBurn.Location = new System.Drawing.Point(4, 3);
      this.ribbonGroupBurn.Name = "ribbonGroupBurn";
      this.ribbonGroupBurn.Size = new System.Drawing.Size(95, 0);
      this.ribbonGroupBurn.TabIndex = 0;
      // 
      // buttonBurnStart
      // 
      this.buttonBurnStart.CommandName = "BurnStart";
      this.buttonBurnStart.Id = "a741c025-7b86-4b1b-bf57-132c84317d09";
      this.buttonBurnStart.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonBurnStart.LargeImages.Images"))))});
      this.buttonBurnStart.Location = new System.Drawing.Point(13, 2);
      this.buttonBurnStart.Name = "buttonBurnStart";
      this.buttonBurnStart.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonBurnStart.ScreenTip.Image")));
      this.ribbonGroupBurn.SetShortcutKeys(this.buttonBurnStart, System.Windows.Forms.Keys.None);
      this.buttonBurnStart.Size = new System.Drawing.Size(56, 0);
      this.buttonBurnStart.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonBurnStart.SmallImages.Images"))))});
      this.buttonBurnStart.TabIndex = 0;
      this.buttonBurnStart.Text = "Burn Start";
      // 
      // buttonBurnCancel
      // 
      this.buttonBurnCancel.CommandName = "BurnCancel";
      this.buttonBurnCancel.Id = "227f00e7-f669-417b-8c6f-505d30d342a0";
      this.buttonBurnCancel.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonBurnCancel.LargeImages.Images"))))});
      this.buttonBurnCancel.Location = new System.Drawing.Point(13, 2);
      this.buttonBurnCancel.Name = "buttonBurnCancel";
      this.buttonBurnCancel.ScreenTip.Image = ((System.Drawing.Image)(resources.GetObject("buttonBurnCancel.ScreenTip.Image")));
      this.ribbonGroupBurn.SetShortcutKeys(this.buttonBurnCancel, System.Windows.Forms.Keys.None);
      this.buttonBurnCancel.Size = new System.Drawing.Size(67, 0);
      this.buttonBurnCancel.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonBurnCancel.SmallImages.Images"))))});
      this.buttonBurnCancel.TabIndex = 1;
      this.buttonBurnCancel.Text = "Burn Cancel";
      // 
      // ribbonGroupBurnOptions
      // 
      this.ribbonGroupBurnOptions.Controls.Add(this.comboBoxBurner);
      this.ribbonGroupBurnOptions.Controls.Add(this.comboBoxBurnerSpeed);
      this.ribbonGroupBurnOptions.Location = new System.Drawing.Point(101, 3);
      this.ribbonGroupBurnOptions.Name = "ribbonGroupBurnOptions";
      this.ribbonGroupBurnOptions.Size = new System.Drawing.Size(357, 0);
      this.ribbonGroupBurnOptions.TabIndex = 1;
      this.ribbonGroupBurnOptions.Text = "Burner Options";
      // 
      // comboBoxBurner
      // 
      this.comboBoxBurner.Editable = false;
      this.comboBoxBurner.FormattingEnabled = false;
      this.comboBoxBurner.Id = "91b0d24e-514a-4df6-acc7-83af9eecc776";
      this.comboBoxBurner.LabelAreaWidthTemplate = "Burner Speed:                          ";
      this.comboBoxBurner.LabelText = "Select Burner:";
      this.comboBoxBurner.Location = new System.Drawing.Point(4, 2);
      this.comboBoxBurner.Name = "comboBoxBurner";
      this.ribbonGroupBurnOptions.SetShortcutKeys(this.comboBoxBurner, System.Windows.Forms.Keys.None);
      this.comboBoxBurner.Size = new System.Drawing.Size(348, 19);
      this.comboBoxBurner.TabIndex = 0;
      this.comboBoxBurner.TextEditorWidth = 180;
      this.comboBoxBurner.SelectedIndexChanged += new System.EventHandler(this.comboBoxBurner_SelectedIndexChanged);
      // 
      // comboBoxBurnerSpeed
      // 
      this.comboBoxBurnerSpeed.Editable = false;
      this.comboBoxBurnerSpeed.FormattingEnabled = false;
      this.comboBoxBurnerSpeed.Id = "f13f2b7f-bee8-4078-8614-0efe27832435";
      this.comboBoxBurnerSpeed.LabelAreaWidthTemplate = "Burner Speed:                          ";
      this.comboBoxBurnerSpeed.LabelText = "Burner Speed:";
      this.comboBoxBurnerSpeed.Location = new System.Drawing.Point(354, 2);
      this.comboBoxBurnerSpeed.Name = "comboBoxBurnerSpeed";
      this.ribbonGroupBurnOptions.SetShortcutKeys(this.comboBoxBurnerSpeed, System.Windows.Forms.Keys.None);
      this.comboBoxBurnerSpeed.Size = new System.Drawing.Size(268, 19);
      this.comboBoxBurnerSpeed.TabIndex = 1;
      this.comboBoxBurnerSpeed.SelectedIndexChanged += new System.EventHandler(this.comboBoxBurnerSpeed_SelectedIndexChanged);
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1008, 1189);
      this.Controls.Add(this.backstageView);
      this.Controls.Add(this.panelMiddle);
      this.Controls.Add(this.ribbon);
      this.Controls.Add(this.splitterPlayer);
      this.Controls.Add(this.panelBottom);
      this.Controls.Add(this.statusBar);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Main";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "MPTagThat - The MediaPortal Tag Editor";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Close);
      this.Load += new System.EventHandler(this.Main_Load);
      this.Move += new System.EventHandler(this.Main_Move);
      this.Resize += new System.EventHandler(this.Main_Resize);
      this.panelBottom.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      this.panelMiddleTop.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
      this.statusBar.ResumeLayout(false);
      this.statusBar.PerformLayout();
      this.statusBarNotificationsArea1.ResumeLayout(false);
      this.statusBarNotificationsArea1.PerformLayout();
      this.statusBarPane2.ResumeLayout(false);
      this.statusBarPane2.PerformLayout();
      this.statusBarPane3.ResumeLayout(false);
      this.statusBarPane3.PerformLayout();
      this.statusBarControlsArea1.ResumeLayout(false);
      this.statusBarControlsArea1.PerformLayout();
      this.statusBarPane4.ResumeLayout(false);
      this.statusBarPane4.PerformLayout();
      this.statusBarPane1.ResumeLayout(false);
      this.statusBarPane1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.backstageView)).EndInit();
      this.backstageViewPageOptions.ResumeLayout(false);
      this.backstageViewPageOptions.PerformLayout();
      this.panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.tabControlSettings)).EndInit();
      this.tabPageSettingsGeneral.ResumeLayout(false);
      this.groupBoxGeneral.ResumeLayout(false);
      this.groupBoxGeneral.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrackListTop)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrackListBottom)).EndInit();
      this.tabPageSettingsKeys.ResumeLayout(false);
      this.groupBoxKeys.ResumeLayout(false);
      this.groupBoxKeys.PerformLayout();
      this.tabPageSettingsTagsGeneral.ResumeLayout(false);
      this.groupBoxTagsGeneral.ResumeLayout(false);
      this.tabPageSettingsTagsId3.ResumeLayout(false);
      this.groupBoxTagValidate.ResumeLayout(false);
      this.groupBoxTagsID3.ResumeLayout(false);
      this.groupBoxTagsID3.PerformLayout();
      this.groupBoxID3Update.ResumeLayout(false);
      this.tabPageSettingsLyricsCover.ResumeLayout(false);
      this.groupBoxPictures.ResumeLayout(false);
      this.groupBoxPictures.PerformLayout();
      this.groupBoxLyrics.ResumeLayout(false);
      this.groupBoxLyricsSites.ResumeLayout(false);
      this.tabPageSettingsDatabase.ResumeLayout(false);
      this.groupBoxDatabaseBuild.ResumeLayout(false);
      this.groupBoxDatabaseBuild.PerformLayout();
      this.groubBoxTagsDatabase.ResumeLayout(false);
      this.groubBoxTagsDatabase.PerformLayout();
      this.tabPageSettingsRipGeneral.ResumeLayout(false);
      this.groupBoxEncoding.ResumeLayout(false);
      this.groupBoxEncoding.PerformLayout();
      this.groupBoxRippingOptions.ResumeLayout(false);
      this.groupBoxCustomPath.ResumeLayout(false);
      this.groupBoxCustomPath.PerformLayout();
      this.groupBoxRippingFormatOptions.ResumeLayout(false);
      this.groupBoxRippingFormatOptions.PerformLayout();
      this.tabPageSettingsRipMp3.ResumeLayout(false);
      this.groupBoxPresets.ResumeLayout(false);
      this.groupBoxPresets.PerformLayout();
      this.groupBoxMp3Experts.ResumeLayout(false);
      this.groupBoxMp3Experts.PerformLayout();
      this.tabPageSettingsRipOgg.ResumeLayout(false);
      this.groupBoxOggEncoding.ResumeLayout(false);
      this.groupBoxOggEncoding.PerformLayout();
      this.groupBoxOggExpert.ResumeLayout(false);
      this.groupBoxOggExpert.PerformLayout();
      this.tabPageSettingsRipFlac.ResumeLayout(false);
      this.groupBoxFlacEncoding.ResumeLayout(false);
      this.groupBoxFlacEncoding.PerformLayout();
      this.groupBoxFlacSettings.ResumeLayout(false);
      this.groupBoxFlacSettings.PerformLayout();
      this.tabPageSettingsRipAAC.ResumeLayout(false);
      this.groupBoxAACEncoding.ResumeLayout(false);
      this.groupBoxAACEncoding.PerformLayout();
      this.tabPageSettingsRipWMA.ResumeLayout(false);
      this.groupBoxWMAEncoding.ResumeLayout(false);
      this.groupBoxWMAEncoding.PerformLayout();
      this.tabPageSettingsRipMPC.ResumeLayout(false);
      this.groupBoxMPCExpert.ResumeLayout(false);
      this.groupBoxMPCExpert.PerformLayout();
      this.groupBoxMPCPresets.ResumeLayout(false);
      this.groupBoxMPCPresets.PerformLayout();
      this.tabPageSettingsRipWV.ResumeLayout(false);
      this.groupBoxWVExpertSettings.ResumeLayout(false);
      this.groupBoxWVExpertSettings.PerformLayout();
      this.groupBoxWVPresets.ResumeLayout(false);
      this.groupBoxWVPresets.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.backstageViewPanel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.groupedNavigationBar1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.navigationBarGroupItemsContainer1)).EndInit();
      this.navigationBarGroupItemsContainer1.ResumeLayout(false);
      this.navigationBarGroupItemsContainer1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.navigationBarGroupItemsContainer2)).EndInit();
      this.navigationBarGroupItemsContainer2.ResumeLayout(false);
      this.navigationBarGroupItemsContainer2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.navigationBarGroupItemsContainer3)).EndInit();
      this.navigationBarGroupItemsContainer3.ResumeLayout(false);
      this.navigationBarGroupItemsContainer3.PerformLayout();
      this.backstageViewPageRecentFolders.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pinListRecentFolders)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageTag)).EndInit();
      this.ribbonTabPageTag.ResumeLayout(false);
      this.ribbonTabPageTag.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupTagsRetrieve)).EndInit();
      this.ribbonGroupTagsRetrieve.ResumeLayout(false);
      this.ribbonGroupTagsRetrieve.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupTagsEdit)).EndInit();
      this.ribbonGroupTagsEdit.ResumeLayout(false);
      this.ribbonGroupTagsEdit.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
      this.buttonGroup1.ResumeLayout(false);
      this.buttonGroup1.PerformLayout();
      this.buttonGroup3.ResumeLayout(false);
      this.buttonGroup3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupPicture)).EndInit();
      this.ribbonGroupPicture.ResumeLayout(false);
      this.ribbonGroupPicture.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.galleryPicture)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupOrganise)).EndInit();
      this.ribbonGroupOrganise.ResumeLayout(false);
      this.ribbonGroupOrganise.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupOther)).EndInit();
      this.ribbonGroupOther.ResumeLayout(false);
      this.ribbonGroupOther.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageRip)).EndInit();
      this.ribbonTabPageRip.ResumeLayout(false);
      this.ribbonTabPageRip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupRip)).EndInit();
      this.ribbonGroupRip.ResumeLayout(false);
      this.ribbonGroupRip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupRipOptions)).EndInit();
      this.ribbonGroupRipOptions.ResumeLayout(false);
      this.ribbonGroupRipOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageConvert)).EndInit();
      this.ribbonTabPageConvert.ResumeLayout(false);
      this.ribbonTabPageConvert.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupConvert)).EndInit();
      this.ribbonGroupConvert.ResumeLayout(false);
      this.ribbonGroupConvert.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupConvertOptions)).EndInit();
      this.ribbonGroupConvertOptions.ResumeLayout(false);
      this.ribbonGroupConvertOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonTabPageBurn)).EndInit();
      this.ribbonTabPageBurn.ResumeLayout(false);
      this.ribbonTabPageBurn.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupBurn)).EndInit();
      this.ribbonGroupBurn.ResumeLayout(false);
      this.ribbonGroupBurn.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ribbonGroupBurnOptions)).EndInit();
      this.ribbonGroupBurnOptions.ResumeLayout(false);
      this.ribbonGroupBurnOptions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTPanel panelLeft;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterLeft;
    private MPTagThat.Core.WinControls.MPTPanel panelRight;
    private System.Windows.Forms.ToolTip toolTip;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterRight;
    private MPTagThat.Core.WinControls.MPTPanel panelLeftTop;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterBottom;
    private MPTagThat.Core.WinControls.MPTPanel panelFileList;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private MPTagThat.Core.WinControls.MPTPanel panelMiddle;
    private MPTagThat.Core.WinControls.MPTPanel playerPanel;
    private MPTagThat.Core.WinControls.MPTPanel panelMiddleTop;
    private MPTagThat.Core.WinControls.MPTPanel panelMiddleBottom;
    private NJFLib.Controls.CollapsibleSplitter splitterTop;
    private MPTagThat.Core.WinControls.MPTPanel panelMiddleDBSearch;
    private System.Windows.Forms.Panel panelBottom;
    private NJFLib.Controls.CollapsibleSplitter splitterPlayer;
    private Elegant.Ui.FormFrameSkinner formFrameSkinner;
    private Elegant.Ui.StatusBar statusBar;
    private Elegant.Ui.StatusBarNotificationsArea statusBarNotificationsArea1;
    private Elegant.Ui.StatusBarPane statusBarPane2;
    private Elegant.Ui.StatusBarControlsArea statusBarControlsArea1;
    private Elegant.Ui.StatusBarPane statusBarPane3;
    private Elegant.Ui.Label toolStripStatusLabelFolder;
    private Elegant.Ui.Label toolStripStatusLabelFiles;
    private Elegant.Ui.Label toolStripStatusLabelFilter;
    private Elegant.Ui.StatusBarPane statusBarPane1;
    internal Elegant.Ui.ProgressBar progressBar1;
    private Elegant.Ui.Button buttonProgressCancel;
    private Elegant.Ui.StatusBarPane statusBarPane4;
    private Elegant.Ui.Label toolStripStatusLabelScanProgress;
    private Elegant.Ui.Ribbon ribbon;
    private Elegant.Ui.Button startMenuSave;
    private Elegant.Ui.Button startMenuRefresh;
    private Elegant.Ui.RibbonTabPage ribbonTabPageTag;
    private Elegant.Ui.RibbonGroup ribbonGroupTagsRetrieve;
    private Elegant.Ui.Button buttonTagFromFile;
    private Elegant.Ui.Button buttonTagIdentifyFiles;
    private Elegant.Ui.Button buttonTagFromInternet;
    private Elegant.Ui.RibbonGroup ribbonGroupTagsEdit;
    private Elegant.Ui.Button buttonGetCoverArt;
    private Elegant.Ui.Button buttonRemoveCoverArt;
    private Elegant.Ui.Button buttonGetLyrics;
    private Elegant.Ui.Button buttonRemoveComment;
    private Elegant.Ui.RibbonGroup ribbonGroupOrganise;
    private Elegant.Ui.SplitButton buttonRenameFiles;
    private Elegant.Ui.PopupMenu popupMenu1;
    private Elegant.Ui.Button buttonRenameFilesOptions;
    private Elegant.Ui.SplitButton buttonDeleteTag;
    private Elegant.Ui.PopupMenu popupMenu2;
    private Elegant.Ui.Button buttonDeleteAllTags;
    private Elegant.Ui.SplitButton buttonCaseConversion;
    private Elegant.Ui.PopupMenu popupMenu3;
    private Elegant.Ui.Button buttonCaseConversionOptions;
    private Elegant.Ui.ButtonGroup buttonGroup3;
    private Elegant.Ui.TextBox textBoxNumber;
    private Elegant.Ui.Button buttonOrganiseFiles;
    private Elegant.Ui.ComboBox comboBoxScripts;
    private Elegant.Ui.Button buttonScriptExecute;
    private Elegant.Ui.RibbonGroup ribbonGroupOther;
    private Elegant.Ui.Button buttonAddToBurner;
    private Elegant.Ui.Button buttonAddToConversion;
    private Elegant.Ui.Button buttonAddToPlaylist;
    private Elegant.Ui.RibbonTabPage ribbonTabPageConvert;
    private Elegant.Ui.RibbonTabPage ribbonTabPageRip;
    private Elegant.Ui.RibbonTabPage ribbonTabPageBurn;
    private Elegant.Ui.RibbonGroup ribbonGroupRipOptions;
    private Elegant.Ui.ComboBox comboBoxRipEncoder;
    private Elegant.Ui.TextBox textBoxRipOutputFolder;
    private Elegant.Ui.RibbonGroup ribbonGroupRip;
    private Elegant.Ui.Button buttonRipStart;
    private Elegant.Ui.Button buttonRipCancel;
    private Elegant.Ui.RibbonGroup ribbonGroupConvert;
    private Elegant.Ui.RibbonGroup ribbonGroupConvertOptions;
    private Elegant.Ui.TextBox textBoxConvertOutputFolder;
    private Elegant.Ui.Button buttonDeleteID3v1;
    private Elegant.Ui.Button buttonDeleteID3v2;
    private Elegant.Ui.Button buttonRipFolderSelect;
    private Elegant.Ui.ComboBox comboBoxConvertEncoder;
    private Elegant.Ui.Button buttonConvertStart;
    private Elegant.Ui.Button buttonConvertCancel;
    private Elegant.Ui.Button buttonConvertFolderSelect;
    private Elegant.Ui.RibbonGroup ribbonGroupBurn;
    private Elegant.Ui.RibbonGroup ribbonGroupBurnOptions;
    private Elegant.Ui.ComboBox comboBoxBurner;
    private Elegant.Ui.ComboBox comboBoxBurnerSpeed;
    private Elegant.Ui.Button buttonBurnStart;
    private Elegant.Ui.Button buttonBurnCancel;
    private Elegant.Ui.Button buttonAutoNumber;
    private Elegant.Ui.ToggleButton buttonNumberOnClick;
    private Elegant.Ui.RibbonGroup ribbonGroupPicture;
    private Elegant.Ui.Gallery galleryPicture;
    private Elegant.Ui.ButtonGroup buttonGroup1;
    private Elegant.Ui.Button buttonSaveAsThumb;
    private Elegant.Ui.Separator separator2;
    private Elegant.Ui.Separator separator3;
    private Elegant.Ui.BackstageView backstageView;
    private Elegant.Ui.BackstageViewPage backstageViewPageRecentFolders;
    private Elegant.Ui.BackstageViewButton backstageViewButtonSave;
    private Elegant.Ui.BackstageViewButton backstageViewButtonRefresh;
    private Elegant.Ui.BackstageViewSeparator backstageViewSeparator1;
    private Elegant.Ui.BackstageViewButton backstageViewButtonChangeColumns;
    private Elegant.Ui.BackstageViewSeparator backstageViewSeparator2;
    private Elegant.Ui.BackstageViewButton backstageViewButtonExit;
    private Elegant.Ui.Panel panel1;
    private Elegant.Ui.PinList pinListRecentFolders;
    private Elegant.Ui.Separator separatorRecentFolders;
    private Elegant.Ui.BackstageViewPage backstageViewPageOptions;
    private Elegant.Ui.Panel panel2;
    private Elegant.Ui.BackstageViewPanel backstageViewPanel1;
    private Elegant.Ui.GroupedNavigationBar groupedNavigationBar1;
    private Elegant.Ui.NavigationBarGroup navigationBarGroupGeneral;
    private Elegant.Ui.NavigationBarGroupItemsContainer navigationBarGroupItemsContainer1;
    private Elegant.Ui.NavigationBarItem navigationBarItemGeneral;
    private Elegant.Ui.NavigationBarItem navigationBarItemKeys;
    private Elegant.Ui.NavigationBarGroup navigationBarGroupTags;
    private Elegant.Ui.NavigationBarGroupItemsContainer navigationBarGroupItemsContainer2;
    private Elegant.Ui.NavigationBarItem navigationBarItemTagsGeneral;
    private Elegant.Ui.NavigationBarItem navigationBarItemTagsId3;
    private Elegant.Ui.NavigationBarItem navigationBarItemTagsLyricsCover;
    private Elegant.Ui.NavigationBarItem navigationBarItemTagsDatabase;
    private Elegant.Ui.NavigationBarGroup navigationBarGroupRipConvert;
    private Elegant.Ui.NavigationBarGroupItemsContainer navigationBarGroupItemsContainer3;
    private Elegant.Ui.NavigationBarItem navigationBarItemRipGeneral;
    private Elegant.Ui.NavigationBarItem navigationBarItemRipMp3;
    private Elegant.Ui.NavigationBarItem navigationBarItemRipOgg;
    private Elegant.Ui.NavigationBarItem navigationBarItemFlac;
    private Elegant.Ui.NavigationBarItem navigationBarItemRipAAC;
    private Elegant.Ui.NavigationBarItem navigationBarItemRipWMA;
    private Elegant.Ui.NavigationBarItem navigationBarItemMPC;
    private Elegant.Ui.NavigationBarItem navigationBarItemRipWV;
    private Elegant.Ui.Panel panel3;
    private Elegant.Ui.TabControl tabControlSettings;
    private Elegant.Ui.TabPage tabPageSettingsGeneral;
    private Core.WinControls.MPTGroupBox groupBoxGeneral;
    private Core.WinControls.MPTComboBox comboBoxDebugLevel;
    private Core.WinControls.MPTLabel lbDebugLevel;
    private Core.WinControls.MPTLabel lbTheme;
    private Core.WinControls.MPTComboBox comboBoxThemes;
    private Core.WinControls.MPTComboBox comboBoxLanguage;
    private Core.WinControls.MPTLabel lbLanguage;
    private Elegant.Ui.TabPage tabPageSettingsKeys;
    private Core.WinControls.MPTGroupBox groupBoxKeys;
    private TextBox tbRibbonKeyValue;
    private Core.WinControls.MPTLabel lblRibbonShortCut;
    private Core.WinControls.MPTLabel lblKeyShortCut;
    private Core.WinControls.MPTButton buttonChangeKey;
    private TextBox tbKeyValue;
    private Core.WinControls.MPTLabel ttLabel1;
    private Core.WinControls.MPTCheckBox ckShift;
    private Core.WinControls.MPTCheckBox ckCtrl;
    private Core.WinControls.MPTCheckBox ckAlt;
    private TextBox tbKeyDescription;
    private Core.WinControls.MPTLabel lbKeyDescription;
    private TextBox tbAction;
    private Core.WinControls.MPTLabel lbKeyAction;
    private TreeView treeViewKeys;
    private Elegant.Ui.TabPage tabPageSettingsTagsGeneral;
    private Core.WinControls.MPTButton buttonSettingsCancel;
    private Core.WinControls.MPTButton buttonSettingsApply;
    private Core.WinControls.MPTGroupBox groupBoxTagsGeneral;
    private Core.WinControls.MPTCheckBox ckAutoFillNumberOfTracks;
    private Core.WinControls.MPTCheckBox ckUseCaseConversionWhenSaving;
    private Core.WinControls.MPTCheckBox ckCopyArtistToAlbumArtist;
    private Elegant.Ui.TabPage tabPageSettingsTagsId3;
    private Core.WinControls.MPTGroupBox groupBoxTagValidate;
    private Core.WinControls.MPTCheckBox ckAutoFixMp3;
    private Core.WinControls.MPTCheckBox ckValidateMP3;
    private Core.WinControls.MPTGroupBox groupBoxTagsID3;
    private Core.WinControls.MPTRadioButton radioButtonUseApe;
    private Core.WinControls.MPTRadioButton radioButtonUseV4;
    private Core.WinControls.MPTRadioButton radioButtonUseV3;
    private Core.WinControls.MPTGroupBox groupBoxID3Update;
    private Core.WinControls.MPTCheckBox checkBoxRemoveID3V1;
    private Core.WinControls.MPTCheckBox checkBoxRemoveID3V2;
    private Core.WinControls.MPTRadioButton radioButtonID3Both;
    private Core.WinControls.MPTRadioButton radioButtonID3V2;
    private Core.WinControls.MPTRadioButton radioButtonID3V1;
    private Elegant.Ui.TabPage tabPageSettingsLyricsCover;
    private Core.WinControls.MPTGroupBox groupBoxPictures;
    private Core.WinControls.MPTComboBox comboBoxAmazonSite;
    private Core.WinControls.MPTLabel lbAmazonSearchSite;
    private Core.WinControls.MPTCheckBox ckUseExistinbgThumb;
    private Core.WinControls.MPTCheckBox ckOverwriteExistingCovers;
    private Core.WinControls.MPTCheckBox ckCreateMissingFolderThumb;
    private Core.WinControls.MPTGroupBox groupBoxLyrics;
    private Core.WinControls.MPTCheckBox ckOverwriteExistingLyrics;
    private Core.WinControls.MPTCheckBox ckSwitchArtist;
    private Core.WinControls.MPTGroupBox groupBoxLyricsSites;
    private Core.WinControls.MPTCheckBox ckLRCFinder;
    private Core.WinControls.MPTCheckBox ckLyrDB;
    private Core.WinControls.MPTCheckBox ckActionext;
    private Core.WinControls.MPTCheckBox ckLyricsPlugin;
    private Core.WinControls.MPTCheckBox ckLyricsOnDemand;
    private Core.WinControls.MPTCheckBox ckLyrics007;
    private Core.WinControls.MPTCheckBox ckHotLyrics;
    private Core.WinControls.MPTCheckBox ckLyricWiki;
    private Elegant.Ui.TabPage tabPageSettingsDatabase;
    private Core.WinControls.MPTGroupBox groupBoxDatabaseBuild;
    private Core.WinControls.MPTCheckBox checkBoxClearDatabase;
    private Core.WinControls.MPTLabel lbDBScanStatus;
    private Core.WinControls.MPTButton buttonDBScanStatus;
    private Core.WinControls.MPTButton buttonStartDatabaseScan;
    private Core.WinControls.MPTLabel lbDatabaseNote;
    private Core.WinControls.MPTGroupBox groubBoxTagsDatabase;
    private Core.WinControls.MPTButton buttonMusicDatabaseBrowse;
    private TextBox tbMediaPortalDatabase;
    private Core.WinControls.MPTCheckBox ckUseMediaPortalDatabase;
    private Elegant.Ui.TabPage tabPageSettingsRipGeneral;
    private Core.WinControls.MPTGroupBox groupBoxEncoding;
    private Core.WinControls.MPTComboBox comboBoxEncoder;
    private Core.WinControls.MPTLabel lbEncodingFormat;
    private Core.WinControls.MPTGroupBox groupBoxRippingOptions;
    private Core.WinControls.MPTCheckBox ckActivateTargetFolder;
    private Core.WinControls.MPTCheckBox ckRipEjectCD;
    private Core.WinControls.MPTGroupBox groupBoxCustomPath;
    private Core.WinControls.MPTGroupBox groupBoxRippingFormatOptions;
    private Core.WinControls.MPTLabel lblParmFolder;
    private Core.WinControls.MPTLabel lblAlbumArtist;
    private Core.WinControls.MPTLabel lblParmGenre;
    private Core.WinControls.MPTLabel lblParmTrack;
    private Core.WinControls.MPTLabel lblParmYear;
    private Core.WinControls.MPTLabel lblParmAlbum;
    private Core.WinControls.MPTLabel lblParmTitle;
    private Core.WinControls.MPTLabel lblParmArtist;
    private TextBox textBoxRippingFilenameFormat;
    private Core.WinControls.MPTLabel lbFormat;
    private Core.WinControls.MPTButton buttonTargetFolderBrowse;
    private TextBox tbTargetFolder;
    private Core.WinControls.MPTLabel lbTargetFolder;
    private Elegant.Ui.TabPage tabPageSettingsRipMp3;
    private Core.WinControls.MPTGroupBox groupBoxPresets;
    private TextBox textBoxPresetDesc;
    private TextBox textBoxABRBitrate;
    private Core.WinControls.MPTLabel lbABRBitrate;
    private Core.WinControls.MPTComboBox comboBoxLamePresets;
    private Core.WinControls.MPTLabel lbPreset;
    private Core.WinControls.MPTGroupBox groupBoxMp3Experts;
    private Core.WinControls.MPTLabel lbLameExpertsWarning;
    private TextBox textBoxLameParms;
    private Core.WinControls.MPTLabel lbLameExpertOptions;
    private Elegant.Ui.TabPage tabPageSettingsRipOgg;
    private Core.WinControls.MPTGroupBox groupBoxOggEncoding;
    private Core.WinControls.MPTLabel lbOggQualitySelected;
    private Core.WinControls.MPTLabel lbOggQuality;
    private HScrollBar hScrollBarOggEncodingQuality;
    private Core.WinControls.MPTGroupBox groupBoxOggExpert;
    private Core.WinControls.MPTLabel lbOggExpertWarning;
    private TextBox textBoxOggParms;
    private Core.WinControls.MPTLabel lbOggExpert;
    private Elegant.Ui.TabPage tabPageSettingsRipFlac;
    private Core.WinControls.MPTGroupBox groupBoxFlacEncoding;
    private Core.WinControls.MPTLabel lbFlacQualitySelected;
    private Core.WinControls.MPTLabel lbFlacQuality;
    private HScrollBar hScrollBarFlacEncodingQuality;
    private Core.WinControls.MPTGroupBox groupBoxFlacSettings;
    private Core.WinControls.MPTLabel lbFlacExpertsWarning;
    private TextBox textBoxFlacParms;
    private Core.WinControls.MPTLabel lbFlacExperts;
    private Elegant.Ui.TabPage tabPageSettingsRipAAC;
    private Core.WinControls.MPTGroupBox groupBoxAACEncoding;
    private Core.WinControls.MPTComboBox comboBoxAACBitrates;
    private Core.WinControls.MPTLabel labelAACBitrate;
    private Elegant.Ui.TabPage tabPageSettingsRipWMA;
    private Elegant.Ui.TabPage tabPageSettingsRipMPC;
    private Core.WinControls.MPTGroupBox groupBoxWMAEncoding;
    private Core.WinControls.MPTComboBox comboBoxWMABitRate;
    private Core.WinControls.MPTComboBox comboBoxWMACbrVbr;
    private Core.WinControls.MPTLabel labelWMAQuality;
    private Core.WinControls.MPTComboBox comboBoxWMASampleFormat;
    private Core.WinControls.MPTLabel labelWMASampleFormat;
    private Core.WinControls.MPTComboBox comboBoxWMAEncoderFormat;
    private Core.WinControls.MPTLabel labelWMAEncoderFormat;
    private Elegant.Ui.TabPage tabPageSettingsRipWV;
    private Core.WinControls.MPTGroupBox groupBoxMPCExpert;
    private Core.WinControls.MPTLabel lbMPCExpertsWarning;
    private TextBox textBoxMPCParms;
    private Core.WinControls.MPTLabel lbMPCExperts;
    private Core.WinControls.MPTGroupBox groupBoxMPCPresets;
    private Core.WinControls.MPTComboBox comboBoxMPCPresets;
    private Core.WinControls.MPTLabel lbMPCPresets;
    private Core.WinControls.MPTGroupBox groupBoxWVExpertSettings;
    private Core.WinControls.MPTLabel lbWVExpertWarning;
    private TextBox textBoxWVParms;
    private Core.WinControls.MPTLabel lbWVExpert;
    private Core.WinControls.MPTGroupBox groupBoxWVPresets;
    private Core.WinControls.MPTComboBox comboBoxWVPresets;
    private Core.WinControls.MPTLabel lbWVPreset;
    private Core.WinControls.MPTLabel lbTracklistLocation;
    private System.Windows.Forms.PictureBox pictureBoxTrackListBottom;
    private System.Windows.Forms.PictureBox pictureBoxTrackListTop;
    private Elegant.Ui.ComboBox comboBoxCharacterEncoding;
    private Core.WinControls.MPTLabel lbCharacterEncoding;
    private Core.WinControls.MPTCheckBox ckChangeReadonlyAttributte;
    private Core.WinControls.MPTCheckBox ckOnlySaveFolderThumb;
  }
}

