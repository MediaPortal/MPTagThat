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
  public class ShellOpenFileDialog
  {
    #region Variables

    private OpenFileName _ofn;

    private delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private const int OFN_ALLOWMULTISELECT = 0x00000200;
    private const int OFN_CREATEPROMPT = 0x00002000;
    private const int OFN_DONTADDTORECENT = 0x02000000;
    private const int OFN_ENABLEHOOK = 0x00000020;
    private const int OFN_ENABLEINCLUDENOTIFY = 0x00400000;
    private const int OFN_ENABLESIZING = 0x00800000;
    private const int OFN_ENABLETEMPLATE = 0x00000040;
    private const int OFN_ENABLETEMPLATEHANDLE = 0x00000080;
    private const int OFN_EXPLORER = 0x00080000;
    private const int OFN_EXTENSIONDIFFERENT = 0x00000400;
    private const int OFN_FILEMUSTEXIST = 0x00001000;
    private const int OFN_FORCESHOWHIDDEN = 0x10000000;
    private const int OFN_HIDEREADONLY = 0x00000004;
    private const int OFN_LONGNAMES = 0x00200000;
    private const int OFN_NOCHANGEDIR = 0x00000008;
    private const int OFN_NODEREFERENCELINKS = 0x00100000;
    private const int OFN_NOLONGNAMES = 0x00040000;
    private const int OFN_NONETWORKBUTTON = 0x00020000;
    private const int OFN_NOREADONLYRETURN = 0x00008000;
    private const int OFN_NOTESTFILECREATE = 0x00010000;
    private const int OFN_NOVALIDATE = 0x00000100;
    private const int OFN_OVERWRITEPROMPT = 0x00000002;
    private const int OFN_PATHMUSTEXIST = 0x00000800;
    private const int OFN_READONLY = 0x00000001;
    private const int OFN_SHAREAWARE = 0x00004000;
    private const int OFN_SHOWHELP = 0x00000010;

    #endregion

    #region Imports

    [DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool GetOpenFileName([In, Out] OpenFileName _ofn);

    #endregion

    #region struct

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
      public int structSize = 0;
      public IntPtr dlgOwner = IntPtr.Zero;
      public IntPtr instance = IntPtr.Zero;

      public String filter = null;
      public String customFilter = null;
      public int maxCustFilter = 0;
      public int filterIndex = 0;

      public String file = null;
      public int maxFile = 0;

      public String fileTitle = null;
      public int maxFileTitle = 0;

      public String initialDir = null;

      public String title = null;

      public int flags = 0;
      public short fileOffset = 0;
      public short fileExtension = 0;

      public String defExt = null;

      public IntPtr custData = IntPtr.Zero;
      public IntPtr hook = IntPtr.Zero;

      public String templateName = null;

      public IntPtr reservedPtr = IntPtr.Zero;
      public int reservedInt = 0;
      public int flagsEx = 0;
    }

    #endregion

    #region Properties

    public string FileName
    {
      get { return _ofn.file; }
      set { _ofn.file = value; }
    }

    public string Filter
    {
      get { return _ofn.filter; }
      set { _ofn.filter = value; }
    }

    public string InitialDirectory
    {
      set { _ofn.initialDir = value; }
    }

    #endregion


    #region ctor

    public ShellOpenFileDialog()
    {
      _ofn = new OpenFileName();
      _ofn.structSize = Marshal.SizeOf(_ofn);
      _ofn.file = new String(new char[256]);
      _ofn.maxFile = _ofn.file.Length;

      _ofn.fileTitle = new String(new char[64]);
      _ofn.maxFileTitle = _ofn.fileTitle.Length;  

      _ofn.flags = OFN_ENABLESIZING | OFN_FORCESHOWHIDDEN;
    }

    public bool ShowDialog()
    {
      return GetOpenFileName(_ofn);
    }

    #endregion

  }
}
