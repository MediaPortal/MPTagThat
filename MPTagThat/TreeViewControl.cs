using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
      treeViewFolderBrowser.DataSource = new TreeViewFolderBrowserDataProvider();
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
        _main.ClearFileInfoPanel();

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
    }
    #endregion

    #endregion

    #region Events
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
        _main.CurrentDirectory = node.Path;
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
          contextMenuTreeView.Show(treeViewFolderBrowser, p);
        }
      }
    }

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
    /// Refresh the Folder List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRefreshFolder_Click(object sender, EventArgs e)
    {
      RefreshFolders();
    }
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
