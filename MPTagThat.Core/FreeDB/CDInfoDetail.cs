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
namespace MPTagThat.Core.Freedb
{
  /// <summary>
  ///   Contains Details about the CD
  /// </summary>
  public class CDInfoDetail
  {
    private CDTrackDetail[] m_tracks;

    public CDInfoDetail() {}

    public CDInfoDetail(string discID, string artist, string title,
                        string genre, int year, int duration, CDTrackDetail[] tracks,
                        string extd, int[] playorder)
    {
      DiscID = discID;
      Artist = artist;
      Title = title;
      Genre = genre;
      Year = year;
      Duration = duration;
      m_tracks = tracks;
      EXTD = extd;
      PlayOrder = playorder;
    }

    public string DiscID { get; set; }

    public string Artist { get; set; }

    public string Title { get; set; }

    public string Genre { get; set; }

    public int Year { get; set; }

    public int Duration { get; set; }

    public CDTrackDetail[] Tracks
    {
      get { return m_tracks; }
      set { m_tracks = value; }
    }

    public string EXTD { get; set; }

    public int[] PlayOrder { get; set; }

    public CDTrackDetail getTrack(int index)
    {
      return m_tracks[index - 1];
    }
  }
}