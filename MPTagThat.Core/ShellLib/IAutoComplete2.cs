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
  [Guid("EAC04BC0-3791-11D2-BB95-0060977B464C")]
  public interface IAutoComplete2 /*: IAutoComplete */
  {
    /// <summary>
    ///   Initializes the autocomplete object.
    /// </summary>
    [PreserveSig]
    Int32 Init(
      IntPtr hwndEdit, // Handle to the window for the system edit control that is to 
      // have autocompletion enabled. 
      [MarshalAs(UnmanagedType.IUnknown)] Object punkACL,
      // Pointer to the IUnknown interface of the string list object that 
      // is responsible for generating candidates for the completed 
      // string. The object must expose an IEnumString interface. 
      //IntPtr pwszRegKeyPath,
      [MarshalAs(UnmanagedType.LPWStr)] String pwszRegKeyPath,
      // Pointer to an optional null-terminated Unicode string that gives
      // the registry path, including the value name, where the format 
      // string is stored as a REG_SZ value. The autocomplete object 
      // first looks for the path under HKEY_CURRENT_USER . If it fails,
      // it then tries HKEY_LOCAL_MACHINE . For a discussion of the 
      // format string, see the definition of pwszQuickComplete. 
      //IntPtr pwszQuickComplete);
      [MarshalAs(UnmanagedType.LPWStr)] String pwszQuickComplete);

    // Pointer to an optional string that specifies the format to be
    // used if the user enters some text and presses CTRL+ENTER. Set
    // this parameter to NULL to disable quick completion. Otherwise, 
    // the autocomplete object treats pwszQuickComplete as a sprintf 
    // format string, and the text in the edit box as its associated 
    // argument, to produce a new string. For example, set 
    // pwszQuickComplete to "http://www. %s.com/". When a user enters
    // "MyURL" into the edit box and presses CTRL+ENTER, the text in 
    // the edit box is updated to "http://www.MyURL.com/". 

    /// <summary>
    ///   Enables or disables autocompletion.
    /// </summary>
    [PreserveSig]
    Int32 Enable(
      Int32 fEnable);

    // Value that is set to TRUE to enable autocompletion, or to 
    // FALSE to disable it. 


    /// <summary>
    ///   Sets the current autocomplete options.
    /// </summary>
    [PreserveSig]
    Int32 SetOptions(
      UInt32 dwFlag);

    // Flags that allow an application to specify autocomplete options. 

    /// <summary>
    ///   Retrieves the current autocomplete options.
    /// </summary>
    [PreserveSig]
    Int32 GetOptions(
      out UInt32 pdwFlag);

    // that indicate the options that are currently set. 
  }
}