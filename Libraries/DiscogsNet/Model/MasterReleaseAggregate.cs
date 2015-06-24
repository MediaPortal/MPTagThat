using System;
using System.Linq;

namespace DiscogsNet.Model
{
    public class MasterReleaseAggregate
    {
        private MasterRelease master;

        public string JoinedArtists
        {
            get
            {
                if (this.master.Artists == null)
                {
                    return "";
                }
                return this.master.Artists.Join();
            }
        }

        public string JoinedArtistsFixed
        {
            get
            {
                if (this.master.Artists == null)
                {
                    return "";
                }
                return this.master.Artists.JoinFixed();
            }
        }

        public Image PrimaryImage
        {
            get
            {
                if (this.master.Images == null)
                {
                    return null;
                }
                return this.master.Images.Where(i => i.Type == ImageType.Primary).FirstOrDefault();
            }
        }

        public Image DesiredImage
        {
            get
            {
                if (this.master.Images == null)
                {
                    return null;
                }
                if (this.PrimaryImage != null)
                {
                    return this.PrimaryImage;
                }
                return this.master.Images.FirstOrDefault();
            }
        }

        public string JoinedGenres
        {
            get
            {
                if (this.master.Genres == null)
                {
                    return "";
                }
                return this.master.Genres.Join(", ");
            }
        }

        public string JoinedStyles
        {
            get
            {
                if (this.master.Styles == null)
                {
                    return "";
                }
                return this.master.Styles.Join(", ");
            }
        }

        public bool HasTrackArtists
        {
            get
            {
                return this.master.Tracklist.Any(t => t.Artists != null);
            }
        }

        public bool HasNotes
        {
            get
            {
                return !string.IsNullOrEmpty(this.master.Notes);
            }
        }

        public MasterReleaseAggregate(MasterRelease master)
        {
            this.master = master;
        }
    }
}
