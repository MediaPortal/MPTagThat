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

#region

using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace MPTagThat.Core
{
  public class SortableBindingList<T> : BindingList<T>
  {
    private bool _isSorted;
    private ListSortDirection _sortDirection;
    private PropertyDescriptor _sortProperty;

    protected override bool SupportsSortingCore
    {
      get { return true; }
    }

    protected override ListSortDirection SortDirectionCore
    {
      get { return _sortDirection; }
    }

    protected override PropertyDescriptor SortPropertyCore
    {
      get { return _sortProperty; }
    }

    protected override bool IsSortedCore
    {
      get { return _isSorted; }
    }

    protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
    {
      _sortProperty = property;
      _sortDirection = direction;

      // Get list to sort
      List<T> items = Items as List<T>;

      // Apply and set the sort, if items to sort
      if (items != null)
      {
        PropertyComparer<T> pc = new PropertyComparer<T>(property, direction);
        items.Sort(pc);
        _isSorted = true;
      }
      else
      {
        _isSorted = false;
      }

      // Let bound controls know they should refresh their views
      OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    protected override void RemoveSortCore()
    {
      _isSorted = false;
    }
  }
}