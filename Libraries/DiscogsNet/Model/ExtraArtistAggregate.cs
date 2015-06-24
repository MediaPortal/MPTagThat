namespace DiscogsNet.Model
{
    public class ExtraArtistAggregate
    {
        private ExtraArtist extraArtist;

        public string NameFixed
        {
            get
            {
                return ArtistAggregate.FixName(this.extraArtist.Name);
            }
        }

        public string NameVariationFixed
        {
            get
            {
                if (string.IsNullOrEmpty(this.extraArtist.NameVariation))
                {
                    return this.NameFixed;
                }
                return ArtistAggregate.FixName(this.extraArtist.NameVariation);
            }
        }

        public ExtraArtistAggregate(ExtraArtist extraArtist)
        {
            this.extraArtist = extraArtist;
        }
    }
}
