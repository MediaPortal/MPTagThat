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
  ///   Default <see cref = "ILogger" /> implementation that does absolutely nothing.
  /// </summary>
  internal class NoLogger : ILogger
  {
    #region ILogger Members

    public NLog.Logger GetLogger
    {
      get { return null; }
    }

    /// <summary>
    ///   Gets or sets the log level.
    /// </summary>
    /// <value>A <see cref = "LogLevel" /> value that indicates the minimum level messages must have to be 
    ///   written to the file.</value>
    public NLog.LogLevel Level
    {
      get { return NLog.LogLevel.Off; }
      set { }
    }

    #endregion
  }
}