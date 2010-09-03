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
  [Guid("470141A0-5186-11D2-BBB6-0060977B464C")]
  public interface IACList2
  {
    /// <summary>
    ///   Sets the current autocomplete options.
    /// </summary>
    [PreserveSig]
    Int32 SetOptions(
      UInt32 dwFlag);

    // New option flags. Use these flags to ask the client to include the names 
    // of the files and subfolders of the specified folders the next time the 
    // client's IEnumString interface is called.

    /// <summary>
    ///   Retrieves the current autocomplete options.
    /// </summary>
    [PreserveSig]
    Int32 GetOptions(
      out UInt32 pdwFlag);

    // Pointer to a value that will hold the current option flag when the 
    // method returns.
  }
}