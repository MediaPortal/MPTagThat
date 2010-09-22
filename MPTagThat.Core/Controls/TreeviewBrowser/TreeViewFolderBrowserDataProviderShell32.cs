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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Raccoom.Win32;
using ROOT.CIMV2.Win32;
using Shell32;

#endregion

namespace Raccoom.Windows.Forms
{
  /// <summary>
  ///   <c>TreeViewFolderBrowserDataProvider</c> is the shell32 interop data provider for <see cref = "TreeViewFolderBrowser" /> which is based on <see cref = "ROOT.CIMV2.Win32.Logicaldisk" />, <c>Shell32</c> Interop and <see cref = "Raccoom.Win32.SystemImageList" />
  ///   <seealso cref = "ITreeViewFolderBrowserDataProvider" />
  /// </summary>
  /// <remarks>
  ///   Shell32 does not support the .NET System.Security.Permissions system. There is no code access permission, only FileSystem ACL.
  /// </remarks>
  [DefaultProperty("ShowAllShellObjects"), ToolboxBitmap(typeof (SqlDataAdapter))]
  public class TreeViewFolderBrowserDataProviderShell32 : TreeViewFolderBrowserDataProvider
  {
    #region fields

    /// <summary>
    ///   Shell32 Com Object
    /// </summary>
    private readonly Shell32Namespaces _shell = new Shell32Namespaces();

    private TreeViewFolderBrowserHelper _helper;

    /// <summary>
    ///   drive tree node (mycomputer) root collection
    /// </summary>
    private TreeNodeCollection _rootCollection;

    /// <summary>
    ///   show only filesystem
    /// </summary>
    private bool _showAllShellObjects;

    #endregion

    #region constructors

    #endregion

    #region public interface

    /// <summary>
    ///   Enables or disables the context menu which show's the folder item's shell verbs.
    /// </summary>
    [Browsable(true), Category("Behaviour"), Description("Specifies if the context menu is enabled."),
     DefaultValue(false)]
    public bool EnableContextMenu { get; set; }

    [Browsable(true), Category("Behaviour"), Description("Display file system and virtual shell folders."),
     DefaultValue(false)]
    public bool ShowAllShellObjects
    {
      get { return _showAllShellObjects; }
      set { _showAllShellObjects = value; }
    }

    #endregion

    public override void QueryContextMenuItems(TreeViewFolderBrowserHelper helper, TreeNodePath node)
    {
      if (!EnableContextMenu) return;
      //
      FolderItem fi = node.Tag as FolderItem;
      if (fi == null) return;
      //
      foreach (FolderItemVerb verb in fi.Verbs())
      {
        if (verb.Name.Length == 0) continue;
        //
        MenuItemShellVerb item = new MenuItemShellVerb(verb);
        helper.TreeView.ContextMenu.MenuItems.Add(item);
      }
    }

    public override void RequestRoot(TreeViewFolderBrowserHelper helper)
    {
      _helper = helper;
      AttachSystemImageList(helper);
      //
      // setup up root node collection
      switch (helper.TreeView.RootFolder)
      {
        case Environment.SpecialFolder.Desktop:
          Folder2 desktopFolder = (Folder2)_shell.GetDesktop();
          // create root node <Desktop>
          TreeNodePath desktopNode = CreateTreeNode(helper, desktopFolder.Title, desktopFolder.Self.Path, false, false,
                                                    true);
          helper.TreeView.Nodes.Add(desktopNode);
          desktopNode.Tag = desktopFolder;
          //
          Folder2 myComputer = (Folder2)_shell.Shell.NameSpace(ShellSpecialFolderConstants.ssfDRIVES);
          foreach (FolderItem fi in desktopFolder.Items())
          {
            if (!fi.IsFolder) continue;
            //
            TreeNodePath node = CreateTreeNode(helper, fi.Name, fi.Path, true, false, true);
            node.Tag = fi;
            desktopNode.Nodes.Add(node);
            //
            if (_shell.Shell.NameSpace(ShellSpecialFolderConstants.ssfDRIVES).Title == fi.Name)
              _rootCollection = node.Nodes;
          }
          break;
        case Environment.SpecialFolder.MyComputer:
          FillMyComputer(((Folder2)_shell.Shell.NameSpace(ShellSpecialFolderConstants.ssfDRIVES)).Self,
                         helper.TreeView.Nodes, helper);
          break;
        default:
          // create root node with specified SpecialFolder
          Folder2 root = (Folder3)_shell.Shell.NameSpace(helper.TreeView.RootFolder);
          TreeNodePath rootNode = CreateTreeNode(helper, root.Title, root.Self.Path, true, false, true);
          rootNode.Tag = root.Self;
          helper.TreeView.Nodes.Add(rootNode);
          _rootCollection = rootNode.Nodes;
          break;
      }
    }

