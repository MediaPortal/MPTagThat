using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPTagThat.Core;

namespace MPTagThat.InternetLookup
{
  public partial class AlbumDetails : Telerik.WinControls.UI.ShapedForm
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    #endregion

    #region Properties
    public string Artist
    {
      get { return tbArtist.Text.Trim(); }
      set { tbArtist.Text = value; }
    }

    public string Album
    {
      get { return tbAlbum.Text.Trim(); }
      set { tbAlbum.Text = value; }
    }

    public string Year
    {
      get
      {
        string strYear = tbYear.Text.Trim();
        if (strYear.Length > 4)
          strYear = strYear.Substring(0, 4);

        return strYear;
      }
      set
      {
        string strYear = value;
        if (strYear == null) 
          strYear = "";

        if (strYear.Length > 4)
          strYear = strYear.Substring(0, 4);

        tbYear.Text = strYear;
      }
    }

    public string Genre
    {
      get { return cbGenre.SelectedText; }
      set { cbGenre.SelectedText = value; }
    }

    public PictureBox Cover
    {
      get { return pictureBoxCover; }
    }

    public ListView AlbumTracks
    {
      get { return lvAlbumTracks; }
    }

    public ListView DiscTracks
    {
      get { return lvDiscTracks; }
    }
    #endregion

    #region ctor
    public AlbumDetails()
    {
      InitializeComponent();

      // Fill the Genre Combo Box
      cbGenre.Items.AddRange(TagLib.Genres.Audio);

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
      this.labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

      LocaliseScreen();
    }
    #endregion

    #region Methods
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      this.labelHeader.Text = localisation.ToString("Lookup", "HeaderDetails");
      this.chTrackNum.Text = localisation.ToString("Lookup", "ColTrackNum");
      this.chTitle.Text = localisation.ToString("Lookup", "ColTitle");
      this.chFileName.Text = localisation.ToString("Lookup", "ColFileName");
    }

    /// <summary>
    /// Renumber the list, when the item was moved with the buttons
    /// </summary>
    public void Renumber()
    {
      for (int i = 0; i < lvDiscTracks.Items.Count; i++)
      {
        lvDiscTracks.Items[i].SubItems[0].Text = (i + 1).ToString();
      }
    }
    #endregion

    #endregion

    #region Button Events
    private void btUp_Click(object sender, EventArgs e)
    {
      // Set focus back on Listview
      lvDiscTracks.Focus();

      int currPos = lvDiscTracks.SelectedIndices[0];
      if (currPos == 0)
        return;

      ListViewItem lvItem = lvDiscTracks.Items[currPos];
      if ((int)lvItem.Tag > -1)
        lvItem.Checked = true;

      lvDiscTracks.Items.RemoveAt(currPos);
      lvDiscTracks.Items.Insert(currPos - 1, lvItem);
      lvDiscTracks.Items[currPos - 1].Selected = true;

      Renumber();
    }

    private void btDown_Click(object sender, EventArgs e)
    {
      // Set focus back on Listview
      lvDiscTracks.Focus();

      int currPos = lvDiscTracks.SelectedIndices[0];
      if (currPos == lvDiscTracks.Items.Count - 1)
        return;

      ListViewItem lvItem = lvDiscTracks.Items[currPos];
      if ((int)lvItem.Tag > -1)
        lvItem.Checked = true;

      lvDiscTracks.Items.RemoveAt(currPos);
      lvDiscTracks.Items.Insert(currPos + 1, lvItem);
      lvDiscTracks.Items[currPos + 1].Selected = true;

      Renumber();
    }
    #endregion
  }
}
