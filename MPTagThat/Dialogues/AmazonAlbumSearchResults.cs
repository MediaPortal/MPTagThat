using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MPTagThat.Core;
using MPTagThat.Core.Amazon;


namespace MPTagThat.Dialogues
{
  public partial class AmazonAlbumSearchResults : Form
  {
    #region Variables
    ImageList imagelist = new ImageList();
    List<AmazonAlbum> albums = null;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    #endregion

    #region Properties
    public int SelectedListItem
    {
      get { return lvSearchResults.SelectedIndices[0]; }
    }
    #endregion

    #region ctor
    public AmazonAlbumSearchResults(List<AmazonAlbum> albums)
    {
      InitializeComponent();

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      LocaliseScreen();

      this.albums = albums;
      FillResults();

      lvSearchResults.View = View.LargeIcon;
      imagelist.ImageSize = new Size(128, 128);
      lvSearchResults.LargeImageList = this.imagelist;

    }
    #endregion

    #region Methods
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Text = localisation.ToString("AmazonAlbumSearch", "Header");
      this.chAlbum.Text = localisation.ToString("AmazonAlbumSearch", "Albums");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    private void FillResults()
    {
      int i = 0;
      foreach (AmazonAlbum album in albums)
      {
        AddImageToList(album);
        string itmText = string.Format("{0} {1}x{2}", album.Title, album.CoverWidth, album.CoverHeight);
        ListViewItem item = new ListViewItem(itmText);
        item.ImageIndex = i;
        lvSearchResults.Items.Add(item);
        i++;
      }
    }

    private void AddImageToList(AmazonAlbum album)
    {
      if (album.AlbumImage == null)
        return;

      using (System.IO.MemoryStream ms = new System.IO.MemoryStream(album.AlbumImage.Data))
      {
        Image img = Image.FromStream(ms);
        if (img != null)
        {
          imagelist.Images.Add(img);
        }
      }
    }
    #endregion

    #region Events
    private void btClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btUpdate_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void lvSearchResults_DoubleClick(object sender, EventArgs e)
    {
      btUpdate.PerformClick();
    }
    #endregion
  }
}