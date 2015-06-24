using DiscogsNet.Model;
using DiscogsNet.Model.Search;

namespace DiscogsNet
{
    public interface IDiscogs3Api
    {
        /// <summary>
        /// This stores the X-RateLimit-Limit header returned by the last request.
        /// </summary>
        int RateLimitLimit { get; }

        /// <summary>
        /// This stores the X-RateLimit-Remaining header returned by the last request.
        /// </summary>
        int RateLimitRemaining { get; }

        /// <summary>
        /// This stores the X-RateLimit-Reset header returned by the last request.
        /// </summary>
        int RateLimitReset { get; }

        Release GetRelease(int id);

        Artist GetArtist(int id);

        Label GetLabel(int id);

        MasterRelease GetMasterRelease(int id);

        ArtistReleases GetArtistReleases(int artistId, PaginationRequest paginationRequest = null);

        byte[] GetImage(string url);

        SearchResults Search(SearchQuery query, PaginationRequest paginationRequest = null);
    }
}