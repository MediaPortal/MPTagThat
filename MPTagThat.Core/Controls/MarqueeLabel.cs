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
using System.Windows.Forms;

#endregion

namespace MPTagThat.Core
{
  public class MarqueeLabel : Control
  {
    #region Variables

    private readonly Timer _tmrScroll;
    private string _displayText;
    private int _maxLeft;
    private int _position;
    private int _scrollDirection = -1;
    private int _scrollPixelAmount = 10;
    private int _wait;

    #endregion

    #region Public Properties

    public string DisplayText
    {
      get { return _displayText; }
      set
      {
        _displayText = value;
        _scrollDirection = -1;
        _position = 0;
        Invalidate();
      }
    }

    public int ScrollPixelAmount
    {
      get { return _scrollPixelAmount; }
      set { _scrollPixelAmount = value; }
    }

    public Timer ScrollTimer
    {
      get { return _tmrScroll; }
    }

    #endregion

    #region ctor

    public MarqueeLabel()
    {
      _tmrScroll = new Timer(new Container());
      _tmrScroll.Tick += tmrScroll_Tick;
      Size = new Size(360, 104);

      DoubleBuffered = true;
      ResizeRedraw = true;

      _tmrScroll.Enabled = false;
    }

    #endregion

    #region Private Methods

    private void tmrScroll_Tick(object sender, EventArgs e)
    {
      if (_wait > 0)
      {
        _wait--;
        return;
      }

      _position += ScrollPixelAmount * _scrollDirection;
      Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      int width = (int)e.Graphics.MeasureString(DisplayText, Font).Width;
      _maxLeft = Width - width;

      // Don't scroll, if text fits in control
      if (width <= Width)
      {
        _position = 0;
        _tmrScroll.Enabled = false; // Stop Timer
      }

      if (_position < _maxLeft)
        _scrollDirection = -_scrollDirection;

      if (_position > 0)
      {
        _scrollDirection = -_scrollDirection;
        _wait = 3; // Pause 3 cycles
      }

      e.Graphics.DrawString(DisplayText, Font, new SolidBrush(ForeColor), _position, 0);
    }

    #endregion
  }
}