using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using MPTagThat.Core;
using MPTagThat.Core.ShellLib;

namespace MPTagThat
{
  public partial class MiscInfoControl : UserControl
  {
    #region Variables

    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private IThemeManager themeManager = ServiceScope.Get<IThemeManager>();
    private ILogger log = ServiceScope.Get<ILogger>();

    private bool _inLabeledit = false;
    private string _savedLabelValue = "";

    private ImageList _imgList = new ImageList();

    #endregion

    #region ctor

    public MiscInfoControl()
    {
      InitializeComponent();

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      LocaliseScreen();
      SetColorBase();

      // Build the Context Menu for the Error Grid
      MenuItem[] rmitems = new MenuItem[1];
      rmitems[0] = new MenuItem();
      rmitems[0].Text = localisation.ToString("main", "ErrorContextMenuClear");
      rmitems[0].Click += new System.EventHandler(dataGridViewError_ClearList);
      rmitems[0].DefaultItem = true;
      this.dataGridViewError.ContextMenu = new ContextMenu(rmitems);

      // Setup Non-Music ListView
      listViewNonMusicFiles.View = View.LargeIcon;
      listViewNonMusicFiles.ShowItemToolTips = true;
      listViewNonMusicFiles.LabelEdit = true;
      listViewNonMusicFiles.FullRowSelect = true;
      listViewNonMusicFiles.GridLines = true;
      listViewNonMusicFiles.Sorting = SortOrder.Ascending;

      // Build the Context Menu for the Non Music Files Listview
      MenuItem[] nonMusicMenuitems = new MenuItem[2];
      nonMusicMenuitems[0] = new MenuItem();
      nonMusicMenuitems[0].Text = localisation.ToString("main", "NonMusicContextMenuRenameToFolderJpg");
      nonMusicMenuitems[0].Click += new System.EventHandler(listViewNonMusicFiles_RenameToFolderJpg);
      nonMusicMenuitems[0].DefaultItem = true;
      nonMusicMenuitems[1] = new MenuItem();
      nonMusicMenuitems[1].Text = localisation.ToString("main", "NonMusicContextMenuDeleteFiles");
      nonMusicMenuitems[1].Click += new System.EventHandler(listViewNonMusicFiles_DeleteFiles);
      listViewNonMusicFiles.ContextMenu = new ContextMenu(nonMusicMenuitems);
    }

    #endregion

    #region Localisation

    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      this.dataGridViewError.Columns[0].HeaderText = localisation.ToString("main", "ErrorHeaderFile");
      this.dataGridViewError.Columns[1].HeaderText = localisation.ToString("main", "ErrorHeaderMessage");
    }

