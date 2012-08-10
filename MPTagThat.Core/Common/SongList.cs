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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace MPTagThat.Core
{
  /// <summary>
  /// This class is used to store a list of Songs, respresented as <see cref="MPTagThat.Core.TrackData" />.
  /// Based on the amount of songs reatrieved, it either stores them in a <see cref="SortableBindingList{T}"/> or uses
  /// a temporary db4o database created on the fly to prevent Out of Memory issues, when processing a large collection of songs.
  /// </summary>
  public class SongList : IEnumerable<TrackData>
  {
    private SortableBindingList<TrackData> _bindingList = new SortableBindingList<TrackData>();

    #region Properties

    /// <summary>
    /// Return the count of songs in the list
    /// </summary>
    /// <returns></returns>
    public int Count
    {
      get { return _bindingList.Count; }
    }

    #endregion

    /// <summary>
    /// Implementation of indexer
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public TrackData this[int i]
    {
      get
      {
        return _bindingList[i];
      }
      set
      {
        _bindingList[i] = value;
      }
    }

    /// <summary>
    /// Adding of new songs to the list
    /// </summary>
    /// <param name="track"></param>
    public void Add(TrackData track)
    {
      _bindingList.Add(track);
    }

    /// <summary>
    /// Removes the object at the specified index
    /// </summary>
    /// <param name="index"></param>
    public void RemoveAt(int index)
    {
      _bindingList.RemoveAt(index);
    }

    /// <summary>
    /// Clear the list
    /// </summary>
    public void Clear()
    {
      _bindingList.Clear();
    }

    /// <summary>
    /// Apply Sorting
    /// </summary>
    /// <param name="property"></param>
    /// <param name="direction"></param>
    public void Sort(PropertyDescriptor property, ListSortDirection direction)
    {
      _bindingList.ApplySortCore(property, direction);
    }

    /// <summary>
    /// Provide enumeration
    /// </summary>
    /// <returns></returns>
    public IEnumerator<TrackData> GetEnumerator()
    {
      return _bindingList.GetEnumerator();
    }

    /// <summary>
    /// Provide enumeration
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
