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
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Raccoom.Windows.Forms
{
	/// <summary>
	/// <c>TreeViewFolderBrowserDataProvider</c> is the shell32 interop data provider for <see cref="TreeViewFolderBrowser"/> which is based on <see cref="ROOT.CIMV2.Win32.Logicaldisk"/>, <c>Shell32</c> Interop and <see cref="Raccoom.Win32.SystemImageList"/>
	/// <seealso cref="ITreeViewFolderBrowserDataProvider"/>
	/// </summary>
	/// <remarks>
	/// Shell32 does not support the .NET System.Security.Permissions system. There is no code access permission, only FileSystem ACL.
	/// </remarks>
	[System.ComponentModel.DefaultProperty("ShowAllShellObjects"), System.Drawing.ToolboxBitmap(typeof(System.Data.SqlClient.SqlDataAdapter))]
	public class TreeViewFolderBrowserDataProviderShell32 : TreeViewFolderBrowserDataProvider
	{		
		#region fields
		private TreeViewFolderBrowserHelper _helper;
		/// <summary>Shell32 Com Object</summary>
		private Raccoom.Win32.Shell32Namespaces _shell = new Raccoom.Win32.Shell32Namespaces();		
		/// <summary>drive tree node (mycomputer) root collection</summary>
		private TreeNodeCollection _rootCollection = null;
		/// <summary>show only filesystem</summary>
		private bool _showAllShellObjects = false;
		private bool _enableContextMenu;
		#endregion

		#region constructors
		#endregion

		#region public interface
		/// <summary>
		/// Enables or disables the context menu which show's the folder item's shell verbs.
		/// </summary>
		[System.ComponentModel.Browsable(true), System.ComponentModel.Category("Behaviour"), System.ComponentModel.Description("Specifies if the context menu is enabled."), System.ComponentModel.DefaultValue(false)]
		public bool EnableContextMenu
		{
			get
			{
				return _enableContextMenu;
			}
			set
			{
				_enableContextMenu = value;
			}
		}
		[System.ComponentModel.Browsable(true), System.ComponentModel.Category("Behaviour"), System.ComponentModel.Description("Display file system and virtual shell folders."), System.ComponentModel.DefaultValue(false)]
		public bool ShowAllShellObjects
		{
			get
			{
				return _showAllShellObjects;
			}
			set
			{
				_showAllShellObjects = value;
			}
		}
		#endregion

		#region ITreeViewFolderBrowserDataProvider Members
		public override void QueryContextMenuItems(TreeViewFolderBrowserHelper helper, TreeNodePath node)
		{
			if(!EnableContextMenu) return;
			//
			Shell32.FolderItem fi = node.Tag as Shell32.FolderItem;
			if(fi==null) return;
			//
			foreach(Shell32.FolderItemVerb verb in fi.Verbs())
			{
				if(verb.Name.Length==0) continue;
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
			switch(helper.TreeView.RootFolder)
			{
				case Environment.SpecialFolder.Desktop:
					Shell32.Folder2 desktopFolder = (Shell32.Folder2) _shell.GetDesktop();					
					// create root node <Desktop>
					TreeNodePath desktopNode = CreateTreeNode(helper, desktopFolder.Title,desktopFolder.Self.Path,false,false, true);
					helper.TreeView.Nodes.Add(desktopNode);
					desktopNode.Tag = desktopFolder;					
					//
					Shell32.Folder2 myComputer = (Shell32.Folder2) _shell.Shell.NameSpace(Shell32.ShellSpecialFolderConstants.ssfDRIVES);
					foreach(Shell32.FolderItem fi in desktopFolder.Items())
					{
						if(!fi.IsFolder) continue;
						//
						TreeNodePath node = CreateTreeNode(helper, fi.Name, fi.Path,true, false,true);
						node.Tag = fi;
						desktopNode.Nodes.Add(node);
						//
						if(_shell.Shell.NameSpace(Shell32.ShellSpecialFolderConstants.ssfDRIVES).Title==fi.Name)_rootCollection = node.Nodes;
					}				
					break;
				case Environment.SpecialFolder.MyComputer:					
					this.FillMyComputer(((Shell32.Folder2)_shell.Shell.NameSpace(Shell32.ShellSpecialFolderConstants.ssfDRIVES)).Self, helper.TreeView.Nodes, helper);
					break;
				default:
					// create root node with specified SpecialFolder
					Shell32.Folder2 root = (Shell32.Folder3)_shell.Shell.NameSpace(helper.TreeView.RootFolder);
					TreeNodePath rootNode = CreateTreeNode(helper, root.Title,root.Self.Path,true,false, true);
					rootNode.Tag = root.Self;
					helper.TreeView.Nodes.Add(rootNode);
					_rootCollection = rootNode.Nodes;
					break;
			}
		}

		public override void RequestSubDirs(TreeViewFolderBrowserHelper helper, TreeNodePath parent, TreeViewCancelEventArgs e)
		{		
			if(!parent.IsSpecialFolder) return;
			//
			Shell32.FolderItem folderItem = ((Shell32.FolderItem)parent.Tag);
			//
			if(_shell.Shell.NameSpace(Shell32.ShellSpecialFolderConstants.ssfDRIVES).Title==folderItem.Name)
			{
				FillMyComputer(folderItem, parent.Nodes, helper);
			} 
			else
			{
				foreach(Shell32.FolderItem fi in ((Shell32.Folder)folderItem.GetFolder).Items())
				{						
					if(!_showAllShellObjects && !fi.IsFileSystem || !fi.IsFolder) continue;
					//						
					TreeNodePath node = CreateTreeNode(helper, fi.Name, fi.Path,IsFolderWithChilds(fi), false,true);
					node.Tag = fi;
					parent.Nodes.Add( node );						
				}
			}		
		}
		public override  TreeNodeCollection RequestDriveCollection(TreeViewFolderBrowserHelper helper)
		{
			return _rootCollection;
			
		}
		#endregion

		#region internal interface
		protected virtual void FillMyComputer(Shell32.FolderItem folderItem, TreeNodeCollection parentCollection, TreeViewFolderBrowserHelper helper)
		{
			_rootCollection = parentCollection;
			ROOT.CIMV2.Win32.Logicaldisk.LogicaldiskCollection logicalDisks = null;
			// get wmi logical disk's if we have to 			
			if(helper.TreeView.DriveTypes != DriveTypes.All) logicalDisks = ROOT.CIMV2.Win32.Logicaldisk.GetInstances(null,GetWMIQueryStatement(helper.TreeView));
			//
			foreach(Shell32.FolderItem fi in ((Shell32.Folder)folderItem.GetFolder).Items())
			{				
				// only File System shell objects ?
				if(!_showAllShellObjects && !fi.IsFileSystem) continue;	
				// check drive type 
				if(fi.IsFileSystem && logicalDisks!=null)
				{
					bool skipDrive = true;
					foreach(ROOT.CIMV2.Win32.Logicaldisk lg in logicalDisks)
					{
						if(lg.Name+"\\" == fi.Path)
						{
							skipDrive = false;
							break;
						}
					}
					if(skipDrive) continue;
				}
				// create new node
				TreeNodePath node = CreateTreeNode(helper, fi.Name, fi.Path,IsFolderWithChilds(fi), false, true);
				node.Tag = fi;
				parentCollection.Add( node );						
			}
		}	
		/// <summary>
		/// Do we have to add a dummy node (+ sign)
		/// </summary>
		protected virtual bool IsFolderWithChilds(Shell32.FolderItem fi)
		{
			return _showAllShellObjects || (fi.IsFileSystem && fi.IsFolder && !fi.IsBrowsable);
		}
		#endregion		

		public override string ToString()
		{
			return "Shell32 Provider";
		}
	}
	/// <summary>
	/// Extends the <c>MenuItem</c> class with a Shell32.FolderItemVerb.
	/// </summary>
	public class MenuItemShellVerb : MenuItem
	{
		#region fields
		private Shell32.FolderItemVerb _verb;
		#endregion

		#region constructors
		public MenuItemShellVerb(Shell32.FolderItemVerb verb)
		{
			_verb = verb;			
			//
			Text = verb.Name;			
		}
		#endregion

		#region internal interface
		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);
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
