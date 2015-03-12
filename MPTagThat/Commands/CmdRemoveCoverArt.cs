#region Copyright (C) 2009-2015 Team MediaPortal
// Copyright (C) 2009-2015 Team MediaPortal
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

using System;
using MPTagThat.Core;
using MPTagThat.GridView;

namespace MPTagThat.Commands
{
  [SupportedCommandType("RemoveCoverArt")]
  public class CmdRemoveCoverArt : ICommand, IDisposable
  {
    #region Variables

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private bool _progressCancelled = false;

    #endregion

    #region ICommand Implementation

    public bool Execute(ref TrackData track, GridViewTracks tracksGrid, int rowIndex)
    {
      if (track.NumPics > 0)
      {
        track.Pictures.Clear();
        return true;
      }
      return false;
    }

    /// <summary>
    /// Indicate, whether we need Preprocess the tracks
    /// </summary>
    /// <returns></returns>
    public bool NeedsPreprocessing()
    {
      return false;
    }

    /// <summary>
    /// Do Preprocessing of the Tracks
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public bool PreProcess(TrackData track)
    {
      return true;
    }

    /// <summary>
    /// Post Process after command execution
    /// </summary>
    /// <param name="tracksGrid"></param>
    /// <returns></returns>
    public bool PostProcess(GridViewTracks tracksGrid)
    {
      tracksGrid.MainForm.SetGalleryItem();
      return false;
    }

    /// <summary>
    /// Set indicator, that Command processing got interupted by user
    /// </summary>
    public void CancelCommand()
    {
      _progressCancelled = true;
    }

    /// <summary>
    /// Cleanup resources
    /// </summary>
    public void Dispose()
    {

    }

    #endregion
  }
}