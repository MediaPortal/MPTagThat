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
  internal class NoLocalisation : ILocalisation
  {
    #region ILocalisation Members

    public CultureInfo CurrentCulture
    {
      get { return CultureInfo.CurrentUICulture; }
    }

    public int Characters
    {
      get { return 1; }
    }

    /// <summary>
    ///   Changes the language.
    /// </summary>
    /// <param name = "cultureName">Name of the culture.</param>
    public void ChangeLanguage(string cultureName) {}

    /// <summary>
    ///   Get the translation for a given id and format the sting with
    ///   the given parameters
    /// </summary>
    /// <param name = "id">id of text</param>
    /// <param name = "parameters">parameters used in the formating</param>
    /// <returns>
    ///   string containing the translated text
    /// </returns>
    public string ToString(string section, string id, object[] parameters)
    {
      return null;
    }

    /// <summary>
    ///   Get the translation for a given id
    /// </summary>
    /// <param name = "id">id of text</param>
    /// <returns>
    ///   string containing the translated text
    /// </returns>
    public string ToString(string section, string id)
    {
      return null;
    }

    public string ToString(StringId id)
    {
      return id.ToString();
    }

    public bool IsLocalSupported()
    {
      return false;
    }

    public CultureInfo[] AvailableLanguages()
    {
      return new[] {CultureInfo.CurrentUICulture};
    }

    public CultureInfo GetBestLanguage()
    {
      return CultureInfo.CurrentUICulture;
    }

    #endregion
  }
}