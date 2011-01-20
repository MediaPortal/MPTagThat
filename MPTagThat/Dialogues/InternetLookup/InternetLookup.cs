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
using TagLib;

#endregion

namespace MPTagThat.InternetLookup
{
  public class InternetLookup
  {
    #region Variables

    private readonly DataGridView _tracksGrid;
    private readonly Main main;

    private bool _askForAlbum;
    private string _selectedAlbum = "";
    private string _selectedArtist = "";

    // Dialogs
    private ArtistAlbumDialog dlgAlbumArtist;
    private AlbumDetails dlgAlbumDetails;
    private AlbumSearchResult dlgSearchResult;

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

        TrackData track = main.TracksGridView.TrackList[row.Index];
        if (_selectedArtist == "")
          _selectedArtist = track.Artist;

        if (_selectedAlbum == "")
          _selectedAlbum = track.Album;

        // If we found both values, we can leave
        if (_selectedAlbum != "" && _selectedArtist != "")
          break;
      }

      _askForAlbum = (_selectedAlbum == "");

      while (true)
      {
        // If no Album was specified, we need to show the select dialog
        if (_askForAlbum)
        {
          if (!RequestArtistAlbum())
            break;
        }

        AmazonAlbum album = GetAlbumInformation();

        // It may happen that an album doesn't return Track Information
        // Inform the uer to make a new selection
        if (album == null || album.Discs.Count == 0)
        {
          if (_askForAlbum)
          {
            MessageBox.Show(ServiceScope.Get<ILocalisation>().ToString("Lookup", "NoAlbumFound"),
                            ServiceScope.Get<ILocalisation>().ToString("message", "Error"), MessageBoxButtons.OK);
            continue;
          }
          else
            break;
        }

        ShowAlbumDetails(album);

        // Now that we've come so far, we don't need to reshow that artist / album selection panel
        break;
      }
    }

    public bool RequestArtistAlbum()
    {
      dlgAlbumArtist = new ArtistAlbumDialog();
      dlgAlbumArtist.Artist = _selectedArtist;
      dlgAlbumArtist.Album = _selectedAlbum;

      if (main.ShowModalDialog(dlgAlbumArtist) != DialogResult.OK)
      {
        dlgAlbumArtist.Dispose();
        return false;
      }

      _selectedArtist = dlgAlbumArtist.Artist;
      _selectedAlbum = dlgAlbumArtist.Album;
      dlgAlbumArtist.Dispose();
      return true;
    }

    public AmazonAlbum GetAlbumInformation()
    {
      main.Cursor = Cursors.WaitCursor;
      List<AmazonAlbum> albums = new List<AmazonAlbum>();
      using (AmazonAlbumInfo amazonInfo = new AmazonAlbumInfo())
      {
        albums = amazonInfo.AmazonAlbumSearch(_selectedArtist, _selectedAlbum);
      }

      // Show the Album Selection dialog, if we got more than one album
      AmazonAlbum amazonAlbum = null;
      _askForAlbum = true;
      if (albums.Count > 0)
      {
        _askForAlbum = false;
        if (albums.Count == 1)
        {
          amazonAlbum = albums[0];
          // If we didn't get any disc info, consider the album as not found
          if (amazonAlbum.Discs.Count == 0)
            _askForAlbum = true;
        }
        else
        {
          dlgSearchResult = new AlbumSearchResult();
          foreach (AmazonAlbum foundAlbum in albums)
          {
            // Skip Albums with no Discs returned
            if (foundAlbum.Discs.Count == 0)
              continue;

            // count the number of tracks, as we may have multiple discs
            int trackCount = 0;
            foreach (List<AmazonAlbumTrack> tracks in foundAlbum.Discs)
              trackCount += tracks.Count;

            ListViewItem lvItem = new ListViewItem(foundAlbum.Artist);
            lvItem.SubItems.Add(foundAlbum.Title);
            lvItem.SubItems.Add(trackCount.ToString());
            lvItem.SubItems.Add(foundAlbum.Year);
            lvItem.SubItems.Add(foundAlbum.Label);
            dlgSearchResult.ResultView.Items.Add(lvItem);
          }
          main.Cursor = Cursors.Default;

          // When the Listview contains no items, none of the found albums has track information
          if (dlgSearchResult.ResultView.Items.Count == 0)
            _askForAlbum = true;
          else
          {
            if (main.ShowModalDialog(dlgSearchResult) == DialogResult.OK)
            {
              if (dlgSearchResult.ResultView.SelectedIndices[0] > -1)
                amazonAlbum = albums[dlgSearchResult.ResultView.SelectedIndices[0]];
              else
                amazonAlbum = albums[0];
            }
            else
            {
              // Don't ask for album again, since the user cancelled
              _askForAlbum = false;
              dlgSearchResult.Dispose();
              return amazonAlbum;
            }
          }
          dlgSearchResult.Dispose();
        }
      }
      main.Cursor = Cursors.Default;
      return amazonAlbum;
    }

    public void ShowAlbumDetails(AmazonAlbum album)
    {
      dlgAlbumDetails = new AlbumDetails();

      // Prepare the Details Dialog
      dlgAlbumDetails.Artist = album.Artist;
      dlgAlbumDetails.Album = album.Title;
      dlgAlbumDetails.Year = album.Year;

      try
      {
        using (MemoryStream ms = new MemoryStream(album.AlbumImage.Data))
        {
          Image img = Image.FromStream(ms);
          if (img != null)
          {
            dlgAlbumDetails.Cover.Image = img;
          }
        }
      }
      catch {}

      // Add Tracks of the selected album
      foreach (List<AmazonAlbumTrack> disc in album.Discs)
      {
        foreach (AmazonAlbumTrack track in disc)
        {
          ListViewItem lvItem = new ListViewItem(track.Number.ToString());
          lvItem.SubItems.Add(track.Title);
          dlgAlbumDetails.AlbumTracks.Items.Add(lvItem);
        }
      }

      // Add selected Files from Grid
      foreach (DataGridViewRow row in _tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = main.TracksGridView.TrackList[row.Index];
        ListViewItem lvItem = new ListViewItem((row.Index + 1).ToString());
        lvItem.SubItems.Add(track.FileName);
        lvItem.Tag = row.Index;
        dlgAlbumDetails.DiscTracks.Items.Add(lvItem);
      }

      // if we have less files selected, than in the album, fill the rest with"unassigned"
      if (dlgAlbumDetails.DiscTracks.Items.Count < dlgAlbumDetails.AlbumTracks.Items.Count)
      {
        for (int i = dlgAlbumDetails.DiscTracks.Items.Count - 1; i < dlgAlbumDetails.AlbumTracks.Items.Count - 1; i++)
        {
          ListViewItem unassignedItem = new ListViewItem((i + 2).ToString());
          unassignedItem.SubItems.Add(ServiceScope.Get<ILocalisation>().ToString("Lookup", "Unassigned"));
          unassignedItem.Tag = -1;
          unassignedItem.Checked = false;
          dlgAlbumDetails.DiscTracks.Items.Add(unassignedItem);
        }
      }

      int albumTrackPos = 0;
      foreach (ListViewItem lvAlbumItem in dlgAlbumDetails.AlbumTracks.Items)
      {
        int discTrackPos = 0;
        foreach (ListViewItem lvDiscItem in dlgAlbumDetails.DiscTracks.Items)
        {
          if (Util.LongestCommonSubstring(lvAlbumItem.SubItems[1].Text, lvDiscItem.SubItems[1].Text) > 0.75)
          {
            lvDiscItem.Checked = true;
            dlgAlbumDetails.DiscTracks.Items.RemoveAt(discTrackPos);
            dlgAlbumDetails.DiscTracks.Items.Insert(albumTrackPos, lvDiscItem);
            break;
          }
          discTrackPos++;
        }
        albumTrackPos++;
      }

      dlgAlbumDetails.Renumber();

      if (main.ShowModalDialog(dlgAlbumDetails) == DialogResult.OK)
      {
        int i = -1;
        foreach (ListViewItem lvItem in dlgAlbumDetails.DiscTracks.Items)
        {
          i++;
          int index = (int)lvItem.Tag;
          if (index == -1 || lvItem.Checked == false)
            continue;

          TrackData track = main.TracksGridView.TrackList[index];
          track.Artist = dlgAlbumDetails.Artist;
          track.Album = dlgAlbumDetails.Album;
          string strYear = dlgAlbumDetails.Year;
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
            // Prepare Picture Array and then add the cover retrieved
            List<IPicture> pics = new List<IPicture>();
            Picture pic = new Picture(vector);
            pics.Add(pic);
            track.Pictures = pics.ToArray();
          }

          ListViewItem trackItem = dlgAlbumDetails.AlbumTracks.Items[i];
          track.Track = trackItem.SubItems[0].Text;
          track.Title = trackItem.SubItems[1].Text;

          main.TracksGridView.SetBackgroundColorChanged(index);
          track.Changed = true;
          main.TracksGridView.Changed = true;
          main.TracksGridView.View.Rows[index].Cells[0].Value = ServiceScope.Get<ILocalisation>().ToString("message",
                                                                                                           "Ok");
        }
      }
      dlgAlbumDetails.Dispose();
    }

    #endregion
  }
}