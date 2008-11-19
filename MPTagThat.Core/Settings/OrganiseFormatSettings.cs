using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class OrganiseFormatSettings : ParameterFormat
  {
    #region Variable
    private string _lastUsedFolder;
    private bool _overWriteFiles;
    private bool _copyFiles;
    private bool _copyNonMusicFiles;
    #endregion

    #region Properties
    [Setting(SettingScope.User, "")]
    public string LastUsedFolder
    {
      get { return _lastUsedFolder; }
      set { _lastUsedFolder = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool CopyFiles
    {
      get { return _copyFiles; }
      set { _copyFiles = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool OverWriteFiles
    {
      get { return _overWriteFiles; }
      set { _overWriteFiles = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool CopyNonMusicFiles
    {
      get { return _copyNonMusicFiles; }
      set { _copyNonMusicFiles = value; }
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
