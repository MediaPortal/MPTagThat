using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class LyrDb : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "LyrDb";

        // Base url
        private const string SiteBaseUrl = "http://webservices.lyrdb.com";

        // Agent token
        private const string Agent = "MediaPortal/MyLyrics";

        # endregion

        public LyrDb(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var artist = LyricUtil.RemoveFeatComment(Artist);
            var title = LyricUtil.TrimForParenthesis(Title);

            var urlString = string.Format(SiteBaseUrl + "/lookup.php?q={0}%7c{1}&for=match&agant={2}", artist, title, Agent);

            var client = new LyricsWebClient();
            var uri = new Uri(urlString);
            client.OpenReadCompleted += CallbackMethodSearch;
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
            return SiteComplexity.TwoSteps;
        }

        public override SiteSpeed GetSiteSpeed()
        {
            return SiteSpeed.VerySlow;
        }

        public override bool SiteActive()
        {
            return true;
        }

        public override string Name
        {
            get { return SiteName; }
        }

        public override string BaseUrl
        {
            get { return SiteBaseUrl; }
        }

        #endregion interface implemetation

        #region private methods

        private void CallbackMethodSearch(object sender, OpenReadCompletedEventArgs e)
        {
            Stream reply = null;
            StreamReader reader = null;

            try
            {
                reply = e.Result;
                reader = new StreamReader(reply, Encoding.UTF8);

                var result = reader.ReadToEnd();

                if (result.Equals(""))
                {
                    LyricText = NotFound;
                    return;
                }

                var id = result.Substring(0, result.IndexOf(@"\", StringComparison.Ordinal));

                var urlString = string.Format(BaseUrl + "/getlyr.php?q={0}", id);

                var client2 = new LyricsWebClient();

                var uri = new Uri(urlString);
                client2.OpenReadCompleted += CallbackMethodGetLyric;
                client2.OpenReadAsync(uri);

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
            }
        }

        private void CallbackMethodGetLyric(object sender, OpenReadCompletedEventArgs e)
        {
            Stream reply = null;
            StreamReader reader = null;

            try
            {
                reply = e.Result;
                reader = new StreamReader(reply, Encoding.UTF8);

                LyricText = reader.ReadToEnd().Trim();

                LyricText = LyricText.Replace("*", "");
                LyricText = LyricText.Replace("&amp;", "&");
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