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
  [Guid("77A130B0-94FD-11D0-A544-00C04FD7D062")]
  public interface IACList
  {
    /// <summary>
    ///   Requests that the autocompletion client generate candidate strings associated with a specified item in 
    ///   its namespace.
    /// </summary>
    [PreserveSig]
    Int32 Expand(
      [MarshalAs(UnmanagedType.LPWStr)] String pszExpand);

    // Null-terminated string to be expanded by the autocomplete object. 
  }
}