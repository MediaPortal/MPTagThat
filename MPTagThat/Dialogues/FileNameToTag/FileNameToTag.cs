using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.FileNameToTag
{
  public partial class FileNameToTag : Telerik.WinControls.UI.ShapedForm
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
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
          TrackData track = _main.TracksGridView.TrackList[row.Index];
          track.Changed = true;

          List<string> splittedFileValues = new List<string>();

          // Split up the file name
          // We use already the FileName from the Track instance, which might be already modified by the user.
          string file = String.Format(@"{0}\{1}", Path.GetDirectoryName(track.File.Name), Path.GetFileNameWithoutExtension(track.FileName));
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
                if (filePart.IndexOf(delims[j]) == 0)
                {
                  splittedFileValues.Add(filePart);
                  break;
                }
                splittedFileValues.Add(filePart.Substring(0, filePart.IndexOf(delims[j])));
                filePart = filePart.Substring(filePart.IndexOf(delims[j]) + delims[j].Length);
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
                  track.File.Tag.Performers = artists;
                  break;

                case "<T>":
                  track.File.Tag.Title = splittedFileValues[index];
                  break;

                case "<B>":
                  track.File.Tag.Album = splittedFileValues[index];
                  break;

                case "<Y>":
                  track.File.Tag.Year = Convert.ToUInt32(splittedFileValues[index]);
                  break;

                case "<K>":
                  track.File.Tag.Track = Convert.ToUInt32(splittedFileValues[index]);
                  break;

                case "<k>":
                  track.File.Tag.TrackCount = Convert.ToUInt32(splittedFileValues[index]);
                  break;

                case "<D>":
                  track.File.Tag.Disc = Convert.ToUInt32(splittedFileValues[index]);
                  break;

                case "<d>":
                  track.File.Tag.DiscCount = Convert.ToUInt32(splittedFileValues[index]);
                  break;

                case "<G>":
                  string[] genres = splittedFileValues[index].Split(';');
                  track.File.Tag.Genres = genres;
                  break;

                case "<O>":
                  string[] albumartists = splittedFileValues[index].Split(';');
                  track.File.Tag.AlbumArtists = albumartists;
                  break;

                case "<C>":
                  track.File.Tag.Comment = splittedFileValues[index];
                  break;

                case "<N>":
                  track.File.Tag.Conductor = splittedFileValues[index];
                  break;

                case "<R>":
                  string[] composers = splittedFileValues[index].Split(';');
                  track.File.Tag.Composers = composers;
                  break;

                case "<U>":
                  track.File.Tag.Grouping = splittedFileValues[index];
                  break;

                case "<S>":
                  track.SubTitle = splittedFileValues[index];
                  break;

                case "<M>":
                  track.Interpreter = splittedFileValues[index];
                  break;

                case "<E>":
                  track.File.Tag.BeatsPerMinute = Convert.ToUInt32(splittedFileValues[index]);
                  break;

                case "<X>":
                  // ignore it
                  break;
              }
            }
          }
        }
        catch (Exception ex)
        {
          log.Error("Error applying changes from Filename To Tag: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[1].Value = localisation.ToString("message", "Error");
          _main.TracksGridView.AddErrorMessage(_main.TracksGridView.TrackList[row.Index].File.Name, ex.Message);
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