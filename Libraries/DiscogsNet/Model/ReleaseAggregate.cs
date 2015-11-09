using System.Linq;
using System.Collections.Generic;

namespace DiscogsNet.Model
{
    public class ReleaseAggregate
    {
        private Release release;

        private int releaseYear = -1;
        public int ReleaseYear
        {
            get
            {
                if (this.releaseYear != -1)
                {
                    return this.releaseYear;
                }
                return this.releaseYear = ReleaseYearGuesser.Guess(this.release.ReleaseDate);
            }
        }

        public string JoinedArtists
        {
            get
            {
                return this.release.Artists.Join();
            }
        }

        public string JoinedArtistsFixed
        {
            get
            {
                return this.release.Artists.JoinFixed();
            }
        }

        public Image PrimaryImage
        {
            get
            {
                if (this.release.Images == null)
                {
                    return null;
                }
                return this.release.Images.Where(i => i.Type == ImageType.Primary).FirstOrDefault();
            }
        }

        public Image DesiredImage
        {
            get
            {
                if (this.release.Images == null)
                {
                    return null;
                }
                if (this.PrimaryImage != null)
                {
                    return this.PrimaryImage;
                }
                return this.release.Images.FirstOrDefault();
            }
        }

        public string JoinedLabels
        {
            get
            {
                return this.release.Labels.Select(l => l.Name).Join(", ");
            }
        }

        public string JoinedLabelsFixed
        {
            get
            {
                return this.release.Labels.Select(l => l.Aggregate.NameFixed).Join(", ");
            }
        }

        public string JoinedCatalogNumbers
        {
            get
            {
                return this.release.Labels.Select(l => l.CatalogNumber).Join(", ");
            }
        }

        public string JoinedGenres
        {
            get
            {
                if (this.release.Genres == null)
                {
                    return "";
                }
                return this.release.Genres.Join(", ");
            }
        }

        public string JoinedStyles
        {
            get
            {
                if (this.release.Styles == null)
                {
                    return "";
                }
                return this.release.Styles.Join(", ");
            }
        }

        public bool HasTrackArtists
        {
            get
            {
                return this.release.Tracklist.Any(t => t.Artists != null);
            }
        }

        public bool HasNotes
        {
            get
            {
                return !string.IsNullOrEmpty(this.release.Notes);
            }
        }

        public bool HasExtraArtists
        {
            get
            {
                return this.release.ExtraArtists != null && this.release.ExtraArtists.Length > 0;
            }
        }

        public string JoinedFormats
        {
            get
            {
                List<string> formats = new List<string>();
                foreach (ReleaseFormat format in this.release.Formats)
                {
                    string description = "";
                    if (format.Quantity != 1)
                    {
                        description += format.Quantity + " x ";
                    }
                    description += format.Name;
                    if (format.Descriptions != null && format.Descriptions.Length > 0)
                    {
                        description += ", " + format.Descriptions.Join(", ");
                    }
                    formats.Add(description);
                }
                return formats.Join(", ");
            }
        }

        public ReleaseAggregate(Release release)
        {
            this.release = release;
        }
    }
}
