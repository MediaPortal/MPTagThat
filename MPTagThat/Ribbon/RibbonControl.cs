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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Elegant.Ui;
using MPTagThat.Core;
using MPTagThat.Core.Burning;
using MPTagThat.Dialogues;
using MPTagThat.Organise;
using MPTagThat.TagEdit;
using TagLib;
using Button = Elegant.Ui.Button;
using ComboBox = Elegant.Ui.ComboBox;
using Action = MPTagThat.Core.Action;

#endregion

namespace MPTagThat
{
  public partial class RibbonControl : UserControl
  {
    #region Variables

    private readonly IActionHandler actionhandler = ServiceScope.Get<IActionHandler>();
    private readonly List<Item> encodersRip = new List<Item>();
    private readonly List<Item> encodersConvert = new List<Item>();
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly Main main;
    private readonly RecentDocumentsControl recentFolders = new RecentDocumentsControl();
    private bool _initialising = true;
    private bool _numberingOnClick;
    private PictureControl picControl;
    private GalleryItem _displayedGalleryItem;

    #endregion

    #region Properties

    /// <summary>
    ///   Inidicates state of Applikation / Riobbon Initialising
    /// </summary>
    public bool Initialising
    {
      get { return _initialising; }
      set { _initialising = value; }
    }

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
    public ComboBox ScriptsCombo
    {
      get { return comboBoxScripts; }
    }

    /// <summary>
    ///   Returns the Conversion Encoder Combo Box
    /// </summary>
    public ComboBox EncoderCombo
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
    public ComboBox RipEncoderCombo
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
    public ComboBox BurnerCombo
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

    #region ctor

