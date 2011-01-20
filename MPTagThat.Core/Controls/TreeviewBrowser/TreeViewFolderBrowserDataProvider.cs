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
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using Raccoom.Win32;
using ROOT.CIMV2.Win32;

#endregion

namespace Raccoom.Windows.Forms
{
  /// <summary>
  ///   <c>TreeViewFolderBrowserDataProvider</c> is the standard data provider for <see cref = "TreeViewFolderBrowser" /> which is based on <see cref = "ROOT.CIMV2.Win32.Logicaldisk" />, System.IO and <see cref = "Raccoom.Win32.SystemImageList" />
  ///   <seealso cref = "ITreeViewFolderBrowserDataProvider" />
  /// </summary>
  [ToolboxBitmap(typeof (SqlDataAdapter))]
  public class TreeViewFolderBrowserDataProvider : ITreeViewFolderBrowserDataProvider
  {
    #region fields

    /// <summary>
    ///   last CheckboxMode used to fill the tree view, saved to know about changes
    /// </summary>
    private CheckboxBehaviorMode _checkboxMode;

    /// <summary>
    ///   Shell32 ImageList
    /// </summary>
    private SystemImageList _systemImageList;

    #endregion

    #region ITreeViewFolderBrowserDataProvider Members

    [Browsable(false)]
    public virtual ImageList ImageList
    {
      get { return null; }
    }

    public virtual void QueryContextMenuItems(TreeViewFolderBrowserHelper helper, TreeNodePath node) {}

    public virtual TreeNodeCollection RequestDriveCollection(TreeViewFolderBrowserHelper helper)
    {
      switch (helper.TreeView.RootFolder)
      {
        case Environment.SpecialFolder.Desktop:
          return helper.TreeView.Nodes[0].Nodes[1].Nodes;
        default:
          return helper.TreeView.Nodes;
      }
    }

    public virtual void RequestSubDirs(TreeViewFolderBrowserHelper helper, TreeNodePath parent,
                                       TreeViewCancelEventArgs e)
    {
      if (parent.Path == null) return;
      //
      DirectoryInfo directory = new DirectoryInfo(parent.Path);
      // check persmission
      new FileIOPermission(FileIOPermissionAccess.PathDiscovery, directory.FullName).Demand();
      //	

      // Sort the Directories, as Samba might return unsorted
      DirectoryInfo[] dirInfo = directory.GetDirectories();
      Array.Sort(dirInfo,
                 new Comparison<DirectoryInfo>(
                   delegate(DirectoryInfo d1, DirectoryInfo d2) { return string.Compare(d1.Name, d2.Name); }));


      foreach (DirectoryInfo dir in dirInfo)
      {
        if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
        {
          continue;
        }
        TreeNodePath newNode = CreateTreeNode(helper, dir.Name, dir.FullName, false,
                                              ((helper.TreeView.CheckboxBehaviorMode != CheckboxBehaviorMode.None) &&
                                               (parent.Checked)), false);
        parent.Nodes.Add(newNode);
        //
        try
        {
          if (dir.GetDirectories().GetLength(0) > 0)
          {
            newNode.AddDummyNode();
          }
        }
        catch {}
      }
    }

    public virtual void RequestRoot(TreeViewFolderBrowserHelper helper)
    {
      AttachSystemImageList(helper);
      //
      bool populateDrives = true;
      //
      TreeNodeCollection rootNodeCollection;
      TreeNodeCollection driveRootNodeCollection;
      // setup up root node collection
      switch (helper.TreeView.RootFolder)
      {
        case Environment.SpecialFolder.Desktop:
          // create root node <Desktop>
          TreeNodePath desktopNode = CreateTreeNode(helper, Environment.SpecialFolder.Desktop.ToString(), string.Empty,
                                                    false, false, true);
          helper.TreeView.Nodes.Add(desktopNode);
          rootNodeCollection = helper.TreeView.Nodes[0].Nodes;
          // create child node <Personal>
          string personalDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          rootNodeCollection.Add(CreateTreeNode(helper, Path.GetFileName(personalDirectory), personalDirectory, true,
                                                false, false));
          // create child node <MyComuter>
          TreeNodePath myComputerNode = CreateTreeNode(helper, Environment.SpecialFolder.MyComputer.ToString(),
                                                       string.Empty, false, false, true);
          rootNodeCollection.Add(myComputerNode);
          driveRootNodeCollection = myComputerNode.Nodes;
          break;
        case Environment.SpecialFolder.MyComputer:
          rootNodeCollection = helper.TreeView.Nodes;
          driveRootNodeCollection = rootNodeCollection;
          break;
        default:
          rootNodeCollection = helper.TreeView.Nodes;
          driveRootNodeCollection = rootNodeCollection;
          // create root node with specified SpecialFolder
          rootNodeCollection.Add(CreateTreeNode(helper,
                                                Path.GetFileName(Environment.GetFolderPath(helper.TreeView.RootFolder)),
                                                Environment.GetFolderPath(helper.TreeView.RootFolder), true, false,
                                                false));
          populateDrives = false;
          break;
      }
      if (populateDrives)
      {
        // populate local machine drives
        foreach (Logicaldisk logicalDisk in Logicaldisk.GetInstances(null, GetWMIQueryStatement(helper.TreeView)))
        {
          try
          {
            string name = string.Empty;
            string path = logicalDisk.Name + "\\";
            name = logicalDisk.Description;
            //
            name += (name != string.Empty) ? " (" + path + ")" : path;
            // add node to root collection
            driveRootNodeCollection.Add(CreateTreeNode(helper, name, path, true, false, false));
          }
          catch (Exception doh)
          {
            throw doh;
          }
        }
      }
    }

