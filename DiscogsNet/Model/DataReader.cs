using System;
using System.Linq;
using System.Xml.Linq;

namespace DiscogsNet.Model
{
    public partial class DataReader
    {
        public static ReleaseStatus ParseReleaseStatus(string status)
        {
            if (status == "Accepted")
            {
                return ReleaseStatus.Accepted;
            }
            else if (status == "Draft")
            {
                return ReleaseStatus.Draft;
            }
            else if (status == "Deleted")
            {
                return ReleaseStatus.Deleted;
            }
            else if (status == "Rejected")
            {
                return ReleaseStatus.Rejected;
            }
            else if (status == "Pending")
            {
                return ReleaseStatus.Pending;
            }
            else
            {
                throw new Exception("Unknown release status: " + status);
            }
        }

        private static ArtistReleaseType ParseArtistReleaseType(string type)
        {
            ArtistReleaseType artistReleaseType;
            if (Enum.TryParse<ArtistReleaseType>(type, true, out artistReleaseType))
            {
                return artistReleaseType;
            }
            else
            {
                throw new Exception("Unknown artist release type: " + type);
            }
        }

        public static ImageType ParseImageType(string imageType)
        {
            if (imageType == "primary")
            {
                return ImageType.Primary;
            }
            else if (imageType == "secondary")
            {
                return ImageType.Secondary;
            }
            else
            {
                throw new Exception("Unknown image type: " + imageType);
            }
        }

        public static Image ReadImage(XElement el)
        {
            el.AssertName("image");
            el.AssertNoElements();

            Image result = new Image();
            foreach (XAttribute attr in el.Attributes())
            {
                if (attr.Name == "type")
                {
                    result.Type = ParseImageType(attr.Value);
                }
                else if (attr.Name == "width")
                {
                    result.Width = int.Parse(attr.Value);
                }
                else if (attr.Name == "height")
                {
                    result.Height = int.Parse(attr.Value);
                }
                else if (attr.Name == "uri")
                {
                    result.Uri = attr.Value;
                }
                else if (attr.Name == "uri150")
                {
                    result.Uri150 = attr.Value;
                }
                else
                {
                    throw new Exception("Unknown image attribute: " + attr.Name);
                }
            }
            return result;
        }

        public static Artist ReadArtist(XElement artist)
        {
            artist.AssertName("artist");
            artist.AssertNoAttributes();

            Artist result = new Artist();
            foreach (XElement e in artist.Elements())
            {
                if (e.Name == "images")
                {
                    result.Images = e.Elements().Select(image => ReadImage(image)).ToArray();
                }
                else if (e.Name == "name")
                {
                    e.AssertOnlyText();
                    result.Name = e.Value;
                }
                else if (e.Name == "realname")
                {
                    e.AssertOnlyText();
                    result.RealName = e.Value;
                }
                else if (e.Name == "namevariations")
                {
                    result.NameVariations = e.Elements().AssertNames("name").AssertOnlyText().Select(nameVariation => nameVariation.Value).ToArray();
                }
                else if (e.Name == "aliases")
                {
                    result.Aliases = e.Elements().AssertNames("name").AssertOnlyText().Select(alias => new ArtistAlias() { Name = alias.Value }).ToArray();
                }
                else if (e.Name == "members")
                {
                    result.Members = e.Elements().AssertNames("name").AssertOnlyText().Select(member => new Members() { Name = member.Value }).ToArray();
                }
                else if (e.Name == "profile")
                {
                    e.AssertOnlyText();
                    result.Profile = e.Value.TrimAndNormalizeLineEndings();
                }
                else if (e.Name == "urls")
                {
                    result.Urls = e.Elements().AssertNames("url").AssertOnlyText().Select(url => url.Value).ToArray();
                }
                else if (e.Name == "groups")
                {
                    result.Groups = e.Elements().AssertNames("name").AssertOnlyText().Select(group => group.Value).ToArray();
                }
                else if (e.Name == "releases")
                {
                    result.Releases = e.Elements().Select(release => ReadArtistRelease(release)).ToArray();
                }
                else
                {
                    throw new Exception("Unknown artist element: " + e.Name);
                }
            }
            return result;
        }

