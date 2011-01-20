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

namespace MPTagThat.Core.Burning
{
  public class EventHelper : IDisposable
  {
    private readonly IBurnManager burnManager = ServiceScope.Get<IBurnManager>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

    #region static methods

    private static EventHelper fEventHelper;

    /// <summary>
    ///   Init class.
    /// </summary>
    public static void Init()
    {
      if (fEventHelper == null)
        fEventHelper = new EventHelper();
    }

    public static void DeInit()
    {
      if (fEventHelper != null)
        fEventHelper.Dispose();
    }

    #endregion

    #region constructor

    public EventHelper()
    {
      RegisterEvents();
    }

    #endregion

    #region private functions

    private void RegisterEvents()
    {
      burnManager.BurningFailed += burnManager_BurningFailed;
      burnManager.BurnProgressUpdate += burnManager_BurnProgressUpdate;
    }

    private void DeregisterEvents()
    {
      burnManager.BurningFailed -= burnManager_BurningFailed;
      burnManager.BurnProgressUpdate -= burnManager_BurnProgressUpdate;
    }

    private void burnManager_BurnProgressUpdate(BurnStatus eBurnStatus, int eTrack, int ePercentage)
    {
      log.Info("BurnEvent: Status: {0} ({1})", eBurnStatus.ToString(), Convert.ToString(ePercentage));
    }

    private void burnManager_BurningFailed(BurnResult eBurnResult, ProjectType eProjectType)
    {
      log.Info("BurnEvent: Burning of {0} failed with result: {1}", eProjectType.ToString(), eBurnResult.ToString());
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
      DeregisterEvents();
    }

    #endregion
  }
}