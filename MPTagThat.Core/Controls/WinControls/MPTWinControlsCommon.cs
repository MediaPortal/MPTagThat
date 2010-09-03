#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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

namespace MPTagThat.Core.WinControls
{
  public sealed class MPTWinControlsCommon
  {
    #region Variables

    private static readonly ILocalisation localisation;

    #endregion

    #region ctor

    static MPTWinControlsCommon()
    {
      try
      {
        localisation = ServiceScope.Get<ILocalisation>();
      }
      catch (ServiceNotFoundException)
      {
        localisation = null;
      }
    }

    #endregion

    #region Public Methods

    public static string Localise(string context, string text)
    {
      if (localisation != null)
        return localisation.ToString(context, text);
      else
        return "";
    }

    #endregion
  }
}