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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

namespace NJFLib.Controls
{

  #region Enums

  /// <summary>
  ///   Enumeration to sepcify the visual style to be applied to the CollapsibleSplitter control
  /// </summary>
  public enum VisualStyles
  {
    Mozilla = 0,
    XP,
    Win9x,
    DoubleDots,
    Lines
  }

  /// <summary>
  ///   Enumeration to specify the current animation state of the control.
  /// </summary>
  public enum SplitterState
  {
    Collapsed = 0,
    Expanding,
    Expanded,
    Collapsing
  }

  #endregion

  /// <summary>
  ///   A custom collapsible splitter that can resize, hide and show associated form controls
  /// </summary>
  [ToolboxBitmap(typeof (CollapsibleSplitter))]
  [Designer(typeof (CollapsibleSplitterDesigner))]
  public class CollapsibleSplitter : Splitter
  {
    #region Private Properties

    // declare and define some base properties
    private readonly Timer animationTimer;
    private readonly Color hotColor = CalculateColor(SystemColors.Highlight, SystemColors.Window, 70);
    private int animationDelay = 20;
    private int animationStep = 20;

    // Border added in version 1.3
    private Border3DStyle borderStyle = Border3DStyle.Flat;

    // animation controls introduced in version 1.22
    private int controlHeight;
    private Control controlToHide;
    private int controlWidth;
    private SplitterState currentState;
    private bool expandParentForm;
    private bool hot;
    private Form parentForm;
    private int parentFormHeight;
    private int parentFormWidth;
    private Rectangle rr;
    private bool useAnimations;
    private VisualStyles visualStyle;

    #endregion

    #region Public Properties

    /// <summary>
    ///   The initial state of the Splitter. Set to True if the control to hide is not visible by default
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue("False"),
     Description("The initial state of the Splitter. Set to True if the control to hide is not visible by default")]
    public bool IsCollapsed
    {
      get
      {
        if (controlToHide != null)
          return !controlToHide.Visible;
        else
          return true;
      }
    }

