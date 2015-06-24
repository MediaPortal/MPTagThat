namespace DiscogsNet.Model
{
    public class Image
    {
        public ImageType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Uri { get; set; }
        public string Uri150 { get; set; }

        public ImageAggregate Aggregate { get; private set; }

        public Image()
        {
            this.Aggregate = new ImageAggregate(this);
        }
    }
}
