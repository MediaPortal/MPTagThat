using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class OrganiseFormatSettings : ParameterFormat
  {
    #region Variable
    private List<string> _lastUsedFolders = new List<string>();
    private int _lastUsedFolderIndex = -1;
    private bool _overWriteFiles;
    private bool _copyFiles;
    private bool _copyNonMusicFiles;
    #endregion

    #region Properties
    [Setting(SettingScope.User, "")]
    public List<string> LastUsedFolders
    {
      get { return _lastUsedFolders; }
      set { _lastUsedFolders = value; }
    }

    [Setting(SettingScope.User, "-1")]
    public int LastUsedFolderIndex
    {
      get { return _lastUsedFolderIndex; }
      set { _lastUsedFolderIndex = value; }
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
