using System;
using System.Collections;
using System.Text;

namespace LyricsEngine
{
    public static class Setup
    {
        public static string[] BatchSearchSites = new string[8]
        {
            "LrcFinder",
            "LyricWiki",
            "Lyrics007",
            "LyricsOnDemand",
            "HotLyrics",
            "Actionext",
            "LyrDB",
            "LyricsPluginSite"
        };


        public static string[] AllSites()
        {
            ArrayList allSites = new ArrayList();
            allSites.AddRange(BatchSearchSites);
            string[] allSitesArray = (string[])allSites.ToArray(typeof(string));
            return allSitesArray;
        }

        public static bool IsMember(string value)
        {
            return System.Array.IndexOf<string>(Setup.BatchSearchSites, value) != -1;
        }

        public static int NoOfSites()
        {
            return BatchSearchSites.Length;
        }
    }
}
