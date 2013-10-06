using System;

namespace MyLyrics
{
  [Serializable]
  public class LyricsItem
  {
    private string artist;
    private string lyrics;
    private string source;
    private string title;

    public LyricsItem(string artist, string title, string lyrics, string source)
    {
      this.artist = artist;
      this.title = title;
      this.lyrics = lyrics;
      this.source = source;
    }

    #region Properties

    public string Artist
    {
      get { return artist; }
      set { artist = value; }
    }

    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    public string Lyrics
    {
      get { return lyrics; }
      set { lyrics = value; }
    }

    public string Source
    {
      get { return source; }
      set { source = value; }
    }

    #endregion
  }
}