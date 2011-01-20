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
using System.ComponentModel;
using System.Reflection;

#endregion

namespace MPTagThat.Core
{
  public class PropertyComparer<T> : IComparer<T>
  {
    private readonly ListSortDirection _direction;
    private readonly PropertyDescriptor _property;

    public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
    {
      _property = property;
      _direction = direction;
    }

    #region IComparer<T>

    public int Compare(T xWord, T yWord)
    {
      // Get property values
      object xValue = GetPropertyValue(xWord, _property.Name);
      object yValue = GetPropertyValue(yWord, _property.Name);

      // Determine sort order
      if (_direction == ListSortDirection.Ascending)
      {
        return CompareAscending(xValue, yValue);
      }
      else
      {
        return CompareDescending(xValue, yValue);
      }
    }

    public bool Equals(T xWord, T yWord)
    {
      return xWord.Equals(yWord);
    }

    public int GetHashCode(T obj)
    {
      return obj.GetHashCode();
    }

    #endregion

    // Compare two property values of any type
    private int CompareAscending(object xValue, object yValue)
    {
      int result;

      // If values implement IComparer
      if (xValue is IComparable)
      {
        result = ((IComparable)xValue).CompareTo(yValue);
      }
        // If values don't implement IComparer but are equivalent
      else if (xValue.Equals(yValue))
      {
        result = 0;
      }
        // Values don't implement IComparer and are not equivalent, so compare as string values
      else result = xValue.ToString().CompareTo(yValue.ToString());

      // Return result
      return result;
    }

    private int CompareDescending(object xValue, object yValue)
    {
      // Return result adjusted for ascending or descending sort order ie
      // multiplied by 1 for ascending or -1 for descending
      return CompareAscending(xValue, yValue) * -1;
    }

    private object GetPropertyValue(T value, string property)
    {
      // Get property
      PropertyInfo propertyInfo = value.GetType().GetProperty(property);

      // Return value
      return propertyInfo.GetValue(value, null);
    }
  }
}