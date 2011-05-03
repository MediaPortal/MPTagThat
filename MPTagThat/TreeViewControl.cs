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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using MPTagThat.Core;
using Raccoom.Windows.Forms;

#endregion

namespace MPTagThat
{
  public partial class TreeViewControl : UserControl
  {
    #region Variables

    private readonly List<Item> _fileFormats = new List<Item>();

    private readonly string[] _filterFieldValues = new[]
                                                     {
                                                       "artist", "albumartist", "album", "title", "year", "genre",
                                                       "picture",
                                                       "lyrics", "track", "numtracks", "disc", "numdiscs", "rating",
                                                       "bpm",
                                                       "comment", "composer", "conductor", "bitrate", "samplerate",
                                                       "channels"
                                                     };

    private readonly Main _main;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private bool _actionCopy;
    private bool _databaseMode;

    private TreeViewFilter _filter;
    private TreeNodePath _nodeToCopyCut;
    private TreeNode _previousHoverNode;
    private DateTime _savedTime;

    #endregion

    #region Properties

    public TreeViewFolderBrowser TreeView
    {
      get { return treeViewFolderBrowser; }
    }

    public bool ScanFolderRecursive
    {
      get { return checkBoxRecursive.Checked; }
      set { checkBoxRecursive.Checked = value; }
    }

    public bool DatabaseMode
    {
      get { return _databaseMode; }
      set
      {
        _databaseMode = value;
        SwitchMode();
      }
    }

    public TreeViewFilter ActiveFilter
    {
      get { return _filter; }
    }

    #endregion

    #region ctor

    public TreeViewControl(Main main)
    {
      _main = main;

      InitializeComponent();

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      LocaliseScreen();

      LoadSettings();

      tabControlTreeView.SelectFirstTab();
    }

    #endregion

    #region Public Methods

    #region Init

    public void Init()
    {
      _databaseMode = false;
      cbDataProvider.SelectedIndex = Options.MainSettings.DataProvider;
      //  Add the event Handler here to prevent it from firing, when setting the selected item
      cbDataProvider.SelectedIndexChanged += cbDataProvider_SelectedIndexChanged;
      if (Options.MainSettings.DataProvider == 2 && File.Exists(Options.MainSettings.MediaPortalDatabase))
      {
        _databaseMode = true;
      }
      treeViewFolderBrowser.DriveTypes = DriveTypes.LocalDisk | DriveTypes.NetworkDrive | DriveTypes.RemovableDisk |
                                         DriveTypes.CompactDisc;
      treeViewFolderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
      treeViewFolderBrowser.CheckboxBehaviorMode = CheckboxBehaviorMode.None;
      SwitchMode();
    }


    /// <summary>
    ///   Refreshes the Foldrs
    /// </summary>
    public void RefreshFolders()
    {
      treeViewFolderBrowser.Populate();
      treeViewFolderBrowser.ShowFolder(_main.CurrentDirectory);
    }

    public void DeleteFolder()
    {
      log.Trace(">>>");
      TreeNodePath node = treeViewFolderBrowser.SelectedNode as TreeNodePath;
      if (node != null)
      {
        try
        {
          FileSystem.DeleteDirectory(node.Path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);

          // Clear the tracks
          _main.TracksGridView.TrackList.Clear();
          _main.ClearGallery();

          // Now set the Selected directory to the Parent of the delted folder and reread the view
          TreeNodePath parent = node.Parent as TreeNodePath;
          _main.CurrentDirectory = parent.Path;
          treeViewFolderBrowser.Populate();
          treeViewFolderBrowser.ShowFolder(_main.CurrentDirectory);
        }
        catch (OperationCanceledException)
        { }
        catch (DirectoryNotFoundException)
        { }
        catch (NotSupportedException)
        { }
        catch (Exception ex)
        {
          log.Error("Error deleting Folder {0} {1}", node.Path, ex.Message);
        }
      }
      log.Trace("<<<");
    }

    #endregion

    #endregion

