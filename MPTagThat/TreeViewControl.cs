using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

using Raccoom.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat
{
  public partial class TreeViewControl : UserControl
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    private TreeNode _previousHoverNode = null;
    private DateTime _savedTime;
    private bool _actionCopy = false;
    private TreeNodePath _nodeToCopyCut = null;
    private bool _databaseMode = false;
    #endregion

    #region Properties
    public TreeViewFolderBrowser TreeView
    {
      get { return this.treeViewFolderBrowser; }
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
    #endregion

    #region ctor
    public TreeViewControl(Main main)
    {
      _main = main;

      InitializeComponent();

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      LocaliseScreen();
    }
    #endregion

    #region Public Methods
    #region Init
    public void Init()
    {
      if (Options.MainSettings.DatabaseMode && File.Exists(Options.MainSettings.MediaPortalDatabase))
      {
        treeViewFolderBrowser.DataSource = new TreeViewFolderBrowserDataProviderMediaPortal();
        _databaseMode = true;
      }
      else
      {
        treeViewFolderBrowser.DataSource = new TreeViewFolderBrowserDataProvider();
        _databaseMode = false;
      }
      SwitchMode();
      treeViewFolderBrowser.DriveTypes = DriveTypes.LocalDisk | DriveTypes.NetworkDrive | DriveTypes.RemovableDisk | DriveTypes.CompactDisc;
      treeViewFolderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
      treeViewFolderBrowser.CheckboxBehaviorMode = CheckboxBehaviorMode.None;
    }

    /// <summary>
    /// Refreshes the Foldrs
    /// </summary>
    public void RefreshFolders()
    {
      treeViewFolderBrowser.Populate();
      treeViewFolderBrowser.ShowFolder(_main.CurrentDirectory);
    }

    public void DeleteFolder()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      TreeNodePath node = treeViewFolderBrowser.SelectedNode as TreeNodePath;
      if (node != null)
      {
        FileSystem.DeleteDirectory(node.Path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);

        // Clear the tracks
        _main.TracksGridView.TrackList.Clear();
        _main.FileInfoPanel.ClearFileInfoPanel();

        // Now set the Selected directory to the Parent of the delted folder and reread the view
        TreeNodePath parent = node.Parent as TreeNodePath;
        _main.CurrentDirectory = parent.Path;
        treeViewFolderBrowser.Populate();
        treeViewFolderBrowser.ShowFolder(_main.CurrentDirectory);
      }
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    #endregion
    #endregion

    #region Private Methids
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      // Extended Panels. Doing it via TTExtendedPanel doesn't work for some reason
      this.treeViewPanel.CaptionText = localisation.ToString("main", "TreeViewPanel");
      this.optionsPanelLeft.CaptionText = localisation.ToString("main", "OptionsPanel");
      this.contextMenuTreeView.Items[0].Text = localisation.ToString("contextmenu", "Copy");
      this.contextMenuTreeView.Items[1].Text = localisation.ToString("contextmenu", "Cut");
      this.contextMenuTreeView.Items[2].Text = localisation.ToString("contextmenu", "Paste");
      this.contextMenuTreeView.Items[3].Text = localisation.ToString("contextmenu", "Delete");
      this.contextMenuTreeView.Items[4].Text = localisation.ToString("contextmenu", "Refresh");
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
        _previousHoverNode.BackColor = Color.White;
      }
      _previousHoverNode = node;

    }

    private void SwitchMode()
    {
      _main.TracksGridView.TrackList.Clear();
      _main.FileInfoPanel.ClearFileInfoPanel();
      if (_databaseMode)
      {
        if (_main.SplitterTop.IsCollapsed)
        {
          _main.SplitterTop.ToggleState();
        }
        treeViewFolderBrowser.AllowDrop = false;
        checkBoxRecursive.Enabled = false;
        btnRefreshFolder.Enabled = false;
      }
      else
      {
        if (!_main.SplitterTop.IsCollapsed)
        {
          _main.SplitterTop.ToggleState();
        }
        treeViewFolderBrowser.AllowDrop = true;
        checkBoxRecursive.Enabled = true;
        btnRefreshFolder.Enabled = true;
      }
    }
    #endregion

    #region Events
    #region Treeview
    /// <summary>
    /// The Treeview Control is the active control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeViewFolderBrowser_Enter(object sender, EventArgs e)
    {
      _main.TreeViewSelected = true;
    }

    /// <summary>
    /// The Treeview Control is no longer the active Control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeViewFolderBrowser_Leave(object sender, EventArgs e)
    {
      _main.TreeViewSelected = false;
    }

    /// <summary>
    /// A new Folder has been selected
    /// Only allow navigation, if no folder scanning is active
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeViewFolderBrowser_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      if (_main.FolderScanning)
        e.Cancel = true;
    }

    /// <summary>
    /// A Folder has been selected in the TreeView. Read the content
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeViewFolderBrowser_AfterSelect(object sender, TreeViewEventArgs e)
    {
      Util.EnterMethod(Util.GetCallingMethod());
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
        if (node.Text.Contains("CD-ROM Disc ("))
        {
          _main.MainRibbon.MainRibbon.RibbonBarElement.TabStripElement.SelectedTab = _main.MainRibbon.TabRip;
          _main.BurnGridView.Hide();
          _main.RipGridView.Show();
          _main.TracksGridView.Hide();
          if (!_main.SplitterRight.IsCollapsed)
          {
            _main.SplitterRight.ToggleState();
          }
          _main.RipGridView.SelectedCDRomDrive = node.Text.Substring("CD-ROM Disc (".Length, 1);
        }
        else
        {
          // If we selected a folder, while being in the Burn or Rip View, go to the TagTab
          if (_main.MainRibbon.TabBurn.IsSelected || _main.MainRibbon.TabRip.IsSelected || _main.MainRibbon.TabConvert.IsSelected)
          {
            _main.MainRibbon.MainRibbon.RibbonBarElement.TabStripElement.SelectedTab = _main.MainRibbon.TabTag;
            _main.BurnGridView.Hide();
            _main.RipGridView.Hide();
            _main.TracksGridView.Show();
            if (_main.SplitterRight.IsCollapsed && !Options.MainSettings.RightPanelCollapsed)
              _main.SplitterRight.ToggleState();
          }
          _main.RefreshTrackList();
        }
      }
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// The User selected the Treeview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeViewFolderBrowser_Click(object sender, EventArgs e)
    {
      _main.TreeViewSelected = true;
    }

    /// <summary>
    /// Show the Treview Context Menu on Right Mouse Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// The user hoevrs with the mouse over a node. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeViewFolderBrowser_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
    {
      SetNodeHoverColor(e.Node);
    }

    /// <summary>
    /// Gets the treenode, where the cursor currently points to
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private TreeNode GetNode(Point point)
    {
      // We need to tranlate the coordinates to the position within the Main form
      Point pt = this.Parent.Parent.PointToClient(this.Parent.PointToScreen(this.PointToScreen(treeViewFolderBrowser.Location)));
      TreeViewHitTestInfo hitTestInfo = treeViewFolderBrowser.HitTest(point.X - pt.X, point.Y - pt.Y - 20);
      return hitTestInfo.Node;
    }
    #endregion

    #region Drag & Drop
    /// <summary>
    /// Some files are dragged over the Treeview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeViewFolderBrowser_DragOver(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(typeof(List<TrackData>)))
      {
        return;
      }

      if (e.KeyState == 9 && (e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy) // The Ctrl Key + LMB was pressed
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
    /// The drag and drop is completed. do the actual move / copy
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        TreeNodePath node = GetNode(new Point(e.X, e.Y)) as TreeNodePath; ;
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
          string targetFile = System.IO.Path.Combine(node.Path, track.FileName);
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
    /// Refresh Button on Treeview Context Menu has been clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void contextMenuTreeViewRefresh_Click(object sender, EventArgs e)
    {
      RefreshFolders();
    }

    /// <summary>
    /// Delete Button on Treeview Context Menu has been clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void contextMenuTreeViewDelete_Click(object sender, EventArgs e)
    {
      DeleteFolder();
    }

    /// <summary>
    /// Copy Button has been selected in Context Menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void contextMenuTreeViewCopy_Click(object sender, EventArgs e)
    {
      contextMenuTreeView.Items[2].Enabled = true;  // Enable the Paste Menu item
      _actionCopy = true;
      _nodeToCopyCut = treeViewFolderBrowser.SelectedNode as TreeNodePath;
    }

    /// <summary>
    /// Cut Button has been selected in Context Menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void contextMenuTreeViewCut_Click(object sender, EventArgs e)
    {
      contextMenuTreeView.Items[2].Enabled = true;  // Enable the Paste Menu item
      _actionCopy = false;  // we do a move
      _nodeToCopyCut = treeViewFolderBrowser.SelectedNode as TreeNodePath;
    }

    /// <summary>
    /// Paste Button has been selected in Context Menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void contextMenuTreeViewPaste_Click(object sender, EventArgs e)
    {
      contextMenuTreeView.Items[2].Enabled = false;  // Disable the Paste Menu item
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

    #region Buttons
    /// <summary>
    /// Refresh the Folder List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRefreshFolder_Click(object sender, EventArgs e)
    {
      RefreshFolders();
    }


    private void btnSwitchView_Click(object sender, EventArgs e)
    {
      if (treeViewFolderBrowser.DataSource.GetType() != typeof(TreeViewFolderBrowserDataProvider))
      {
        treeViewFolderBrowser.DataSource = new TreeViewFolderBrowserDataProvider();
        _databaseMode = false;
      }
      else
      {
        if (File.Exists(Options.MainSettings.MediaPortalDatabase))
        {
          treeViewFolderBrowser.DataSource = new TreeViewFolderBrowserDataProviderMediaPortal();
          _databaseMode = true;
        }
      }
      SwitchMode();
      treeViewFolderBrowser.Populate();
      treeViewFolderBrowser.Nodes[0].Expand();
    }
    #endregion
    #endregion

    #region General Message Handling
    /// <summary>
    /// Handle Messages
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "languagechanged":
          {
            LocaliseScreen();
            this.Refresh();
            break;
          }
      }
    }
    #endregion
  }
}
