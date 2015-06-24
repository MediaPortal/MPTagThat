namespace DiscogsNet.Model
{
    public class ReleaseArtist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameVariation { get; set; }
        public string Join { get; set; }
        public string Role { get; set; }
        public string Tracks { get; set; }

        public ReleaseArtistAggregate Aggregate { get; private set; }

        public ReleaseArtist()
        {
            this.Aggregate = new ReleaseArtistAggregate(this);
        }
    }
}
