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

using MPTagThat.Core.Settings;

namespace MPTagThat.Core
{
  /// <summary>
  ///   Default <see cref = "ISettingsManager" /> implementation that does absolutely nothing
  /// </summary>
  /// <remarks>
  /// </remarks>
  internal class NoSettingsManager : ISettingsManager
  {
    #region ISettingsManager Members

    /// <summary>
    ///   Retrieves an object's public properties from a given Xml file
    /// </summary>
    /// <param name = "settingsObject">Object's instance</param>
    public void Load(object settingsObject) {}

    /// <summary>
    ///   Stores an object's public properties to a given Xml file
    /// </summary>
    /// <param name = "settingsObject">Object's instance</param>
    public void Save(object settingsObject) {}

    public StartupSettings StartSettings { get; set; }

    #endregion
  }
}