    /// <summary>
    ///   The System.Windows.Forms.Control that the splitter will collapse
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue(""),
     Description("The System.Windows.Forms.Control that the splitter will collapse")]
    public Control ControlToHide
    {
      get { return controlToHide; }
      set { controlToHide = value; }
    }

    /// <summary>
    ///   Determines if the collapse and expanding actions will be animated
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue("True"),
     Description("Determines if the collapse and expanding actions will be animated")]
    public bool UseAnimations
    {
      get { return useAnimations; }
      set { useAnimations = value; }
    }

    /// <summary>
    ///   The delay in millisenconds between animation steps
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue("20"),
     Description("The delay in millisenconds between animation steps")]
    public int AnimationDelay
    {
      get { return animationDelay; }
      set { animationDelay = value; }
    }

    /// <summary>
    ///   The amount of pixels moved in each animation step
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue("20"),
     Description("The amount of pixels moved in each animation step")]
    public int AnimationStep
    {
      get { return animationStep; }
      set { animationStep = value; }
    }

    /// <summary>
    ///   When true the entire parent form will be expanded and collapsed, otherwise just the contol to expand will be changed
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue("False"),
     Description(
       "When true the entire parent form will be expanded and collapsed, otherwise just the contol to expand will be changed"
       )]
    public bool ExpandParentForm
    {
      get { return expandParentForm; }
      set { expandParentForm = value; }
    }

    /// <summary>
    ///   The visual style that will be painted on the control
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue("VisualStyles.XP"),
     Description("The visual style that will be painted on the control")]
    public VisualStyles VisualStyle
    {
      get { return visualStyle; }
      set
      {
        visualStyle = value;
        Invalidate();
      }
    }

    /// <summary>
    ///   An optional border style to paint on the control. Set to Flat for no border
    /// </summary>
    [Bindable(true), Category("Collapsing Options"), DefaultValue("System.Windows.Forms.Border3DStyle.Flat"),
     Description("An optional border style to paint on the control. Set to Flat for no border")]
    public Border3DStyle BorderStyle3D
    {
      get { return borderStyle; }
      set
      {
        borderStyle = value;
        Invalidate();
      }
    }

    #endregion

    #region Public Methods

    public void ToggleState()
    {
      ToggleSplitter();
    }

    #endregion

    #region Constructor

    public CollapsibleSplitter()
    {
      // Register mouse events
      Click += OnClick;
      Resize += OnResize;
      MouseLeave += OnMouseLeave;
      MouseMove += OnMouseMove;

      // Setup the animation timer control
      animationTimer = new Timer();
      animationTimer.Interval = animationDelay;
      animationTimer.Tick += animationTimerTick;
    }

    #endregion

    #region Overrides

    protected override void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);
      parentForm = FindForm();

      // set the current state
      if (controlToHide != null)
      {
        if (controlToHide.Visible)
        {
          currentState = SplitterState.Expanded;
        }
        else
        {
          currentState = SplitterState.Collapsed;
        }
      }
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
      base.OnEnabledChanged(e);
      Invalidate();
    }

    #endregion

    #region Event Handlers

    protected override void OnMouseDown(MouseEventArgs e)
    {
      // if the hider control isn't hot, let the base resize action occur
      if (controlToHide != null)
      {
        if (!hot && controlToHide.Visible)
        {
          base.OnMouseDown(e);
        }
      }
    }

    private void OnResize(object sender, EventArgs e)
    {
      Invalidate();
    }

    // this method was updated in version 1.11 to fix a flickering problem
    // discovered by John O'Byrne
    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      // check to see if the mouse cursor position is within the bounds of our control
      if (e.X >= rr.X && e.X <= rr.X + rr.Width && e.Y >= rr.Y && e.Y <= rr.Y + rr.Height)
      {
        if (!hot)
        {
          hot = true;
          Cursor = Cursors.Hand;
          Invalidate();
        }
      }
      else
      {
        if (hot)
        {
          hot = false;
          Invalidate();
          ;
        }

        Cursor = Cursors.Default;

        if (controlToHide != null)
        {
          if (!controlToHide.Visible)
            Cursor = Cursors.Default;
          else // Changed in v1.2 to support Horizontal Splitters
          {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
              Cursor = Cursors.VSplit;
            }
            else
            {
              Cursor = Cursors.HSplit;
            }
          }
        }
      }
    }

    private void OnMouseLeave(object sender, EventArgs e)
    {
      // ensure that the hot state is removed
      hot = false;
      Invalidate();
      ;
    }

    private void OnClick(object sender, EventArgs e)
    {
      if (controlToHide != null && hot &&
          currentState != SplitterState.Collapsing &&
          currentState != SplitterState.Expanding)
      {
        ToggleSplitter();
      }
    }

    private void ToggleSplitter()
    {
      // if an animation is currently in progress for this control, drop out
      if (currentState == SplitterState.Collapsing || currentState == SplitterState.Expanding)
        return;

      controlWidth = controlToHide.Width;
      controlHeight = controlToHide.Height;

      if (controlToHide.Visible)
      {
        if (useAnimations)
        {
          currentState = SplitterState.Collapsing;

          if (parentForm != null)
          {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
              parentFormWidth = parentForm.Width - controlWidth;
            }
            else
            {
              parentFormHeight = parentForm.Height - controlHeight;
            }
          }

          animationTimer.Enabled = true;
        }
        else
        {
          // no animations, so just toggle the visible state
          currentState = SplitterState.Collapsed;
          controlToHide.Visible = false;
          if (expandParentForm && parentForm != null)
          {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
              parentForm.Width -= controlToHide.Width;
            }
            else
            {
              parentForm.Height -= controlToHide.Height;
            }
          }
        }
      }
      else
      {
        // control to hide is collapsed
        if (useAnimations)
        {
          currentState = SplitterState.Expanding;

          if (Dock == DockStyle.Left || Dock == DockStyle.Right)
          {
            if (parentForm != null)
            {
              parentFormWidth = parentForm.Width + controlWidth;
            }
            controlToHide.Width = 0;
          }
          else
          {
            if (parentForm != null)
            {
              parentFormHeight = parentForm.Height + controlHeight;
            }
            controlToHide.Height = 0;
          }
          controlToHide.Visible = true;
          animationTimer.Enabled = true;
        }
        else
        {
          // no animations, so just toggle the visible state
          currentState = SplitterState.Expanded;
          controlToHide.Visible = true;
          if (expandParentForm && parentForm != null)
          {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
              parentForm.Width += controlToHide.Width;
            }
            else
            {
              parentForm.Height += controlToHide.Height;
            }
          }
        }
      }
    }

    #endregion

    #region Implementation

    #region Animation Timer Tick

    private void animationTimerTick(object sender, EventArgs e)
    {
      switch (currentState)
      {
        case SplitterState.Collapsing:

          if (Dock == DockStyle.Left || Dock == DockStyle.Right)
          {
            // vertical splitter
            if (controlToHide.Width > animationStep)
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Width -= animationStep;
              }
              controlToHide.Width -= animationStep;
            }
            else
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Width = parentFormWidth;
              }
              controlToHide.Visible = false;
              animationTimer.Enabled = false;
              controlToHide.Width = controlWidth;
              currentState = SplitterState.Collapsed;
              Invalidate();
            }
          }
          else
          {
            // horizontal splitter
            if (controlToHide.Height > animationStep)
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Height -= animationStep;
              }
              controlToHide.Height -= animationStep;
            }
            else
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Height = parentFormHeight;
              }
              controlToHide.Visible = false;
              animationTimer.Enabled = false;
              controlToHide.Height = controlHeight;
              currentState = SplitterState.Collapsed;
              Invalidate();
            }
          }
          break;

        case SplitterState.Expanding:

          if (Dock == DockStyle.Left || Dock == DockStyle.Right)
          {
            // vertical splitter
            if (controlToHide.Width < (controlWidth - animationStep))
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Width += animationStep;
              }
              controlToHide.Width += animationStep;
            }
            else
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Width = parentFormWidth;
              }
              controlToHide.Width = controlWidth;
              controlToHide.Visible = true;
              animationTimer.Enabled = false;
              currentState = SplitterState.Expanded;
              Invalidate();
            }
          }
          else
          {
            // horizontal splitter
            if (controlToHide.Height < (controlHeight - animationStep))
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Height += animationStep;
              }
              controlToHide.Height += animationStep;
            }
            else
            {
              if (expandParentForm && parentForm.WindowState != FormWindowState.Maximized
                  && parentForm != null)
              {
                parentForm.Height = parentFormHeight;
              }
              controlToHide.Height = controlHeight;
              controlToHide.Visible = true;
              animationTimer.Enabled = false;
              currentState = SplitterState.Expanded;
              Invalidate();
            }
          }
          break;
      }
    }

    #endregion

    #region Paint the control

    // OnPaint is now an override rather than an event in version 1.1
    protected override void OnPaint(PaintEventArgs e)
    {
      // create a Graphics object
      Graphics g = e.Graphics;

      // find the rectangle for the splitter and paint it
      Rectangle r = ClientRectangle; // fixed in version 1.1
      g.FillRectangle(new SolidBrush(BackColor), r);

      #region Vertical Splitter

      // Check the docking style and create the control rectangle accordingly
      if (Dock == DockStyle.Left || Dock == DockStyle.Right)
      {
        // create a new rectangle in the vertical center of the splitter for our collapse control button
        rr = new Rectangle(r.X, r.Y + ((r.Height - 115) / 2), 8, 115);
        // force the width to 8px so that everything always draws correctly
        Width = 8;

        // draw the background color for our control image
        if (hot)
        {
          g.FillRectangle(new SolidBrush(hotColor), new Rectangle(rr.X + 1, rr.Y, 6, 115));
        }
        else
        {
          g.FillRectangle(new SolidBrush(BackColor), new Rectangle(rr.X + 1, rr.Y, 6, 115));
        }

        // draw the top & bottom lines for our control image
        g.DrawLine(new Pen(SystemColors.ControlDark, 1), rr.X + 1, rr.Y, rr.X + rr.Width - 2, rr.Y);
        g.DrawLine(new Pen(SystemColors.ControlDark, 1), rr.X + 1, rr.Y + rr.Height, rr.X + rr.Width - 2,
                   rr.Y + rr.Height);

        if (Enabled)
        {
          // draw the arrows for our control image
          // the ArrowPointArray is a point array that defines an arrow shaped polygon
          g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark), ArrowPointArray(rr.X + 2, rr.Y + 3));
          g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark), ArrowPointArray(rr.X + 2, rr.Y + rr.Height - 9));
        }

        // draw the dots for our control image using a loop
        int x = rr.X + 3;
        int y = rr.Y + 14;

        // Visual Styles added in version 1.1
        switch (visualStyle)
        {
          case VisualStyles.Mozilla:

            for (int i = 0; i < 30; i++)
            {
              // light dot
              g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y + (i * 3), x + 1, y + 1 + (i * 3));
              // dark dot
              g.DrawLine(new Pen(SystemColors.ControlDarkDark), x + 1, y + 1 + (i * 3), x + 2, y + 2 + (i * 3));
              // overdraw the background color as we actually drew 2px diagonal lines, not just dots
              if (hot)
              {
                g.DrawLine(new Pen(hotColor), x + 2, y + 1 + (i * 3), x + 2, y + 2 + (i * 3));
              }
              else
              {
                g.DrawLine(new Pen(BackColor), x + 2, y + 1 + (i * 3), x + 2, y + 2 + (i * 3));
              }
            }
            break;

          case VisualStyles.DoubleDots:
            for (int i = 0; i < 30; i++)
            {
              // light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x, y + 1 + (i * 3), 1, 1);
              // dark dot
              g.DrawRectangle(new Pen(SystemColors.ControlDark), x - 1, y + (i * 3), 1, 1);
              i++;
              // light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 2, y + 1 + (i * 3), 1, 1);
              // dark dot
              g.DrawRectangle(new Pen(SystemColors.ControlDark), x + 1, y + (i * 3), 1, 1);
            }
            break;

          case VisualStyles.Win9x:

            g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x + 2, y);
            g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x, y + 90);
            g.DrawLine(new Pen(SystemColors.ControlDark), x + 2, y, x + 2, y + 90);
            g.DrawLine(new Pen(SystemColors.ControlDark), x, y + 90, x + 2, y + 90);
            break;

          case VisualStyles.XP:

            for (int i = 0; i < 18; i++)
            {
              // light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLight), x, y + (i * 5), 2, 2);
              // light light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1, y + 1 + (i * 5), 1, 1);
              // dark dark dot
              g.DrawRectangle(new Pen(SystemColors.ControlDarkDark), x, y + (i * 5), 1, 1);
              // dark fill
              g.DrawLine(new Pen(SystemColors.ControlDark), x, y + (i * 5), x, y + (i * 5) + 1);
              g.DrawLine(new Pen(SystemColors.ControlDark), x, y + (i * 5), x + 1, y + (i * 5));
            }
            break;

          case VisualStyles.Lines:

            for (int i = 0; i < 44; i++)
            {
              g.DrawLine(new Pen(SystemColors.ControlDark), x, y + (i * 2), x + 2, y + (i * 2));
            }

            break;
        }

        // Added in version 1.3
        if (borderStyle != Border3DStyle.Flat)
        {
          // Paint the control border
          ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, borderStyle, Border3DSide.Left);
          ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, borderStyle, Border3DSide.Right);
        }
      }

        #endregion

        // Horizontal Splitter support added in v1.2

        #region Horizontal Splitter

      else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
      {
        // create a new rectangle in the horizontal center of the splitter for our collapse control button
        rr = new Rectangle(r.X + ((r.Width - 115) / 2), r.Y, 115, 8);
        // force the height to 8px
        Height = 8;

        // draw the background color for our control image
        if (hot)
        {
          g.FillRectangle(new SolidBrush(hotColor), new Rectangle(rr.X, rr.Y + 1, 115, 6));
        }
        else
        {
          g.FillRectangle(new SolidBrush(BackColor), new Rectangle(rr.X, rr.Y + 1, 115, 6));
        }

        // draw the left & right lines for our control image
        g.DrawLine(new Pen(SystemColors.ControlDark, 1), rr.X, rr.Y + 1, rr.X, rr.Y + rr.Height - 2);
        g.DrawLine(new Pen(SystemColors.ControlDark, 1), rr.X + rr.Width, rr.Y + 1, rr.X + rr.Width,
                   rr.Y + rr.Height - 2);

        if (Enabled)
        {
          // draw the arrows for our control image
          // the ArrowPointArray is a point array that defines an arrow shaped polygon
          g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark), ArrowPointArray(rr.X + 3, rr.Y + 2));
          g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark), ArrowPointArray(rr.X + rr.Width - 9, rr.Y + 2));
        }

        // draw the dots for our control image using a loop
        int x = rr.X + 14;
        int y = rr.Y + 3;

        // Visual Styles added in version 1.1
        switch (visualStyle)
        {
          case VisualStyles.Mozilla:

            for (int i = 0; i < 30; i++)
            {
              // light dot
              g.DrawLine(new Pen(SystemColors.ControlLightLight), x + (i * 3), y, x + 1 + (i * 3), y + 1);
              // dark dot
              g.DrawLine(new Pen(SystemColors.ControlDarkDark), x + 1 + (i * 3), y + 1, x + 2 + (i * 3), y + 2);
              // overdraw the background color as we actually drew 2px diagonal lines, not just dots
              if (hot)
              {
                g.DrawLine(new Pen(hotColor), x + 1 + (i * 3), y + 2, x + 2 + (i * 3), y + 2);
              }
              else
              {
                g.DrawLine(new Pen(BackColor), x + 1 + (i * 3), y + 2, x + 2 + (i * 3), y + 2);
              }
            }
            break;

          case VisualStyles.DoubleDots:

            for (int i = 0; i < 30; i++)
            {
              // light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1 + (i * 3), y, 1, 1);
              // dark dot
              g.DrawRectangle(new Pen(SystemColors.ControlDark), x + (i * 3), y - 1, 1, 1);
              i++;
              // light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1 + (i * 3), y + 2, 1, 1);
              // dark dot
              g.DrawRectangle(new Pen(SystemColors.ControlDark), x + (i * 3), y + 1, 1, 1);
            }
            break;

          case VisualStyles.Win9x:

            g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x, y + 2);
            g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x + 88, y);
            g.DrawLine(new Pen(SystemColors.ControlDark), x, y + 2, x + 88, y + 2);
            g.DrawLine(new Pen(SystemColors.ControlDark), x + 88, y, x + 88, y + 2);
            break;

          case VisualStyles.XP:

            for (int i = 0; i < 18; i++)
            {
              // light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLight), x + (i * 5), y, 2, 2);
              // light light dot
              g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1 + (i * 5), y + 1, 1, 1);
              // dark dark dot
              g.DrawRectangle(new Pen(SystemColors.ControlDarkDark), x + (i * 5), y, 1, 1);
              // dark fill
              g.DrawLine(new Pen(SystemColors.ControlDark), x + (i * 5), y, x + (i * 5) + 1, y);
              g.DrawLine(new Pen(SystemColors.ControlDark), x + (i * 5), y, x + (i * 5), y + 1);
            }
            break;

          case VisualStyles.Lines:

            for (int i = 0; i < 44; i++)
            {
              g.DrawLine(new Pen(SystemColors.ControlDark), x + (i * 2), y, x + (i * 2), y + 2);
            }

            break;
        }

        // Added in version 1.3
        if (borderStyle != Border3DStyle.Flat)
        {
          // Paint the control border
          ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, borderStyle, Border3DSide.Top);
          ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, borderStyle, Border3DSide.Bottom);
        }
      }

        #endregion

      else
      {
        throw new Exception("The Collapsible Splitter control cannot have the Filled or None Dockstyle property");
      }


      // dispose the Graphics object
      g.Dispose();
    }

    #endregion

    #region Arrow Polygon Array

    // This creates a point array to draw a arrow-like polygon
    private Point[] ArrowPointArray(int x, int y)
    {
      Point[] point = new Point[3];

      if (controlToHide != null)
      {
        // decide which direction the arrow will point
        if (
          (Dock == DockStyle.Right && controlToHide.Visible)
          || (Dock == DockStyle.Left && !controlToHide.Visible)
          )
        {
          // right arrow
          point[0] = new Point(x, y);
          point[1] = new Point(x + 3, y + 3);
          point[2] = new Point(x, y + 6);
        }
        else if (
          (Dock == DockStyle.Right && !controlToHide.Visible)
          || (Dock == DockStyle.Left && controlToHide.Visible)
          )
        {
          // left arrow
          point[0] = new Point(x + 3, y);
          point[1] = new Point(x, y + 3);
          point[2] = new Point(x + 3, y + 6);
        }

          // Up/Down arrows added in v1.2

        else if (
          (Dock == DockStyle.Top && controlToHide.Visible)
          || (Dock == DockStyle.Bottom && !controlToHide.Visible)
          )
        {
          // up arrow
          point[0] = new Point(x + 3, y);
          point[1] = new Point(x + 6, y + 4);
          point[2] = new Point(x, y + 4);
        }
        else if (
          (Dock == DockStyle.Top && !controlToHide.Visible)
          || (Dock == DockStyle.Bottom && controlToHide.Visible)
          )
        {
          // down arrow
          point[0] = new Point(x, y);
          point[1] = new Point(x + 6, y);
          point[2] = new Point(x + 3, y + 3);
        }
      }

      return point;
    }

    #endregion

    #region Color Calculator

    // this method was borrowed from the RichUI Control library by Sajith M
    private static Color CalculateColor(Color front, Color back, int alpha)
    {
      // solid color obtained as a result of alpha-blending

      Color frontColor = Color.FromArgb(255, front);
      Color backColor = Color.FromArgb(255, back);

      float frontRed = frontColor.R;
      float frontGreen = frontColor.G;
      float frontBlue = frontColor.B;
      float backRed = backColor.R;
      float backGreen = backColor.G;
      float backBlue = backColor.B;

      float fRed = frontRed * alpha / 255 + backRed * ((float)(255 - alpha) / 255);
      byte newRed = (byte)fRed;
      float fGreen = frontGreen * alpha / 255 + backGreen * ((float)(255 - alpha) / 255);
      byte newGreen = (byte)fGreen;
      float fBlue = frontBlue * alpha / 255 + backBlue * ((float)(255 - alpha) / 255);
      byte newBlue = (byte)fBlue;

      return Color.FromArgb(255, newRed, newGreen, newBlue);
    }

    #endregion

    #endregion
  }

  /// <summary>
  ///   A simple designer class for the CollapsibleSplitter control to remove 
  ///   unwanted properties at design time.
  /// </summary>
  public class CollapsibleSplitterDesigner : ControlDesigner
  {
    protected override void PreFilterProperties(IDictionary properties)
    {
      properties.Remove("IsCollapsed");
      properties.Remove("BorderStyle");
      properties.Remove("Size");
    }
  }
}