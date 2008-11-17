using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class WindowMap
  {
    #region Variales
    private int iWindow;
    private string description;
    private List<Button> mapButtons = new List<Button>();
    #endregion


    #region Properties
    public int Window
    {
      get { return iWindow; }
      set { iWindow = value; }
    }

    public List<Button> Buttons
    {
      get { return mapButtons; }
    }

    public string Description
    {
      get { return description; }
      set { description = value; }
    }
    #endregion

    #region ctor
    public WindowMap()
    {
    }
    #endregion
  }
}
