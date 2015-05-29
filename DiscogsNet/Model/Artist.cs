using System;
namespace DiscogsNet.Model
{
    public class Artist
    {
        public int Id { get; set; }
        public Image[] Images { get; set; }
        public string Name  { get; set; }
        public string RealName { get; set; }
        public string[] Urls { get; set; }
        public string[] NameVariations { get; set; }
        public ArtistAlias[] Aliases { get; set; }
        public Members[] Members { get; set; }
        public string[] Groups { get; set; }
        public string Profile { get; set; }

        [Obsolete("The new JSON API doesn't return this. Instead, users should make another request to the API.")]
        public ArtistRelease[] Releases { get; set; }

        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public DataQuality DataQuality { get; set; }

        public ArtistAggregate Aggregate { get; private set; }

        public Artist()
        {
            this.Aggregate = new ArtistAggregate(this);

            this.Name = "";
            this.RealName = "";
            this.Profile = "";
        }

        public override string ToString()
        {
            return this.Aggregate.NameFixed;
        }
    }
}
