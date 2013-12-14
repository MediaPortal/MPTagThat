using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class LyricsNet : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "LyricsNet";

        // Base url
        private const string SiteBaseUrl = "http://www.lyrics.net";

        private const string SearchPathQuery = "/artist/";
        private const string LyricsPath = "/lyric/";

        # endregion

        # region patterns

        //////////////////////////
        // First phase patterns //
        //////////////////////////

        // RegEx to find lyrics page
        private const string FindLyricsPagePatternPrefix = @"<a href=""/lyric/(?<lyricsIndex>\d+)"">";
        private const string FindLyricsPagePatternSuffix = "</a>";

        ///////////////////////////
        // Second phase patterns //
        ///////////////////////////

        // Lyrics RegEx
        // Lyrics start RegEx
        private const string LyricsStartSearchPattern = @"<pre id=""lyric-body-text"" class=""lyric-body"" dir=""ltr"" data-lang=""en"">";
        // Lyrics end RegEx
        private const string LyricsEndSearchPattern =  @"</pre>";

        # endregion

        // step 1 output
        private string _lyricsIndex;
        private bool _firstStepComplete;


        public LyricsNet(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var artist = FixEscapeCharacters(Artist);

            // 1st step - find lyrics page
            var firstUrlString = BaseUrl + SearchPathQuery + artist;

            var findLyricsPageWebClient = new LyricsWebClient();
            findLyricsPageWebClient.OpenReadCompleted += FirstCallbackMethod;
            findLyricsPageWebClient.OpenReadAsync(new Uri(firstUrlString));

            while (_firstStepComplete == false)
            {
                if (MEventStopSiteSearches.WaitOne(1, true))
                {
                    _firstStepComplete = true;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            if (_lyricsIndex == null)
            {
                LyricText = NotFound;
                return;
            }
            // 2nd step - find lyrics
            var secondUrlString = BaseUrl + LyricsPath + _lyricsIndex;

            var findLyricsWebClient = new LyricsWebClient(firstUrlString);
            findLyricsWebClient.OpenReadCompleted += SecondCallbackMethod;
            findLyricsWebClient.OpenReadAsync(new Uri(secondUrlString));

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

        public override string Name
        {
            get { return SiteName; }
        }

        public override string BaseUrl
        {
            get { return SiteBaseUrl;}
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
            return SiteSpeed.Slow;
        }

        public override bool SiteActive()
        {
            return true;
        }

        #endregion interface implemetation

        #region private methods

        // Finds lyrics page
        private void FirstCallbackMethod(object sender, OpenReadCompletedEventArgs e)
        {
            var thisMayBeTheCorrectPage = false;

            Stream reply = null;
            StreamReader reader = null;

            try
            {
                reply = e.Result;
                reader = new StreamReader(reply, Encoding.UTF8);

                while (!thisMayBeTheCorrectPage)
                {
                    // Read line
                    if (reader.EndOfStream)
                    {
                        break;
                    }
                    var line = reader.ReadLine() ?? "";

                    // Try to find match in line
                    var findLyricsPagePattern = FindLyricsPagePatternPrefix + Title + FindLyricsPagePatternSuffix;
                    var findLyricsPageMatch = Regex.Match(line, findLyricsPagePattern, RegexOptions.IgnoreCase);

                    if (findLyricsPageMatch.Groups.Count == 2)
                    {
                        _lyricsIndex = findLyricsPageMatch.Groups[1].Value;

                        if (Convert.ToUInt32(_lyricsIndex) > 0)
                        {
                            // Found page
                            thisMayBeTheCorrectPage = true;
                        }
                    }
                }

                // Not found
                if (!thisMayBeTheCorrectPage)
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
                _firstStepComplete = true;
            }
        }

        // Find lyrics
        private void SecondCallbackMethod(object sender, OpenReadCompletedEventArgs e)
        {
            var thisMayBeTheCorrectLyric = false;
            var lyricTemp = new StringBuilder();

            Stream reply = null;
            StreamReader reader = null;

            try
            {
                reply = e.Result;
                reader = new StreamReader(reply, Encoding.UTF8);

                var foundStart = false;

                while (!Complete)
                {
                    // Read line
                    if (reader.EndOfStream)
                    {
                        break;
                    }
                    var line = reader.ReadLine() ?? string.Empty;

                    if (!foundStart)
                    {
                        // Try to find lyrics start in line
                        var findLyricsPageMatch = Regex.Match(line, LyricsStartSearchPattern, RegexOptions.IgnoreCase);

                        if (findLyricsPageMatch.Success)
                        {
                            foundStart = true;

                            // Initialize with first line
                            lyricTemp.Append(findLyricsPageMatch.Groups[1].Value).Append(Environment.NewLine);
                        }
                    }
                    else // already found start
                    {
                        // Try to find lyrics end in line
                        var findLyricsPageMatch = Regex.Match(line, LyricsEndSearchPattern, RegexOptions.IgnoreCase);
                        if (findLyricsPageMatch.Success)
                        {
                            // Add last line
                            lyricTemp.Append(findLyricsPageMatch.Groups[1].Value).Append(Environment.NewLine);
                            
                            thisMayBeTheCorrectLyric = true;
                            break;
                        }

                        // Add line to lyrics
                        lyricTemp.Append(line).Append(Environment.NewLine);
                    }
                }

                if (thisMayBeTheCorrectLyric)
                {
                    // Clean lyrics
                    LyricText = CleanLyrics(lyricTemp);

                    if (LyricText.Length == 0 || (LyricText.Contains("<") || LyricText.Contains(">") || LyricText.Contains("a href")))
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

        private static string CleanLyrics(StringBuilder lyricTemp)
        {
            lyricTemp.Replace("<br>", "");
            lyricTemp.Replace("<br/>", "");
            lyricTemp.Replace("&quot;", "\"");

            lyricTemp.Replace(LyricsStartSearchPattern, "");
            lyricTemp.Replace(LyricsEndSearchPattern, "");

            return lyricTemp.ToString().Trim();
        }

        private static string FixEscapeCharacters(string text)
        {
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("#", "");
            text = text.Replace("/", "");

            text = text.Replace("%", "%25");

            text = text.Replace(" ", "%20");
            text = text.Replace("$", "%24");
            text = text.Replace("&", "%26");
            text = text.Replace("'", "%27");
            text = text.Replace("+", "%2B");
            text = text.Replace(",", "%2C");
            text = text.Replace(":", "%3A");
            text = text.Replace(";", "%3B");
            text = text.Replace("=", "%3D");
            text = text.Replace("?", "%3F");
            text = text.Replace("@", "%40");
            text = text.Replace("&amp;", "&");

            return text;
        }

        #endregion private methods
    }
}
