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
using System.Linq;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.AlbumInfo;
using TagLib;

#endregion

namespace MPTagThat.InternetLookup
{
  public class InternetLookup : IAlbumInfo
  {
		#region Variables

		private readonly DataGridView _tracksGrid;
    private readonly Main main;

    private bool _askForAlbum;
		private List<Album> _albums = new List<Album>();
		private string _selectedAlbum = "";
    private string _selectedArtist = "";

		private Album _album = null;

		// Dialogs
		private ArtistAlbumDialog _dlgAlbumArtist;
    private AlbumDetails _dlgAlbumDetails;
    private AlbumSearchResult _dlgSearchResult;

	  private delegate void ThreadSafeDelegate();

		#endregion

		#region ctor

		public InternetLookup(Main main)
    {
      this.main = main;
      _tracksGrid = main.TracksGridView.View;
		}

    #endregion

    #region Methods

    public void SearchForAlbumInformation()
    {
      // Loop through the selected rows and see, if we got an Artist and/or Album set
      // Need at least an album
      foreach (DataGridViewRow row in _tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        if (_selectedArtist == "")
          _selectedArtist = track.Artist;

        if (_selectedAlbum == "")
          _selectedAlbum = track.Album;

        // If we found both values, we can leave
        if (_selectedAlbum != "" && _selectedArtist != "")
          break;
      }

      _askForAlbum = (_selectedAlbum == "");

			// If no Album was specified, we need to show the select dialog
			if (_askForAlbum)
			{
				if (!RequestArtistAlbum())
					return;
			}

	    _askForAlbum = false;
			main.Cursor = Cursors.WaitCursor;
			_dlgSearchResult = new AlbumSearchResult();

			var albumSearch = new AlbumSearch(this, _selectedArtist, _selectedAlbum);
			albumSearch.AlbumSites = Options.MainSettings.AlbumInfoSites;
			albumSearch.Run();
		}

    private bool RequestArtistAlbum()
    {
      _dlgAlbumArtist = new ArtistAlbumDialog();
      _dlgAlbumArtist.Artist = _selectedArtist;
      _dlgAlbumArtist.Album = _selectedAlbum;

      if (main.ShowModalDialog(_dlgAlbumArtist) != DialogResult.OK)
      {
        _dlgAlbumArtist.Dispose();
        return false;
      }

      _selectedArtist = _dlgAlbumArtist.Artist;
      _selectedAlbum = _dlgAlbumArtist.Album;
      _dlgAlbumArtist.Dispose();
      return true;
    }

		private void FillResults(List<Album> albums, string site)
		{
			int i = 0;
			foreach (var album in albums)
			{
				int trackCount = album.Discs.Sum(tracks => tracks.Count);

				var lvItem = new ListViewItem(album.Artist);
				lvItem.SubItems.Add(album.Title);
				lvItem.SubItems.Add(trackCount.ToString());
				lvItem.SubItems.Add(album.Year);
				lvItem.SubItems.Add(site);
				lvItem.Tag = album;
				_dlgSearchResult.ResultView.Items.Insert(0, lvItem);
			}
		}

