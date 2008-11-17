using System;
using System.Collections.Generic;
using System.Text;
using MPTagThat.Core;

namespace MPTagThat.Core.WinControls
{
  public sealed class MPTWinControlsCommon
  {
    #region Variables
    private static ILocalisation localisation = null;

    #endregion

    #region ctor
    static MPTWinControlsCommon()
    {
      try
      {
        localisation = ServiceScope.Get<ILocalisation>();
      }
      catch (ServiceNotFoundException)
      {
        localisation = null;
      }
    }
    #endregion

    #region Public Methods
    public static string Localise(string context, string text)
    {
      if (localisation != null)
        return localisation.ToString(context, text);
      else
        return "";
    }
    #endregion
  }
}
