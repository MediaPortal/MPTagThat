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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.Amazon;

#endregion

namespace MPTagThat.Dialogues
{
  public partial class AmazonAlbumSearchResults : ShapedForm
  {
    #region Variables

    private readonly List<AmazonAlbum> albums;
    private readonly ImageList imagelist = new ImageList();
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();

    #endregion

    #region Properties

    public int SelectedListItem
    {
      get { return lvSearchResults.SelectedIndices[0]; }
    }

    public string FileDetails
    {
      set { lbFileDetails.Text = value; }
    }

    public string Artist
    {
      set { lbArtistDetail.Text = value; }
    }

    public string Album
    {
      set { lbAlbumDetail.Text = value; }
    }

    #endregion

    #region ctor

    public AmazonAlbumSearchResults(List<AmazonAlbum> albums)
    {
      InitializeComponent();

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      LocaliseScreen();

      lbFileDetails.Text = "";
      lbAlbumDetail.Text = "";
      lbArtistDetail.Text = "";

      this.albums = albums;
      FillResults();

      lvSearchResults.View = View.LargeIcon;
      imagelist.ImageSize = new Size(128, 128);
      lvSearchResults.LargeImageList = imagelist;
    }

    #endregion

    #region Methods

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Text = localisation.ToString("AmazonAlbumSearch", "Header");
      chAlbum.Text = localisation.ToString("AmazonAlbumSearch", "Albums");
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

      using (MemoryStream ms = new MemoryStream(album.AlbumImage.Data))
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
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void btUpdate_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void lvSearchResults_DoubleClick(object sender, EventArgs e)
    {
      btUpdate.PerformClick();
    }

    #endregion
  }
}