    #region Private Methids

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      // Extended Panels. Doing it via TTExtendedPanel doesn't work for some reason
      treeViewPanel.CaptionText = localisation.ToString("main", "TreeViewPanel");
      optionsPanelLeft.CaptionText = localisation.ToString("main", "OptionsPanel");
      contextMenuTreeView.Items[0].Text = localisation.ToString("contextmenu", "Copy");
      contextMenuTreeView.Items[1].Text = localisation.ToString("contextmenu", "Cut");
      contextMenuTreeView.Items[2].Text = localisation.ToString("contextmenu", "Paste");
      contextMenuTreeView.Items[3].Text = localisation.ToString("contextmenu", "Delete");
      contextMenuTreeView.Items[4].Text = localisation.ToString("contextmenu", "Refresh");

      contextMenuStripFilter.Items[0].Text = localisation.ToString("contextmenu", "InsertFilter");
      contextMenuStripFilter.Items[1].Text = localisation.ToString("contextmenu", "DeleteFilter");

      // Filter Grid Headings
      TagFilterField.HeaderText = localisation.ToString("main", "FilterHeadingField");
      TagFilterValue.HeaderText = localisation.ToString("main", "FilterHeadingFilter");
      TagFilterOperator.HeaderText = localisation.ToString("main", "FilterHeadingOperator");

      // Data Provider Combo
      cbDataProvider.Items.Clear();
      cbDataProvider.Items.Add(localisation.ToString("main", "FolderView"));
      cbDataProvider.Items.Add(localisation.ToString("main", "NetworkView"));
      cbDataProvider.Items.Add(localisation.ToString("main", "DBView"));
    }

    #endregion

    #region Settings

    private void LoadSettings()
    {
      // Fill the Filter Field Combo with values
      //
      // For some reason assigning a DataSource doesn't work
      // So i need to keep track of the value members in a separate string array.
      TagFilterField.Items.Add(localisation.ToString("column_header", "Artist"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "AlbumArtist"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Album"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Title"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Year"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Genre"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Picture"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Lyrics"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Track"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "NumTracks"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Disc"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "NumDisc"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Rating"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "BPM"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Comment"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Composer"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Conductor"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "BitRate"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "SampleRate"));
      TagFilterField.Items.Add(localisation.ToString("column_header", "Channels"));

      TagFilterOperator.Items.Add(localisation.ToString("main", "FilterOperatorAnd"));
      TagFilterOperator.Items.Add(localisation.ToString("main", "FilterOperatorOr"));

      // Fill the File Select Combo with values
      _fileFormats.Add(new Item(localisation.ToString("main", "FilterAllFiles"), "*", ""));
      _fileFormats.Add(new Item(".mp3 (MPEG Layer-3)", "mp3", ""));
      _fileFormats.Add(new Item(".ogg (OGG Vorbis)", "ogg", ""));
      _fileFormats.Add(new Item(".wma (Windows Media Audio)", "wma", ""));
      _fileFormats.Add(new Item(".flac (Free Lossless Audio Codec)", "flac", ""));
      _fileFormats.Add(new Item(".ape (Monkey's Audio)", "ape", ""));
      _fileFormats.Add(new Item(".mpc/.mpp/.mp+ (MusePack)", "mpc|mpp|mp+", ""));
      _fileFormats.Add(new Item(".mp4/.m4a/.m4p (MPEG-4/AAC)", "mp4|m4a|m4p", ""));
      _fileFormats.Add(new Item(".wv (Wavpack)", "wv", ""));
      _fileFormats.Add(new Item(".wav (Wave / RIFF)", "wav", ""));
      _fileFormats.Add(new Item(".aif|.aiff (Audio Interchange File Format)", "aif|aiff", ""));
      cbListFormats.DisplayMember = "Name";
      cbListFormats.ValueMember = "Value";
      cbListFormats.DataSource = _fileFormats;


      if (Options.TreeViewSettings.Filter.Count > 0)
      {
        _filter = Options.TreeViewSettings.Filter[0];
        if (Options.TreeViewSettings.LastUsedFormat != "")
        {
          foreach (TreeViewFilter filter in Options.TreeViewSettings.Filter)
          {
            if (filter.Name == Options.TreeViewSettings.LastUsedFormat)
            {
              _filter = filter;
              break;
            }
          }
        }

        // Now set the File Formats
        foreach (Item item in _fileFormats)
        {
          if ((string)item.Value == _filter.FileFilter)
          {
            cbListFormats.SelectedItem = item;
            break;
          }
        }

        ckUseTagFilter.Checked = _filter.UseTagFilter;
        ckUseTagFilter_CheckedChanged(ckUseTagFilter, new EventArgs());
        dataGridViewTagFilter.Enabled = ckUseTagFilter.Checked;
        int rowIndex = 0;
        foreach (TreeViewTagFilter tagFilter in _filter.TagFilter)
        {
          dataGridViewTagFilter.Rows.Insert(rowIndex, 1);

          int i = 0;
          foreach (string filterFieldValue in _filterFieldValues)
          {
            if (filterFieldValue == tagFilter.Field)
            {
              dataGridViewTagFilter.Rows[rowIndex].Cells[0].Value = TagFilterField.Items[i];
              break;
            }
            i++;
          }

          if (dataGridViewTagFilter.Rows[rowIndex].Cells[0].Value == null)
          {
            rowIndex++;
            continue;
          }

          if (IsSpecialFilterColumn(dataGridViewTagFilter.Rows[rowIndex].Cells[0].Value.ToString()))
          {
            DataGridViewCheckBoxCell ckCell = new DataGridViewCheckBoxCell();
            if (tagFilter.FilterValue == "1")
            {
              tagFilter.FilterValue = "true";
            }
            ckCell.Value = tagFilter.FilterValue;
            dataGridViewTagFilter.Rows[rowIndex].Cells[1] = ckCell;
          }
          else
          {
            dataGridViewTagFilter.Rows[rowIndex].Cells[1].Value = tagFilter.FilterValue;
          }

          string op = null;
          if (tagFilter.FilterOperator == "or")
          {
            op = "or";
          }
          else if (tagFilter.FilterOperator == "and")
          {
            op = "and";
          }
          dataGridViewTagFilter.Rows[rowIndex].Cells[2].Value = op;
          rowIndex++;
        }
      }
    }

