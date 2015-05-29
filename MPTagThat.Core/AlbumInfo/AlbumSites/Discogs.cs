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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using DiscogsNet.Api;
using DiscogsNet.Model;
using DiscogsNet.Model.Search;

namespace MPTagThat.Core.AlbumInfo.AlbumSites
{
  public class Discogs : AbstractAlbumSite
  {
    #region Variables

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
		private Discogs3 _discogs;

    #endregion

    #region Properties

    public override string SiteName
    {
      get { return "Discogs"; }
    }

    public override bool SiteActive()
    {
      return true;
    }

    #endregion

    #region ctor

    public Discogs(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
    {
			_discogs = new Discogs3("MPTagThat/3.2 +http://www.team-mediaportal.com");
    }

    #endregion

    #region Methods

    protected override void GetAlbumInfoWithTimer()
    {
      log.Debug("Discogs: Looking up Album on Discogs");
			Albums.Clear();
	    try
	    {
				var query = new SearchQuery { Artist = ArtistName, ReleaseTitle = AlbumName, Type = SearchItemType.Master };
				var searchresults = _discogs.Search(query);

				// Look for the Master Release only
				foreach (var result in searchresults.Results)
				{
					if (result.Type == SearchItemType.Master)
					{
						var album = GetRelease(result.Id);
						Albums.Add(album);
					}
				}
				log.Debug("Discogs: Found {0} albums", Albums.Count);
	    }
	    catch (Exception ex)
	    {
				log.Debug("Discogs: Exception receiving Album Information. {0} {1}", ex.Message, ex.StackTrace);
	    }
    }

    private Album GetRelease(int releaseid)
    {
	    var release = _discogs.GetMasterRelease(releaseid);
	    var album = new Album();
	    album.LargeImageUrl = release.Aggregate.PrimaryImage.Uri;
	    album.Artist = release.Aggregate.JoinedArtists;
	    album.CoverHeight = release.Aggregate.PrimaryImage.Height.ToString();
			album.CoverWidth = release.Aggregate.PrimaryImage.Width.ToString();
	    album.Title = release.Title;
	    album.Year = release.Year.ToString();
	    album.DiscCount = 1;

			// Get the Tracks
			var discs = new List<List<AlbumTrack>>();
	    var albumTracks = new List<AlbumTrack>();
	    var numDiscs = 1;

	    foreach (var track in release.Tracklist)
	    {
		    var pos = track.Position;
		    var albumTrack = new AlbumTrack();
		    
		    if (track.Position.Contains("-"))
		    {
			    album.DiscCount = Convert.ToInt16(track.Position.Substring(0, track.Position.IndexOf("-", StringComparison.Ordinal)));
					// Has the number of Discs changed?
			    if (album.DiscCount != numDiscs)
			    {
				    numDiscs = album.DiscCount;
						discs.Add(new List<AlbumTrack>(albumTracks));
						albumTracks.Clear();
			    }
					pos = track.Position.Substring(track.Position.IndexOf("-", StringComparison.Ordinal) + 1);
		    }
		    albumTrack.Number = Convert.ToInt16(pos);
				albumTrack.Title = track.Title;
				albumTracks.Add(albumTrack);
	    }
			discs.Add(albumTracks);
	    album.Discs = discs;

	    return album;
    }

    #endregion

  }
}
