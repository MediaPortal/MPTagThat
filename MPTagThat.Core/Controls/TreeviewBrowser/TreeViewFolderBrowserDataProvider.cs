// Copyright © 2004 by Christoph Richner. All rights are reserved.
// 
// If you like this code then feel free to go ahead and use it.
// The only thing I ask is that you don't remove or alter my copyright notice.
//
// Your use of this software is entirely at your own risk. I make no claims or
// warrantees about the reliability or fitness of this code for any particular purpose.
// If you make changes or additions to this code please mark your code as being yours.
// 
// website http://raccoom.sytes.net, email microweb@bluewin.ch, msn chrisdarebell@msn.com

using System;
using System.Windows.Forms;
using System.IO;

namespace Raccoom.Windows.Forms
{
  /// <summary>
  /// <c>TreeViewFolderBrowserDataProvider</c> is the standard data provider for <see cref="TreeViewFolderBrowser"/> which is based on <see cref="ROOT.CIMV2.Win32.Logicaldisk"/>, System.IO and <see cref="Raccoom.Win32.SystemImageList"/>
  /// <seealso cref="ITreeViewFolderBrowserDataProvider"/>
  /// </summary>
  [System.Drawing.ToolboxBitmap(typeof(System.Data.SqlClient.SqlDataAdapter))]
  public class TreeViewFolderBrowserDataProvider : ITreeViewFolderBrowserDataProvider
  {
    #region fields
    /// <summary>Shell32 ImageList</summary>
    private Raccoom.Win32.SystemImageList _systemImageList;
    /// <summary>last CheckboxMode used to fill the tree view, saved to know about changes</summary>
    private CheckboxBehaviorMode _checkboxMode;
    #endregion

    #region ITreeViewFolderBrowserDataProvider Members
    [System.ComponentModel.Browsable(false)]
    public virtual System.Windows.Forms.ImageList ImageList
    {
      get
      {
        return null;
      }
    }
    public virtual void QueryContextMenuItems(TreeViewFolderBrowserHelper helper, TreeNodePath node)
    {
    }
    public virtual System.Windows.Forms.TreeNodeCollection RequestDriveCollection(TreeViewFolderBrowserHelper helper)
    {
      switch (helper.TreeView.RootFolder)
      {
        case Environment.SpecialFolder.Desktop:
          return helper.TreeView.Nodes[0].Nodes[1].Nodes;
        default:
          return helper.TreeView.Nodes;
      }
    }

