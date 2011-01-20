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

using System.Globalization;

#endregion

namespace MPTagThat.Core
{
  public class RegionSettings
  {
    #region Variables

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the culture used for localisation
    /// </summary>
    /// <value>The Culture</value>
    [Setting(SettingScope.User, "")]
    public string Culture { get; set; }

    /// <summary>
    ///   Gets or sets the region used for localisation
    /// </summary>
    /// <value>The Region (ISO 2 letters)</value>
    [Setting(SettingScope.Global, "")]
    public string Region { get; set; }

    /// <summary>
    ///   Gets or sets the city used for localisation
    /// </summary>
    /// <value>The city name</value>
    [Setting(SettingScope.Global, "")]
    public string City { get; set; }

    #endregion

    #region static Methods

    public static string LocalCulture()
    {
      return CultureInfo.CurrentCulture.Name;
    }

    public static string LocalRegion()
    {
      return RegionInfo.CurrentRegion.TwoLetterISORegionName;
    }

    #endregion
  }
}