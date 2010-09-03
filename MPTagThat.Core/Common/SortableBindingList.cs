using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

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

    protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
    {
      _sortProperty = property;
      _sortDirection = direction;

      // Get list to sort
      List<T> items = this.Items as List<T>;

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
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    protected override bool IsSortedCore
    {
      get { return _isSorted; }
    }

    protected override void RemoveSortCore()
    {
      _isSorted = false;
    }
  }
}
