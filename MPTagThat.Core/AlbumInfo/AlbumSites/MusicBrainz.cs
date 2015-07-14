using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Hqub.MusicBrainz.API;
using Hqub.MusicBrainz.API.Entities;

namespace MPTagThat.Core.AlbumInfo.AlbumSites
{
	class MusicBrainz : AbstractAlbumSite
	{
		#region Variables

		private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
		private Regex _switchedArtist = new Regex(@"^.*, .*$");

		#endregion

		#region Properties

		public override string SiteName
		{
			get { return "MusicBrainz"; }
		}

		public override bool SiteActive()
		{
			return true;
		}

		#endregion

		#region ctor

		public MusicBrainz(string artist, string album, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, album, mEventStopSiteSearches, timeLimit)
		{
		}

		#endregion

		#region Methods

		protected override void GetAlbumInfoWithTimer()
		{
			log.Debug("MusicBrainz: Looking up Album on MusicBrainz");
			Albums.Clear();
			try
			{
				GetAlbumQuery(ArtistName, AlbumName);
				log.Debug("MusicBrainz: Found {0} albums", Albums.Count);
			}
			catch (Exception ex)
			{
				log.Debug("MusicBrainz: Exception receiving Album Information. {0} {1}", ex.Message, ex.StackTrace);
			}
		}

		private async void GetAlbumQuery(string artistName, string albumName)
		{
			// If we have an artist in form "LastName, FirstName" change it to "FirstName LastName" to have both results
			var artistNameOriginal = _switchedArtist.IsMatch(artistName) ? string.Format(" OR {0}",SwitchArtist(artistName)) : "";

			var query = new QueryParameters<Release>();
			query.Add("artist", string.Format("{0} {1}", artistName, artistNameOriginal));
			query.Add("release", albumName);
			var albums = await Release.SearchAsync(query);

			// First look for Albums from the selected country in AmazonSites
			var mbAlbum = albums.Items.FirstOrDefault(r => (r.Title != null && r.Title.ToLower() == albumName.ToLower()) && (r.Country != null && r.Country.ToLower() == Options.MainSettings.AmazonSite.ToLower()));
			if (mbAlbum == null)
			{
				// Look for European wide release
				mbAlbum = albums.Items.FirstOrDefault(r => (r.Title != null && r.Title.ToLower() == albumName.ToLower()) && (r.Country != null && r.Country.ToLower() == "xe"));
				if (mbAlbum == null)
				{
					// Look for US release
					mbAlbum = albums.Items.FirstOrDefault(r => (r.Title != null && r.Title.ToLower() == albumName.ToLower()) && (r.Country != null && r.Country.ToLower() == "us"));
					if (mbAlbum == null)
					{
						mbAlbum = albums.Items.Count > 0 ? albums.Items[0] : null;
						if (mbAlbum == null)
						{
							return;
						}
					}
				}
			}

			var release = await Release.GetAsync(mbAlbum.Id, new[] { "recordings", "media", "artists", "discids" });
			
			var album = new Album();
			album.LargeImageUrl = release.CoverArtArchive != null && release.CoverArtArchive.Front
				? string.Format(@"http://coverartarchive.org/release/{0}/front.jpg", release.Id)
				: "";
			album.CoverHeight = "0";
			album.CoverWidth = "0";
			album.Artist = JoinArtists(release.Credits);
			album.Title = release.Title;
			album.Year = release.Date;
			album.DiscCount = release.MediumList.Items.Count;

			// Get the Tracks
			var discs = new List<List<AlbumTrack>>();	
			foreach (var medium in release.MediumList.Items)
			{
				var albumTracks = new List<AlbumTrack>();
				foreach (var track in medium.Tracks.Items)
				{
					AlbumTrack albumtrack = new AlbumTrack();
					albumtrack.Number = track.Position;
					TimeSpan duration = TimeSpan.FromMilliseconds(track.Recording.Length);
					albumtrack.Duration = string.Format("{0:mm\\:ss}", duration);
					albumtrack.Title = track.Recording.Title;
					albumTracks.Add(albumtrack);
				}
				discs.Add(albumTracks);
			}
			album.Discs = discs;
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

		private string JoinArtists(List<NameCredit> credits)
		{
			var joinedArtist = "";
			var firstElement = true;

			foreach (var credit in credits)
			{
				if (!firstElement)
				{
					joinedArtist += string.Format("; {0}", credit.Artist.Name);
				}
				else
				{
					joinedArtist = credit.Artist.Name;
					firstElement = false;
				}
			}

			return joinedArtist;
		}

		#endregion
	}
}
