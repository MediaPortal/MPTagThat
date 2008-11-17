using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.GridView
{
  public partial class FreeDBMultiCDSelect : Form
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private string _discID;
    #endregion

    #region Properties
    public ListBox CDList
    {
      get { return listBoxCDMatches; }
    }

    public string DiscID
    {
      get { return _discID; }
    }
    #endregion

    #region ctor
    public FreeDBMultiCDSelect()
    {
      InitializeComponent();

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.Text = localisation.ToString("FreeDB", "Header");
    }
    #endregion

    #region Event Handler
    private void buttonOK_Click(object sender, EventArgs e)
    {
      _discID = (string)listBoxCDMatches.SelectedValue;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void listBoxCDMatches_DoubleClick(object sender, EventArgs e)
    {
      _discID = (string)listBoxCDMatches.SelectedValue;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion
  }
}