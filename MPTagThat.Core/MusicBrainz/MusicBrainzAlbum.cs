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
using MPTagThat.Core.AlbumInfo;

#endregion

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzAlbum
  {
    #region Private Fields

    #endregion

    #region ctor

    public MusicBrainzAlbum()
    {
      Tracks = new List<MusicBrainzTrack>();
    }

    #endregion

    #region Properties

    public string Id { get; set; }

    public string Asin { get; set; }

    public string Title { get; set; }

    public string Artist { get; set; }

    public string Year { get; set; }

    public int DiscCount { get; set; }

    public List<MusicBrainzTrack> Tracks { get; set; }

    public Album Amazon { get; set; }

    #endregion
  }
}