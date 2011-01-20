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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

#endregion

namespace MPTagThat.Core
{
  public class MP3Val
  {
    #region Variables

    private static List<string> StdOutList;
    private static readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    #endregion

    #region File Validation

    public static TrackData.MP3Error ValidateMp3File(string fileName)
    {
      ValidateOrFixFile(fileName, false);

      // we might have an error in mp3val. the Log should contain the error
      if (StdOutList.Count == 0)
      {
        return TrackData.MP3Error.NoError;
      }

      TrackData.MP3Error error = TrackData.MP3Error.NoError;

      // No errors found
      if (StdOutList[0].Contains("Done!"))
      {
        return TrackData.MP3Error.NoError;
      }
      else if (StdOutList[0].Contains(@"No supported tags in the file"))
      {
        return TrackData.MP3Error.NoError; // Fixed by MPTagThat :-)
      }
      else if (StdOutList[0].Contains(@"Garbage at the beginning of the file "))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"Garbage at the end of the file "))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"MPEG stream error, resynchronized successfully"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"This is a RIFF file, not MPEG stream"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"It seems that file is truncated or there is garbage at the end of the file"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"Wrong number of MPEG frames specified in Xing header"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"Wrong number of MPEG data bytes specified in Xing header"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"Wrong number of MPEG frames specified in VBRI header"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"Wrong number of MPEG data bytes specified in VBRI header"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"Wrong CRC in"))
      {
        error = TrackData.MP3Error.Fixable; // Fixable error
      }
      else if (StdOutList[0].Contains(@"Several APEv2 tags in one file"))
      {
        return TrackData.MP3Error.NoError; // Handled by MPTagThat
      }
      else if (StdOutList[0].Contains(@"Too few MPEG frames "))
      {
        error = TrackData.MP3Error.NonFixable; // Non Fixable error
      }
      else if (StdOutList[0].Contains(@"VBR detected, but no VBR header is present. Seeking may not work properly"))
      {
        error = TrackData.MP3Error.NonFixable; // Non Fixable error
      }
      else if (StdOutList[0].Contains(@"Different MPEG versions or layers in one file"))
      {
        error = TrackData.MP3Error.NonFixable; // Non Fixable error
      }
      else if (StdOutList[0].Contains(@"Non-layer-III frame encountered"))
      {
        error = TrackData.MP3Error.NonFixable; // Non Fixable error
      }

      // This happens, if we fixed an error
      if (StdOutList.Count > 2)
      {
        if (StdOutList[StdOutList.Count - 2].Contains(@"FIXED:"))
        {
          error = TrackData.MP3Error.Fixed;
        }
      }

      return error;
    }

    public static TrackData.MP3Error FixMp3File(string fileName)
    {
      ValidateOrFixFile(fileName, true);
      // we might have an error in mp3val. the Log should contain the error
      if (StdOutList.Count == 0)
      {
        return TrackData.MP3Error.NoError;
      }

      TrackData.MP3Error error = TrackData.MP3Error.NoError;

      // No errors found
      if (StdOutList[0].Contains("Done!"))
      {
        return TrackData.MP3Error.NoError;
      }

      // This happens, if we fixed an error
      if (StdOutList.Count > 2)
      {
        if (StdOutList[StdOutList.Count - 2].Contains(@"FIXED:"))
        {
          error = TrackData.MP3Error.Fixed;
        }
      }

      return error;
    }

    #endregion

    #region Mp3Val Handling

    private static List<string> ValidateOrFixFile(string mp3file, bool fix)
    {
      if (StdOutList == null)
      {
        StdOutList = new List<string>();
      }

      string parm = "-si ";
      if (Options.MainSettings.MP3AutoFix || fix)
      {
        parm += "-f -nb -t ";
      }

      parm = string.Format("{0} \"{1}\"", parm, mp3file);

      return ExecuteProcReturnStdOut(parm, 3000);
    }

    /// <summary>
    ///   Executes commandline processes and parses their output
    /// </summary>
    /// <param name = "aArguments">The arguments to supply for the given process</param>
    /// <param name = "aExpectedTimeoutMs">How long the function will wait until the tool's execution will be aborted</param>
    /// <returns>A list containing the redirected StdOut line by line</returns>
    public static List<string> ExecuteProcReturnStdOut(string aArguments, int aExpectedTimeoutMs)
    {
      StdOutList.Clear();

      Process MP3ValProc = new Process();
      ProcessStartInfo ProcOptions = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"Bin\mp3val.exe"));

      ProcOptions.Arguments = aArguments;
      ProcOptions.UseShellExecute = false; // Important for WorkingDirectory behaviour
      ProcOptions.RedirectStandardError = true; // .NET bug? Some stdout reader abort to early without that!
      ProcOptions.RedirectStandardOutput = true; // The precious data we're after
      ProcOptions.StandardOutputEncoding = Encoding.GetEncoding("ISO-8859-1"); // the output contains "Umlaute", etc.
      ProcOptions.StandardErrorEncoding = Encoding.GetEncoding("ISO-8859-1");
      ProcOptions.CreateNoWindow = true; // Do not spawn a "Dos-Box"      
      ProcOptions.ErrorDialog = false; // Do not open an error box on failure        

      MP3ValProc.OutputDataReceived += StdOutDataReceived;
      MP3ValProc.ErrorDataReceived += StdErrDataReceived;
      MP3ValProc.EnableRaisingEvents = true; // We want to know when and why the process died        
      MP3ValProc.StartInfo = ProcOptions;
      if (File.Exists(ProcOptions.FileName))
      {
        try
        {
          MP3ValProc.Start();
          MP3ValProc.BeginErrorReadLine();
          MP3ValProc.BeginOutputReadLine();

          // wait this many seconds until mp3val has to be finished
          MP3ValProc.WaitForExit(aExpectedTimeoutMs);
          if (MP3ValProc.HasExited && MP3ValProc.ExitCode != 0)
          {
            log.Warn("MP3Val: Did not exit properly with arguments: {0}, exitcode: {1}", aArguments, MP3ValProc.ExitCode);
          }
        }
        catch (Exception ex)
        {
          log.Error("MP3Val: Error executing mp3val: {0}", ex.Message);
        }
      }
      else
      {
        log.Warn("MP3VAL: Could not start {0} because it doesn't exist!", ProcOptions.FileName);
      }

      return StdOutList;
    }

    #endregion

    #region output handler

    private static void StdErrDataReceived(object sendingProc, DataReceivedEventArgs errLine)
    {
      if (!string.IsNullOrEmpty(errLine.Data))
      {
        log.Error("MP3Val: Error executing mp3val: {0}", errLine.Data);
      }
    }

    private static void StdOutDataReceived(object sendingProc, DataReceivedEventArgs e)
    {
      if (!string.IsNullOrEmpty(e.Data))
      {
        if (e.Data.Contains(@"Analyzing file"))
        {
          return;
        }

        StdOutList.Add(e.Data);
      }
    }

    #endregion
  }
}