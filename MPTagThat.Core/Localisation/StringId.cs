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

#region

using System.Text.RegularExpressions;

#endregion

namespace MPTagThat.Core
{
  /// <summary>
  ///   Generates string section and id from combo string
  /// </summary>
  public class StringId
  {
    private readonly string _id;
    private readonly string _section;

    public StringId(string section, string id)
    {
      _section = section;
      _id = id;
    }

    public StringId(string skinLabel)
    {
      // Parse string example @mytv#10
      Regex label = new Regex("@(?<section>[a-z]+):(?<id>[a-z][0-9]+)");

      Match combineString = label.Match(skinLabel);

      if (combineString.Success)
      {
        _section = combineString.Groups["section"].Value;
        _id = combineString.Groups["id"].Value;
      }
      else
      {
        ServiceScope.Get<ILogger>().GetLogger.Error("String Manager - Invalid string Id: {0}", skinLabel);
        _section = "system";
        _id = "NotFound";
      }
    }

    public string Section
    {
      get { return _section; }
    }

    public string Id
    {
      get { return _id; }
    }
  }
}