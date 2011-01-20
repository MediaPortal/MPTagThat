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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

namespace Stepi.UI
{
  /// <summary>
  ///   A class that provides support for rounded/normal borders
  /// </summary>
  public class CornerCtrl : BufferPaintingCtrl
  {
    #region Members

    /// <summary>
    ///   Color used in drawing the border
    /// </summary>
    protected Color borderColor = Color.Gray;

    /// <summary>
    ///   used to create the rectangular that defines the region for the ellipse whose arc is being drawn
    /// </summary>
    protected int cornerSquare;

    /// <summary>
    ///   corner type
    /// </summary>
    protected CornerStyle cornerStyle = CornerStyle.Rounded;

    /// <summary>
    ///   The graphic path
    /// </summary>
    protected GraphicsPath graphicPath;

    protected GraphicsPath regionPath;

    #endregion

    #region Protected

    /// <summary>
    ///   Creates the graphic path used for drawing the border
    /// </summary>
    protected virtual void InitializeGraphicPath()
    {
      if (null != graphicPath)
      {
        graphicPath.Dispose();
        graphicPath = null;
      }

      if (null != regionPath)
      {
        regionPath.Dispose();
        regionPath = null;
      }

      graphicPath = new GraphicsPath();
      regionPath = new GraphicsPath();

      switch (cornerStyle)
      {
        case CornerStyle.Rounded:

          graphicPath.AddArc(0, 0, cornerSquare, cornerSquare, 180, 90);
          regionPath.AddArc(0, 0, cornerSquare, cornerSquare, 180, 90);
          graphicPath.AddLine(cornerSquare - cornerSquare / 2, 0, Width - cornerSquare + cornerSquare / 2 - 1, 0);
          regionPath.AddLine(cornerSquare - cornerSquare / 2, 0, Width - cornerSquare + cornerSquare / 2, 0);
          graphicPath.AddArc(Width - cornerSquare - 1, 0, cornerSquare, cornerSquare, -90, 90);
          regionPath.AddArc(Width - cornerSquare, 0, cornerSquare, cornerSquare, -90, 90);

          graphicPath.AddLine(Width - 1, cornerSquare - cornerSquare / 2, Width - 1,
                              Height - cornerSquare + cornerSquare / 2);
          regionPath.AddLine(Width, cornerSquare - cornerSquare / 2, Width, Height - cornerSquare + cornerSquare / 2);
          graphicPath.AddArc(Width - cornerSquare - 1, Height - 1 - cornerSquare, cornerSquare, cornerSquare, 0, 90);
          regionPath.AddArc(Width - cornerSquare, Height - cornerSquare, cornerSquare, cornerSquare, 0, 90);
          graphicPath.AddLine(cornerSquare - cornerSquare / 2, Height - 1, Width - cornerSquare + cornerSquare / 2,
                              Height - 1);
          regionPath.AddLine(cornerSquare - cornerSquare / 2, Height, Width - cornerSquare + cornerSquare / 2, Height);

          graphicPath.AddArc(0, Height - cornerSquare - 1, cornerSquare, cornerSquare, 90, 90);
          regionPath.AddArc(0, Height - cornerSquare, cornerSquare, cornerSquare, 90, 90);

          graphicPath.AddLine(0, cornerSquare - cornerSquare / 2, 0, Height - cornerSquare + cornerSquare / 2);
          regionPath.AddLine(0, cornerSquare - cornerSquare / 2, 0, Height - cornerSquare + cornerSquare / 2);
          //this.Region = new Region(graphicPath);

          //this.Region = new Region(graphicPath);
          break;

        case CornerStyle.Normal:

          graphicPath.AddLine(0, 0, Width - 1, 0);
          regionPath.AddLine(0, 0, Width, 0);
          graphicPath.AddLine(Width - 1, 0, Width - 1, Height - 1);
          regionPath.AddLine(Width, 0, Width, Height);
          graphicPath.AddLine(Width - 1, Height - 1, 0, Height - 1);
          regionPath.AddLine(Width, Height, 0, Height);
          graphicPath.AddLine(0, Height - 1, 0, 0);
          regionPath.AddLine(0, Height, 0, 0);
          break;

        default:
          throw new ApplicationException("Unrecognized style for rendering the corners");
          break;
      }
    }

    #endregion

    #region Public

    /// <summary>
    ///   Need to override this as it needs to recreate the graphicPath created
    /// </summary>
    public override void Refresh()
    {
      if (null != graphicPath)
      {
        graphicPath.Dispose();
        InitializeGraphicPath();
      }
      base.Refresh();
    }

    #endregion

    #region Properties

    [Category("Behavior")]
    [Description("Set/Get the color used to draw the borders")]
    public Color BorderColor
    {
      get { return borderColor; }
      set
      {
        borderColor = value;
        Refresh();
      }
    }

    [Category("Behavior")]
    [DefaultValue(CornerStyle.Rounded)]
    [Description("Set/Get the style used for rendering the corners")]
    public CornerStyle CornerStyle
    {
      get { return cornerStyle; }
      set
      {
        if (value != cornerStyle)
        {
          cornerStyle = value;
          foreach (Control control in Controls)
          {
            if (control is CornerCtrl)
            {
              (control as CornerCtrl).CornerStyle = cornerStyle;
              control.Refresh();
            }
          }
          Refresh();
        }
      }
    }

    #endregion
  }
}