    private void SetColorBase()
    {
      this.BackColor = themeManager.CurrentTheme.BackColor;
      // We want to have our own header color
      dataGridViewError.EnableHeadersVisualStyles = false;
      dataGridViewError.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      dataGridViewError.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Returns the Error Gridview
    /// </summary>
    public DataGridView ErrorGridView
    {
      get { return this.dataGridViewError; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Clear the Non Music Files View
    /// </summary>
    public void ClearNonMusicFiles()
    {
      listViewNonMusicFiles.Items.Clear();
    }

    /// <summary>
    /// Add Non Music Files to the List View
    /// </summary>
    /// <param name="files"></param>
    public void AddNonMusicFiles(List<FileInfo> files)
    {
      listViewNonMusicFiles.Items.Clear();
      _imgList = new ImageList();

      int i = 0;
      foreach (FileInfo fi in files)
      {
        ListViewItem item = new ListViewItem(fi.FullName);
        item.ToolTipText = string.Format("{0} | {1} {2} | {3}kb", fi.Name, fi.LastWriteTime.ToShortDateString(),
                                         fi.LastWriteTime.ToShortTimeString(), fi.Length/1024);

        // Create Image
        bool imgFailure = false;
        bool nonPicFile = true;
        if (Util.IsPicture(fi.Name))
        {
          nonPicFile = false;
          try
          {
            Image img = GetImageFromFile(fi.FullName);
            if (img != null)
            {
              _imgList.Images.Add(img);
            }
            else
            {
              imgFailure = true;
            }
          }
          catch (Exception)
          {
            imgFailure = true;
          }
        }
        if (nonPicFile || imgFailure)
        {
          // For a non Picture file or if we had troubles creating the thumb, see if we have a file specific icon
          string defaultName = string.Format("Fileicons\\{0}.png", fi.Extension.Substring(1));
          if (File.Exists(defaultName))
          {
            Image img = GetImageFromFile(defaultName);
            if (img != null)
            {
              _imgList.Images.Add(img);
            }
          }
        }

        item.ImageIndex = i;
        listViewNonMusicFiles.Items.Add(item);
        i++;
      }
      _imgList.ImageSize = new Size(64, 64);
      listViewNonMusicFiles.LargeImageList = _imgList;
    }

    /// <summary>
    /// Return an image from a given filename
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private Image GetImageFromFile(string fileName)
    {
      FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
      Image img = Image.FromStream(fs);
      fs.Close();
      return img;
    }

    /// <summary>
    /// Activate the Error Tab
    /// </summary>
    public void ActivateErrorTab()
    {
      tabControlMisc.SelectedIndex = 0;
    }

    /// <summary>
    /// Activate the Non Music Files Tab
    /// </summary>
    public void ActivateNonMusicTab()
    {
      tabControlMisc.SelectedIndex = 1;
    }

    #endregion

    #region Events

    #region Error Grid

    /// <summary>
    /// Handle Right Mouse Click to open the context Menu in the Error DataGrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void datagridViewError_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        this.dataGridViewError.ContextMenu.Show(dataGridViewError, new Point(e.X, e.Y));
    }

    /// <summary>
    /// Context Menu entry has been selected
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void dataGridViewError_ClearList(object o, System.EventArgs e)
    {
      dataGridViewError.Rows.Clear();
    }

    #endregion

    #region Non Music File Grid

    /// <summary>
    /// Save the Old file Name and set indicator that we are in label edit,
    /// so that the delete key can be used while editing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listViewNonMusicFiles_BeforeLabelEdit(object sender, LabelEditEventArgs e)
    {
      _inLabeledit = true;
      _savedLabelValue = listViewNonMusicFiles.Items[e.Item].Text;
    }

    /// <summary>
    /// Rename the file, if the name has changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listViewNonMusicFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      _inLabeledit = false;
      if (_savedLabelValue != e.Label)
      {
        try
        {
          FileSystem.MoveFile(_savedLabelValue, e.Label, UIOption.AllDialogs, UICancelOption.DoNothing);
        }
        catch (Exception ex)
        {
          listViewNonMusicFiles.Items[e.Item].Text = _savedLabelValue;
          log.Error("Error renaming file: {0} to {1} Exception: {2}", _savedLabelValue, e.Label, ex.Message);
        }
      }
    }

    /// <summary>
    /// Capture the delete key in the list view.
    /// Will delete the selected item
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == Keys.Delete)
      {
        if (tabControlMisc.SelectedIndex == 1 && listViewNonMusicFiles.SelectedItems.Count > 0 && !_inLabeledit)
        {
          UIOption dialogOption = UIOption.AllDialogs;
          if (listViewNonMusicFiles.SelectedItems.Count > 1)
          {
            // Don't display the "do you want to delete..." for multiple files
            dialogOption = UIOption.OnlyErrorDialogs;
            string dialogMsg = string.Format(localisation.ToString("main", "NonMusicDeleteFilesMessage"),
                                       listViewNonMusicFiles.SelectedItems.Count);
            if (MessageBox.Show(dialogMsg, localisation.ToString("main", "NonMusicDeleteFilesHeader"), MessageBoxButtons.YesNo) == DialogResult.No)
            {
              return false;
            }
          }
          foreach (ListViewItem item in listViewNonMusicFiles.SelectedItems)
          {
            try
            {
              FileSystem.DeleteFile(item.Text, dialogOption, RecycleOption.SendToRecycleBin,
                                    UICancelOption.ThrowException);
              listViewNonMusicFiles.Items[item.Index].Remove();
            }
            catch (System.OperationCanceledException) // User pressed No on delete. Do nothing
            { }
            catch (Exception ex)
            {
              log.Error("Error deleting file: {0} Exception: {1}", item.Text, ex.Message);
            }
          }
        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// A double click occured´in the list view
    /// If an item was selected, open it with the default program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listViewNonMusicFiles_DoubleClick(object sender, EventArgs e)
    {
      ListViewHitTestInfo hitInfo = listViewNonMusicFiles.HitTest(listViewNonMusicFiles.PointToClient(MousePosition));
      if (listViewNonMusicFiles.SelectedItems.Count > 0)
      {
        ListViewItem item = listViewNonMusicFiles.SelectedItems[0];
        if (item == hitInfo.Item)
        {
          ShellExecute shell = new ShellExecute();
          shell.Path = item.Text;
          shell.Execute();
        }
      }
    }

    /// <summary>
    /// Rename Context Menu entry has been selected
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void listViewNonMusicFiles_RenameToFolderJpg(object o, System.EventArgs e)
    {
      ListViewItem item = null;
      if (listViewNonMusicFiles.SelectedItems.Count > 0)
      {
        item = listViewNonMusicFiles.SelectedItems[0];
      }
      else if (listViewNonMusicFiles.Items.Count > 0)
      {
        item = listViewNonMusicFiles.Items[0];
      }

      if (item != null)
      {
        string path = Path.GetDirectoryName(item.Text);
        string newName = Path.Combine(path, "folder.jpg");
        try
        {
          FileSystem.MoveFile(item.Text, newName, UIOption.OnlyErrorDialogs, UICancelOption.DoNothing);
          item.Text = newName;
        }
        catch (Exception ex)
        {
          log.Error("Error renaming file: {0} to {1} Exception: {2}", item.Text, newName, ex.Message);
        }
      }
    }

    /// <summary>
    /// Deelete Context Menu entry has been selected
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void listViewNonMusicFiles_DeleteFiles(object o, System.EventArgs e)
    {
      // Invoke the Process Key method
      Keys key = Keys.Delete;
      Message msg = new Message();
      ProcessCmdKey(ref msg, key);
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
        case "themechanged":
          {
            SetColorBase();
            break;
          }

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