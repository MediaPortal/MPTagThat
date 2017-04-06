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
using FreeImageAPI;
using Microsoft.VisualBasic.FileIO;
using MPTagThat.Core;
using MPTagThat.Core.ShellLib;

#endregion

namespace MPTagThat
{
  public partial class MiscInfoControl : UserControl
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly IThemeManager themeManager = ServiceScope.Get<IThemeManager>();
    private ImageList _imgList = new ImageList();

    private bool _inLabeledit;
    private string _savedImageSize = "";
    private string _savedFileName = "";

    private Rectangle _dragBoxFromMouseDown;
    private Point _screenOffset;

    // ListView messages
    private const int LVM_FIRST = 0x1000;
    private const int LVM_GETEDITCONTROL = (LVM_FIRST + 24);

    #endregion

    #region ctor

    public MiscInfoControl()
    {
      // Activates double buffering 
      this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();

      InitializeComponent();

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

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
      nonMusicMenuitems[0].Click += listViewNonMusicFiles_RenameToFolderJpg;
      nonMusicMenuitems[0].DefaultItem = true;
      nonMusicMenuitems[1] = new MenuItem();
      nonMusicMenuitems[1].Text = localisation.ToString("main", "NonMusicContextMenuDeleteFiles");
      nonMusicMenuitems[1].Click += listViewNonMusicFiles_DeleteFiles;
      listViewNonMusicFiles.ContextMenu = new ContextMenu(nonMusicMenuitems);
    }

    #endregion

    #region Localisation

