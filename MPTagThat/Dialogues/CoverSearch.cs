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
  public partial class CoverSearch : ShapedForm
  {
    #region Variables

    private readonly ImageList imagelist = new ImageList();
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    
    private List<AmazonAlbum> _albums;
    private AmazonAlbum _amazonAlbum = null;
    private string _artist = "";
    private string _album = "";
    #endregion

    #region Properties

    /// <summary>
    /// Returns the selected Item
    /// </summary>
    public AmazonAlbum SelectedAlbum
    {
      get { return _amazonAlbum;  }
    }

    /// <summary>
    /// Sets the File detail which was searched
    /// </summary>
    public string FileDetails
    {
      set { lbFileDetails.Text = value; }
    }

    /// <summary>
    /// Set the Artist to search for
    /// </summary>
    public string Artist
    {
      set { tbArtist.Text = _artist = value; }
    }

    /// <summary>
    /// Set the Album to search for
    /// </summary>
    public string Album
    {
      set { tbAlbum.Text = _album = value; }
    }

    #endregion

    #region ctor

    public CoverSearch()
    {
      InitializeComponent();

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      LocaliseScreen();

      lbFileDetails.Text = "";
      tbArtist.Text = "";
      tbAlbum.Text = "";

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

    private void OnLoad(object sender, EventArgs e)
    {
      DoSearchAlbum();
    }

    private void DoSearchAlbum()
    {
      Cursor = Cursors.WaitCursor;
      tbArtist.Enabled = false;
      tbAlbum.Enabled = false;
      btSearch.Enabled = false;
      btUpdate.Enabled = false;
      using (AmazonAlbumInfo amazonInfo = new AmazonAlbumInfo())
      {
        _albums = amazonInfo.AmazonAlbumSearch(_artist, _album);
      }

      if (_albums.Count > 0)
      {
        btUpdate.Enabled = true;
        groupBoxAmazonMultipleAlbums.Text = ServiceScope.Get<ILocalisation>().ToString("AmazonAlbumSearch", "GroupBoxResults");
        if (_albums.Count == 1)
        {
          _amazonAlbum = _albums[0];
          btUpdate.PerformClick();  // Close the Dialog
        }
        else
        {
          FillResults();
        }
      }
      else
      {
        groupBoxAmazonMultipleAlbums.Text = ServiceScope.Get<ILocalisation>().ToString("AmazonAlbumSearch", "NotFound");
        ServiceScope.Get<ILogger>().GetLogger.Debug("No Cover Art found");
        btUpdate.Enabled = false;
      }
      tbArtist.Enabled = true;
      tbAlbum.Enabled = true;
      btSearch.Enabled = true;
      Cursor = Cursors.Default;
    }


    private void FillResults()
    {
      int i = 0;
      foreach (AmazonAlbum album in _albums)
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

    /// <summary>
    /// Cancel Button has been clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btClose_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    /// <summary>
    /// Update Button has been clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btUpdate_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      if (lvSearchResults.SelectedIndices.Count > 0)
      {
        _amazonAlbum = _albums[lvSearchResults.SelectedIndices[0]];   
      }
      else if (_albums.Count > 0)
      {
        _amazonAlbum = _albums[0];
      }
      Close();
    }

    /// <summary>
    /// Cancel All Button has been clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btCancelAll_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Abort;
      Close();
    }

    /// <summary>
    /// User Double clicked on a Cover
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void lvSearchResults_DoubleClick(object sender, EventArgs e)
    {
      btUpdate.PerformClick();
    }

    /// <summary>
    /// A Search is performed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btSearch_Click(object sender, EventArgs e)
    {
      _artist = tbArtist.Text;
      _album = tbAlbum.Text;
      lvSearchResults.Items.Clear();
      imagelist.Images.Clear();
      _albums.Clear();
      groupBoxAmazonMultipleAlbums.Text = ServiceScope.Get<ILocalisation>().ToString("AmazonAlbumSearch", "Searching");
      groupBoxAmazonMultipleAlbums.Refresh();
      DoSearchAlbum();
    }
    #endregion
  }
}