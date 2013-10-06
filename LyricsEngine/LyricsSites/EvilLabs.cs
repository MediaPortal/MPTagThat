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
    class EvilLabs
    {
        string lyric = "";
        bool complete;
        System.Timers.Timer timer;
        int timeLimit;

        public string Lyric
        {
            get { return lyric; }
        }

        public EvilLabs(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit;
            timer = new System.Timers.Timer();

            if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Information, 0, LyricDiagnostics.ElapsedTimeString() + "EvilLabs(" + artist + ", " + title + ")");

            artist = LyricUtil.RemoveFeatComment(artist);
            artist = LyricUtil.TrimForParenthesis(artist);
            artist = artist.Replace(" ", "+");
            title = LyricUtil.RemoveFeatComment(title);
            title = LyricUtil.TrimForParenthesis(title);
            title = title.Replace(" ", "+");
            string urlString = "http://www.evillabs.sk/lyrics/" + artist + "+-+" + title;

            WebClient client = new WebClient();

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
            
            WebClient client = (WebClient)sender;
            Stream reply = null;
            StreamReader sr = null;

            try
            {
                reply = (Stream)e.Result;
                sr = new StreamReader(reply);

                string line = "";
                int noOfLinesCount = 0;

                while (line.IndexOf("</style>") == -1)
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
                    line = sr.ReadLine();
                    lyric = line.Replace("<br>", "\r\n").Trim();

                    // if warning message from Evil Labs' sql-server, then lyric isn't found
                    if (lyric.Contains("<b>Warning</b>") || lyric.Contains("type="))
                    {
                        lyric = "Not found";
                    }
                }
            }
            catch (System.Reflection.TargetInvocationException)
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
