using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class HotLyrics : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "HotLyrics";

        // Base url
        private const string SiteBaseUrl = "http://www.hotlyrics.net";

        # endregion

        public HotLyrics(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var artist = LyricUtil.RemoveFeatComment(Artist);
            artist = LyricUtil.CapatalizeString(artist);

            artist = artist.Replace(" ", "_");
            artist = artist.Replace(",", "_");
            artist = artist.Replace(".", "_");
            artist = artist.Replace("'", "_");
            artist = artist.Replace("(", "%28");
            artist = artist.Replace(")", "%29");
            artist = artist.Replace(",", "");
            artist = artist.Replace("#", "");
            artist = artist.Replace("%", "");
            artist = artist.Replace("+", "%2B");
            artist = artist.Replace("=", "%3D");
            artist = artist.Replace("-", "_");

            // French letters
            artist = artist.Replace("é", "%E9");

            var title = LyricUtil.TrimForParenthesis(Title);
            title = LyricUtil.CapatalizeString(title);

            title = title.Replace(" ", "_");
            title = title.Replace(",", "_");
            title = title.Replace(".", "_");
            title = title.Replace("'", "_");
            title = title.Replace("(", "%28");
            title = title.Replace(")", "%29");
            title = title.Replace(",", "_");
            title = title.Replace("#", "_");
            title = title.Replace("%", "_");
            title = title.Replace("?", "_");
            title = title.Replace("+", "%2B");
            title = title.Replace("=", "%3D");
            title = title.Replace("-", "_");
            title = title.Replace(":", "_");

            // German letters
            artist = artist.Replace("ü", "%FC");
            artist = artist.Replace("Ü", "%DC");
            artist = artist.Replace("ä", "%E4");
            artist = artist.Replace("Ä", "%C4");
            artist = artist.Replace("ö", "%F6");
            artist = artist.Replace("Ö", "%D6");
            artist = artist.Replace("ß", "%DF");

            // Danish letters
            title = title.Replace("å", "%E5");
            title = title.Replace("Å", "%C5");
            title = title.Replace("æ", "%E6");
            title = title.Replace("ø", "%F8");

            // French letters
            title = title.Replace("é", "%E9");

            // Validation
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                return;
            }

            var firstLetter = artist[0].ToString(CultureInfo.InvariantCulture);

            var urlString = SiteBaseUrl + "/lyrics/" + firstLetter + "/" + artist + "/" + title + ".html";

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
            return false;
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
                reader = new StreamReader(reply, Encoding.Default);

                var line = "";

                while (line.IndexOf("GOOGLE END", StringComparison.Ordinal) == -1)
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
                    var lyricTemp = new StringBuilder();
                    line = reader.ReadLine() ?? "";

                    while (line.IndexOf("<script type", StringComparison.Ordinal) == -1)
                    {
                        lyricTemp.Append(line);
                        if (reader.EndOfStream)
                        {
                            break;
                        }
                        line = reader.ReadLine() ?? "";
                    }

                    lyricTemp.Replace("?s", "'s");
                    lyricTemp.Replace("?t", "'t");
                    lyricTemp.Replace("?m", "'m");
                    lyricTemp.Replace("?l", "'l");
                    lyricTemp.Replace("?v", "'v");
                    lyricTemp.Replace("<br>", "\r\n");
                    lyricTemp.Replace("<br />", "\r\n");
                    lyricTemp.Replace("&quot;", "\"");
                    lyricTemp.Replace("</p>", "");
                    lyricTemp.Replace("<BR>", "");
                    lyricTemp.Replace("<br/>", "\r\n");
                    lyricTemp.Replace("&amp;", "&");

                    LyricText = lyricTemp.ToString().Trim();

                    if (LyricText.Contains("<td"))
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