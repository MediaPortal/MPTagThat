using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Timers;
using System.Text.RegularExpressions;

namespace LyricsEngine.LyricSites
{
    class LyricsPluginSite
    {
        string lyric = "";
        bool complete;
        System.Timers.Timer timer;
        int timeLimit;

        public string Lyric
        {
            get { return lyric; }
        }

        public LyricsPluginSite(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit;
            timer = new System.Timers.Timer();

            artist = LyricUtil.RemoveFeatComment(artist);
            title = LyricUtil.TrimForParenthesis(title);

            // Escape characters
            artist = fixEscapeCharacters(artist);
            title = fixEscapeCharacters(title);

            // Hebrew letters
            artist = fixHebrew(artist);
            title = fixHebrew(title);
            
            string urlString = "http://www.lyricsplugin.com/winamp03/plugin/?" + "artist=" + artist + "&title=" + title;

            timer.Enabled = true;
            timer.Interval = timeLimit;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Uri uri = new Uri(urlString);
            LyricsWebClient client = new LyricsWebClient();
            client.OpenReadCompleted += new System.Net.OpenReadCompletedEventHandler(callbackMethod);
            client.OpenReadAsync(uri);

            while (complete == false)
            {
                if (m_EventStop_SiteSearches.WaitOne(1, true))
                {
                    complete = true;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        private static string fixEscapeCharacters(string text)
        {
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("#", "");
            text = text.Replace("/", "");
            
            text = text.Replace("%", "%25");

            text = text.Replace(" ", "%20");
            text = text.Replace("$", "%24");
            text = text.Replace("&", "%26");
            text = text.Replace("+", "%2B");
            text = text.Replace(",", "%2C");
            text = text.Replace(":", "%3A");
            text = text.Replace(";", "%3B");
            text = text.Replace("=", "%3D");
            text = text.Replace("?", "%3F");
            text = text.Replace("@", "%40");
            
            return text;
        }

        private static string fixHebrew(string text)
        {
            text = text.Replace("\uC3A0", "%d7%90"); // à
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
            text = text.Replace("\uC3BA", "%D7%AA");  // ú

            text = text.Replace("\uD790", "%d7%90");  // à
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
            text = text.Replace("\uD7AA", "%D7%AA");  // ú

            return text;
        }

        private void callbackMethod(object sender, OpenReadCompletedEventArgs e)
        {
            bool thisMayBeTheCorrectLyric = true;
            StringBuilder lyricTemp = new StringBuilder();

            LyricsWebClient client = (LyricsWebClient)sender;
            Stream reply = null;
            StreamReader sr = null;

            try
            {
                reply = (Stream)e.Result;
                sr = new StreamReader(reply, Encoding.UTF8);

                string line = "";
                int noOfLinesCount = 0;

                while (line.IndexOf(@"<div id=""lyrics"">") == -1)
                {
                    if (sr.EndOfStream || ++noOfLinesCount > 300)
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
                    lyricTemp = new StringBuilder();
                    line = sr.ReadLine();

                    while (line.IndexOf("</div>") == -1)
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

                    lyricTemp.Replace("<br>", Environment.NewLine);
                    lyricTemp.Replace("<br />", Environment.NewLine);
                    lyricTemp.Replace("&quot;", "\"");

                    lyric = lyricTemp.ToString().Trim();

                    if (lyric.Length == 0)
                    {
                        lyric = "Not found";
                    }
                }
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

        void timer_Elapsed(object sender, ElapsedEventArgs e)
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
