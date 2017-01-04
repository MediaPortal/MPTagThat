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

using System.Collections;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.Services.MusicDatabase;
using MPTagThat.Core.Services.MusicDatabase.Indexes;
using Raccoom.Win32;

#endregion

namespace Raccoom.Windows.Forms
{
  /// <summary>
  ///   <c>TreeViewFolderBrowserDataProviderMediaPortal</c> is the data provider for <see cref = "TreeViewFolderBrowser" /> which retreives it's data from the Music database
  ///   <seealso cref = "ITreeViewFolderBrowserDataProvider" />
  /// </summary>
  [ToolboxBitmap(typeof (SqlDataAdapter))]
  public class TreeViewFolderBrowserDataProviderMusicDb : ITreeViewFolderBrowserDataProvider
  {
    #region Enums

    private enum RootFolder
    {
      None,
      Artist,
      AlbumArtist,
      Genre
    }

    #endregion

    #region fields

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

    /// <summary>
    ///   last CheckboxMode used to fill the tree view, saved to know about changes
    /// </summary>
    private CheckboxBehaviorMode _checkboxMode;

    /// <summary>
    ///   Shell32 ImageList
    /// </summary>
    private SystemImageList _systemImageList;

    private RootFolder _rootFolder = RootFolder.None;

    #endregion

    #region ITreeViewFolderBrowserDataProvider Members

    public ImageList ImageList => null;

    public void QueryContextMenuItems(TreeViewFolderBrowserHelper helper, TreeNodePath node) {}

    public void RequestRoot(TreeViewFolderBrowserHelper helper)
    {
      AttachSystemImageList(helper);

      TreeNodePath rootNode = CreateTreeNode(helper, "Music Database", "Root", false, false, true);
      helper.TreeView.Nodes.Add(rootNode);
      var rootNodeCollection = helper.TreeView.Nodes[0].Nodes;

      TreeNodePath artistNode = CreateTreeNode(helper, "Artist", "Artist", true, false, true);
      artistNode.Tag = "artist";
      rootNodeCollection.Add(artistNode);

      TreeNodePath albumArtistNode = CreateTreeNode(helper, "Album Artist", "AlbumArtist", true, false, true);
      albumArtistNode.Tag = "albumartist";
      rootNodeCollection.Add(albumArtistNode);

      TreeNodePath genreNode = CreateTreeNode(helper, "Genre", "Genre", true, false, true);
      genreNode.Tag = "genre";
      rootNodeCollection.Add(genreNode);
    }

    public void RequestSubDirs(TreeViewFolderBrowserHelper helper, TreeNodePath parent, TreeViewCancelEventArgs e)
    {
      if (parent.Path == null)
      {
        return;
      }

      if (parent.Path == "")
      {
        _rootFolder = RootFolder.None;
      }

      bool createDummyNode = true;
      string nodeTag;
      IEnumerable result = null;

      // We have a Special folder, when we are at the root level
      if (parent.IsSpecialFolder)
      {
        _rootFolder = RootFolder.None;
      }

      if (_rootFolder == RootFolder.None)
      {
        switch ((string)parent.Tag)
        {
          case "artist":
            _rootFolder = RootFolder.Artist;
            result = ServiceScope.Get<IMusicDatabase>().GetArtists();
            break;

          case "albumartist":
            _rootFolder = RootFolder.AlbumArtist;
            result = ServiceScope.Get<IMusicDatabase>().GetAlbumArtists();
            break;

          case "genre":
            _rootFolder = RootFolder.Genre;
            result = ServiceScope.Get<IMusicDatabase>().GetGenres();
            break;
        }

        foreach (var item in result)
        {
          string value = "";
          if (_rootFolder == RootFolder.Artist || _rootFolder == RootFolder.AlbumArtist)
          {
            value = (item as DistinctResult)?.Name;
          }
          else
          {
            value = (item as DistinctResult)?.Genre;
          }
          
          TreeNodePath newNode = CreateTreeNode(helper, value, value, true, false, false);
          nodeTag = $@"{parent.Tag}\{value}";
          newNode.Tag = nodeTag;
          parent.Nodes.Add(newNode);
        }
        return;
      }

      bool isGenreArtistLevel = true;
      if (_rootFolder == RootFolder.Artist)
      {
        result = ServiceScope.Get<IMusicDatabase>().GetArtistAlbums(parent.Path);
        createDummyNode = false;
      }
      else if (_rootFolder == RootFolder.AlbumArtist)
      {
        result = ServiceScope.Get<IMusicDatabase>().GetAlbumArtistAlbums(parent.Path);
        createDummyNode = false;
      }
      else if (_rootFolder == RootFolder.Genre)
      {
        string[] searchString = (parent.Tag as string).Split('\\');
        if (searchString.GetLength(0) == 2)
        {
          result = ServiceScope.Get<IMusicDatabase>().GetGenreArtists(parent.Path);
        }
        else
        {
          isGenreArtistLevel = false;
          result = ServiceScope.Get<IMusicDatabase>().GetGenreArtistAlbums(searchString[1], searchString[2]);
          createDummyNode = false;
        }
      }

      foreach (var item in result)
      {
        string value = ""; 

        if (_rootFolder == RootFolder.Artist || _rootFolder == RootFolder.AlbumArtist)
        {
          value = (item as DistinctResult)?.Album;
        }
        else
        {
          if (isGenreArtistLevel)
          {
            value = (item as DistinctResult)?.Name;
          }
          else
          {
            value = (item as DistinctResult)?.Album;
          }
        }
        
        TreeNodePath newNode = CreateTreeNode(helper, value, value, createDummyNode, false, false);
        nodeTag = $@"{parent.Tag}\{value}";
        newNode.Tag = nodeTag;
        parent.Nodes.Add(newNode);
      }
    }

    public TreeNodeCollection RequestDriveCollection(TreeViewFolderBrowserHelper helper, bool isNetwork)
    {
      return helper.TreeView.Nodes;
    }

    public void Dispose()
    {
      _systemImageList?.Dispose();
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
    /// <param name="isSpecialFolder"></param>
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

      node.ImageIndex = _systemImageList.IconIndex("", true);
      node.SelectedImageIndex = node.ImageIndex;
    }

    #endregion

    public override string ToString()
    {
      return "Music Database";
    }
  }
}
