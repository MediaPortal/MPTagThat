using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    // This class searches www.shiron.net for Hebrew lyrics
    public class Shironet : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "Shironet";

        // Base url
        private const string SiteBaseUrl = "http://shironet.mako.co.il";

        // Cannot find exact match
        private const string CannotFindExactMatch = "Cannot find exact match";
        // space
        private const string Space = "%20";

        private const string SearchPathQuery = "/searchSongs?type=lyrics&q=";
        private const string LyricsPath = "/artist?type=lyrics&lang=1";
        private const string Prfid = "&prfid=";
        private const string Wrkid = "&wrkid=";

        # endregion

        # region patterns

        //////////////////////////
        // First phase patterns //
        //////////////////////////

        // Hebrew pattern
        private const string HebrewPattern = @"[\xE0-\xFA\u0590-\u05FF]";

        // RegEx to find lyrics page
        private const string FindLyricsPagePattern =
            @"<a href=""/artist\?type=lyrics&lang=1&prfid=(?<prfid>\d+)&wrkid=(?<wrkid>\d+)"" class=""search_link_name_big"">";

        ///////////////////////////
        // Second phase patterns //
        ///////////////////////////

        // Validation RegEx - Either TitleAndArtist or Title & Artist should be valid
        // Title+Artist RegEx
        private const string TitleAndArtistSearchPattern = @"<title>(?<titleartist>.*?)</title>";
        // Title RegEx (multiline)
        private const string TitleSearchStartPattern = @"<span class=""artist_song_name_txt"">";
        private const string TitleSearchEndPattern = @"</span>";
        private const string TitleSearchPattern = @"<span class=""artist_song_name_txt"">(?<title>.*?)</span>";
        // Artist RegEx
        private const string ArtistSearchPattern =
            @"<a class=""artist_singer_title"" href=""/artist\?prfid=?\d+&lang=?\d+"">(?<artist>.*?)</a>";


        // Lyrics RegEx
        // Lyrics start RegEx
        private const string LyricsStartSearchPattern = @"<span class=""artist_lyrics_text"">(?<lyricsStart>.*)";
        // Lyrics end RegEx
        private const string LyricsEndSearchPattern = @"(?<lyricsEnd>.*?)</span>";

        # endregion

        // step 1 output
        private string _prfid;
        private string _wrkid;
        private bool _firstStepComplete;


        public Shironet(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit)
            : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var fixHebrewArtist = FixHebrewName(Artist);
            var fixedHebrewTitle = FixHebrewName(Title);

            // Check there's at least one Hebrew letter in either artist or title. We skip test if not
            if (!IsHebrew(fixHebrewArtist) && !IsHebrew(fixedHebrewTitle))
            {
                LyricText = NotFound;
                return;
            }

            // 1st step - find lyrics page
            var firstUrlString = BaseUrl + SearchPathQuery + fixHebrewArtist + Space + fixedHebrewTitle;

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

            if (_prfid == null || _wrkid == null)
            {
                LyricText = NotFound;
                return;
            }
            // 2nd step - find lyrics
            var secondUrlString = BaseUrl + LyricsPath + Prfid + _prfid + Wrkid + _wrkid;

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
            return SiteSpeed.Fast;
        }

        public override bool SiteActive()
        {
            return true;
        }

        #endregion interface implemetation

        #region public methods

        public static bool IsHebrew(string str)
        {
            var myRegex = new Regex(HebrewPattern, RegexOptions.None);
            var matchCollection = myRegex.Match(str);
            return matchCollection.Success;
        }

        #endregion

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
                    var findLyricsPageMatch = Regex.Match(line, FindLyricsPagePattern, RegexOptions.IgnoreCase);

                    if (findLyricsPageMatch.Groups.Count == 3)
                    {
                        _prfid = findLyricsPageMatch.Groups[1].Value;
                        _wrkid = findLyricsPageMatch.Groups[2].Value;

                        if (Convert.ToUInt32(_prfid) > 0 && Convert.ToUInt32(_wrkid) > 0)
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

                // Validation
                var titleAndArtistInPage = string.Empty;
                var inTitle = false;
                var titleInPage = string.Empty;
                var artistInPage = string.Empty;
                var validateArtistAndTitle = false;
                var validateArtist = false;
                var validateTitle = false;

                var foundStart = false;

                while (!Complete)
                {
                    // Read line
                    if (reader.EndOfStream)
                    {
                        break;
                    }
                    var line = reader.ReadLine() ?? string.Empty;

                    // Find artist + title in <title> line and validate correct artist/title
                    if (titleAndArtistInPage == string.Empty)
                    {
                        var findTitleAndArtistMatch = Regex.Match(line, TitleAndArtistSearchPattern, RegexOptions.IgnoreCase);
                        if (findTitleAndArtistMatch.Groups.Count == 2)
                        {
                            titleAndArtistInPage = findTitleAndArtistMatch.Groups[1].Value;

                            // validation ArtistAndTitle
                            if (ValidateArtistAndTitle(titleAndArtistInPage))
                            {
                                validateArtistAndTitle = true;
                            }
                        }
                    }

                    //Find title in <span class="artist_song_name_txt">(?.*)</span> line
                    if (titleInPage == String.Empty)
                    {
                        var findTitleStartMatch = Regex.Match(line, TitleSearchStartPattern, RegexOptions.IgnoreCase);
                        if (findTitleStartMatch.Success)
                        {
                            inTitle = true;
                        }
                    }
                    if (inTitle) // title found in page
                    {
                        titleInPage += line;

                        // Search for ending of title 
                        var findTitleEndMatch = Regex.Match(line, TitleSearchEndPattern, RegexOptions.IgnoreCase);
                        if (findTitleEndMatch.Success)
                        {
                            inTitle = false;
                        }
                    }
                    // Finish and validate
                    if (titleInPage != string.Empty && !inTitle)
                    {
                        // Search for ending of artist 
                        var findTitleMatch = Regex.Match(titleInPage, TitleSearchPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        if (findTitleMatch.Groups.Count == 2)
                        {
                            titleInPage = findTitleMatch.Groups[1].Value;

                            // validation Title
                            if (IgnoreSpecialChars(Title).Equals(IgnoreSpecialChars(titleInPage).Trim()))
                            {
                                validateTitle = true;
                            }
                        }
                    }

                    // Find artist in <a class="artist_singer_title" href="/artist?prfid=975&lang=1">()</a>
                    if (artistInPage == string.Empty)
                    {
                        var findArtistMatch = Regex.Match(line, ArtistSearchPattern, RegexOptions.IgnoreCase);
                        if (findArtistMatch.Groups.Count == 2)
                        {
                            artistInPage = findArtistMatch.Groups[1].Value;

                            // validation Artist
                            if (IgnoreSpecialChars(Artist).Equals(IgnoreSpecialChars(artistInPage).Trim()))
                            {
                                validateArtist = true;
                            }
                        }
                    }

                    if (!foundStart)
                    {
                        // Try to find lyrics start in line
                        var findLyricsPageMatch = Regex.Match(line, LyricsStartSearchPattern, RegexOptions.IgnoreCase);

                        if (findLyricsPageMatch.Groups.Count == 2)
                        {
                            foundStart = true;

                            // Here's where we use the data from the validation - just when we hit the first lyrics row
                            if (!((validateArtist && validateTitle) || validateArtistAndTitle))
                            {
                                throw new ArgumentException(CannotFindExactMatch);
                            }

                            // Initialize with first line
                            lyricTemp.Append(findLyricsPageMatch.Groups[1].Value).Append(Environment.NewLine);
                        }
                    }
                    else // already found start
                    {
                        // Try to find lyrics end in line
                        var findLyricsPageMatch = Regex.Match(line, LyricsEndSearchPattern, RegexOptions.IgnoreCase);
                        if (findLyricsPageMatch.Groups.Count == 2)
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

        private bool ValidateArtistAndTitle(string titleLine)
        {
            // Clean title
            titleLine = titleLine.Replace("מילים לשיר", "");
            titleLine = titleLine.Replace("- שירונט", "");
            titleLine = titleLine.Trim();

            // Split tille
            var strings = titleLine.Split('-');
            if (strings.Length == 2)
            {
                // check artist
                if (!IgnoreSpecialChars(Artist).Equals(IgnoreSpecialChars(strings[1]).Trim()))
                {
                    return false;
                }
                // check title
                if (!IgnoreSpecialChars(Title).Equals(IgnoreSpecialChars(strings[0]).Trim()))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private static string IgnoreSpecialChars(string orig)
        {
            return orig.Replace("\'", "").Replace("\"", "").Trim();
        }

        private static string CleanLyrics(StringBuilder lyricTemp)
        {
            lyricTemp.Replace("<br>", "");
            lyricTemp.Replace("<br/>", "");
            lyricTemp.Replace("&quot;", "\"");

            return lyricTemp.ToString().Trim();
        }

        // Escape characters & Fix Hebrew letters
        private static string FixHebrewName(string name)
        {
            return FixHebrew(FixEscapeCharacters(name));
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

        private static string FixHebrew(string text)
        {
            text = text.Replace("\uC3A0", "%d7%90"); // א
            text = text.Replace("\uC3A1", "%D7%91");
            text = text.Replace("\uC3A2", "%D7%92");
            text = text.Replace("\uC3A3", "%D7%93");
            text = text.Replace("\uC3A4", "%D7%94");
            text = text.Replace("\uC3A5", "%D7%95");
            text = text.Replace("\uC3A6", "%D7%96");
            text = text.Replace("\uC3A7", "%D7%97");
            text = text.Replace("\uC3A8", "%D7%98");
            text = text.Replace("\uC3A9", "%D7%99");
            text = text.Replace("\uC3AA", "%D7%9A");
            text = text.Replace("\uC3AB", "%D7%9B");
            text = text.Replace("\uC3AC", "%D7%9C");
            text = text.Replace("\uC3AD", "%D7%9D");
            text = text.Replace("\uC3AE", "%D7%9E");
            text = text.Replace("\uC3AF", "%D7%9F");
            text = text.Replace("\uC3B0", "%D7%A0");
            text = text.Replace("\uC3B1", "%D7%A1");
            text = text.Replace("\uC3B2", "%D7%A2");
            text = text.Replace("\uC3B3", "%D7%A3");
            text = text.Replace("\uC3B4", "%D7%A4");
            text = text.Replace("\uC3B5", "%D7%A5");
            text = text.Replace("\uC3B6", "%D7%A6");
            text = text.Replace("\uC3B7", "%D7%A7");
            text = text.Replace("\uC3B8", "%D7%A8");
            text = text.Replace("\uC3B9", "%D7%A9");
            text = text.Replace("\uC3BA", "%D7%AA"); // ת

            text = text.Replace("\uD790", "%d7%90"); // א
            text = text.Replace("\uD791", "%D7%91");
            text = text.Replace("\uD792", "%D7%92");
            text = text.Replace("\uD793", "%D7%93");
            text = text.Replace("\uD794", "%D7%94");
            text = text.Replace("\uD795", "%D7%95");
            text = text.Replace("\uD796", "%D7%96");
            text = text.Replace("\uD797", "%D7%97");
            text = text.Replace("\uD798", "%D7%98");
            text = text.Replace("\uD799", "%D7%99");
            text = text.Replace("\uD79A", "%D7%9A");
            text = text.Replace("\uD79B", "%D7%9B");
            text = text.Replace("\uD79C", "%D7%9C");
            text = text.Replace("\uD79D", "%D7%9D");
            text = text.Replace("\uD79E", "%D7%9E");
            text = text.Replace("\uD79F", "%D7%9F");
            text = text.Replace("\uD7A0", "%D7%A0");
            text = text.Replace("\uD7A1", "%D7%A1");
            text = text.Replace("\uD7A2", "%D7%A2");
            text = text.Replace("\uD7A3", "%D7%A3");
            text = text.Replace("\uD7A4", "%D7%A4");
            text = text.Replace("\uD7A5", "%D7%A5");
            text = text.Replace("\uD7A6", "%D7%A6");
            text = text.Replace("\uD7A7", "%D7%A7");
            text = text.Replace("\uD7A8", "%D7%A8");
            text = text.Replace("\uD7A9", "%D7%A9");
            text = text.Replace("\uD7AA", "%D7%AA"); // ת

            return text;
        }

        #endregion private methods
    }
}
