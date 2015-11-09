using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class Lyrics007 : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "Lyrics007";

        // Base url
        private const string SiteBaseUrl = "http://www.lyrics007.com";

        # endregion const

        # region patterns

        // lyrics mark pattern 
        private const string LyricsMarkPattern = @"data-artist=""(?<artist>.*)"" data-song=""(?<song>.*)"" data-search";

        # endregion patterns

        public Lyrics007(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var artist = LyricUtil.RemoveFeatComment(Artist);
            artist = artist.Replace("#", "");
            var title = LyricUtil.TrimForParenthesis(Title);
            title = title.Replace("#", "");

            // Cannot find lyrics contaning non-English letters!

            var urlString = SiteBaseUrl + "/" + artist + " Lyrics/" + title + " Lyrics.html";

            var uri = new Uri(urlString);
            var client = new LyricsWebClient();
            client.OpenReadCompleted += CallbackMethod;
            client.OpenReadAsync(uri);

            while (Complete == false)
            {
                if (MEventStopSiteSearches.WaitOne(500, true))
                {
                    Complete = true;
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
            var lyricTemp = new StringBuilder();
            
            Stream reply = null;
            StreamReader reader = null;

            try
            {
                reply = e.Result;
                reader = new StreamReader(reply, Encoding.UTF8);

                // Look for start
                var inLyrics = false;
                while (true)
                {
                    if (reader.EndOfStream)
                    {
                        break;
                    }

                    // Read next line
                    var line = reader.ReadLine() ?? "";

                    // Find lyrics mark
                    var match = Regex.Match(line, LyricsMarkPattern, RegexOptions.IgnoreCase);

                    // Handle line
                    if (!inLyrics) // before lyrics started
                    {
                        // mark start of lyrics
                        if (match.Success)
                        {
                            if (match.Groups.Count == 3)
                            {
                                if (match.Groups[1].Value.Equals(Artist, StringComparison.OrdinalIgnoreCase) &&
                                    match.Groups[2].Value.Equals(Title, StringComparison.OrdinalIgnoreCase))
                                {
                                    inLyrics = true;
                                }
                                else
                                {
                                    LyricText = NotFound;
                                    break;
                                }
                            }
                        }
                    }
                    else // after lyrics started
                    {
                        // end of lyrics
                        if (match.Success)
                        {
                            break;
                        }

                        // Add line
                        lyricTemp.Append(line);
                    }
                }

                LyricText = lyricTemp.ToString();

                if (LyricText.Length > 0)
                {
                    CleanLyrics();
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

        // Cleans the lyrics
        private void CleanLyrics()
        {
            LyricText = LyricText.Replace("</script>", "");
            LyricText = LyricText.Replace("??s", "'s");
            LyricText = LyricText.Replace("??t", "'t");
            LyricText = LyricText.Replace("??m", "'m");
            LyricText = LyricText.Replace("??l", "'l");
            LyricText = LyricText.Replace("??v", "'v");
            LyricText = LyricText.Replace("?s", "'s");
            LyricText = LyricText.Replace("?t", "'t");
            LyricText = LyricText.Replace("?m", "'m");
            LyricText = LyricText.Replace("?l", "'l");
            LyricText = LyricText.Replace("?v", "'v");
            LyricText = LyricText.Replace("<br>", "\r\n");
            LyricText = LyricText.Replace("<br />", "\r\n");
            LyricText = LyricText.Replace("<BR>", "\r\n");
            LyricText = LyricText.Replace("&amp;", "&");
            LyricText = Regex.Replace(LyricText, @"<span.*</span>", "", RegexOptions.Singleline);
            LyricText = Regex.Replace(LyricText, @"<.*?>", "", RegexOptions.Singleline);
            LyricText = Regex.Replace(LyricText, @"<!--.*-->", "", RegexOptions.Singleline);
            LyricText = LyricText.Trim();
        }

        #endregion private methods
    }
}