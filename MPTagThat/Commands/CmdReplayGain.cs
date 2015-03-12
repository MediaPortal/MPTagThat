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
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.GridView;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace MPTagThat.Commands
{
  [SupportedCommandType("ReplayGain")]
  public class CmdReplayGain : Command
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private bool albumGain = false;
    private bool gainInitialised = false;
    private int usedFrequency = -1;
    private float maxPeak = 0.0f;

    #endregion

    #region Imports

    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int InitGainAnalysis(long samplefreq);
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ResetSampleFrequency(long samplefreq);
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int AnalyzeSamples(double[] left_samples, double[] right_samples, UIntPtr num_samples, int num_channels);
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern float GetTitleGain();
    [DllImport("gain.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern float GetAlbumGain();

    #endregion

    #region ctor

    public CmdReplayGain(object[] parameters)
    {
      NeedsPreprocessing = true;
    }

    #endregion


    #region Command Implementation

    public override bool Execute(ref TrackData track, GridViewTracks tracksGrid,int rowIndex)
    {
      int stream = Bass.BASS_StreamCreateFile(track.FullFileName, 0, 0, BASSFlag.BASS_STREAM_DECODE);
      if (stream == 0)
      {
        Log.Error("ReplayGain: Could not create stream for {0}. {1}", track.FullFileName, Bass.BASS_ErrorGetCode());
        return false;
      }

      BASS_CHANNELINFO chInfo = Bass.BASS_ChannelGetInfo(stream);
      if (chInfo == null)
      {
        Log.Error("ReplayGain: Could not get channel info for {0}. {1}", track.FullFileName, Bass.BASS_ErrorGetCode());
        return false;
      }

      Util.SendProgress(string.Format("Analysing gain for {0}", track.FileName));
      Log.Info("ReplayGain: Start gain analysis for: {0}", track.FullFileName);

      if (!albumGain || !gainInitialised)
      {
        InitGainAnalysis(chInfo.freq);
        gainInitialised = true;
        usedFrequency = chInfo.freq;
      }
      else
      {
        if (usedFrequency != chInfo.freq)
        {
          ResetSampleFrequency(chInfo.freq);
          usedFrequency = chInfo.freq;
        }
      }

      ReplayAnalyze(stream);
      float titleGain = GetTitleGain();

      // Calculating the peak level
      float peak = 0;
      Bass.BASS_ChannelSetPosition(stream, 0);

      float[] level = new float[chInfo.chans];  // allocate for all Channels of the stream
      while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
      { // not reached the end yet
        if (Bass.BASS_ChannelGetLevel(stream, level))
        {
          for (int i = 0; i < chInfo.chans; i++)
          {
            if (peak < level[i]) peak = level[i];
          }
        }
      }

      if (albumGain && maxPeak < peak)
      {
        maxPeak = peak;
      }

      Bass.BASS_StreamFree(stream);

      Log.Info("ReplayGain: Finished analysis. Gain: {0} Peak level: {1}", titleGain.ToString(CultureInfo.InvariantCulture), peak.ToString(CultureInfo.InvariantCulture));
      track.ReplayGainTrack = titleGain.ToString(CultureInfo.InvariantCulture);
      track.ReplayGainTrackPeak = peak.ToString(CultureInfo.InvariantCulture);
      return true;
    }

    /// <summary>
    /// Do a Replaygain and Peak analyses
    /// </summary>
    /// <param name="channel"></param>
    void ReplayAnalyze(int channel)
    {
      int result;
      int peak = 0;

      int[] chanmap = new int[2];
      chanmap[0] = 0; // left channel
      chanmap[1] = -1;
      int stream1 = BassMix.BASS_Split_StreamCreate(channel, BASSFlag.BASS_STREAM_DECODE, chanmap);
      chanmap[0] = 1; // right channel
      int stream2 = BassMix.BASS_Split_StreamCreate(channel, BASSFlag.BASS_STREAM_DECODE, chanmap);

      int length = (int)Bass.BASS_ChannelSeconds2Bytes(channel, 0.02); // 20ms window

      Int16[] buffer = new Int16[length / 2];
      double[] leftSamples = new double[length / 2];
      double[] rightSamples = new double[length / 2];

      Bass.BASS_ChannelSetPosition(stream1, 0); // make sure to start from the beginning
      Bass.BASS_ChannelSetPosition(stream2, 0); // make sure to start from the beginning
      while (Bass.BASS_ChannelIsActive(stream1) == BASSActive.BASS_ACTIVE_PLAYING
        || Bass.BASS_ChannelIsActive(stream2) == BASSActive.BASS_ACTIVE_PLAYING)
      {
        result = Bass.BASS_ChannelGetData(stream1, buffer, length);

        int l4 = result / 2;
        for (int a = 0; a < l4; a++)
        {
          leftSamples[a] = Convert.ToDouble(buffer[a]);
        }

        result = Bass.BASS_ChannelGetData(stream2, buffer, length);
        l4 = result / 2;
        for (int a = 0; a < l4; a++)
        {
          rightSamples[a] = Convert.ToDouble(buffer[a]);
        }

        AnalyzeSamples(leftSamples, rightSamples, new UIntPtr((uint)l4 / 2), 2);
        leftSamples = new double[length / 2];
        rightSamples = new double[length / 2];
      }
    }

    /// <summary>
    /// Do Preprocessing of the Tracks
    /// </summary>
    /// <param name="track"></param>
    /// <param name="tracksGrid"></param>
    /// <returns></returns>
    public override bool PreProcess(TrackData track, GridViewTracks tracksGrid)
    {
      // Check, if all rows have been selected and provide the option to invoke Album Gain analysis
      if (!albumGain && tracksGrid.View.Rows.Count == tracksGrid.View.SelectedRows.Count)
      {
        if (MessageBox.Show(localisation.ToString("albumgain", "Explanation"),
                 localisation.ToString("albumgain", "Header"), MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          albumGain = true;
        }
      }
      return true;
    }

    /// <summary>
    /// Post Process after command execution
    /// </summary>
    /// <param name="tracksGrid"></param>
    /// <returns></returns>
    public override bool PostProcess(GridViewTracks tracksGrid)
    {
      // Should we also get Album Gain
      if (albumGain)
      {
        float albumGainValue = GetAlbumGain();
        string albumGainValueStr = albumGainValue.ToString(CultureInfo.InvariantCulture);
        string albumPeakValueStr = maxPeak.ToString(CultureInfo.InvariantCulture);

        foreach (DataGridViewRow row in tracksGrid.View.Rows)
        {
          if (!row.Selected)
          {
            continue;
          }

          TrackData track = Options.Songlist[row.Index];
          track.ReplayGainAlbum = albumGainValueStr;
          track.ReplayGainAlbumPeak = albumPeakValueStr;
          track.Changed = true;
          Options.Songlist[row.Index] = track;
        }
      }
      return true;
    }

    #endregion
  }
}