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
  public class ShellFunctions
  {
    public static IMalloc GetMalloc()
    {
      IntPtr ptrRet;
      ShellApi.SHGetMalloc(out ptrRet);

      Object obj = Marshal.GetTypedObjectForIUnknown(ptrRet, GetMallocType());
      IMalloc imalloc = (IMalloc)obj;

      return imalloc;
    }

    public static IShellFolder GetDesktopFolder()
    {
      IntPtr ptrRet;
      ShellApi.SHGetDesktopFolder(out ptrRet);

      Type shellFolderType = Type.GetType("ShellLib.IShellFolder");
      Object obj = Marshal.GetTypedObjectForIUnknown(ptrRet, shellFolderType);
      IShellFolder ishellFolder = (IShellFolder)obj;

      return ishellFolder;
    }

    public static Type GetShellFolderType()
    {
      Type shellFolderType = Type.GetType("ShellLib.IShellFolder");
      return shellFolderType;
    }

    public static Type GetMallocType()
    {
      Type mallocType = Type.GetType("ShellLib.IMalloc");
      return mallocType;
    }

    public static Type GetFolderFilterType()
    {
      Type folderFilterType = Type.GetType("ShellLib.IFolderFilter");
      return folderFilterType;
    }

    public static Type GetFolderFilterSiteType()
    {
      Type folderFilterSiteType = Type.GetType("ShellLib.IFolderFilterSite");
      return folderFilterSiteType;
    }

    public static IShellFolder GetShellFolder(IntPtr ptrShellFolder)
    {
      Type shellFolderType = GetShellFolderType();
      Object obj = Marshal.GetTypedObjectForIUnknown(ptrShellFolder, shellFolderType);
      IShellFolder RetVal = (IShellFolder)obj;
      return RetVal;
    }
  }
}