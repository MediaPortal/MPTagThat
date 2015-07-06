using System;
using System.Linq;

namespace DiscogsNet.Model.Search
{
    public class ReleaseBaseSearchResult : SearchResult
    {
        public string[] Styles { get; set; }
        public string[] Genres { get; set; }
        public int Year { get; set; }
    }
}
