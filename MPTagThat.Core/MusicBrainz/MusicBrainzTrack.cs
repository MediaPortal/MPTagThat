﻿#region Copyright (C) 2009-2011 Team MediaPortal
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

#endregion

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzTrack
  {
    #region Constructors

    #endregion

    #region Private Fields

    private List<MusicBrainzRelease> _releases = new List<MusicBrainzRelease>(); 

    #endregion

    #region Properties

    public string Id { get; set; }

    public int DiscId { get; set; }

    public int Number { get; set; }

    public int TrackCount { get; set; }

    public string Title { get; set; }

    public string Artist { get; set; }

    public string AlbumId { get; set; }

    public int Duration { get; set; }

    public List<MusicBrainzRelease> Releases
    {
      get { return _releases; }
    }

    #endregion
  }
}