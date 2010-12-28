#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MPTagThat.Core;
using MPTagThat.Core.WinControls;
using Un4seen.Bass.AddOn.Wma;

#endregion

namespace MPTagThat.Preferences
{
  public partial class Preferences : Form
  {
    #region Variables

    private readonly List<Item> amazonSites = new List<Item>();
    private readonly List<Item> encoders = new List<Item>();
    private readonly List<Item> encodersWMA = new List<Item>();
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly Main main;
    private readonly List<ActionWindow> mapWindows = new List<ActionWindow>();
    private readonly List<Item> presetsMPC = new List<Item>();
    private readonly List<Item> presetsWV = new List<Item>();
    private readonly List<Item> themes = new List<Item>();

    private int _defaultBitRateIndex;
    private bool _keysChanged;
    private TreeNode _selectedNode;
    private string headerText = "General";
    private Theme prevTheme;

    private TreeNode windowsNode;

    #endregion

    #region ctor

    public Preferences(Main main)
    {
      this.main = main;
      InitializeComponent();
      LocaliseScreen();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;
    }

    #endregion

    #region Methods

    #region Form Load

    private void OnLoad(object sender, EventArgs e)
    {
      log.Trace(">>>");
      //textBoxPresetDesc.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      //textBoxPresetDesc.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
      //treeViewKeys.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      //treeViewKeys.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;

      // Set the region for the Tabcontrol to hide the tabs
      tabControlOptions.Region = new Region(new RectangleF(tabPageGeneral.Left,
                                                           tabPageGeneral.Top,
                                                           tabPageGeneral.Width,
                                                           tabPageGeneral.Height));

      #region TabPage General

      // Get available languages
      CultureInfo[] availLanguages = localisation.AvailableLanguages();
      comboBoxLanguage.DisplayMember = "DisplayName";
      comboBoxLanguage.ValueMember = "TwoLetterISOLanguageName";
      comboBoxLanguage.DataSource = availLanguages;
      comboBoxLanguage.SelectedValue = localisation.CurrentCulture.Name;

      // Themes
      themes.Add(new Item(localisation.ToString("Settings", "Blue"), "ControlDefault", ""));
      themes.Add(new Item(localisation.ToString("Settings", "Silver"), "Office2007Silver", ""));
      themes.Add(new Item(localisation.ToString("Settings", "Black"), "Office2007Black", ""));
      comboBoxThemes.DisplayMember = "Name";
      comboBoxThemes.ValueMember = "Value";
      comboBoxThemes.DataSource = themes;
      comboBoxThemes.SelectedIndex = Options.MainSettings.Theme;

      // Save the currently used theme, in case the user presses Cancel.
      prevTheme = ServiceScope.Get<IThemeManager>().CurrentTheme;

      // Debug Level
      comboBoxDebugLevel.Items.Add("Off");
      comboBoxDebugLevel.Items.Add("Fatal");
      comboBoxDebugLevel.Items.Add("Error");
      comboBoxDebugLevel.Items.Add("Warn");
      comboBoxDebugLevel.Items.Add("Info");
      comboBoxDebugLevel.Items.Add("Debug");
      comboBoxDebugLevel.Items.Add("Trace");
      comboBoxDebugLevel.Text = Options.MainSettings.DebugLevel;

      // Load the keymap file into the treeview
      if (LoadKeyMap())
        PopulateKeyTreeView();

      #endregion

      #region TabPage Ripping

      #region General

      tbTargetFolder.Text = Options.MainSettings.RipTargetFolder == ""
                              ? Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
                              : Options.MainSettings.RipTargetFolder;
      ckRipEjectCD.Checked = Options.MainSettings.RipEjectCD;
      ckActivateTargetFolder.Checked = Options.MainSettings.RipActivateTargetFolder;
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderMP3"), "mp3", ""));
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderOgg"), "ogg", ""));
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderFlac"), "flac", ""));
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderAAC"), "m4a", ""));
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderWMA"), "wma", ""));
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderWAV"), "wav", ""));
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderMPC"), "mpc", ""));
      encoders.Add(new Item(localisation.ToString("Settings", "EncoderWV"), "wv", ""));
      comboBoxEncoder.DisplayMember = "Name";
      comboBoxEncoder.ValueMember = "Value";
      comboBoxEncoder.DataSource = encoders;

      foreach (Item item in encoders)
      {
        if ((string)item.Value == Options.MainSettings.RipEncoder)
        {
          comboBoxEncoder.SelectedItem = item;
          break;
        }
      }

      #endregion

      #region AAC

      comboBoxAACBitrates.Items.AddRange(Options.BitRatesLCAAC);
      SetBitRate();

      #endregion

      #region WMA

      encodersWMA.Add(new Item(localisation.ToString("Settings", "EncoderWmaStandard"), "wma", ""));
      encodersWMA.Add(new Item(localisation.ToString("Settings", "EncoderWmaPro"), "wmapro", ""));
      encodersWMA.Add(new Item(localisation.ToString("Settings", "EncoderWmaLossless"), "wmalossless", ""));
      comboBoxWMAEncoderFormat.DisplayMember = "Name";
      comboBoxWMAEncoderFormat.ValueMember = "Value";
      comboBoxWMAEncoderFormat.DataSource = encodersWMA;

      int selectedIndex = 0;
      foreach (Item item in encodersWMA)
      {
        if ((string)item.Value == Options.MainSettings.RipEncoderWMA)
        {
          comboBoxWMAEncoderFormat.SelectedItem = item;
          // Set the selected index and fire the selected index events for filling the bitrate and channel combo boxes
          comboBoxWMAEncoderFormat.SelectedIndex = selectedIndex;
          comboBoxWMAEncoderFormat_SelectedIndexChanged(null, null);
          break;
        }
        selectedIndex++;
      }

      #endregion

      #region MPC

      presetsMPC.Add(new Item(localisation.ToString("Settings", "EncoderMPCStandard"), "standard", ""));
      presetsMPC.Add(new Item(localisation.ToString("Settings", "EncoderMPCxtreme"), "xtreme", ""));
      presetsMPC.Add(new Item(localisation.ToString("Settings", "EncoderMPCinsane"), "insane", ""));
      presetsMPC.Add(new Item(localisation.ToString("Settings", "EncoderMPCbraindead"), "braindead", ""));
      comboBoxMPCPresets.DisplayMember = "Name";
      comboBoxMPCPresets.ValueMember = "Value";
      comboBoxMPCPresets.DataSource = presetsMPC;

      foreach (Item item in presetsMPC)
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

      presetsWV.Add(new Item(localisation.ToString("Settings", "EncoderWVFast"), "-f", ""));
      presetsWV.Add(new Item(localisation.ToString("Settings", "EncoderWVHigh"), "-h", ""));
      comboBoxWVPresets.DisplayMember = "Name";
      comboBoxWVPresets.ValueMember = "Value";
      comboBoxWVPresets.DataSource = presetsWV;

      foreach (Item item in presetsWV)
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

      ckCopyArtistToAlbumArtist.Checked = Options.MainSettings.CopyArtist;
      ckAutoFillNumberOfTracks.Checked = Options.MainSettings.AutoFillNumberOfTracks;
      ckUseCaseConversionWhenSaving.Checked = Options.MainSettings.UseCaseConversion;
      ckCreateMissingFolderThumb.Checked = Options.MainSettings.CreateFolderThumb;
      ckUseExistinbgThumb.Checked = Options.MainSettings.EmbedFolderThumb;
      ckOverwriteExistingCovers.Checked = Options.MainSettings.OverwriteExistingCovers;
      ckOverwriteExistingLyrics.Checked = Options.MainSettings.OverwriteExistingLyrics;

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

      amazonSites.Add(new Item("United States (US)", "com", ""));
      amazonSites.Add(new Item("Deutschland (DE)", "de", ""));
      amazonSites.Add(new Item("United Kingdom (UK)", "co.uk", ""));
      amazonSites.Add(new Item("Nippon (JP)", "jp", ""));
      amazonSites.Add(new Item("France (FR)", "fr", ""));
      amazonSites.Add(new Item("Canada (CA)", "ca", ""));
      comboBoxAmazonSite.DisplayMember = "Name";
      comboBoxAmazonSite.ValueMember = "Value";
      comboBoxAmazonSite.DataSource = amazonSites;

      foreach (Item item in amazonSites)
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

    #endregion

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
      if (!File.Exists(strFilename))
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
        tbAction.Text = Enum.GetName(typeof (Action.ActionType), action.ActionType);
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
    ///   A Key has been pressed
    /// </summary>
    /// <param name = "e"></param>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      // Only handle the key press, if the keyvalue field is focused
      if (tbKeyValue.Focused)
      {
        tbKeyValue.Text = Enum.GetName(typeof (Keys), keyData);
        return true;
      }

      return base.ProcessCmdKey(ref msg, keyData);
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

    #endregion

    #region Event Handler

    #region General

    /// <summary>
    ///   A Theme has been changed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void comboBoxThemes_SelectedIndexChanged(object sender, EventArgs e)
    {
      main.MainRibbon.Theme = (string)themes[comboBoxThemes.SelectedIndex].Value;
    }

    /// <summary>
    ///   Hide the Tabcontrol Tabs, as we navigate via Nav Bar
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void panelOptions_Paint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      Rectangle rect = new Rectangle(tabControlOptions.Location.X + 4, tabControlOptions.Location.Y + 3,
                                     tabPageGeneral.Width - 1, 22);
      g.FillRectangle(new SolidBrush(ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor), rect);
      g.DrawRectangle(new Pen(Color.LightSteelBlue), rect);
      g.DrawString(headerText, new Font(new FontFamily("Arial"), 12f, FontStyle.Bold),
                   new SolidBrush(ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor),
                   new PointF(tabControlOptions.Location.X + 8, tabControlOptions.Location.Y + 6));
    }