    public override void RequestSubDirs(TreeViewFolderBrowserHelper helper, TreeNodePath parent,
                                        TreeViewCancelEventArgs e)
    {
      if (!parent.IsSpecialFolder) return;
      //
      FolderItem2 folderItem = ((FolderItem2)parent.Tag);
      //
      if (_shell.Shell.NameSpace(ShellSpecialFolderConstants.ssfDRIVES).Title == folderItem.Name)
      {
        FillMyComputer(folderItem, parent.Nodes, helper);
      }
      else
      {
        List<TreeNodePath> nodes = new List<TreeNodePath>();
        foreach (FolderItem2 fi in ((Folder2)folderItem.GetFolder).Items())
        {
          if (!_showAllShellObjects && !fi.IsFileSystem || !fi.IsFolder) continue;
          //						
          TreeNodePath node = CreateTreeNode(helper, fi.Name, fi.Path, IsFolderWithChilds(fi), false, true);
          node.Tag = fi;
          nodes.Add(node);
        }

        // Sort the Directories, as Samba might return unsorted
        TreeNodePath[] nodesArray = nodes.ToArray();
        Array.Sort(nodesArray,
                   new Comparison<TreeNodePath>(
                     delegate(TreeNodePath p1, TreeNodePath p2) { return string.Compare(p1.Text, p2.Text); }));

        parent.Nodes.AddRange(nodesArray);
      }
    }

    public override TreeNodeCollection RequestDriveCollection(TreeViewFolderBrowserHelper helper)
    {
      return _rootCollection;
    }

    public override string ToString()
    {
      return "Shell32 Provider";
    }

    #region internal interface

    protected virtual void FillMyComputer(FolderItem folderItem, TreeNodeCollection parentCollection,
                                          TreeViewFolderBrowserHelper helper)
    {
      _rootCollection = parentCollection;
      Logicaldisk.LogicaldiskCollection logicalDisks = null;
      // get wmi logical disk's if we have to 			
      if (helper.TreeView.DriveTypes != DriveTypes.All)
        logicalDisks = Logicaldisk.GetInstances(null, GetWMIQueryStatement(helper.TreeView));
      //
      foreach (FolderItem fi in ((Folder)folderItem.GetFolder).Items())
      {
        // only File System shell objects ?
        if (!_showAllShellObjects && !fi.IsFileSystem) continue;
        // check drive type 
        if (fi.IsFileSystem && logicalDisks != null)
        {
          bool skipDrive = true;
          foreach (Logicaldisk lg in logicalDisks)
          {
            if (lg.Name + "\\" == fi.Path)
            {
              skipDrive = false;
              break;
            }
          }
          if (skipDrive) continue;
        }
        // create new node
        TreeNodePath node = CreateTreeNode(helper, fi.Name, fi.Path, IsFolderWithChilds(fi), false, true);
        node.Tag = fi;
        parentCollection.Add(node);
      }
    }

    /// <summary>
    ///   Do we have to add a dummy node (+ sign)
    /// </summary>
    protected virtual bool IsFolderWithChilds(FolderItem fi)
    {
      return _showAllShellObjects || (fi.IsFileSystem && fi.IsFolder && !fi.IsBrowsable);
    }

    #endregion
  }

  /// <summary>
  ///   Extends the <c>MenuItem</c> class with a Shell32.FolderItemVerb.
  /// </summary>
  public class MenuItemShellVerb : MenuItem
  {
    #region fields

    private readonly FolderItemVerb _verb;

    #endregion

    #region constructors

    public MenuItemShellVerb(FolderItemVerb verb)
    {
      _verb = verb;
      //
      Text = verb.Name;
    }

    #endregion

    #region internal interface

    protected override void OnClick(EventArgs e)
    {
      base.OnClick(e);
      //
      try
      {
        _verb.DoIt();
      }
      catch {}
    }

    #endregion
  }
}