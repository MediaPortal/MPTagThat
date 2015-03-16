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
using System.IO;
using Elegant.Ui;
using MPTagThat.Core;

namespace MPTagThat.Commands
{
  [SupportedCommandType("Save")]
  [SupportedCommandType("SaveAll")]
  public class CmdSave : Command
  {
    public object[] Parameters { get; private set; }

    #region Variables

    private bool bErrors = false;

    #endregion

    #region ctor

    public CmdSave(object[] parameters)
    {
      Parameters = parameters;

      if ((string) parameters[0] == "SaveAll")
      {
        NeedsCallback = true;
      }
    }

    #endregion

    #region Command Implementation

    public override bool Execute(ref TrackData track, int rowIndex)
    {
      if (!SaveTrack(track, rowIndex))
      {
        bErrors = true;
      }

      // returning false here, since we are setting the Track Status in Save Track
      // and don't want to have it cvhanged again in calling routine
      return false; 
    }

    public override bool PostProcess()
    {
      Options.ReadOnlyFileHandling = 2; //No
      return bErrors;
    }

    #endregion

    #region Private Methods

    /// <summary>
    ///   Does the actual save of the track
    /// </summary>
    /// <param name = "track"></param>
    /// <returns></returns>
    private bool SaveTrack(TrackData track, int rowIndex)
    {
      try
      {
        if (track.Changed)
        {
          Util.SendProgress(string.Format("Saving file {0}", track.FullFileName));
          Log.Debug("Save: Saving track: {0}", track.FullFileName);

          // The track to be saved, may be currently playing. If this is the case stop playback to free the file
          if (track.FullFileName == TracksGrid.MainForm.Player.CurrentSongPlaying)
          {
            Log.Debug("Save: Song is played in Player. Stop playback to free the file");
            TracksGrid.MainForm.Player.Stop();
          }

          if (Options.MainSettings.CopyArtist && track.AlbumArtist == "")
          {
            track.AlbumArtist = track.Artist;
          }

          if (Options.MainSettings.UseCaseConversion)
          {
            CaseConversion.CaseConversion convert = new CaseConversion.CaseConversion(TracksGrid.MainForm, true);
            convert.CaseConvert(track, rowIndex);
            convert.Dispose();
          }

          // Save the file 
          string errorMessage = "";
          if (Track.SaveFile(track, ref errorMessage))
          {
            // If we are in Database mode, we should also update the MediaPortal Database
            if (TracksGrid.MainForm.TreeView.DatabaseMode)
            {
              TracksGrid.UpdateMusicDatabase(track);
            }

            if (RenameFile(track))
            {
              // rename was ok, so get the new file into the binding list
              string ext = Path.GetExtension(track.FileName);
              string newFileName = Path.Combine(Path.GetDirectoryName(track.FullFileName),
                                                String.Format("{0}{1}", Path.GetFileNameWithoutExtension(track.FileName),
                                                              ext));

              track = Track.Create(newFileName);
              Options.Songlist[rowIndex] = track;
            }

            // Check, if we need to create a folder.jpg
            if (!System.IO.File.Exists(Path.Combine(Path.GetDirectoryName(track.FullFileName), "folder.jpg")) &&
                Options.MainSettings.CreateFolderThumb)
            {
              Util.SavePicture(track);
            }

            track.Status = 0;
            TracksGrid.View.Rows[rowIndex].Cells[0].ToolTipText = "";
            track.Changed = false;
            TracksGrid.View.Rows[rowIndex].Tag = "";
            Options.Songlist[rowIndex] = track;
            TracksGrid.SetGridRowColors(rowIndex);
          }
          else
          {
            track.Status = 2;
            TracksGrid.AddErrorMessage(TracksGrid.View.Rows[rowIndex], errorMessage);
          }
        }
      }
      catch (Exception ex)
      {
        Options.Songlist[rowIndex].Status = 2;
        TracksGrid.AddErrorMessage(TracksGrid.View.Rows[rowIndex], ex.Message);
        Log.Error("Save: Error Saving data for row {0}: {1} {2}", rowIndex, ex.Message, ex.StackTrace);
        return false;
      }
      return true;
    }

    /// <summary>
    ///   Rename the file if necessary
    ///   Called by Save and SaveAll
    /// </summary>
    /// <param name = "track"></param>
    private bool RenameFile(TrackData track)
    {
      string originalFileName = Path.GetFileName(track.FullFileName);
      if (originalFileName != track.FileName)
      {
        string ext = Path.GetExtension(track.FileName);
        string filename = Path.GetFileNameWithoutExtension(track.FileName);
        string path = Path.GetDirectoryName(track.FullFileName);
        string newFileName = Path.Combine(path, string.Format("{0}{1}", filename, ext));

        // Check, if the New file name already exists
        // Don't change the newfilename, when only the Case change happened in filename
        int i = 1;
        if (System.IO.File.Exists(newFileName) && originalFileName.ToLowerInvariant() != track.FileName.ToLowerInvariant())
        {
          newFileName = Path.Combine(path, string.Format("{0} ({1}){2}", filename, i, ext));
          while (System.IO.File.Exists(newFileName))
          {
            i++;
            newFileName = Path.Combine(path, string.Format("{0} ({1}){2}", filename, i, ext));
          }
        }

        System.IO.File.Move(track.FullFileName, newFileName);
        Log.Debug("Save: Renaming track: {0} Newname: {1}", track.FullFileName, newFileName);
        return true;
      }
      return false;
    }

    #endregion
  }
}