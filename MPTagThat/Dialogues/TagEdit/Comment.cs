using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.TagEdit
{
  public class Comment
  {
    #region Variables
    private string _commentDescriptor;
    private string _commentLanguge;
    private string _commentText;

    public Comment()
    {

    }

    public Comment(string desc, string lang, string text)
    {
      _commentDescriptor = desc;
      if (lang.Length > 3)
        _commentLanguge = lang.Substring(0, 3);
      else
        _commentLanguge = lang;

      _commentText = text;
    }

    public string Description
    {
      get { return _commentDescriptor; }
      set { _commentDescriptor = value; }
    }

    public string Language
    {
      get { return _commentLanguge; }
      set { _commentLanguge = value; }

    }

    public string Text
    {
      get { return _commentText; }
      set { _commentText = value; }
    }
    #endregion
  }
}
