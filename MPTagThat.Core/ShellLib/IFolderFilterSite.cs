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
  [Guid("C0A651F5-B48B-11d2-B5ED-006097C686F6")]
  public interface IFolderFilterSite
  {
    // Exposed by a host to allow clients to pass the host their IUnknown interface pointers.
    [PreserveSig]
    Int32 SetFilter(
      [MarshalAs(UnmanagedType.Interface)] Object punk);

    // A pointer to the client's IUnknown interface. To notify the host to terminate 
    // filtering and stop calling your IFolderFilter interface, set this parameter to NULL. 
  }
}