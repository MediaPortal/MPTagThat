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
using System.Runtime.InteropServices;

#endregion

namespace MPTagThat.Core.ShellLib
{
  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("9CC22886-DC8E-11d2-B1D0-00C04F8EEB3E")]
  public interface IFolderFilter
  {
    // Allows a client to specify which individual items should be enumerated.
    // Note: The host calls this method for each item in the folder. Return S_OK, to have the item enumerated. 
    // Return S_FALSE to prevent the item from being enumerated.
    [PreserveSig]
    Int32 ShouldShow(
      [MarshalAs(UnmanagedType.Interface)] Object psf, // A pointer to the folder's IShellFolder interface.
      IntPtr pidlFolder, // The folder's PIDL.
      IntPtr pidlItem);

    // The item's PIDL.

    // Allows a client to specify which classes of objects in a Shell folder should be enumerated.
    [PreserveSig]
    Int32 GetEnumFlags(
      [MarshalAs(UnmanagedType.Interface)] Object psf, // A pointer to the folder's IShellFolder interface.
      IntPtr pidlFolder, // The folder's PIDL.
      IntPtr phwnd, // A pointer to the host's window handle.
      out UInt32 pgrfFlags);

    // One or more SHCONTF values that specify which classes of objects to enumerate.
  } ;
}