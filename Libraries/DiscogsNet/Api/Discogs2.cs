using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using DiscogsNet.Model;
using System.Text;

namespace DiscogsNet.Api
{
    public class Discogs2
    {
        private static readonly string[] ProblematicChars = new string[] { "\u000B", "\u0010", "\u0003", "\u0007" };

        private string baseUrl;
        private WebClient webClient;

        /// <summary>
        /// This stores the X-RateLimit-Limit header returned by the last request.
        /// </summary>
        public int RateLimitLimit { get; set; }

        /// <summary>
        /// This stores the X-RateLimit-Remaining header returned by the last request.
        /// </summary>
        public int RateLimitRemaining { get; set; }

        /// <summary>
        /// This stores the X-RateLimit-Reset header returned by the last request.
        /// </summary>
        public int RateLimitReset { get; set; }

        public Discogs2()
        {
            this.baseUrl = "http://api.discogs.com/";

            this.webClient = new WebClient();
            this.webClient.Headers["Accept"] = "application/xml";
            this.webClient.Headers["Accept-Encoding"] = "gzip";
        }

        private void UpdateRateLimit()
        {
            string limit = this.webClient.ResponseHeaders["X-RateLimit-Limit"];
            string remaining = this.webClient.ResponseHeaders["X-RateLimit-Remaining"];
            string reset = this.webClient.ResponseHeaders["X-RateLimit-Reset"];

            this.RateLimitLimit = limit != null ? int.Parse(limit) : -1;
            this.RateLimitRemaining = remaining != null ? int.Parse(remaining) : -1;
            this.RateLimitReset = reset != null ? int.Parse(reset) : -1;
        }

        public Release GetRelease(int id)
        {
            string response = this.DownloadString(this.baseUrl + "release/" + id);
            this.UpdateRateLimit();
            using (StringReader stringReader = new StringReader(response))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    DataReader2 dataReader = new DataReader2(xmlReader);
                    dataReader.ReadResponseHeader();
                    return dataReader.ReadRelease();
                }
            }
        }

        public MasterRelease GetMasterRelease(int id)
        {
            string response = this.DownloadString(this.baseUrl + "master/" + id);
            this.UpdateRateLimit();
            using (StringReader stringReader = new StringReader(response))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    DataReader2 dataReader = new DataReader2(xmlReader);
                    dataReader.ReadResponseHeader();
                    return dataReader.ReadMasterRelease();
                }
            }
        }

        public Artist GetArtist(string artistName)
        {
            string escapedArtistName = Uri.EscapeDataString(artistName);
            string response = this.DownloadString(this.baseUrl + "artist/" + escapedArtistName);
            this.UpdateRateLimit();
            using (StringReader stringReader = new StringReader(response))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    DataReader2 dataReader = new DataReader2(xmlReader);
                    dataReader.ReadResponseHeader();
                    return dataReader.ReadArtist();
                }
            }
        }

        public Label GetLabel(string artistName)
        {
            string escapedArtistName = Uri.EscapeDataString(artistName);
            string response = this.DownloadString(this.baseUrl + "label/" + escapedArtistName);
            this.UpdateRateLimit();
            using (StringReader stringReader = new StringReader(response))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    DataReader2 dataReader = new DataReader2(xmlReader);
                    dataReader.ReadResponseHeader();
                    return dataReader.ReadLabel();
                }
            }
        }

        private string DownloadString(string uri)
        {
            byte[] data = this.webClient.DownloadData(uri);
            string response = Encoding.UTF8.GetString(data);
            foreach (string problematicChar in ProblematicChars)
            {
                response = response.Replace(problematicChar, "");
            }
            return response;
        }
    }
}
