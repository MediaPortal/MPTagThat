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
using System.Collections.Generic;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.GridView;

namespace MPTagThat.Commands
{
  [SupportedCommandType("GetLyrics")]
  public class CmdGetLyrics : Command
  {
    public object[] Parameters { get; private set; }

    #region Variables

    List<TrackData> tracks = new List<TrackData>();

    #endregion

    #region ctor

    public CmdGetLyrics(object[] parameters)
    {
      Parameters = parameters;
      NeedsPreprocessing = true;
    }

    #endregion


    #region Command Implementation

    public override bool Execute(ref TrackData track, int rowIndex)
    {
      return false;
    }

    /// <summary>
    /// Do Preprocessing of the Tracks
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public override bool PreProcess(TrackData track)
    {
      if (track.Lyrics == null || Options.MainSettings.OverwriteExistingLyrics)
      {
        tracks.Add(track);
      }
      return true;
    }

    /// <summary>
    /// Post Process after command execution
    /// </summary>
    /// <returns></returns>
    public override bool PostProcess()
    {
      bool itemsChanged = false;

      if (tracks.Count > 0)
      {
        try
        {
          LyricsSearch lyricssearch = new LyricsSearch(tracks);
          lyricssearch.Owner = TracksGrid.MainForm;
          lyricssearch.StartPosition = FormStartPosition.CenterParent;
          if (lyricssearch.ShowDialog() == DialogResult.OK)
          {
            DataGridView lyricsResult = lyricssearch.GridView;
            foreach (DataGridViewRow lyricsRow in lyricsResult.Rows)
            {
              if (lyricsRow.Cells[0].Value == DBNull.Value || lyricsRow.Cells[0].Value == null)
                continue;

              if ((bool)lyricsRow.Cells[0].Value != true)
                continue;

              foreach (DataGridViewRow row in TracksGrid.View.Rows)
              {
                TrackData lyricsTrack = tracks[lyricsRow.Index];
                TrackData track = Options.Songlist[row.Index];
                if (lyricsTrack.FullFileName == track.FullFileName)
                {
                  track.Lyrics = (string)lyricsRow.Cells[5].Value;
                  TracksGrid.SetBackgroundColorChanged(row.Index);
                  track.Changed = true;
                  Options.Songlist[row.Index] = track;
                  itemsChanged = true;
                  break;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Error in Lyricssearch: {0}", ex.Message);
        }
      }
      return itemsChanged;
    }

    #endregion
  }
}