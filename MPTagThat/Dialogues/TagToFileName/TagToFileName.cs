using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Dialogues;

namespace MPTagThat.TagToFileName
{
  public partial class TagToFileName : ShapedForm
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    private int enumerateStartValue = 0;
    private int enumerateNumberDigits = 0;
    private bool _isPreviewOpen = false;
    private Preview _previewForm = null;
    private TrackData track = null;
    private TrackDataPreview trackPreview = null;
    #endregion

    #region ctor
    public TagToFileName(Main main)
    {
      _main = main;
      InitForm(false);

    }

    public TagToFileName(Main main, bool batchMode)
    {
      _main = main;
      InitForm(batchMode);
    }
    #endregion

    #region Methods
    /// <summary>
    /// The form is used in Batch Mode, when using the default button on the ribbon
    /// </summary>
    /// <param name="batchMode"></param>
    private void InitForm(bool batchMode)
    {
      InitializeComponent();
      LoadSettings();

      if (!batchMode)
      {
        this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
        ServiceScope.Get<IThemeManager>().NotifyThemeChange();

        this.labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
        this.labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

        LocaliseScreen();
      }
      else
      {
        if (cbFormat.Text == "")
        {
          this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
          ServiceScope.Get<IThemeManager>().NotifyThemeChange();

          LocaliseScreen();

          // We don't have a valid parameter. Show dialog
          this.ShowDialog();
        }
        else
        {
          Tag2FileName(cbFormat.Text);
        }
      }
    }

    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      this.labelHeader.Text = localisation.ToString("TagAndRename", "HeadingRename");
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

    #region Tag2FileName
    /// <summary>
    /// Convert File Name to Tag
    /// </summary>
    /// <param name="parameters"></param>
    private void Tag2FileName(string parameter)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      bool bErrors = false;
      DataGridView tracksGrid = _main.TracksGridView.View;
      enumerateStartValue = (int)numericUpDownStartAt.Value;
      enumerateNumberDigits = (int)numericUpDownNumberDigits.Value;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        string fileName = parameter;
        track = _main.TracksGridView.TrackList[row.Index];

        try
        {

          fileName = ReplaceParametersWithValues(parameter);

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
          track.FileName = string.Format("{0}{1}", fileName, ext);
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

    /// <summary>
    /// Replace the parameter with the REAL values of the file
    /// </summary>
    /// <param name="parameter">The Parameter Vlue</param>
    /// <param name="track">The Track Data</param>
    /// <param name="preView">Is it a Preview only</param>
    /// <returns></returns>
    private string ReplaceParametersWithValues(string parameter)
    {
      string fileName = "";
      try
      {
        if (parameter.IndexOf("<A>") > -1)
          parameter = parameter.Replace("<A>", track.Artist.Replace(';', '_').Trim());

        if (parameter.IndexOf("<T>") > -1)
          parameter = parameter.Replace("<T>", track.Title.Trim());

        if (parameter.IndexOf("<B>") > -1)
          parameter = parameter.Replace("<B>", track.Album.Trim());

        if (parameter.IndexOf("<Y>") > -1)
          parameter = parameter.Replace("<Y>", track.Year.ToString().Trim());

        if (parameter.IndexOf("<K>") > -1)
        {
          string[] str = track.Track.Split('/');
          parameter = parameter.Replace("<K>", str[0]);
        }

        if (parameter.IndexOf("<k>") > -1)
        {
          string[] str = track.Track.Split('/');
          if (str.Length > 1)
          {
            parameter = parameter.Replace("<k>", str[1]);
          }
          else
          {
            parameter = parameter.Replace("<k>", "");
          }
        }

        if (parameter.IndexOf("<D>") > -1)
        {
          string[] str = track.Disc.Split('/');
          parameter = parameter.Replace("<D>", str[0]);
        }

        if (parameter.IndexOf("<d>") > -1)
        {
          string[] str = track.Disc.Split('/');
          if (str.Length > 1)
          {
            parameter = parameter.Replace("<d>", str[1]);
          }
          else
          {
            parameter = parameter.Replace("<d>", "");
          }
        }

        if (parameter.IndexOf("<G>") > -1)
        {
          string[] str = track.Genre.Split(';');
          parameter = parameter.Replace("<G>", str[0].Trim());
        }

        if (parameter.IndexOf("<O>") > -1)
          parameter = parameter.Replace("<O>", track.AlbumArtist.Replace(';', '_').Trim());

        if (parameter.IndexOf("<C>") > -1)
          parameter = parameter.Replace("<C>", track.Comment.Trim());

        if (parameter.IndexOf("<U>") > -1)
          parameter = parameter.Replace("<U>", track.Grouping.Trim());

        if (parameter.IndexOf("<N>") > -1)
          parameter = parameter.Replace("<N>", track.Conductor.Trim());

        if (parameter.IndexOf("<R>") > -1)
          parameter = parameter.Replace("<R>", track.Composer.Replace(';', '_').Trim());

        if (parameter.IndexOf("<S>") > -1)
          parameter = parameter.Replace("<S>", track.SubTitle.Trim());

        if (parameter.IndexOf("<E>") > -1)
          parameter = parameter.Replace("<E>", track.BPM.ToString());

        if (parameter.IndexOf("<M>") > -1)
          parameter = parameter.Replace("<M>", track.Interpreter.Trim());

        if (parameter.IndexOf("<F>") > -1)
          parameter = parameter.Replace("<F>", System.IO.Path.GetFileNameWithoutExtension(track.FileName));

        if (parameter.IndexOf("<#>") > -1)
        {
            parameter = parameter.Replace("<#>", enumerateStartValue.ToString().PadLeft(enumerateNumberDigits, '0'));
            enumerateStartValue++;
        }

        fileName = Util.MakeValidFileName(parameter);
      }
      catch (Exception ex)
      {
        log.Error("Error Replacing parameters in file: {0} stack: {1}", ex.Message, ex.StackTrace);
      }
      return fileName;
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
    private void Tag2FileNamePreview(string parameters)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      
      enumerateStartValue = (int)numericUpDownStartAt.Value;
      enumerateNumberDigits = (int)numericUpDownNumberDigits.Value;
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
          trackPreview.NewFileName = ReplaceParametersWithValues(parameters);
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

      if (Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.TagToFileName))
      {
        Tag2FileNamePreview(cbFormat.Text);
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
    /// Removes the current selected format from the list
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
          _previewForm.AddGridColumn(1, "NewFileName", localisation.ToString("column_header", "NewFileName"), 350);
          FillPreview();
        }
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
    private void TagToFileName_Move(object sender, EventArgs e)
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