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

namespace MPTagThat.Core.MediaChangeMonitor
{
  public delegate void MediaInsertedEvent(string eDriveLetter);

  public delegate void MediaRemovedEvent(string eDriveLetter);

  public interface IMediaChangeMonitor
  {
    /// <summary>
    ///   This event is fired, whenever a media is inserted
    /// </summary>
    event MediaInsertedEvent MediaInserted;

    /// <summary>
    ///   This event is fired, when a media is removed
    /// </summary>
    event MediaRemovedEvent MediaRemoved;

    /// <summary>
    ///   Starts Listening for Volume Change Events
    /// </summary>
    void StartListening(IntPtr aHandle);

    /// <summary>
    ///   Stops Listening for Volume Change Events
    /// </summary>
    void StopListening();
  }
}