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

using System.Drawing;

#endregion

namespace MPTagThat.Core
{
  public class Theme
  {
    public Font ButtonFont { get; set; }

    public Color ButtonForeColor { get; set; }

    public Color ButtonBackColor { get; set; }

    public Color ChangedForeColor { get; set; }

    public Color ChangedBackColor { get; set; }

    public Color DefaultBackColor { get; set; }

    public Color SelectionBackColor { get; set; }

    public Color AlternatingRowForeColor { get; set; }

    public Color AlternatingRowBackColor { get; set; }


    public Color PanelHeadingDirectionCtrlColor { get; set; }

    public Font PanelHeadingFont { get; set; }

    public Color PanelHeadingBackColor { get; set; }

    public string ThemeName { get; set; }

    public Font LabelFont { get; set; }

    public Color LabelForeColor { get; set; }

    public Font FormHeaderFont { get; set; }

    public Color FormHeaderForeColor { get; set; }

    public Color BackColor { get; set; }

    public Color FixableErrorForeColor { get; set; }

    public Color FixableErrorBackColor { get; set; }

    public Color NonFixableErrorForeColor { get; set; }

    public Color NonFixableErrorBackColor { get; set; }

    public Color FindReplaceForeColor { get; set; }

    public Color FindReplaceBackColor { get; set; }

    #region ctor

    #endregion
  }
}