using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using MPTagThat.Core;

namespace Raccoom.Windows.Forms
{
  /// <summary>
  /// <c>TreeViewFolderBrowserDataProviderMediaPortal</c> is the data provider for <see cref="TreeViewFolderBrowser"/> which retreives it's data from the MediaPOrtal database
  /// <seealso cref="ITreeViewFolderBrowserDataProvider"/>
  /// </summary>
  [System.Drawing.ToolboxBitmap(typeof(System.Data.SqlClient.SqlDataAdapter))]
  public class TreeViewFolderBrowserDataProviderMediaPortal : ITreeViewFolderBrowserDataProvider
  {
    #region Enums
    private enum RootFolder : int
    {
      None,
      Artist,
      AlbumArtist,
      Album,
      Genre
    }


    #endregion

    #region fields
    /// <summary>Shell32 ImageList</summary>
    private Raccoom.Win32.SystemImageList _systemImageList;
    /// <summary>last CheckboxMode used to fill the tree view, saved to know about changes</summary>
    private CheckboxBehaviorMode _checkboxMode;

    private ILogger log = ServiceScope.Get<ILogger>();
    private RootFolder rootFolder = RootFolder.None;
    private bool isRootFolder = false;

    private const string SQL_STMT_ARTIST = "select strArtist from artist order by strArtist";
    private const string SQL_STMT_ARTISTSEARCH = "select distinct strAlbum from tracks where strArtist like '%{0}%' order by strAlbum";
    private const string SQL_STMT_ALBUMARTIST = "select strAlbumArtist from albumartist order by strAlbumArtist";
    private const string SQL_STMT_ALBUMARTISTSEARCH = "select distinct strAlbum from tracks where strAlbumArtist like '%{0}%' order by strAlbum";
    private const string SQL_STMT_ALBUM = "select distinct strAlbum from tracks order by strAlbum";
    private const string SQL_STMT_GENRE = "select strGenre from genre order by strGenre";
    private const string SQL_STMT_GENREARTISTSEARCH = "select distinct ltrim(rtrim(strArtist, ' |'), '| ') from tracks where strGenre like '%{0}%' order by strArtist";
    private const string SQL_STMT_GENREARTISTALBUMSEARCH = "select distinct strAlbum from tracks where strGenre like '%{0}%' AND strArtist like '%{1}%' order by strAlbum";
    #endregion

    #region ITreeViewFolderBrowserDataProvider Members

    public ImageList ImageList
    {
      get
      {
        return null;
      }
    }

    public void QueryContextMenuItems(TreeViewFolderBrowserHelper helper, TreeNodePath node)
    {
    }

    public void RequestRoot(TreeViewFolderBrowserHelper helper)
    {
      AttachSystemImageList(helper);
      TreeNodeCollection rootNodeCollection;

      isRootFolder = true;
      TreeNodePath rootNode = this.CreateTreeNode(helper, "MediaPortal Database", "Root", false, false, true);
      helper.TreeView.Nodes.Add(rootNode);
      rootNodeCollection = helper.TreeView.Nodes[0].Nodes;

      TreeNodePath artistNode = this.CreateTreeNode(helper, "Artist", "Artist", true, false, true);
      artistNode.Tag = "artist";
      rootNodeCollection.Add(artistNode);

      TreeNodePath albumArtistNode = this.CreateTreeNode(helper, "Album Artist", "AlbumArtist", true, false, true);
      albumArtistNode.Tag = "albumartist";
      rootNodeCollection.Add(albumArtistNode);

      TreeNodePath albumNode = this.CreateTreeNode(helper, "Album", "Album", true, false, true);
      albumNode.Tag = "album";
      rootNodeCollection.Add(albumNode);

      TreeNodePath genreNode = this.CreateTreeNode(helper, "Genre", "Genre", true, false, true);
      genreNode.Tag = "genre";
      rootNodeCollection.Add(genreNode);
      isRootFolder = false;
    }

    public void RequestSubDirs(TreeViewFolderBrowserHelper helper, TreeNodePath parent, TreeViewCancelEventArgs e)
    {
      if (parent.Path == null)
      {
        return;
      }

      if (parent.Path == "")
      {
        rootFolder = RootFolder.None;
      }

      string sql = string.Empty;
      bool createDummyNode = true;
      string nodeTag;

      // We have a Special folder, when we are at the root level
      if (parent.IsSpecialFolder)
      {
        rootFolder = RootFolder.None;
      }

      if (rootFolder == RootFolder.None)
      {
        switch ((string)parent.Tag)
        {
          case "artist":
            rootFolder = RootFolder.Artist;
            sql = SQL_STMT_ARTIST;
            break;

          case "albumartist":
            rootFolder = RootFolder.AlbumArtist;
            sql = SQL_STMT_ALBUMARTIST;
            break;

          case "album":
            rootFolder = RootFolder.Album;
            sql = SQL_STMT_ALBUM;
            createDummyNode = false;
            break;

          case "genre":
            rootFolder = RootFolder.Genre;
            sql = SQL_STMT_GENRE;
            break;
        }
      }
      else if (rootFolder == RootFolder.Artist)
      {
        sql = string.Format(SQL_STMT_ARTISTSEARCH, Util.RemoveInvalidChars(parent.Path));
        isRootFolder = false;
        createDummyNode = false;
      }
      else if (rootFolder == RootFolder.AlbumArtist)
      {
        sql = string.Format(SQL_STMT_ALBUMARTISTSEARCH, Util.RemoveInvalidChars(parent.Path));
        isRootFolder = false;
        createDummyNode = false;
      }
      else if (rootFolder == RootFolder.Genre)
      {
        isRootFolder = false;
        string[] searchString = (parent.Tag as string).Split('\\');
        if (searchString.GetLength(0) == 2)
        {
          sql = string.Format(SQL_STMT_GENREARTISTSEARCH, Util.RemoveInvalidChars(parent.Path));
          createDummyNode = true;
        }
        else 
        {
          sql = string.Format(SQL_STMT_GENREARTISTALBUMSEARCH, Util.RemoveInvalidChars(searchString[1]), Util.RemoveInvalidChars(parent.Path));
          createDummyNode = false;
        }
      }

      string connection = string.Format(@"Data Source={0}", MPTagThat.Core.Options.MainSettings.MediaPortalDatabase);
      try
      {
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = sql;
          log.Debug("TreeViewBrowser: Executing sql: {0}", sql);
          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              string dbValue = reader.GetString(0);
              TreeNodePath newNode = this.CreateTreeNode(helper, dbValue, dbValue, createDummyNode, false, false);
              if (isRootFolder)
              {
                nodeTag = (string)parent.Tag;
              }
              else
              {
                nodeTag = string.Format(@"{0}\{1}", (string)parent.Tag, dbValue);
              }
              newNode.Tag = nodeTag;
              parent.Nodes.Add(newNode);
            }
          }
        }
        conn.Close();
      }
      catch (Exception ex)
      {
        log.Error("TreeViewBrowser: Error executing sql: {0}", ex.Message);
      }

    }

    public TreeNodeCollection RequestDriveCollection(TreeViewFolderBrowserHelper helper)
    {
      return helper.TreeView.Nodes;
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
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

      node.ImageIndex = this._systemImageList.IconIndex("", true);
      node.SelectedImageIndex = node.ImageIndex;
    }
    #endregion

    public override string ToString()
    {
      return "MediaPortal Database Provider";
    }
  }
}
