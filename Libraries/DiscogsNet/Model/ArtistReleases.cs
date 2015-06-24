using System;
using System.Linq;

namespace DiscogsNet.Model
{
    public class ArtistReleases
    {
        public PaginationInfo Pagination { get; set; }
        public ReleaseVersion[] Releases { get; set; }
    }
}
