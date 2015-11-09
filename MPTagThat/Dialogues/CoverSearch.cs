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
using System.Threading;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.AlbumInfo;

#endregion

namespace MPTagThat.Dialogues
{
  public partial class CoverSearch : ShapedForm, IAlbumInfo
  {
    #region Delegates

    private delegate void DelegateAlbumFound(List<Album> albums, String site);
    private delegate void DelegateSearchFinished();

    #endregion

    #region Variables

    private DelegateAlbumFound _albumFound;
    private DelegateSearchFinished _searchFinished;

    private readonly ImageList _imagelist = new ImageList();
    private readonly ILocalisation _localisation = ServiceScope.Get<ILocalisation>();
    
    private List<Album> _albums = new List<Album>();
    private Album _album = null;
    private string _artist = "";
    private string _albumName = "";

    #endregion

    #region Properties

    /// <summary>
    /// Returns the selected Item
    /// </summary>
    public Album SelectedAlbum
    {
      get { return _album;  }
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
      set { tbAlbum.Text = _albumName = value; }
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
      _imagelist.ImageSize = new Size(128, 128);
      lvSearchResults.LargeImageList = _imagelist;

      _albumFound = new DelegateAlbumFound(AlbumFoundMethod);
      _searchFinished = new DelegateSearchFinished(SearchFinishedMethod);
    }

    #endregion

    #region Methods

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Text = _localisation.ToString("AmazonAlbumSearch", "Header");
      chAlbum.Text = _localisation.ToString("AmazonAlbumSearch", "Albums");
    }

    #endregion

    private void OnShown(object sender, EventArgs e)
    {
      DoSearchAlbum();
    }

    private void DoSearchAlbum()
    {
			groupBoxAmazonMultipleAlbums.Text = ServiceScope.Get<ILocalisation>().ToString("AmazonAlbumSearch", "Searching");
			groupBoxAmazonMultipleAlbums.Refresh();
			Update();

			Cursor = Cursors.WaitCursor;
      tbArtist.Enabled = false;
      tbAlbum.Enabled = false;
      btSearch.Enabled = false;
      btUpdate.Enabled = false;

      var albumSearch = new AlbumSearch(this, _artist, _albumName);
      albumSearch.AlbumSites = Options.MainSettings.AlbumInfoSites;
      albumSearch.Run();
    }


    private void FillResults(List<Album> albums, string site)
    {
      int i = 0;
      foreach (var album in albums)
      {
        AddImageToList(album);
				var albumSize = (album.CoverWidth == "0" || album.CoverWidth == "") ? " " : string.Format(" {0}x{1} ", album.CoverWidth, album.CoverHeight);
        var itmText = string.Format("{0}{1}({2})", album.Title, albumSize, site);
	      var item = new ListViewItem(itmText) {ImageIndex = i};
	      lvSearchResults.Items.Add(item);
        i++;
      }
	    Update();
    }

    private void AddImageToList(Album album)
    {
      if (album.AlbumImage == null)
        return;

	    try
	    {
		    using (MemoryStream ms = new MemoryStream(album.AlbumImage.Data))
		    {
			    Image img = Image.FromStream(ms);
			    _imagelist.Images.Add(img);
		    }
	    }
			catch (ArgumentException)
	    {}
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
        _album = _albums[lvSearchResults.SelectedIndices[0]];   
      }
      else if (_albums.Count > 0)
      {
        _album = _albums[0];
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
      _albumName = tbAlbum.Text;
      lvSearchResults.Items.Clear();
      _imagelist.Images.Clear();
      _albums.Clear();
      DoSearchAlbum();
    }
    #endregion

    #region Delegate Calls

    public Object[] AlbumFound
    {
      set
      {
        if (IsDisposed == false)
        {
          try
          {
            Invoke(_albumFound, value);
          }
          catch (InvalidOperationException) { }
        }
      }
    }

    public Object[] SearchFinished
    {
      set
      {
        if (IsDisposed == false)
        {
          try
          {
            Invoke(_searchFinished, value);
          }
          catch (InvalidOperationException) { }
        }
      }
    }


    private void AlbumFoundMethod(List<Album> albums, string siteName)
    {
      btUpdate.Enabled = true;
      groupBoxAmazonMultipleAlbums.Text = ServiceScope.Get<ILocalisation>().ToString("AmazonAlbumSearch", "GroupBoxResults");
      _albums.AddRange(albums);
      FillResults(albums, siteName);
    }

    private void SearchFinishedMethod()
    {
      if (_imagelist.Images.Count == 0)
      {
        groupBoxAmazonMultipleAlbums.Text = ServiceScope.Get<ILocalisation>().ToString("AmazonAlbumSearch", "NotFound");
        ServiceScope.Get<ILogger>().GetLogger.Debug("No Cover Art found");
        btUpdate.Enabled = false;
      }
      else
      {
        btUpdate.Enabled = true;
        groupBoxAmazonMultipleAlbums.Text = ServiceScope.Get<ILocalisation>().ToString("AmazonAlbumSearch", "GroupBoxResults");
        if (_imagelist.Images.Count == 1)
        {
          btUpdate.PerformClick();  // Close the Dialog
        }
      }
			Update();
      tbArtist.Enabled = true;
      tbAlbum.Enabled = true;
      btSearch.Enabled = true;
			Cursor = Cursors.Default;
    }

    #endregion
  }
}