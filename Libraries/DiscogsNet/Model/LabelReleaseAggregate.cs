namespace DiscogsNet.Model
{
    public class LabelReleaseAggregate
    {
        private LabelRelease labelRelease;

        public string ArtistFixed
        {
            get
            {
                return ArtistAggregate.FixName(this.labelRelease.Artist);
            }
        }

        public LabelReleaseAggregate(LabelRelease labelRelease)
        {
            this.labelRelease = labelRelease;
        }
    }
}
