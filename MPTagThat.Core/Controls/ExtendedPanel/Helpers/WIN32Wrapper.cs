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

namespace Stepi.UI
{
  /// <summary>
  ///   A wrapper class for the WIN32 API used
  /// </summary>
  internal class Win32Wrapper
  {
    #region FlagsSetWindowPos enum

    /// <summary>
    ///   Flags used in SetWindowsPos
    /// </summary>
    public enum FlagsSetWindowPos
    {
      SWP_NOSIZE = 0x0001,
      SWP_NOMOVE = 0x0002,
      SWP_NOZORDER = 0x0004,
      SWP_NOREDRAW = 0x0008,
      SWP_NOACTIVATE = 0x0010,
      SWP_FRAMECHANGED = 0x0020,
      SWP_SHOWWINDOW = 0x0040,
      SWP_HIDEWINDOW = 0x0080,
      SWP_NOCOPYBITS = 0x0100,
      SWP_NOOWNERZORDER = 0x0200,
      SWP_NOSENDCHANGING = 0x0400
    } ;

    #endregion

    /// <summary>
    ///   Msdn:"The SetWindowPos function changes the size, position, and Z order of a child, pop-up, or top-level window. Child, pop-up, and top-level windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order."
    /// </summary>
    /// <param name = "hWnd">window handle</param>
    /// <param name = "hWndInsertAfter">handle to the window it should insert Z order</param>
    /// <param name = "X">position on X axis of the top left corner</param>
    /// <param name = "Y">position on the Y axis of the top left corner</param>
    /// <param name = "cx">window width</param>
    /// <param name = "cy">window height</param>
    /// <param name = "uFlags">flags</param>
    /// <returns></returns>
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
                                           FlagsSetWindowPos uFlags);
  }
}