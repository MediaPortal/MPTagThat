namespace DiscogsNet.Model
{
    public class ReleaseArtistAggregate
    {
        ReleaseArtist releaseArtist;

        public string NameVariationFixed
        {
            get
            {
                if (string.IsNullOrEmpty(this.releaseArtist.NameVariation))
                {
                    return this.NameFixed;
                }
                return ArtistAggregate.FixName(this.releaseArtist.NameVariation);
            }
        }

        /// <summary>
        /// Gets the name variation of the artist if one exists, name otherwise.
        /// </summary>
        public string NameVariationWithFallback
        {
            get
            {
                if (string.IsNullOrEmpty(this.releaseArtist.NameVariation))
                {
                    return this.NameFixed;
                }
                return ArtistAggregate.FixName(this.releaseArtist.NameVariation);
            }
        }

        public string NameFixed
        {
            get
            {
                if (string.IsNullOrEmpty(this.releaseArtist.Name))
                {
                    return "";
                }
                return ArtistAggregate.FixName(this.releaseArtist.Name);
            }
        }

        public ReleaseArtistAggregate(ReleaseArtist releaseArtist)
        {
            this.releaseArtist = releaseArtist;
        }
    }
}
