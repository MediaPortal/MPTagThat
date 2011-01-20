#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion
#region

using System.Collections.Generic;

#endregion

namespace MPTagThat.Core
{
  public class CaseConversionSettings
  {
    #region Variables

    private List<string> _caseConvExceptions = new List<string>();

    #endregion

    #region Properties

    [Setting(SettingScope.User, "true")]
    public bool ConvertFileName { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertTags { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertArtist { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAlbumArtist { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAlbum { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertTitle { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertComment { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAllWaysFirstUpper { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool ConvertAllLower { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool ConvertAllUpper { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool ConvertFirstUpper { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool ConvertAllFirstUpper { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool Replace20BySpace { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool ReplaceSpaceBy20 { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool ReplaceUnderscoreBySpace { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool ReplaceSpaceByUnderscore { get; set; }

    [Setting(SettingScope.User, "")]
    public List<string> CaseConvExceptions
    {
      get { return _caseConvExceptions; }
      set { _caseConvExceptions = value; }
    }

    #endregion
  }
}