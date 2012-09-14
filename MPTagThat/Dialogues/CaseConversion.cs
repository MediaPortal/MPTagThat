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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat.CaseConversion
{
  public partial class CaseConversion : UserControl
  {
    #region Variables

    private readonly Main _main;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

    private readonly TextInfo textinfo = Thread.CurrentThread.CurrentCulture.TextInfo;

    private string strExcep;

    #endregion

    #region ctor

    public CaseConversion(Main main)
    {
      _main = main;
      InitForm(false);
    }

    public CaseConversion(Main main, bool batchMode)
    {
      _main = main;
      InitForm(batchMode);
    }

    #endregion

    #region Methods

    /// <summary>
    ///   The form is used in Batch Mode, when saving tags
    /// </summary>
    /// <param name = "batchMode"></param>
    private void InitForm(bool batchMode)
    {
      InitializeComponent();

      if (!batchMode)
      {
        BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
        ServiceScope.Get<IThemeManager>().NotifyThemeChange();

        labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
        labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

        LocaliseScreen();
      }

      // Bind the List with the Exceptions to the list box
      listBoxExceptions.DataSource = Options.ConversionSettings.CaseConvExceptions;

      // Load the Settings
      checkBoxConvertFileName.Checked = Options.ConversionSettings.ConvertFileName;
      checkBoxConvertTags.Checked = Options.ConversionSettings.ConvertTags;
      checkBoxArtist.Checked = Options.ConversionSettings.ConvertArtist;
      checkBoxAlbumArtist.Checked = Options.ConversionSettings.ConvertAlbumArtist;
      checkBoxAlbum.Checked = Options.ConversionSettings.ConvertAlbum;
      checkBoxTitle.Checked = Options.ConversionSettings.ConvertTitle;
      checkBoxComment.Checked = Options.ConversionSettings.ConvertComment;
      radioButtonAllLowerCase.Checked = Options.ConversionSettings.ConvertAllLower;
      radioButtonAllUpperCase.Checked = Options.ConversionSettings.ConvertAllUpper;
      radioButtonFirstLetterUpperCase.Checked = Options.ConversionSettings.ConvertFirstUpper;
      radioButtonAllFirstLetterUpperCase.Checked = Options.ConversionSettings.ConvertAllFirstUpper;
      checkBoxReplace20bySpace.Checked = Options.ConversionSettings.Replace20BySpace;
      checkBoxReplaceSpaceby20.Checked = Options.ConversionSettings.ReplaceSpaceBy20;
      checkBoxReplaceSpaceByUnderscore.Checked = Options.ConversionSettings.ReplaceSpaceByUnderscore;
      checkBoxReplaceUnderscoreBySpace.Checked = Options.ConversionSettings.ReplaceUnderscoreBySpace;
      checkBoxAlwaysUpperCaseFirstLetter.Checked = Options.ConversionSettings.ConvertAllWaysFirstUpper;

      tabControlConversion.SelectFirstTab();
    }

    /// <summary>
    ///   Do Case Conversion for the given track.
    ///   Called internally by the Convert button and by the Save clause, if set in Preferences
    /// </summary>
    /// <param name = "track"></param>
    public void CaseConvert(TrackData track, int rowIndex)
    {
      bool bErrors = false;
      // Convert the Filename
      if (checkBoxConvertFileName.Checked)
      {
        string fileName = ConvertCase(Path.GetFileNameWithoutExtension(track.FileName));

        // Now check the length of the filename
        if (fileName.Length > 255)
        {
          log.Debug("Filename too long: {0}", fileName);
          track.Status = 2;
          _main.TracksGridView.AddErrorMessage(_main.TracksGridView.View.Rows[rowIndex],
                                               String.Format("{0}: {1}",
                                                             localisation.ToString("tag2filename", "NameTooLong"),
                                                             fileName));
          bErrors = true;
        }

        if (!bErrors)
        {
          // Now that we have a correct Filename
          track.FileName = string.Format("{0}{1}", fileName, Path.GetExtension(track.FileName));
          track.Changed = true;
          _main.TracksGridView.Changed = true;
          _main.TracksGridView.SetBackgroundColorChanged(rowIndex);
          Options.Songlist[rowIndex] = track;
        }
      }

      // Convert the Tags
      if (checkBoxConvertTags.Checked)
      {
        string strConv = "";
        bool bChanged = false;
        if (checkBoxArtist.Checked)
        {
          strConv = track.Artist;
          bChanged = (strConv = ConvertCase(strConv)) != track.Artist ? true : false || bChanged;
          if (bChanged)
            track.Artist = strConv;
        }
        if (checkBoxAlbumArtist.Checked)
        {
          strConv = track.AlbumArtist;
          bChanged = (strConv = ConvertCase(strConv)) != track.AlbumArtist ? true : false || bChanged;
          if (bChanged)
            track.AlbumArtist = strConv;
        }
        if (checkBoxAlbum.Checked)
        {
          strConv = track.Album;
          bChanged = (strConv = ConvertCase(strConv)) != track.Album ? true : false || bChanged;
          if (bChanged)
            track.Album = strConv;
        }
        if (checkBoxTitle.Checked)
        {
          strConv = track.Title;
          bChanged = (strConv = ConvertCase(strConv)) != track.Title ? true : false || bChanged;
          if (bChanged)
            track.Title = strConv;
        }
        if (checkBoxComment.Checked)
        {
          strConv = track.Comment;
          bChanged = (strConv = ConvertCase(strConv)) != track.Comment ? true : false || bChanged;
          if (bChanged)
            track.Comment = strConv;
        }

        if (bChanged)
        {
          track.Changed = true;
          _main.TracksGridView.Changed = true;
          _main.TracksGridView.SetBackgroundColorChanged(rowIndex);
          Options.Songlist[rowIndex] = track;
        }
      }
    }

    public void CaseConvertSelectedTracks()
    {
      DataGridView tracksGrid = _main.TracksGridView.View;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        CaseConvert(track, row.Index);
      }

      foreach (TrackData track in Options.Songlist)
      {
        if (track.Changed)
          _main.TracksGridView.Changed = true;
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
    }

    private string ConvertCase(string strText)
    {
      if (strText == null || strText == string.Empty)
        return string.Empty;

      if (checkBoxReplace20bySpace.Checked)
        strText = strText.Replace("%20", " ");

      if (checkBoxReplaceSpaceby20.Checked)
        strText = strText.Replace(" ", "%20");

      if (checkBoxReplaceUnderscoreBySpace.Checked)
        strText = strText.Replace("_", " ");

      if (checkBoxReplaceSpaceByUnderscore.Checked)
        strText = strText.Replace(" ", "_");

      if (radioButtonAllLowerCase.Checked)
        strText = strText.ToLowerInvariant();
      else if (radioButtonAllUpperCase.Checked)
        strText = strText.ToUpperInvariant();
      else if (radioButtonFirstLetterUpperCase.Checked)
      {
        // Go to Lowercase first, in case that everything is already uppercase
        strText = strText.ToLowerInvariant();
        strText = strText.Substring(0, 1).ToUpperInvariant() + strText.Substring(1);
      }
      else if (radioButtonAllFirstLetterUpperCase.Checked)
      {
        // Go to Lowercase first, in case that everything is already uppercase
        strText = strText.ToLowerInvariant();
        strText = textinfo.ToTitleCase(strText);
      }

      if (checkBoxAlwaysUpperCaseFirstLetter.Checked)
        strText = strText.Substring(0, 1).ToUpperInvariant() + strText.Substring(1);

      // Handle the Exceptions
      foreach (string excep in Options.ConversionSettings.CaseConvExceptions)
      {
        strExcep = Regex.Escape(excep);
        strText = Regex.Replace(strText, @"(\W|^)" + strExcep + @"(\W|$)", new MatchEvaluator(RegexReplaceCallback),
                                RegexOptions.Singleline | RegexOptions.IgnoreCase);
      }

      return strText;
    }

    /// <summary>
    ///   Callback Method for every Match of the Regex
    /// </summary>
    /// <param name = "Match"></param>
    /// <returns></returns>
    private string RegexReplaceCallback(Match Match)
    {
      strExcep = strExcep.Replace(@"\\", "\x0001").Replace(@"\", "").Replace("\x0001", @"\");
      return Util.ReplaceEx(Match.Value, strExcep, strExcep);
    }

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      labelHeader.Text = localisation.ToString("CaseConversion", "Header");
    }

    #endregion

    #endregion

    #region Event Handler

    /// <summary>
    ///   Do the Conversion
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonConvert_Click(object sender, EventArgs e)
    {
      CaseConvertSelectedTracks();

      // Save the settings
      Options.ConversionSettings.ConvertFileName = checkBoxConvertFileName.Checked;
      Options.ConversionSettings.ConvertTags = checkBoxConvertTags.Checked;
      Options.ConversionSettings.ConvertArtist = checkBoxArtist.Checked;
      Options.ConversionSettings.ConvertAlbumArtist = checkBoxAlbumArtist.Checked;
      Options.ConversionSettings.ConvertAlbum = checkBoxAlbum.Checked;
      Options.ConversionSettings.ConvertTitle = checkBoxTitle.Checked;
      Options.ConversionSettings.ConvertComment = checkBoxComment.Checked;
      Options.ConversionSettings.ConvertAllLower = radioButtonAllLowerCase.Checked;
      Options.ConversionSettings.ConvertAllUpper = radioButtonAllUpperCase.Checked;
      Options.ConversionSettings.ConvertFirstUpper = radioButtonFirstLetterUpperCase.Checked;
      Options.ConversionSettings.ConvertAllFirstUpper = radioButtonAllFirstLetterUpperCase.Checked;
      Options.ConversionSettings.Replace20BySpace = checkBoxReplace20bySpace.Checked;
      Options.ConversionSettings.ReplaceSpaceBy20 = checkBoxReplaceSpaceby20.Checked;
      Options.ConversionSettings.ReplaceSpaceByUnderscore = checkBoxReplaceSpaceByUnderscore.Checked;
      Options.ConversionSettings.ReplaceUnderscoreBySpace = checkBoxReplaceUnderscoreBySpace.Checked;
      Options.ConversionSettings.ConvertAllWaysFirstUpper = checkBoxAlwaysUpperCaseFirstLetter.Checked;

      _main.ShowTagEditPanel(true);
      Dispose();
    }

    /// <summary>
    ///   Cancel Has been pressed. Close Form
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      _main.ShowTagEditPanel(true);
      Dispose();
    }

    /// <summary>
    ///   Add the Exception to the List
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonAddException_Click(object sender, EventArgs e)
    {
      foreach (string exc in Options.ConversionSettings.CaseConvExceptions)
      {
        if (exc == tbException.Text.Trim())
          return;
      }
      Options.ConversionSettings.CaseConvExceptions.Add(tbException.Text.Trim());
      tbException.Text = string.Empty;

      // Refresh the Listbox
      listBoxExceptions.DataSource = null;
      listBoxExceptions.DataSource = Options.ConversionSettings.CaseConvExceptions;
      listBoxExceptions.Refresh();
    }

    /// <summary>
    ///   Remove the selected Exception from the List
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonRemoveException_Click(object sender, EventArgs e)
    {
      int index = listBoxExceptions.SelectedIndex;
      if (index > -1)
      {
        Options.ConversionSettings.CaseConvExceptions.RemoveAt(index);

        // Refresh the Listbox
        listBoxExceptions.DataSource = null;
        listBoxExceptions.DataSource = Options.ConversionSettings.CaseConvExceptions;
        listBoxExceptions.Refresh();
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
        buttonConvert_Click(null, new EventArgs());
        return true;
      }
      else if ((int)keyData == 27)  // Handle Escape to Close the form
      {
        buttonCancel_Click(null, new EventArgs());
        return true;
      }


      return base.ProcessCmdKey(ref msg, keyData);
    }
    #endregion
  }
}