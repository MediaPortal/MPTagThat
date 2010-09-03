using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.Dialogues
{
  public partial class ColumnSelect : ShapedForm
  {
    #region Variables
    MPTagThat.GridView.GridViewTracks grid;
    #endregion

    public ColumnSelect(MPTagThat.GridView.GridViewTracks grid)
    {
      InitializeComponent();
      this.grid = grid;

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
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
      this.Close();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}