    /// <summary>
    ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
      if (_systemImageList != null) _systemImageList.Dispose();
    }

    #endregion

    #region internal interface

    protected virtual void AttachSystemImageList(TreeViewFolderBrowserHelper helper)
    {
      if (_checkboxMode != helper.TreeView.CheckboxBehaviorMode)
      {
        // checkboxes recreate the control internal
        if (_systemImageList != null)
        {
          SystemImageListHelper.SetTreeViewImageList(helper.TreeView, _systemImageList, false);
        }
      }
      _checkboxMode = helper.TreeView.CheckboxBehaviorMode;
    }

    /// <summary>
    ///   Creates a new node and assigns a icon
    /// </summary>
    /// <param name = "helper"></param>
    /// <param name = "text"></param>
    /// <param name = "path"></param>
    /// <param name = "addDummyNode"></param>
    /// <param name = "forceChecked"></param>
    /// <returns></returns>
    protected virtual TreeNodePath CreateTreeNode(TreeViewFolderBrowserHelper helper, string text, string path,
                                                  bool addDummyNode, bool forceChecked, bool isSpecialFolder)
    {
      TreeNodePath node = helper.CreateTreeNode(text, path, addDummyNode, forceChecked, isSpecialFolder);
      try
      {
        SetIcon(helper.TreeView, node);
      }
      catch
      {
        node.ImageIndex = -1;
        node.SelectedImageIndex = -1;
      }
      return node;
    }

    /// <summary>
    ///   Extract the icon for the file type (Extension)
    /// </summary>
    protected virtual void SetIcon(TreeViewFolderBrowser treeView, TreeNodePath node)
    {
      // create on demand
      if (_systemImageList == null)
      {
        // Shell32 ImageList
        _systemImageList = new SystemImageList(SystemImageListSize.SmallIcons);
        SystemImageListHelper.SetTreeViewImageList(treeView, _systemImageList, false);
      }
      node.ImageIndex = _systemImageList.IconIndex(node.Path, true);
      node.SelectedImageIndex = node.ImageIndex;
    }

    /// <summary>
    ///   Gets the WMI query string based on the current drive types.
    /// </summary>
    /// <returns></returns>
    protected virtual string GetWMIQueryStatement(TreeViewFolderBrowser treeView)
    {
      if ((treeView.DriveTypes & DriveTypes.All) == DriveTypes.All) return string.Empty;
      //
      string where = string.Empty;
      //
      Array array = Enum.GetValues(typeof (DriveTypes));
      //
      foreach (DriveTypes type in array)
      {
        if ((treeView.DriveTypes & type) == type)
        {
          if (where == string.Empty)
          {
            where += "drivetype = " +
                     Enum.Format(typeof (Win32_LogicalDiskDriveTypes),
                                 Enum.Parse(typeof (Win32_LogicalDiskDriveTypes), type.ToString(), true), "d");
          }
          else
          {
            where += " OR drivetype = " +
                     Enum.Format(typeof (Win32_LogicalDiskDriveTypes),
                                 Enum.Parse(typeof (Win32_LogicalDiskDriveTypes), type.ToString(), true), "d");
          }
        }
      }
      //
      return where;
    }

    #endregion

    public override string ToString()
    {
      return "Standard Provider";
    }
  }
}