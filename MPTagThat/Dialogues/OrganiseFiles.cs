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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Elegant.Ui;
using Microsoft.VisualBasic.FileIO;
using MPTagThat.Core;
using MPTagThat.Core.WinControls;
using MPTagThat.Dialogues;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;

#endregion

namespace MPTagThat.Organise
{
  public partial class OrganiseFiles : UserControl
  {
    #region Variables

    private readonly Main _main;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private Dictionary<string, string> _directories;
    private bool _isPreviewFilled;
    private bool _progressCancelled;
    private bool _waitCursorActive;
    private Preview _previewForm;
    private Assembly _scriptAssembly;
    private TrackData track;
    private TrackDataPreview trackPreview;

    #endregion

    #region ctor

    public OrganiseFiles(Main main)
    {
      _main = main;
      InitializeComponent();

      LoadSettings();

      LocaliseScreen();

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
      labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

      tabControl1.SelectFirstTab();

      // Register for ProgressBar Events
      ApplicationCommands.ProgressCancel.Executed += ProgressCancel_Executed;
      _main.ProgressCancelHovering += new Main.ProgressCancelHover(ProgressCancel_Hover);
      _main.ProgressCancelLeaving += new Main.ProgressCancelLeave(ProgressCancel_Leave);
    }

    #endregion

    #region Methods

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      labelHeader.Text = localisation.ToString("organise", "Heading");
    }

    #endregion

    #region Settings

    private void LoadSettings()
    {
      log.Trace(">>>");
      ckCopyFiles.Checked = Options.OrganiseSettings.CopyFiles;
      ckOverwriteFiles.Checked = Options.OrganiseSettings.OverWriteFiles;
      ckCopyNonMusicFiles.Checked = Options.OrganiseSettings.CopyNonMusicFiles;

      // Get Last Used Folders
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

      // Get Format Settings
      foreach (string item in Options.OrganiseSettings.FormatValues)
      {
        cbFormat.Items.Add(new Item(item, item, ""));
      }

      if (Options.OrganiseSettings.LastUsedFormat > cbFormat.Items.Count - 1)
        cbFormat.SelectedIndex = -1;
      else
        cbFormat.SelectedIndex = Options.OrganiseSettings.LastUsedFormat;

      // Get Scripts
      ArrayList scripts = ServiceScope.Get<IScriptManager>().GetOrganiseScripts();
      cbScripts.Items.Add(new Item("", "", "")); // Empty item first in the list
      foreach (string[] script in scripts)
      {
        cbScripts.Items.Add(new Item(script[1], script[0], script[2]));
      }
      int i = 0;
      foreach (Item item in cbScripts.Items)
      {
        if (item.Name == Options.OrganiseSettings.LastUsedScript)
        {
          cbScripts.SelectedIndex = i;
          break;
        }
        i++;
      }

      log.Trace("<<<");
    }

    #endregion

    #region Organise