    /// <summary>
    ///   Apply the Changes to the Options
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonApply_Click(object sender, EventArgs e)
    {
      log.Trace(">>>");
      bool bErrors = false;
      string message = "";

      #region General

      string currentLanguage = ServiceScope.Get<ILocalisation>().CurrentCulture.Name;
      string selectedLanguage = (string)comboBoxLanguage.SelectedValue;

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

      Options.MainSettings.RipEncoder = (string)(comboBoxEncoder.SelectedValue as Item).Value;
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
      catch (FormatException) {}
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

      Options.MainSettings.CopyArtist = ckCopyArtistToAlbumArtist.Checked;
      Options.MainSettings.AutoFillNumberOfTracks = ckAutoFillNumberOfTracks.Checked;
      Options.MainSettings.UseCaseConversion = ckUseCaseConversionWhenSaving.Checked;
      Options.MainSettings.CreateFolderThumb = ckCreateMissingFolderThumb.Checked;
      Options.MainSettings.EmbedFolderThumb = ckUseExistinbgThumb.Checked;
      Options.MainSettings.OverwriteExistingCovers = ckOverwriteExistingCovers.Checked;
      Options.MainSettings.OverwriteExistingLyrics = ckOverwriteExistingLyrics.Checked;
      Options.MainSettings.MP3Validate = ckValidateMP3.Checked;
      Options.MainSettings.MP3AutoFix = ckAutoFixMp3.Checked;

      if (ckUseMediaPortalDatabase.Checked && File.Exists(tbMediaPortalDatabase.Text))
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
      Options.MainSettings.AmazonSite = (string)(comboBoxAmazonSite.SelectedValue as Item).Value;

      #endregion

      if (bErrors)
      {
        MessageBox.Show(message, localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      }
      else
        Close();
      log.Trace("<<<");
    }

    /// <summary>
    ///   Cancel changes
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      if (prevTheme.ThemeName != ServiceScope.Get<IThemeManager>().CurrentTheme.ThemeName)
      {
        main.MainRibbon.Theme = prevTheme.ThemeName;
      }
      Close();
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
        // Message sent, when a Theme is changing
        case "themechanged":
          {
            textBoxPresetDesc.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            textBoxPresetDesc.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
            treeViewKeys.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            treeViewKeys.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
            break;
         }
      }
    }
    #endregion

