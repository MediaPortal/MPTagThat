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
  [Guid("91956D21-9276-11D1-921A-006097DF5BD4")]
  public interface ICurrentWorkingDirectory
  {
    /// <summary>
    ///   Retrieves the current working directory.
    /// </summary>
    [PreserveSig]
    Int32 GetDirectory(
      [MarshalAs(UnmanagedType.LPWStr)] String pwzPath,
      // Address of a buffer. On return, it will hold a null-terminated Unicode string with 
      // the current working directory's fully qualified path. 
      UInt32 cchSize);

    // Size of the buffer in Unicode characters, including the terminating NULL character. 

    /// <summary>
    ///   Sets the current working directory.
    /// </summary>
    [PreserveSig]
    Int32 SetDirectory(
      [MarshalAs(UnmanagedType.LPWStr)] String pwzPath);

    // Address of a null-terminated Unicode string with the fully qualified path of the 
    // new working directory. 
  }
}