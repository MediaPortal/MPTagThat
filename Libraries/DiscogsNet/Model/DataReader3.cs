using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using DiscogsNet.Model.Search;

namespace DiscogsNet.Model
{
    public class DataReader3
    {
        public bool Strict { get; set; }

        public DataReader3(bool strict)
        {
            this.Strict = strict;
        }

        private void ThrowIfStrict(string message)
        {
            if (this.Strict)
            {
                throw new Exception(message);
            }
        }

        public Release ReadRelease(JObject source)
        {
            Release release = new Release();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                        release.Id = v.Value<int>();
                        break;
                    case "status":
                        release.Status = DataReader.ParseReleaseStatus(v.Value<string>());
                        break;
                    case "videos":
                        release.Videos = this.ReadReleaseVideos(v.Value<JArray>());
                        break;
                    case "series":
                        if (((JArray)v).Count != 0)
                        {
                            this.ThrowIfStrict("Unable to process \"series\" item.");
                        }
                        break;
                    case "labels":
                        release.Labels = this.ReadReleaseLabels(v.Value<JArray>());
                        break;
                    case "year":
                        // We skip year. Can extract it from release date.
                        break;
                    case "images":
                        release.Images = this.ReadImages(v.Value<JArray>());
                        break;
                    case "genres":
                        release.Genres = v.ValueAsStringArray();
                        break;
                    case "thumb":
                        // We skip thumb. Can extract it from images.
                        break;
                    case "extraartists":
                        release.ExtraArtists = this.ReadExtraArtists(v.Value<JArray>());
                        break;
                    case "title":
                        release.Title = v.Value<string>();
                        break;
                    case "artists":
                        release.Artists = this.ReadReleaseArtists(v.Value<JArray>());
                        break;
                    case "master_id":
                        release.MasterId = v.Value<int>();
                        break;
                    case "tracklist":
                        release.Tracklist = this.ReadReleaseTracklist(v.Value<JArray>());
                        break;
                    case "styles":
                        release.Styles = v.ValueAsStringArray();
                        break;
                    case "released_formatted":
                        // We skip released_formatted. Can extract it from released.
                        break;
                    case "released":
                        release.ReleaseDate = v.Value<string>();
                        break;
                    case "master_url":
                        // We skip master_url. Can extract it from master_id.
                        break;
                    case "country":
                        release.Country = v.Value<string>();
                        break;
                    case "notes":
                        release.Notes = v.Value<string>();
                        break;
                    case "companies":
                        release.Compaines = this.ReadCompanies(v.Value<JArray>());
                        break;
                    case "uri":
                        // TODO: implement model for this
                        break;
                    case "formats":
                        release.Formats = this.ReadReleaseFormats(v.Value<JArray>());
                        break;
                    case "resource_url":
                        break;
                    case "data_quality":
                        release.DataQuality = DataReader.ParseDataQuality(v.Value<string>());
                        break;
                    case "identifiers":
                        release.Identifiers = this.ReadReleaseIdentifiers(v.Value<JArray>());
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return release;
        }
        private Companies[] ReadCompanies(JArray source)
        {
            List<Companies> companie = new List<Companies>();
            foreach (JObject item in source)
            {
                companie.Add(this.ReadCompanie(item));
            }
            return companie.ToArray();
           
        }
        private Companies ReadCompanie(JObject source)
        {            
            Companies companie = new Companies();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                        companie.Id = v.Value<int>();
                        break;
                    case "name":
                        companie.Name = v.Value<string>();
                        break;
                    case "catno":
                        companie.CategoryNo = v.Value<string>();
                        break;
                    case "entity_type":
                        companie.EntityType = v.Value<int>();
                        break;
                    case "entity_type_name":
                        companie.EntityTypeName = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return companie;
        }
        private ReleaseIdentifier[] ReadReleaseIdentifiers(JArray source)
        {
            List<ReleaseIdentifier> result = new List<ReleaseIdentifier>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadReleaseIdentifier(item));
            }
            return result.ToArray();
        }

        private ReleaseIdentifier ReadReleaseIdentifier(JObject source)
        {
            ReleaseIdentifier releaseIdentifier = new ReleaseIdentifier();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "type":
                        releaseIdentifier.Type = DataReader.ParseReleaseIdentifierType(v.Value<string>());
                        break;
                    case "value":
                        releaseIdentifier.Value = v.Value<string>();
                        break;
                    case "description":
                        releaseIdentifier.Description = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return releaseIdentifier;
        }

        private ReleaseFormat[] ReadReleaseFormats(JArray source)
        {
            List<ReleaseFormat> result = new List<ReleaseFormat>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadReleaseFormat(item));
            }
            return result.ToArray();
        }

        private ReleaseFormat ReadReleaseFormat(JObject source)
        {
            ReleaseFormat releaseFormat = new ReleaseFormat();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "qty":
                        releaseFormat.Quantity = v.Value<int>();
                        break;
                    case "descriptions":
                        releaseFormat.Descriptions = v.ValueAsStringArray();
                        break;
                    case "name":
                        releaseFormat.Name = v.Value<string>();
                        break;
                    case "text":
                        releaseFormat.Text = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return releaseFormat;
        }

        private Track[] ReadReleaseTracklist(JArray source)
        {
            List<Track> result = new List<Track>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadReleaseTrack(item));
            }
            return result.ToArray();
        }

        private Track ReadReleaseTrack(JObject source)
        {
            Track track = new Track();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "duration":
                        track.Duration = v.Value<string>();
                        break;
                    case "position":
                        track.Position = v.Value<string>();
                        break;
                    case "title":
                        track.Title = v.Value<string>();
                        break;
                    case "artists":
                        track.Artists = this.ReadReleaseArtists(v.Value<JArray>());
                        break;
                    case "extraartists":
                        track.ExtraArtists = this.ReadExtraArtists(v.Value<JArray>());
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return track;
        }

        private ReleaseArtist[] ReadReleaseArtists(JArray source)
        {
            List<ReleaseArtist> result = new List<ReleaseArtist>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadReleaseArtist(item));
            }
            return result.ToArray();
        }

        private ReleaseArtist ReadReleaseArtist(JObject source)
        {
            ReleaseArtist releaseArtist = new ReleaseArtist();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "join":
                        releaseArtist.Join = v.Value<string>();
                        break;
                    case "anv":
                        releaseArtist.NameVariation = v.Value<string>();
                        break;
                    case "name":
                        releaseArtist.Name = v.Value<string>();
                        break;
                    case "id":
                        releaseArtist.Id = v.Value<int>();
                        break;
                    case "role":
                        releaseArtist.Role = v.Value<string>();
                        break;
                    case "resource_url":
                        break;
                    case "tracks":
                        releaseArtist.Tracks = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return releaseArtist;
        }

        private ExtraArtist[] ReadExtraArtists(JArray source)
        {
            List<ExtraArtist> result = new List<ExtraArtist>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadExtraArtist(item));
            }
            return result.ToArray();
        }

        private ExtraArtist ReadExtraArtist(JObject source)
        {
            ExtraArtist extraArtist = new ExtraArtist();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "join":
                        extraArtist.Join = v.Value<string>();
                        break;
                    case "name":
                        extraArtist.Name = v.Value<string>();
                        break;
                    case "anv":
                        extraArtist.NameVariation = v.Value<string>();
                        break;
                    case "tracks":
                        extraArtist.Tracks = v.Value<string>();
                        break;
                    case "role":
                        extraArtist.Role = v.Value<string>();
                        break;
                    case "resource_url":
                        break;
                    case "id":
                        extraArtist.Id = v.Value<int>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return extraArtist;
        }

        private Image[] ReadImages(JArray source)
        {
            List<Image> result = new List<Image>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadReleaseImage(item));
            }
            return result.ToArray();
        }

        private Image ReadReleaseImage(JObject source)
        {
            Image image = new Image();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "uri":
                        image.Uri = v.Value<string>();
                        break;
                    case "uri150":
                        image.Uri150 = v.Value<string>();
                        break;
                    case "width":
                        image.Width = v.Value<int>();
                        break;
                    case "height":
                        image.Height = v.Value<int>();
                        break;
                    case "resource_url":
                        break;
                    case "type":
                        image.Type = DataReader.ParseImageType(v.Value<string>());
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return image;
        }

        private ReleaseLabel[] ReadReleaseLabels(JArray source)
        {
            List<ReleaseLabel> result = new List<ReleaseLabel>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadReleaseLabel(item));
            }
            return result.ToArray();
        }

        private ReleaseLabel ReadReleaseLabel(JObject source)
        {
            ReleaseLabel releaseLabel = new ReleaseLabel();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "name":
                        releaseLabel.Name = v.Value<string>();
                        break;
                    case "entity_type":
                        break;
                    case "entity_type_name":
                        if (v.Value<string>() != "Label")
                        {
                            this.ThrowIfStrict("Unknown entity type name.");
                        }
                        break;
                    case "catno":
                        releaseLabel.CatalogNumber = v.Value<string>();
                        break;
                    case "id":
                        releaseLabel.Id = v.Value<int>();
                        break;
                    case "resource_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return releaseLabel;
        }

        private ReleaseVideo[] ReadReleaseVideos(JArray source)
        {
            List<ReleaseVideo> result = new List<ReleaseVideo>();
            foreach (JObject item in source)
            {
                result.Add(this.ReadReleaseVideo(item));
            }
            return result.ToArray();
        }

        private ReleaseVideo ReadReleaseVideo(JObject source)
        {
            ReleaseVideo releaseVideo = new ReleaseVideo();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "duration":
                        releaseVideo.Duration = v.Value<int>();
                        break;
                    case "embed":
                        releaseVideo.Embed = v.Value<bool>();
                        break;
                    case "title":
                        releaseVideo.Title = v.Value<string>();
                        break;
                    case "description":
                        releaseVideo.Description = v.Value<string>();
                        break;
                    case "uri":
                        releaseVideo.Src = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return releaseVideo;
        }

        public Members ReadMembers(JObject source)
        {
            Members member = new Members();
            foreach (var item in source)
            {
                var v = item.Value;
                switch(item.Key)
                {
                    case "active":
                        member.Active = v.Value<bool>();
                        break;
                    case "resource_url":
                        member.ResourceURL = v.Value<string>();
                        break;
                    case "id":
                        member.Id = v.Value<int>();
                        break;
                    case "name":
                        member.Name = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return member;
        }
        public Artist ReadArtist(JObject source)
        {
            Artist artist = new Artist();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                        artist.Id = v.Value<int>();
                        break;
                    case "name":
                        artist.Name = v.Value<string>();
                        break;
                    case "profile":
                        artist.Profile = v.Value<string>();
                        break;
                    case "realname":
                        artist.RealName = v.Value<string>();
                        break;
                    case "releases_url":
                        break;
                    case "urls":
                        artist.Urls = v.ValueAsStringArray();
                        break;
                    case "images":
                        artist.Images = this.ReadImages(v.Value<JArray>());
                        break;
                    case "aliases":
                        artist.Aliases = v.Value<JArray>().Cast<JObject>().Select(t => this.ReadArtistAlias(t)).ToArray();
                        break;
                    case "members":
                        artist.Members = v.Value<JArray>().Cast<JObject>().Select(t => this.ReadMembers(t)).ToArray();
                        break;
                    case "uri":
                        break;
                    case "resource_url":
                        break;
                    case "data_quality":
                        artist.DataQuality = DataReader.ParseDataQuality(v.Value<string>());
                        break;
                    case "namevariations":
                        artist.NameVariations = v.ValueAsStringArray();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }

            return artist;
        }

        private ArtistAlias ReadArtistAlias(JObject source)
        {
            ArtistAlias artistAlias = new ArtistAlias();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                        artistAlias.Id = v.Value<int>();
                        break;
                    case "name":
                        artistAlias.Name = v.Value<string>();
                        break;
                    case "resource_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return artistAlias;
        }

        public MasterRelease ReadMasterRelease(JObject source)
        {
            MasterRelease master = new MasterRelease();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                        master.Id = v.Value<int>();
                        break;
                    case "videos":
                        master.Videos = this.ReadReleaseVideos(v.Value<JArray>());
                        break;
                    case "series":
                        if (((JArray)v).Count != 0)
                        {
                            this.ThrowIfStrict("Unable to process \"series\" item.");
                        }
                        break;
                    case "year":
                        // We skip year. Can extract it from release date.
                        break;
                    case "images":
                        master.Images = this.ReadImages(v.Value<JArray>());
                        break;
                    case "genres":
                        master.Genres = v.ValueAsStringArray();
                        break;
                    case "thumb":
                        // We skip thumb. Can extract it from images.
                        break;
                    case "title":
                        master.Title = v.Value<string>();
                        break;
                    case "artists":
                        master.Artists = this.ReadReleaseArtists(v.Value<JArray>());
                        break;
                    case "tracklist":
                        master.Tracklist = this.ReadReleaseTracklist(v.Value<JArray>());
                        break;
                    case "styles":
                        master.Styles = v.ValueAsStringArray();
                        break;
                    case "released_formatted":
                        // We skip released_formatted. Can extract it from released.
                        break;
                    case "master_url":
                        // We skip master_url. Can extract it from master_id.
                        break;
                    case "notes":
                        master.Notes = v.Value<string>();
                        break;
                    case "companies":
                        // TODO: implement model for this
                        break;
                    case "uri":
                        // TODO: implement model for this
                        break;
                    case "resource_url":
                        break;
                    case "data_quality":
                        master.DataQuality = DataReader.ParseDataQuality(v.Value<string>());
                        break;
                    case "main_release":
                        master.MainRelease = v.Value<int>();
                        break;
                    case "main_release_url":
                        break;
                    case "versions_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return master;
        }

        public ArtistReleases ReadArtistReleases(JObject source)
        {
            ArtistReleases artistReleases = new ArtistReleases();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "pagination":
                        artistReleases.Pagination = this.ReadPaginationInfo(v.Value<JObject>());
                        break;
                    case "releases":
                        artistReleases.Releases = v.Value<JArray>().Cast<JObject>().Select(t => this.ReadReleaseVersion(t)).ToArray();
                        break;
                    case "resource_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return artistReleases;
        }

        private ReleaseVersion ReadReleaseVersion(JObject source)
        {
            ReleaseVersion version = new ReleaseVersion();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "thumb":
                        version.Thumb = v.Value<string>();
                        break;
                    case "title":
                        version.Title = v.Value<string>();
                        break;
                    case "main_release":
                        version.MainRelease = v.Value<int>();
                        break;
                    case "role":
                        version.Role = v.Value<string>();
                        break;
                    case "year":
                        version.Year = v.Value<int>();
                        break;
                    case "type":
                        version.Type = ParseReleaseVersionType(v.Value<string>());
                        break;
                    case "id":
                        version.Id = v.Value<int>();
                        break;
                    case "label":
                        version.Label = v.Value<string>();
                        break;
                    case "format":
                        version.Format = v.Value<string>();
                        break;
                    case "status":
                        version.Status = DataReader.ParseReleaseStatus(v.Value<string>());
                        break;
                    case "trackinfo":
                        version.TrackInfo = v.Value<string>();
                        break;
                    case "resource_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return version;
        }

        private static ReleaseVersionType ParseReleaseVersionType(string value)
        {
            switch (value)
            {
                case "master":
                    return ReleaseVersionType.MasterRelease;
                case "release":
                    return ReleaseVersionType.Release;
                default:
                    throw new Exception("Unknown release version type: " + value);
            }
        }

        private PaginationInfo ReadPaginationInfo(JObject source)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "per_page":
                        paginationInfo.PerPage = v.Value<int>();
                        break;
                    case "pages":
                        paginationInfo.Pages = v.Value<int>();
                        break;
                    case "page":
                        paginationInfo.Page = v.Value<int>();
                        break;
                    case "items":
                        paginationInfo.Items = v.Value<int>();
                        break;
                    case "urls":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return paginationInfo;
        }

        public Label ReadLabel(JObject source)
        {
            Label label = new Label();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                        label.Id = v.Value<int>();
                        break;
                    case "profile":
                        label.Profile = v.Value<string>();
                        break;
                    case "releases_url":
                        break;
                    case "name":
                        label.Name = v.Value<string>();
                        break;
                    case "contact_info":
                        label.ContactInfo = v.Value<string>();
                        break;
                    case "uri":
                        label.Uri = v.Value<string>();
                        break;
                    case "sublabels":
                        label.Sublabels = v.Value<JArray>().Cast<JObject>().Select(t => this.ReadSublabel(t)).ToArray();
                        break;
                    case "urls":
                        label.Urls = v.ValueAsStringArray();
                        break;
                    case "images":
                        label.Images = this.ReadImages(v.Value<JArray>());
                        break;
                    case "resource_url":
                        break;
                    case "data_quality":
                        label.DataQuality = DataReader.ParseDataQuality(v.Value<string>());
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return label;
        }

        public Sublabel ReadSublabel(JObject source)
        {
            Sublabel label = new Sublabel();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                        label.Id = v.Value<int>();
                        break;
                    case "name":
                        label.Name = v.Value<string>();
                        break;
                    case "resource_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return label;
        }


        public SearchResults ReadSearchResults(JObject source)
        {
            SearchResults searchResults = new SearchResults();
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "pagination":
                        searchResults.Pagination = this.ReadPaginationInfo(v.Value<JObject>());
                        break;
                    case "results":
                        searchResults.Results = v.Value<JArray>().Cast<JObject>().Select(t => this.ReadSearchResult(t)).Where(t => t != null).ToArray();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return searchResults;
        }

        private SearchResult ReadSearchResult(JObject source)
        {
            SearchItemType? type = null;

            foreach (var item in source)
            {
                if (item.Key == "type")
                {
                    switch (item.Value.Value<string>())
                    {
                        case "artist":
                            type = SearchItemType.Artist;
                            goto foreachEnd;
                        case "label":
                            type = SearchItemType.Label;
                            goto foreachEnd;
                        case "release":
                            type = SearchItemType.Release;
                            goto foreachEnd;
                        case "master":
                            type = SearchItemType.Master;
                            goto foreachEnd;
                    }
                }
            }

        foreachEnd:
            if (type == null && this.Strict)
            {
                throw new FormatException("Unknown search result type: " + type);
            }

            SearchResult result;

            switch (type.Value)
            {
                case SearchItemType.Artist:
                    result = this.ReadArtistSearchResult(source);
                    break;
                case SearchItemType.Label:
                    result = this.ReadLabelSearchResult(source);
                    break;
                case SearchItemType.Release:
                    result = this.ReadReleaseSearchResult(source);
                    break;
                case SearchItemType.Master:
                    result = this.ReadMasterSearchResult(source);
                    break;
                default:
                    this.ThrowIfStrict("Unknown SearchItemType: " + type.Value);
                    return null;
            }

            result.Type = type.Value;

            this.ReadSearchResultData(result, source);

            return result;
        }

        private void ReadSearchResultData(SearchResult result, JObject source)
        {
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "thumb":
                        result.Thumb = v.Value<string>();
                        break;
                    case "title":
                        result.Title = v.Value<string>();
                        break;
                    case "id":
                        result.Id = v.Value<int>();
                        break;
                }
            }
        }

        private void ReadReleaseBaseSearchResultData(ReleaseBaseSearchResult result, JObject source)
        {
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "style":
                        result.Styles = v.ValueAsStringArray();
                        break;
                    case "year":
                        int year;
                        if (int.TryParse(v.Value<string>(), out year))
                        {
                            result.Year = year;
                        }
                        break;
                    case "genre":
                        result.Genres = v.ValueAsStringArray();
                        break;
                }
            }
        }

        private MasterReleaseSearchResult ReadMasterSearchResult(JObject source)
        {
            MasterReleaseSearchResult masterResult = new MasterReleaseSearchResult();
            this.ReadReleaseBaseSearchResultData(masterResult, source);
            foreach (var item in source)
            {
                switch (item.Key)
                {
                    case "id":
                    case "thumb":
                    case "title":
                    case "type":
                    case "uri":
                    case "resource_url":
                        break;
                    case "style":
                    case "year":
                    case "genre":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return masterResult;
        }

        private ReleaseSearchResult ReadReleaseSearchResult(JObject source)
        {
            ReleaseSearchResult releaseResult = new ReleaseSearchResult();
            this.ReadReleaseBaseSearchResultData(releaseResult, source);
            foreach (var item in source)
            {
                var v = item.Value;

                switch (item.Key)
                {
                    case "id":
                    case "thumb":
                    case "title":
                    case "type":
                    case "uri":
                    case "resource_url":
                        break;
                    case "style":
                    case "year":
                    case "genre":
                        break;
                    case "format":
                        releaseResult.Formats = v.ValueAsStringArray();
                        break;
                    case "country":
                        releaseResult.Country = v.Value<string>();
                        break;
                    case "label":
                        releaseResult.Label = v.ValueAsStringArray();
                        break;
                    case "catno":
                        releaseResult.CatalogNumber = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return releaseResult;
        }

        private LabelSearchResult ReadLabelSearchResult(JObject source)
        {
            LabelSearchResult labelResult = new LabelSearchResult();
            foreach (var item in source)
            {
                switch (item.Key)
                {
                    case "id":
                    case "thumb":
                    case "title":
                    case "type":
                    case "uri":
                    case "resource_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return labelResult;
        }

        private ArtistSearchResult ReadArtistSearchResult(JObject source)
        {
            ArtistSearchResult artistResult = new ArtistSearchResult();
            foreach (var item in source)
            {
                switch (item.Key)
                {
                    case "id":
                    case "thumb":
                    case "title":
                    case "type":
                    case "uri":
                    case "resource_url":
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return artistResult;
        }
        public Identity ReadIdentity(JObject source)
        {
            Identity OAuthIdentity = new Identity();
            foreach(var item in source)
            {
                var v = item.Value;
                switch(item.Key)
                {
                    case "id":
                        OAuthIdentity.Id = v.Value<int>();
                        break;
                    case "username":
                        OAuthIdentity.username = v.Value<string>();
                        break;
                    case "resource_url":
                        OAuthIdentity.resource_url = v.Value<string>();
                        break;
                    case "consumer_name":
                        OAuthIdentity.consumer_name = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return OAuthIdentity;
        }
        public UserProfile ReadUserProfile(JObject source)
        {
            UserProfile userProfile = new UserProfile();
            foreach(var item in source)
            {
                var v = item.Value;
                switch(item.Key)
                {
                    case "id":
                        userProfile.Id = v.Value<int>();
                        break;
                    case "username":
                        userProfile.username = v.Value<string>();
                        break;
                    case "resource_url":
                        userProfile.resource_url = v.Value<string>();
                        break;
                    case "inventory_url":
                        userProfile.inventory_url = v.Value<string>();
                        break;
                    case "collection_folders_url":
                        userProfile.collection_folders_url = v.Value<string>();
                        break;
                    case "collection_fields_url":
                        userProfile.collection_fields_url = v.Value<string>();
                        break;
                    case "wantlist_url":
                        userProfile.wantlist_url = v.Value<string>();
                        break;
                    case "uri":
                        userProfile.uri = v.Value<string>();
                        break;
                    case "name":
                        userProfile.name = v.Value<string>();
                        break;
                    case "email":
                        userProfile.email = v.Value<string>();
                        break;
                    case "profile":
                        userProfile.profile = v.Value<string>();
                        break;
                    case "home_page":
                        userProfile.home_page = v.Value<string>();
                        break;
                    case "location":
                        userProfile.location = v.Value<string>();
                        break;
                    case "registered":
                        userProfile.registered = v.Value<string>();
                        break;
                    case "num_lists":
                        userProfile.num_lists = v.Value<int>();
                        break;
                    case "num_for_sale":
                        userProfile.num_for_sale = v.Value<int>();
                        break;
                    case "num_collection":
                        userProfile.num_collection = v.Value<int>();
                        break;
                    case "num_wantlist":
                        userProfile.num_wantlist = v.Value<int>();
                        break;
                    case "num_pending":
                        userProfile.num_pending = v.Value<int>();
                        break;
                    case "releases_contributed":
                        userProfile.releases_contributed = v.Value<int>();
                        break;
                    case "rank":
                        userProfile.rank = v.Value<string>();
                        break;
                    case "releases_rated":
                        userProfile.releases_rated = v.Value<int>();
                        break;
                    case "rating_avg":
                        userProfile.rating_avg = v.Value<string>();
                        break;
                    default:
                        this.ThrowIfStrict("Unknown key: " + item.Key);
                        break;
                }
            }
            return userProfile;
        }
    }
}
