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
  class Lyrics007
  {
    string lyric = "";
    bool complete;
    System.Timers.Timer timer;
    int timeLimit;

    public string Lyric
    {
      get { return lyric; }
    }

    public Lyrics007(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
    {
      this.timeLimit = timeLimit;
      timer = new System.Timers.Timer();

      if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Information, 0, LyricDiagnostics.ElapsedTimeString() + "Lyrics007(" + artist + ", " + title + ")");

      artist = LyricUtil.RemoveFeatComment(artist);
      artist = LyricUtil.TrimForParenthesis(artist);
      artist = artist.Replace("#", "");
      title = LyricUtil.RemoveFeatComment(title);
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
      WebClient client = (WebClient)sender;
      Stream reply = null;
      StreamReader sr = null;

      try
      {
        reply = (Stream)e.Result;
        sr = new StreamReader(reply);

        string line = sr.ReadToEnd();
        
        // Replace the new line stuff in the result, as the regex might have problems
        line = line.Replace("\n", "");
        line = line.Replace("\r", "");
        line = line.Replace("\t", "");

        string pat = @"<script\s*type=""text/javascript""\s*src=""http://www2\.ringtonematcher\.com/jsstatic/lyrics007\.js""></script>.*?</div>(.*?)<div.*";

        // Compile the regular expression.
        Regex r = new Regex(pat, RegexOptions.IgnoreCase);
        // Match the regular expression pattern against a text string.
        Match m = r.Match(line);
        if (m.Success)
        {
          Group g1 = m.Groups[1];
          foreach (Capture c1 in g1.Captures)
          {
            lyric = c1.Value;
          }
        }

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
          lyric = lyric.Trim();
        }
        else
          lyric = "Not found";
      }
      catch (Exception)
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
