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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

#endregion

namespace Stepi.UI
{
  /// <summary>
  ///   Delegate for the dragging event.By dragging the caption you tell the panel to reposition itself
  /// </summary>
  /// <param name = "sender">the object delegating the process, an instance of a CaptionCtrl</param>
  /// <param name = "e">instance of an object holding the event data</param>
  public delegate void CaptionDraggingEvent(object sender, CaptionDraggingEventArgs e);

  /// <summary>
  ///   This defines the caption bar used for the ExtendedPanel
  /// </summary>
  internal class CaptionCtrl : CornerCtrl
  {
    #region Members

    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private readonly IContainer components;

    /// <summary>
    /// </summary>
    private Brush brush;

    /// <summary>
    /// </summary>
    private BrushType brushType = BrushType.Gradient;

    /// <summary>
    ///   Instance of the image to be displayed near the text of the text
    /// </summary>
    private Icon captionIcon;

    /// <summary>
    /// </summary>
    private Color colorOne = Color.White;

    /// <summary>
    /// </summary>
    private Color colorTwo = Color.FromArgb(155, Color.Orange);

    /// <summary>
    /// </summary>
    private DirectionCtrl directionCtrl;

    /// <summary>
    ///   Boolean value specifing if the mouse button is down
    /// </summary>
    private bool mouseDown;

    /// <summary>
    /// </summary>
    private int mouseX;

    private int mouseY;

    /// <summary>
    ///   Instance of the string being drawn
    /// </summary>
    private string text = "Caption";

    /// <summary>
    /// </summary>
    private Color textColor = Color.Black;


    /// <summary>
    /// </summary>
    public event CaptionDraggingEvent Dragging = null;

    #endregion

    #region ctor

    public CaptionCtrl()
    {
      InitializeComponent();

      //set up the mouse move event in case the style is to move the parent control
      MouseMove += OnMouseMoveEvent;

      //set up the mouse down event
      MouseDown += OnMouseDownEvent;

      //set up the mouse up event
      MouseUp += OnMouseUpEvent;

      //set up the brush
      InitializeBrush();
    }

    #endregion

    #region Properties

    [Browsable(false)]
    public Color DirectionCtrlColor
    {
      get { return directionCtrl.Color; }
      set { directionCtrl.Color = value; }
    }

    [Browsable(false)]
    public Color DirectionCtrlHoverColor
    {
      get { return directionCtrl.HoverColor; }
      set { directionCtrl.HoverColor = value; }
    }


    [Category("Appearance")]
    [Description("Get or set the image to be displayed in the caption")]
    public Icon CaptionIcon
    {
      get { return captionIcon; }

      set
      {
        captionIcon = value;
        Update();
      }
    }

    [Category("Appearance")]
    [Description("Get/Set the text to be displayed")]
    public string CaptionText
    {
      get { return text; }
      set
      {
        text = value;
        Update();
      }
    }

    [Category("Appearance")]
    [Description("The font for this control also used to draw the text in the caption")]
    public Font CaptionFont
    {
      get { return Font; }
      set
      {
        Font = value;
        Refresh();
      }
    }

    [Category("Appearance")]
    [Description("The starting color for the gradient brush")]
    public Color ColorOne
    {
      get { return colorOne; }
      set
      {
        colorOne = value;
        InitializeBrush();
        Refresh();
      }
    }

    [Category("Appearance")]
    [Description("The end color for the gradient brush ")]
    public Color ColorTwo
    {
      get { return colorTwo; }
      set
      {
        colorTwo = value;
        InitializeBrush();
        Refresh();
      }
    }

    [Category("Appearance")]
    [Description("The color used for drawing caption text ")]
    public Color TextColor
    {
      get { return textColor; }
      set
      {
        textColor = value;
        Refresh();
      }
    }

    [Category("Appearance")]
    [Description("The brush used in painting the caption")]
    public BrushType BrushType
    {
      get { return brushType; }
      set
      {
        if (value != brushType)
        {
          brushType = value;
          InitializeBrush();
          Refresh();
        }
      }
    }

    #endregion

    #region Protected

    /// <summary>
    ///   Clean up any resources being used.
    /// </summary>
    /// <param name = "disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    /// <summary>
    ///   Set up the graphic path used for drawing the borders
    /// </summary>
    protected override void InitializeGraphicPath()
    {
      cornerSquare = (int)(Height > Width ? Height * 0.05f : Width * 0.05f); // Width * 0.25f;
      base.InitializeGraphicPath();
    }

    #endregion

    #region Private

    /// <summary>
    /// </summary>
    private void InitializeComponent()
    {
      directionCtrl = new DirectionCtrl();
      SuspendLayout();

      Name = "CaptionCtrl";


      directionCtrl.Width = (Height);
      directionCtrl.Height = Height;
      //System.Windows.Forms.MessageBox.Show((this.Width-this.Height).ToString());
      directionCtrl.Location = new Point((Width - Height), 0);
      directionCtrl.Anchor = AnchorStyles.Left | AnchorStyles.Top;
      directionCtrl.Name = "directionCtrl";

      Controls.Add(directionCtrl);
      ResumeLayout(false);
    }

