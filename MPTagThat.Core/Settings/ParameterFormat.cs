using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class ParameterFormat
  {
    #region Variables
    private List<string> _formatValues = new List<string>();
    private int _lastUsedFormat;
    #endregion

    #region Properties
    [Setting(SettingScope.User, "")]
    public List<string> FormatValues
    {
      get { return _formatValues; }
      set { _formatValues = value; }
    }

    [Setting(SettingScope.User, "-1")]
    public int LastUsedFormat
    {
      get { return _lastUsedFormat; }
      set { _lastUsedFormat = value; }
    }
    #endregion
  }
}
