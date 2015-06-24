using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class Actionext : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "Actionext";

        // Base url
        private const string SiteBaseUrl = "http://www.actionext.com";

        # endregion

        public Actionext(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            // clean artist
            var artist = LyricUtil.RemoveFeatComment(Artist);
            artist = artist.Replace(" ", "_");
            // Clean title
            var title = LyricUtil.TrimForParenthesis(Title);
            title = title.Replace(" ", "_");

            // Validation
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                return;
            }

            var urlString = SiteBaseUrl + "/names_" + artist[0] + "/" + artist + "_lyrics/" + title + ".html";
            urlString = urlString.ToLower();

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
                var noOfLinesCount = 0;

                while (line.IndexOf(@"<div class=""lyrics-text"">", StringComparison.Ordinal) == -1)
                {
                    if (reader.EndOfStream || ++noOfLinesCount > 300)
                    {
                        thisMayBeTheCorrectLyric = false;
                        break;
                    }
                    line = reader.ReadLine() ?? "";
                }

                if (thisMayBeTheCorrectLyric)
                {
                    var lyricTemp = new StringBuilder();
                    line = reader.ReadLine() ?? "";

                    while (line.IndexOf("</div>", StringComparison.Ordinal) == -1)
                    {
                        lyricTemp.Append(line);
                        if (reader.EndOfStream)
                        {
                            break;
                        }
                        line = reader.ReadLine() ?? "";
                    }

                    lyricTemp.Replace("<br>", Environment.NewLine);
                    lyricTemp.Replace(",<br />", Environment.NewLine);
                    lyricTemp.Replace("<br />", Environment.NewLine);
                    lyricTemp.Replace("&amp;", "&");

                    LyricText = lyricTemp.ToString().Trim();

                    if (LyricText.Contains("but we do not have the lyrics"))
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