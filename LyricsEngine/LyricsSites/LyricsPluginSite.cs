using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace LyricsEngine.LyricSites
{
    internal class LyricsPluginSite
    {
        private bool firstStepComplete;
        private bool complete;
        private string lyric = "";
        private int timeLimit;
        private Timer timer;
        private string timestamp;
        private string checkCode;

        public LyricsPluginSite(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit;
            timer = new Timer();

            artist = LyricUtil.RemoveFeatComment(artist);
            title = LyricUtil.TrimForParenthesis(title);

            // Escape characters
            artist = fixEscapeCharacters(artist);
            title = fixEscapeCharacters(title);

            // Hebrew letters
            artist = fixHebrew(artist);
            title = fixHebrew(title);

            // timer
            timer.Enabled = true;
            timer.Interval = timeLimit;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            // LyricsPlugin.com changed their API to need 2 calls. The first is to get a timer & code

            // 1st step
            string firstUrlString = "http://www.lyricsplugin.com/winamp03/plugin/?" + "artist=" + artist + "&title=" + title;

            Uri uri1 = new Uri(firstUrlString);
            LyricsWebClient client1 = new LyricsWebClient();
            client1.OpenReadCompleted += new OpenReadCompletedEventHandler(firstCallbackMethod);
            client1.OpenReadAsync(uri1);

            while (firstStepComplete == false)
            {
                if (m_EventStop_SiteSearches.WaitOne(1, true))
                {
                    firstStepComplete = true;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            // assertion: !timestamp.Equals("") & !checkCode.Equals("");

            // 2nd step
            string secondUrlString = "http://www.lyricsplugin.com/winamp03/plugin/content.php?" + "artist=" + artist + "&title=" + title + "&time=" + timestamp + "&check=" + checkCode;

            Uri uri2 = new Uri(secondUrlString);
            LyricsWebClient client2 = new LyricsWebClient(firstUrlString);
            client2.OpenReadCompleted += new OpenReadCompletedEventHandler(secondCallbackMethod);
            client2.OpenReadAsync(uri2);

            while (complete == false)
            {
                if (m_EventStop_SiteSearches.WaitOne(1, true))
                {
                    complete = true;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        public string Lyric
        {
            get { return lyric; }
        }

        private void firstCallbackMethod(object sender, OpenReadCompletedEventArgs e)
        {
            bool thisMayBeTheCorrectPage = true;
            StringBuilder lyricsCommand = new StringBuilder();

            LyricsWebClient client = (LyricsWebClient)sender;
            Stream reply = null;
            StreamReader sr = null;

            try
            {
                reply = (Stream)e.Result;
                sr = new StreamReader(reply, Encoding.UTF8);

                string line = "";
                int noOfLinesCount = 0;

                while (line.IndexOf(@"getContent(") == -1)
                {
                    if (sr.EndOfStream || ++noOfLinesCount > 300)
                    {
                        thisMayBeTheCorrectPage = false;
                        break;
                    }
                    else
                    {
                        line = sr.ReadLine();
                    }
                }

                if (thisMayBeTheCorrectPage)
                {
                    lyricsCommand.Append(line);

                    string[] parameters = line.Split('\'');

                    // todo - Find a better check here
                    if (parameters.Length == 9)
                    {
                        timestamp = parameters[5];
                        checkCode = parameters[7];
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
                firstStepComplete = true;
            }
        }

        private void secondCallbackMethod(object sender, OpenReadCompletedEventArgs e)
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

                    // Clean lyrics
                    lyric = cleanLyrics(lyricTemp);

                    if (lyric.Length == 0 || (lyric.Contains("<") || lyric.Contains(">") || lyric.Contains("a href") || lyric.ToLower().Contains("www")))
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

        private string cleanLyrics(StringBuilder lyricTemp)
        {
            lyricTemp.Replace("<br>", Environment.NewLine);
            lyricTemp.Replace("<br />", Environment.NewLine);
            lyricTemp.Replace("&quot;", "\"");

            lyricTemp.Replace(@"<a href=""http://www.tunerankings.com/"" target=""_blank"">www.tunerankings.com</a>", string.Empty);

            return lyricTemp.ToString().Trim();
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
            text = text.Replace("&amp;", "&");

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
            text = text.Replace("\uC3BA", "%D7%AA"); // ú

            text = text.Replace("\uD790", "%d7%90"); // à
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
            text = text.Replace("\uD7AA", "%D7%AA"); // ú

            return text;
        }
    }
}