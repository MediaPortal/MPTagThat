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
    class HotLyrics
    {
        string lyric = "";
        private bool complete;
        System.Timers.Timer timer;
        int timeLimit;

        public string Lyric
        {
            get { return lyric; }
        }

        public HotLyrics(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit / 2;
            timer = new System.Timers.Timer();

            if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Information, 0, LyricDiagnostics.ElapsedTimeString() + "SeekLyrics(" + artist + ", " + title + ")");

            artist = LyricUtil.RemoveFeatComment(artist);
            artist = LyricUtil.TrimForParenthesis(artist);
            artist = LyricUtil.CapatalizeString(artist);

            artist = artist.Replace(" ", "_");
            artist = artist.Replace(",", "_");
            artist = artist.Replace(".", "_");
            artist = artist.Replace("'", "_");
            artist = artist.Replace("(", "%28");
            artist = artist.Replace(")", "%29");
            artist = artist.Replace(",", "");
            artist = artist.Replace("#", "");
            artist = artist.Replace("%", "");
            artist = artist.Replace("+", "%2B");
            artist = artist.Replace("=", "%3D");
            artist = artist.Replace("-", "_");

            // French letters
            artist = artist.Replace("é", "%E9");

            title = LyricUtil.RemoveFeatComment(title);
            title = LyricUtil.TrimForParenthesis(title);
            title = LyricUtil.CapatalizeString(title);

            title = title.Replace(" ", "_");
            title = title.Replace(",", "_");
            title = title.Replace(".", "_");
            title = title.Replace("'", "_");
            title = title.Replace("(", "%28");
            title = title.Replace(")", "%29");
            title = title.Replace(",", "_");
            title = title.Replace("#", "_");
            title = title.Replace("%", "_");
            title = title.Replace("?", "_");
            title = title.Replace("+", "%2B");
            title = title.Replace("=", "%3D");
            title = title.Replace("-", "_");
            title = title.Replace(":", "_");

            // German letters
            artist = artist.Replace("ü", "%FC");
            artist = artist.Replace("Ü", "%DC");
            artist = artist.Replace("ä", "%E4");
            artist = artist.Replace("Ä", "%C4");
            artist = artist.Replace("ö", "%F6");
            artist = artist.Replace("Ö", "%D6");
            artist = artist.Replace("ß", "%DF");

            // Danish letters
            title = title.Replace("å", "%E5");
            title = title.Replace("Å", "%C5");
            title = title.Replace("æ", "%E6");
            title = title.Replace("ø", "%F8");

            // French letters
            title = title.Replace("é", "%E9");

            string firstLetter = "";
            if (artist.Length > 0)
                firstLetter = artist[0].ToString();

            string urlString = "http://www.hotlyrics.net/lyrics/" + firstLetter + "/" + artist + "/" + title + ".html";

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
                    System.Threading.Thread.Sleep(300);
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

              while (line.IndexOf("GOOGLE END") == -1)
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
                lyricTemp = new StringBuilder();
                line = sr.ReadLine();

                while (line.IndexOf("<script type") == -1)
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

                lyricTemp.Replace("?s", "'s");
                lyricTemp.Replace("?t", "'t");
                lyricTemp.Replace("?m", "'m");
                lyricTemp.Replace("?l", "'l");
                lyricTemp.Replace("?v", "'v");
                lyricTemp.Replace("<br>", "\r\n");
                lyricTemp.Replace("<br />", "\r\n");
                lyricTemp.Replace("&quot;", "\"");
                lyricTemp.Replace("</p>", "");
                lyricTemp.Replace("<BR>", "");
                lyricTemp.Replace("<br/>", "\r\n");

                lyric = lyricTemp.ToString().Trim();

                if (lyric.Contains("<td"))
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

                if (timer != null)
                {
                    timer.Stop();
                    timer.Close();
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
