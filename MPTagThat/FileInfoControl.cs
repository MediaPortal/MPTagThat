using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;
using TagLib;

namespace MPTagThat
{
  public partial class FileInfoControl : UserControl
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private IThemeManager themeManager = ServiceScope.Get<IThemeManager>();
    #endregion

    #region ctor
    public FileInfoControl(Main main)
    {
      _main = main;
      InitializeComponent();

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      LocaliseScreen();
      SetColorBase();

      listViewFileInfo.Columns.Add("", 70);
      listViewFileInfo.Columns.Add("", 200);
    }
    #endregion

    #region Public Methods
     /// <summary>
    /// Fills the File Info Panel
    /// </summary>
    public void FillPanel()
    {
      Image img = null;
      try
      {
        listViewFileInfo.Items.Clear();
        TrackData track = _main.TracksGridView.SelectedTrack;
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
    /// Clears the information in the FileInfo Panel
    /// </summary>
    public void ClearFileInfoPanel()
    {
      pictureBoxAlbumArt.Image = null;
      listViewFileInfo.Items.Clear();
    }
    #endregion

    #region Private Methods
    #region Localisation / Colors
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      // Extended Panels. Doing it via TTExtendedPanel doesn't work for some reason
      this.picturePanel.CaptionText = localisation.ToString("main", "PicturePanel");
      this.fileInfoPanel.CaptionText = localisation.ToString("main", "InformationPanel");
    }

    private void SetColorBase()
    {
      this.BackColor = themeManager.CurrentTheme.BackColor;
      pictureBoxAlbumArt.BackColor = themeManager.CurrentTheme.BackColor;
      listViewFileInfo.BackColor = themeManager.CurrentTheme.BackColor;
      listViewFileInfo.ForeColor = themeManager.CurrentTheme.LabelForeColor;
    }
    #endregion


    /// <summary>
    /// Save the currently selected picture as folder.jpg
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSaveFolderThumb_Click(object sender, EventArgs e)
    {
      TrackData track = _main.TracksGridView.SelectedTrack;
      _main.TracksGridView.SavePicture(track);
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
        case "themechanged":
          {
            SetColorBase();
            break;
          }

        case "languagechanged":
          {
            LocaliseScreen();
            this.Refresh();
            break;
          }
      }
    }
    #endregion
  }
}
