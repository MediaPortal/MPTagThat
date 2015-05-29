using System;
using System.Linq;

namespace DiscogsNet.Model.Search
{
    public class ReleaseSearchResult : ReleaseBaseSearchResult
    {
        public string Country { get; set; }
        public string[] Label { get; set; }
        public string CatalogNumber { get; set; }
        public string[] Formats { get; set; }
    }
}