    #endregion

    private void SetNodeHoverColor(TreeNode node)
    {
      if (node == _previousHoverNode)
      {
        return;
      }

      _savedTime = DateTime.Now;
      node.BackColor = Color.LightSkyBlue;
      if (_previousHoverNode != null)
      {
        _previousHoverNode.BackColor = treeViewFolderBrowser.BackColor;
      }
      _previousHoverNode = node;
    }

    private void SwitchMode()
    {
      _main.TracksGridView.TrackList.Clear();
      _main.ClearGallery();

      if (_databaseMode)
      {
        treeViewFolderBrowser.DataSource = new TreeViewFolderBrowserDataProviderMediaPortal();
        if (_main.SplitterTop.IsCollapsed)
        {
          _main.SplitterTop.ToggleState();
        }
        treeViewPanel.CaptionText = localisation.ToString("main", "TreeViewPanelDatabase");
        treeViewFolderBrowser.AllowDrop = false;
        checkBoxRecursive.Enabled = false;
        btnRefreshFolder.Enabled = false;
        _databaseMode = true;
      }
      else
      {
        if (Options.MainSettings.DataProvider == 0)
        {
          treeViewFolderBrowser.DataSource = new TreeViewFolderBrowserDataProvider();
        }
        else
        {
          TreeViewFolderBrowserDataProviderShell32 shell32Provider = new TreeViewFolderBrowserDataProviderShell32();
          shell32Provider.ShowAllShellObjects = true;
          treeViewFolderBrowser.DataSource = shell32Provider;
        }

        if (!_main.SplitterTop.IsCollapsed)
        {
          _main.SplitterTop.ToggleState();
        }
        treeViewPanel.CaptionText = localisation.ToString("main", "TreeViewPanel");
        treeViewFolderBrowser.AllowDrop = true;
        checkBoxRecursive.Enabled = true;
        btnRefreshFolder.Enabled = true;
        _databaseMode = false;
      }
    }


    private bool IsSpecialFilterColumn(string filterField)
    {
      string filter = _filterFieldValues[TagFilterField.Items.IndexOf(filterField)];

      switch (filter)
      {
        case "picture":
        case "lyrics":
          return true;

        default:
          return false;
      }
    }

    #endregion

    #region Events

    #region Treeview

    /// <summary>
    ///   The Treeview Control is the active control
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_Enter(object sender, EventArgs e)
    {
      _main.TreeViewSelected = true;
    }

    /// <summary>
    ///   The Treeview Control is no longer the active Control
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_Leave(object sender, EventArgs e)
    {
      _main.TreeViewSelected = false;
    }

