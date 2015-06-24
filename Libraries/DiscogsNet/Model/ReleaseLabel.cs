namespace DiscogsNet.Model
{
    public class ReleaseLabel
    {
        public int Id { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }

        public ReleaseLabelAggregate Aggregate { get; private set; }

        public ReleaseLabel()
        {
            this.Aggregate = new ReleaseLabelAggregate(this);
        }

        public override string ToString()
        {
            return this.Name + " - " + this.CatalogNumber;
        }
    }
}