    /// <summary>
    ///   Set up the brush used in filling this control
    /// </summary>
    private void InitializeBrush()
    {
      if (null != brush)
      {
        brush.Dispose();
      }
      if (brushType == BrushType.Solid)
      {
        brush = new SolidBrush(colorOne);
      }
      else
      {
        int width = Width;
        int height = Height;
        if (width == 0)
        {
          width = 1;
        }
        if (height == 0)
        {
          height = 1;
        }
        if (directionCtrl.DirectionStyle == DirectionStyle.Up || directionCtrl.DirectionStyle == DirectionStyle.Down)
        {
          brush = new LinearGradientBrush(new Rectangle(0, 0, width, height), colorOne, colorTwo,
                                          LinearGradientMode.Vertical);
        }
        else
        {
          brush = new LinearGradientBrush(new Rectangle(0, 0, width, height), colorOne, colorTwo,
                                          LinearGradientMode.Horizontal);
        }
      }
    }

    #endregion

    #region Public

    /// <summary>
    ///   Set the handler for the event raised by the "click" control once it is pressed
    /// </summary>
    /// <param name = "handler">instance of an handler for the event </param>
    public void SetStyleChangedHandler(DirectionCtrlStyleChangedEvent handler)
    {
      directionCtrl.handlerStyleChange += handler;
    }

    /// <summary>
    ///   Checks to see if dragging is enabled.Used by ExtendedPanel to add/remove the handler
    /// </summary>
    public bool IsDraggingEnabled()
    {
      return (Dragging != null);
    }

    /// <summary>
    ///   Set the direction style for the "click" control
    /// </summary>
    /// <param name = "style">the new direction style</param>
    public void SetDirectionStyle(DirectionStyle style)
    {
      directionCtrl.DirectionStyle = style;
    }

    #endregion

    #region WM_PAINT

    /// <summary>
    ///   Method for handling the WM_PAINT message
    /// </summary>
    /// <param name = "e">instance of the object holding the event data</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      //set up some flags
      e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
      e.Graphics.InterpolationMode = InterpolationMode.High;
      e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
      e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

      //check to see if the path has been initialized
      if (graphicPath == null)
      {
        InitializeGraphicPath();
      }
      //draw the border
      e.Graphics.DrawPath(new Pen(borderColor), graphicPath);

      //paint the background
      e.Graphics.FillPath(brush, graphicPath);

      //measure text length
      StringFormat stringFormat = null;
      float xAxis = 0;
      float yAxis = 0;
      if (Width >= Height)
      {
        stringFormat = new StringFormat();
        SizeF size = e.Graphics.MeasureString(text, Font, new PointF(0, 0), stringFormat);
        yAxis = (Height - size.Height) * 0.5f;
      }
      else
      {
        stringFormat = new StringFormat(StringFormatFlags.DirectionVertical);
        SizeF size = e.Graphics.MeasureString(text, Font, new PointF(0, 0), stringFormat);
        xAxis = (Width - size.Width) * 0.5f;
      }

      //draw the image if it has been set up
      if (captionIcon != null)
      {
        if (Width >= Height)
        {
          e.Graphics.DrawIcon(captionIcon, cornerSquare / 6, cornerSquare / 6); //, cornerSquare, cornerSquare);
          //draw the text
          e.Graphics.DrawString(text, Font, new SolidBrush(textColor),
                                new PointF(xAxis + cornerSquare / 6 + captionIcon.Width, yAxis + cornerSquare / 6),
                                stringFormat);
        }
        else
        {
          e.Graphics.DrawIcon(captionIcon, cornerSquare / 6, cornerSquare / 6);
          //draw the text
          e.Graphics.DrawString(text, Font, new SolidBrush(textColor),
                                new PointF(xAxis + cornerSquare / 6, yAxis + cornerSquare / 6 + +captionIcon.Height),
                                stringFormat);
        }
      }
      else
      {
        //draw the text
        e.Graphics.DrawString(text, Font, new SolidBrush(textColor),
                              new PointF(xAxis + cornerSquare / 6, yAxis + cornerSquare / 6), stringFormat);
      }
    }

    #endregion

    #region Events

    /// <summary>
    ///   Event raised when the mouse is moved
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void OnMouseMoveEvent(object sender, MouseEventArgs e)
    {
      //is the mouse down
      if (mouseDown && Dragging != null)
      {
        if (mouseX != e.X || mouseY != e.Y)
        {
          int width = mouseX - e.X;
          int height = mouseY - e.Y;
          //raise the event
          Dragging(this, new CaptionDraggingEventArgs(width, height));
        }
      }
    }

    /// <summary>
    ///   Event raised when one of the mouse buttons is pressed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void OnMouseDownEvent(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        mouseDown = true;
        //save mouse coordinates
        mouseX = e.X;
        mouseY = e.Y;
      }
    }

    /// <summary>
    ///   Event raised when the mouse button being pressed is released
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void OnMouseUpEvent(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        mouseDown = false;
        //reset mouse X,Y coordinates
        mouseX = 0;
        mouseY = 0;
      }
    }

    #endregion

    #region Override

    /// <summary>
    ///   Override the OnResize because it needs to set up the location of the "direction" control and the size of that control
    /// </summary>
    /// <param name = "e"></param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      //need to recreate the graphicpath for the border
      InitializeGraphicPath();

      if (Width >= Height)
      {
        directionCtrl.Location = new Point((Width - Height), 0);
        directionCtrl.Width = (Height);
        directionCtrl.Height = Height;
      }
      else
      {
        directionCtrl.Location = new Point(0, (Height - Width));
        directionCtrl.Width = Width;
        directionCtrl.Height = Width;
      }
      //set the brush
      InitializeBrush();
      Region = new Region(regionPath);
    }

    #endregion
  }
}