    #region Localisation

    private void LocaliseScreen()
    {
      Text = localisation.ToString("Settings", "Header");
      navPanel.CaptionText = localisation.ToString("Settings", "NavigationPanel");
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
          comboBoxWMACbrVbr.Items.AddRange(new[] {new Item(localisation.ToString("settings", "Vbr"), "Vbr", "")});
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

      string encoder = (string)(comboBoxWMAEncoderFormat.SelectedValue as Item).Value;
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

    #region Navigation Page

    /// <summary>
    ///   User selected a link in the navbar
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void OnNavpageLink_Clicked(object sender, MouseEventArgs e)
    {
      tabPageGeneral.Hide();
      tabPageTags.Hide();
      tabPageRipping.Hide();

      MPTLabel label = sender as MPTLabel;
      switch (label.Name)
      {
        case "lbLinkGeneral":
          headerText = localisation.ToString("settings", "LinkGeneral");
          tabPageGeneral.Show();
          break;

        case "lbLinkTags":
          headerText = localisation.ToString("settings", "LinkTags");
          tabPageTags.Show();
          break;

        case "lbLinkRipping":
          headerText = localisation.ToString("settings", "LinkRipping");
          tabPageRipping.Show();
          break;
      }
      panelOptions_Paint(panelOptions, new PaintEventArgs(panelOptions.CreateGraphics(), panelOptions.ClientRectangle));
    }

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

      if (!File.Exists(oFD.FileName))
      {
        // THe selected dababase does not exist. Ask if we create it.
        if (MessageBox.Show(localisation.ToString("Settings", "DBNotExists"), "", MessageBoxButtons.YesNo) ==
            DialogResult.Yes)
        {
          main.CreateMusicDatabase(oFD.FileName);
        }
      }
    }

    private void buttonStartDatabaseScan_Click(object sender, EventArgs e)
    {
      if (checkBoxClearDatabase.Checked)
      {
        if (File.Exists(tbMediaPortalDatabase.Text))
        {
          File.Delete(tbMediaPortalDatabase.Text);
        }
      }

      if (!File.Exists(tbMediaPortalDatabase.Text))
      {
        main.CreateMusicDatabase(tbMediaPortalDatabase.Text);
      }
      FolderBrowserDialog oFB = new FolderBrowserDialog();
      oFB.Description = localisation.ToString("Settings", "SelectMusicFolder");
      if (oFB.ShowDialog() == DialogResult.OK)
      {
        main.FillMusicDatabase(oFB.SelectedPath, tbMediaPortalDatabase.Text);
      }
    }

    private void buttonDBScanStatus_Click(object sender, EventArgs e)
    {
      lbDBScanStatus.Text = main.DatabaseScanStatus();
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
  }
}