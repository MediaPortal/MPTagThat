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
  public class Lyrics : ICommand, IDisposable
  {
    #region Variables

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private bool _progressCancelled = false;
    private GridViewTracks _tracksGrid;

    List<TrackData> tracks = new List<TrackData>();

    #endregion

    #region ICommand Implementation

    public bool Execute(ref TrackData track, GridViewTracks tracksGrid)
    {
      return false;
    }

    /// <summary>
    /// Indicate, whether we need Preprocess the tracks
    /// </summary>
    /// <returns></returns>
    public bool NeedsPreprocessing()
    {
      return true;
    }

    /// <summary>
    /// Do Preprocessing of the Tracks
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public bool PreProcess(TrackData track)
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
    /// <param name="tracksGrid"></param>
    /// <returns></returns>
    public bool PostProcess(GridViewTracks tracksGrid)
    {
      _tracksGrid = tracksGrid;
      bool itemsChanged = false;

      if (tracks.Count > 0)
      {
        try
        {
          LyricsSearch lyricssearch = new LyricsSearch(tracks);
          lyricssearch.Owner = _tracksGrid.MainForm;
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

              foreach (DataGridViewRow row in _tracksGrid.View.Rows)
              {
                TrackData lyricsTrack = tracks[lyricsRow.Index];
                TrackData track = Options.Songlist[row.Index];
                if (lyricsTrack.FullFileName == track.FullFileName)
                {
                  track.Lyrics = (string)lyricsRow.Cells[5].Value;
                  _tracksGrid.SetBackgroundColorChanged(row.Index);
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
          log.Error("Error in Lyricssearch: {0}", ex.Message);
        }
      }
      return itemsChanged;
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