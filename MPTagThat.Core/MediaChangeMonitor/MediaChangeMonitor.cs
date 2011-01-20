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
  public class MediaChangeMonitor : IMediaChangeMonitor
  {
    #region Event delegates

    public event MediaInsertedEvent MediaInserted;
    public event MediaRemovedEvent MediaRemoved;

    #endregion

    #region Variables

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private DeviceVolumeMonitor _deviceMonitor;

    #endregion

    #region ctor / dtor

    public MediaChangeMonitor()
    {
    }

    ~MediaChangeMonitor()
    {
      if (_deviceMonitor != null)
        _deviceMonitor.Dispose();

      _deviceMonitor = null;
    }

    #endregion

    #region IMediaChangeMonitor implementation

    public void StartListening(IntPtr aHandle)
    {
      try
      {
        _deviceMonitor = new DeviceVolumeMonitor(aHandle);
        _deviceMonitor.OnVolumeInserted += VolumeInserted;
        _deviceMonitor.OnVolumeRemoved += VolumeRemoved;
        _deviceMonitor.AsynchronousEvents = true;
        _deviceMonitor.Enabled = true;

        log.Info("MediaChangeMonitor: Monitoring System for Media Changes");
      }
      catch (DeviceVolumeMonitorException ex)
      {
        log.Error("MediaChangeMonitor: Error enabling MediaChangeMonitor Service. {0}", ex.Message);
      }
    }

    public void StopListening()
    {
      if (_deviceMonitor != null)
        _deviceMonitor.Dispose();

      _deviceMonitor = null;
    }

    #endregion

    #region Events

    /// <summary>
    ///   The event that gets triggered whenever a new volume is inserted.
    /// </summary>
    private void VolumeInserted(int bitMask)
    {
      string driveLetter = _deviceMonitor.MaskToLogicalPaths(bitMask);
      log.Info("MediaChangeMonitor: Media inserted in drive {0}", driveLetter);

      if (MediaInserted != null)
        MediaInserted(driveLetter);
    }

    /// <summary>
    ///   The event that gets triggered whenever a volume is removed.
    /// </summary>
    private void VolumeRemoved(int bitMask)
    {
      string driveLetter = _deviceMonitor.MaskToLogicalPaths(bitMask);
      log.Info("MediaChangeMonitor: Media removed from drive {0}", driveLetter);

      if (MediaRemoved != null)
        MediaRemoved(driveLetter);
    }

    #endregion
  }
}