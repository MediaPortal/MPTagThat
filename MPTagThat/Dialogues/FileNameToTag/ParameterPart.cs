using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.FileNameToTag
{
  public class ParameterPart
  {
    private string[] delimiters;
    private List<string> parms = new List<string>();

    #region Properties
    public string[] Delimiters
    {
      get
      {
        return delimiters;
      }
    }

    public List<string> Parameters
    {
      get
      {
        return parms;
      }
    }
    #endregion

    public ParameterPart(string parm)
    {
      string str = parm;
      str = str.Replace("<A>", "\x0001"); // Artist
      str = str.Replace("<T>", "\x0001"); // Title
      str = str.Replace("<B>", "\x0001"); // Album
      str = str.Replace("<G>", "\x0001"); // Genre
      str = str.Replace("<C>", "\x0001"); // Comment
      str = str.Replace("<Y>", "\x0001"); // Year
      str = str.Replace("<X>", "\x0001"); // Unused
      str = str.Replace("<O>", "\x0001"); // Album Artist / Orchestra / Band
      str = str.Replace("<D>", "\x0001"); // Disk (Position in Mediaset)
      str = str.Replace("<d>", "\x0001"); // Total # of Disks
      str = str.Replace("<K>", "\x0001"); // Track
      str = str.Replace("<k>", "\x0001"); // Total # of Tracks
      str = str.Replace("<N>", "\x0001"); // Conductor
      str = str.Replace("<R>", "\x0001"); // Composer
      str = str.Replace("<U>", "\x0001"); // Grouping
      str = str.Replace("<S>", "\x0001"); // Subtitle
      str = str.Replace("<M>", "\x0001"); // Modified by
      str = str.Replace("<E>", "\x0001"); // BPM

      delimiters = str.Split(new char[] { '\x0001' });
      str = parm;

      int upperBound = delimiters.GetUpperBound(0);
      for (int i = 0; i <= upperBound; i++)
      {
        if ((i == upperBound) | (delimiters[i] != ""))
        {
          parms.Add(str.Substring(str.IndexOf("<"), 3));
          str = str.Substring(str.IndexOf("<") + 3);
        }
      }
    }
  }
}
