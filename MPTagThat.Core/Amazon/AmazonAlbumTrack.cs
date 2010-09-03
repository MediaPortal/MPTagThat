using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core.Amazon
{
  public class AmazonAlbumTrack
  {
    #region Variables
    private int _number;
    private string _title;
    #endregion

    #region Properties
    public int Number
    {
      get { return _number; }
      set { _number = value; }
    }

    public string Title
    {
      get { return _title; }
      set { _title = value; }
    }
    #endregion
  }
}
