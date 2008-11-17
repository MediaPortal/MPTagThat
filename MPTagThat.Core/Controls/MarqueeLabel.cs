using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MPTagThat.Core
{
  public class MarqueeLabel : Control
  {
    #region Variables
    private string _displayText;
    private int _scrollPixelAmount = 10;
    private Timer _tmrScroll;
    private int _position = 0;
    private int _maxLeft = 0;
    private int _scrollDirection = -1;
    private int _wait = 0;
    #endregion

    #region Public Properties
    public string DisplayText
    {
      get { return _displayText; }
      set { 
        _displayText = value;
        _scrollDirection = -1;
        _position = 0;
        Invalidate();
      }
    }

    public int ScrollPixelAmount
    {
      get { return _scrollPixelAmount;}
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
      this._tmrScroll = new System.Windows.Forms.Timer(new System.ComponentModel.Container());
      this._tmrScroll.Tick += new System.EventHandler(this.tmrScroll_Tick);
      this.Size = new System.Drawing.Size(360, 104);

      DoubleBuffered = true;
      ResizeRedraw = true;

      _tmrScroll.Enabled = false;
    }
    #endregion

    #region Private Methods
    private void tmrScroll_Tick(object sender, System.EventArgs e)
    {
      if (_wait > 0)
      {
        _wait--;
        return;
      }

      _position += ScrollPixelAmount * _scrollDirection;
      Invalidate();
    }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
      base.OnPaint(e);
      int width = (int)e.Graphics.MeasureString(DisplayText, Font).Width;
      _maxLeft = this.Width - width;

      // Don't scroll, if text fits in control
      if (width <= this.Width)
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
