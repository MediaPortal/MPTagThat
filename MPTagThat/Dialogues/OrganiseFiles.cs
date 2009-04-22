using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

using MPTagThat.Core;
using MPTagThat.Dialogues;

namespace MPTagThat.Organise
{
  public partial class OrganiseFiles : ShapedForm
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    private Dictionary<string, string> _directories = null;
    private bool _isPreviewOpen = false;
    private Preview _previewForm = null;
    private TrackData track = null;
    private TrackDataPreview trackPreview = null;
    #endregion

    #region ctor
    public OrganiseFiles(Main main)
    {
      this._main = main;
      InitializeComponent();

      LoadSettings();

      LocaliseScreen();

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
      this.labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;
    }
    #endregion

    #region Methods
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      this.labelHeader.Text = localisation.ToString("organise", "Heading");
    }
    #endregion

    #region Settings
    private void LoadSettings()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      ckCopyFiles.Checked = Options.OrganiseSettings.CopyFiles;
      ckOverwriteFiles.Checked = Options.OrganiseSettings.OverWriteFiles;
      ckCopyNonMusicFiles.Checked = Options.OrganiseSettings.CopyNonMusicFiles;

      foreach (string item in Options.OrganiseSettings.LastUsedFolders)
      {
        cbRootDir.Items.Add(new Item(item, item, ""));
      }

      if (Options.OrganiseSettings.LastUsedFolders.Count == 0)
      {
        string musicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        Options.OrganiseSettings.LastUsedFolders.Insert(0, musicFolder);
        cbRootDir.Items.Add(new Item(musicFolder, musicFolder, ""));
        cbRootDir.SelectedIndex = 0;
      }
      else
      {
        if (Options.OrganiseSettings.LastUsedFolderIndex == -1)
        {
          cbRootDir.SelectedIndex = 0;
        }
        else
        {
          cbRootDir.SelectedIndex = Options.OrganiseSettings.LastUsedFolderIndex;
        }
      }

      foreach (string item in Options.OrganiseSettings.FormatValues)
      {
        cbFormat.Items.Add(new Item(item, item, ""));
      }

      if (Options.OrganiseSettings.LastUsedFormat > cbFormat.Items.Count - 1)
        cbFormat.SelectedIndex = -1;
      else
        cbFormat.SelectedIndex = Options.OrganiseSettings.LastUsedFormat;

      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Organise
    /// <summary>
    /// Organise Files
    /// </summary>
    /// <param name="parameters"></param>
    private void Organise(string parameter)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      DataGridView tracksGrid = _main.TracksGridView.View;
      _directories = new Dictionary<string, string>();

      // First do an automatic save of all pending changes
      _main.TracksGridView.SaveAll(false);

      bool bError = false;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        track = _main.TracksGridView.TrackList[row.Index];
        string directoryName = Util.ReplaceParametersWithTrackValues(parameter, track);
        directoryName = System.IO.Path.Combine(cbRootDir.Text, directoryName);

        try
        {
          // Now check the validity of the directory
          if (!System.IO.Directory.Exists(directoryName))
          {
            try
            {
              System.IO.Directory.CreateDirectory(directoryName);
            }
            catch (Exception e1)
            {
              bError = true;
              log.Debug("Error creating folder: {0} {1]", directoryName, e1.Message);
              row.Cells[0].Value = localisation.ToString("message", "Error");
              _main.TracksGridView.AddErrorMessage(track.File.Name, String.Format("{0}: {1} {2}", localisation.ToString("message", "Error"), directoryName, e1.Message));
              continue; // Process next row
            }
          }

          // Store the directory of the current file in the dictionary, so that we may copy later all non-music files
          string dir = System.IO.Path.GetDirectoryName(track.FullFileName);
          if (!_directories.ContainsKey(dir))
          {
            _directories.Add(dir, directoryName);
          }

          string newFilename = System.IO.Path.Combine(directoryName, track.FileName);
          try
          {
            if (!ckOverwriteFiles.Checked)
            {
              if (System.IO.File.Exists(newFilename))
              {
                bError = true;
                log.Debug("File exists: {0}", newFilename);
                row.Cells[0].Value = localisation.ToString("organise", "Exists");
                _main.TracksGridView.AddErrorMessage(newFilename, localisation.ToString("organise", "FileExists"));
                continue;
              }
            }

            // If new file name validates to be the same as the old file, i.e. it goes into the source folder
            // then continue, as this would lead in the source file to be deleted first and then there's nothing, which could be copied
            if (newFilename.ToLowerInvariant() == track.FullFileName.ToLowerInvariant())
            {
              bError = true;
              log.Debug("Old File and New File same: {0}", newFilename);
              row.Cells[0].Value = localisation.ToString("organise", "Exists");
              _main.TracksGridView.AddErrorMessage(newFilename, localisation.ToString("organise", "SameFile"));
              continue;
            }

            if (ckCopyFiles.Checked)
            {
              FileSystem.CopyFile(track.FullFileName, newFilename, ckOverwriteFiles.Checked);
              row.Cells[0].Value = localisation.ToString("organise", "Copied");
            }
            else
            {
              FileSystem.MoveFile(track.FullFileName, newFilename, ckOverwriteFiles.Checked);
              row.Cells[0].Value = localisation.ToString("organise", "Moved");
            }
          }
          catch (Exception e2)
          {
            bError = true;
            log.Error("Error Copy/Move File: {0} {1}", track.FullFileName, e2.Message);
            row.Cells[0].Value = localisation.ToString("mesage", "Error");
            _main.TracksGridView.AddErrorMessage(track.File.Name, String.Format("{0}: {1}", localisation.ToString("message", "Error"), e2.Message));
          }
        }
        catch (Exception ex)
        {
          bError = true;
          log.Error("Error Organising Files: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[0].Value = localisation.ToString("message", "Error");
          _main.TracksGridView.AddErrorMessage(_main.TracksGridView.TrackList[row.Index].File.Name, String.Format("{0}: {1} {2}", localisation.ToString("message", "Error"), directoryName, ex.Message));
        }
      }

      // Now that we have Moved/Copied the individual Files, we will copy / move the Pictures, etc.
      if (ckCopyNonMusicFiles.Checked)
      {
        foreach (string dir in _directories.Keys)
        {
          string[] files = System.IO.Directory.GetFiles(dir);
          foreach (string file in files)
          {
            // ignore audio files, we've processed them before
            if (Util.IsAudio(file))
              continue;

            string newFilename = System.IO.Path.Combine(_directories[dir], System.IO.Path.GetFileName(file));
            try
            {
              if (!ckOverwriteFiles.Checked)
              {
                if (System.IO.File.Exists(newFilename))
                {
                  log.Debug("File exists: {0}", newFilename);
                  continue;
                }
              }

              if (ckCopyFiles.Checked)
              {
                FileSystem.CopyFile(file, newFilename, UIOption.AllDialogs, UICancelOption.DoNothing);
              }
              else
              {
                FileSystem.MoveFile(file, newFilename, UIOption.AllDialogs, UICancelOption.DoNothing);
              }
            }
            catch (Exception ex)
            {
              log.Error("Error Copy/Move File: {0} {1}", file, ex.Message);
            }
          }
        }
      }

      // Delete empty folders,if we didn't get any error
      if (!ckCopyFiles.Checked && !bError)
      {
        foreach (string dir in _directories.Keys)
        {
          DeleteSubFolders(dir);
          DeleteParentFolders(dir);
        }

        string currentSelectedFolder = _main.CurrentDirectory;
        // Go up 1 level in the directory structure to find an existing folder
        int i = 0;
        while (i < 10)
        {
          if (System.IO.Directory.Exists(currentSelectedFolder))
            break;

          currentSelectedFolder = currentSelectedFolder.Substring(0, currentSelectedFolder.LastIndexOf("\\"));
          i++; // Max of 10 levels, to avoid possible infinity loop
        }
        _main.CurrentDirectory = currentSelectedFolder;
        _main.TreeView.RefreshFolders();
        _main.RefreshTrackList();
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    private void DeleteSubFolders(string folder)
    {
      string[] subFolders = System.IO.Directory.GetDirectories(folder);
      for (int i = 0; i < subFolders.Length; ++i)
      {
        DeleteSubFolders(subFolders[i]);
      }
      string[] files = System.IO.Directory.GetFiles(folder);
      string[] subDirs = System.IO.Directory.GetDirectories(folder);
      // Do we still have files or folders then skip the delete
      if (files.Length == 0 && subDirs.Length == 0)
        DeleteFolder(folder);
    }

    private void DeleteParentFolders(string folder)
    {
      int lastSlash = folder.LastIndexOf("\\");

      string parentFolder = folder;
      while (lastSlash > -1)
      {
        parentFolder = parentFolder.Substring(0, lastSlash);
        string[] files = System.IO.Directory.GetFiles(parentFolder);
        string[] subDirs = System.IO.Directory.GetDirectories(parentFolder);
        // Do we still have files or folders then skip the delete
        if (files.Length == 0 && subDirs.Length == 0)
          DeleteFolder(parentFolder);

        lastSlash = parentFolder.LastIndexOf("\\");
      }

    }

    private void DeleteFolder(string folder)
    {
      try
      {
        System.IO.Directory.Delete(folder);
      }
      catch (Exception ex)
      {
        log.Error("Error Deleting Folder: {0} {1}", folder, ex.Message);
      }
    }
    #endregion

    #region Preview Handling
    /// <summary>
    /// Fill the Preview Grid with the selected rows
    /// </summary>
    private void FillPreview()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      _previewForm.Tracks.Clear();
      foreach (DataGridViewRow row in _main.TracksGridView.View.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = _main.TracksGridView.TrackList[row.Index];
        _previewForm.Tracks.Add(new TrackDataPreview(track.FullFileName));
      }
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Loop through all the selected rows and set the Preview
    /// </summary>
    /// <param name="parameters"></param>
    private void OrganiseFilesPreview(string parameters)
    {
      Util.EnterMethod(Util.GetCallingMethod());

      int index = -1;

      foreach (DataGridViewRow row in _main.TracksGridView.View.Rows)
      {
        if (!row.Selected)
          continue;

        index++;
        try
        {
          track = _main.TracksGridView.TrackList[row.Index];
          trackPreview = _previewForm.Tracks[index];
          trackPreview.NewFullFileName = System.IO.Path.Combine(Util.ReplaceParametersWithTrackValues(parameters, track), trackPreview.FileName);
        }
        catch (Exception)
        {
        }
      }

      _previewForm.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion
    #endregion

    #region Event Handlers
    /// <summary>
    /// Form is Closed. Save Settings
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnClose(object sender, FormClosedEventArgs e)
    {
      Options.OrganiseSettings.LastUsedFormat = cbFormat.SelectedIndex;
      Options.OrganiseSettings.LastUsedFolderIndex = cbRootDir.SelectedIndex;
      Options.OrganiseSettings.CopyFiles = ckCopyFiles.Checked;
      Options.OrganiseSettings.OverWriteFiles = ckOverwriteFiles.Checked;
      Options.OrganiseSettings.CopyNonMusicFiles = ckCopyNonMusicFiles.Checked;
    }

    /// <summary>
    /// Open a folder browser dialog
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonBrowseRootDir_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog oFD = new FolderBrowserDialog();
      //oFD.RootFolder = Environment.SpecialFolder.MyMusic;
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        Options.OrganiseSettings.LastUsedFolders.Insert(0, oFD.SelectedPath);
        cbRootDir.Items.Insert(0, new Item(oFD.SelectedPath, oFD.SelectedPath, ""));
        cbRootDir.SelectedIndex = 0;
      }
    }

    /// <summary>
    /// A folder has been selected from the combo box, set the text
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbRootDir_SelectedIndexChanged(object sender, EventArgs e)
    {
      cbRootDir.Text = (cbRootDir.Items[cbRootDir.SelectedIndex] as Item).Value;
    }

    /// <summary>
    /// The user has left the Control, now let's see, if there's a new folder in the combo and add it to the list 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbRootDir_Leave(object sender, EventArgs e)
    {
      string selectedFolder = cbRootDir.Text.Trim();
      if (selectedFolder == "")
        return;

      bool found = false;
      foreach (Item item in cbRootDir.Items)
      {
        if (item.Value == selectedFolder)
        {
          found = true;
          break;
        }
      }
      if (!found)
      {
        Options.OrganiseSettings.LastUsedFolders.Insert(0, selectedFolder);
        cbRootDir.Items.Insert(0, new Item(selectedFolder, selectedFolder, ""));
        cbRootDir.SelectedIndex = 0;
      }
    }

    /// <summary>
    /// Check for the Delete Key pressed, while the Combo is dropped down and delete the selected folder
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbRootDir_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete)
      {
        if (cbRootDir.DroppedDown)
        {
          int selIndex = cbRootDir.SelectedIndex;
          cbRootDir.Items.RemoveAt(selIndex);
          Options.OrganiseSettings.LastUsedFolders.RemoveAt(selIndex);
          selIndex--;
          if (selIndex == -1 && cbRootDir.Items.Count > 0)
          {
            selIndex = 0;
          }
          if (selIndex > -1)
          {
            cbRootDir.SelectedIndex = selIndex;
          }
          e.Handled = true;
        }
      }
    }

    /// <summary>
    /// Apply the changes to the selected files.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btApply_Click(object sender, EventArgs e)
    {
      if (!Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.Organise))
        MessageBox.Show(localisation.ToString("tag2filename", "InvalidParm"), localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      else
      {
        Organise(cbFormat.Text);

        // Did we get a new Format in the list, then store it temporarily
        bool newFormat = true;
        foreach (string format in Options.OrganiseSettingsTemp)
        {
          if (format == cbFormat.Text)
          {
            newFormat = false;
            break;
          }
        }
        if (newFormat)
          Options.OrganiseSettingsTemp.Add(cbFormat.Text);

        if (_isPreviewOpen)
          _previewForm.Close();

        this.Close();
      }
    }

    /// <summary>
    /// Handle the Cancel button. Close Form without applying any changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btCancel_Click(object sender, EventArgs e)
    {
      if (_isPreviewOpen)
        _previewForm.Close();

      this.Close();
    }

    /// <summary>
    /// Text in the Combo is been changed, Update the Preview Value
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbFormat_TextChanged(object sender, EventArgs e)
    {
      if (!_isPreviewOpen)
        return;

      if (Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.Organise))
      {
        OrganiseFilesPreview(cbFormat.Text);
      }
    }

    /// <summary>
    /// User clicked on a parameter label. Update combo box with value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void lblParm_Click(object sender, EventArgs e)
    {
      Label label = (Label)sender;
      int cursorPos = cbFormat.SelectionStart;
      string text = cbFormat.Text;

      string parameter = Util.LabelToParameter(label.Name);

      if (parameter != String.Empty)
      {
        text = text.Insert(cursorPos, parameter);
        cbFormat.Text = text;
        cbFormat.SelectionStart = cursorPos + parameter.Length;
      }
    }

    /// <summary>
    /// Adds the current Format to the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btAddFormat_Click(object sender, EventArgs e)
    {
      bool found = false;
      foreach (string format in Options.OrganiseSettings.FormatValues)
      {
        if (format == cbFormat.Text)
        {
          found = true;
          break;
        }
      }

      if (!found)
      {
        Options.OrganiseSettings.FormatValues.Add(cbFormat.Text);
        Options.OrganiseSettings.Save();

        Options.OrganiseSettingsTemp.Add(cbFormat.Text);
        cbFormat.Items.Add(new Item(cbFormat.Text, cbFormat.Text, ""));
      }
    }

    /// <summary>
    /// Removes the current selected format from the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btRemoveFormat_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < Options.OrganiseSettings.FormatValues.Count; i++)
      {
        if (Options.OrganiseSettings.FormatValues[i] == cbFormat.Text)
        {
          Options.OrganiseSettings.FormatValues.RemoveAt(i);
          Options.OrganiseSettings.Save();
        }
      }

      Options.OrganiseSettingsTemp.RemoveAt(cbFormat.SelectedIndex);
      cbFormat.Items.RemoveAt(cbFormat.SelectedIndex);
    }

    /// <summary>
    /// Toggle the Review window display
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btReview_Click(object sender, EventArgs e)
    {
      if (_isPreviewOpen)
      {
        _isPreviewOpen = false;
        _previewForm.Hide();
      }
      else
      {
        _isPreviewOpen = true;
        if (_previewForm == null)
        {
          _previewForm = new Preview();
          // Add the second column for the new filename to preview
          _previewForm.AddGridColumn(1, "NewFullFileName", localisation.ToString("column_header", "NewFileName"), 480);
          FillPreview();
        }
        _previewForm.MaximumSize = new Size(this.Width, _previewForm.Height);
        _previewForm.Size = new Size(this.Width + 50, _previewForm.Height);
        _previewForm.Location = new Point(this.Location.X, this.Location.Y + this.Height);
        cbFormat_TextChanged(null, new EventArgs());
        _previewForm.Show();

      }
    }

    /// <summary>
    /// The form is moved. Move the Preview Window as well
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OrganiseFiles_Move(object sender, EventArgs e)
    {
      if (_isPreviewOpen)
      {
        _previewForm.Location = new Point(this.Location.X, this.Location.Y + this.Height);
      }
    }

    /// <summary>
    /// Don't allow invalid characters to be entered into the Format Field
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbFormat_Keypress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '|':
        case '"':
        case '/':
        case '*':
        case '?':
        case ':':
          e.Handled = true;
          break;
      }
    }
    #endregion
  }
}