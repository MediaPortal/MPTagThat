#region Copyright (C) 2009-2016 Team MediaPortal
// Copyright (C) 2009-2016 Team MediaPortal
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

using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

#endregion

namespace MPTagThat.Core.Services.MusicDatabase.Indexes
{
  /// <summary>
  /// Map Reduce Index to retrieve distinct Artist
  /// </summary>
  public class DistinctArtistIndex : AbstractIndexCreationTask<TrackData, DistinctResult>
  {
    public DistinctArtistIndex()
    {
      Map = tracks => from track in tracks
                            from artists in track.Artist.Split(';').ToList()
                            select new { Name = artists };


      Reduce = results => from result in results
                          group result by result.Name into g
                          select new { Name = g.Key };

      Store(song => song.Name, FieldStorage.Yes);
      Sort(song => song.Name, SortOptions.String);
    }
  }
}
