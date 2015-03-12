#region Copyright (C) 2009-2015 Team MediaPortal
// Copyright (C) 2009-2015 Team MediaPortal
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.Amazon;
using MPTagThat.Dialogues;
using MPTagThat.GridView;
using TagLib;

namespace MPTagThat.Commands
{
  [SupportedCommandType("GetCoverArt")]
  public class CmdGetCoverArt : ICommand, IDisposable
  {
    #region Variables

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private bool _progressCancelled = false;
    private GridViewTracks _tracksGrid;

    AmazonAlbum amazonAlbum = null;
    bool isMultipleArtistAlbum = false;
    string savedArtist = "";
    string savedAlbum = "";
    string savedFolder = "";
    MPTagThat.Core.Common.Picture folderThumb = null;
    Dictionary<string, AmazonAlbum> savedCoverCash = new Dictionary<string, AmazonAlbum>();

    #endregion

    #region ICommand Implementation

    public bool Execute(ref TrackData track, GridViewTracks tracksGrid, int rowIndex)
    {
      _tracksGrid = tracksGrid;

      Util.SendProgress(string.Format("Search coverart for {0}", track.FileName));
      log.Debug("CoverArt: Retrieving coverart for: {0} - {1}", track.Artist, track.Album);
      // Should we take an existing folder.jpg instead of searching the web
      if (Options.MainSettings.EmbedFolderThumb && !Options.MainSettings.OnlySaveFolderThumb)
      {
        if (folderThumb == null || Path.GetDirectoryName(track.FullFileName) != savedFolder)
        {
          savedFolder = Path.GetDirectoryName(track.FullFileName);
          folderThumb = Util.GetFolderThumb(savedFolder);
        }

        if (folderThumb != null)
        {
          // Only write a picture if we don't have a picture OR Overwrite Pictures is set
          if (track.Pictures.Count == 0 || Options.MainSettings.OverwriteExistingCovers)
          {
            if (Options.MainSettings.ChangeCoverSize && MPTagThat.Core.Common.Picture.ImageFromData(folderThumb.Data).Width > Options.MainSettings.MaxCoverWidth)
            {
              folderThumb.Resize(Options.MainSettings.MaxCoverWidth);
            }

            log.Debug("CoverArt: Using existing folder.jpg");
            // First Clear all the existingPictures
            track.Pictures.Clear();
            track.Pictures.Add(folderThumb);
            _tracksGrid.MainForm.SetGalleryItem();
          }
          return true;
        }
      }

      // If we don't have an Album don't do any query
      if (track.Album == "")
        return true;

      string coverSearchString = track.Artist + track.Album;
      if (isMultipleArtistAlbum)
      {
        coverSearchString = track.Album;
      }

      bool foundInCash = savedCoverCash.ContainsKey(coverSearchString);
      if (foundInCash)
      {
        amazonAlbum = savedCoverCash[coverSearchString];
      }
      else
      {
        amazonAlbum = null;
      }

      // Only retrieve the Cover Art, if we don't have it yet)
      if (!foundInCash || amazonAlbum == null)
      {
        CoverSearch dlgAlbumResults = new CoverSearch();
        dlgAlbumResults.Artist = isMultipleArtistAlbum ? "" : track.Artist;
        dlgAlbumResults.Album = track.Album;
        dlgAlbumResults.FileDetails = track.FullFileName;
        dlgAlbumResults.Owner = _tracksGrid.MainForm;
        dlgAlbumResults.StartPosition = FormStartPosition.CenterParent;

        amazonAlbum = null;
        DialogResult dlgResult = dlgAlbumResults.ShowDialog();
        if (dlgResult == DialogResult.OK)
        {
          if (dlgAlbumResults.SelectedAlbum != null)
          {
            amazonAlbum = dlgAlbumResults.SelectedAlbum;
          }
        }
        else if (dlgResult == DialogResult.Abort)
        {
          log.Debug("CoverArt: Search for all albums cancelled");
          return true;
        }
        else
        {
          log.Debug("CoverArt: Album Selection cancelled");
          return true;
        }
        dlgAlbumResults.Dispose();
      }

      // Now update the Cover Art
      if (amazonAlbum != null)
      {
        if (!savedCoverCash.ContainsKey(coverSearchString))
        {
          savedCoverCash.Add(coverSearchString, amazonAlbum);
        }

        // Only write a picture if we don't have a picture OR Overwrite Pictures is set);
        if ((track.Pictures.Count == 0 || Options.MainSettings.OverwriteExistingCovers) && !Options.MainSettings.OnlySaveFolderThumb)
        {
          track.Pictures.Clear();

          ByteVector vector = amazonAlbum.AlbumImage;
          if (vector != null)
          {
            MPTagThat.Core.Common.Picture pic = new MPTagThat.Core.Common.Picture();
            pic.MimeType = "image/jpg";
            pic.Description = "Front Cover";
            pic.Type = PictureType.FrontCover;
            pic.Data = vector.Data;

            if (Options.MainSettings.ChangeCoverSize && Core.Common.Picture.ImageFromData(pic.Data).Width > Options.MainSettings.MaxCoverWidth)
            {
              pic.Resize(Options.MainSettings.MaxCoverWidth);
            }

            track.Pictures.Add(pic);
          }

          // And also set the Year from the Release Date delivered by Amazon
          // only if not present in Track
          if (amazonAlbum.Year != null)
          {
            string strYear = amazonAlbum.Year;
            if (strYear.Length > 4)
              strYear = strYear.Substring(0, 4);

            int year = 0;
            try
            {
              year = Convert.ToInt32(strYear);
            }
            catch (Exception) { }
            if (year > 0 && track.Year == 0)
              track.Year = year;
          }

          tracksGrid.MainForm.SetGalleryItem();
        }
      }

      // If the user has selected to store only the folder thumb, without touching the file 
      if (amazonAlbum != null && Options.MainSettings.OnlySaveFolderThumb)
      {
        ByteVector vector = amazonAlbum.AlbumImage;
        if (vector != null)
        {
          string fileName = Path.Combine(Path.GetDirectoryName(track.FullFileName), "folder.jpg");
          try
          {
            Core.Common.Picture pic = new Core.Common.Picture();
            if (Options.MainSettings.ChangeCoverSize && Core.Common.Picture.ImageFromData(pic.Data).Width > Options.MainSettings.MaxCoverWidth)
            {
              pic.Resize(Options.MainSettings.MaxCoverWidth);
            }

            Image img = Core.Common.Picture.ImageFromData(vector.Data);

            // Need to make a copy, otherwise we have a GDI+ Error
            Bitmap bmp = new Bitmap(img);
            bmp.Save(fileName, ImageFormat.Jpeg);

            FileInfo fi = new FileInfo(fileName);
            _tracksGrid.NonMusicFiles.RemoveAll(f => f.Name == fi.Name);
            _tracksGrid.NonMusicFiles.Add(fi);
            _tracksGrid.MainForm.MiscInfoPanel.AddNonMusicFiles(_tracksGrid.NonMusicFiles);
          }
          catch (Exception ex)
          {
            log.Error("Exception Saving picture: {0} {1}", fileName, ex.Message);
          }
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Indicate, whether we need Preprocess the tracks
    /// </summary>
    /// <returns></returns>
    public bool NeedsPreprocessing()
    {
      return true;
    }

    /// <summary>
    /// Do Preprocessing of the Tracks
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public bool PreProcess(TrackData track)
    {
      if (savedArtist == "")
      {
        savedArtist = track.Artist;
        savedAlbum = track.Album;
      }
      if (savedArtist != track.Artist)
      {
        isMultipleArtistAlbum = true;
      }
      if (savedAlbum != track.Album)
      {
        isMultipleArtistAlbum = false;
      }
      return true;
    }

    /// <summary>
    /// Post Process after command execution
    /// </summary>
    /// <param name="tracksGrid"></param>
    /// <returns></returns>
    public bool PostProcess(GridViewTracks tracksGrid)
    {
      return false;
    }

    /// <summary>
    /// Set indicator, that Command processing got interupted by user
    /// </summary>
    public void CancelCommand()
    {
      _progressCancelled = true;
    }

    /// <summary>
    /// Cleanup resources
    /// </summary>
    public void Dispose()
    {

    }

    #endregion
  }
}