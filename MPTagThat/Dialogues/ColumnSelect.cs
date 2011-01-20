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
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.GridView;

#endregion

namespace MPTagThat.Dialogues
{
  public partial class ColumnSelect : ShapedForm
  {
    #region Variables

    private readonly GridViewTracks grid;

    #endregion

    public ColumnSelect(GridViewTracks grid)
    {
      InitializeComponent();
      this.grid = grid;

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();
    }

    private void OnLoad(object sender, EventArgs e)
    {
      int i = -1;
      foreach (DataGridViewColumn column in grid.View.Columns)
      {
        i++;
        // Don't allow FileName and Status to be selected
        if (i < 2)
          continue;
        // Don't add the Dummmy Column
        if (i == grid.View.Columns.Count - 1)
          break;

        ListViewItem lvItem = new ListViewItem();
        lvItem.Checked = column.Visible;
        lvItem.Text = column.HeaderText;
        lvColumns.Items.Add(lvItem);
      }
    }

    private void btOk_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < lvColumns.Items.Count; i++)
      {
        if (lvColumns.Items[i].Checked)
          grid.View.Columns[i + 2].Visible = true;
        else
          grid.View.Columns[i + 2].Visible = false;
      }
      grid.Refresh();
      Close();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}