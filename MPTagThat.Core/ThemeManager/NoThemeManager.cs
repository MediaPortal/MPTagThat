using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  class NoThemeManager : IThemeManager
  {
    #region IThemeManager Members

    public void ChangeTheme(string aThemeName)
    {
    }

    public void NotifyThemeChange()
    {
    }

    public Theme CurrentTheme
    {
      get { return new Theme();}
    }

    #endregion
  }
}
