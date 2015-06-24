namespace DiscogsNet.Model
{
    public class ExtraArtist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameVariation { get; set; }
        public string Role { get; set; }
        public string Tracks { get; set; }
        public string Join { get; set; }

        public ExtraArtistAggregate Aggregate { get; private set; }

        public ExtraArtist()
        {
            this.Aggregate = new ExtraArtistAggregate(this);
        }

        public override string ToString()
        {
            return this.Aggregate.NameVariationFixed;
        }
    }
}
