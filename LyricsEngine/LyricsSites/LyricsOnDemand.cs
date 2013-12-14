using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class LyricsOnDemand : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "LyricsOnDemand";

        // Base url
        private const string SiteBaseUrl = "http://www.lyricsondemand.com";

        # endregion

        public LyricsOnDemand(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var artist = LyricUtil.RemoveFeatComment(Artist);
            artist = LyricUtil.DeleteSpecificChars(artist);
            artist = artist.Replace(" ", "");
            artist = artist.Replace("The ", "");
            artist = artist.Replace("the ", "");
            artist = artist.Replace("-", "");

            artist = artist.ToLower();

            // Cannot find lyrics contaning non-English letters!

            var title = LyricUtil.TrimForParenthesis(Title);
            title = LyricUtil.DeleteSpecificChars(title);
            title = title.Replace(" ", "");
            title = title.Replace("#", "");
            artist = artist.Replace("-", "");

            // Danish letters
            title = title.Replace("æ", "");
            title = title.Replace("ø", "");
            title = title.Replace("å", "");
            title = title.Replace("Æ", "");
            title = title.Replace("Ø", "");
            title = title.Replace("Å", "");
            title = title.Replace("ö", "");
            title = title.Replace("Ö", "");

            title = title.ToLower();

            // Validation
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                return;
            }

            var firstLetter = artist[0].ToString(CultureInfo.InvariantCulture);

            int firstNumber;
            if (int.TryParse(firstLetter, out firstNumber))
            {
                firstLetter = "0";
            }

            var urlString = SiteBaseUrl + "/" + firstLetter + "/" + artist + "lyrics/" + title + "lyrics.html";

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
                    Thread.Sleep(100);
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
            return SiteSpeed.Fast;
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
                reader = new StreamReader(reply, Encoding.Default);

                var line = "";
                var noOfLinesCount = 0;

                while (line.IndexOf(@"<font size=""2"" face=""Verdana"">", StringComparison.Ordinal) == -1)
                {
                    if (reader.EndOfStream || ++noOfLinesCount > 300)
                    {
                        thisMayBeTheCorrectLyric = false;
                        break;
                    }
                    line = (reader.ReadLine() ?? "").Trim();
                }

                if (thisMayBeTheCorrectLyric)
                {
                    var lyricTemp = new StringBuilder();
                    line = (reader.ReadLine() ?? "").Trim();

                    while (!line.StartsWith("<script") && !line.StartsWith("<!--"))
                    {
                        lyricTemp.Append(line);
                        if (reader.EndOfStream || ++noOfLinesCount > 300)
                        {
                            break;
                        }
                        line = (reader.ReadLine() ?? "").Trim();
                    }

                    lyricTemp.Replace("<br>", " \r\n");
                    lyricTemp.Replace("</font></p>", " \r\n");
                    lyricTemp.Replace("<p><font size=\"2\" face=\"Verdana\">", " \r\n");
                    lyricTemp.Replace("</p>", "");
                    lyricTemp.Replace("<p>", "");
                    lyricTemp.Replace("<i>", "");
                    lyricTemp.Replace("</i>", "");
                    lyricTemp.Replace("*", "");
                    lyricTemp.Replace("?s", "'s");
                    lyricTemp.Replace("?t", "'t");
                    lyricTemp.Replace("?m", "'m");
                    lyricTemp.Replace("?l", "'l");
                    lyricTemp.Replace("?v", "'v");
                    lyricTemp.Replace("<p>", " \r\n");
                    lyricTemp.Replace("<BR>", " \r\n");
                    lyricTemp.Replace("<br />", " \r\n");
                    lyricTemp.Replace("&#039;", "'");
                    lyricTemp.Replace("&amp;", "&");

                    LyricText = lyricTemp.ToString().Trim();

                    if (LyricText.Contains("<td") || LyricText.Contains("<IFRAME"))
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