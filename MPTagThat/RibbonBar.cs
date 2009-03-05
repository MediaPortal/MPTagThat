using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using TagLib;
using MPTagThat.Core;
using MPTagThat.Core.Burning;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace MPTagThat
{
  public partial class RibbonBar : UserControl
  {
    #region Variables
    private Main main;
    private List<Item> encoders = new List<Item>();
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private bool _numberingOnClick;
    #endregion

    #region Properties
    /// <summary>
    /// Returns the Ribbbon
    /// </summary>
    public RadRibbonBar MainRibbon
    {
      get { return radRibbonBar; }
    }

    /// <summary>
    /// Returns the Tag Tab
    /// </summary>
    public TabItem TabTag
    {
      get { return ribbonTabTag; }
    }

    /// <summary>
    /// Returns the Burn Tab
    /// </summary>
    public TabItem TabBurn
    {
      get { return ribbonTabBurn; }
    }

    /// <summary>
    /// Returns the Rip Tab
    /// </summary>
    public TabItem TabRip
    {
      get { return ribbonTabRip; }
    }

    /// <summary>
    /// Returns the Convert Tab
    /// </summary>
    public TabItem TabConvert
    {
      get { return ribbonTabConvert; }
    }

    /// <summary>
    /// Returns the Burner Combo Box
    /// </summary>
    public RadComboBoxElement BurnerCombo
    {
      get { return ribbonComboBoxBurner; }
    }

    /// <summary>
    /// Returns the Scripts Combo Box
    /// </summary>
    public RadComboBoxElement ScriptsCombo
    {
      get { return ribbonComboBoxScripts; }
    }

    /// <summary>
    /// Returns the Conversion Encoder Combo Box
    /// </summary>
    public RadComboBoxElement EncoderCombo
    {
      get { return ribbonComboBoxEncoder; }
    }

    /// <summary>
    /// Returns the selected output Directory
    /// </summary>
    public string EncoderOutputDirectory
    {
      get { return ribbonTextBoxConvertOutputDirectory.Text; }
    }

    /// <summary>
    /// Returns the Rip Encoder Combo Box
    /// </summary>
    public RadComboBoxElement RipEncoderCombo
    {
      get { return ribbonComboBoxRipEncoder; }
    }


    /// <summary>
    /// Returns the selected output Directory
    /// </summary>
    public string RipOutputDirectory
    {
      get { return ribbonTextBoxRipOutputDirectory.Text; }
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
          return Convert.ToInt32(ribbonTextBoxNumber.Text);
        }
        catch (Exception)
        {
          return -1;
        }
      }

      set 
      {
        ribbonTextBoxNumber.Text = value.ToString(); 
      }
    }

    /// <summary>
    /// Return Numbering On Click
    /// </summary>
    public bool NumberingOnClick
    {
      get { return _numberingOnClick; }
    }
    #endregion

    #region ctor
    public RibbonBar(Main main)
    {
      this.main = main;

      InitializeComponent();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      LocaliseScreen();

      // Add the Help Button to the right in the TabStrip
      RadButtonElement helpButton = new RadButtonElement();
      helpButton.MaxSize = new Size(18, 18);
      helpButton.Margin = new Padding(40, 0, 0, 0);
      helpButton.Image = MPTagThat.Properties.Resources.ribbon_help;
      helpButton.Alignment = ContentAlignment.MiddleRight;
      helpButton.Click += new EventHandler(helpButton_Click);
      this.radRibbonBar.RootElement.Children[0].Children[1].Children[0].Children.Insert(3, helpButton);

      // Add the option Button to the Start Menu
      RadButtonElement optionsButton = new RadButtonElement();
      optionsButton.Text = localisation.ToString("ribbon", "Settings");
      optionsButton.Alignment = ContentAlignment.TopRight;
      optionsButton.Padding = new Padding(3);
      optionsButton.Margin = new Padding(3);
      optionsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
      optionsButton.Image = global::MPTagThat.Properties.Resources.QuickAccessMenuOptions;
      optionsButton.Click += new EventHandler(quickAccessMenuOptions_Click);
      this.radRibbonBar.StartMenuBottomStrip.Children.Add(optionsButton);

      // Capture the drop dopwn of the Quick Access Toolbar
      RadToolStripOverFlowButtonElement quickAccessButton = ((RadToolStripOverFlowButtonElement)this.radRibbonBar.RootElement.Children[0].Children[0].Children[0].Children[1].Children[1]);
      quickAccessButton.DropDownOpened += new EventHandler(quickAccessButton_DropDownOpened);

      // Set the DefaultItem for the Split Buttons to indicate which action should happen, when pressing the button only
      // Note: If this isn't set, then the button opens it's menu. with the property set, only the arrow opens the menu
      ribbonButtonCaseConversion.DefaultItem = ribbonButtonCaseConversionExecute;
      ribbonButtonRenameFile.DefaultItem = ribbonButtonRenameFileExecute;

      // Hide the Maximize Button, as this causes problems on some systems.
      // It's a failure of the RibbonBar
      ClassSelector buttonSelector = new ClassSelector();
      buttonSelector.ElementClass = "MaximizeButton";
      buttonSelector.GetSelectedElements(this.radRibbonBar.RibbonBarElement)[0].Visibility = ElementVisibility.Collapsed;

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
      ribbonComboBoxEncoder.DisplayMember = "Name";
      ribbonComboBoxEncoder.ValueMember = "Value";
      ribbonComboBoxEncoder.DataSource = encoders;

      ribbonComboBoxRipEncoder.DisplayMember = "Name";
      ribbonComboBoxRipEncoder.ValueMember = "Value";
      ribbonComboBoxRipEncoder.DataSource = encoders;


      int i = 0;
      foreach (Item item in encoders)
      {
        if (item.Value == Options.MainSettings.LastConversionEncoderUsed)
        {
          ribbonComboBoxEncoder.SelectedIndex = i;
        }

        if (item.Value == Options.MainSettings.RipEncoder)
        {
          ribbonComboBoxRipEncoder.SelectedIndex = i;
        }
        i++;
      }

      ribbonTextBoxRipOutputDirectory.Text = Options.MainSettings.RipTargetFolder;

      radRibbonBar.RibbonBarElement.TabStripElement.SelectedTab = TabTag;
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
      this.radRibbonBar.Text = localisation.ToString("system", "ApplicationName");

      // Start Menu
      this.startMenuSave.Text = localisation.ToString("ribbon", "Save");
      this.startMenuExit.Text = localisation.ToString("ribbon", "Exit");
      this.startMenuItemChangeDisplayColumns.Text = localisation.ToString("ribbon", "ColumnsSelect");

      // Quick Access Menu
      this.quickAccessMenuSave.Text = localisation.ToString("ribbon", "Save");
      this.quickAccessMenuOptions.Text = localisation.ToString("ribbon", "Settings");
      this.quickAccesMenuRefresh.Text = localisation.ToString("ribbon", "Refresh");


      // Tags Tab
      this.ribbonTabTag.Text = localisation.ToString("ribbon", "TagTab");
      this.ribbonButtonTagFromFile.Text = localisation.ToString("ribbon", "TagFromFile");
      this.ribbonButtonTagIdentify.Text = localisation.ToString("ribbon", "IdentifyFile");
      this.ribbonButtonTagFromInternet.Text = localisation.ToString("ribbon", "TagFromInternet");
      this.radRibbonBarChunkTagsRetrieve.Text = localisation.ToString("ribbon", "RetrieveTags");

      this.ribbonButtonSingleEdit.Text = localisation.ToString("ribbon", "SingleTagEdit");
      this.ribbonButtonMultiEdit.Text = localisation.ToString("ribbon", "MultiTagEdit");
      this.ribbonButtonGetCoverArt.ToolTipText = localisation.ToString("ribbon", "GetCoverArt");
      this.ribbonButtonGetLyrics.ToolTipText = localisation.ToString("ribbon", "GetLyrics");
      this.ribbonButtonAutoNumber.ToolTipText = localisation.ToString("ribbon", "AutoNumber");
      this.ribbonButtonClickNumber.ToolTipText = localisation.ToString("ribbon", "NumberOnClick");
      this.radLabelRemove.Text = localisation.ToString("ribbon", "RemoveCommentPicture");
      this.ribbonButtonRemoveComments.ToolTipText = localisation.ToString("ribbon", "RemoveComments");
      this.ribbonButtonRemovePictures.ToolTipText = localisation.ToString("ribbon", "RemovePictures");
      this.ribbonButtonCaseConversion.Text = localisation.ToString("ribbon", "CaseConversion");
      this.ribbonButtonCaseConversionExecute.Text = localisation.ToString("ribbon", "CaseConversionMenu");
      this.ribbonButtonCaseConversionOptions.Text = localisation.ToString("ribbon", "CaseConversionOption");
      this.ribbonButtonDeleteTags.Text = localisation.ToString("ribbon", "DeleteTags");
      this.ribbonButtonDeleteAllTags.Text = localisation.ToString("ribbon", "DeleteAllTags");
      this.ribbonButtondeleteID3V1.Text = localisation.ToString("ribbon", "DeleteID3V1Tags");
      this.ribbonButtondeleteID3V2.Text = localisation.ToString("ribbon", "DeleteID3V2Tags");
      this.radRibbonBarChunkTagsEdit.Text = localisation.ToString("ribbon", "EditTags");

      this.ribbonButtonRenameFile.Text = localisation.ToString("ribbon", "RenameFile");
      this.ribbonButtonRenameFileExecute.Text = localisation.ToString("ribbon", "RenameFileMenu");
      this.ribbonButtonRenameFileOptions.Text = localisation.ToString("ribbon", "RenameFileOptions");
      this.ribbonButtonOrganise.Text = localisation.ToString("ribbon", "Organise");
      this.radRibbonBarChunkOrganise.Text = localisation.ToString("ribbon", "OrganiseFiles");

      this.ribbonButtonScriptExecute.Text = localisation.ToString("ribbon", "ExecuteScript");
      this.radRibbonBarChunkScripts.Text = localisation.ToString("ribbon", "Scripts");

      this.ribbonButtonAddToBurnList.Text = localisation.ToString("ribbon", "AddBurner");
      this.ribbonButtonAddToConvertList.Text = localisation.ToString("ribbon", "AddConvert");
      this.ribbonButtonAddToPlayList.Text = localisation.ToString("ribbon", "AddPlaylist");
      this.radRibbonBarChunkOther.Text = localisation.ToString("ribbon", "Other");

      // Rip Tab
      this.ribbonTabRip.Text = localisation.ToString("ribbon", "RipTab");
      this.ribbonButtonRip.Text = localisation.ToString("ribbon", "RipButton");
      this.ribbonLabelRipEncoder.Text = localisation.ToString("ribbon", "RipEncoder");
      this.ribbonLabelRipOutputDirectory.Text = localisation.ToString("ribbon", "RipFolder");
      this.ribbonButtonRipCancel.Text = localisation.ToString("ribbon", "RipCancel");
      this.radRibbonBarChunkRip.Text = localisation.ToString("ribbon", "RipOptions");

      // Convert Tab
      this.ribbonTabConvert.Text = localisation.ToString("ribbon", "ConvertTab");
      this.ribbonButtonConvert.Text = localisation.ToString("ribbon", "ConvertButton");
      this.ribbonLabelConvertEncoder.Text = localisation.ToString("ribbon", "ConvertEncoder");
      this.ribbonLabelConvertOutputDirectory.Text = localisation.ToString("ribbon", "ConvertFolder");
      this.ribbonButtonConvertCancel.Text = localisation.ToString("ribbon", "ConvertCancel");
      this.radRibbonBarChunkConvert.Text = localisation.ToString("ribbon", "ConvertOptions");

      // Burn Tab
      this.ribbonTabBurn.Text = localisation.ToString("ribbon", "BurnTab");
      this.ribbonButtonBurn.Text = localisation.ToString("ribbon", "Burn");
      this.ribbonButtonBurnCancel.Text = localisation.ToString("ribbon", "BurnCancel");
      this.radRibbonBarChunkBurn.Text = localisation.ToString("ribbon", "BurnOptions");

      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Fill the Script Combo  Box with all scripts found
    /// </summary>
    private void PopulateScriptsCombo()
    {
      ribbonComboBoxScripts.Items.Clear();
      ArrayList scripts = null;

      scripts = ServiceScope.Get<IScriptManager>().GetScripts();
      int i = 0;
      foreach (string[] item in scripts)
      {
        RadComboBoxItem rb = new RadComboBoxItem(item[1], new Item(item[1], item[0], item[2]));
        rb.ToolTipText = item[2];
        ribbonComboBoxScripts.Items.Add(rb);
        if (item[1] == Options.MainSettings.ActiveScript)
        {
          ribbonComboBoxScripts.SelectedIndex = i;
          ribbonComboBoxScripts.ToolTipText = item[2];
          i++;
        }
      }
    }

    /// <summary>
    /// Rename Menu entries Localisation
    /// </summary>
    private void RenameQuickAccessDropDownItems()
    {
      RadDropDownMenu quickAccessMenu =
          ((RadToolStripOverFlowButtonElement)this.radRibbonBar.RootElement.Children[0].Children[0].Children[0].Children[1].Children[1]).DropDownMenu;

      for (int i = 0; i < quickAccessMenu.Items.Count; i++)
      {
        if (!(quickAccessMenu.Items[i] is RadMenuSeparatorItem))
        {

        }
      }
    }


    #endregion

    #region Ribbon Events
    #region Tab Clicks
    private void ribbonTabItem_click(object sender, EventArgs e)
    {
      TabItem ti = sender as TabItem;
      if (ti == ribbonTabRip)
      {
        // Don't allow navigation, while Burning
        if (main.Burning)
          return;

        main.TracksGridView.Hide();
        main.BurnGridView.Hide();
        main.RipGridView.Show();
        if (!main.SplitterRight.IsCollapsed)
          main.SplitterRight.ToggleState();
      }
      else if (ti == ribbonTabBurn)
      {
        // Don't allow navigation, while Ripping
        if (main.Ripping)
          return;

        main.TracksGridView.Hide();
        main.RipGridView.Hide();
        main.BurnGridView.SetMediaInfo();
        main.BurnGridView.Show();
        if (!main.SplitterRight.IsCollapsed)
          main.SplitterRight.ToggleState();
      }
      else if (ti == ribbonTabConvert)
      {
        // Don't allow navigation, while Ripping or Burning
        if (main.Burning || main.Ripping)
          return;

        ribbonTextBoxConvertOutputDirectory.Text = Options.MainSettings.RipTargetFolder;

        main.TracksGridView.Hide();
        main.RipGridView.Hide();
        main.BurnGridView.Hide();
        main.ConvertGridView.Show();
        if (!main.SplitterRight.IsCollapsed)
          main.SplitterRight.ToggleState();
      }
      else
      {
        // Don't allow navigation, while Ripping or Burning
        if (main.Burning || main.Ripping)
          return;

        main.BurnGridView.Hide();
        main.RipGridView.Hide();
        main.TracksGridView.Show();
        if (main.SplitterRight.IsCollapsed && !main.RightSplitterStatus)
          main.SplitterRight.ToggleState();
      }
    }
    #endregion

    #region Start Menu and Quick Access Toolbar
    /// <summary>
    /// Hide the Minimize and "Show below" menu entries
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void quickAccessButton_DropDownOpened(object sender, EventArgs e)
    {
      RadToolStripOverFlowButtonElement overflowButton = (RadToolStripOverFlowButtonElement)sender;
      overflowButton.Items[overflowButton.Items.Count - 2].Visibility = ElementVisibility.Collapsed;
      overflowButton.Items[overflowButton.Items.Count - 1].Visibility = ElementVisibility.Collapsed;
    }

    /// <summary>
    /// Show the Options Panel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void quickAccessMenuOptions_Click(object sender, EventArgs e)
    {
      MPTagThat.Preferences.Preferences dlgPreferences = new MPTagThat.Preferences.Preferences(main);
      main.ShowForm(dlgPreferences);
    }

    private void quickAccesMenuRefresh_Click(object sender, EventArgs e)
    {
      main.RefreshTrackList();
    }

    private void quickAccessMenuSave_Click(object sender, EventArgs e)
    {
      main.TracksGridView.Save();
    }

    private void startMenuSave_Click(object sender, EventArgs e)
    {
      main.TracksGridView.Save();
    }


    private void startMenuItemChangeDisplayColumns_Click(object sender, EventArgs e)
    {
      Form dlg = new MPTagThat.Dialogues.ColumnSelect(main.TracksGridView);
      main.ShowForm(dlg);
    }

    private void startMenuExit_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }
    #endregion

    #region Tags Tab
    /// <summary>
    /// Handle the Button Clicks
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButton_Click(object sender, EventArgs e)
    {
      // If no Rows are selected, select ALL of them and do the necessary action
      if (!main.TracksGridView.CheckSelections(true))
        return;

      RadButtonElement rb = sender as RadButtonElement;
      if (rb == ribbonButtonTagFromFile)
      {
        MPTagThat.FileNameToTag.FileNameToTag dlgFileNameToTag = new MPTagThat.FileNameToTag.FileNameToTag(main);
        main.ShowForm(dlgFileNameToTag);
      }
      else if (rb == ribbonButtonMultiEdit)
      {
        MPTagThat.TagEdit.MultiTagEdit dlgMultiTagEdit = new MPTagThat.TagEdit.MultiTagEdit(main);
        main.ShowForm(dlgMultiTagEdit);
      }
      else if (rb == ribbonButtonSingleEdit)
      {
        MPTagThat.TagEdit.SingleTagEdit dlgSingleTagEdit = new MPTagThat.TagEdit.SingleTagEdit(main);
        main.ShowForm(dlgSingleTagEdit);
      }
      else if (rb == ribbonButtonOrganise)
      {
        MPTagThat.Organise.OrganiseFiles dlgOrganise = new MPTagThat.Organise.OrganiseFiles(main);
        main.ShowForm(dlgOrganise);
      }
      else if (rb == ribbonButtonTagIdentify)
      {
        main.TracksGridView.IdentifyFiles();
      }
      else if (rb == ribbonButtonTagFromInternet)
      {
        MPTagThat.InternetLookup.InternetLookup lookup = new MPTagThat.InternetLookup.InternetLookup(main);
        lookup.SearchForAlbumInformation();
      }
      else if (rb == ribbonButtonGetCoverArt)
        main.TracksGridView.GetCoverArt();
      else if (rb == ribbonButtonGetLyrics)
        main.TracksGridView.GetLyrics();
      else if (rb == ribbonButtonAutoNumber)
        main.TracksGridView.AutoNumber();
      else if (rb == ribbonButtonRemoveComments)
        main.TracksGridView.RemoveComments();
      else if (rb == ribbonButtonRemovePictures)
        main.TracksGridView.RemovePictures();
    }


    /// <summary>
    /// Rename File Execute has been pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonRenameFileExecute_Click(object sender, EventArgs e)
    {
      MPTagThat.TagToFileName.TagToFileName dlgTagToFile = new MPTagThat.TagToFileName.TagToFileName(main, true);
    }

    private void ribbonButtonRenameFileOptions_Click(object sender, EventArgs e)
    {
      MPTagThat.TagToFileName.TagToFileName dlgTagToFile = new MPTagThat.TagToFileName.TagToFileName(main, false);
      main.ShowForm(dlgTagToFile);
    }

    /// <summary>
    /// Case Conversion Execute has been pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonCaseConversionExecute_Click(object sender, EventArgs e)
    {
      MPTagThat.CaseConversion.CaseConversion dlgCaseConversion = new MPTagThat.CaseConversion.CaseConversion(main, true);
      dlgCaseConversion.CaseConvertSelectedTracks();
    }

    /// <summary>
    /// Case Conversion Options has been pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonCaseConversionOptions_Click(object sender, EventArgs e)
    {
      MPTagThat.CaseConversion.CaseConversion dlgCaseConversion = new MPTagThat.CaseConversion.CaseConversion(main);
      main.ShowForm(dlgCaseConversion);
    }

    /// <summary>
    /// The Delete Tags Button has been pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtondeleteTags_Click(object sender, EventArgs e)
    {
      RadMenuItem rb = sender as RadMenuItem;
      if (rb == ribbonButtonDeleteAllTags)
        main.TracksGridView.DeleteTags(TagTypes.AllTags);
      else if (rb == ribbonButtondeleteID3V1)
        main.TracksGridView.DeleteTags(TagTypes.Id3v1);
      else if (rb == ribbonButtondeleteID3V2)
        main.TracksGridView.DeleteTags(TagTypes.Id3v2);
    }

    /// <summary>
    /// Execute the selectedScript
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ribbonButtonScriptExecute_Click(object sender, EventArgs e)
    {
      if (ribbonComboBoxScripts.SelectedIndex < 0)
        return;

      Item tag = (Item)(ribbonComboBoxScripts.SelectedItem as RadComboBoxItem).Value;
      main.TracksGridView.ExecuteScript(tag.Value);
    }

    /// <summary>
    /// Add to the Burning, Conversion or PlayList List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonAddToList_Click(object sender, EventArgs e)
    {
      RadButtonElement rb = sender as RadButtonElement;
      if (rb == ribbonButtonAddToBurnList)
        main.TracksGridView.tracksGrid_AddToBurner(sender, new EventArgs());
      else if (rb == ribbonButtonAddToConvertList)
        main.TracksGridView.tracksGrid_AddToConvert(sender, new EventArgs());
      else if (rb == ribbonButtonAddToPlayList)
        main.TracksGridView.tracksGrid_AddToPlayList(sender, new EventArgs());
    }


    /// <summary>
    /// Turn On / Off the Numbering on Click option
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ribbonButtonClickNumber_ToggleStateChanged(object sender, StateChangedEventArgs args)
    {
      if (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
        _numberingOnClick = true;
      else
        _numberingOnClick = false;
    }
    #endregion

    #region Burn Tab
    /// <summary>
    /// Burn the selected tracks to CD
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonBurn_Click(object sender, EventArgs e)
    {
      Burner burner = null;
      if (ribbonComboBoxBurner.SelectedIndex < 0)
        burner = (Burner)(ribbonComboBoxBurner.Items[0] as RadComboBoxItem).Value;
      else
        burner = (Burner)(ribbonComboBoxBurner.SelectedItem as RadComboBoxItem).Value;

      main.BurnGridView.SetActiveBurner(burner);
      main.BurnGridView.BurnAudioCD();
    }

    /// <summary>
    /// Cancel Burning
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonBurnCancel_Click(object sender, EventArgs e)
    {
      main.BurnGridView.BurnAudioCDCancel();
    }

    /// <summary>
    /// A Burner has been selected. Set it as active Burner
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonComboBoxBurner_SelectedIndexChanged(object sender, EventArgs e)
    {
      Burner burner = (Burner)(ribbonComboBoxBurner.SelectedItem as RadComboBoxItem).Value;
      // Set Speed Table
      ribbonComboBoxSpeed.Items.Clear();
      List<string> speeds = burner.SupportedDriveSpeed;
      if (speeds.Count > 0)
      {
        RadComboBoxItem ri = new RadComboBoxItem("Maximum", Convert.ToInt32(speeds[0].Trim(new char[] { 'x' })));
        ribbonComboBoxSpeed.Items.Add(ri);
        burner.SelectedWriteSpeed = (int)ri.Value;
        foreach (string speed in speeds)
        {
          ri = new RadComboBoxItem(speed, Convert.ToInt32(speed.Trim(new char[] { 'x' })));
          ribbonComboBoxSpeed.Items.Add(ri);
        }
      }
      if (ribbonComboBoxSpeed.Items.Count > 0)
        ribbonComboBoxSpeed.SelectedIndex = 0;

      main.BurnGridView.SetActiveBurner(burner);
    }

    /// <summary>
    /// The Speed has been selected. Set it for the active Burner
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonComboBoxSpeed_SelectedIndexChanged(object sender, EventArgs e)
    {
      Burner burner = (Burner)(ribbonComboBoxBurner.SelectedItem as RadComboBoxItem).Value;
      burner.SelectedWriteSpeed = (int)(ribbonComboBoxSpeed.SelectedItem as RadComboBoxItem).Value;
    }
    #endregion

    #region Tab Rip
    /// <summary>
    /// Rip the selected CDs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonRip_Click(object sender, EventArgs e)
    {
      main.RipGridView.RipAudioCD();
    }

    /// <summary>
    /// Cancel the Ripping
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ribbonButtonRipCancel_Click(object sender, EventArgs e)
    {
      main.RipGridView.RipAudioCDCancel();
    }

    private void ribbonButtonRipSelectOutputDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog oFD = new FolderBrowserDialog();
      //oFD.RootFolder = Environment.SpecialFolder.MyDocuments;
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        ribbonTextBoxRipOutputDirectory.Text = oFD.SelectedPath;
      }
    }
    #endregion

    #region Tab Convert
    private void ribbonButtonConvert_Click(object sender, EventArgs e)
    {
      main.ConvertGridView.ConvertFiles();
    }

    private void ribbonButtonConvertCancel_Click(object sender, EventArgs e)
    {
      main.ConvertGridView.ConvertFilesCancel();
    }

    private void ribbonButtonSelectOutputDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog oFD = new FolderBrowserDialog();
      //oFD.RootFolder = Environment.SpecialFolder.MyDocuments;
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        ribbonTextBoxConvertOutputDirectory.Text = oFD.SelectedPath;
      }
    }
    #endregion

    /// <summary>
    /// The Theme has been changed
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    private void radRibbonBar_ThemeNameChanged(object source, ThemeNameChangedEventArgs args)
    {
      ServiceScope.Get<IThemeManager>().ChangeTheme(args.newThemeName);
    }

    /// <summary>
    /// Help Button has been pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void helpButton_Click(object sender, EventArgs e)
    {
      MPTagThat.Dialogues.About dlgAbout = new MPTagThat.Dialogues.About();
      main.ShowForm(dlgAbout);
    }
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
          LanguageChanged();
          this.Refresh();
          break;
      }
    }
    #endregion
  }
}