        public static ArtistRelease ReadArtistRelease(XElement artistRelease)
        {
            artistRelease.AssertName("release");

            ArtistRelease result = new ArtistRelease();
            foreach (XAttribute attr in artistRelease.Attributes())
            {
                if (attr.Name == "id")
                {
                    result.Id = int.Parse(attr.Value);
                }
                else if (attr.Name == "status")
                {
                    result.Status = ParseReleaseStatus(attr.Value);
                }
                else if (attr.Name == "type")
                {
                    result.Type = ParseArtistReleaseType(attr.Value);
                }
                else
                {
                    throw new Exception("Unknown artist release attribute: " + attr.Name);
                }
            }

            foreach (XElement e in artistRelease.Elements())
            {
                if (e.Name == "title")
                {
                    e.AssertOnlyText();
                    result.Title = e.Value;
                }
                else if (e.Name == "format")
                {
                    e.AssertOnlyText();
                    result.Format = e.Value;
                }
                else if (e.Name == "label")
                {
                    e.AssertOnlyText();
                    result.Label = e.Value;
                }
                else if (e.Name == "year")
                {
                    e.AssertOnlyText();
                    result.Year = int.Parse(e.Value);
                }
                else if (e.Name == "trackinfo")
                {
                    e.AssertOnlyText();
                    result.TrackInfo = e.Value;
                }
                else
                {
                    throw new Exception("Unknown artist release element: " + e.Name);
                }
            }

            return result;
        }

        public static ReleaseLabel ReadReleaseLabel(XElement label)
        {
            label.AssertName("label");
            label.AssertNoElements();

            ReleaseLabel result = new ReleaseLabel();

            foreach (XAttribute attr in label.Attributes())
            {
                if (attr.Name == "catno")
                {
                    result.CatalogNumber = attr.Value;
                }
                else if (attr.Name == "name")
                {
                    result.Name = attr.Value;
                }
                else
                {
                    throw new Exception("Unknown release label attribute: " + attr.Name);
                }
            }

            return result;
        }

        public static ReleaseArtist ReadReleaseArtist(XElement releaseArtist)
        {
            releaseArtist.AssertName("artist");
            releaseArtist.AssertNoAttributes();

            ReleaseArtist result = new ReleaseArtist();

            foreach (XElement e in releaseArtist.Elements())
            {
                if (e.Name == "name")
                {
                    e.AssertOnlyText();
                    result.Name = e.Value;
                }
                else if (e.Name == "anv")
                {
                    e.AssertOnlyText();
                    result.NameVariation = e.Value;
                }
                else if (e.Name == "join")
                {
                    e.AssertOnlyText();
                    result.Join = e.Value;
                }
                else if (e.Name == "role")
                {
                    if (e.IsEmpty)
                    {
                        continue;
                    }
                    throw new NotImplementedException();
                }
                else if (e.Name == "tracks")
                {
                    if (e.IsEmpty)
                    {
                        continue;
                    }
                    throw new NotImplementedException();
                }
                else
                {
                    throw new Exception("Unknown release artist element: " + e.Name);
                }
            }

            return result;
        }