    public void ShowAlbumDetails(Album album)
    {
      _dlgAlbumDetails = new AlbumDetails();

      // Prepare the Details Dialog
      _dlgAlbumDetails.Artist = album.Artist;
      _dlgAlbumDetails.Album = album.Title;
      _dlgAlbumDetails.Year = album.Year;

      try
      {
        using (MemoryStream ms = new MemoryStream(album.AlbumImage.Data))
        {
          Image img = Image.FromStream(ms);
					_dlgAlbumDetails.Cover.Image = img;
        }
      }
      catch {}

      // Add Tracks of the selected album
      foreach (List<AlbumTrack> disc in album.Discs)
      {
        foreach (AlbumTrack track in disc)
        {
          ListViewItem lvItem = new ListViewItem(track.Number.ToString());
          lvItem.SubItems.Add(track.Title);
          _dlgAlbumDetails.AlbumTracks.Items.Add(lvItem);
        }
      }

      // Add selected Files from Grid
      foreach (DataGridViewRow row in _tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        ListViewItem lvItem = new ListViewItem((row.Index + 1).ToString());
        lvItem.SubItems.Add(track.FileName);
        lvItem.Tag = row.Index;
        _dlgAlbumDetails.DiscTracks.Items.Add(lvItem);
      }

      // if we have less files selected, than in the album, fill the rest with"unassigned"
      if (_dlgAlbumDetails.DiscTracks.Items.Count < _dlgAlbumDetails.AlbumTracks.Items.Count)
      {
        for (int i = _dlgAlbumDetails.DiscTracks.Items.Count - 1; i < _dlgAlbumDetails.AlbumTracks.Items.Count - 1; i++)
        {
          ListViewItem unassignedItem = new ListViewItem((i + 2).ToString());
          unassignedItem.SubItems.Add(ServiceScope.Get<ILocalisation>().ToString("Lookup", "Unassigned"));
          unassignedItem.Tag = -1;
          unassignedItem.Checked = false;
          _dlgAlbumDetails.DiscTracks.Items.Add(unassignedItem);
        }
      }

      int albumTrackPos = 0;
      foreach (ListViewItem lvAlbumItem in _dlgAlbumDetails.AlbumTracks.Items)
      {
        int discTrackPos = 0;
        foreach (ListViewItem lvDiscItem in _dlgAlbumDetails.DiscTracks.Items)
        {
          if (Util.LongestCommonSubstring(lvAlbumItem.SubItems[1].Text, lvDiscItem.SubItems[1].Text) > 0.75)
          {
            lvDiscItem.Checked = true;
            _dlgAlbumDetails.DiscTracks.Items.RemoveAt(discTrackPos);
            _dlgAlbumDetails.DiscTracks.Items.Insert(albumTrackPos, lvDiscItem);
            break;
          }
          discTrackPos++;
        }
        albumTrackPos++;
      }

      _dlgAlbumDetails.Renumber();

      if (main.ShowModalDialog(_dlgAlbumDetails) == DialogResult.OK)
      {
        int i = -1;
        foreach (ListViewItem lvItem in _dlgAlbumDetails.DiscTracks.Items)
        {
          i++;
          int index = (int)lvItem.Tag;
          if (index == -1 || lvItem.Checked == false)
            continue;

          TrackData track = Options.Songlist[index];
          track.Artist = _dlgAlbumDetails.Artist;
          track.Album = _dlgAlbumDetails.Album;
          string strYear = _dlgAlbumDetails.Year;
          if (strYear.Length > 4)
            strYear = strYear.Substring(0, 4);

          int year = 0;
          try
          {
            year = Convert.ToInt32(strYear);
          }
          catch (Exception) {}
          if (year > 0 && track.Year == 0)
            track.Year = year;

          // Add the picture
          ByteVector vector = album.AlbumImage;
          if (vector != null)
          {
            MPTagThat.Core.Common.Picture pic = new MPTagThat.Core.Common.Picture();
            pic.MimeType = "image/jpg";
            pic.Description = "";
            pic.Type = PictureType.FrontCover;
            pic.Data = vector.Data;
            track.Pictures.Add(pic);
          }

          ListViewItem trackItem = _dlgAlbumDetails.AlbumTracks.Items[i];
          track.Track = trackItem.SubItems[0].Text;
          track.Title = trackItem.SubItems[1].Text;

          main.TracksGridView.SetBackgroundColorChanged(index);
          track.Changed = true;
          Options.Songlist[index] = track;
          main.TracksGridView.Changed = true;
        }
      }
      _dlgAlbumDetails.Dispose();
    }

		#region Delegate Calls

		public Object[] AlbumFound
		{
			set
			{
				AlbumFoundMethod((List<Album>)value[0], (string) value[1]);
			}
		}

		public Object[] SearchFinished
		{
			set
			{
				SearchFinishedMethod();
			}
		}


		private void AlbumFoundMethod(List<Album> albums, string siteName)
		{
			_albums.AddRange(albums);
			FillResults(albums, siteName);
		}

		private void SearchFinishedMethod()
		{
			if (main.InvokeRequired)
			{
				ThreadSafeDelegate d = SearchFinishedMethod;
				main.Invoke(d);
				return;
			}

			main.Cursor = Cursors.Default;

      if (_dlgSearchResult.ResultView.Items.Count == 0)
			{
				ServiceScope.Get<ILogger>().GetLogger.Debug("No AlbumInformation found");
				if (RequestArtistAlbum())
				{
					var albumSearch = new AlbumSearch(this, _selectedArtist, _selectedAlbum);
					albumSearch.AlbumSites = Options.MainSettings.AlbumInfoSites;
					albumSearch.Run();
					return;
				}
			}
			else if (_dlgSearchResult.ResultView.Items.Count == 1)
			{
				// We might have ended up, with just only one Album
				_album = _albums[0];
				ShowAlbumDetails(_album);
			}
			else
			{
				if (main.ShowModalDialog(_dlgSearchResult) == DialogResult.OK)
				{
					if (_dlgSearchResult.ResultView.SelectedIndices.Count > 0)
					{
						_album = (Album) _dlgSearchResult.ResultView.SelectedItems[0].Tag;
					}
					else
					{
						_album = _albums[0];
					}
					ShowAlbumDetails(_album);
				}
				else
				{
					// Don't ask for album again, since the user cancelled
					_askForAlbum = false;
					_dlgSearchResult.Dispose();
				}
			}
			_dlgSearchResult.Dispose();
		}

		#endregion

		#endregion
	}
}