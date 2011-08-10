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

using System;

#endregion

namespace MPTagThat.Core
{
  /// <summary>
  ///   Enumerator for a setting's scope
  /// </summary>
  public enum SettingScope
  {
    Global = 1, // global setting, doesn't allow per user/per plugin override
    User = 2 // per user setting : allows per user storage
  }

  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class SettingAttribute : Attribute
  {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name = "settingScope">Setting's scope</param>
    /// <param name = "defaultValue">Default value</param>
    public SettingAttribute(SettingScope settingScope, string defaultValue)
    {
      SettingScope = settingScope;
      DefaultValue = defaultValue;
    }

    /// <summary>
    ///   Get/Set setting's scope (User/Global)
    /// </summary>
    public SettingScope SettingScope { get; set; }

    /// <summary>
    ///   Get/Set the default value
    /// </summary>
    public string DefaultValue { get; set; }
  }
}