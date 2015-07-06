using System;
using System.Linq;

namespace DiscogsNet.Model.Search
{
    public class SearchResults
    {
        public PaginationInfo Pagination { get; set; }
        public SearchResult[] Results { get; set; }
    }
}
