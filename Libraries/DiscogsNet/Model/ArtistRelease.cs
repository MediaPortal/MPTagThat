namespace DiscogsNet.Model
{
    public class ArtistRelease
    {
        public int Id { get; set; }
        public ReleaseStatus Status { get; set; }
        public ArtistReleaseType Type { get; set; }
        public string Title { get; set; }
        public string Format { get; set; }
        public string Label { get; set; }
        public int Year { get; set; }

        /// <summary>
        /// Present on only some types of ArtistReleaseType's.
        /// Not present in ArtistReleaseType.Main.
        /// </summary>
        public string TrackInfo { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
