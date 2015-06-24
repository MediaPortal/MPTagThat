using System;
using System.Collections.Generic;
using System.Linq;
using DiscogsNet.Model;
using DiscogsNet.Model.Search;

namespace DiscogsNet.Api
{
    public class Discogs3Cache : IDiscogs3Api
    {
        private IDiscogs3Api discogs;

        private Dictionary<int, Release> releaseCache;
        private Dictionary<int, Artist> artistCache;
        private Dictionary<int, Label> labelCache;
        private Dictionary<int, MasterRelease> masterCache;
        private Dictionary<Tuple<int, PaginationRequest>, ArtistReleases> artistReleasesCache;
        private Dictionary<string, byte[]> imageCache;
        private Dictionary<Tuple<SearchQuery, PaginationRequest>, SearchResults> searchCache;

        public int RateLimitLimit { get { return this.discogs.RateLimitLimit; } }
        public int RateLimitRemaining { get { return this.discogs.RateLimitRemaining; } }
        public int RateLimitReset { get { return this.discogs.RateLimitReset; } }

        public Discogs3Cache(IDiscogs3Api source)
        {
            this.discogs = source;
            this.releaseCache = new Dictionary<int, Release>();
            this.artistCache = new Dictionary<int, Artist>();
            this.masterCache = new Dictionary<int, MasterRelease>();
            this.labelCache = new Dictionary<int, Label>();
            this.artistReleasesCache = new Dictionary<Tuple<int, PaginationRequest>, ArtistReleases>();
            this.imageCache = new Dictionary<string, byte[]>();
            this.searchCache = new Dictionary<Tuple<SearchQuery, PaginationRequest>, SearchResults>();
        }

        public Release GetRelease(int id)
        {
            if (releaseCache.ContainsKey(id))
            {
                return releaseCache[id];
            }
            return releaseCache[id] = this.discogs.GetRelease(id);
        }

        public Artist GetArtist(int id)
        {
            if (artistCache.ContainsKey(id))
            {
                return artistCache[id];
            }
            return artistCache[id] = this.discogs.GetArtist(id);
        }

        public Label GetLabel(int id)
        {
            if (labelCache.ContainsKey(id))
            {
                return labelCache[id];
            }
            return labelCache[id] = this.discogs.GetLabel(id);
        }

        public MasterRelease GetMasterRelease(int id)
        {
            if (masterCache.ContainsKey(id))
            {
                return masterCache[id];
            }
            return masterCache[id] = this.discogs.GetMasterRelease(id);
        }

        public ArtistReleases GetArtistReleases(int artistId, PaginationRequest paginationRequest = null)
        {
            Tuple<int, PaginationRequest> key = new Tuple<int, PaginationRequest>(artistId, paginationRequest);
            if (artistReleasesCache.ContainsKey(key))
            {
                return artistReleasesCache[key];
            }
            return artistReleasesCache[key] = this.discogs.GetArtistReleases(artistId, paginationRequest);
        }

        public byte[] GetImage(string url)
        {
            if (imageCache.ContainsKey(url))
            {
                return imageCache[url];
            }
            return imageCache[url] = this.discogs.GetImage(url);
        }

        public SearchResults Search(SearchQuery query, PaginationRequest paginationRequest = null)
        {
            Tuple<SearchQuery, PaginationRequest> key = new Tuple<SearchQuery, PaginationRequest>(query, paginationRequest);
            if (searchCache.ContainsKey(key))
            {
                return searchCache[key];
            }
            return searchCache[key] = this.discogs.Search(query, paginationRequest);
        }
    }
}
