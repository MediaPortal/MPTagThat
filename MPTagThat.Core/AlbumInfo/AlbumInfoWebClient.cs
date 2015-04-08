using System;
using System.Net;

namespace MPTagThat.Core.AlbumInfo
{
    /// <summary>
    /// Some sites need to send cookies and a valid User Agent, so we use this class to enable a cookie container.
    /// </summary>
    internal class AlbumInfoWebClient : WebClient
    {
        private readonly string _referer;


        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            if (response != null)
            {
                ResponseUri = response.ResponseUri;
            }
            return response;
        }

        public AlbumInfoWebClient()
        {
            Timeout = -1;
            UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0)";
            CookieContainer = new CookieContainer();
        }

        public AlbumInfoWebClient(string referer)
        {
            Timeout = -1;
            _referer = referer;
            CookieContainer = new CookieContainer();
        }

        public CookieContainer CookieContainer { get; set; }

        public string UserAgent { get; set; }

        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            if (request != null && request.GetType() == typeof (HttpWebRequest))
            {
                ((HttpWebRequest) request).CookieContainer = CookieContainer;
                ((HttpWebRequest) request).UserAgent = UserAgent;
                if (_referer != null)
                {
                    ((HttpWebRequest)request).Referer = _referer;
                }

                (request).Timeout = Timeout;
                (request).Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            return request;
        }

        public Uri ResponseUri { get; private set; }
    }
}