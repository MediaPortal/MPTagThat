#region Using directives
using System;
using System.Text;
using System.Runtime.InteropServices;
#endregion

namespace Stepi.UI
{
    /// <summary>
    /// A wrapper class for the WIN32 API used 
    /// </summary>
    class Win32Wrapper
    {
        /// <summary>
        /// Flags used in SetWindowsPos
        /// </summary>
        public enum FlagsSetWindowPos { SWP_NOSIZE = 0x0001, SWP_NOMOVE = 0x0002, SWP_NOZORDER = 0x0004, SWP_NOREDRAW = 0x0008, SWP_NOACTIVATE = 0x0010, SWP_FRAMECHANGED = 0x0020, SWP_SHOWWINDOW = 0x0040, SWP_HIDEWINDOW = 0x0080, SWP_NOCOPYBITS = 0x0100, SWP_NOOWNERZORDER = 0x0200, SWP_NOSENDCHANGING = 0x0400 };

        /// <summary>
        /// Msdn:"The SetWindowPos function changes the size, position, and Z order of a child, pop-up, or top-level window. Child, pop-up, and top-level windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order."
        /// </summary>
        /// <param name="hWnd">window handle</param>
        /// <param name="hWndInsertAfter">handle to the window it should insert Z order</param>
        /// <param name="X">position on X axis of the top left corner</param>
        /// <param name="Y">position on the Y axis of the top left corner</param>
        /// <param name="cx">window width</param>
        /// <param name="cy">window height</param>
        /// <param name="uFlags">flags</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, FlagsSetWindowPos uFlags);
    }
}

