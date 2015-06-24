using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DiscogsNet.Model;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Web;

namespace DiscogsNet.User
{
    public class UserAPI
    {        
        private readonly string _AuthHeader;
        private readonly string _UserAgent;       
        private string baseUrl;

        public bool StrictReading { get; set; }
        public UserAPI(string useragent, string AuthHeader)
        {
            _UserAgent = useragent;
            _AuthHeader = AuthHeader;
            
            this.baseUrl = "http://api.discogs.com/";
        }
        public Identity verifyUser()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.baseUrl + "oauth/identity");
            request.Headers["Authorization"] = _AuthHeader;
            request.UserAgent = _UserAgent;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return new DataReader3(this.StrictReading).ReadIdentity(JObject.Parse(json));
        }
        public UserProfile GetUserInfo(string username)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.baseUrl + "users/" + username);
            request.Headers["Authorization"] = _AuthHeader;
            request.UserAgent = _UserAgent;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return new DataReader3(this.StrictReading).ReadUserProfile(JObject.Parse(json));
        }
        //Settings are not working, contacted Discogs to see what's the problem
       //public int ChangeSettings(string username, string setting, string value)
       // {
       //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.baseUrl + "users/" + username);
       //    request.Headers["Authorization"] = _AuthHeader;
       //    request.UserAgent = _UserAgent;
       //    request.Method = "POST";
       //    StringBuilder postData = new StringBuilder();
       //    postData.Append(String.Format("{0}={1}",setting,value));
       //    request.ContentType = "application/x-www-form-urlencoded";
       //    ASCIIEncoding ascii = new ASCIIEncoding();
       //    byte[] data = ascii.GetBytes(postData.ToString());           
       //    request.ContentLength = data.Length;
       //    Stream postStream = request.GetRequestStream();
       //    postStream.Write(data, 0, data.Length);
       //    postStream.Flush();
       //    postStream.Close();
       //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
       //    return (int)response.StatusCode;
       // }
    }
}
