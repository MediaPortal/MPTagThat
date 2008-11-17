using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class CaseConversionSettings
  {
    #region Variables
    private List<string> _caseConvExceptions = new List<string>();
    private bool _convertFileName;
    private bool _convertTags;
    private bool _convertArtist;
    private bool _convertAlbumArtist;
    private bool _convertAlbum;
    private bool _convertTitle;
    private bool _convertComment;
    private bool _convertAllLower;
    private bool _convertAllUpper;
    private bool _convertFirstUpper;
    private bool _convertAllFirstUpper;
    private bool _replace20BySpace;
    private bool _replaceSpaceBy20;
    private bool _replaceUnderscoreBySpace;
    private bool _replaceSpaceByUnderscore;
    private bool _convertAllwaysFirstUpper;
    #endregion

    #region Properties
    [Setting(SettingScope.User, "true")]
    public bool ConvertFileName
    {
      get { return _convertFileName; }
      set { _convertFileName = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertTags
    {
      get { return _convertTags; }
      set { _convertTags = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertArtist
    {
      get { return _convertArtist; }
      set { _convertArtist = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAlbumArtist
    {
      get { return _convertAlbumArtist; }
      set { _convertAlbumArtist = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAlbum
    {
      get { return _convertAlbum; }
      set { _convertAlbum = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertTitle
    {
      get { return _convertTitle; }
      set { _convertTitle = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertComment
    {
      get { return _convertComment; }
      set { _convertComment = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAllWaysFirstUpper
    {
      get { return _convertAllwaysFirstUpper; }
      set { _convertAllwaysFirstUpper = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ConvertAllLower
    {
      get { return _convertAllLower; }
      set { _convertAllLower = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ConvertAllUpper
    {
      get { return _convertAllUpper; }
      set { _convertAllUpper = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ConvertFirstUpper
    {
      get { return _convertFirstUpper; }
      set { _convertFirstUpper = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAllFirstUpper
    {
      get { return _convertAllFirstUpper; }
      set { _convertAllFirstUpper = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool Replace20BySpace
    {
      get { return _replace20BySpace; }
      set { _replace20BySpace = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ReplaceSpaceBy20
    {
      get { return _replaceSpaceBy20; }
      set { _replaceSpaceBy20 = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ReplaceUnderscoreBySpace
    {
      get { return _replaceUnderscoreBySpace; }
      set { _replaceUnderscoreBySpace = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ReplaceSpaceByUnderscore
    {
      get { return _replaceSpaceByUnderscore; }
      set { _replaceSpaceByUnderscore = value; }
    }

    [Setting(SettingScope.User, "")]
    public List<string> CaseConvExceptions
    {
      get { return _caseConvExceptions; }
      set { _caseConvExceptions = value; }
    }
    #endregion
  }
}
