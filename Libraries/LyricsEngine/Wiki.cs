using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using LyricsEngine.org.lyricwiki;
using LyricsEngine;

namespace MyLyricWiki
{
    public class Wiki
    {

        private LyricWiki lyricWiki;
        private LyricsResult lyricsResult = null;
        private string artist = "";
        private string title = "";
        private int noOfTries = 0;
        public static bool Abort = false;

        private string[] commonReplacementsLyricWiki = new string[16] { "<br>", "", "<BR>", "", "?d", "'d", "?s", "'s", "?t", "'t", "?v", "'v", "?e", "'e", "?r", "'r" };
        private string[] commonRemoveFromLyricWiki = new string[2] { "[[category", "[[Category" };

        public Wiki()
        {
            lyricWiki = new LyricWiki();
        }

        delegate LyricsResult DelegateClass(string artist, string title);

        private bool searchForWiki(string artist, string title)
        {
            DelegateClass del = lyricWiki.getSongResult;

            IAsyncResult ar = del.BeginInvoke(this.artist, this.title, null, null);

            while (noOfTries < 15)
            {
                // If the user has aborted stop the search and return (false)
                if (Abort)
                    return false;

                if (ar.AsyncWaitHandle.WaitOne(0, true))
                {
                    lyricsResult = del.EndInvoke(ar);
                    break;
                }
                else
                {
                    // if we don't allow this pause of 2 sec the webservice behaves in a strange maneur
                    Thread.Sleep(2000);
                }
                ++noOfTries;
            }

            if (lyricsResult != null && isLyric(lyricsResult.lyrics))
            {
                return true;
            }
            else
            {
                noOfTries = 0;
                return false;
            }

        }

        public string getLyricAsynch(string artist, string title)
        {
            this.artist = artist;
            this.title = title;
            try
            {
                // Step 1: search title as it is
                if (Abort == false && searchForWiki(this.artist, this.title))
                {
                    return makeLyricFit(lyricsResult.lyrics);
                }

                // Step 2: search with parentheses and other disturbance removed
                optimizeString(ref this.artist, ref this.title);
                if (searchForWiki(this.artist, this.title))
                {
                    return (lyricsResult.lyrics);
                }

                // Step 3: search with altered and strings if any
                this.artist = LyricUtil.changeAnd_and_and(this.artist);
                this.title = LyricUtil.changeAnd_and_and(this.title);
                if (searchForWiki(this.artist, this.title))
                {
                    return (lyricsResult.lyrics);
                }

                // final step: return "Not found" if no lyric found
                return "Not found";
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.ToString());
                //System.Windows.Forms.MessageBox.Show(lyricsResult.);
                Console.Write(e.Message);
                return "";
            }
        }

        public string getLyricSynchron(string artist, string title)
        {
            this.artist = artist;
            this.title = title;
            try
            {
                // Step 1: search title as it is
                lyricsResult = lyricWiki.getSong(this.artist, this.title);
                if (isLyric(lyricsResult.lyrics))
                {
                    return lyricsResult.lyrics;
                }

                //Thread.Sleep(1000);

                //// Step 2: search with parentheses and other disturbance removed
                //optimizeString(ref this.artist, ref this.title);
                //lyricsResult = lyricWiki.getSongResult(this.artist, this.title);
                //if (isLyric(lyricsResult.lyrics))
                //{
                //    return lyricsResult.lyrics;
                //}

                //Thread.Sleep(1000);

                //// Step 3: search with altered and strings if any
                //this.artist = LyricUtil.changeAnd_and_and(this.artist);
                //this.title = LyricUtil.changeAnd_and_and(this.title);
                //lyricsResult = lyricWiki.getSongResult(this.artist, this.title);
                //if (isLyric(lyricsResult.lyrics))
                //{
                //    return lyricsResult.lyrics;
                //}

                //Thread.Sleep(3000);

                // final step: return "Not found" if no lyric found
                return "Not found";
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.ToString());
                //System.Windows.Forms.MessageBox.Show(lyricsResult.);
                Console.Write(e.Message);
                return "";
            }
        }

        private bool isLyric(string lyric)
        {
            if (lyricsResult != null && !lyricsResult.lyrics.Equals("Not found") && !lyricsResult.lyrics.Equals(""))
                return true;
            else
                return false;
        }


        private void optimizeString(ref string artist, ref string title)
        {
            LyricUtil.trimForParenthesis(ref artist);
            LyricUtil.trimForParenthesis(ref title);
        }

        private string makeLyricFit(string lyric)
        {
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

            return lyric;
        }
    }
}
