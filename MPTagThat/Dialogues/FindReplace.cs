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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.GridView;

#endregion

namespace MPTagThat.Dialogues
{
  public partial class FindReplace : ShapedForm
  {
    #region Variables

    private readonly Main _main;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly Theme theme = ServiceScope.Get<IThemeManager>().CurrentTheme;

    private int _curCell;
    private int _curCellFindPos;
    private int _curRow;

    private FindResult _findResult;
    private bool _replace;
    private bool _searchStringFound;
    private ILogger log = ServiceScope.Get<ILogger>();

    #endregion

    #region Properties

    public bool Replace
    {
      get { return _replace; }
      set
      {
        _replace = value;
        LocaliseScreen();
        if (_replace)
        {
          tabControlFindReplace.SelectedTabPage = tabPageReplace;
        }
      }
    }

    #endregion

    #region ctor

    public FindReplace(Main main)
    {
      _main = main;
      InitializeComponent();

      labelHeader.ForeColor = theme.FormHeaderForeColor;
      labelHeader.Font = theme.FormHeaderFont;

      if (Options.FindBuffer != null)
      {
        cbFind.Items.AddRange(Options.FindBuffer.ToArray());
      }

      if (Options.ReplaceBuffer != null)
      {
        cbReplace.Items.AddRange((Options.ReplaceBuffer.ToArray()));
      }

      LocaliseScreen();

      tabControlFindReplace.SelectedTabPage = tabPageFind;
    }

    #endregion

    #region Methods

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      if (_replace)
      {
        labelHeader.Text = localisation.ToString("FindReplace", "HeaderReplace");
      }
      else
      {
        labelHeader.Text = localisation.ToString("FindReplace", "HeaderFind");
      }
    }

    #endregion

    private bool FindString()
    {
      string searchString = cbFind.Text;

      if (checkBoxMatchWholeWords.Checked)
      {
        searchString = string.Format(@"\b{0}\b", searchString);
      }

      RegexOptions searchOptions = RegexOptions.CultureInvariant;
      if (!checkBoxMatchCase.Checked)
      {
        searchOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      }


      for (int i = _curRow; i < _main.TracksGridView.View.Rows.Count; i++)
      {
        DataGridViewRow row = _main.TracksGridView.View.Rows[i];
        for (int j = _curCell; j < row.Cells.Count; j++)
        {
          if (!(_main.TracksGridView.View.Columns[j]).Visible)
          {
            continue;
          }

          string cellContent = row.Cells[j].Value == null ? "" : row.Cells[j].Value.ToString();
          cellContent = cellContent.Substring(_curCellFindPos);

          MatchCollection regexMatches = Regex.Matches(cellContent, searchString, searchOptions);
          if (regexMatches.Count > 0)
          {
            _searchStringFound = true;
            _curCell = j;
            _curRow = i;
            _curCellFindPos = regexMatches[0].Index + 1;
            _findResult = new FindResult();
            _findResult.Row = _curRow;
            _findResult.Column = _curCell;
            _findResult.StartPos = regexMatches[0].Index;
            _findResult.Length = regexMatches[0].Length;
            _main.TracksGridView.ResultFind = _findResult;

            // Position Cell and deselect row, so that we have our find color
            _main.TracksGridView.View.CurrentCell = _main.TracksGridView.View[j, i];
            _main.TracksGridView.View.Rows[i].Selected = false;
            // Repaint the cell 
            _main.TracksGridView.View.InvalidateCell(_main.TracksGridView.View.CurrentCell);
            return true;
          }
          _curCellFindPos = 0;
        }
        _curCell = 0;
      }
      return false;
    }

    private void ReplaceText()
    {
      if (_main.TracksGridView.View.Columns[_findResult.Column].ReadOnly)
      {
        _findResult = null;
        return;
      }

      string cellContent = _main.TracksGridView.View.Rows[_findResult.Row].Cells[_findResult.Column].Value.ToString();
      string replaceString = cellContent.Substring(0, _findResult.StartPos);
      replaceString += cbReplace.Text;
      replaceString += cellContent.Substring(_findResult.StartPos + _findResult.Length);
      _main.TracksGridView.View.Rows[_findResult.Row].Cells[_findResult.Column].Value = replaceString;
      _findResult = null;
    }

    private void MaintainFindReplaceBuffer()
    {
      bool found = false;
      if (cbFind.Text.Length > 0)
      {
        foreach (string item in cbFind.Items)
        {
          if (item == cbFind.Text)
          {
            found = true;
            break;
          }
        }
        if (!found)
        {
          cbFind.Items.Insert(0, cbFind.Text);
        }
      }

      found = false;
      if (cbReplace.Text.Length > 0)
      {
        foreach (string item in cbReplace.Items)
        {
          if (item == cbReplace.Text)
          {
            found = true;
            break;
          }
        }
        if (!found)
        {
          cbReplace.Items.Insert(0, cbReplace.Text);
        }
      }
    }

    #endregion

    #region Events

    private void OnFormClosed(object sender, FormClosedEventArgs e)
    {
      _main.TracksGridView.ResultFind = null;
    }

    private void buttonClose_Click(object sender, EventArgs e)
    {
      Options.FindBuffer = new ArrayList(cbFind.Items);
      Options.ReplaceBuffer = new ArrayList(cbReplace.Items);
      Close();
    }

    private void buttonFindNext_Click(object sender, EventArgs e)
    {
      if (!FindString())
      {
        if (!_searchStringFound)
        {
          MessageBox.Show(localisation.ToString("FindReplace", "NotFound"), "", MessageBoxButtons.OK);
        }
        else
        {
          MessageBox.Show(localisation.ToString("FindReplace", "NoMoreOccurencesFound"), "", MessageBoxButtons.OK);
          _curRow = 0;
          _curCell = 0;
          _searchStringFound = false;
        }
      }
      MaintainFindReplaceBuffer();
    }

    private void buttonReplace_Click(object sender, EventArgs e)
    {
      if (_findResult == null)
      {
        buttonFindNext_Click(this, new EventArgs());
        return;
      }

      ReplaceText();
    }


    private void buttonReplaceAll_Click(object sender, EventArgs e)
    {
      while (FindString())
      {
        ReplaceText();
      }
      MaintainFindReplaceBuffer();
    }

    #endregion
  }
}