    private void SetColorBase()
    {
      BackColor = themeManager.CurrentTheme.BackColor;
      // We want to have our own header color
      listViewNonMusicFiles.BackColor = themeManager.CurrentTheme.BackColor;
      listViewNonMusicFiles.ForeColor = themeManager.CurrentTheme.LabelForeColor;
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Clear the Non Music Files View
    /// </summary>
    public void ClearNonMusicFiles()
    {
      listViewNonMusicFiles.Items.Clear();
    }

    /// <summary>
    ///   Add Non Music Files to the List View
    /// </summary>
    /// <param name = "files"></param>
    public void AddNonMusicFiles(List<FileInfo> files)
    {
      listViewNonMusicFiles.Items.Clear();
      _imgList = new ImageList();
      _imgList.ImageSize = new Size(64, 64);

      int i = 0;

      log.Info("Creating Thumbs for selected folder(s). Found: {0} file(s)", files.Count);
      foreach (FileInfo fi in files)
      {
        log.Trace("Creatung thumb for: {0}", fi.FullName);

        // Create Image
        bool imgFailure = false;
        bool nonPicFile = true;
        Size originalImageSize = new Size(0,0);

        if (Util.IsPicture(fi.Name))
        {
          nonPicFile = false;
          try
          {
            Image img = GetImageFromFile(fi.FullName, out originalImageSize);
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
          string defaultName = "";
          if (fi.Extension.Length > 0)
          {
            try
            {
              defaultName = string.Format("Fileicons\\{0}.png", fi.Extension.Substring(1));
            }
            catch (Exception) {}
          }
          if (File.Exists(defaultName))
          {
            Image img = GetImageFromFile(defaultName, out originalImageSize);
            if (img != null)
            {
              _imgList.Images.Add(img);
            }
          }
          else
          {
            Image img = GetImageFromFile("Fileicons\\unknown.png", out originalImageSize);
            if (img != null)
            {
              _imgList.Images.Add(img);
            }
          }
        }

        string itemName = string.Format("{0}\n| {1}x{2} |", fi.Name, originalImageSize.Width, originalImageSize.Height);
        ListViewItem item = new ListViewItem(itemName);
        item.Tag = fi.FullName;
        item.ToolTipText = string.Format("{0} | {1} {2} | {3}x{4} | {5}kb", fi.FullName, fi.LastWriteTime.ToShortDateString(),
                                         fi.LastWriteTime.ToShortTimeString(), originalImageSize.Width, originalImageSize.Height, fi.Length / 1024);

        item.ImageIndex = i;
        listViewNonMusicFiles.Items.Add(item);
        i++;
      }
      log.Info("Finished creating folder thumbs.");
      listViewNonMusicFiles.LargeImageList = _imgList;
    }

    /// <summary>
    ///   Return an image from a given filename
    /// </summary>
    /// <param name = "fileName"></param>
    /// <param name = "size"></param>
    /// <returns></returns>
    private Image GetImageFromFile(string fileName, out Size size)
    {
      FreeImageBitmap img = null;
      size = new Size(0, 0);
      try
      {
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          img = new FreeImageBitmap(fs);
          fs.Close();
        }
        size = img.Size;

        // convert Image Size to 64 x 64 for display in the Imagelist
        img.Rescale(64, 64, FREE_IMAGE_FILTER.FILTER_BOX);
      }
      catch (Exception ex)
      {
        log.Error("File has invalid Picture: {0} {1}", fileName, ex.Message);
      }
      return img != null ? (Image)img : null;
    }

    /// <summary>
    ///   Activate the Non Music Files Tab
    /// </summary>
    public void ActivateNonMusicTab()
    {
      tabControlMisc.SelectFirstTab();
    }

    #endregion

    #region Events

    #region Non Music File Grid

    /// <summary>
    ///   Save the Old file Name and set indicator that we are in label edit,
    ///   so that the delete key can be used while editing.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void listViewNonMusicFiles_BeforeLabelEdit(object sender, LabelEditEventArgs e)
    {
      _inLabeledit = true;
      _savedFileName = (string)listViewNonMusicFiles.Items[e.Item].Tag;
      _savedImageSize = listViewNonMusicFiles.Items[e.Item].Text.Substring(listViewNonMusicFiles.Items[e.Item].Text.IndexOf("|"));

      // Get Edit Control Handle and Set the text
      IntPtr editCtrl = IntPtr.Zero;
      editCtrl = Util.SendMessage(listViewNonMusicFiles.Handle, LVM_GETEDITCONTROL, 0, IntPtr.Zero);
      Util.SetWindowText(editCtrl, _savedFileName);
    }
    
    /// <summary>
    ///   Rename the file, if the name has changed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void listViewNonMusicFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      _inLabeledit = false;

      if (e.Label == null)
      {
        return;
      }

      try
      {
        FileSystem.MoveFile(_savedFileName, e.Label, UIOption.AllDialogs, UICancelOption.DoNothing);
        listViewNonMusicFiles.Items[e.Item].Tag = e.Label;
        
        // We have stored the changed label in the Tag and are now setting the alternate title
        e.CancelEdit = true; 
        listViewNonMusicFiles.Items[e.Item].Text = string.Format("{0}\n{1}", Path.GetFileName(e.Label), _savedImageSize);

        FileInfo fi = new FileInfo((string)listViewNonMusicFiles.Items[e.Item].Tag);
        listViewNonMusicFiles.Items[e.Item].ToolTipText = string.Format("{0} | {1} {2} {3} {4}kb", fi.FullName, fi.LastWriteTime.ToShortDateString(),
                                         fi.LastWriteTime.ToShortTimeString(), _savedImageSize, fi.Length / 1024);

      }
      catch (Exception ex)
      {
        e.CancelEdit = true;
        listViewNonMusicFiles.Items[e.Item].Text = (string)listViewNonMusicFiles.Items[e.Item].Tag;
        log.Error("Error renaming file: {0} to {1} Exception: {2}", _savedFileName, e.Label, ex.Message);
      }
    }

