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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;
#endregion

namespace MPTagThat.Core
{
  public partial class ReadOnlyDialog : ShapedForm
  {
    #region ctor
    public ReadOnlyDialog(string fileName)
    {
      InitializeComponent();

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;

      labelHeader.Text = ServiceScope.Get<ILocalisation>().ToString("readonly", "Header");
      lbFile.Text = fileName;

      ServiceScope.Get<IThemeManager>().NotifyThemeChange();
    }
    #endregion

    #region Events

    /// <summary>
    /// A button was clicked. Dialogresults are set in Form
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button_Click(object sender, EventArgs e)
    {
      Close();
    }

    #endregion
  }
}
