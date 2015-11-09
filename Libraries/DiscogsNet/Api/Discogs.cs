using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using DiscogsNet.Model;
using DiscogsNet.Model.Obsolete;

namespace DiscogsNet.Api
{
    public class Discogs : IDiscogsApi
    {
        public string UserAgent { get; set; }
        public string ApiKey { get; set; }
        public int RequestCountToday { get; private set; }

        public Discogs(string apiKey)
        {
            this.UserAgent = "DiscogsNet Library";
            this.ApiKey = apiKey;
        }

        private string GetReponseString(HttpWebResponse response)
        {
            Stream responseStream = response.GetResponseStream();
            Stream readStream;
            if (response.ContentType == "text/xml; charset=utf-8" || response.ContentType == "application/xml; charset=utf-8")
            {
                readStream = responseStream;
            }
            else
            {
                readStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            using (StreamReader reader = new StreamReader(readStream))
            {
                return Utility.FixXmlText(reader.ReadToEnd());
            }
        }

        private XElement ProcessResponseDocument(XDocument document)
        {
            XElement root = document.Root;
            string stat = root.Attribute("stat").Value;
            if (stat == "ok")
            {
                this.RequestCountToday = int.Parse(root.Attribute("requests").Value);
                return document.Root;
            }
            else
            {
                throw new DiscogsApiException(root.Element("error").Attribute("msg").Value);
            }
        }

        private string GetQueryString(Dictionary<string, string> getArguments)
        {
            if (getArguments == null || getArguments.Count == 0)
            {
                return "";
            }

            return "?" +
                getArguments.Select(item => Uri.EscapeDataString(item.Key) + "=" + Uri.EscapeDataString(item.Value)).Join("&");
        }

        private HttpWebRequest CreateRequest(string relativeUrl, Dictionary<string, string> getArguments = null)
        {
            if (getArguments == null)
            {
                getArguments = new Dictionary<string, string>();
            }

            getArguments["f"] = "xml";
            getArguments["api_key"] = this.ApiKey;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.discogs.com/" + relativeUrl + this.GetQueryString(getArguments));
            request.Headers["Accept-Encoding"] = "gzip";
            request.UserAgent = this.UserAgent;
            return request;
        }

        public Release GetRelease(int releaseId)
        {
            HttpWebRequest request = this.CreateRequest("release/" + releaseId);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = this.GetReponseString(response);
            return this.ParseGetReleaseResponse(responseString);
        }

        public Release ParseGetReleaseResponse(string getReleaseResponse)
        {
            XDocument response = XDocument.Parse(getReleaseResponse);
            XElement releaseElement = this.ProcessResponseDocument(response).Elements().First();
            return DataReader.ReadRelease(releaseElement);
        }

        public Artist GetArtist(string artistName)
        {
            string escapedArtistName = Uri.EscapeDataString(artistName);

            HttpWebRequest request = this.CreateRequest("artist/" + escapedArtistName);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = this.GetReponseString(response);
            return this.ParseGetArtistResponse(responseString);
        }

        public Artist ParseGetArtistResponse(string getArtistResponse)
        {
            XDocument response = XDocument.Parse(getArtistResponse);
            XElement releaseElement = this.ProcessResponseDocument(response).Elements().First();
            return DataReader.ReadArtist(releaseElement);
        }

        public Label GetLabel(string labelName)
        {
            string escapedLabelName = Uri.EscapeDataString(labelName);

            HttpWebRequest request = this.CreateRequest("label/" + escapedLabelName);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = this.GetReponseString(response);
            return this.ParseGetLabelResponse(responseString);
        }

        public Label ParseGetLabelResponse(string getLabelResponse)
        {
            XDocument response = XDocument.Parse(getLabelResponse);
            XElement releaseElement = this.ProcessResponseDocument(response).Elements().First();
            return DataReader.ReadLabel(releaseElement);
        }

        public SearchResults_Obsolete Search(string searchString)
        {
            Dictionary<string, string> getArguments = new Dictionary<string, string>() {
                {"type", "all"},
                {"q", searchString}
            };
            HttpWebRequest request = this.CreateRequest("search", getArguments);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = this.GetReponseString(response);
            return this.ParseSearchResponse(responseString);
        }

        public SearchResults_Obsolete ParseSearchResponse(string searchResponse)
        {
            XDocument response = XDocument.Parse(searchResponse);
            XElement searchElement = this.ProcessResponseDocument(response);
            return DataReader.ReadSearchResults(searchElement);
        }
    }
}
