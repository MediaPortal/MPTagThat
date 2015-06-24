using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth;

namespace DiscogsNet.User
{
    public class DiscogsAuth
    {
        private readonly string _UserAgent;
        private readonly string _ConsumerKey;
        private readonly string _ConsumerSecret;
        private OAuth.Manager OAuth;

        public DiscogsAuth(string UserAgent, string ConsumerKey, string ConsumerSecret)
        {
            _UserAgent = UserAgent;
            _ConsumerKey = ConsumerKey;
            _ConsumerSecret = ConsumerSecret;
        }
        public Dictionary<string,string> GrabTokens() {           
            return OAuth.ReturnTokens();
        }
        public string RegenerateHeaders(string token, string token_secret)
        {
            OAuth = new OAuth.Manager(_ConsumerKey,_ConsumerSecret,token,token_secret);
            var search = "http://api.discogs.com/oauth/identity";
            var authHeader = OAuth.GenerateAuthzHeader(search, "GET");
            return authHeader; 
        }
        public string AuthenticateUser()
        {
            OAuth = new OAuth.Manager();
            OAuth["consumer_key"] = _ConsumerKey;
            OAuth["consumer_secret"] = _ConsumerSecret;
            OAuthResponse requestToken = OAuth.AcquireRequestToken("http://api.discogs.com/oauth/request_token", "POST", _UserAgent);
            string url = "http://www.discogs.com/oauth/authorize?oauth_token=" + OAuth["token"];
            return url;
        }
        public string AuthorizeApp(string pin)
        {
            OAuthResponse accessToken = OAuth.AcquireAccessToken("http://api.discogs.com/oauth/access_token", "POST", pin, _UserAgent);
            var search = "http://api.discogs.com/oauth/identity";
            var authHeader = OAuth.GenerateAuthzHeader(search, "GET");
            return authHeader;
        }
    }
}
