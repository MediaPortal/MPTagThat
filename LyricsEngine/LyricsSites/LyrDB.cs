using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using Timer=System.Timers.Timer;

namespace LyricsEngine.LyricSites
{
    internal class LyrDB
    {
        private bool complete;
        private string lyric = "";
        private ManualResetEvent m_EventStop_SiteSearches;
        private int timeLimit;
        private Timer timer;

        public LyrDB(string artist, string title, ManualResetEvent eventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit;
            timer = new Timer();

            m_EventStop_SiteSearches = eventStop_SiteSearches;

            artist = LyricUtil.RemoveFeatComment(artist);
            title = LyricUtil.TrimForParenthesis(title);
            //string urlString = string.Format("http://www.lyrdb.com/lookup.php?q={0}|{1}&for=match", artist, title);
            string urlString =
                string.Format("http://webservices.lyrdb.com/lookup.php?q={0}%7c{1}&for=match", artist, title);


            LyricsWebClient client = new LyricsWebClient();


            timer.Enabled = true;
            timer.Interval = timeLimit;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Uri uri = new Uri(urlString);
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(CallbackMethodSearch);
            client.OpenReadAsync(uri);

            while (complete == false)
            {
                if (m_EventStop_SiteSearches.WaitOne(1, true))
                {
                    complete = true;
                }
                else
                {
                    Thread.Sleep(300);
                }
            }
        }

        public string Lyric
        {
            get { return lyric; }
        }

        private void CallbackMethodSearch(object sender, OpenReadCompletedEventArgs e)
        {
            StringBuilder lyricTemp = new StringBuilder();

            LyricsWebClient client = (LyricsWebClient) sender;
            Stream reply = null;
            StreamReader sr = null;

            string id = string.Empty;

            try
            {
                reply = (Stream) e.Result;
                sr = new StreamReader(reply, Encoding.Default);

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
                client2.OpenReadCompleted += new OpenReadCompletedEventHandler(CallbackMethodGetLyric);
                client2.OpenReadAsync(uri);

                while (complete == false)
                {
                    if (m_EventStop_SiteSearches.WaitOne(1, true))
                    {
                        complete = true;
                    }
                    else
                    {
                        Thread.Sleep(300);
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

            LyricsWebClient client = (LyricsWebClient) sender;
            Stream reply = null;
            StreamReader sr = null;

            try
            {
                reply = (Stream) e.Result;
                sr = new StreamReader(reply, Encoding.Default);

                lyric = sr.ReadToEnd().Trim();

                lyric = lyric.Replace("*", "");
                lyric = lyric.Replace("&amp;", "&");
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