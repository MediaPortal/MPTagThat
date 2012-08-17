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
using MPTagThat.Core;
using MPTagThat.Core.WinControls;
using MPTagThat.Dialogues;

#endregion

namespace MPTagThat.FileNameToTag
{
  public partial class FileNameToTag : UserControl
  {
    #region Variables

    private readonly Main _main;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private bool _isPreviewFilled;
    private Preview _previewForm;
    private TrackData track;
    private TrackDataPreview trackPreview;

    #endregion

    #region ctor

    public FileNameToTag(Main main)
    {
      _main = main;
      InitializeComponent();

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
      labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

      LoadSettings();

      LocaliseScreen();

      tabControl1.SelectFirstTab();
    }

    #endregion

    #region Methods

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      labelHeader.Text = localisation.ToString("TagAndRename", "HeadingTag");
    }

    #endregion

    #region Settings

    private void LoadSettings()
    {
      log.Trace(">>>");
      foreach (string item in Options.FileNameToTagSettingsTemp)
        cbFormat.Items.Add(new Item(item, item, ""));

      if (Options.FileNameToTagSettings.LastUsedFormat > cbFormat.Items.Count - 1)
        cbFormat.SelectedIndex = -1;
      else
        cbFormat.SelectedIndex = Options.FileNameToTagSettings.LastUsedFormat;
      log.Trace("<<<");
    }

    #endregion

    #region File Name To Tag

    /// <summary>
    ///   Convert File Name to Tag
    /// </summary>
    /// <param name = "parameters"></param>
    private void FileName2Tag(List<ParameterPart> parameters)
    {
      log.Trace(">>>");
      bool bErrors = false;
      DataGridView tracksGrid = _main.TracksGridView.View;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        try
        {
          _main.TracksGridView.Changed = true;
          _main.TracksGridView.SetBackgroundColorChanged(row.Index);
          track = Options.Songlist[row.Index];
          track.Changed = true;

          ReplaceParametersWithValues(parameters, false);
        }
        catch (Exception ex)
        {
          log.Error("Error applying changes from Filename To Tag: {0} stack: {1}", ex.Message, ex.StackTrace);
          Options.Songlist[row.Index].Status = 2;
          _main.TracksGridView.AddErrorMessage(row, localisation.ToString("TagAndRename", "InvalidParm"));
          bErrors = true;
        }
      }

      _main.TracksGridView.Changed = bErrors;
      // check, if we still have changed items in the list
      foreach (TrackData track in Options.Songlist)
      {
        if (track.Changed)
          _main.TracksGridView.Changed = true;
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      log.Trace("<<<");
    }

