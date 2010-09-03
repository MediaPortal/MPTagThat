using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.TagEdit
{
  public class Lyric
  {
    #region Variables
    private string _lyricsDescriptor;
    private string _lyricsLanguge;
    private string _lyricsText;

    public Lyric()
    {

    }

    public Lyric(string desc, string lang, string text)
    {
      _lyricsDescriptor = desc;
      _lyricsLanguge = lang.Substring(0,3);
      _lyricsText = text;
    }

    public string Description
    {
      get { return _lyricsDescriptor; }
      set { _lyricsDescriptor = value; }
    }

    public string Language
    {
      get { return _lyricsLanguge; }
      set { _lyricsLanguge = value; }

    }

    public string Text
    {
      get { return _lyricsText; }
      set { _lyricsText = value; }
    }
    #endregion
  }
}
