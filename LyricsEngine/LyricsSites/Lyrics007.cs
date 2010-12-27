using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using Timer=System.Timers.Timer;

namespace LyricsEngine.LyricSites
{
    internal class Lyrics007
    {
        private bool complete;
        private string lyric = "";
        private int timeLimit;
        private Timer timer;

        public Lyrics007(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit/2;
            timer = new Timer();

            artist = LyricUtil.RemoveFeatComment(artist);
            artist = artist.Replace("#", "");
            title = LyricUtil.TrimForParenthesis(title);
            title = title.Replace("#", "");

            // Cannot find lyrics contaning non-English letters!

            string urlString = "http://www.lyrics007.com/" + artist + " Lyrics/" + title + " Lyrics.html";

            timer.Enabled = true;
            timer.Interval = timeLimit;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Uri uri = new Uri(urlString);
            LyricsWebClient client = new LyricsWebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(callbackMethod);
            client.OpenReadAsync(uri);

            while (complete == false)
            {
                if (m_EventStop_SiteSearches.WaitOne(500, true))
                {
                    complete = true;
                }
            }
        }

        public string Lyric
        {
            get { return lyric; }
        }


        private void callbackMethod(object sender, OpenReadCompletedEventArgs e)
        {
            bool thisMayBeTheCorrectLyric = true;
            StringBuilder lyricTemp = new StringBuilder();
            LyricsWebClient client = (LyricsWebClient) sender;
            Stream reply = null;
            StreamReader sr = null;

            try
            {
                reply = (Stream) e.Result;
                sr = new StreamReader(reply, Encoding.Default);

                //string line = sr.ReadToEnd();
                string line = sr.ReadLine();

                // Replace the new line stuff in the result, as the regex might have problems
                //line = line.Replace("\n", "");
                //line = line.Replace("\r", "");
                //line = line.Replace("\t", "");

                //string pat = @"<script\s*type=""text/javascript""\s*src=""http://www2\.ringtonematcher\.com/jsstatic/lyrics007\.js""></script>.*?</div>(.*?)<div.*";
                //string pat = @"src=""/images/phone2.gif""><br><br><br></div>(.*?)<div align=center>";
                //string pat = @"<br><br><br></div>(.*?)<div.*";

                string pat = @"rtm.js""></scr' + 'ipt>');";

                while (line.IndexOf(pat) == -1)
                {
                    if (sr.EndOfStream)
                    {
                        thisMayBeTheCorrectLyric = false;
                        break;
                    }
                    else
                    {
                        line = sr.ReadLine();
                    }
                }

                if (thisMayBeTheCorrectLyric)
                {
                    line = sr.ReadLine();
                    line = line.Replace("</script><br><br><br>", string.Empty);

                    while (line.IndexOf("<script") == -1)
                    {
                        lyricTemp.Append(line);
                        if (sr.EndOfStream)
                        {
                            thisMayBeTheCorrectLyric = false;
                            break;
                        }
                        else
                        {
                            line = sr.ReadLine();
                        }
                    }
                }

                lyric = lyricTemp.ToString();

                if (lyric.Length > 0)
                {
                    lyric = lyric.Replace("??s", "'s");
                    lyric = lyric.Replace("??t", "'t");
                    lyric = lyric.Replace("??m", "'m");
                    lyric = lyric.Replace("??l", "'l");
                    lyric = lyric.Replace("??v", "'v");
                    lyric = lyric.Replace("?s", "'s");
                    lyric = lyric.Replace("?t", "'t");
                    lyric = lyric.Replace("?m", "'m");
                    lyric = lyric.Replace("?l", "'l");
                    lyric = lyric.Replace("?v", "'v");
                    lyric = lyric.Replace("<br>", "\r\n");
                    lyric = lyric.Replace("<br />", "\r\n");
                    lyric = lyric.Replace("<BR>", "\r\n");
                    lyric = lyric.Replace("&amp;", "&");
                    lyric = lyric.Trim();
                }
                else
                    lyric = "Not found";
            }
            catch
            {
                lyric = "Not found";
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }

                if (reply != null)
                {
                    reply.Close();
                }
                complete = true;
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Close();
            timer.Dispose();

            lyric = "Not found";
            complete = true;
            Thread.CurrentThread.Abort();
        }
    }
}