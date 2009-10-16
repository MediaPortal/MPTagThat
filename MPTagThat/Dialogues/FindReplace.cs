using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.GridView;

namespace MPTagThat.Dialogues
{
  public partial class FindReplace : ShapedForm
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    private Theme theme = ServiceScope.Get<IThemeManager>().CurrentTheme;

    private bool _replace = false;
    private bool _searchStringFound = false;
    private int _curRow = 0;
    private int _curCell = 0;
    private int _curCellFindPos = 0;

    private FindResult _findResult = null;
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
          tabControlFindReplace.SelectedIndex = 1;
        }
      }
    }
    #endregion

    #region ctor
    public FindReplace(Main main)
    {
      _main = main;
      InitializeComponent();

      this.labelHeader.ForeColor = theme.FormHeaderForeColor;
      this.labelHeader.Font = theme.FormHeaderFont;

      if (Options.FindBuffer != null)
      {
        cbFind.Items.AddRange(Options.FindBuffer.ToArray());
      }

      if (Options.ReplaceBuffer != null)
      {
        cbReplace.Items.AddRange((Options.ReplaceBuffer.ToArray()));
      }

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
      if (!checkBoxMatchCase.Checked)
      {
        searchString = searchString.ToLowerInvariant();
      }

      for (int i = _curRow; i < _main.TracksGridView.View.Rows.Count; i++ )
      {
        DataGridViewRow row = _main.TracksGridView.View.Rows[i];
        for (int j = _curCell; j < row.Cells.Count; j++)
        {
          if (!(_main.TracksGridView.View.Columns[j] as DataGridViewColumn).Visible)
          {
            continue;
          }

          string cellContent = row.Cells[j].Value == null ? "" : row.Cells[j].Value.ToString();
          if (!checkBoxMatchCase.Checked)
          {
            cellContent = cellContent.ToLowerInvariant();
          }
          int findPos = cellContent.IndexOf(searchString, _curCellFindPos);
          if (findPos > -1)
          {
            _searchStringFound = true;
            _curCell = j;
            _curRow = i;
            _curCellFindPos = findPos + 1;
            _findResult = new FindResult();
            _findResult.Row = _curRow;
            _findResult.Column = _curCell;
            _findResult.StartPos = findPos;
            _findResult.Length = searchString.Length;
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