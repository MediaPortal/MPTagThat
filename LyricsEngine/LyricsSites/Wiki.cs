using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using LyricsEngine.org.lyricwiki;
using LyricsEngine;

namespace LyricsEngine.LyricSites
{
    public class Wiki
    {
        private LyricWiki lyricWiki;
        private LyricsResult lyricsResult;
        private string artist = "";
        private string title = "";
        private int noOfTries;
        public static bool Abort;
        LyricSearch lyricSearch;

        private string[] commonReplacementsLyricWiki = new string[18] { "<br>", "", "<br/>", "", "<BR>", "", "?d", "'d", "?s", "'s", "?t", "'t", "?v", "'v", "?e", "'e", "?r", "'r" };
        private string[] commonRemoveFromLyricWiki = new string[2] { "[[category", "[[Category" };

        public Wiki(LyricSearch lyricSearch)
        {
            this.lyricSearch = lyricSearch;
            lyricWiki = new LyricWiki();
        }

        delegate LyricsResult DelegateClass(string artist, string title);

        private bool SearchForWiki()
        {
            DelegateClass del = lyricWiki.getSong;

            IAsyncResult ar = del.BeginInvoke(this.artist, this.title, null, null);

            while (noOfTries < 9)
            {
                // If the user has aborted stop the search and return (false)
                if (Abort || lyricSearch.SearchHasEnded)
                    return false;
                else if (ar.AsyncWaitHandle.WaitOne(0, true))
                {
                    lyricsResult = del.EndInvoke(ar);
                    
                    string lyric = lyricsResult.lyrics;
                    Encoding iso8859 = Encoding.GetEncoding("ISO-8859-1");
                    lyricsResult.lyrics = Encoding.UTF8.GetString(iso8859.GetBytes(lyricsResult.lyrics));
                    break;
                }
                else
                {
                    // if we don't allow this pause of 2 sec the webservice behaves in a strange maneur
                    Thread.Sleep(2000);
                }
                ++noOfTries;
            }

            if (lyricsResult != null && IsLyric(lyricsResult.lyrics))
            {
                return true;
            }
            else
            {
                noOfTries = 0;
                return false;
            }
        }

        public string GetLyricAsynch(string artist, string title)
        {
            this.artist = artist;
            this.title = title;
            try
            {
                // search with parentheses and other disturbance removed
                OptimizeString(ref this.artist, ref this.title);
                if (Abort == false && SearchForWiki())
                {
                    return MakeLyricFit(lyricsResult.lyrics);
                }
                else
                {
                    return "Not found";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private bool IsLyric(string lyric)
        {
            if (lyricsResult != null && !lyricsResult.lyrics.Equals("Not found") && lyricsResult.lyrics.Length != 0)
                return true;
            else
                return false;
        }


        private void OptimizeString(ref string artist, ref string title)
        {
            title = LyricUtil.TrimForParenthesis(title);
        }

        private string MakeLyricFit(string lyric)
        {
            if (!lyric.Contains("\r\n"))
            {
                lyric = lyric.Replace("\n", "\r\n");
            }

            for (int i = 0; i < commonReplacementsLyricWiki.Length; i = i + 2)
            {
                lyric = lyric.Replace(commonReplacementsLyricWiki[i], commonReplacementsLyricWiki[i + 1]);
            }

            for (int i = 0; i < commonRemoveFromLyricWiki.Length; i++)
            {
                int index = lyric.IndexOf(commonRemoveFromLyricWiki[i]);
                if (index != -1)
                {
                    lyric = lyric.Substring(0, index);
                    break;
                }
            }

            if (lyric.Contains("API request randomly") || lyric.Contains("Upgrading right") || lyric.Contains("LyricWiki.org")
                || lyric.Contains("<!-- PUT LYRICS HERE"))
            {
                lyric = string.Empty;
            }

            return lyric;
        }
    }
}
