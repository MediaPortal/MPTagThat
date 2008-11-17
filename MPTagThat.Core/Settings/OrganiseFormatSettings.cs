using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class OrganiseFormatSettings : ParameterFormat
  {
    #region Variable
    private string _lastUsedFolder;
    #endregion

    #region Properties
    [Setting(SettingScope.User, "")]
    public string LastUsedFolder
    {
      get { return _lastUsedFolder; }
      set { _lastUsedFolder = value; }
    }
    #endregion

    #region Public Methods
    public void Save()
    {
      ServiceScope.Get<ISettingsManager>().Save(this);
    }
    #endregion
  }
}
