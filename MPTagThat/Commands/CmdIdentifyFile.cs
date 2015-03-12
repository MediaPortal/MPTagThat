﻿#region Copyright (C) 2009-2015 Team MediaPortal
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
using MPTagThat.GridView;
using TagLib;

namespace MPTagThat.Commands
{
  [SupportedCommandType("IdentifyFiles")]
  public class CmdIdentifyFile : ICommand, IDisposable
  {
    #region Variables

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private bool _progressCancelled = false;
    private GridViewTracks _tracksGrid;

    MusicBrainzAlbum _musicBrainzAlbum = new MusicBrainzAlbum();

    #endregion

    #region ICommand Implementation

    /// <summary>
    /// Lookup the file in Music Brainz with the Fingerprint
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public bool Execute(ref TrackData track, GridViewTracks tracksGrid)
    {
      _tracksGrid = tracksGrid;

      using (MusicBrainzTrackInfo trackinfo = new MusicBrainzTrackInfo())
      {
        Util.SendProgress(string.Format("Identifying file {0}", track.FileName));
        log.Debug("Identify: Processing file: {0}", track.FullFileName);
        List<MusicBrainzTrack> musicBrainzTracks = trackinfo.GetMusicBrainzTrack(track.FullFileName);

        if (musicBrainzTracks == null)
        {
          log.Debug("Identify: Couldn't identify file");
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
                _musicBrainzAlbum = albumInfo.GetMusicBrainzAlbumById(musicBrainzTrack.AlbumId);
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
              dlgAlbumResults.Owner = _tracksGrid.MainForm;
              dlgAlbumResults.StartPosition = FormStartPosition.CenterParent;
              if (dlgAlbumResults.ShowDialog() == DialogResult.OK)
              {
                var itemTag = dlgAlbumResults.SelectedListItem as Dictionary<string, MusicBrainzTrack>;
                foreach (var albumId in itemTag.Keys)
                {
                  itemTag.TryGetValue(albumId, out musicBrainzTrack);
                  musicBrainzTrack.AlbumId = albumId;
                }

              }
              dlgAlbumResults.Dispose();
            }
          }

          // We didn't get a track
          if (musicBrainzTrack == null)
          {
            log.Debug("Identify: No information returned from Musicbrainz");
            return false;
          }

          // Are we still at the same album?
          // if not, get the album, so that we have the release date
          if (_musicBrainzAlbum.Id != musicBrainzTrack.AlbumId)
          {
            using (var albumInfo = new MusicBrainzAlbumInfo())
            {
              Application.DoEvents();
              if (_progressCancelled)
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
                var pic = new MPTagThat.Core.Common.Picture();
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

    /// <summary>
    /// Indicate, whether we need Preprocess the tracks
    /// </summary>
    /// <returns></returns>
    public bool NeedsPreprocessing()
    {
      return false;
    }

    /// <summary>
    /// Do Preprocessing of the Tracks
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public bool PreProcess(TrackData track)
    {
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