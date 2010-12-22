#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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

using System.Drawing;
using System.Windows.Forms;

#endregion

namespace MPTagThat.Core.WinControls
{
  public class MPTComboBox : Elegant.Ui.ComboBox
  {
    #region Variables

    public System.Windows.Forms.ComboBoxStyle DropDownStyle;

    #endregion

    #region ctor

    public MPTComboBox()
      : base()
    {
      this.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.AutoCompleteSource = AutoCompleteSource.ListItems;
    }

    #endregion
  }
}