    /// <summary>
    ///   Capture the delete key in the list view.
    ///   Will delete the selected item
    /// </summary>
    /// <param name = "msg"></param>
    /// <param name = "keyData"></param>
    /// <returns></returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == Keys.Delete)
      {
        if (listViewNonMusicFiles.SelectedItems.Count > 0 && !_inLabeledit)
        {
          UIOption dialogOption = UIOption.AllDialogs;
          if (listViewNonMusicFiles.SelectedItems.Count > 1)
          {
            // Don't display the "do you want to delete..." for multiple files
            dialogOption = UIOption.OnlyErrorDialogs;
            string dialogMsg = string.Format(localisation.ToString("main", "NonMusicDeleteFilesMessage"),
                                             listViewNonMusicFiles.SelectedItems.Count);
            if (
              MessageBox.Show(dialogMsg, localisation.ToString("main", "NonMusicDeleteFilesHeader"),
                              MessageBoxButtons.YesNo) == DialogResult.No)
            {
              return false;
            }
          }
          foreach (ListViewItem item in listViewNonMusicFiles.SelectedItems)
          {
            try
            {
              FileSystem.DeleteFile((string)item.Tag, dialogOption, RecycleOption.SendToRecycleBin,
                                    UICancelOption.ThrowException);
              listViewNonMusicFiles.Items[item.Index].Remove();
            }
            catch (OperationCanceledException) // User pressed No on delete. Do nothing
            {}
            catch (Exception ex)
            {
              log.Error("Error deleting file: {0} Exception: {1}", (string)item.Tag, ex.Message);
            }
          }
        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    ///   A double click occured´in the list view
    ///   If an item was selected, open it with the default program
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void listViewNonMusicFiles_DoubleClick(object sender, EventArgs e)
    {
      ListViewHitTestInfo hitInfo = listViewNonMusicFiles.HitTest(listViewNonMusicFiles.PointToClient(MousePosition));
      if (listViewNonMusicFiles.SelectedItems.Count > 0)
      {
        ListViewItem item = listViewNonMusicFiles.SelectedItems[0];
        if (item == hitInfo.Item)
        {
          ShellExecute shell = new ShellExecute();
          shell.Path = (string)item.Tag;
          shell.Execute();
        }
      }
    }

    /// <summary>
    ///   Rename Context Menu entry has been selected
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    private void listViewNonMusicFiles_RenameToFolderJpg(object o, EventArgs e)
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

      if (item == null)
      {
        return;
      }

      string fileName = Path.GetFileName((string) item.Tag);
      if (fileName == "folder.jpg")
      {
        return;
      }

      
      string path = Path.GetDirectoryName((string)item.Tag);
      string newName = Path.Combine(path, "folder.jpg");
      string savedImageSize = item.Text.Substring(item.Text.IndexOf("|"));

      try
      {
        FileSystem.MoveFile((string)item.Tag, newName, UIOption.OnlyErrorDialogs, UICancelOption.DoNothing);
        item.Tag = newName;

        // Set Text and fileInfo for renamed Item
        item.Text = string.Format("{0}\n{1}", Path.GetFileName(newName), savedImageSize);
        FileInfo fi = new FileInfo(newName);
        item.ToolTipText = string.Format("{0} | {1} {2} {3} {4}kb", fi.FullName, fi.LastWriteTime.ToShortDateString(),
                                         fi.LastWriteTime.ToShortTimeString(), savedImageSize, fi.Length / 1024);
      }
      catch (Exception ex)
      {
        log.Error("Error renaming file: {0} to {1} Exception: {2}", item.Text, newName, ex.Message);
      }
    }

    /// <summary>
    ///   Deelete Context Menu entry has been selected
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    private void listViewNonMusicFiles_DeleteFiles(object o, EventArgs e)
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
    ///   Handle Messages
    /// </summary>
    /// <param name = "message"></param>
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
            Refresh();
            break;
          }
      }
    }

    #endregion

    private void listViewNonMusicFiles_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left && listViewNonMusicFiles.SelectedItems.Count > 0)
      {
        // Remember the point where the mouse down occurred. The DragSize indicates
        // the size that the mouse can move before a drag event should be started.                
        Size dragSize = SystemInformation.DragSize;

        // Create a rectangle using the DragSize, with the mouse position being
        // at the center of the rectangle.
        _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)),
                                              dragSize);
      }
      else
        _dragBoxFromMouseDown = Rectangle.Empty;
    }

    private void listViewNonMusicFiles_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        // If the mouse moves outside the rectangle, start the drag.
        if (_dragBoxFromMouseDown != Rectangle.Empty &&
            !_dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          // The screenOffset is used to account for any desktop bands 
          // that may be at the top or left side of the screen when 
          // determining when to cancel the drag drop operation.
          _screenOffset = SystemInformation.WorkingArea.Location;

          string fileName = (string)listViewNonMusicFiles.SelectedItems[0].Tag;
          listViewNonMusicFiles.DoDragDrop(fileName, DragDropEffects.Copy | DragDropEffects.Move);
        }
      }
    }

    private void listViewNonMusicFiles_MouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      _dragBoxFromMouseDown = Rectangle.Empty;
    }
  }
}