    /// <summary>
    ///   A new Folder has been selected
    ///   Only allow navigation, if no folder scanning is active
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      if (_main.FolderScanning)
        e.Cancel = true;
    }

    /// <summary>
    ///   A Folder has been selected in the TreeView. Read the content
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_AfterSelect(object sender, TreeViewEventArgs e)
    {
      log.Trace(">>>");
      if (e.Action == TreeViewAction.Unknown)
        // Called on startup of the program and can be ignored
        return;

      // Don't allow navigation, while Ripping or Burning
      if (_main.Burning || _main.Ripping)
        return;

      TreeNodePath node = e.Node as TreeNodePath;
      if (node != null)
      {
        if (DatabaseMode)
        {
          _main.CurrentDirectory = (string)node.Tag;
        }
        else
        {
          _main.CurrentDirectory = node.Path;
        }

        // Check, if the user selected a CD/DVD drive
        bool isCDDrive = false;
        string driveLetter = "";
        if (node.Path.Length == 3)
        {
          driveLetter = node.Path.Substring(0, 1);
          isCDDrive = Util.IsCDDrive(driveLetter);
        }
        if (isCDDrive)
        {
          _main.Ribbon.CurrentTabPage = _main.TabRip;
          _main.BurnGridView.Hide();
          _main.RipGridView.Show();
          _main.TracksGridView.Hide();
          if (!_main.SplitterRight.IsCollapsed)
          {
            _main.SplitterRight.ToggleState();
          }
          _main.RipGridView.SelectedCDRomDrive = driveLetter;
        }
        else
        {
          // If we selected a folder, while being in the Burn or Rip View, go to the TagTab
          if (_main.Ribbon.CurrentTabPage != _main.TabTag)
          {
            _main.Ribbon.CurrentTabPage = _main.TabTag;
            _main.BurnGridView.Hide();
            _main.RipGridView.Hide();
            _main.TracksGridView.Show();
            if (_main.SplitterRight.IsCollapsed && !Options.MainSettings.RightPanelCollapsed)
              _main.SplitterRight.ToggleState();
          }
          _main.SetRecentFolder(_main.CurrentDirectory);
          _main.RefreshTrackList();
        }
      }
      log.Trace("<<<");
    }

    /// <summary>
    ///   The User selected the Treeview
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_Click(object sender, EventArgs e)
    {
      _main.TreeViewSelected = true;
    }

    /// <summary>
    ///   THe user edited (renamed) a folder
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      if (e.Label == null)
      {
        return;
      }

      TreeNodePath node = e.Node as TreeNodePath;

      string sourcePath = node.Path;
      string targetPath = Path.Combine(Path.GetDirectoryName(node.Path), e.Label);
      if (Directory.Exists(targetPath)) { }

      bool bError = false;
      try
      {
        log.Debug("TreeView: Renaming folder {0} to {1}", sourcePath, targetPath);
        FileSystem.RenameDirectory(sourcePath, e.Label);
      }
      catch (Exception ex)
      {
        e.CancelEdit = true;
        bError = true;
        log.Error("TreeView: Error renaming folder {0}. {1}", sourcePath, ex.Message);
      }
      if (!bError)
      {
        e.CancelEdit = false;
        node.Path = targetPath;
        node.Text = e.Label;
        _main.CurrentDirectory = targetPath;
        treeViewFolderBrowser.Sort();
        _main.RefreshTrackList();
      }
    }

    /// <summary>
    ///   Show the Treview Context Menu on Right Mouse Click
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        // Point where the mouse is clicked.
        Point p = new Point(e.X, e.Y);

        // Get the node that the user has clicked.
        TreeNode node = treeViewFolderBrowser.GetNodeAt(p);
        if (node != null)
        {
          treeViewFolderBrowser.SelectedNode = node;
          if (!_databaseMode)
          {
            contextMenuTreeView.Show(treeViewFolderBrowser, p);
          }
        }
      }
    }

    /// <summary>
    ///   The user hoevrs with the mouse over a node.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
    {
      SetNodeHoverColor(e.Node);
    }

    /// <summary>
    ///   Gets the treenode, where the cursor currently points to
    /// </summary>
    /// <param name = "point"></param>
    /// <returns></returns>
    private TreeNode GetNode(Point point)
    {
      // We need to tranlate the coordinates to the position within the Main form
      Point pt = Parent.Parent.PointToClient(Parent.PointToScreen(PointToScreen(treeViewFolderBrowser.Location)));
      TreeViewHitTestInfo hitTestInfo = treeViewFolderBrowser.HitTest(point.X - pt.X, point.Y - pt.Y - 20);
      return hitTestInfo.Node;
    }

    #endregion

    #region Drag & Drop

    /// <summary>
    ///   Some files are dragged over the Treeview
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_DragOver(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(typeof(List<TrackData>)))
      {
        return;
      }

      if (e.KeyState == 9 && (e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
      // The Ctrl Key + LMB was pressed
      {
        e.Effect = DragDropEffects.Copy;
      }
      else
      {
        e.Effect = DragDropEffects.Move;
      }

      TreeNode node = GetNode(new Point(e.X, e.Y));
      if (node != null)
      {
        SetNodeHoverColor(node);


        if (!node.IsExpanded)
        {
          TimeSpan span = DateTime.Now - _savedTime;
          if (span.Seconds >= 1)
          {
            node.Expand();
          }
        }
      }
    }

    /// <summary>
    ///   The drag and drop is completed. do the actual move / copy
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void treeViewFolderBrowser_DragDrop(object sender, DragEventArgs e)
    {
      bool bMove = true;
      if (!e.Data.GetDataPresent(typeof(List<TrackData>)))
      {
        return;
      }

      if (e.KeyState == 8) // The Ctrl Key was pressed
      {
        bMove = false;
      }
      else
      {
        bMove = true;
      }

      try
      {
        TreeNodePath node = GetNode(new Point(e.X, e.Y)) as TreeNodePath;
        ;
        if (node == null)
        {
          log.Debug("TreeView: Files are not dragged to a node. Abort processing");
          return;
        }
        if (node.Path == (treeViewFolderBrowser.SelectedNode as TreeNodePath).Path)
        {
          log.Debug("TreeView: Source and Target may not be the same on drag and drop. Abort processing");
          return;
        }

        List<TrackData> selectedRows = (List<TrackData>)e.Data.GetData(typeof(List<TrackData>));
        foreach (TrackData track in selectedRows)
        {
          string targetFile = Path.Combine(node.Path, track.FileName);
          if (bMove)
          {
            log.Debug("TreeView: Moving file {0} to {1}", track.FullFileName, targetFile);
            FileSystem.MoveFile(track.FullFileName, targetFile, UIOption.AllDialogs, UICancelOption.DoNothing);
          }
          else
          {
            log.Debug("TreeView: Copying file {0} to {1}", track.FullFileName, targetFile);
            FileSystem.CopyFile(track.FullFileName, targetFile, UIOption.AllDialogs, UICancelOption.DoNothing);
          }
        }
      }
      catch (Exception ex)
      {
        log.Debug("TreeView: Exception while copying moving file {0} to {1}", ex.Message, ex.StackTrace);
      }
    }

    #endregion

    #region Context Menu

    /// <summary>
    ///   Refresh Button on Treeview Context Menu has been clicked
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void contextMenuTreeViewRefresh_Click(object sender, EventArgs e)
    {
      RefreshFolders();
    }

    /// <summary>
    ///   Delete Button on Treeview Context Menu has been clicked
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void contextMenuTreeViewDelete_Click(object sender, EventArgs e)
    {
      DeleteFolder();
    }

    /// <summary>
    ///   Copy Button has been selected in Context Menu
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void contextMenuTreeViewCopy_Click(object sender, EventArgs e)
    {
      contextMenuTreeView.Items[2].Enabled = true; // Enable the Paste Menu item
      _actionCopy = true;
      _nodeToCopyCut = treeViewFolderBrowser.SelectedNode as TreeNodePath;
    }

    /// <summary>
    ///   Cut Button has been selected in Context Menu
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void contextMenuTreeViewCut_Click(object sender, EventArgs e)
    {
      contextMenuTreeView.Items[2].Enabled = true; // Enable the Paste Menu item
      _actionCopy = false; // we do a move
      _nodeToCopyCut = treeViewFolderBrowser.SelectedNode as TreeNodePath;
    }

    /// <summary>
    ///   Paste Button has been selected in Context Menu
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void contextMenuTreeViewPaste_Click(object sender, EventArgs e)
    {
      contextMenuTreeView.Items[2].Enabled = false; // Disable the Paste Menu item
      string targetPath = Path.Combine((treeViewFolderBrowser.SelectedNode as TreeNodePath).Path, _nodeToCopyCut.Text);
      try
      {
        if (!Directory.Exists(targetPath))
        {
          Directory.CreateDirectory(targetPath);
        }
      }
      catch (Exception ex)
      {
        log.Error("TreeView: Error creating folder {0}. {1}", targetPath, ex.Message);
      }

      bool bError = false;
      try
      {
        if (_actionCopy)
        {
          log.Debug("TreeView: Copying folder {0} to {1}", _nodeToCopyCut, targetPath);
          FileSystem.CopyDirectory(_nodeToCopyCut.Path, targetPath, UIOption.AllDialogs, UICancelOption.DoNothing);
        }
        else
        {
          log.Debug("TreeView: Moving folder {0} to {1}", _nodeToCopyCut, targetPath);
          FileSystem.MoveDirectory(_nodeToCopyCut.Path, targetPath, UIOption.AllDialogs, UICancelOption.DoNothing);
        }
      }
      catch (Exception ex)
      {
        bError = true;
        log.Error("TreeView: Error copying / moving folder {0}. {1}", _nodeToCopyCut.Path, ex.Message);
      }
      if (!bError)
      {
        _nodeToCopyCut = null;
        _main.CurrentDirectory = targetPath;
        RefreshFolders();
        _main.RefreshTrackList();
      }
    }

    #endregion

    #region Buttons / Combo

    /// <summary>
    ///   Refresh the Folder List
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btnRefreshFolder_Click(object sender, EventArgs e)
    {
      RefreshFolders();
    }

    private void cbDataProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
      Options.MainSettings.DataProvider = cbDataProvider.SelectedIndex;
      if (cbDataProvider.SelectedIndex == 2 && File.Exists(Options.MainSettings.MediaPortalDatabase))
      {
        _databaseMode = true;
      }
      else
      {
        _databaseMode = false;
      }
      SwitchMode();
      treeViewFolderBrowser.Populate();
      treeViewFolderBrowser.Nodes[0].Expand();
    }

    #endregion

    #region Filter

    /// <summary>
    ///   Handles editing of data columns
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void dataGridViewTagFilter_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      // For combo box and check box cells, commit any value change as soon
      // as it is made rather than waiting for the focus to leave the cell.
      if (!dataGridViewTagFilter.CurrentCell.GetType().Equals(typeof(DataGridViewTextBoxCell)))
      {
        dataGridViewTagFilter.CommitEdit(DataGridViewDataErrorContexts.Commit);

        if (dataGridViewTagFilter.CurrentCell.ColumnIndex == 0 &&
            IsSpecialFilterColumn(dataGridViewTagFilter.CurrentCell.EditedFormattedValue.ToString()))
        {
          DataGridViewCheckBoxCell ckCell = new DataGridViewCheckBoxCell();
          ckCell.Value = true;
          dataGridViewTagFilter.CurrentRow.Cells[1] = ckCell;
        }
        else
        {
          if (dataGridViewTagFilter.CurrentCell.ColumnIndex == 0 &&
              dataGridViewTagFilter.CurrentRow.Cells[1].GetType().Equals(typeof(DataGridViewCheckBoxCell)))
          {
            DataGridViewTextBoxCell tbCell = new DataGridViewTextBoxCell();
            tbCell.Value = "";
            dataGridViewTagFilter.CurrentRow.Cells[1] = tbCell;
          }
        }
        RefreshFilter();
      }
    }

    /// <summary>
    ///   Handle Data Error
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void dataGridViewTagFilter_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      e.Cancel = false;
    }

    /// <summary>
    ///   Capture Key presses in the Filter GridView
    /// </summary>
    /// <param name = "msg"></param>
    /// <param name = "keyData"></param>
    /// <returns></returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (dataGridViewTagFilter.Focused)
      {
        int curIndex = -1;
        if (dataGridViewTagFilter.CurrentRow == null)
        {
          curIndex = -1;
        }
        else
        {
          curIndex = dataGridViewTagFilter.CurrentRow.Index;
        }

        if (keyData == Keys.Insert)
        {
          dataGridViewTagFilter.Rows.Insert(curIndex + 1, 1);
          return true;
        }
        else if (keyData == Keys.Delete)
        {
          if (curIndex > -1 && dataGridViewTagFilter.CurrentRow != null)
          {
            dataGridViewTagFilter.Rows.RemoveAt(curIndex);
            return true;
          }
        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    ///   A New Filter Format has been selected
    ///   Update the filter
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void cbListFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
      Item item = (Item)(sender as ComboBox).SelectedItem;
      _filter.FileFilter = (string)item.Value;
    }

    /// <summary>
    ///   The File Mask is changed. Update the filter
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tbFileMask_TextChanged(object sender, EventArgs e)
    {
      _filter.FileMask = (sender as TextBox).Text;
    }

    /// <summary>
    ///   The stats of the Use Tag Fileter Check box has changed
    ///   Set the filter Value
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void ckUseTagFilter_CheckedChanged(object sender, EventArgs e)
    {
      _filter.UseTagFilter = ckUseTagFilter.Checked;
      dataGridViewTagFilter.Enabled = ckUseTagFilter.Checked;

      // Set the Status label
      if (_filter.UseTagFilter)
      {
        _main.ToolStripStatusFilter.Text = localisation.ToString("main", "FilterActive");
      }
      else
      {
        _main.ToolStripStatusFilter.Text = localisation.ToString("main", "FilterInActive");
      }
    }

    /// <summary>
    ///   Called when Rows are Deleted / Changed
    ///   Fileter Values need to be updated
    /// </summary>
    private void RefreshFilter()
    {
      _filter.TagFilter.Clear();
      foreach (DataGridViewRow row in dataGridViewTagFilter.Rows)
      {
        TreeViewTagFilter tagfilter = new TreeViewTagFilter();

        // there's no SelectIndex property for DatagridCellComboBox
        // and i have also problems using a DataSource, so we need to loop
        if (row.Cells[0].Value != null)
        {
          tagfilter.Field = _filterFieldValues[TagFilterField.Items.IndexOf(row.Cells[0].Value.ToString())];
        }

        if (row.Cells[1].Value != null)
        {
          tagfilter.FilterValue = row.Cells[1].Value.ToString();
        }

        if (row.Cells[2].Value != null)
        {
          string selectedValue = row.Cells[2].Value.ToString();
          int i = 0;
          foreach (string item in TagFilterOperator.Items)
          {
            if (item == selectedValue)
            {
              tagfilter.FilterOperator = i == 0 ? "and" : "or";
              break;
            }
            i++;
          }
        }
        _filter.TagFilter.Add(tagfilter);
      }

      if (dataGridViewTagFilter.Rows.Count == 0)
      {
        ckUseTagFilter.Checked = false;
      }
    }

    /// <summary>
    ///   A Row has been deleted. Refreah Filter
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void dataGridViewTagFilter_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      RefreshFilter();
    }

    /// <summary>
    ///   A Row has been changed. Refreah Filter
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void dataGridViewTagFilter_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      RefreshFilter();
    }

    #region Filter Context Menu

    private void dataGridViewTagFilter_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        // Point where the mouse is clicked.
        Point p = new Point(e.X, e.Y);

        contextMenuStripFilter.Show(dataGridViewTagFilter, p);
      }
    }

    private void menuInsertFilter_Click(object sender, EventArgs e)
    {
      int curIndex = -1;
      if (dataGridViewTagFilter.CurrentRow == null)
      {
        curIndex = -1;
      }
      else
      {
        curIndex = dataGridViewTagFilter.CurrentRow.Index;
      }

      dataGridViewTagFilter.Rows.Insert(curIndex + 1, 1);
    }

    private void menuDeleteFilter_Click(object sender, EventArgs e)
    {
      int curIndex = dataGridViewTagFilter.CurrentRow.Index;
      if (curIndex > -1 && dataGridViewTagFilter.CurrentRow != null)
      {
        dataGridViewTagFilter.Rows.RemoveAt(curIndex);
      }
    }

    #endregion

    #endregion

    #endregion

    #region General Message Handling

    /// <summary>
    ///   Handle Messages
    /// </summary>
    /// <param name = "message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "languagechanged":
          {
            LocaliseScreen();
            Refresh();
            break;
          }

        case "themechanged":
          {
            dataGridViewTagFilter.BackgroundColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            treeViewFolderBrowser.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            treeViewFolderBrowser.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
            break;
          }
      }
    }

    #endregion
  }
}