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

using Shell32;

#endregion

namespace Raccoom.Win32
{
  /// <summary>
  ///   Summary description for Shell32Namespaces.
  /// </summary>
  public class Shell32Namespaces
  {
    #region fields

    private Shell _shell;

    #endregion

    #region public interface

    public Folder GetDesktop()
    {
      return Shell.NameSpace(ShellSpecialFolderConstants.ssfDESKTOP);
    }

    public FolderItems GetEntries(ShellSpecialFolderConstants shellFolder)
    {
      Folder shell32Folder = Shell.NameSpace(shellFolder);
      return shell32Folder.Items();
    }

    #endregion

    #region internal interface

    internal Shell Shell
    {
      get
      {
        // create on demand
        if (_shell == null) _shell = new ShellClass();
        return _shell;
      }
    }

    #endregion
  }
}