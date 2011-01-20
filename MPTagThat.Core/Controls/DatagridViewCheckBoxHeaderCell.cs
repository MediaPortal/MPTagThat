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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

namespace MPTagThat.Core
{
  public delegate void CheckBoxClickedHandler(bool state);

  public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
  {
    private readonly bool _bChecked;

    public DataGridViewCheckBoxHeaderCellEventArgs(bool bChecked)
    {
      _bChecked = bChecked;
    }

    public bool Checked
    {
      get { return _bChecked; }
    }
  }

  public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
  {
    private CheckBoxState _cbState = CheckBoxState.UncheckedNormal;
    private Point _cellLocation;
    private bool _checked;
    private Point checkBoxLocation;
    private Size checkBoxSize;

    public bool Checked
    {
      get { return _checked; }
      set { _checked = value; }
    }

    public event CheckBoxClickedHandler OnCheckBoxClicked;

    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                                  DataGridViewElementStates dataGridViewElementState, object value,
                                  object formattedValue,
                                  string errorText, DataGridViewCellStyle cellStyle,
                                  DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                  DataGridViewPaintParts paintParts
      )
    {
      base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                 dataGridViewElementState, value,
                 formattedValue, errorText, cellStyle,
                 advancedBorderStyle, paintParts);
      Point p = new Point();
      Size s = CheckBoxRenderer.GetGlyphSize(graphics,
                                             CheckBoxState.UncheckedNormal);
      p.X = cellBounds.Location.X +
            (cellBounds.Width / 2) - (s.Width / 2);
      p.Y = cellBounds.Location.Y +
            (cellBounds.Height / 2) - (s.Height / 2);
      _cellLocation = cellBounds.Location;
      checkBoxLocation = p;
      checkBoxSize = s;
      if (_checked)
        _cbState = CheckBoxState.CheckedNormal;
      else
        _cbState = CheckBoxState.UncheckedNormal;
      CheckBoxRenderer.DrawCheckBox
        (graphics, checkBoxLocation, _cbState);
    }

    protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
    {
      Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
      if (p.X >= checkBoxLocation.X && p.X <=
                                       checkBoxLocation.X + checkBoxSize.Width
          && p.Y >= checkBoxLocation.Y && p.Y <=
          checkBoxLocation.Y + checkBoxSize.Height)
      {
        _checked = !_checked;
        if (OnCheckBoxClicked != null)
        {
          OnCheckBoxClicked(_checked);
          DataGridView.InvalidateCell(this);
        }
      }
      base.OnMouseClick(e);
    }
  }
}