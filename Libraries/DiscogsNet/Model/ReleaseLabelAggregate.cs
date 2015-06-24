namespace DiscogsNet.Model
{
    public class ReleaseLabelAggregate
    {
        private ReleaseLabel releaseLabel;

        public string NameFixed
        {
            get
            {
                return ArtistAggregate.FixName(this.releaseLabel.Name);
            }
        }

        public ReleaseLabelAggregate(ReleaseLabel releaseLabel)
        {
            this.releaseLabel = releaseLabel;
        }
    }
}
