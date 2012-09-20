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
namespace MPTagThat.Core
{
  public interface ISettingsManager
  {
    /// <summary>
    ///   Retrieves an object's public properties from a given Xml file
    /// </summary>
    /// <param name = "settingsObject">Object's instance</param>
    /// <param name = "filename">Xml file wich contains stored datas</param>
    void Load(object settingsObject);

    /// <summary>
    ///   Stores an object's public properties to a given Xml file
    /// </summary>
    /// <param name = "settingsObject">Object's instance</param>
    /// <param name = "filename">Xml file where we wanna store datas</param>
    void Save(object settingsObject);

    /// <summary>
    ///   Sets the Portable Status
    /// </summary>
    /// <param name = "portable"></param>
    void SetPortable(int portable);

    /// <summary>
    ///   Gets the Portable Status
    /// </summary>
    /// <param name = "portable"></param>
    int GetPortable();

    /// <summary>
    /// Sets the maximum of Songs before switching to B Mode
    /// </summary>
    /// <param name="maxsongs"></param>
    void SetMaxSongs(int maxsongs);

    /// <summary>
    /// Gets the maximum of songs alllowed in List
    /// </summary>
    int GetMaxSongs();
  }
}