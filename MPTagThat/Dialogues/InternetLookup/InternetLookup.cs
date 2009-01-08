using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MPTagThat.Core;
using MPTagThat.Core.Amazon;

namespace MPTagThat.InternetLookup
{
  public class InternetLookup
  {
    #region Variables
    private Main main;
    private Thread _asyncThread = null;

    private string _selectedArtist = "";
    private string _selectedAlbum = "";

    private List<ListViewItem> _fileList = null;

    // Dialogs
    private ArtistAlbumDialog dlg = null;
    private AlbumSearchResult dlgSearchResult = null;
    private AlbumDetails dlgAlbumDetails = null;
    #endregion

    #region ctor
    public InternetLookup(Main main)
    {
      this.main = main;
    }
    #endregion

    #region Methods
    public void SearchForAlbumInformation()
    {
      if (_asyncThread == null)
      {
        // Do the init outside the thread, so that we don't get Crossthread errors
        dlg = new ArtistAlbumDialog();
        dlgSearchResult = new AlbumSearchResult();
        dlgAlbumDetails = new AlbumDetails();
        _asyncThread = new Thread(new ThreadStart(SearchForAlbumInformationThread));
        _asyncThread.Name = "InternetLookup";
      }

      if (_asyncThread.ThreadState != ThreadState.Running)
      {
        _asyncThread = new Thread(new ThreadStart(SearchForAlbumInformationThread));
        _asyncThread.Start();
      }
    }

    private void SearchForAlbumInformationThread()
    {
      DataGridView tracksGrid = main.TracksGridView.View;

      // Loop through the selected rows and see, if we got an Artist and/or Album set
      // Need at least an album
      foreach (DataGridViewRow row in tracksGrid.Rows)
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

      // If no Album was specified, we need to show the select dialog
      if (_selectedAlbum == "")
      {
        dlg.Artist = _selectedArtist;
        dlg.Album = _selectedAlbum;

        if (main.ShowForm(dlg) != DialogResult.OK)
        {
          dlg.Dispose();
          return;
        }

        _selectedArtist = dlg.Artist;
        _selectedAlbum = dlg.Album;
      }

      // Query Amazon for the Album
      List<AmazonAlbum> albums = new List<AmazonAlbum>();
      using (AmazonAlbumInfo amazonInfo = new AmazonAlbumInfo())
      {
        albums = amazonInfo.AmazonAlbumSearch(_selectedArtist, _selectedAlbum);
      }

      // Show the Album Selection dialog, if we got more than one album
      AmazonAlbum amazonAlbum = null;
      if (albums.Count > 0)
      {
        if (albums.Count == 1)
        {
          amazonAlbum = albums[0];
        }
        else
        {
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
          if (main.ShowForm(dlgSearchResult) == DialogResult.OK)
          {
            if (dlgSearchResult.ResultView.SelectedIndices[0] > -1)
              amazonAlbum = albums[dlgSearchResult.ResultView.SelectedIndices[0]];
            else
              amazonAlbum = albums[0];
          }
          else
          {
            dlgSearchResult.Dispose();
            return;
          }
          dlgSearchResult.Dispose();
        }
      }

      // It may happen that an album doesn't return Track Information
      // Inform the uer to make a new selection
      if (amazonAlbum == null || amazonAlbum.Discs.Count == 0)
      {
        MessageBox.Show(ServiceScope.Get<ILocalisation>().ToString("Lookup", "NoAlbumFound"), ServiceScope.Get<ILocalisation>().ToString("message", "Error"), MessageBoxButtons.OK);
        dlg.Dispose();
        dlgAlbumDetails.Dispose();
        return;
      }

      // Prepare the Details Dialog
      dlgAlbumDetails.Artist = amazonAlbum.Artist;
      dlgAlbumDetails.Album = amazonAlbum.Title;
      dlgAlbumDetails.Year = amazonAlbum.Year;

      try
      {
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(amazonAlbum.AlbumImage.Data))
        {
          System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
          if (img != null)
          {
            dlgAlbumDetails.Cover.Image = img;
          }
        }
      }
      catch { }

      // Add Tracks of the selected album
      foreach (List<AmazonAlbumTrack> disc in amazonAlbum.Discs)
      {
        foreach (AmazonAlbumTrack track in disc)
        {
          ListViewItem lvItem = new ListViewItem(track.Number.ToString());
          lvItem.SubItems.Add(track.Title);
          dlgAlbumDetails.AlbumTracks.Items.Add(lvItem);
        }
      }

      // Add selected Files from Grid
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = main.TracksGridView.TrackList[row.Index];
        ListViewItem lvItem = new ListViewItem(track.FileName);
        lvItem.Tag = row.Index;
        dlgAlbumDetails.DiscTracks.Items.Add(lvItem);
      }

      // if we have less files selected, than in the album, fill the rest with"unassigned"
      if (dlgAlbumDetails.DiscTracks.Items.Count < dlgAlbumDetails.AlbumTracks.Items.Count)
      {
        for (int i = dlgAlbumDetails.DiscTracks.Items.Count - 1; i < dlgAlbumDetails.AlbumTracks.Items.Count; i++)
        {
          ListViewItem unassignedItem = new ListViewItem(ServiceScope.Get<ILocalisation>().ToString("Lookup", "Unassigned"));
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
          if (Util.LongestCommonSubstring(lvAlbumItem.SubItems[1].Text, lvDiscItem.SubItems[0].Text) > 0.75)
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

      if (dlgAlbumDetails.ShowDialog() == DialogResult.OK)
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
          catch (Exception)
          { }
          if (year > 0 && track.Year == 0)
            track.Year = year;

          // Add the picture
          TagLib.ByteVector vector = amazonAlbum.AlbumImage;
          if (vector != null)
          {
            // Get the availbe Covers
            List<TagLib.IPicture> pics = new List<TagLib.IPicture>();
            pics = new List<TagLib.IPicture>(track.Pictures);
            TagLib.Picture pic = new TagLib.Picture(vector);
            pics.Add(pic);
            track.Pictures = pics.ToArray();
          }

          ListViewItem trackItem = dlgAlbumDetails.AlbumTracks.Items[i];
          track.Track = trackItem.SubItems[0].Text;
          track.Title = trackItem.SubItems[1].Text;

          main.TracksGridView.SetBackgroundColorChanged(index);
          track.Changed = true;
          main.TracksGridView.View.Rows[index].Cells[1].Value = ServiceScope.Get<ILocalisation>().ToString("message", "Ok");
        }
      }
      dlgAlbumDetails.Dispose();
    }
    #endregion
  }
}
