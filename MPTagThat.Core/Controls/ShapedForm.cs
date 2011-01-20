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

namespace MPTagThat.Core
{
  public partial class ShapedForm : Form
  {
    #region Variables

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HT_CAPTION = 0x2;
    private Color _borderColor = Color.DarkGray;
    private int _borderWidth = 3;
    private bool _formDrag;
    private bool _resizeable;

    private Point _sizeOffset = Point.Empty;
    private bool _sizing;

    #endregion

    #region Properties

    /// <summary>
    ///   Gets  / Sets the Border Color
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Browsable(true)]
    [Description("The border Color of the shaped form"), Category("ShapedForm")]
    public Color BorderColor
    {
      get { return _borderColor; }
      set { _borderColor = value; }
    }

    /// <summary>
    ///   Gets / Sets the Border Width
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Browsable(true)]
    [Description("The width of the border of the shaped form"), Category("ShapedForm")]
    public int BorderWidth
    {
      get { return _borderWidth; }
      set { _borderWidth = value; }
    }

    /// <summary>
    ///   Gets / Sets the Resizeable Atribute of the form
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Browsable(true)]
    [Description("Should the shaped form be resizeable"), Category("ShapedForm")]
    public bool Resizeable
    {
      get { return _resizeable; }
      set { _resizeable = value; }
    }

    #endregion

    #region Private Methods

    private GraphicsPath FormShape
    {
      get
      {
        GraphicsPath gp = new GraphicsPath();
        Rectangle r = ClientRectangle;
        int radius = 12;

        gp.AddArc(r.Left, r.Top, radius, radius, 180, 90);
        gp.AddArc(r.Right - radius, r.Top, radius, radius, 270, 90);
        gp.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
        gp.AddArc(r.Left, r.Bottom - radius, radius, radius, 90, 90);
        gp.CloseFigure();

        return gp;
      }
    }

    private Region TitleBar
    {
      get { return new Region(new Rectangle(0, 0, Width, 26)); }
    }

    #endregion

    #region Overrides

    protected override void OnLoad(EventArgs e)
    {
      InitializeComponent();

      if (!base.DesignMode)
      {
        if (_resizeable)
        {
          labelResize.MouseDown += labelResize_MouseDown;
          labelResize.MouseMove += labelResize_MouseMove;
          labelResize.MouseUp += labelResize_MouseUp;
          labelResize.Location = new Point(ClientSize.Width - 21, ClientSize.Height - 18);
          labelResize.Visible = true;
        }
        else
        {
          labelResize.Visible = false;
        }
        Region = new Region(FormShape);
        base.OnLoad(e);
      }
      else
      {
        labelResize.Visible = false;
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (!base.DesignMode)
      {
        Pen BorderPen = new Pen(_borderColor, _borderWidth);
        BorderPen.Alignment = PenAlignment.Inset;
        e.Graphics.DrawPath(BorderPen, FormShape);
        BorderPen.Dispose();
      }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      _formDrag = TitleBar.IsVisible(e.X, e.Y) && e.Button == MouseButtons.Left;

      if (_formDrag)
      {
        Capture = false;
        Message msg = Message.Create(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
        WndProc(ref msg);
      }

      Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
      Region = new Region(FormShape);
    }

    private void labelResize_MouseDown(object sender, MouseEventArgs e)
    {
      _sizing = true;
      _sizeOffset = new Point(Right - Cursor.Position.X, Bottom - Cursor.Position.Y);
    }


    private void labelResize_MouseMove(object sender, MouseEventArgs e)
    {
      if (_sizing)
      {
        //Clip cursor to dissallow sizing of form below 100x100
        Rectangle ClipRectangle = RectangleToScreen(new Rectangle(100, 100, Width, Height));
        ClipRectangle.Offset(_sizeOffset);
        Cursor.Clip = ClipRectangle;
        ClientSize = new Size(Cursor.Position.X + _sizeOffset.X - Location.X,
                              Cursor.Position.Y + _sizeOffset.Y - Location.Y);
        labelResize.Location = new Point(Cursor.Position.X + _sizeOffset.X - Location.X - 21,
                                         Cursor.Position.Y + _sizeOffset.Y - Location.Y - 18);
        Invalidate();
      }
    }

    private void labelResize_MouseUp(object sender, MouseEventArgs e)
    {
      _sizing = false;
      Cursor.Clip = Screen.PrimaryScreen.Bounds;
    }

    #endregion
  }
}