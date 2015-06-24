using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class LyricWiki : AbstractSite
    {
        private const string SiteName = "LyricWiki";

        // Base url
        private const string SiteBaseUrl = "http://lyricwiki.org";

        const string StartString = "class='lyricbox' >";

        public LyricWiki(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit)
            : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            // Clean artist name
            var artist = LyricUtil.RemoveFeatComment(Artist);
            artist = LyricUtil.CapatalizeString(artist);
            artist = artist.Replace(" ", "_");

            // Clean title name
            var title = LyricUtil.TrimForParenthesis(Title);
            title = LyricUtil.CapatalizeString(title);
            title = title.Replace(" ", "_");
            title = title.Replace("?", "%3F");

            // Validate not empty
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                return;
            }

            var urlString = SiteBaseUrl + "/" + artist + ":" + title;

            var client = new LyricsWebClient();

            var uri = new Uri(urlString);
            client.OpenReadCompleted += CallbackMethod;
            client.OpenReadAsync(uri);

            while (Complete == false)
            {
                if (MEventStopSiteSearches.WaitOne(1, true))
                {
                    Complete = true;
                }
                else
                {
                    Thread.Sleep(300);
                }
            }
        }

        public override string Name
        {
            get { return SiteName; }
        }

        public override string BaseUrl
        {
            get { return SiteBaseUrl; }
        }

        public override LyricType GetLyricType()
        {
            return LyricType.UnsyncedLyrics;
        }

        public override SiteType GetSiteType()
        {
            return SiteType.Scrapper;
        }

        public override SiteComplexity GetSiteComplexity()
        {
            return SiteComplexity.OneStep;
        }

        public override SiteSpeed GetSiteSpeed()
        {
            return SiteSpeed.Fast;
        }

        public override bool SiteActive()
        {
            return false;
        }

        #endregion interface implemetation

        
        #region private methods

        private void CallbackMethod(object sender, OpenReadCompletedEventArgs e)
        {
            var thisMayBeTheCorrectLyric = true;

            Stream reply = null;
            StreamReader reader = null;

            try
            {
                reply = e.Result;
                reader = new StreamReader(reply, Encoding.UTF8);

                var line = "";

                while (line.IndexOf(StartString, StringComparison.Ordinal) == -1)
                {
                    if (reader.EndOfStream)
                    {
                        thisMayBeTheCorrectLyric = false;
                        break;
                    }
                    line = reader.ReadLine() ?? "";
                }

                if (thisMayBeTheCorrectLyric)
                {
                    var cutIndex = line.IndexOf(StartString, StringComparison.Ordinal);

                    if (cutIndex != -1)
                    {
                        LyricText = line.Substring(cutIndex + StartString.Length);
                    }

                    var iso8859 = Encoding.GetEncoding("ISO-8859-1");
                    LyricText = Encoding.UTF8.GetString(iso8859.GetBytes(LyricText));

                    LyricText = LyricText.Replace("<br />", "\r\n");
                    LyricText = LyricText.Replace("<i>", "");
                    LyricText = LyricText.Replace("</i>", "");
                    LyricText = LyricText.Replace("<b>", "");
                    LyricText = LyricText.Replace("</b>", "");
                    LyricText = LyricText.Replace("&amp;", "&");

                    LyricText = LyricText.Trim();

                    if (LyricText.Contains("<"))
                    {
                        LyricText = NotFound;
                    }
                }
                else
                {
                    LyricText = NotFound;
                }
            }
            catch
            {
                LyricText = NotFound;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (reply != null)
                {
                    reply.Close();
                }
                Complete = true;
            }
        }

        #endregion private methods
    }
}