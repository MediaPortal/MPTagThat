namespace DiscogsNet.Model
{
    public class Release : ReleaseBase
    {
        public int MasterId { get; set; }
        public ReleaseStatus Status { get; set; }
        public ExtraArtist[] ExtraArtists { get; set; }
        public ReleaseLabel[] Labels { get; set; }
        public ReleaseFormat[] Formats { get; set; }
        public string Country { get; set; }
        public string ReleaseDate { get; set; }
        public Companies[] Compaines { get; set; } 

        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public ReleaseIdentifier[] Identifiers { get; set; }

        public ReleaseAggregate Aggregate { get; private set; }

        public Release()
        {
            this.Aggregate = new ReleaseAggregate(this);
        }

        public override string ToString()
        {
            return this.Aggregate.JoinedArtistsFixed + " - " + this.Title;
        }
    }
}