    /// <summary>
    ///   Organise Files
    /// </summary>
    /// <param name = "parameter"></param>
    private void Organise(string parameter)
    {
      log.Trace(">>>");
      DataGridView tracksGrid = _main.TracksGridView.View;
      _directories = new Dictionary<string, string>();

      // First do an automatic save of all pending changes
      _main.TracksGridView.SaveAll(false);

      bool bError = false;
      string targetFolder = cbRootDir.Text;

      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        Application.DoEvents();
        _main.progressBar1.Value += 1;
        if (_progressCancelled)
        {
          ResetProgressBar();
          return;
        }

        track = _main.TracksGridView.TrackList[row.Index];

        // Replace the Parameter Value with the Values from the track
        string resolvedParmString = Util.ReplaceParametersWithTrackValues(parameter, track);
        
        if (_scriptAssembly != null) // Do we have a script selected
        {
          try
          {
            IScript script = (IScript)_scriptAssembly.CreateInstance("Script");
            targetFolder = script.Invoke(track);
            if (targetFolder == "")
            {
              // Fall back to standard folder, if something happened in the script
              targetFolder = cbRootDir.Text;
            }
          }
          catch (Exception ex)
          {
            log.Error("Script Execution failed: {0}", ex.Message);
          }
        }

        string directoryName = "";
        if (resolvedParmString.Contains(@"\"))
        {
          directoryName = Path.GetDirectoryName(resolvedParmString);
        }

        directoryName = Path.Combine(targetFolder, directoryName);

        try
        {
          // Now check the validity of the directory
          if (!Directory.Exists(directoryName))
          {
            try
            {
              Directory.CreateDirectory(directoryName);
            }
            catch (Exception e1)
            {
              bError = true;
              log.Debug("Error creating folder: {0} {1]", directoryName, e1.Message);
              _main.TracksGridView.SetStatusColumnError(row);
              _main.TracksGridView.AddErrorMessage(row,
                                                   String.Format("{0}: {1} {2}",
                                                                 localisation.ToString("message", "Error"),
                                                                 directoryName, e1.Message));
              continue; // Process next row
            }
          }

          // Store the directory of the current file in the dictionary, so that we may copy later all non-music files
          string dir = Path.GetDirectoryName(track.FullFileName);
          if (!_directories.ContainsKey(dir))
          {
            _directories.Add(dir, directoryName);
          }

          // Now construct the new File Name
          string newFilename = resolvedParmString;
          int lastBackSlash = resolvedParmString.LastIndexOf(@"\");
          if (lastBackSlash > -1)
          {
            newFilename = resolvedParmString.Substring(lastBackSlash + 1);
          }

          newFilename += track.FileExt;
          newFilename = Path.Combine(directoryName, newFilename);
          try
          {
            if (!ckOverwriteFiles.Checked)
            {
              if (File.Exists(newFilename))
              {
                bError = true;
                log.Debug("File exists: {0}", newFilename);
                _main.TracksGridView.SetStatusColumnError(row);
                _main.TracksGridView.AddErrorMessage(row, string.Format("{0}: {1}", newFilename, localisation.ToString("organise", "FileExists")));
                continue;
              }
            }

            // If new file name validates to be the same as the old file, i.e. it goes into the source folder
            // then continue, as this would lead in the source file to be deleted first and then there's nothing, which could be copied
            if (newFilename.ToLowerInvariant() == track.FullFileName.ToLowerInvariant())
            {
              bError = true;
              log.Debug("Old File and New File same: {0}", newFilename);
              _main.TracksGridView.SetStatusColumnError(row);
              _main.TracksGridView.AddErrorMessage(row, string.Format("{0}: {1}", newFilename, localisation.ToString("organise", "SameFile")));
              continue;
            }

            if (ckCopyFiles.Checked)
            {
              FileSystem.CopyFile(track.FullFileName, newFilename, ckOverwriteFiles.Checked);
              _main.TracksGridView.SetStatusColumnOk(row);
            }
            else
            {
              FileSystem.MoveFile(track.FullFileName, newFilename, ckOverwriteFiles.Checked);
              _main.TracksGridView.SetStatusColumnOk(row);
            }
          }
          catch (Exception e2)
          {
            bError = true;
            log.Error("Error Copy/Move File: {0} {1}", track.FullFileName, e2.Message);
            _main.TracksGridView.SetStatusColumnError(row);
            _main.TracksGridView.AddErrorMessage(row,
                                                 String.Format("{0}: {1}", localisation.ToString("message", "Error"),
                                                               e2.Message));
          }
        }
        catch (Exception ex)
        {
          bError = true;
          log.Error("Error Organising Files: {0} stack: {1}", ex.Message, ex.StackTrace);
          _main.TracksGridView.SetStatusColumnError(row);
          _main.TracksGridView.AddErrorMessage(row,
                                               String.Format("{0}: {1} {2}", localisation.ToString("message", "Error"),
                                                             directoryName, ex.Message));
        }
      }

      // Now that we have Moved/Copied the individual Files, we will copy / move the Pictures, etc.
      if (ckCopyNonMusicFiles.Checked)
      {
        foreach (string dir in _directories.Keys)
        {
          string[] files = Directory.GetFiles(dir);
          foreach (string file in files)
          {
            // ignore audio files, we've processed them before
            if (Util.IsAudio(file))
              continue;

            string newFilename = Path.Combine(_directories[dir], Path.GetFileName(file));
            try
            {
              if (!ckOverwriteFiles.Checked)
              {
                if (File.Exists(newFilename))
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

      ResetProgressBar();

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
          if (Directory.Exists(currentSelectedFolder))
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

      log.Trace("<<<");
    }

    private void DeleteSubFolders(string folder)
    {
      string[] subFolders = Directory.GetDirectories(folder);
      for (int i = 0; i < subFolders.Length; ++i)
      {
        DeleteSubFolders(subFolders[i]);
      }
      string[] files = Directory.GetFiles(folder);
      string[] subDirs = Directory.GetDirectories(folder);
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
        string[] files = Directory.GetFiles(parentFolder);
        string[] subDirs = Directory.GetDirectories(parentFolder);
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
        Directory.Delete(folder);
      }
      catch (Exception ex)
      {
        log.Error("Error Deleting Folder: {0} {1}", folder, ex.Message);
      }
    }

    #endregion

    #region Preview Handling

    /// <summary>
    ///   Fill the Preview Grid with the selected rows
    /// </summary>
    private void FillPreview()
    {
      log.Trace(">>>");
      _previewForm.Tracks.Clear();
      _previewForm.AddGridColumn(1, "NewFullFileName", localisation.ToString("column_header", "NewFileName"), 250);
      foreach (DataGridViewRow row in _main.TracksGridView.View.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = _main.TracksGridView.TrackList[row.Index];
        _previewForm.Tracks.Add(new TrackDataPreview(track.FullFileName));
      }
      log.Trace("<<<");
    }

    /// <summary>
    ///   Loop through all the selected rows and set the Preview
    /// </summary>
    /// <param name = "parameters"></param>
    private void OrganiseFilesPreview(string parameters)
    {
      log.Trace(">>>");

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
          trackPreview.NewFullFileName = Util.ReplaceParametersWithTrackValues(parameters, track);
        }
        catch (Exception) {}
      }

      _previewForm.Refresh();
      log.Trace("<<<");
    }

    #endregion

    #region Private Methods
    /// <summary>
    ///   Sets the maximum value of Progressbar
    /// </summary>
    /// <param name = "maxCount"></param>
    private void SetProgressBar(int maxCount)
    {
      _main.progressBar1.Maximum = maxCount == 0 ? 100 : maxCount;
      _main.progressBar1.Value = 0;
      _progressCancelled = false;
      SetWaitCursor();
    }

    /// <summary>
    ///   Reset the Progressbar to Initiaövfalue
    /// </summary>
    private void ResetProgressBar()
    {
      _main.progressBar1.Value = 0;
      _progressCancelled = false;
      ResetWaitCursor();
    }

    /// <summary>
    ///   Sets the WaitCursor during various operations
    /// </summary>
    private void SetWaitCursor()
    {
      _main.Cursor = Cursors.WaitCursor;
      this.Cursor = Cursors.WaitCursor;
      _waitCursorActive = true;
    }

    /// <summary>
    ///   Resets the WaitCursor to the default
    /// </summary>
    private void ResetWaitCursor()
    {
      _main.Cursor = Cursors.Default;
      this.Cursor = Cursors.Default;
      _waitCursorActive = false;
    }

    /// <summary>
    ///   The Progress Cancel has been fired from the Statusbar Button
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void ProgressCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      _progressCancelled = true;
      ResetWaitCursor();
    }

    /// <summary>
    ///   We're hovering over the Progress Cancel button.
    ///   If the Wait Cursor is active, change it to Default
    /// </summary>
    private void ProgressCancel_Hover(object sender, EventArgs e)
    {
      if (_waitCursorActive)
      {
        _main.Cursor = Cursors.Default;
      }
    }

    /// <summary>
    ///   We are leaving the Button again. If WaitCursor is active, we should set it back again
    /// </summary>
    private void ProgressCancel_Leave(object sender, EventArgs e)
    {
      if (_waitCursorActive)
      {
        _main.Cursor = Cursors.WaitCursor;
      }
    }
    #endregion

    #endregion

    #region Event Handlers

    /// <summary>
    ///   Open a folder browser dialog
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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
    ///   A folder has been selected from the combo box, set the text
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void cbRootDir_SelectedIndexChanged(object sender, EventArgs e)
    {
      cbRootDir.Text = (string)(cbRootDir.Items[cbRootDir.SelectedIndex] as Item).Value;
    }

    /// <summary>
    ///   The user has left the Control, now let's see, if there's a new folder in the combo and add it to the list
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void cbRootDir_Leave(object sender, EventArgs e)
    {
      string selectedFolder = cbRootDir.Text.Trim();
      if (selectedFolder == "")
        return;

      bool found = false;
      foreach (Item item in cbRootDir.Items)
      {
        if ((string)item.Value == selectedFolder)
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
    ///   Check for the Delete Key pressed, while the Combo is dropped down and delete the selected folder
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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
    ///   Apply the changes to the selected files.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btApply_Click(object sender, EventArgs e)
    {
      if (!Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.Organise))
        MessageBox.Show(localisation.ToString("tag2filename", "InvalidParm"),
                        localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      else
      {
        // See, if we have a script selected
        if (cbScripts.SelectedIndex > 0)
        {
          string scriptName = (string)(cbScripts.SelectedItem as Item).Value;
          _scriptAssembly = ServiceScope.Get<IScriptManager>().Load(scriptName);
        }

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

        Options.OrganiseSettings.LastUsedFormat = cbFormat.SelectedIndex;
        Options.OrganiseSettings.LastUsedFolderIndex = cbRootDir.SelectedIndex;
        Options.OrganiseSettings.CopyFiles = ckCopyFiles.Checked;
        Options.OrganiseSettings.OverWriteFiles = ckOverwriteFiles.Checked;
        Options.OrganiseSettings.CopyNonMusicFiles = ckCopyNonMusicFiles.Checked;
        Options.OrganiseSettings.LastUsedScript = cbScripts.Text;

        _main.ShowTagEditPanel(true);
        Dispose();
      }
    }

    /// <summary>
    ///   Handle the Cancel button. Close Form without applying any changes
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btCancel_Click(object sender, EventArgs e)
    {
      _main.ShowTagEditPanel(true);
      Dispose();
    }

    /// <summary>
    ///   Text in the Combo is been changed, Update the Preview Value
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void cbFormat_TextChanged(object sender, EventArgs e)
    {
      if (!_isPreviewFilled)
        return;

      if (Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.Organise))
      {
        OrganiseFilesPreview(cbFormat.Text);
      }
    }

    /// <summary>
    ///   User clicked on a parameter label. Update combo box with value.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void lblParm_Click(object sender, EventArgs e)
    {
      MPTLabel label = (MPTLabel)sender;
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
    ///   Adds the current Format to the list
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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
    ///   Removes the current selected format from the list
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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
    ///   Toggle the Review window display
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btReview_Click(object sender, EventArgs e)
    {
      tabControl1.SelectLastTab();
    }

    /// <summary>
    ///   Don't allow invalid characters to be entered into the Format Field
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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

    /// <summary>
    /// Tabpage select event to invoke Preview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tabControl1_SelectedTabPageChanged(object sender, Elegant.Ui.TabPageChangedEventArgs e)
    {
      if (sender == null)
      {
        return;
      }

      if ((sender as Elegant.Ui.TabControl).SelectedTabPage == tabPagePreview)
      {
        if (!_isPreviewFilled)
        {
          _isPreviewFilled = true;
          if (_previewForm == null)
          {
            _previewForm = new Preview();
            _previewForm.Dock = DockStyle.Fill;
            FillPreview();
            tabPagePreview.Controls.Add(_previewForm);
          }
          cbFormat_TextChanged(null, new EventArgs());
        }
      }
    }

    /// <summary>
    ///   A Key has been pressed
    /// </summary>
    /// <param name = "e"></param>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if ((int)keyData == 13)   // Handle Enter key as default Apply Button
      {
        btApply_Click(null, new EventArgs());
        return true;
      }
      else if ((int)keyData == 27)  // Handle Escape to Close the form
      {
        btCancel_Click(null, new EventArgs());
        return true;
      }


      return base.ProcessCmdKey(ref msg, keyData);
    }

    #endregion
  }
}