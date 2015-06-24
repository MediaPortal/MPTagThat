using System;
using System.Linq;

namespace DiscogsNet.Model
{
    public class ReleaseVersion
    {
        public int Id { get; set; }
        public ReleaseVersionType Type { get; set; }
        public ReleaseStatus Status { get; set; }
        public string Title { get; set; }
        public string Format { get; set; }
        public string Label { get; set; }
        public string CatalogNumber { get; set; }
        public string Country { get; set; }
        public string ReleaseDate { get; set; }
        public string Thumb { get; set; }
        public string TrackInfo { get; set; }
        public int MainRelease { get; set; }
        public string Role { get; set; }
        public int Year { get; set; }
    }
}
