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
  public class SortableBindingList<T> : BindingList<T>, IBindingListView
  {
    private bool _isSorted;
    private ListSortDirection _sortDirection;
    private PropertyDescriptor _sortProperty;
    private ListSortDescriptionCollection _sortDescriptions = new ListSortDescriptionCollection();
    private List<PropertyComparer<T>> _comparers;

    #region IBindingListView Members
    public string Filter
    {
      get
      {
        throw new System.NotImplementedException();
      }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    public void RemoveFilter()
    {
      throw new System.NotImplementedException();
    }

    public ListSortDescriptionCollection SortDescriptions
    {
      get { return _sortDescriptions; }
    }

    public bool SupportsAdvancedSorting
    {
      get { return true; }
    }

    public bool SupportsFiltering
    {
      get { return false; }
    }

    public void ApplySort(ListSortDescriptionCollection sorts)
    {
      // Get list to sort
      // Note: this.Items is a non-sortable ICollection<T>
      List<T> items = this.Items as List<T>;

      // Apply and set the sort, if items to sort
      if (items != null)
      {
        _sortDescriptions = sorts;
        _comparers = new List<PropertyComparer<T>>();
        foreach (ListSortDescription sort in sorts)
          _comparers.Add(new PropertyComparer<T>(sort.PropertyDescriptor, sort.SortDirection));
        items.Sort(CompareValuesByProperties);
      }
    }

    #endregion

    #region Private Methods

    private int CompareValuesByProperties(T x, T y)
    {
      if (x == null)
      {
        return (y == null) ? 0 : -1;
      }

      if (y == null)
      {
        return 1;
      }
        
      foreach (PropertyComparer<T> comparer in _comparers)
      {
        int retval = comparer.Compare(x, y);
        if (retval != 0)
        return retval;
      }
      return 0;
    }

    #endregion

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
        ListSortDescription sort = new ListSortDescription(property, direction);
        ListSortDescription[] sortArr = new ListSortDescription[_sortDescriptions.Count + 1];
        _sortDescriptions.CopyTo(sortArr, 1);
        sortArr[0] = sort;
        _sortDescriptions = new ListSortDescriptionCollection(sortArr);
        ApplySort(_sortDescriptions);
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