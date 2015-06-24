namespace DiscogsNet.Model
{
    public class Label
    {
        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public int Id { get; set; }

        public Image[] Images { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// A URI to the label profile page in the Discogs site
        /// </summary>
        public string Uri { get; set; }

        public string ContactInfo { get; set; }
        public string Profile { get; set; }
        public string[] Urls { get; set; }
        public string ParentLabel { get; set; }
        public LabelRelease[] Releases { get; set; }

        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public DataQuality DataQuality { get; set; }

        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public Sublabel[] Sublabels { get; set; }

        public Label()
        {
            this.Name = "";
            this.ContactInfo = "";
            this.Profile = "";
            this.ParentLabel = "";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