    public RibbonControl(Main main)
    {
      log.Trace(">>>");
      log.Info("Initialising Ribbon");
      this.main = main;

      InitializeComponent();

      // Register the Ribbon Button Events
      RegisterCommands();

      // Register Ribbon KeyTips
      RegisterKeyTips();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      LocaliseScreen();

      // Load Recent Folders
      recentFolders.Items.AddRange(Options.MainSettings.RecentFolders.ToArray());
      applicationMenu1.RightPaneControl = recentFolders;
      applicationMenu1.VisibleChanged += applicationMenu1_VisibleChanged;
      recentFolders.ItemClick += recentFolders_ItemClick;

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
      log.Info("Finished Initialising Ribbon");
      log.Trace("<<<");
    }

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
      startMenuSave.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_SAVE);
      startMenuRefresh.KeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_REFRESH);
      applicationMenu1.OptionsButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_OPTIONS);
      applicationMenu1.ExitButtonKeyTip = actionhandler.GetKeyCode(Action.ActionType.ACTION_EXIT);

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

    #endregion

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      log.Trace(">>>");
      log.Debug("Localise Ribbon");
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

    #region Private Methods

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

    #endregion

    #region Public Methods

    /// <summary>
    ///   Get the Coverart out of the selected TRack item and fill the Ribbon Gallery
    /// </summary>
    public void SetGalleryItem()
    {
      Image img = null;
      ClearGallery();
      try
      {
        TrackData track = main.TracksGridView.SelectedTrack;
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
      catch (Exception) {}
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
      try
      {
        recentFolders.Items.Remove(newFolder);
      }
      catch (ArgumentException) {}

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
            if (main.Burning)
            {
              return;
            }

            main.ToggleDetailPanel(false);
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

            main.ToggleDetailPanel(false);
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

            main.ToggleDetailPanel(false);
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

            main.ToggleDetailPanel(true);
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
    ///   Exit the Application
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Exit_Executed(object sender, CommandExecutedEventArgs e)
    {
      Application.Exit();
    }

    /// <summary>
    ///   The Save button was pressed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Save_Executed(object sender, CommandExecutedEventArgs e)
    {
      main.TracksGridView.Save();
    }

    /// <summary>
    ///   Show the Options Panel
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Options_Executed(object sender, CommandExecutedEventArgs e)
    {
      Preferences.Preferences dlgPreferences = new Preferences.Preferences(main);
      main.ShowModalDialog(dlgPreferences);
    }

    /// <summary>
    ///   Refresh the Tracksgrid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Refresh_Executed(object sender, CommandExecutedEventArgs e)
    {
      main.RefreshTrackList();
    }

    /// <summary>
    ///   Change Display Columns
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void ChangeDisplayColumns_Executed(object sender, CommandExecutedEventArgs e)
    {
      Form dlg = new ColumnSelect(main.TracksGridView);
      main.ShowModalDialog(dlg);
    }

    /// <summary>
    ///   Help Button has been pressed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void Help_Executed(object sender, CommandExecutedEventArgs e)
    {
      About dlgAbout = new About();
      main.ShowModalDialog(dlgAbout);
    }

    /// <summary>
    ///   Refresh the Recent Folder Menu, when we get a change
    ///   Seems to be a bug in version 3.7
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void applicationMenu1_VisibleChanged(object sender, EventArgs e)
    {
      applicationMenu1.RightPaneControl.Controls[0].PerformLayout();
    }

    /// <summary>
    ///   An Item has been selected in the Recent Folder List
    ///   Change to that folder.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void recentFolders_ItemClick(object sender, RecentDocumentsControlItemClickEventArgs e)
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
    ///   Handle various button click events
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void TagsTabButton_Executed(object sender, CommandExecutedEventArgs e)
    {
      // If no Rows are selected, select ALL of them and do the necessary action
      if (!main.TracksGridView.CheckSelections(true))
      {
        return;
      }

      switch (e.Command.Name)
      {
        case "FileNameToTag":
          FileNameToTag.FileNameToTag dlgFileNameToTag = new FileNameToTag.FileNameToTag(main);
          main.ShowDialogInDetailPanel(dlgFileNameToTag);
          break;

        case "IdentifyFiles":
          main.TracksGridView.IdentifyFiles();
          break;

        case "TagFromInternet":
          InternetLookup.InternetLookup lookup = new InternetLookup.InternetLookup(main);
          lookup.SearchForAlbumInformation();
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
          OrganiseFiles dlgOrganise = new OrganiseFiles(main);
          main.ShowDialogInDetailPanel(dlgOrganise);
          break;

        case "RenameFiles":
          TagToFileName.TagToFileName dlgTagToFile = new TagToFileName.TagToFileName(main, true);
          break;

        case "RenameFileOptions":
          TagToFileName.TagToFileName dlgTagToFileOptions = new TagToFileName.TagToFileName(main, false);
          main.ShowDialogInDetailPanel(dlgTagToFileOptions);
          break;

        case "CaseConversion":
          CaseConversion.CaseConversion dlgCaseConversion = new CaseConversion.CaseConversion(main, true);
          dlgCaseConversion.CaseConvertSelectedTracks();
          break;

        case "CaseConversionOptions":
          CaseConversion.CaseConversion dlgCaseConversionOptions = new CaseConversion.CaseConversion(main);
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

          Item tag = (Item)comboBoxScripts.SelectedItem;
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
      TrackData track = main.TracksGridView.SelectedTrack;
      main.TracksGridView.SavePicture(track);
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
      main.RipGridView.RipAudioCD();
    }

    private void RipCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      main.RipGridView.RipAudioCDCancel();
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

      main.BurnGridView.SetActiveBurner(burner);
      main.BurnGridView.BurnAudioCD();
    }

    /// <summary>
    ///   Cancel burning
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void BurnCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      main.BurnGridView.BurnAudioCDCancel();
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
        Item item = new Item("Maximum", Convert.ToInt32(speeds[0].Trim(new[] {'x'})));
        comboBoxBurnerSpeed.Items.Add(item);
        burner.SelectedWriteSpeed = (int)item.Value;
        foreach (string speed in speeds)
        {
          item = new Item(speed, Convert.ToInt32(speed.Trim(new[] {'x'})));
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
      main.ConvertGridView.ConvertFiles();
    }

    private void ConvertCancel_Executed(object sender, CommandExecutedEventArgs e)
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

    #region General Events

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
          LocaliseScreen();
          break;
      }
    }

    #endregion
  }
}