namespace DiscogsNet.Model
{
    public class LabelRelease
    {
        public int Id { get; set; }
        public ReleaseStatus Status { get; set; }
        public string CatalogNumber { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Format { get; set; }

        public LabelReleaseAggregate Aggregate { get; private set; }

        public LabelRelease()
        {
            this.Aggregate = new LabelReleaseAggregate(this);
        }

        public override string ToString()
        {
            return this.Aggregate.ArtistFixed + " - " + this.Title;
        }
    }
}
