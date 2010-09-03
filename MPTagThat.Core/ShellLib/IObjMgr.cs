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
  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("00BB2761-6A77-11D0-A535-00C04FD7D062")]
  public interface IObjMgr
  {
    /// <summary>
    ///   Appends an object to the collection of managed objects.
    /// </summary>
    [PreserveSig]
    Int32 Append(
      [MarshalAs(UnmanagedType.IUnknown)] Object punk);

    // Address of the IUnknown interface of the object to be added to the list. 

    /// <summary>
    ///   Removes an object from the collection of managed objects.
    /// </summary>
    [PreserveSig]
    Int32 Remove(
      [MarshalAs(UnmanagedType.IUnknown)] Object punk);

    // Address of the IUnknown interface of the object to be removed from the list. 
  }
}