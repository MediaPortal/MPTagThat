using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Timers;

namespace LyricsEngine.LyricSites
{
    class LyricsOnDemand
    {
        string lyric = "";
        bool complete;
        System.Timers.Timer timer;
        int timeLimit;

        public string Lyric
        {
            get { return lyric; }
        }

        public LyricsOnDemand(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit;
            timer = new System.Timers.Timer();

            if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Information, 0, LyricDiagnostics.ElapsedTimeString() + "LyricsOnDemand(" + artist + ", " + title + ")");

            artist = LyricUtil.RemoveFeatComment(artist);
            artist = LyricUtil.TrimForParenthesis(artist);
            artist = LyricUtil.DeleteSpecificChars(artist);
            artist = artist.Replace(" ", "");
            artist = artist.Replace("The ", "");
            artist = artist.Replace("the ", "");
            artist = artist.Replace("-", "");

            artist = artist.ToLower();

            // Cannot find lyrics contaning non-English letters!

            title = LyricUtil.RemoveFeatComment(title);
            title = LyricUtil.TrimForParenthesis(title);
            title = LyricUtil.DeleteSpecificChars(title);
            title = title.Replace(" ", "");
            title = title.Replace("#", "");
            artist = artist.Replace("-", "");

            // Danish letters
            title = title.Replace("æ", "");
            title = title.Replace("ø", "");
            title = title.Replace("å", "");
            title = title.Replace("Æ", "");
            title = title.Replace("Ø", "");
            title = title.Replace("Å", "");
            title = title.Replace("ö", "");
            title = title.Replace("Ö", "");

            title = title.ToLower();

            string firstLetter = "";
            if (artist.Length > 0)
                firstLetter = artist[0].ToString();

            int firstNumber = 0;
            if (int.TryParse(firstLetter, out firstNumber))
            {
                firstLetter = "0";
            }

            string urlString = "http://www.lyricsondemand.com/" + firstLetter + "/" + artist + "lyrics/" + title + "lyrics.html";

            LyricsWebClient client = new LyricsWebClient();

            timer.Enabled = true;
            timer.Interval = timeLimit;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Uri uri = new Uri(urlString);
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
                sr = new StreamReader(reply, Encoding.Default);

                string line = "";
                int noOfLinesCount = 0;

                while (line.IndexOf(@"<font size=""2"" face=""Verdana"">") == -1)
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
                    line = sr.ReadLine().Trim();

                    while (line.IndexOf("<BR><BR>") == -1)
                    {
                        lyricTemp.Append(line);
                        if (sr.EndOfStream || ++noOfLinesCount > 300)
                        {
                            thisMayBeTheCorrectLyric = false;
                            break;
                        }
                        else
                        {
                            line = sr.ReadLine().Trim();
                        }
                    }

                    lyricTemp.Replace("<br>", " \r\n");
                    lyricTemp.Replace("</font></p>", " \r\n");
                    lyricTemp.Replace("<p><font size=\"2\" face=\"Verdana\">", " \r\n");
                    lyricTemp.Replace("<i>", "");
                    lyricTemp.Replace("</i>", "");
                    lyricTemp.Replace("*", "");
                    lyricTemp.Replace("?s", "'s");
                    lyricTemp.Replace("?t", "'t");
                    lyricTemp.Replace("?m", "'m");
                    lyricTemp.Replace("?l", "'l");
                    lyricTemp.Replace("?v", "'v");
                    lyricTemp.Replace("<p>", " \r\n");
                    lyricTemp.Replace("<BR>", " \r\n");
                    lyricTemp.Replace("<br />", " \r\n");
                    lyricTemp.Replace("&#039;", "'");

                    lyric = lyricTemp.ToString().Trim();

                    if (lyric.Contains("<td") || lyric.Contains("<IFRAME"))
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



