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

using MPTagThat.Core;

namespace MPTagThat.Commands
{
  [SupportedCommandType("RemoveCoverArt")]
  public class CmdRemoveCoverArt : Command
  {
    public object[] Parameters { get; private set; }

    #region Variables

    #endregion

    #region ctor

    public CmdRemoveCoverArt(object[] parameters)
    {
      Parameters = parameters;
    }

    #endregion

    #region Command Implementation

    public override bool Execute(ref TrackData track, int rowIndex)
    {
      if (track.NumPics > 0)
      {
        track.Pictures.Clear();
        return true;
      }
      return false;
    }

    /// <summary>
    /// Post Process after command execution
    /// </summary>
    /// <returns></returns>
    public override bool PostProcess()
    {
      TracksGrid.MainForm.SetGalleryItem();
      return false;
    }

    #endregion
  }
}