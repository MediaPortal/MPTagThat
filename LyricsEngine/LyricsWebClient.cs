using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace LyricsEngine
{
  /// <summary>
  /// Some sites need to send cookies and a valid User AGent, so we use this class to enable a cookie container.
  /// </summary>
  class LyricsWebClient : WebClient
  {
    private CookieContainer cookieContainer;
    private string userAgent;
    private int timeout;

    public CookieContainer CookieContainer
    {
      get { return cookieContainer; }
      set { cookieContainer = value; }
    }

    public string UserAgent
    {
      get { return userAgent; }
      set { userAgent = value; }
    }

    public int Timeout
    {
      get { return timeout; }
      set { timeout = value; }
    }

    public LyricsWebClient()
    {
      timeout = -1;
      userAgent = @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
      cookieContainer = new CookieContainer();
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
      WebRequest request = base.GetWebRequest(address);

      if (request.GetType() == typeof(HttpWebRequest))
      {
        ((HttpWebRequest)request).CookieContainer = cookieContainer;
        ((HttpWebRequest)request).UserAgent = userAgent;
        ((HttpWebRequest)request).Timeout = timeout;
      }

      return request;
    }

  }
}
