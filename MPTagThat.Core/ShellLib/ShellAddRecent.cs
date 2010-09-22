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

#endregion

namespace MPTagThat.Core.ShellLib
{
  public class ShellAddRecent
  {
    #region ShellAddRecentDocs enum

    public enum ShellAddRecentDocs
    {
      SHARD_PIDL = 0x00000001, // The pv parameter points to a null-terminated string with the path 
      // and file name of the object.
      SHARD_PATHA = 0x00000002, // The pv parameter points to a pointer to an item identifier list 
      // (PIDL) that identifies the document's file object. PIDLs that 
      // identify nonfile objects are not allowed.
      SHARD_PATHW = 0x00000003 // same as SHARD_PATHA but unicode string
    }

    #endregion

    public static void AddToList(String path)
    {
      ShellApi.SHAddToRecentDocs((uint)ShellAddRecentDocs.SHARD_PATHW, path);
    }

    public static void ClearList()
    {
      ShellApi.SHAddToRecentDocs((uint)ShellAddRecentDocs.SHARD_PIDL, IntPtr.Zero);
    }
  }
}