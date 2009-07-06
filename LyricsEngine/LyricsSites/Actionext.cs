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
  class Actionext
  {
    string lyric = "";
    bool complete;
    System.Timers.Timer timer;
    int timeLimit;

    public string Lyric
    {
      get { return lyric; }
    }

    public Actionext(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
    {
      this.timeLimit = timeLimit;
      timer = new System.Timers.Timer();

      artist = LyricUtil.RemoveFeatComment(artist);
      artist = artist.Replace(" ", "_");
      title = LyricUtil.TrimForParenthesis(title);
      title = title.Replace(" ", "_");

            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                return;
            }

      string urlString = "http://www.actionext.com/names_" + artist[0] + "/" + artist + "_lyrics/" + title + ".html";
      urlString = urlString.ToLower();

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
        int noOfLinesCount = 0;

        while (line.IndexOf(@"<div class=""lyrics-text"">") == -1)
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
          lyricTemp.Replace(",<br />", Environment.NewLine);
          lyricTemp.Replace("<br />", Environment.NewLine);

          lyric = lyricTemp.ToString().Trim();

          if (lyric.Contains("but we do not have the lyrics"))
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
