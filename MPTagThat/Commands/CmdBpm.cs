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
using System.Runtime.InteropServices;
using MPTagThat.Core;
using MPTagThat.GridView;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace MPTagThat.Commands
{
  [SupportedCommandType("Bpm")]
  public class CmdBpm : Command
  {
    #region Variables

    private GridViewTracks tracksGrid;
    private BPMPROCESSPROC _bpmProc;

    #endregion

    #region ctor

    public CmdBpm(object[] parameters)
    {
    }

    #endregion

    #region Command Implementation

    public override bool Execute(ref TrackData track, GridViewTracks tracksGrid,int rowIndex)
    {
      this.tracksGrid = tracksGrid;

      tracksGrid.SetProgressBar(100);

      int stream = Bass.BASS_StreamCreateFile(track.FullFileName, 0, 0, BASSFlag.BASS_STREAM_DECODE);
      if (stream == 0)
      {
        Log.Error("BPM: Could not create stream for {0}. {1}", track.FullFileName, Bass.BASS_ErrorGetCode());
        return false;
      }

      GCHandle index = GCHandle.Alloc(rowIndex);
      _bpmProc = BPMProgressProc;

      double len = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream));
      float bpm = BassFx.BASS_FX_BPM_DecodeGet(stream, 0.0, len, 0, BASSFXBpm.BASS_FX_BPM_BKGRND | BASSFXBpm.BASS_FX_FREESOURCE | BASSFXBpm.BASS_FX_BPM_MULT2,
                                                  _bpmProc, GCHandle.ToIntPtr(index));

      track.BPM = Convert.ToInt32(bpm);
      BassFx.BASS_FX_BPM_Free(stream);
      tracksGrid.MainForm.progressBar1.Value = 0;
      return true;
    }

    private void BPMProgressProc(int channel, float percent, IntPtr userData)
    {
      GCHandle gch = GCHandle.FromIntPtr(userData);
      int rowIndex = (int)gch.Target;
      tracksGrid.MainForm.progressBar1.Value = Convert.ToInt32(percent);
    }

    #endregion
  }
}