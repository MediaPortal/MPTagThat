using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class Lyricsmode : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "Lyricsmode";

        // Base url
        private const string SiteBaseUrl = "http://www.lyricsmode.com";
        
        private const string StartIndication = "<!-- SONG LYRICS -->";
        private const string EndIndication = "<!-- /SONG LYRICS -->";

        # endregion

        public Lyricsmode(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
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
            return SiteSpeed.Medium;
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

                while (line.IndexOf(StartIndication, StringComparison.Ordinal) == -1)
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

                    
                    while (line.IndexOf(EndIndication, StringComparison.Ordinal) == -1)
                    {
                        lyricTemp.Append(line);
                        if (reader.EndOfStream)
                        {
                            break;
                        }
                        line = reader.ReadLine() ?? "";
                    }

                    lyricTemp.Replace("<div id='songlyrics_h' class='dn'>", "");
                    lyricTemp.Replace("</div>", "");
                    lyricTemp.Replace("<br>", "\r\n");
                    lyricTemp.Replace("<br />", "\r\n");
                    lyricTemp.Replace("&quot;", "\"");
                    lyricTemp.Replace("<br/>", "\r\n");
                    lyricTemp.Replace("&amp;", "&");

                    LyricText = lyricTemp.ToString().Trim();
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
            // Spaces and special characters
            name = name.Replace(" ", "_");
            name = name.Replace("#", "_");
            name = name.Replace("%", "_");
            name = name.Replace("'", "");
            name = name.Replace("(", "%28");
            name = name.Replace(")", "%29");
            name = name.Replace("+", "%2B");
            name = name.Replace(",", "");
            name = name.Replace(".", "_");
            name = name.Replace(":", "_");
            name = name.Replace("=", "%3D");
            name = name.Replace("?", "_");

            // German letters
            name = name.Replace("�", "%FC");
            name = name.Replace("�", "%DC");
            name = name.Replace("�", "%E4");
            name = name.Replace("�", "%C4");
            name = name.Replace("�", "%F6");
            name = name.Replace("�", "%D6");
            name = name.Replace("�", "%DF");

            // Danish letters
            name = name.Replace("�", "%E5");
            name = name.Replace("�", "%C5");
            name = name.Replace("�", "%E6");
            name = name.Replace("�", "%F8");

            // French letters
            name = name.Replace("�", "%E9");


            return name;
        }

        #endregion private methods
    }
}