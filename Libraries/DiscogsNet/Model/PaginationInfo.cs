using System;
using System.Linq;

namespace DiscogsNet.Model
{
    public class PaginationInfo
    {
        public int PerPage { get; set; }
        public int Pages { get; set; }
        public int Page { get; set; }
        public int Items { get; set; }
    }
}
