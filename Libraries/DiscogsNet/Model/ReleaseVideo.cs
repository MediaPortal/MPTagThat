namespace DiscogsNet.Model
{
    public class ReleaseVideo
    {
        /// <summary>
        /// The duration of the video in seconds
        /// </summary>
        public int Duration { get; set; }

        public bool Embed { get; set; }
        public string Src { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
