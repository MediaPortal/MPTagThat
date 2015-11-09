using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class Lyrster : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "Lyrster";

        // Base url
        private const string SiteBaseUrl = "http://www.lyrster.com";

        # endregion

        public Lyrster(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var artist = Artist.ToLower();
            artist = ClearName(artist);

            var title = Title.ToLower();
            title = ClearName(title);
            
            // Validation
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                LyricText = NotFound;
                return;
            }

            var urlString = SiteBaseUrl + "/lyrics/" + title + "-lyrics-" + artist + ".html";

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
            return SiteSpeed.Slow;
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

                while (line.IndexOf(@"<div id=""lyrics"">", StringComparison.Ordinal) == -1)
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

                    while (line.IndexOf("</div>", StringComparison.Ordinal) == -1)
                    {
                        lyricTemp.Append(line);
                        if (reader.EndOfStream)
                        {
                            break;
                        }
                        line = reader.ReadLine() ?? "";
                    }


                    if (lyricTemp.Length > 0)
                    {

                        lyricTemp.Replace(@"<div id=""lyrics"">", "");
                        lyricTemp.Replace("</div>", "");
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

        private static string ClearName(string name)
        {
            // Spaces
            name = name.Replace(" ", "-");
            name = name.Replace(",", "-");
            name = name.Replace(".", "-");
            name = name.Replace("'", "-");
            name = name.Replace("(", "%28");
            name = name.Replace(")", "%29");
            name = name.Replace(",", "");
            name = name.Replace("#", "");
            name = name.Replace("%", "");
            name = name.Replace("+", "%2B");
            name = name.Replace("=", "%3D");
            
            // French letters
            name = name.Replace("é", "%E9");

            name = name.Replace(" ", "-");
            name = name.Replace(",", "-");
            name = name.Replace(".", "-");
            name = name.Replace("'", "-");
            name = name.Replace("(", "%28");
            name = name.Replace(")", "%29");
            name = name.Replace(",", "-");
            name = name.Replace("#", "-");
            name = name.Replace("%", "-");
            name = name.Replace("?", "-");
            name = name.Replace("+", "%2B");
            name = name.Replace("=", "%3D");
            name = name.Replace(":", "-");

            // German letters
            name = name.Replace("ü", "%FC");
            name = name.Replace("Ü", "%DC");
            name = name.Replace("ä", "%E4");
            name = name.Replace("Ä", "%C4");
            name = name.Replace("ö", "%F6");
            name = name.Replace("Ö", "%D6");
            name = name.Replace("ß", "%DF");

            // Danish letters
            name = name.Replace("å", "%E5");
            name = name.Replace("Å", "%C5");
            name = name.Replace("æ", "%E6");
            name = name.Replace("ø", "%F8");

            // French letters
            name = name.Replace("é", "%E9");


            return name;
        }

        #endregion private methods
    }
}