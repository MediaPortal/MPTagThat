using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Player
{
  public class PlayListData
  {
    #region Variables
    private string _title;
    private string _duration;
    private string _fileName;
    #endregion

    #region Properties
    public string Title 
    { 
      get { return _title; } 
      set { _title = value; } 
    }

    public string Duration
    {
      get { return _duration; }
      set { _duration = value; }
    }

    public string FileName
    {
      get { return _fileName; }
      set { _fileName = value; }
    }
    #endregion
  }
}
