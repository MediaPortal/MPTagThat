﻿#region Copyright (C) 2009-2015 Team MediaPortal
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
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace MPTagThat.Commands
{
  [SupportedCommandType("Bpm")]
  public class CmdBpm : Command
  {
    public object[] Parameters { get; private set; }

    #region Variables

    private BPMPROCESSPROC _bpmProc;

    #endregion

    #region ctor

    public CmdBpm(object[] parameters)
    {
      Parameters = parameters;
    }

    #endregion

    #region Command Implementation

    public override bool Execute(ref TrackData track, int rowIndex)
    {
      TracksGrid.SetProgressBar(100);

      int stream = Bass.BASS_StreamCreateFile(track.FullFileName, 0, 0, BASSFlag.BASS_STREAM_DECODE);
      if (stream == 0)
      {
        Log.Error("BPM: Could not create stream for {0}. {1}", track.FullFileName, Bass.BASS_ErrorGetCode());
        return false;
      }

      _bpmProc = BpmProgressProc;

      double len = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream));
      float bpm = BassFx.BASS_FX_BPM_DecodeGet(stream, 0.0, len, 0, BASSFXBpm.BASS_FX_BPM_BKGRND | BASSFXBpm.BASS_FX_FREESOURCE | BASSFXBpm.BASS_FX_BPM_MULT2,
                                                  _bpmProc, IntPtr.Zero);

      track.BPM = Convert.ToInt32(bpm);
      BassFx.BASS_FX_BPM_Free(stream);
      TracksGrid.MainForm.progressBar1.Value = 0;
      return true;
    }

    private void BpmProgressProc(int channel, float percent, IntPtr userData)
    {
      TracksGrid.MainForm.progressBar1.Value = Convert.ToInt32(percent);
    }

    #endregion
  }
}