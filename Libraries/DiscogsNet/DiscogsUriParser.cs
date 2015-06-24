using System.Text.RegularExpressions;
using System;

namespace DiscogsNet
{
    public class DiscogsUriParser
    {
        public static int ParseReleaseIdFromUri(string uri)
        {
            SimpleRegex regex = new SimpleRegex("^http://(www\\.)?discogs\\.com/.*release/(?<releaseId>[1-9][0-9]+?)(\\?.*)?$");
            GroupCollection collection = regex.Match(uri);
            if (collection == null)
            {
                return 0;
            }
            return int.Parse(collection["releaseId"].Value);
        }

        public static string ParseArtistNameFromUri(string uri)
        {
            SimpleRegex regex = new SimpleRegex("^http://(www\\.)?discogs\\.com/artist/(?<artistName>.+?)(\\?.*)?$");
            GroupCollection collection = regex.Match(uri);
            if (collection == null)
            {
                return null;
            }

            string artistName = collection["artistName"].Value.Replace('+', ' ');
            return Uri.UnescapeDataString(artistName);
        }
    }
}
