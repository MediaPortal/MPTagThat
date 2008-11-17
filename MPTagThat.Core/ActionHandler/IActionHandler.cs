using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MPTagThat.Core
{
  public interface IActionHandler
  {
    bool GetAction(int iWindow, Keys key, ref Action action);
    bool LoadKeyMap();
  }
}
