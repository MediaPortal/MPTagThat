using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.TagToFileName
{
  public partial class TagToFileName : Form
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    #endregion

    #region ctor
    public TagToFileName(Main main)
    {
      this._main = main;
      InitializeComponent();
      LoadSettings();

      LocaliseScreen();

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
      this.Text = localisation.ToString("TagAndRename", "HeadingRename");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    private void LoadSettings()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      foreach (string item in Options.TagToFileNameSettingsTemp)
        cbFormat.Items.Add(new Item(item, item, ""));

      if (Options.TagToFileNameSettings.LastUsedFormat > cbFormat.Items.Count - 1)
        cbFormat.SelectedIndex = -1;
      else
        cbFormat.SelectedIndex = Options.TagToFileNameSettings.LastUsedFormat;

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Convert File Name to Tag
    /// </summary>
    /// <param name="parameters"></param>
    private void Tag2FileName(string parameter)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      bool bErrors = false;
      DataGridView tracksGrid = _main.TracksGridView.View;
      int enumerateStartValue = (int)numericUpDownStartAt.Value;
      int enumerateNumberDigits = (int)numericUpDownNumberDigits.Value;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        string fileName = parameter;
        TrackData track = _main.TracksGridView.TrackList[row.Index];

        try
        {
          if (fileName.IndexOf("<A>") > -1)
            fileName = fileName.Replace("<A>", track.Artist.Replace(';', '_').Trim());

          if (fileName.IndexOf("<T>") > -1)
            fileName = fileName.Replace("<T>", track.Title.Trim());

          if (fileName.IndexOf("<B>") > -1)
            fileName = fileName.Replace("<B>", track.Album.Trim());

          if (fileName.IndexOf("<Y>") > -1)
            fileName = fileName.Replace("<Y>", track.Year.ToString().Trim());

          if (fileName.IndexOf("<K>") > -1)
          {
            string[] str = track.Track.Split('/');
            fileName = fileName.Replace("<K>", str[0]);
          }

          if (fileName.IndexOf("<k>") > -1)
          {
            string[] str = track.Track.Split('/');
            fileName = fileName.Replace("<k>", str[1]);
          }

          if (fileName.IndexOf("<D>") > -1)
          {
            string[] str = track.Disc.Split('/');
            fileName = fileName.Replace("<D>", str[0]);
          }

          if (fileName.IndexOf("<d>") > -1)
          {
            string[] str = track.Disc.Split('/');
            fileName = fileName.Replace("<d>", str[1]);
          }

          if (fileName.IndexOf("<G>") > -1)
          {
            string[] str = track.Genre.Split(';');
            fileName = fileName.Replace("<G>", str[0].Trim());
          }

          if (fileName.IndexOf("<O>") > -1)
            fileName = fileName.Replace("<O>", track.AlbumArtist.Replace(';', '_').Trim());

          if (fileName.IndexOf("<C>") > -1)
            fileName = fileName.Replace("<C>", track.Comment.Trim());

          if (fileName.IndexOf("<U>") > -1)
            fileName = fileName.Replace("<U>", track.Grouping.Trim());

          if (fileName.IndexOf("<N>") > -1)
            fileName = fileName.Replace("<N>", track.Conductor.Trim());

          if (fileName.IndexOf("<R>") > -1)
            fileName = fileName.Replace("<R>", track.Composer.Replace(';', '_').Trim());

          if (fileName.IndexOf("<S>") > -1)
            fileName = fileName.Replace("<S>", track.SubTitle.Trim());

          if (fileName.IndexOf("<E>") > -1)
            fileName = fileName.Replace("<E>", track.BPM.ToString());

          if (fileName.IndexOf("<M>") > -1)
            fileName = fileName.Replace("<M>", track.Interpreter.Trim());

          if (fileName.IndexOf("<F>") > -1)
            fileName = fileName.Replace("<F>", System.IO.Path.GetFileNameWithoutExtension(track.FileName));

          if (fileName.IndexOf("<#>") > -1)
          {
            fileName = fileName.Replace("<#>", enumerateStartValue.ToString().PadLeft(enumerateNumberDigits, '0'));
            enumerateStartValue++;
          }

          fileName = Util.MakeValidFileName(fileName);

          // Now check the length of the filename
          if (fileName.Length > 255)
          {
            log.Debug("Filename too long: {0}", fileName);
            row.Cells[1].Value = localisation.ToString("TagAndRename", "NameTooLong");
            _main.TracksGridView.AddErrorMessage(track.File.Name, String.Format("{0}: {1}", localisation.ToString("tag2filename", "NameTooLong"), fileName));
            continue; // Process next row
          }

          // Check, if we would generate duplicate file names
          foreach (DataGridViewRow file in tracksGrid.Rows)
          {
            TrackData filedata = _main.TracksGridView.TrackList[file.Index];
            if (filedata.FileName.ToLowerInvariant() == fileName.ToLowerInvariant())
            {
              log.Debug("New Filename already exists: {0}", fileName);
              row.Cells[1].Value = localisation.ToString("TagAndRename", "FileExists");
              _main.TracksGridView.AddErrorMessage(_main.TracksGridView.TrackList[row.Index].File.Name, String.Format("{0}: {1}", localisation.ToString("tag2filename", "FileExists"), fileName));
              bErrors = true;
              break;
            }
          }

          if (bErrors)
          {
            bErrors = false;
            continue;
          }

          string ext = System.IO.Path.GetExtension(track.FileName);

          // Now that we have a correct Filename and no duplicates accept the changes
          track.FileName = string.Format("{0}{1}",fileName, ext);
          track.Changed = true;

          _main.TracksGridView.Changed = true;
          _main.TracksGridView.SetBackgroundColorChanged(row.Index);
        }
        catch (Exception ex)
        {
          log.Error("Error Renaming File: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[1].Value = localisation.ToString("message", "Error");
          _main.TracksGridView.AddErrorMessage(_main.TracksGridView.TrackList[row.Index].File.Name, String.Format("{0}: {1}", localisation.ToString("tag2filename", "Rename"), fileName));
          bErrors = true;
        }
      }

      _main.TracksGridView.Changed = bErrors;
      // check, if we still have changed items in the list
      foreach (TrackData track in _main.TracksGridView.TrackList)
      {
        if (track.Changed)
          _main.TracksGridView.Changed = true;
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
      Options.TagToFileNameSettings.LastUsedFormat = cbFormat.SelectedIndex;
    }

    /// <summary>
    /// Apply the changes to the selected files.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btApply_Click(object sender, EventArgs e)
    {
      if (!Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.TagToFileName))
        MessageBox.Show(localisation.ToString("TagAndRename", "InvalidParm"), localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      else
      {
        Tag2FileName(cbFormat.Text);

        // Did we get a new Format in the list, then store it temporarily
        bool newFormat = true;
        foreach (string format in Options.TagToFileNameSettingsTemp)
        {
          if (format == cbFormat.Text)
          {
            newFormat = false;
            break;
          }
        }
        if (newFormat)
          Options.TagToFileNameSettingsTemp.Add(cbFormat.Text);

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
        cbFormat.SelectionStart = cursorPos + 3;
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
      foreach (string format in Options.TagToFileNameSettings.FormatValues)
      {
        if (format == cbFormat.Text)
        {
          found = true;
          break;
        }
      }

      if (!found)
      {
        Options.TagToFileNameSettings.FormatValues.Add(cbFormat.Text);
        Options.TagToFileNameSettings.Save();

        Options.TagToFileNameSettingsTemp.Add(cbFormat.Text);
        cbFormat.Items.Add(new Item(cbFormat.Text, cbFormat.Text, ""));
      }
    }

    /// <summary>
    /// R>emoves the current selected format from the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btRemoveFormat_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < Options.TagToFileNameSettings.FormatValues.Count; i++)
      {
        if (Options.TagToFileNameSettings.FormatValues[i] == cbFormat.Text)
        {
          Options.TagToFileNameSettings.FormatValues.RemoveAt(i);
          Options.TagToFileNameSettings.Save();
        }
      }

      Options.TagToFileNameSettingsTemp.RemoveAt(cbFormat.SelectedIndex);
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
        case '\\':
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