using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Dialogues;

namespace MPTagThat.FileNameToTag
{
  public partial class FileNameToTag : Telerik.WinControls.UI.ShapedForm
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    private TrackData track = null;
    private TrackDataPreview trackPreview = null;
    private bool _isPreviewOpen = false;
    private Preview _previewForm = null;
    #endregion

    #region ctor
    public FileNameToTag(Main main)
    {
      this._main = main;
      InitializeComponent();

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
      this.labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

      LoadSettings();

      LocaliseScreen();
    }
    #endregion

    #region Methods
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      this.labelHeader.Text = localisation.ToString("TagAndRename", "HeadingTag");
    }
    #endregion

    #region Settings
    private void LoadSettings()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      foreach (string item in Options.FileNameToTagSettingsTemp)
        cbFormat.Items.Add(new Item(item, item, ""));

      if (Options.FileNameToTagSettings.LastUsedFormat > cbFormat.Items.Count - 1)
        cbFormat.SelectedIndex = -1;
      else
        cbFormat.SelectedIndex = Options.FileNameToTagSettings.LastUsedFormat;
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region File Name To Tag
    /// <summary>
    /// Convert File Name to Tag
    /// </summary>
    /// <param name="parameters"></param>
    private void FileName2Tag(List<MPTagThat.FileNameToTag.ParameterPart> parameters)
    {
      Util.EnterMethod(Util.GetCallingMethod());
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
          track = _main.TracksGridView.TrackList[row.Index];
          track.Changed = true;

          ReplaceParametersWithValues(parameters, false);
        }
        catch (Exception ex)
        {
          log.Error("Error applying changes from Filename To Tag: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[1].Value = localisation.ToString("message", "Error");
          _main.TracksGridView.AddErrorMessage(_main.TracksGridView.TrackList[row.Index].File.Name, localisation.ToString("TagAndRename", "InvalidParm"));
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

    private void ReplaceParametersWithValues(List<MPTagThat.FileNameToTag.ParameterPart> parameters, bool preview)
    {
      List<string> splittedFileValues = new List<string>();

      // Split up the file name
      // We use already the FileName from the Track instance, which might be already modified by the user.
      string file;
      if (preview)
        file = String.Format(@"{0}\{1}", Path.GetDirectoryName(trackPreview.FullFileName), Path.GetFileNameWithoutExtension(trackPreview.FullFileName));
      else
        file = String.Format(@"{0}\{1}", Path.GetDirectoryName(track.File.Name), Path.GetFileNameWithoutExtension(track.FileName));

      string[] fileArray = file.Split(new char[] { '\\' });

      // Now set Upper Bound depending on the length of parameters and file
      int upperBound;
      if (parameters.Count >= fileArray.Length)
        upperBound = fileArray.Length - 1;
      else
        upperBound = parameters.Count - 1;

      // Now loop through the delimiters and assign files
      for (int i = 0; i <= upperBound; i++)
      {
        MPTagThat.FileNameToTag.ParameterPart parameterpart = parameters[i];
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
              string[] artists = splittedFileValues[index].Split(';');
              if (preview)
                trackPreview.Artist = splittedFileValues[index];
              else
                track.File.Tag.Performers = artists;
              break;

            case "<T>":
              if (preview)
                trackPreview.Title = splittedFileValues[index];
              else
                track.File.Tag.Title = splittedFileValues[index];
              break;

            case "<B>":
              if (preview)
                trackPreview.Album = splittedFileValues[index];
              else
                track.File.Tag.Album = splittedFileValues[index];
              break;

            case "<Y>":
              if (preview)
                trackPreview.Year = splittedFileValues[index];
              else
                track.File.Tag.Year = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<K>":
              if (preview)
                trackPreview.Track = splittedFileValues[index];
              else
                track.File.Tag.Track = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<k>":
              if (preview)
                trackPreview.NumTrack = splittedFileValues[index];
              else
                track.File.Tag.TrackCount = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<D>":
              if (preview)
                trackPreview.Disc = splittedFileValues[index];
              else
                track.File.Tag.Disc = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<d>":
              if (preview)
                trackPreview.NumDisc = splittedFileValues[index];
              else
                track.File.Tag.DiscCount = Convert.ToUInt32(splittedFileValues[index]);
              break;

            case "<G>":
              string[] genres = splittedFileValues[index].Split(';');
              if (preview)
                trackPreview.Genre = splittedFileValues[index];
              else
                track.File.Tag.Genres = genres;
              break;

            case "<O>":
              string[] albumartists = splittedFileValues[index].Split(';');
              if (preview)
                trackPreview.AlbumArtist = splittedFileValues[index];
              else
                track.File.Tag.AlbumArtists = albumartists;
              break;

            case "<C>":
              if (preview)
                trackPreview.Comment = splittedFileValues[index];
              else
                track.File.Tag.Comment = splittedFileValues[index];
              break;

            case "<N>":
              if (preview)
                trackPreview.Conductor = splittedFileValues[index];
              else
                track.File.Tag.Conductor = splittedFileValues[index];
              break;

            case "<R>":
              string[] composers = splittedFileValues[index].Split(';');
              if (preview)
                trackPreview.Composer = splittedFileValues[index];
              else
                track.File.Tag.Composers = composers;
              break;

            case "<U>":
              if (preview)
                trackPreview.Grouping = splittedFileValues[index];
              else
                track.File.Tag.Grouping = splittedFileValues[index];
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
                track.File.Tag.BeatsPerMinute = Convert.ToUInt32(splittedFileValues[index]);
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
    private void FileName2TagPreview(List<MPTagThat.FileNameToTag.ParameterPart> parameters)
    {
      Util.EnterMethod(Util.GetCallingMethod());

      _previewForm.BuildPreviewGrid(cbFormat.Text);

      foreach (TrackDataPreview row in _previewForm.Tracks)
      {
        try
        {
          trackPreview = row;
          ReplaceParametersWithValues(parameters, true);
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
      Options.FileNameToTagSettings.LastUsedFormat = cbFormat.SelectedIndex;
    }

    /// <summary>
    /// Apply the changes to the selected files.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btApply_Click(object sender, EventArgs e)
    {
      if (!Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.FileNameToTag))
        MessageBox.Show(localisation.ToString("TagAndRename", "InvalidParm"), localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
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

        if (_previewForm != null)
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
      if (_previewForm != null)
        _previewForm.Close();

      this.Close();
    }

    /// <summary>
    /// Text in the Combo is been changed, Update the Preview Values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbFormat_TextChanged(object sender, EventArgs e)
    {
      if (!_isPreviewOpen)
        return;

      if (Util.CheckParameterFormat(cbFormat.Text, Options.ParameterFormat.FileNameToTag))
      {
        TagFormat tagFormat = new TagFormat(cbFormat.Text);
        List<ParameterPart> parts = tagFormat.ParameterParts;
        FileName2TagPreview(parts);
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
        if (label.Name == "lblParmFolder")
          cursorPos += 1;
        else
          cursorPos += 3;

        cbFormat.SelectionStart = cursorPos;
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
    /// R>emoves the current selected format from the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    private void FileNameToTag_Move(object sender, EventArgs e)
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