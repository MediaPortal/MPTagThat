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
  /// <summary>
  ///   Main Config Service
  /// </summary>
  public class SettingsManager : ISettingsManager
  {
    private int _portable;
    private int _maxSongs;
		private int _ravenDebug;

		#region ISettingsManager Members

		/// <summary>
		///   Retrieves an object's public properties from an Xml file
		/// </summary>
		/// <param name = "settingsObject">Object's instance</param>
		public void Load(object settingsObject)
    {
      ObjectParser.Deserialize(settingsObject);
    }

    /// <summary>
    ///   Stores an object's public properties to an Xml file
    /// </summary>
    /// <param name = "settingsObject">Object's instance</param>
    public void Save(object settingsObject)
    {
      ObjectParser.Serialize(settingsObject);
    }

    /// <summary>
    ///   Sets the Portable Status
    /// </summary>
    /// <param name = "portable"></param>
    public void SetPortable(int portable)
    {
      _portable = portable;
    }

    /// <summary>
    ///   Gets the Portable Status
    /// </summary>
    /// <returns></returns>
    public int GetPortable()
    {
      return _portable;
    }

    /// <summary>
    /// Sets the maximum of Songs before switching to B Mode
    /// </summary>
    /// <param name="maxsongs"></param>
    public void SetMaxSongs(int maxsongs)
    {
      _maxSongs = maxsongs;
    }

    /// <summary>
    /// Gets the maximum of songs alllowed in List
    /// </summary>
    public int GetMaxSongs()
    {
      return _maxSongs;
    }

		/// <summary>
		/// Sets the Raven Debug Mode
		/// </summary>
		/// <param name="ravendebug"></param>
		public void SetRavenDebug(int ravendebug)
		{
			_ravenDebug = ravendebug;
		}

		/// <summary>
		/// Gets the Raven Debug Mode
		/// </summary>
		public int GetRavenDebug()
		{
			return _ravenDebug;
		}

	  #endregion
  }
}