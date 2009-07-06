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
  class SeekLyrics
  {
    string lyric = "";
    bool complete;
    System.Timers.Timer timer;
    int timeLimit;

    public string Lyric
    {
      get { return lyric; }
    }

    public SeekLyrics(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
    {
      this.timeLimit = timeLimit;
      timer = new System.Timers.Timer();

      artist = LyricUtil.RemoveFeatComment(artist);
      artist = artist.Replace(" ", "-");
      artist = artist.Replace("'", "-");
      artist = artist.Replace("(", "");
      artist = artist.Replace(")", "");
      artist = artist.Replace(",", "");
      artist = artist.Replace("#", "");

      // German letters
      artist = artist.Replace("ü", "%FC");
      artist = artist.Replace("Ü", "");
      artist = artist.Replace("ä", "%E4");
      artist = artist.Replace("Ä", "");
      artist = artist.Replace("ö", "%E4");  // Not correct!!!
      artist = artist.Replace("Ö", "");
      artist = artist.Replace("ß", "%DF");

      // French letters
      artist = artist.Replace("é", "%E9");

      title = LyricUtil.TrimForParenthesis(title);
      title = title.Replace(" ", "-");
      title = title.Replace("'", "-");
      title = title.Replace("(", "");
      title = title.Replace(")", "");
      title = title.Replace(",", "");
      title = title.Replace("#", "");
      title = title.Replace("?", "");

      // German letters
      title = title.Replace("ü", "%FC");
      title = title.Replace("Ü", "%FC");
      title = title.Replace("ä", "%E4");
      title = title.Replace("Ä", "%C4");
      title = title.Replace("ö", "%F6");
      title = title.Replace("Ö", "%D6");
      title = title.Replace("ß", "%DF");

      // French letters
      title = title.Replace("é", "%E9");


      string urlString = "http://www.seeklyrics.com/lyrics/" + artist + "/" + title + ".html";

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
            LyricsWebClient client = (LyricsWebClient)sender;
      Stream reply = null;
      StreamReader sr = null;

      try
      {
        reply = (Stream)e.Result;
        sr = new StreamReader(reply, Encoding.Default);

        string line = sr.ReadToEnd();

        // Replace the new line stuff in the result, as the regex might have problems
        line = line.Replace("\n", "");
        line = line.Replace("\r", "");
        line = line.Replace("\t", "");

        string pat = @"<a href=""http://www\.ringtonematcher\.com/.*?</a>(.*?)<a href";

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
          lyric = lyric.Replace("?s", "'s");
          lyric = lyric.Replace("?t", "'t");
          lyric = lyric.Replace("?m", "'m");
          lyric = lyric.Replace("?l", "'l");
          lyric = lyric.Replace("?v", "'v");
          lyric = lyric.Replace("<br>", "\r\n");
          lyric = lyric.Replace("<br />", "\r\n");
          lyric = lyric.Replace("&#039;", "'");
          lyric = lyric.Replace("</p>", "");
          lyric = lyric.Replace("<BR>", "");
          lyric = lyric.Replace("<br/>", "\r\n");
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
