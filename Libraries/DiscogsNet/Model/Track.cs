namespace DiscogsNet.Model
{
    public class Track
    {
        public string Position { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public ReleaseArtist[] Artists { get; set; }
        public ExtraArtist[] ExtraArtists { get; set; }

        public TrackAggregate Aggregate { get; set; }

        public Track()
        {
            this.Aggregate = new TrackAggregate(this);

            this.Position = "";
            this.Title = "";
            this.Duration = "";
        }
    }
}
