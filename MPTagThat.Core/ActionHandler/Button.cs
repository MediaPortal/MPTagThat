using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MPTagThat.Core
{
  public class Button
  {
    #region Variables
    private Keys eKeyModifier;
    private int eKeyCode;
    private string eRibbonKeyCode;
    private Action.ActionType eAction;
    private string eDescription;
    #endregion

    #region Properties
    public Keys Modifiers
    {
      get { return eKeyModifier; }
      set { eKeyModifier = value; }
    }

    public int KeyCode
    {
      get { return eKeyCode; }
      set { eKeyCode = value; }
    }

    public string RibbonKeyCode
    {
      get { return eRibbonKeyCode; }
      set { eRibbonKeyCode = value; }
    }

    public Action.ActionType ActionType
    {
      get { return eAction; }
      set { eAction = value; }
    }

    public string Description
    {
      get { return eDescription; }
      set { eDescription = value; }
    }
    #endregion
  }
}
