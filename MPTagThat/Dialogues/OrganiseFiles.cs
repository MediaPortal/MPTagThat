using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.Organise
{
  public partial class OrganiseFiles : Telerik.WinControls.UI.ShapedForm
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    private Dictionary<string, string> _directories = null;
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

        // Trim trailing slashes
        string directoryName = parameter.Trim(new char[] { '\\' });
        TrackData track = _main.TracksGridView.TrackList[row.Index];

        try
        {
          if (directoryName.IndexOf("<A>") > -1)
            directoryName = directoryName.Replace("<A>", track.Artist.Replace(';', '_').Trim());

          if (directoryName.IndexOf("<T>") > -1)
            directoryName = directoryName.Replace("<T>", track.Title.Trim());

          if (directoryName.IndexOf("<B>") > -1)
            directoryName = directoryName.Replace("<B>", track.Album.Trim());

          if (directoryName.IndexOf("<Y>") > -1)
            directoryName = directoryName.Replace("<Y>", track.Year.ToString().Trim());

          if (directoryName.IndexOf("<K>") > -1)
          {
            string[] str = track.Track.Split('/');
            directoryName = directoryName.Replace("<K>", str[0]);
          }

          if (directoryName.IndexOf("<k>") > -1)
          {
            string[] str = track.Track.Split('/');
            directoryName = directoryName.Replace("<k>", str[1]);
          }

          if (directoryName.IndexOf("<D>") > -1)
          {
            string[] str = track.Disc.Split('/');
            directoryName = directoryName.Replace("<D>", str[0]);
          }

          if (directoryName.IndexOf("<d>") > -1)
          {
            string[] str = track.Disc.Split('/');
            directoryName = directoryName.Replace("<d>", str[1]);
          }

          if (directoryName.IndexOf("<G>") > -1)
          {
            string[] str = track.Genre.Split(';');
            directoryName = directoryName.Replace("<G>", str[0].Trim());
          }

          if (directoryName.IndexOf("<O>") > -1)
            directoryName = directoryName.Replace("<O>", track.AlbumArtist.Replace(';', '_').Trim());

          if (directoryName.IndexOf("<C>") > -1)
            directoryName = directoryName.Replace("<C>", track.Comment.Trim());

          if (directoryName.IndexOf("<U>") > -1)
            directoryName = directoryName.Replace("<U>", track.Grouping.Trim());

          if (directoryName.IndexOf("<N>") > -1)
            directoryName = directoryName.Replace("<N>", track.Conductor.Trim());

          if (directoryName.IndexOf("<R>") > -1)
            directoryName = directoryName.Replace("<R>", track.Composer.Replace(';', '_').Trim());

          if (directoryName.IndexOf("<S>") > -1)
            directoryName = directoryName.Replace("<S>", track.SubTitle.Trim());

          if (directoryName.IndexOf("<E>") > -1)
            directoryName = directoryName.Replace("<E>", track.BPM.ToString());

          if (directoryName.IndexOf("<M>") > -1)
            directoryName = directoryName.Replace("<M>", track.Interpreter.Trim());

          if (directoryName.IndexOf("<I>") > -1)
            directoryName = directoryName.Replace("<I>", track.File.Properties.AudioBitrate.ToString());

          int index = directoryName.IndexOf("<A:");
          int last = -1;
          if (index > -1)
          {
            last = directoryName.IndexOf(">", index);
            string s1 = directoryName.Substring(index, last - index + 1);
            int strLength = Convert.ToInt32(s1.Substring(3, 1));
            string s2 = track.Artist.Replace(';', '_').Trim();

            if (s2.Length >= strLength)
              s2 = s2.Substring(0, strLength);

            directoryName = directoryName.Replace(s1, s2);
          }

          index = directoryName.IndexOf("<O:");
          last = -1;
          if (index > -1)
          {
            last = directoryName.IndexOf(">", index);
            string s1 = directoryName.Substring(index, last - index + 1);
            int strLength = Convert.ToInt32(s1.Substring(3, 1));
            string s2 = track.AlbumArtist.Replace(';', '_').Trim();

            if (s2.Length >= strLength)
              s2 = s2.Substring(0, strLength);

            directoryName = directoryName.Replace(s1, s2);
          }

          // Empty Values would create invalid folders
          directoryName = directoryName.Replace(@"\\", @"\_\");

          // If the directory name starts with a backslash, we've got an empty value on the beginning
          if (directoryName.IndexOf("\\") == 0)
            directoryName = "_" + directoryName;

          // We might have an empty value on the end of the path, which is indicated by a slash. 
          // replace it with underscore
          if (directoryName.LastIndexOf("\\") == directoryName.Length - 1)
            directoryName += "_";

          directoryName = Util.MakeValidFolderName(directoryName);
          directoryName = System.IO.Path.Combine(cbRootDir.Text, directoryName);

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
              row.Cells[1].Value = localisation.ToString("message", "Error");
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
                row.Cells[1].Value = localisation.ToString("organise", "Exists");
                _main.TracksGridView.AddErrorMessage(newFilename, localisation.ToString("organise", "FileExists"));
                continue;
              }
            }

            // If new file name validates to be the same as the ol file, i.e. it goes into the source folder
            // then continue, as this would lead in the source file to be deleted first and then there's nothing, which could be copied
            if (newFilename.ToLowerInvariant() == track.FullFileName.ToLowerInvariant())
            {
              bError = true;
              log.Debug("Old File and New File same: {0}", newFilename);
              row.Cells[1].Value = localisation.ToString("organise", "Exists");
              _main.TracksGridView.AddErrorMessage(newFilename, localisation.ToString("organise", "SameFile"));
              continue;
            }

            if (ckCopyFiles.Checked)
            {
              System.IO.File.Copy(track.FullFileName, newFilename, true);
              row.Cells[1].Value = localisation.ToString("organise", "Copied");
            }
            else
            {
              if (System.IO.File.Exists(newFilename))
                System.IO.File.Delete(newFilename);

              System.IO.File.Move(track.FullFileName, newFilename);
              row.Cells[1].Value = localisation.ToString("organise", "Moved");
            }
          }
          catch (Exception e2)
          {
            bError = true;
            log.Error("Error Copy/Move File: {0} {1}", track.FullFileName, e2.Message);
            row.Cells[1].Value = localisation.ToString("mesage", "Error");
            _main.TracksGridView.AddErrorMessage(track.File.Name, String.Format("{0}: {1}", localisation.ToString("message", "Error"), e2.Message));
          }
        }
        catch (Exception ex)
        {
          bError = true;
          log.Error("Error Organising Files: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[1].Value = localisation.ToString("message", "Error");
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
                System.IO.File.Copy(file, newFilename, true);
              }
              else
              {
                if (System.IO.File.Exists(newFilename))
                  System.IO.File.Delete(newFilename);

                System.IO.File.Move(file, newFilename);
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
        _main.RefreshFolders();
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
      this.Close();
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