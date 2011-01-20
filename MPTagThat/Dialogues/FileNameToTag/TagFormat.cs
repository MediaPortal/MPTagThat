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
using System.Collections.Generic;

#endregion

namespace MPTagThat.FileNameToTag
{
  public class TagFormat
  {
    #region Variables

    private readonly List<ParameterPart> parameterParts = new List<ParameterPart>();

    #endregion

    #region Properties

    public List<ParameterPart> ParameterParts
    {
      get { return parameterParts; }
    }

    #endregion

    #region Constructor

    /// <summary>
    ///   Parses the Parameter Format to retrieve Tags from Filenames.
    /// </summary>
    /// <param name = "parameterFormat"></param>
    public TagFormat(string parameterFormat)
    {
      parameterParts.Clear();

      // Split the given parameters to see, if folders have been specified
      string[] parms = parameterFormat.Split(new[] {'\\'});
      for (int i = 0; i < parms.Length; i++)
      {
        if (!parms[i].StartsWith("<"))
          parms[i] = String.Format("<X>{0}", parms[i]);

        if (!parms[i].EndsWith(">"))
          parms[i] = String.Format("{0}<X>", parms[i]);
      }
      for (int i = parms.Length - 1; i >= 0; i--)
      {
        ParameterPart part = new ParameterPart(parms[i]);
        parameterParts.Add(part);
      }
    }

    #endregion
  }
}