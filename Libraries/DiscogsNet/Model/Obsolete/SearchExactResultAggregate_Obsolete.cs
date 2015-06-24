using System;

namespace DiscogsNet.Model.Obsolete
{
    public class SearchExactResultAggregate_Obsolete
    {
        private SearchExactResult_Obsolete searchResult;

        /// <summary>
        /// This property tries to parse the artist name from the Uri. Only valid for artist results.
        /// Returns null if the URI can't be parsed.
        /// </summary>
        public string ArtistName
        {
            get
            {
                if (searchResult.Type != SearchResultType_Obsolete.Artist)
                {
                    throw new InvalidOperationException("ArtistName property is only valid for artist results.");
                }

                return DiscogsUriParser.ParseArtistNameFromUri(searchResult.Uri);
            }
        }

        /// <summary>
        /// This property tries to parse the artist name from the Uri. Only valid for artist results.
        /// Returns null if the URI can't be parsed.
        /// </summary>
        public string ArtistNameFixed
        {
            get
            {
                string artistName = this.ArtistName;

                if (artistName == null)
                {
                    return "";
                }
                return ArtistAggregate.FixName(artistName);
            }
        }

        /// <summary>
        /// This property tries to parse the release ID from the Uri. Only valid for release results.
        /// Returns 0 if the URI can't be parsed.
        /// </summary>
        public int ReleaseId
        {
            get
            {
                if (searchResult.Type != SearchResultType_Obsolete.Release)
                {
                    throw new InvalidOperationException("ReleaseId property is only valid for release results.");
                }

                return DiscogsUriParser.ParseReleaseIdFromUri(searchResult.Uri);
            }
        }

        public SearchExactResultAggregate_Obsolete(SearchExactResult_Obsolete searchResult)
        {
            this.searchResult = searchResult;
        }
    }
}
