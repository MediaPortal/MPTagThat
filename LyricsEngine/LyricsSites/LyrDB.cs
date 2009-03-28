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
    class LyrDB
    {
        string lyric = "";
        bool complete;
        System.Timers.Timer timer;
        int timeLimit;
        private ManualResetEvent m_EventStop_SiteSearches;

        public string Lyric
        {
            get { return lyric; }
        }

        public LyrDB(string artist, string title, ManualResetEvent eventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit;
            timer = new System.Timers.Timer();

            m_EventStop_SiteSearches = eventStop_SiteSearches;

            if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Information, 0, LyricDiagnostics.ElapsedTimeString() + "LyrDB(" + artist + ", " + title + ")");

            artist = LyricUtil.RemoveFeatComment(artist);
            artist = LyricUtil.TrimForParenthesis(artist);
            title = LyricUtil.RemoveFeatComment(title);
            title = LyricUtil.TrimForParenthesis(title);
            //string urlString = string.Format("http://www.lyrdb.com/lookup.php?q={0}|{1}&for=match", artist, title);
            string urlString = string.Format("http://webservices.lyrdb.com/lookup.php?q={0}%7c{1}&for=match", artist, title);
        

            LyricsWebClient client = new LyricsWebClient();

            timer.Enabled = true;
            timer.Interval = timeLimit;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Uri uri = new Uri(urlString);
            client.OpenReadCompleted += new System.Net.OpenReadCompletedEventHandler(CallbackMethodSearch);
            client.OpenReadAsync(uri);

            while (complete == false)
            {
                if (m_EventStop_SiteSearches.WaitOne(1, true))
                {
                    complete = true;
                }
                else
                {
                    System.Threading.Thread.Sleep(300);
                }
            }
        }

        private void CallbackMethodSearch(object sender, OpenReadCompletedEventArgs e)
        {
            StringBuilder lyricTemp = new StringBuilder();

            LyricsWebClient client = (LyricsWebClient)sender;
            Stream reply = null;
            StreamReader sr = null;

            string id = string.Empty;

            try
            {
                reply = (Stream)e.Result;
                sr = new StreamReader(reply);

                string result = sr.ReadToEnd();

                if (result.Equals(""))
                {
                    lyric = "Not found";
                    return;
                }

                id = result.Substring(0, result.IndexOf(@"\"));

                string urlString = string.Format("http://www.lyrdb.com/getlyr.php?q={0}", id);

                LyricsWebClient client2 = new LyricsWebClient();

                Uri uri = new Uri(urlString);
                client2.OpenReadCompleted += new System.Net.OpenReadCompletedEventHandler(CallbackMethodGetLyric);
                client2.OpenReadAsync(uri);

                while (complete == false)
                {
                    if (m_EventStop_SiteSearches.WaitOne(1, true))
                    {
                        complete = true;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(300);
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
            }
        }


        private void CallbackMethodGetLyric(object sender, OpenReadCompletedEventArgs e)
        {
            StringBuilder lyricTemp = new StringBuilder();

            LyricsWebClient client = (LyricsWebClient)sender;
            Stream reply = null;
            StreamReader sr = null;

            try
            {
                reply = (Stream)e.Result;
                sr = new StreamReader(reply);

                lyric = sr.ReadToEnd().Trim();

                //lyricTemp.Replace("<br>", Environment.NewLine);
                //lyricTemp.Replace(",<br />", Environment.NewLine);
                //lyricTemp.Replace("<br />", Environment.NewLine);

                //lyric = lyricTemp.ToString().Trim();

                //if (lyric.Contains("but we do not have the lyrics"))
                //{
                //    lyric = "Not found";
                //}
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
