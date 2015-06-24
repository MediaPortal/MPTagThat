using System;
using System.Linq;

namespace DiscogsNet.Model.Search
{
    public class SearchResult
    {
        public int Id { get; set; }
        public string Thumb { get; set; }
        public SearchItemType Type { get; set; }
        public string Title { get; set; }
    }
}
