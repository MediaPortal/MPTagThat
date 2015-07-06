using System;
using System.Linq;

namespace DiscogsNet.Model
{
    public class ReleaseBase
    {
        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public int Id { get; set; }

        public string Title { get; set; }
        public Image[] Images { get; set; }
        public ReleaseArtist[] Artists { get; set; }
        public string[] Genres { get; set; }
        public string[] Styles { get; set; }
        public string Notes { get; set; }
        public Track[] Tracklist { get; set; }

        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public ReleaseVideo[] Videos { get; set; }

        /// <summary>
        /// Present only when loading from API 2.0
        /// </summary>
        public DataQuality DataQuality { get; set; }
    }
}
