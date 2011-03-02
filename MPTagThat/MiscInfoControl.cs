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
    private string _savedLabelValue = "";

    #endregion

    #region ctor

    public MiscInfoControl()
    {
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

      int i = 0;
      foreach (FileInfo fi in files)
      {
        ListViewItem item = new ListViewItem(fi.FullName);
        item.ToolTipText = string.Format("{0} | {1} {2} | {3}kb", fi.Name, fi.LastWriteTime.ToShortDateString(),
                                         fi.LastWriteTime.ToShortTimeString(), fi.Length / 1024);

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
    ///   Return an image from a given filename
    /// </summary>
    /// <param name = "fileName"></param>
    /// <returns></returns>
    private Image GetImageFromFile(string fileName)
    {
      FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
      Image img = Image.FromStream(fs);
      fs.Close();
      return img;
    }

    /// <summary>
    ///   Activate the Non Music Files Tab
    /// </summary>
    public void ActivateNonMusicTab()
    {
      tabControlMisc.SelectedIndex = 1;
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
      _savedLabelValue = listViewNonMusicFiles.Items[e.Item].Text;
    }

    /// <summary>
    ///   Rename the file, if the name has changed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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
        if (tabControlMisc.SelectedIndex == 1 && listViewNonMusicFiles.SelectedItems.Count > 0 && !_inLabeledit)
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
              FileSystem.DeleteFile(item.Text, dialogOption, RecycleOption.SendToRecycleBin,
                                    UICancelOption.ThrowException);
              listViewNonMusicFiles.Items[item.Index].Remove();
            }
            catch (OperationCanceledException) // User pressed No on delete. Do nothing
            {}
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
          shell.Path = item.Text;
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
  }
}