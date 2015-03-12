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
  [SupportedCommandType("ValidateMP3File")]
  public class CmdValidateMp3File : Command
  {
    public object[] Parameters { get; private set; }

    #region Variables

    #endregion

   #region ctor

    public CmdValidateMp3File(object[] parameters)
    {
      Parameters = parameters;
    }

    #endregion


    #region Command Implementation

    public override bool Execute(ref TrackData track, int rowIndex)
    {
      if (track.IsMp3)
      {
        Util.SendProgress(string.Format("Validating file {0}", track.FileName));
        string strError;
        track.MP3ValidationError = MP3Val.ValidateMp3File(track.FullFileName, out strError);
        if (track.MP3ValidationError != Util.MP3Error.NoError)
        {
          TracksGrid.SetColorMP3Errors(rowIndex, track.MP3ValidationError);
          track.Status = 3;
          TracksGrid.View.Rows[rowIndex].Cells[0].ToolTipText = strError;
        }
        else
        {
          TracksGrid.View.Rows[rowIndex].Cells[0].ToolTipText = "";
        }
      }

      // We don't want to mark thze file as changed, since no change occured in the Tags
      return false; 
    }

    #endregion
  }
}