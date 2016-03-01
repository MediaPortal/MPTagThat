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
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.MusicBrainz;
using TagLib;

namespace MPTagThat.Commands
{
  [SupportedCommandType("IdentifyFiles")]
  public class CmdIdentifyFile : Command
  {
    public object[] Parameters { get; private set; }

    #region Variables

    MusicBrainzAlbum _musicBrainzAlbum = new MusicBrainzAlbum();

    #endregion

    #region ctor

    public CmdIdentifyFile(object[] parameters)
    {
      Parameters = parameters;
    }

    #endregion

    #region Command Implementation

    /// <summary>
    /// Lookup the file in Music Brainz with the Fingerprint
    /// </summary>
    /// <param name="track"></param>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    public override bool Execute(ref TrackData track, int rowIndex)
    {
      using (MusicBrainzTrackInfo trackinfo = new MusicBrainzTrackInfo())
      {
        Util.SendProgress(string.Format("Identifying file {0}", track.FileName));
        Log.Debug("Identify: Processing file: {0}", track.FullFileName);
        List<MusicBrainzTrack> musicBrainzTracks = trackinfo.GetMusicBrainzTrack(track.FullFileName);

        if (musicBrainzTracks == null)
        {
          Log.Debug("Identify: Couldn't identify file");
          return false;
        }

        if (musicBrainzTracks.Count > 0)
        {
          MusicBrainzTrack musicBrainzTrack = null;
          if (musicBrainzTracks.Count == 1 && musicBrainzTracks[0].Releases.Count == 1)
          {
            musicBrainzTrack = musicBrainzTracks[0];
            // Have we got already this album
            if (musicBrainzTrack.AlbumId == null || musicBrainzTrack.AlbumId != _musicBrainzAlbum.Id)
            {
              using (var albumInfo = new MusicBrainzAlbumInfo())
              {
                _musicBrainzAlbum = albumInfo.GetMusicBrainzAlbumById(musicBrainzTrack.Releases[0].AlbumId);
              }
            }
            musicBrainzTrack.AlbumId = _musicBrainzAlbum.Id;
          }
          else
          {
            // Skip the Album selection, if the album been selected already for a previous track
            bool albumFound = false;
            foreach (var mbtrack in musicBrainzTracks)
            {
              foreach (var mbRelease in mbtrack.Releases)
              {
                if (mbRelease.AlbumId == _musicBrainzAlbum.Id)
                {
                  albumFound = true;
                  musicBrainzTrack = mbtrack;
                  musicBrainzTrack.AlbumId = mbRelease.AlbumId;
                  break;
                }
              }
            }

            if (!albumFound)
            {
              var dlgAlbumResults = new MusicBrainzAlbumResults(musicBrainzTracks);
              dlgAlbumResults.Owner = TracksGrid.MainForm;
              dlgAlbumResults.StartPosition = FormStartPosition.CenterParent;
              if (dlgAlbumResults.ShowDialog() == DialogResult.OK)
              {
                var itemTag = dlgAlbumResults.SelectedListItem as Dictionary<string, MusicBrainzTrack>;
                if (itemTag != null)
                  foreach (var albumId in itemTag.Keys)
                  {
                    itemTag.TryGetValue(albumId, out musicBrainzTrack);
                    if (musicBrainzTrack != null) musicBrainzTrack.AlbumId = albumId;
                  }
              }
              dlgAlbumResults.Dispose();
            }
          }

          // We didn't get a track
          if (musicBrainzTrack == null)
          {
            Log.Debug("Identify: No information returned from Musicbrainz");
            return false;
          }

          // Are we still at the same album?
          // if not, get the album, so that we have the release date
          if (_musicBrainzAlbum.Id != musicBrainzTrack.AlbumId)
          {
            using (var albumInfo = new MusicBrainzAlbumInfo())
            {
              Application.DoEvents();
              if (ProgressCancelled)
              {
                return false;
              }
              _musicBrainzAlbum = albumInfo.GetMusicBrainzAlbumById(musicBrainzTrack.AlbumId);
            }
          }

          track.Title = musicBrainzTrack.Title;
          track.Artist = musicBrainzTrack.Artist;
          track.Album = _musicBrainzAlbum.Title;
          track.AlbumArtist = _musicBrainzAlbum.Artist;

          // Get the Disic and Track# from the Album
          foreach (var mbTrack in _musicBrainzAlbum.Tracks)
          {
            if (mbTrack.Id == musicBrainzTrack.Id)
            {
              track.TrackNumber = Convert.ToUInt32(mbTrack.Number);
              track.TrackCount = Convert.ToUInt32(mbTrack.TrackCount);
              track.DiscNumber = Convert.ToUInt32(mbTrack.DiscId);
              track.DiscCount = Convert.ToUInt32(_musicBrainzAlbum.DiscCount);
              break;
            }
          }

          if (_musicBrainzAlbum.Year != null && _musicBrainzAlbum.Year.Length >= 4)
            track.Year = Convert.ToInt32(_musicBrainzAlbum.Year.Substring(0, 4));

          // Do we have a valid Amazon Album?
          if (_musicBrainzAlbum.Amazon != null)
          {
            // Only write a picture if we don't have a picture OR Overwrite Pictures is set
            if (track.Pictures.Count == 0 || Options.MainSettings.OverwriteExistingCovers)
            {
              var vector = _musicBrainzAlbum.Amazon.AlbumImage;
              if (vector != null)
              {
                var pic = new Core.Common.Picture();
                pic.MimeType = "image/jpg";
                pic.Description = "";
                pic.Type = PictureType.FrontCover;
                pic.Data = vector.Data;
                track.Pictures.Add(pic);
              }
            }
          }

          return true;
        }
      }
      return false;
    }

    #endregion
  }
}