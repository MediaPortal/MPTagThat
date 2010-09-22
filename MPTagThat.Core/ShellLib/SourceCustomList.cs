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

using System;
using System.Runtime.InteropServices;

#endregion

namespace MPTagThat.Core.ShellLib
{
  [ComVisible(true)]
  [Guid("C5D051E1-B004-4163-A9FE-13526AE0537E")]
  public class SourceCustomList : UCOMIEnumString
  {
    public string[] StringList;
    private Int32 currentPosition;

    #region UCOMIEnumString Members

    /// <summary>
    ///   This method retrieves the next celt items in the enumeration sequence. If there are fewer than the 
    ///   requested number of elements left in the sequence, it retrieves the remaining elements. The number 
    ///   of elements actually retrieved is returned through pceltFetched, unless the caller passed in NULL 
    ///   for that parameter.
    /// </summary>
    public Int32 Next(
      Int32 celt, // Number of elements being requested.
      String[] rgelt, // Array of size celt (or larger) of the elements of interest. The 
      // type of this parameter depends on the item being enumerated. 
      out Int32 pceltFetched)
      // Pointer to the number of elements actually supplied in rgelt. The Caller can pass in NULL if celt is 1. 
    {
      pceltFetched = 0;
      while ((currentPosition <= StringList.Length - 1) && (pceltFetched < celt))
      {
        rgelt[pceltFetched] = StringList[currentPosition];
        pceltFetched++;
        currentPosition++;
      }

      if (pceltFetched == celt)
        return 0; // S_OK;
      else
        return 1; // S_FALSE;
    }

    /// <summary>
    ///   This method skips the next specified number of elements in the enumeration sequence.
    /// </summary>
    public Int32 Skip(
      Int32 celt) // Number of elements to be skipped. 
    {
      currentPosition += celt;
      if (currentPosition <= StringList.Length - 1)
        return 0;
      else
        return 1;
    }

    /// <summary>
    ///   This method resets the enumeration sequence to the beginning.
    /// </summary>
    public Int32 Reset()
    {
      currentPosition = 0;
      return 0;
    }

    /// <summary>
    ///   This method creates another enumerator that contains the same enumeration state as the current one. 
    ///   Using this function, a client can record a particular point in the enumeration sequence and return to 
    ///   that point at a later time. The new enumerator supports the same interface as the original one.
    /// </summary>
    public void Clone(
      out UCOMIEnumString ppenum) // Address of the IEnumString pointer variable that receives the interface 
      // pointer to the enumeration object. If the method is unsuccessful, the 
      // value of this output variable is undefined. 
    {
      SourceCustomList clone = new SourceCustomList();
      clone.currentPosition = currentPosition;
      clone.StringList = (String[])StringList.Clone();
      ppenum = clone;
    }

    #endregion
  }
}