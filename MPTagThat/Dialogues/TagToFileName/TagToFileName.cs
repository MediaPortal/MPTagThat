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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.WinControls;
using MPTagThat.Dialogues;

#endregion

namespace MPTagThat.TagToFileName
{
  public partial class TagToFileName : UserControl
  {
    #region Variables

    private readonly Main _main;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private bool _isPreviewFilled;
    private Preview _previewForm;
    private int enumerateNumberDigits;
    private int enumerateStartValue;
    private TrackData track;
    private TrackDataPreview trackPreview;

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
    ///   The form is used in Batch Mode, when using the default button on the ribbon
    /// </summary>
    /// <param name = "batchMode"></param>
    private void InitForm(bool batchMode)
    {
      InitializeComponent();
      LoadSettings();

      if (!batchMode)
      {
        BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
        ServiceScope.Get<IThemeManager>().NotifyThemeChange();

        labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
        labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

        LocaliseScreen();

        tabControl1.SelectFirstTab();
      }
      else
      {
        if (cbFormat.Text == "")
        {
          BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
          ServiceScope.Get<IThemeManager>().NotifyThemeChange();

          LocaliseScreen();

          // We don't have a valid parameter. Show dialog
          tabControl1.SelectFirstTab();
        }
        else
        {
          Tag2FileName(cbFormat.Text);
          btCancel_Click(null, new EventArgs());
        }
      }
    }

    private void LoadSettings()
    {
      log.Trace(">>>");
      foreach (string item in Options.TagToFileNameSettingsTemp)
        cbFormat.Items.Add(new Item(item, item, ""));

      if (Options.TagToFileNameSettings.LastUsedFormat > cbFormat.Items.Count - 1)
        cbFormat.SelectedIndex = -1;
      else
        cbFormat.SelectedIndex = Options.TagToFileNameSettings.LastUsedFormat;

      log.Trace("<<<");
    }

    #region Tag2FileName

    /// <summary>
    ///   Convert File Name to Tag
    /// </summary>
    /// <param name = "parameters"></param>
    private void Tag2FileName(string parameter)
    {
      log.Trace(">>>");
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
            _main.TracksGridView.TrackList[row.Index].Status = 2;
            _main.TracksGridView.AddErrorMessage(row,
                                                 String.Format("{0}: {1}",
                                                               localisation.ToString("tag2filename", "NameTooLong"),
                                                               fileName));
            continue; // Process next row
          }

          // Check, if we would generate duplicate file names
          foreach (DataGridViewRow file in tracksGrid.Rows)
          {
            TrackData filedata = _main.TracksGridView.TrackList[file.Index];
            if (filedata.FileName.ToLowerInvariant() == fileName.ToLowerInvariant())
            {
              log.Debug("New Filename already exists: {0}", fileName);
              _main.TracksGridView.TrackList[row.Index].Status = 2;
              _main.TracksGridView.AddErrorMessage(row,
                                                   String.Format("{0}: {1}",
                                                                 localisation.ToString("tag2filename", "FileExists"),
                                                                 fileName));
              bErrors = true;
              break;
            }
          }

          if (bErrors)
          {
            bErrors = false;
            continue;
          }

          string ext = Path.GetExtension(track.FileName);

          // Now that we have a correct Filename and no duplicates accept the changes
          track.FileName = string.Format("{0}{1}", fileName, ext);
          track.Changed = true;

          _main.TracksGridView.Changed = true;
          _main.TracksGridView.SetBackgroundColorChanged(row.Index);
        }
        catch (Exception ex)
        {
          log.Error("Error Renaming File: {0} stack: {1}", ex.Message, ex.StackTrace);
          _main.TracksGridView.TrackList[row.Index].Status = 2;
          _main.TracksGridView.AddErrorMessage(row,
                                               String.Format("{0}: {1}", localisation.ToString("tag2filename", "Rename"),
                                                             fileName));
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

      log.Trace("<<<");
    }

    /// <summary>
    ///   Replace the parameter with the REAL values of the file
    /// </summary>
    /// <param name = "parameter">The Parameter Vlue</param>
    /// <returns></returns>
    private string ReplaceParametersWithValues(string parameter)
    {
      string fileName = "";
      try
      {
        // FilenameToTag Special Variables
        if (parameter.IndexOf("<F>") > -1)
          parameter = parameter.Replace("<F>", Path.GetFileNameWithoutExtension(track.FileName));

        if (parameter.IndexOf("<#>") > -1)
        {
          parameter = parameter.Replace("<#>", enumerateStartValue.ToString().PadLeft(enumerateNumberDigits, '0'));
          enumerateStartValue++;
        }

        fileName = Util.ReplaceParametersWithTrackValues(parameter, track);
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
    ///   Fill the Preview Grid with the selected rows
    /// </summary>
    private void FillPreview()
    {
      log.Trace(">>>");
      _previewForm.Tracks.Clear();
      _previewForm.AddGridColumn(1, "NewFileName", localisation.ToString("column_header", "NewFileName"), 250);
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
    private void Tag2FileNamePreview(string parameters)
    {
      log.Trace(">>>");

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
        catch (Exception) {}
      }

      _previewForm.Refresh();
      log.Trace("<<<");
    }

    #endregion

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      labelHeader.Text = localisation.ToString("TagAndRename", "HeadingRename");
    }

    #endregion

    #endregion

    #region Event Handlers

    /// <summary>
    ///   Apply the changes to the selected files.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btApply_Click(object sender, EventArgs e)
    {
      if (!Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.TagToFileName))
        MessageBox.Show(localisation.ToString("TagAndRename", "InvalidParm"),
                        localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
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

        Options.TagToFileNameSettings.LastUsedFormat = cbFormat.SelectedIndex;
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

      if (Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.TagToFileName))
      {
        Tag2FileNamePreview(cbFormat.Text);
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
        cbFormat.SelectionStart = cursorPos + 3;
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
    ///   Removes the current selected format from the list
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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