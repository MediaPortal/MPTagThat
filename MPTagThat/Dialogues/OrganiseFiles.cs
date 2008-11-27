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
  public partial class OrganiseFiles : Form
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

      tbRootDir.Text = Options.OrganiseSettings.LastUsedFolder == "" ? Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) : Options.OrganiseSettings.LastUsedFolder;

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();
    }
    #endregion

    #region Methods
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Text = localisation.ToString("organise", "Heading");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    private void LoadSettings()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      ckCopyFiles.Checked = Options.OrganiseSettings.CopyFiles;
      ckOverwriteFiles.Checked = Options.OrganiseSettings.OverWriteFiles;
      ckCopyNonMusicFiles.Checked = Options.OrganiseSettings.CopyNonMusicFiles;

      foreach (string item in Options.OrganiseSettings.FormatValues)
        cbFormat.Items.Add(new Item(item, item, ""));

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

          // If the directory name starts with a backslash, we've got an empty value on the biginning
          if (directoryName.IndexOf("\\") == 0)
            directoryName = "_" + directoryName;

          // We might have an empty value on the end of the path, which is indicated by a slash. 
          // replace it with underscore
          if (directoryName.LastIndexOf("\\") == directoryName.Length - 1)
            directoryName += "_";
          
          directoryName = Util.MakeValidFolderName(directoryName);
          directoryName = System.IO.Path.Combine(tbRootDir.Text, directoryName);

          // Now check the validity of the directory
          if (!System.IO.Directory.Exists(directoryName))
          {
            try
            {
              System.IO.Directory.CreateDirectory(directoryName);
            }
            catch (Exception e1)
            {
              log.Debug("Error creating folder: {0} {1]", directoryName, e1.Message);
              row.Cells[1].Value = localisation.ToString("message", "Error");
              _main.TracksGridView.AddErrorMessage(track.File.Name, String.Format("{0}: {1} {2}", localisation.ToString("message", "Error"), directoryName, e1.Message));
              continue; // Process next row
            }
          }

          // Store the directory of the current file in the dictionary, so that we may copy later all non-music files
          string dir = System.IO.Path.GetDirectoryName(track.FullFileName);
          if (!_directories.ContainsKey(dir))
            _directories.Add(dir, directoryName);

          string newFilename = System.IO.Path.Combine(directoryName, track.FileName);
          try
          {
            if (!ckOverwriteFiles.Checked)
            {
              if (System.IO.File.Exists(newFilename))
              {
                log.Debug("File exists: {0}", newFilename);
                row.Cells[1].Value = localisation.ToString("organise", "Exists");
                _main.TracksGridView.AddErrorMessage(newFilename, localisation.ToString("organise", "FileExists"));
                continue;
              }
            }

            if (ckCopyFiles.Checked)
            {
              System.IO.File.Copy(track.FullFileName, newFilename, true);
              row.Cells[1].Value = localisation.ToString("organise", "Copied");
            }
            else
            {
              System.IO.File.Move(track.FullFileName, newFilename);
              row.Cells[1].Value = localisation.ToString("organise", "Moved");
            }
          }
          catch (Exception e2)
          {
            log.Error("Error Copy/Move File: {0} {1}", track.FullFileName, e2.Message);
            row.Cells[1].Value = localisation.ToString("mesage", "Error");
            _main.TracksGridView.AddErrorMessage(track.File.Name, String.Format("{0}: {1}", localisation.ToString("message", "Error"), e2.Message));
          }
        }
        catch (Exception ex)
        {
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

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      Util.LeaveMethod(Util.GetCallingMethod());
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
      Options.OrganiseSettings.LastUsedFolder = tbRootDir.Text;
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
        tbRootDir.Text = oFD.SelectedPath;
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