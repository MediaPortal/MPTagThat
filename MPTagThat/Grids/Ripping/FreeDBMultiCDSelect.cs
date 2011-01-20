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

#endregion

namespace MPTagThat.GridView
{
  public partial class FreeDBMultiCDSelect : Form
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
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

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      Text = localisation.ToString("FreeDB", "Header");
    }

    #endregion

    #region Event Handler

    private void buttonOK_Click(object sender, EventArgs e)
    {
      _discID = (string)listBoxCDMatches.SelectedValue;
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void listBoxCDMatches_DoubleClick(object sender, EventArgs e)
    {
      _discID = (string)listBoxCDMatches.SelectedValue;
      DialogResult = DialogResult.OK;
      Close();
    }

    #endregion
  }
}