        public static Release ReadRelease(XElement release)
        {
            release.AssertName("release");

            Release result = new Release();
            foreach (XAttribute attr in release.Attributes())
            {
                if (attr.Name == "id")
                {
                    result.Id = int.Parse(attr.Value);
                }
                else if (attr.Name == "status")
                {
                    result.Status = ParseReleaseStatus(attr.Value);
                }
                else
                {
                    throw new Exception("Unknown release attribute: " + attr.Name);
                }
            }

            foreach (XElement e in release.Elements())
            {
                if (e.Name == "master_id")
                {
                    result.MasterId = int.Parse(e.Value);
                }
                else if (e.Name == "images")
                {
                    result.Images = e.Elements().Select(img => ReadImage(img)).ToArray();
                }
                else if (e.Name == "artists")
                {
                    result.Artists = e.Elements().Select(artist => ReadReleaseArtist(artist)).ToArray();
                }
                else if (e.Name == "title")
                {
                    e.AssertOnlyText();
                    result.Title = e.Value;
                }
                else if (e.Name == "labels")
                {
                    result.Labels = e.Elements().Select(label => ReadReleaseLabel(label)).ToArray();
                }
                else if (e.Name == "formats")
                {
                    result.Formats = e.Elements().Select(format => ReadReleaseFormat(format)).ToArray();
                }
                else if (e.Name == "genres")
                {
                    result.Genres = e.Elements().AssertNames("genre").AssertOnlyText().Select(genre => genre.Value).ToArray();
                }
                else if (e.Name == "styles")
                {
                    result.Styles = e.Elements().AssertNames("style").AssertOnlyText().Select(style => style.Value).ToArray();
                }
                else if (e.Name == "country")
                {
                    e.AssertOnlyText();
                    result.Country = e.Value;
                }
                else if (e.Name == "released")
                {
                    e.AssertOnlyText();
                    result.ReleaseDate = e.Value;
                }
                else if (e.Name == "notes")
                {
                    e.AssertOnlyText();
                    result.Notes = e.Value.TrimAndNormalizeLineEndings();
                }
                else if (e.Name == "tracklist")
                {
                    result.Tracklist = e.Elements().Select(track => ReadTrack(track)).ToArray();
                }
                else if (e.Name == "extraartists")
                {
                    result.ExtraArtists = e.Elements().Select(extraArtist => ReadExtraArtist(extraArtist)).ToArray();
                }
                else
                {
                    throw new Exception("Unknown release element: " + e.Name);
                }
            }
            return result;
        }

        public static ReleaseFormat ReadReleaseFormat(XElement el)
        {
            el.AssertName("format");

            ReleaseFormat result = new ReleaseFormat();

            foreach (XAttribute attr in el.Attributes())
            {
                if (attr.Name == "name")
                {
                    result.Name = attr.Value;
                }
                else if (attr.Name == "qty")
                {
                    result.Quantity = int.Parse(attr.Value);
                }
                else if (attr.Name == "text")
                {
                    result.Text = attr.Value;
                }
                else
                {
                    throw new Exception("Unknown release format attribute: " + attr.Name);
                }
            }

            foreach (XElement e in el.Elements())
            {
                if (e.Name == "descriptions")
                {
                    result.Descriptions = e.Elements().AssertNames("description").AssertOnlyText().Select(d => d.Value).ToArray();
                }
                else
                {
                    throw new Exception("Unknown release format element: " + e.Name);
                }
            }

            return result;
        }

        public static ExtraArtist ReadExtraArtist(XElement el)
        {
            el.AssertName("artist");

            ExtraArtist result = new ExtraArtist();

            foreach (XElement e in el.Elements())
            {
                if (e.Name == "name")
                {
                    e.AssertOnlyText();
                    result.Name.AssertNull();
                    result.Name = e.Value;
                }
                else if (e.Name == "anv")
                {
                    e.AssertOnlyText();
                    result.NameVariation.AssertNull();
                    result.NameVariation = e.Value;
                }
                else if (e.Name == "role")
                {
                    e.AssertOnlyText();
                    result.Role.AssertNull();
                    result.Role = e.Value;
                }
                else if (e.Name == "tracks")
                {
                    e.AssertOnlyText();
                    result.Tracks = e.Value;
                }
                else if (e.Name == "join")
                {
                    e.AssertOnlyText();
                    result.Join.AssertNull();
                    result.Join = e.Value;
                }
                else
                {
                    throw new Exception("Unknown extra artist element: " + e.Name);
                }
            }

            return result;
        }

        public static Track ReadTrack(XElement el)
        {
            el.AssertName("track");
            el.AssertNoAttributes();

            Track result = new Track();

            foreach (XElement e in el.Elements())
            {
                if (e.Name == "position")
                {
                    e.AssertOnlyText();
                    result.Position = e.Value;
                }
                else if (e.Name == "title")
                {
                    e.AssertOnlyText();
                    result.Title = e.Value;
                }
                else if (e.Name == "duration")
                {
                    e.AssertOnlyText();
                    result.Duration = e.Value;
                }
                else if (e.Name == "artists")
                {
                    result.Artists = e.Elements().Select(a => ReadReleaseArtist(a)).ToArray();
                }
                else if (e.Name == "extraartists")
                {
                    result.ExtraArtists = e.Elements().Select(a => ReadExtraArtist(a)).ToArray();
                }
                else
                {
                    throw new Exception("Unknown track element: " + e.Name);
                }
            }

            return result;
        }

