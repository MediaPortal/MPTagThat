using System.Linq;
using System.Text.RegularExpressions;

namespace DiscogsNet.Model
{
    public class ArtistAggregate
    {
        private Artist artist;

        public Image PrimaryImage
        {
            get
            {
                if (this.artist.Images == null)
                {
                    return null;
                }
                return this.artist.Images.Where(i => i.Type == ImageType.Primary).FirstOrDefault();
            }
        }

        public Image DesiredImage
        {
            get
            {
                if (this.artist.Images == null)
                {
                    return null;
                }
                if (this.PrimaryImage != null)
                {
                    return this.PrimaryImage;
                }
                return this.artist.Images.FirstOrDefault();
            }
        }

        public string NameFixed
        {
            get
            {
                return FixName(this.artist.Name);
            }
        }

        public string RealNameFixed
        {
            get
            {
                return FixName(this.artist.RealName);
            }
        }

        public string[] NameVariationsFixed
        {
            get
            {
                if (this.artist.NameVariations == null)
                {
                    return null;
                }
                return this.artist.NameVariations.Select(nv => FixName(nv)).ToArray();
            }
        }

        public static NameFixingLevel NameFixingLevel = NameFixingLevel.All;

        private static SimpleRegex stripNameNumberRegex = new SimpleRegex(@"^(?<name>.*)\s\([0-9]+\)$");
        private static string StripNameNumber(string name)
        {
            GroupCollection match = stripNameNumberRegex.Match(name);
            if (match != null)
            {
                return match["name"].Value;
            }
            return name;
        }

        private static string FixThe(string name)
        {
            if (name.EndsWith(", The"))
            {
                return "The " + name.Substring(0, name.Length - 5);
            }
            else
            {
                return name;
            }
        }

        public static string FixName(string name)
        {
            if ((NameFixingLevel & NameFixingLevel.RemoveNumbers) == NameFixingLevel.RemoveNumbers)
            {
                name = StripNameNumber(name);
            }

            if ((NameFixingLevel & NameFixingLevel.FixThe) == NameFixingLevel.FixThe)
            {
                name = FixThe(name);
            }

            return name;
        }

        public ArtistAggregate(Artist artist)
        {
            this.artist = artist;
        }
    }
}