    private void ReplaceParametersWithValues(List<ParameterPart> parameters, bool preview)
    {
      List<string> splittedFileValues = new List<string>();

      // Split up the file name
      // We use already the FileName from the Track instance, which might be already modified by the user.
      string file;
      if (preview)
        file = String.Format(@"{0}\{1}", Path.GetDirectoryName(trackPreview.FullFileName),
                             Path.GetFileNameWithoutExtension(trackPreview.FullFileName));
      else
        file = String.Format(@"{0}\{1}", Path.GetDirectoryName(track.FullFileName),
                             Path.GetFileNameWithoutExtension(track.FileName));

      string[] fileArray = file.Split(new[] {'\\'});

      // Now set Upper Bound depending on the length of parameters and file
      int upperBound;
      if (parameters.Count >= fileArray.Length)
        upperBound = fileArray.Length - 1;
      else
        upperBound = parameters.Count - 1;

      // Now loop through the delimiters and assign files
      for (int i = 0; i <= upperBound; i++)
      {
        ParameterPart parameterpart = parameters[i];
        string[] delims = parameterpart.Delimiters;
        List<string> parms = parameterpart.Parameters;

        // Set the part of the File to Process
        string filePart = fileArray[fileArray.GetUpperBound(0) - i];
        splittedFileValues.Clear();

        int upperBoundDelims = delims.GetUpperBound(0);
        for (int j = 0; j <= upperBoundDelims; j++)
        {
          if ((j == upperBoundDelims) | (delims[j] != ""))
          {
            if (filePart.IndexOf(delims[j]) == 0 && j == upperBoundDelims)
            {
              splittedFileValues.Add(filePart);
              break;
            }

            int delimIndex = filePart.IndexOf(delims[j]);
            if (delimIndex > -1)
            {
              splittedFileValues.Add(filePart.Substring(0, filePart.IndexOf(delims[j])));
              filePart = filePart.Substring(filePart.IndexOf(delims[j]) + delims[j].Length);
            }
          }
        }

        int index = -1;
        // Now we need to Update the Tag Values
        foreach (string parm in parms)
        {
          index++;
          switch (parm)
          {
            case "<A>":
              if (preview)
                trackPreview.Artist = splittedFileValues[index];
              else
                track.Artist = splittedFileValues[index];
              break;

            case "<T>":
              if (preview)
                trackPreview.Title = splittedFileValues[index];
              else
                track.Title = splittedFileValues[index];
              break;

            case "<B>":
              if (preview)
                trackPreview.Album = splittedFileValues[index];
              else
                track.Album = splittedFileValues[index];
              break;

            case "<Y>":
              if (preview)
                trackPreview.Year = splittedFileValues[index];
              else
                track.Year = Convert.ToInt32(splittedFileValues[index]);
              break;

            case "<K>":
              if (preview)
                trackPreview.Track = splittedFileValues[index];
              else
                track.TrackNumber = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<k>":
              if (preview)
                trackPreview.NumTrack = splittedFileValues[index];
              else
                track.TrackCount = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<D>":
              if (preview)
                trackPreview.Disc = splittedFileValues[index];
              else
                track.DiscNumber = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<d>":
              if (preview)
                trackPreview.NumDisc = splittedFileValues[index];
              else
                track.DiscCount = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<G>":
              if (preview)
                trackPreview.Genre = splittedFileValues[index];
              else
                track.Genre = splittedFileValues[index];
              break;

            case "<O>":
              if (preview)
                trackPreview.AlbumArtist = splittedFileValues[index];
              else
                track.AlbumArtist = splittedFileValues[index];
              break;

            case "<C>":
              if (preview)
                trackPreview.Comment = splittedFileValues[index];
              else
                track.Comment = splittedFileValues[index];
              break;

            case "<N>":
              if (preview)
                trackPreview.Conductor = splittedFileValues[index];
              else
                track.Conductor = splittedFileValues[index];
              break;

            case "<R>":
              if (preview)
                trackPreview.Composer = splittedFileValues[index];
              else
                track.Composer = splittedFileValues[index];
              break;

            case "<U>":
              if (preview)
                trackPreview.Grouping = splittedFileValues[index];
              else
                track.Grouping = splittedFileValues[index];
              break;

            case "<S>":
              if (preview)
                trackPreview.SubTitle = splittedFileValues[index];
              else
                track.SubTitle = splittedFileValues[index];
              break;

            case "<M>":
              if (preview)
                trackPreview.Interpreter = splittedFileValues[index];
              else
                track.Interpreter = splittedFileValues[index];
              break;

            case "<E>":
              if (preview)
                trackPreview.BPM = splittedFileValues[index];
              else
                track.BPM = Convert.ToInt32(splittedFileValues[index]);
              break;

            case "<X>":
              // ignore it
              break;
          }
        }
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
      foreach (DataGridViewRow row in _main.TracksGridView.View.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        _previewForm.Tracks.Add(new TrackDataPreview(track.FullFileName));
      }
      log.Trace("<<<");
    }

    /// <summary>
    ///   Loop through all the selected rows and set the Preview
    /// </summary>
    /// <param name = "parameters"></param>
    private void FileName2TagPreview(List<ParameterPart> parameters)
    {
      log.Trace(">>>");

      _previewForm.BuildPreviewGrid(cbFormat.Text);

      foreach (TrackDataPreview row in _previewForm.Tracks)
      {
        try
        {
          trackPreview = row;
          ReplaceParametersWithValues(parameters, true);
        }
        catch (Exception) {}
      }

      _previewForm.Refresh();
      log.Trace("<<<");
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
      if (!Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.FileNameToTag))
        MessageBox.Show(localisation.ToString("TagAndRename", "InvalidParm"),
                        localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      else
      {
        TagFormat tagFormat = new TagFormat(cbFormat.Text);
        List<ParameterPart> parts = tagFormat.ParameterParts;

        FileName2Tag(parts);

        // Did we get a new Format in the list, then store it temporarily
        bool newFormat = true;
        foreach (string format in Options.FileNameToTagSettingsTemp)
        {
          if (format == cbFormat.Text)
          {
            newFormat = false;
            break;
          }
        }
        if (newFormat)
          Options.FileNameToTagSettingsTemp.Add(cbFormat.Text);

        Options.FileNameToTagSettings.LastUsedFormat = cbFormat.SelectedIndex;
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
    ///   Text in the Combo is been changed, Update the Preview Values
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void cbFormat_TextChanged(object sender, EventArgs e)
    {
      if (!_isPreviewFilled)
        return;

      if (Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.FileNameToTag))
      {
        TagFormat tagFormat = new TagFormat(cbFormat.Text);
        List<ParameterPart> parts = tagFormat.ParameterParts;
        FileName2TagPreview(parts);
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
        if (label.Name == "lblParmFolder")
          cursorPos += 1;
        else
          cursorPos += 3;

        cbFormat.SelectionStart = cursorPos;
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
      foreach (string format in Options.FileNameToTagSettings.FormatValues)
      {
        if (format == cbFormat.Text)
        {
          found = true;
          break;
        }
      }

      if (!found)
      {
        Options.FileNameToTagSettings.FormatValues.Add(cbFormat.Text);
        Options.FileNameToTagSettings.Save();

        Options.FileNameToTagSettingsTemp.Add(cbFormat.Text);
        cbFormat.Items.Add(new Item(cbFormat.Text, cbFormat.Text, ""));
      }
    }

    /// <summary>
    ///   R>emoves the current selected format from the list
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btRemoveFormat_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < Options.FileNameToTagSettings.FormatValues.Count; i++)
      {
        if (Options.FileNameToTagSettings.FormatValues[i] == cbFormat.Text)
        {
          Options.FileNameToTagSettings.FormatValues.RemoveAt(i);
          Options.FileNameToTagSettings.Save();
        }
      }

      Options.FileNameToTagSettingsTemp.RemoveAt(cbFormat.SelectedIndex);
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