        public static Label ReadLabel(XElement el)
        {
            el.AssertName("label");
            el.AssertNoAttributes();

            Label result = new Label();

            foreach (XElement e in el.Elements())
            {
                if (e.Name == "images")
                {
                    result.Images = e.Elements().Select(i => ReadImage(i)).ToArray();
                }
                else if (e.Name == "name")
                {
                    e.AssertOnlyText();
                    result.Name = e.Value;
                }
                else if (e.Name == "contactinfo")
                {
                    e.AssertOnlyText();
                    result.ContactInfo = e.Value.TrimAndNormalizeLineEndings();
                }
                else if (e.Name == "profile")
                {
                    e.AssertOnlyText();
                    result.Profile = e.Value.TrimAndNormalizeLineEndings();
                }
                else if (e.Name == "urls")
                {
                    result.Urls = e.Elements().AssertNames("url").AssertOnlyText().Select(i => e.Value).ToArray();
                }
                else if (e.Name == "sublabels")
                {
                }
                else if (e.Name == "parentLabel")
                {
                    e.AssertOnlyText();
                    result.ParentLabel = e.Value;
                }
                else if (e.Name == "releases")
                {
                    result.Releases = e.Elements().Select(release => ReadLabelRelease(release)).ToArray();
                }
                else
                {
                    throw new Exception("Unknown label element: " + e.Name);
                }
            }

            return result;
        }

        public static LabelRelease ReadLabelRelease(XElement labelRelease)
        {
            labelRelease.AssertName("release");

            LabelRelease result = new LabelRelease();

            foreach (XAttribute attr in labelRelease.Attributes())
            {
                if (attr.Name == "id")
                {
                    result.Id = int.Parse(attr.Value);
                }
                else if (attr.Name == "status")
                {
                    result.Status = ParseReleaseStatus(attr.Value);
                }
                else
                {
                    throw new Exception("Unknown label release attribute: " + attr.Name);
                }
            }

            foreach (XElement e in labelRelease.Elements())
            {
                if (e.Name == "title")
                {
                    e.AssertOnlyText();
                    result.Title = e.Value;
                }
                else if (e.Name == "catno")
                {
                    e.AssertOnlyText();
                    result.CatalogNumber = e.Value;
                }
                else if (e.Name == "artist")
                {
                    e.AssertOnlyText();
                    result.Artist = e.Value;
                }
                else if (e.Name == "format")
                {
                    e.AssertOnlyText();
                    result.Format = e.Value;
                }
                else
                {
                    throw new Exception("Unknown label release element: " + e.Name);
                }
            }

            return result;
        }

        public static DataQuality ParseDataQuality(string dataQuality)
        {
            switch (dataQuality)
            {
                case "Correct":
                    return DataQuality.Correct;
                case "Complete and Correct":
                    return DataQuality.CompleteAndCorrect;
                case "Needs Vote":
                    return DataQuality.NeedsVote;
                case "Needs Minor Changes":
                    return DataQuality.NeedsMinorChanges;
                case "Needs Major Changes":
                    return DataQuality.NeedsMajorChanges;
                case "Entirely Incorrect":
                    return DataQuality.EntirelyIncorrect;
                default:
                    throw new FormatException("Unknown data quality.");
            }
        }

        public static ReleaseIdentifierType ParseReleaseIdentifierType(string identifierType)
        {
            switch (identifierType.ToLower())
            {
                case "barcode":
                    return ReleaseIdentifierType.Barcode;
                case "matrix / runout":
                    return ReleaseIdentifierType.MatrixOrRunout;
                case "asin":
                    return ReleaseIdentifierType.ASIN;
                case "rights society":
                    return ReleaseIdentifierType.RightsSociety;
                case "label code":
                    return ReleaseIdentifierType.LabelCode;
                case "mastering sid code":
                    return ReleaseIdentifierType.MasteringSidCode;
                case "mould sid code":
                    return ReleaseIdentifierType.MouldSidCode;
                case "mastering code":
                    return ReleaseIdentifierType.MasteringCode;
                case "mould code":
                    return ReleaseIdentifierType.MouldCode;
                case "other":
                    return ReleaseIdentifierType.Other;
                default:
                    throw new FormatException("Unknown identifier type.");
            }
        }
    }
}