    public virtual void RequestSubDirs(TreeViewFolderBrowserHelper helper, TreeNodePath parent, TreeViewCancelEventArgs e)
    {
      if (parent.Path == null) return;
      //
      DirectoryInfo directory = new DirectoryInfo(parent.Path);
      // check persmission
      new System.Security.Permissions.FileIOPermission(System.Security.Permissions.FileIOPermissionAccess.PathDiscovery, directory.FullName).Demand();
      //	
     
      // Sort the Directories, as Samba might return unsorted
      DirectoryInfo[] dirInfo = directory.GetDirectories();
      Array.Sort<DirectoryInfo>(dirInfo,
        new Comparison<DirectoryInfo>(delegate(DirectoryInfo d1, DirectoryInfo d2)
      {
        return string.Compare(d1.Name, d2.Name);
      }));


      foreach (DirectoryInfo dir in dirInfo)
      {
        if ((dir.Attributes & System.IO.FileAttributes.System) == System.IO.FileAttributes.System)
        {
          continue;
        }
        if ((dir.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
        {
          continue;
        }
        TreeNodePath newNode = this.CreateTreeNode(helper, dir.Name, dir.FullName, false, ((helper.TreeView.CheckboxBehaviorMode != CheckboxBehaviorMode.None) && (parent.Checked)), false);
        parent.Nodes.Add(newNode);
        //
        try
        {
          if (dir.GetDirectories().GetLength(0) > 0)
          {
            newNode.AddDummyNode();
          }
        }
        catch { }
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
          TreeNodePath desktopNode = this.CreateTreeNode(helper, Environment.SpecialFolder.Desktop.ToString(), string.Empty, false, false, true);
          helper.TreeView.Nodes.Add(desktopNode);
          rootNodeCollection = helper.TreeView.Nodes[0].Nodes;
          // create child node <Personal>
          string personalDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          rootNodeCollection.Add(this.CreateTreeNode(helper, System.IO.Path.GetFileName(personalDirectory), personalDirectory, true, false, false));
          // create child node <MyComuter>
          TreeNodePath myComputerNode = this.CreateTreeNode(helper, Environment.SpecialFolder.MyComputer.ToString(), string.Empty, false, false, true);
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
          rootNodeCollection.Add(this.CreateTreeNode(helper, System.IO.Path.GetFileName(Environment.GetFolderPath(helper.TreeView.RootFolder)), Environment.GetFolderPath(helper.TreeView.RootFolder), true, false, false));
          populateDrives = false;
          break;
      }
      if (populateDrives)
      {
        // populate local machine drives
        foreach (ROOT.CIMV2.Win32.Logicaldisk logicalDisk in ROOT.CIMV2.Win32.Logicaldisk.GetInstances(null, GetWMIQueryStatement(helper.TreeView)))
        {
          try
          {
            string name = string.Empty;
            string path = logicalDisk.Name + "\\";
            name = logicalDisk.Description;
            //
            name += (name != string.Empty) ? " (" + path + ")" : path;
            // add node to root collection
            driveRootNodeCollection.Add(this.CreateTreeNode(helper, name, path, true, false, false));
          }
          catch (Exception doh)
          {
            throw doh;
          }
        }
      }
    }

    #endregion

    #region IDisposable Members
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
        if (this._systemImageList != null)
        {
          Raccoom.Win32.SystemImageListHelper.SetTreeViewImageList(helper.TreeView, _systemImageList, false);
        }
      }
      _checkboxMode = helper.TreeView.CheckboxBehaviorMode;
    }
    /// <summary>
    /// Creates a new node and assigns a icon
    /// </summary>
    /// <param name="helper"></param>
    /// <param name="text"></param>
    /// <param name="path"></param>
    /// <param name="addDummyNode"></param>
    /// <param name="forceChecked"></param>
    /// <returns></returns>
    protected virtual TreeNodePath CreateTreeNode(TreeViewFolderBrowserHelper helper, string text, string path, bool addDummyNode, bool forceChecked, bool isSpecialFolder)
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
    /// Extract the icon for the file type (Extension)
    /// </summary>
    protected virtual void SetIcon(TreeViewFolderBrowser treeView, TreeNodePath node)
    {
      // create on demand
      if (_systemImageList == null)
      {
        // Shell32 ImageList
        _systemImageList = new Raccoom.Win32.SystemImageList(Raccoom.Win32.SystemImageListSize.SmallIcons);
        Raccoom.Win32.SystemImageListHelper.SetTreeViewImageList(treeView, _systemImageList, false);
      }
      node.ImageIndex = this._systemImageList.IconIndex(node.Path, true);
      node.SelectedImageIndex = node.ImageIndex;
    }
    /// <summary>
    /// Gets the WMI query string based on the current drive types.
    /// </summary>
    /// <returns></returns>
    protected virtual string GetWMIQueryStatement(TreeViewFolderBrowser treeView)
    {
      if ((treeView.DriveTypes & DriveTypes.All) == DriveTypes.All) return string.Empty;
      //
      string where = string.Empty;
      //
      System.Array array = Enum.GetValues(typeof(DriveTypes));
      //
      foreach (DriveTypes type in array)
      {
        if ((treeView.DriveTypes & type) == type)
        {
          if (where == string.Empty)
          {
            where += "drivetype = " + Enum.Format(typeof(Win32_LogicalDiskDriveTypes), Enum.Parse(typeof(Win32_LogicalDiskDriveTypes), type.ToString(), true), "d");
          }
          else
          {
            where += " OR drivetype = " + Enum.Format(typeof(Win32_LogicalDiskDriveTypes), Enum.Parse(typeof(Win32_LogicalDiskDriveTypes), type.ToString(), true), "d");
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
