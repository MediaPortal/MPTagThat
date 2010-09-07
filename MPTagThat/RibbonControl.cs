using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Elegant.Ui;
using MPTagThat.Core;
using MPTagThat.Core.Burning;
using TagLib;
using File = System.IO.File;

namespace MPTagThat
{
  public partial class RibbonControl : UserControl
  {
    #region Variables
    private Main main;
    private List<Item> encoders = new List<Item>();
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private IActionHandler actionhandler = ServiceScope.Get<IActionHandler>();
    private bool _numberingOnClick;
    private bool _initialising = true;
    private PictureControl picControl;
    private RecentDocumentsControl recentFolders = new RecentDocumentsControl();
    #endregion

    #region Properties
    /// <summary>
    /// Inidicates state of Applikation / Riobbon Initialising
    /// </summary>
    public bool Initialising
    {
      get { return _initialising; }
      set { _initialising = value; }
    }

    /// <summary>
    /// Set the Theme of the Ribbon
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
        }

        SkinManager.LoadEmbeddedTheme(theme, Product.Ribbon);
        ServiceScope.Get<IThemeManager>().ChangeTheme(value);
      }
    }

    /// <summary>
    /// Returns the Tag Tab
    /// </summary>
    public RibbonTabPage TabTag
    {
      get { return ribbonTabPageTag; }
    }

    /// <summary>
    /// Returns the Burn Tab
    /// </summary>
    public RibbonTabPage TabBurn
    {
      get { return ribbonTabPageBurn; }
    }

    /// <summary>
    /// Returns the Rip Tab
    /// </summary>
    public RibbonTabPage TabRip
    {
      get { return ribbonTabPageRip; }
    }

    /// <summary>
    /// Returns the Convert Tab
    /// </summary>
    public RibbonTabPage TabConvert
    {
      get { return ribbonTabPageConvert; }
    }
    /// <summary>
    /// Returns the Scripts Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox ScriptsCombo
    {
      get { return comboBoxScripts; }
    }

    /// <summary>
    /// Returns the Conversion Encoder Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox EncoderCombo
    {
      get { return comboBoxConvertEncoder; }
    }

    /// <summary>
    /// Returns the selected output Directory
    /// </summary>
    public string EncoderOutputDirectory
    {
      get { return textBoxConvertOutputFolder.Text; }
    }

    /// <summary>
    /// Returns the Rip Encoder Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox RipEncoderCombo
    {
      get { return comboBoxRipEncoder; }
    }

    /// <summary>
    /// Returns the selected output Directory
    /// </summary>
    public string RipOutputDirectory
    {
      get { return textBoxRipOutputFolder.Text; }
    }

    /// <summary>
    /// Returns the Burner Combo Box
    /// </summary>
    public Elegant.Ui.ComboBox BurnerCombo
    {
      get { return comboBoxBurner; }
    }

    /// <summary>
    /// Get / Set Auto Numbering
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

      set
      {
        textBoxNumber.Text = value.ToString();
      }
    }

    /// <summary>
    /// Return Numbering On Click
    /// </summary>
    public bool NumberingOnClick
    {
      get { return _numberingOnClick; }
    }

    /// <summary>
    /// Returns the Picture Gallery
    /// </summary>
    public Gallery PictureGallery
    {
      get { return galleryPicture; }
    }
    #endregion

    #region ctor
    public RibbonControl(Main main)
    {
      this.main = main;

      InitializeComponent();

      // Register the Ribbon Button Events
      RegisterCommands();

      // Register Ribbon KeyTips
      RegisterKeyTips();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      LocaliseScreen();

      // Load Recent Folders
      recentFolders.Items.AddRange(Options.MainSettings.RecentFolders.ToArray());
      applicationMenu1.RightPaneControl = recentFolders;
      applicationMenu1.VisibleChanged += applicationMenu1_VisibleChanged;
      recentFolders.ItemClick += recentFolders_ItemClick;

      // Load the available Scripts
      PopulateScriptsCombo();

      encoders.Add(new Item("MP3 Encoder", "mp3", ""));
      encoders.Add(new Item("OGG Encoder", "ogg", ""));
      encoders.Add(new Item("FLAC Encoder", "flac", ""));
      encoders.Add(new Item("AAC Encoder", "m4a", ""));
      encoders.Add(new Item("WMA Encoder", "wma", ""));
      encoders.Add(new Item("WAV Encoder", "wav", ""));
      encoders.Add(new Item("MusePack Encoder", "mpc", ""));
      encoders.Add(new Item("WavPack Encoder", "wv", ""));
      comboBoxRipEncoder.DisplayMember = "Name";
      comboBoxRipEncoder.ValueMember = "Value";
      comboBoxRipEncoder.DataSource = encoders;

      comboBoxConvertEncoder.DisplayMember = "Name";
      comboBoxConvertEncoder.ValueMember = "Value";
      comboBoxConvertEncoder.DataSource = encoders;

      int i = 0;
      foreach (Item item in encoders)
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
    }

    /// <summary>
    /// Register Ribbon Commands to be executed when a button is pressed.
    /// </summary>
    private void RegisterCommands()
    {
      ApplicationCommands.AddToBurner.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.AddToConversion.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.AddToPlaylist.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.AutoNumber.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.BurnCancel.Executed += new Elegant.Ui.CommandExecutedEventHandler(BurnCancel_Executed);
      ApplicationCommands.BurnStart.Executed += new Elegant.Ui.CommandExecutedEventHandler(BurnStart_Executed);
      ApplicationCommands.CaseConversion.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.CaseConversionOptions.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.ChangeDisplayColumns.Executed += new Elegant.Ui.CommandExecutedEventHandler(ChangeDisplayColumns_Executed);
      ApplicationCommands.ConvertCancel.Executed += new Elegant.Ui.CommandExecutedEventHandler(ConvertCancel_Executed);
      ApplicationCommands.ConvertStart.Executed += new Elegant.Ui.CommandExecutedEventHandler(ConvertStart_Executed);
      ApplicationCommands.DeleteAllTags.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.DeleteID3v1.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.DeleteID3v2.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.Exit.Executed += new Elegant.Ui.CommandExecutedEventHandler(Exit_Executed);
      ApplicationCommands.FileNameToTag.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.FolderSelect.Executed += new Elegant.Ui.CommandExecutedEventHandler(FolderSelect_Executed);
      ApplicationCommands.GetCoverArt.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.GetLyrics.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.Help.Executed += new Elegant.Ui.CommandExecutedEventHandler(Help_Executed);
      ApplicationCommands.IdentifyFiles.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.MultiTagEdit.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.Options.Executed += new Elegant.Ui.CommandExecutedEventHandler(Options_Executed);
      ApplicationCommands.OrganiseFiles.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.Refresh.Executed += new Elegant.Ui.CommandExecutedEventHandler(Refresh_Executed);
      ApplicationCommands.RemoveComment.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.RemoveCoverArt.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.RenameFileOptions.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.RenameFiles.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.RipCancel.Executed += new Elegant.Ui.CommandExecutedEventHandler(RipCancel_Executed);
      ApplicationCommands.RipStart.Executed += new Elegant.Ui.CommandExecutedEventHandler(RipStart_Executed);
      ApplicationCommands.Save.Executed += new Elegant.Ui.CommandExecutedEventHandler(Save_Executed);
      ApplicationCommands.ScriptExecute.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.SingleTagEdit.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.TagFromInternet.Executed += new Elegant.Ui.CommandExecutedEventHandler(TagsTabButton_Executed);
      ApplicationCommands.SaveAsThumb.Executed += new CommandExecutedEventHandler(SaveAsThumb_Executed);

      ApplicationCommands.SaveAsThumb.Enabled = false; // Disable button initally
    }

    /// <summary>
    /// Register the Keytips to be displayed in the menu, when pressing Alt or F10
    /// </summary>
    private void RegisterKeyTips()
    {
      // Start Menu
      startMenuSave.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_SAVE);
      startMenuRefresh.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_REFRESH);
      applicationMenu1.OptionsButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_OPTIONS);
      applicationMenu1.ExitButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_EXIT);

      // Tags Tab
      buttonTagFromFile.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_FILENAME2TAG);
      buttonTagIdentifyFiles.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_IDENTIFYFILE);
      buttonTagFromInternet.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_TAGFROMINTERNET);

      buttonSingleTagEdit.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_EDIT);
      buttonMultiTagEdit.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_MULTI_EDIT);
      buttonGetCoverArt.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_GETCOVERART);
      buttonGetLyrics.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_GETLYRICS);
      buttonRemoveComment.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_REMOVECOMMENT);
      buttonCaseConversion.ButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_CASECONVERSION);
      buttonScriptExecute.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_SCRIPTEXECUTE);

      buttonRenameFiles.ButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_TAG2FILENAME);
      buttonOrganiseFiles.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_ORGANISE);
    }
    #endregion

    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Util.EnterMethod(Util.GetCallingMethod());

      // Start Menu
      startMenuSave.Text = localisation.ToString("ribbon", "Save");
      startMenuSave.ScreenTip.Caption = localisation.ToString("screentip", "Save");
      startMenuSave.ScreenTip.Text = localisation.ToString("screentip", "SaveText");

      startMenuRefresh.Text = localisation.ToString("ribbon", "Refresh");
      startMenuRefresh.ScreenTip.Caption = localisation.ToString("screentip", "Refresh");
      startMenuRefresh.ScreenTip.Text = localisation.ToString("screentip", "RefreshText");

      startMenuChangeDisplayColumns.Text = localisation.ToString("ribbon", "ColumnsSelect");
      startMenuChangeDisplayColumns.ScreenTip.Caption = localisation.ToString("screentip", "ColumnsSelect");
      startMenuChangeDisplayColumns.ScreenTip.Text = localisation.ToString("screentip", "ColumnsSelectText");

      applicationMenu1.OptionsButtonText = localisation.ToString("ribbon", "Settings");
      applicationMenu1.ExitButtonText = localisation.ToString("ribbon", "Exit");

      recentFolders.Caption = localisation.ToString("ribbon", "RecentFolders");

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

      buttonSingleTagEdit.Text = localisation.ToString("ribbon", "SingleTagEdit");
      buttonSingleTagEdit.ScreenTip.Caption = localisation.ToString("screentip", "SingleTagEdit");
      buttonSingleTagEdit.ScreenTip.Text = localisation.ToString("screentip", "SingleTagEditText");

      buttonMultiTagEdit.Text = localisation.ToString("ribbon", "MultiTagEdit");
      buttonMultiTagEdit.ScreenTip.Caption = localisation.ToString("screentip", "MultiTagEdit");
      buttonMultiTagEdit.ScreenTip.Text = localisation.ToString("screentip", "MultiTagEditText");

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
      
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Fill the Script Combo  Box with all scripts found
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
    #endregion

    #region Public Methods
    /// <summary>
    /// Get the Coverart out of the selected TRack item and fill the Ribbon Gallery
    /// </summary>
    public void SetGalleryItem()
    {
      Image img = null;
      ClearGallery();
      try
      {
        TrackData track = main.TracksGridView.SelectedTrack;
        IPicture[] pics = new IPicture[] { };
        pics = track.File.Tag.Pictures;
        ApplicationCommands.SaveAsThumb.Enabled = false;
        if (pics.Length > 0)
        {
          using (MemoryStream ms = new MemoryStream(pics[0].Data.Data))
          {
            img = Image.FromStream(ms);
            if (img != null)
            {

              GalleryItem galleryItem = new GalleryItem();
              galleryItem.Image = img;
              this.galleryPicture.Items.Add(galleryItem);
              ApplicationCommands.SaveAsThumb.Enabled = true;              
            }
          }
        }
      }
      catch (Exception)
      {
      }
    }

    /// <summary>
    /// Clear the Ribbon Gallery
    /// </summary>
    public void ClearGallery()
    {
      this.galleryPicture.Items.Clear();
      this.galleryPicture.Invalidate();
    }

    /// <summary>
    /// Set the Recent Folder, when a new folder has been selected
    /// </summary>
    /// <param name="newFolder"></param>
    public void SetRecentFolder(string newFolder)
    {
      try
      {
        recentFolders.Items.Remove(newFolder);
      }
      catch (System.ArgumentException)
      {
      }

      // Due to an error in the ribbon, we can't insert at position 0
      // So we loop through the list and re-add the items again
      Options.MainSettings.RecentFolders.Clear();

      int i = 0;
      foreach (string item in recentFolders.Items)
      {
        Options.MainSettings.RecentFolders.Add(item);
        i++;
        if (i > 8) // Keep a maximum of 10 entries in the list
        {
          break;
        }
      }
      Options.MainSettings.RecentFolders.Insert(0, newFolder);
      recentFolders.Items.Clear();
      recentFolders.Items.AddRange(Options.MainSettings.RecentFolders.ToArray());
    }
    #endregion

    #region Ribbon Events

    #region General Events
    /// <summary>
    /// Display a Folder Select Dialog and update the Textbox, based on the button being pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void FolderSelect_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
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
    /// Tab Page has changed. Show the correct grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbon_CurrentTabPageChanged(object sender, EventArgs e)
    {
      //Don't do anything, if we are just initialising
      if (_initialising)
      {
        return;
      }

      string tabPage = (sender as Elegant.Ui.Ribbon).CurrentTabPage.Tag as string;

      switch (tabPage)
      {
        case "Rip":
          {
            // Don't allow navigation, while Burning
            if (main.Burning)
            {
              return;
            }

            main.TracksGridView.Hide();
            main.BurnGridView.Hide();
            main.RipGridView.Show();
            if (!main.SplitterRight.IsCollapsed)
            {
              main.SplitterRight.ToggleState();
            }
            break;
          }

        case "Burn":
          {
            // Don't allow navigation, while Ripping
            if (main.Ripping)
            {
              return;
            }

            main.TracksGridView.Hide();
            main.RipGridView.Hide();
            main.BurnGridView.SetMediaInfo();
            main.BurnGridView.Show();
            if (!main.SplitterRight.IsCollapsed)
            {
              main.SplitterRight.ToggleState();
            }

            break;
          }

        case "Convert":
          {
            // Don't allow navigation, while Ripping or Burning
            if (main.Burning || main.Ripping)
            {
              return;
            }

            textBoxConvertOutputFolder.Text = Options.MainSettings.RipTargetFolder;

            main.TracksGridView.Hide();
            main.RipGridView.Hide();
            main.BurnGridView.Hide();
            main.ConvertGridView.Show();
            if (!main.SplitterRight.IsCollapsed)
            {
              main.SplitterRight.ToggleState();
            }

            break;
          }

        default:
          {
            // Don't allow navigation, while Ripping or Burning
            if (main.Burning || main.Ripping)
            {
              return;
            }

            main.BurnGridView.Hide();
            main.RipGridView.Hide();
            main.TracksGridView.Show();
            if (main.SplitterRight.IsCollapsed && !main.RightSplitterStatus)
            {
              main.SplitterRight.ToggleState();
            }

            break;
          }
      }
    }

    #endregion

    #region Application Menu & Quick Access Bar Clicks
    /// <summary>
    /// Exit the Application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Exit_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      Application.Exit();
    }
    
    /// <summary>
    /// The Save button was pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Save_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      main.TracksGridView.Save();
    }

    /// <summary>
    /// Show the Options Panel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Options_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      MPTagThat.Preferences.Preferences dlgPreferences = new MPTagThat.Preferences.Preferences(main);
      main.ShowModalDialog(dlgPreferences);
    }

    /// <summary>
    /// Refresh the Tracksgrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Refresh_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      main.RefreshTrackList();
    }

    /// <summary>
    /// Change Display Columns
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ChangeDisplayColumns_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      Form dlg = new MPTagThat.Dialogues.ColumnSelect(main.TracksGridView);
      main.ShowModalDialog(dlg);
    }

    /// <summary>
    /// Help Button has been pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Help_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      MPTagThat.Dialogues.About dlgAbout = new MPTagThat.Dialogues.About();
      main.ShowModalDialog(dlgAbout);
    }

    /// <summary>
    /// Refresh the Recent Folder Menu, when we get a change
    /// Seems to be a bug in version 3.7
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void applicationMenu1_VisibleChanged(object sender, EventArgs e)
    {
      applicationMenu1.RightPaneControl.Control.Controls[0].PerformLayout();
    }

    /// <summary>
    /// An Item has been selected in the Recent Folder List
    /// Change to that folder.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void recentFolders_ItemClick(object sender, RecentDocumentsControlItemClickEventArgs e)
    {
      string folder = (string)e.Item;
      if (!Directory.Exists(folder))
      {
        recentFolders.Items.Remove(folder);
        return;
      }
      main.CurrentDirectory = folder;
      main.TreeView.TreeView.ShowFolder(folder);
      SetRecentFolder(folder);
      main.RefreshTrackList();
    }
    #endregion

    #region Tags Tab
    /// <summary>
    /// Handle various button click events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void TagsTabButton_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      // If no Rows are selected, select ALL of them and do the necessary action
      if (!main.TracksGridView.CheckSelections(true))
      {
        return;
      }

      switch (e.Command.Name)
      {
        case "FileNameToTag":
          MPTagThat.FileNameToTag.FileNameToTag dlgFileNameToTag = new MPTagThat.FileNameToTag.FileNameToTag(main);
          main.ShowModalDialog(dlgFileNameToTag);
          break;

        case "IdentifyFiles":
          main.TracksGridView.IdentifyFiles();
          break;

          case "TagFromInternet":
          MPTagThat.InternetLookup.InternetLookup lookup = new MPTagThat.InternetLookup.InternetLookup(main);
          lookup.SearchForAlbumInformation();
          break;

        case "SingleTagEdit":
          MPTagThat.TagEdit.SingleTagEdit dlgSingleTagEdit = new MPTagThat.TagEdit.SingleTagEdit(main);
          main.ShowModalDialog(dlgSingleTagEdit);
          break;

          case "MultiTagEdit":
          MPTagThat.TagEdit.MultiTagEdit dlgMultiTagEdit = new MPTagThat.TagEdit.MultiTagEdit(main);
          main.ShowModalDialog(dlgMultiTagEdit);
          break;

        case "GetCoverArt":
          main.TracksGridView.GetCoverArt();
          break;

        case "GetLyrics":
          main.TracksGridView.GetLyrics();
          break;

        case "AutoNumber":
          main.TracksGridView.AutoNumber();
          break;

        case "RemoveComment":
          main.TracksGridView.RemoveComments();
          break;

        case "RemoveCoverArt":
          main.TracksGridView.RemovePictures();
          break;

        case "OrganiseFiles":
          MPTagThat.Organise.OrganiseFiles dlgOrganise = new MPTagThat.Organise.OrganiseFiles(main);
          main.ShowModalDialog(dlgOrganise);
          break;

        case "RenameFiles":
          MPTagThat.TagToFileName.TagToFileName dlgTagToFile = new MPTagThat.TagToFileName.TagToFileName(main, true);
          break;

        case "RenameFileOptions":
          MPTagThat.TagToFileName.TagToFileName dlgTagToFileOptions = new MPTagThat.TagToFileName.TagToFileName(main, false);
          main.ShowModalDialog(dlgTagToFileOptions);
          break;

        case "CaseConversion":
          MPTagThat.CaseConversion.CaseConversion dlgCaseConversion = new MPTagThat.CaseConversion.CaseConversion(main, true);
          dlgCaseConversion.CaseConvertSelectedTracks();
          break;

        case "CaseConversionOptions":
          MPTagThat.CaseConversion.CaseConversion dlgCaseConversionOptions = new MPTagThat.CaseConversion.CaseConversion(main);
          main.ShowModalDialog(dlgCaseConversionOptions);
          break;

        case "DeleteAllTags":
          main.TracksGridView.DeleteTags(TagTypes.AllTags);
          break;

        case "DeleteID3v1":
          main.TracksGridView.DeleteTags(TagTypes.Id3v1);
          break;

        case "DeleteID3v2":
          main.TracksGridView.DeleteTags(TagTypes.Id3v2);
          break;

        case "ScriptExecute":
          if (comboBoxScripts.SelectedIndex < 0)
            return;

          Item tag = (Item) comboBoxScripts.SelectedItem;
          main.TracksGridView.ExecuteScript((string)tag.Value);
          break;

        case "AddToBurner":
          main.TracksGridView.tracksGrid_AddToBurner(sender, new EventArgs());
          break;

        case "AddToConversion":
          main.TracksGridView.tracksGrid_AddToConvert(sender, new EventArgs());
          break;

        case "AddToPlaylist":
          main.TracksGridView.tracksGrid_AddToPlayList(sender, new EventArgs());
          break;
      }
    }

    /// <summary>
    /// The NumberOnClick ToggleButton has been pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Save the selected Picture as Folder THumb
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void SaveAsThumb_Executed(object sender, CommandExecutedEventArgs e)
    {
      TrackData track = main.TracksGridView.SelectedTrack;
      main.TracksGridView.SavePicture(track);
    }
    #endregion

    #region Rip Tab
    /// <summary>
    /// Start Ripping
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void RipStart_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      main.RipGridView.RipAudioCD();
    }

    void RipCancel_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      main.RipGridView.RipAudioCDCancel();
    }
    #endregion

    #region Burn Tab
    /// <summary>
    /// Burn the selected Tracks to CD
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void BurnStart_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      Burner burner = null;
      if (comboBoxBurner.SelectedIndex < 0)
        burner = (Burner) (comboBoxBurner.Items[0] as Item).Value;
      else
        burner = (Burner) (comboBoxBurner.SelectedItem as Item).Value;

      main.BurnGridView.SetActiveBurner(burner);
      main.BurnGridView.BurnAudioCD();
    }

    /// <summary>
    ///  Cancel burning
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void BurnCancel_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      main.BurnGridView.BurnAudioCDCancel();
    }


    /// <summary>
    /// A Burner has been selected. Set it as active Burner
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        Item item = new Item("Maximum", Convert.ToInt32(speeds[0].Trim(new char[] { 'x' })));
        comboBoxBurnerSpeed.Items.Add(item);
        burner.SelectedWriteSpeed = (int)item.Value;
        foreach (string speed in speeds)
        {
          item = new Item(speed, Convert.ToInt32(speed.Trim(new char[] { 'x' })));
          comboBoxBurnerSpeed.Items.Add(item);
        }
      }
      if (comboBoxBurnerSpeed.Items.Count > 0)
      {
        comboBoxBurnerSpeed.SelectedIndex = 0;
      }

      main.BurnGridView.SetActiveBurner(burner);

    }

    /// <summary>
    /// The Speed has been selected. Set it for the active Burner
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    void ConvertStart_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      main.ConvertGridView.ConvertFiles();
    }

    void ConvertCancel_Executed(object sender, Elegant.Ui.CommandExecutedEventArgs e)
    {
      main.ConvertGridView.ConvertFilesCancel();
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
        if (picControl != null && picControl.Text != "")
        {
          picControl.Close();
        }
        picControl = new PictureControl((GalleryItem)e.NewValue, (sender as Gallery).PopupOwnerBounds.Location);
      }
      else
      {
        if (picControl != null)
        {
          picControl.Close();
        }
      }
    }
    #endregion
    #endregion

    #region General Events
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
          LocaliseScreen();
          break;
      }
    }
    #endregion

  }
}
