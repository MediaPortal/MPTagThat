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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MediaPortal.LastFM;


namespace MPTagThat.Core.AlbumInfo.AlbumSites
{
	public class LastFM : AbstractAlbumSite
	{
		#region Variables

		private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

		#endregion

		#region Properties

		public override string SiteName
		{
			get { return "LastFM"; }
		}

		public override bool SiteActive()
		{
			return true;
		}

		#endregion

		#region ctor

		public LastFM(string artist, string album, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, album, mEventStopSiteSearches, timeLimit)
		{
		}

		#endregion

		#region Methods

		protected override void GetAlbumInfoWithTimer()
		{
			log.Debug("LastFM: Looking up Album on LastFM");
			Albums.Clear();
			try
			{
				var lastfmAlbum = LastFMLibrary.GetAlbumInfo(SwitchArtist(ArtistName), AlbumName);

				var album = new Album();
				album.Artist = lastfmAlbum.ArtistName;
				album.Title = lastfmAlbum.AlbumName;
				album.SmallImageUrl = lastfmAlbum.Images.First(image => image.ImageSize == LastFMImage.LastFMImageSize.Small).ImageURL;
				album.MediumImageUrl = lastfmAlbum.Images.First(image => image.ImageSize == LastFMImage.LastFMImageSize.Large).ImageURL;
				album.LargeImageUrl = lastfmAlbum.Images.First(image => image.ImageSize == LastFMImage.LastFMImageSize.Mega).ImageURL;

				var discs = new List<List<AlbumTrack>>();	
				var albumTracks = new List<AlbumTrack>();
				var i = 0;
				foreach (var track in lastfmAlbum.Tracks)
				{
					i++;
					AlbumTrack albumtrack = new AlbumTrack();
					albumtrack.Title = track.TrackTitle;
					albumtrack.Duration = track.Duration.ToString();
					albumtrack.Number = i;
					albumTracks.Add(albumtrack);
				}
				discs.Add(albumTracks);
				album.Discs = discs;
				Albums.Add(album);
				log.Debug("LastFM: Found {0} albums", Albums.Count);
			}
			catch (Exception ex)
			{
				log.Debug("LastFM: Exception receiving Album Information. {0} {1}", ex.Message, ex.StackTrace);
			}
		}

		private string SwitchArtist(string artist)
		{
			int iPos = artist.IndexOf(',');
			if (iPos > 0)
			{
				artist = String.Format("{0} {1}", artist.Substring(iPos + 2), artist.Substring(0, iPos));
			}
			return artist;
		}

		#endregion
	}
}
