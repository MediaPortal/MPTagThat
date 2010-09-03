#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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

namespace MPTagThat.Core.Freedb
{
  /// <summary>
  ///   Contains Information about Tracks
  /// </summary>
  public class CDTrackDetail
  {
    private int m_duration;
    private string m_durationString;

    public CDTrackDetail() {}

    public CDTrackDetail(string songTitle, string artist, string extt,
                         int trackNumber, int offset, int duration)
    {
      Title = songTitle;
      Artist = artist;
      EXTT = extt;
      Track = trackNumber;
      Offset = offset;
      m_duration = duration;
      m_durationString = string.Format("{0}:{1}", (m_duration / 60).ToString().PadLeft(2, '0'),
                                       (m_duration % 60).ToString().PadLeft(2, '0'));
    }

    public string Title { get; set; }

    // can be null if the artist is the same as the main
    // album
    public string Artist { get; set; }


    public int Track { get; set; }

    public int DurationInt
    {
      get { return m_duration; }
      set { m_duration = value; }
    }

    public string Duration
    {
      get
      {
        return string.Format("{0}:{1}", (m_duration / 60).ToString().PadLeft(2, '0'),
                             (m_duration % 60).ToString().PadLeft(2, '0'));
      }
    }

    public int Offset { get; set; }

    public string EXTT { get; set; }
  }
}