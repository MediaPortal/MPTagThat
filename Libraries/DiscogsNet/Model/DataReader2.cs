using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DiscogsNet.Model
{
    public class DataReader2
    {
        public XmlReader XmlReader { get; private set; }

        public DataReader2(XmlReader xmlReader)
        {
            this.XmlReader = xmlReader;
        }

        public void ReadResponseHeader()
        {
            this.XmlReader.AssertRead();
            this.XmlReader.AssertElementStart("resp");
            this.XmlReader.AssertRead();
        }

        public Release ReadRelease()
        {
            this.XmlReader.AssertElementStart("release");

            Release release = new Release();
            release.Id = int.Parse(this.XmlReader.GetAttribute("id"));
            release.Status = DataReader.ParseReleaseStatus(this.XmlReader.GetAttribute("status"));

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("release"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("images"))
                {
                    release.Images = this.ReadImages();
                }
                else if (this.XmlReader.IsElementStart("artists"))
                {
                    release.Artists = this.ReadReleaseArtists();
                }
                else if (this.XmlReader.IsElementStart("title"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    release.Title = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("labels"))
                {
                    release.Labels = this.ReadReleaseLabels();
                }
                else if (this.XmlReader.IsElementStart("extraartists"))
                {
                    release.ExtraArtists = this.ReadExtraArtists();
                }
                else if (this.XmlReader.IsElementStart("formats"))
                {
                    release.Formats = this.ReadFormats();
                }
                else if (this.XmlReader.IsElementStart("genres"))
                {
                    release.Genres = this.ReadGenres();
                }
                else if (this.XmlReader.IsElementStart("styles"))
                {
                    release.Styles = this.ReadStyles();
                }
                else if (this.XmlReader.IsElementStart("country"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    release.Country = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("released"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    release.ReleaseDate = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("notes"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    release.Notes = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("master_id"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    release.MasterId = int.Parse(this.XmlReader.ReadContentAsString());
                }
                else if (this.XmlReader.IsElementStart("data_quality"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    release.DataQuality = DataReader.ParseDataQuality(this.XmlReader.ReadContentAsString());
                }
                else if (this.XmlReader.IsElementStart("tracklist"))
                {
                    release.Tracklist = this.ReadTracklist();
                }
                else if (this.XmlReader.IsElementStart("videos"))
                {
                    release.Videos = this.ReadVideos();
                }
                else if (this.XmlReader.IsElementStart("identifiers"))
                {
                    release.Identifiers = this.ReadReleaseIdentifiers();
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return release;
        }

        private ReleaseIdentifier[] ReadReleaseIdentifiers()
        {
            this.XmlReader.AssertElementStart("identifiers");

            List<ReleaseIdentifier> identifiers = new List<ReleaseIdentifier>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("identifiers"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("identifier"))
                {
                    identifiers.Add(this.ReadReleaseIdentifier());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return identifiers.ToArray();
        }

        private ReleaseIdentifier ReadReleaseIdentifier()
        {
            this.XmlReader.AssertElementStart("identifier");
            this.XmlReader.AssertEmptyElement();

            ReleaseIdentifier identifier = new ReleaseIdentifier();
            identifier.Type = DataReader.ParseReleaseIdentifierType(this.XmlReader.GetAttribute("type"));
            identifier.Value = this.XmlReader.GetAttribute("value");
            identifier.Description = this.XmlReader.GetAttribute("description");

            return identifier;
        }

        private ReleaseVideo[] ReadVideos()
        {
            this.XmlReader.AssertElementStart("videos");

            List<ReleaseVideo> videos = new List<ReleaseVideo>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("videos"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("video"))
                {
                    videos.Add(this.ReadVideo());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return videos.ToArray();
        }

        private ReleaseVideo ReadVideo()
        {
            this.XmlReader.AssertElementStart("video");

            ReleaseVideo video = new ReleaseVideo();
            video.Duration = int.Parse(this.XmlReader.GetAttribute("duration"));
            video.Embed = bool.Parse(this.XmlReader.GetAttribute("embed"));
            video.Src = this.XmlReader.GetAttribute("src");

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("video"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("title"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        video.Title = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("description"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        video.Description = this.XmlReader.ReadContentAsString();
                    }
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return video;
        }

        private Track[] ReadTracklist()
        {
            this.XmlReader.AssertElementStart("tracklist");

            List<Track> tracks = new List<Track>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("tracklist"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("track"))
                {
                    tracks.Add(this.ReadTrack());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return tracks.ToArray();
        }

        private Track ReadTrack()
        {
            this.XmlReader.AssertElementStart("track");

            Track track = new Track();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("track"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("position"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        track.Position = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("title"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        track.Title = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("duration"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        track.Duration = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsStartElement("extraartists"))
                {
                    track.ExtraArtists = this.ReadExtraArtists();
                }
                else if (this.XmlReader.IsElementStart("artists"))
                {
                    track.Artists = this.ReadReleaseArtists();
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return track;
        }

        private string[] ReadGenres()
        {
            this.XmlReader.AssertElementStart("genres");

            List<string> genres = new List<string>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("genres"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("genre"))
                {
                    genres.Add(this.ReadGenre());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return genres.ToArray();
        }

        private string ReadGenre()
        {
            this.XmlReader.AssertElementStart("genre");
            this.XmlReader.AssertNonEmptyElement();
            this.XmlReader.AssertRead();
            return this.XmlReader.ReadContentAsString();
        }

        private string[] ReadStyles()
        {
            this.XmlReader.AssertElementStart("styles");

            List<string> styles = new List<string>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("styles"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("style"))
                {
                    styles.Add(this.ReadStyle());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return styles.ToArray();
        }

        private string ReadStyle()
        {
            this.XmlReader.AssertElementStart("style");
            this.XmlReader.AssertNonEmptyElement();
            this.XmlReader.AssertRead();
            return this.XmlReader.ReadContentAsString();
        }

        private ReleaseFormat[] ReadFormats()
        {
            this.XmlReader.AssertElementStart("formats");

            List<ReleaseFormat> formats = new List<ReleaseFormat>();

            if (this.XmlReader.IsEmptyElement)
            {
                return formats.ToArray();
            }

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("formats"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("format"))
                {
                    formats.Add(this.ReadFormat());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return formats.ToArray();
        }

        private ReleaseFormat ReadFormat()
        {
            this.XmlReader.AssertElementStart("format");

            ReleaseFormat format = new ReleaseFormat();
            format.Name = this.XmlReader.GetAttribute("name");

            int quantity;
            if (int.TryParse(this.XmlReader.GetAttribute("qty"), out quantity))
            {
                format.Quantity = quantity;
            }
            format.Text = this.XmlReader.GetAttribute("qty");

            if (this.XmlReader.IsEmptyElement)
            {
                return format;
            }

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("format"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("descriptions"))
                {
                    format.Descriptions = this.ReadFormatDescriptions();
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return format;
        }

        private string[] ReadFormatDescriptions()
        {
            this.XmlReader.AssertElementStart("descriptions");

            List<string> descriptions = new List<string>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("descriptions"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("description"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    descriptions.Add(this.XmlReader.ReadContentAsString());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return descriptions.ToArray();
        }

        private ExtraArtist[] ReadExtraArtists()
        {
            this.XmlReader.AssertElementStart("extraartists");

            List<ExtraArtist> artists = new List<ExtraArtist>();

            if (this.XmlReader.IsEmptyElement)
            {
                return artists.ToArray();
            }

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("extraartists"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("artist"))
                {
                    artists.Add(this.ReadExtraArtist());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return artists.ToArray();
        }

        private ExtraArtist ReadExtraArtist()
        {
            this.XmlReader.AssertElementStart("artist");

            ExtraArtist artist = new ExtraArtist();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("artist"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("name"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Name = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("anv"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.NameVariation = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("join"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Join = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("role"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Role = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("tracks"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Tracks = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("id"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Id = this.XmlReader.ReadContentAsInt();
                    }
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return artist;
        }

        private ReleaseLabel[] ReadReleaseLabels()
        {
            this.XmlReader.AssertElementStart("labels");

            List<ReleaseLabel> labels = new List<ReleaseLabel>();

            if (this.XmlReader.IsEmptyElement)
            {
                return labels.ToArray();
            }

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("labels"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("label"))
                {
                    labels.Add(this.ReadReleaseLabel());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return labels.ToArray();
        }

        private ReleaseLabel ReadReleaseLabel()
        {
            this.XmlReader.AssertElementStart("label");
            this.XmlReader.AssertEmptyElement();

            ReleaseLabel label = new ReleaseLabel();
            label.CatalogNumber = this.XmlReader.GetAttribute("catno");
            label.Name = this.XmlReader.GetAttribute("name");

            return label;
        }

        private ReleaseArtist[] ReadReleaseArtists()
        {
            this.XmlReader.AssertElementStart("artists");

            List<ReleaseArtist> artists = new List<ReleaseArtist>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("artists"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("artist"))
                {
                    var artist = this.ReadReleaseArtist();
                    if (artist.Name != null)
                    {
                        artists.Add(artist);
                    }
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return artists.ToArray();
        }

        private ReleaseArtist ReadReleaseArtist()
        {
            this.XmlReader.AssertElementStart("artist");

            ReleaseArtist artist = new ReleaseArtist();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("artist"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("name"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Name = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("anv"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.NameVariation = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("join"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Join = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("id"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Id = this.XmlReader.ReadContentAsInt();
                    }
                }
                else if (this.XmlReader.IsElementStart("role"))
                {
                    this.XmlReader.AssertEmptyElement();
                }
                else if (this.XmlReader.IsElementStart("tracks"))
                {
                    this.XmlReader.AssertEmptyElement();
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return artist;
        }

        private Image[] ReadImages()
        {
            this.XmlReader.AssertElementStart("images");

            List<Image> images = new List<Image>();

            if (this.XmlReader.IsEmptyElement)
            {
                return images.ToArray();
            }

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("images"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("image"))
                {
                    images.Add(this.ReadImage());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return images.ToArray();
        }

        private Image ReadImage()
        {
            this.XmlReader.AssertElementStart("image");
            this.XmlReader.AssertEmptyElement();

            Image image = new Image();
            image.Width = int.Parse(this.XmlReader.GetAttribute("width"));
            image.Height = int.Parse(this.XmlReader.GetAttribute("height"));
            image.Type = DataReader.ParseImageType(this.XmlReader.GetAttribute("type"));
            image.Uri = this.XmlReader.GetAttribute("uri");
            image.Uri150 = this.XmlReader.GetAttribute("uri150");

            return image;
        }

        public MasterRelease ReadMasterRelease()
        {
            this.XmlReader.AssertElementStart("master");

            MasterRelease master = new MasterRelease();
            master.Id = int.Parse(this.XmlReader.GetAttribute("id"));

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("master"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("main_release"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    master.MainRelease = int.Parse(this.XmlReader.ReadContentAsString());
                }
                else if (this.XmlReader.IsElementStart("images"))
                {
                    master.Images = this.ReadImages();
                }
                else if (this.XmlReader.IsElementStart("artists"))
                {
                    master.Artists = this.ReadReleaseArtists();
                }
                else if (this.XmlReader.IsElementStart("genres"))
                {
                    master.Genres = this.ReadGenres();
                }
                else if (this.XmlReader.IsElementStart("styles"))
                {
                    master.Styles = this.ReadStyles();
                }
                else if (this.XmlReader.IsElementStart("year"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    master.Year = int.Parse(this.XmlReader.ReadContentAsString());
                }
                else if (this.XmlReader.IsElementStart("title"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        master.Title = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("data_quality"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    master.DataQuality = DataReader.ParseDataQuality(this.XmlReader.ReadContentAsString());
                }
                else if (this.XmlReader.IsElementStart("tracklist"))
                {
                    master.Tracklist = this.ReadTracklist();
                }
                else if (this.XmlReader.IsElementStart("videos"))
                {
                    master.Videos = this.ReadVideos();
                }
                else if (this.XmlReader.IsElementStart("versions"))
                {
                    master.Versions = this.ReadReleaseVersions();
                }
                else if (this.XmlReader.IsElementStart("notes"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    master.Notes = this.XmlReader.ReadContentAsString();
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return master;
        }

        private ReleaseVersion[] ReadReleaseVersions()
        {
            this.XmlReader.AssertElementStart("versions");

            List<ReleaseVersion> versions = new List<ReleaseVersion>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("versions"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("release"))
                {
                    versions.Add(this.ReadReleaseVersion());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return versions.ToArray();
        }

        private ReleaseVersion ReadReleaseVersion()
        {
            this.XmlReader.AssertElementStart("release");

            ReleaseVersion version = new ReleaseVersion();
            version.Id = int.Parse(this.XmlReader.GetAttribute("id"));
            version.Status = DataReader.ParseReleaseStatus(this.XmlReader.GetAttribute("status"));

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("release"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("title"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    version.Title = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("format"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    version.Format = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("label"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    version.Label = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("catno"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    version.CatalogNumber = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("country"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    version.Country = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("released"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    version.ReleaseDate = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("thumb"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    version.Thumb = this.XmlReader.ReadContentAsString();
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return version;
        }

        public Artist ReadArtist()
        {
            this.XmlReader.AssertElementStart("artist");

            Artist artist = new Artist();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("artist"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("images"))
                {
                    artist.Images = this.ReadImages();
                }
                else if (this.XmlReader.IsElementStart("id"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    artist.Id = int.Parse(this.XmlReader.ReadContentAsString());
                }
                else if (this.XmlReader.IsElementStart("name"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        artist.Name = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("realname"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    artist.RealName = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("realname"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    artist.RealName = this.XmlReader.ReadContentAsString();
                }
                else if (this.XmlReader.IsElementStart("profile"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    artist.Profile = this.XmlReader.ReadContentAsString().Trim();
                }
                else if (this.XmlReader.IsElementStart("data_quality"))
                {
                    this.XmlReader.AssertNonEmptyElement();
                    this.XmlReader.AssertRead();
                    artist.DataQuality = DataReader.ParseDataQuality(this.XmlReader.ReadContentAsString());
                }
                else if (this.XmlReader.IsElementStart("urls"))
                {
                    artist.Urls = this.ReadUrls();
                }
                else if (this.XmlReader.IsElementStart("namevariations"))
                {
                    artist.NameVariations = this.ReadNameVariations();
                }
                else if (this.XmlReader.IsElementStart("aliases"))
                {
                    artist.Aliases = this.ReadAliases();
                }
                else if (this.XmlReader.IsElementStart("groups"))
                {
                    artist.Groups = this.ReadGroups();
                }
                //else if (this.XmlReader.IsElementStart("members"))
                //{
                //    artist.Members = this.ReadMembers();
                //}
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return artist;
        }

        private string[] ReadMembers()
        {
            this.XmlReader.AssertElementStart("members");

            List<string> members = new List<string>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("members"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("name"))
                {
                    members.AddIfNotNullOrWhiteSpace(this.ReadName());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return members.ToArray();
        }

        private string[] ReadGroups()
        {
            this.XmlReader.AssertElementStart("groups");

            List<string> groups = new List<string>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("groups"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("name"))
                {
                    groups.AddIfNotNullOrWhiteSpace(this.ReadName());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return groups.ToArray();
        }

        private ArtistAlias[] ReadAliases()
        {
            this.XmlReader.AssertElementStart("aliases");

            List<ArtistAlias> aliases = new List<ArtistAlias>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("aliases"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("name"))
                {
                    string name = this.ReadName();
                    if (name != null)
                    {
                        aliases.Add(new ArtistAlias() { Name = name });
                    }
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return aliases.ToArray();
        }

        private string[] ReadNameVariations()
        {
            this.XmlReader.AssertElementStart("namevariations");

            List<string> nameVariations = new List<string>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("namevariations"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("name"))
                {
                    nameVariations.Add(this.ReadName());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return nameVariations.ToArray();
        }

        private string ReadName()
        {
            this.XmlReader.AssertElementStart("name");
            if (this.XmlReader.IsEmptyElement)
            {
                return null;
            }
            else
            {
                this.XmlReader.AssertRead();
                return this.XmlReader.ReadContentAsString();
            }
        }

        private string[] ReadUrls()
        {
            this.XmlReader.AssertElementStart("urls");

            List<string> urls = new List<string>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("urls"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("url"))
                {
                    urls.AddIfNotNullOrWhiteSpace(this.ReadUrl());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return urls.ToArray();
        }

        private string ReadUrl()
        {
            this.XmlReader.AssertElementStart("url");

            if (this.XmlReader.IsEmptyElement)
            {
                return null;
            }
            else
            {
                this.XmlReader.AssertRead();
                return this.XmlReader.ReadContentAsString();
            }
        }

        public Label ReadLabel()
        {
            this.XmlReader.AssertElementStart("label");

            Label label = new Label();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("label"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("images"))
                {
                    label.Images = this.ReadImages();
                }
                else if (this.XmlReader.IsElementStart("id"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        label.Id = int.Parse(this.XmlReader.ReadContentAsString());
                    }
                }
                else if (this.XmlReader.IsElementStart("name"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        label.Name = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("contactinfo"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        label.ContactInfo = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("profile"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        label.Profile = this.XmlReader.ReadContentAsString();
                    }
                }
                else if (this.XmlReader.IsElementStart("data_quality"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        label.DataQuality = DataReader.ParseDataQuality(this.XmlReader.ReadContentAsString());
                    }
                }
                else if (this.XmlReader.IsElementStart("urls"))
                {
                    label.Urls = this.ReadUrls();
                }
                else if (this.XmlReader.IsElementStart("sublabels"))
                {
                    label.Sublabels = this.ReadSublabels();
                }
                else if (this.XmlReader.IsElementStart("parentLabel"))
                {
                    if (!this.XmlReader.IsEmptyElement)
                    {
                        this.XmlReader.AssertRead();
                        label.ParentLabel = this.XmlReader.ReadContentAsString();
                    }
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return label;
        }

        private Sublabel[] ReadSublabels()
        {
            this.XmlReader.AssertElementStart("sublabels");

            List<Sublabel> sublabels = new List<Sublabel>();

            while (true)
            {
                this.XmlReader.AssertRead();

                if (this.XmlReader.IsElementEnd("sublabels"))
                {
                    break;
                }

                if (this.XmlReader.IsElementStart("label"))
                {
                    sublabels.Add(this.ReadSublabel());
                }
                else
                {
                    this.ThrowInvalidFormatException();
                }
            }

            return sublabels.ToArray();
        }

        private Sublabel ReadSublabel()
        {
            this.XmlReader.AssertElementStart("label");
            this.XmlReader.AssertNonEmptyElement();
            this.XmlReader.AssertRead();
            return new Sublabel()
            {
                Name = this.XmlReader.ReadContentAsString()
            };
        }

        private void ThrowInvalidFormatException()
        {
            throw new FormatException("Invalid or unsupported XML format.");
        }
    }
}
