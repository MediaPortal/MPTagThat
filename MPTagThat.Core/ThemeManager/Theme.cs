using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MPTagThat.Core
{
  public class Theme
  {
    private Color _backColor;
    private Color _labelForeColor;
    private Font _labelFont;
    private Color _formHeaderForeColor;
    private Font _formHeaderFont;
    private string _themeName;
    private Color _panelHeadingBackColor;
    private Font _panelHeadingFont;
    private Color _panelHeadingDirectionCtrlColor;
    private Color _defaultBackColor;
    private Color _selectionBackColor;
    private Color _alternatingRowBackColor;
    private Color _alternatingRowForeColor;
    private Color _changedBackColor;
    private Color _changedForeColor;
    private Color _buttonBackColor;
    private Color _buttonForeColor;
    private Font _buttonFont;

    public Font ButtonFont
    {
      get { return _buttonFont; }
      set { _buttonFont = value; }
    }
	
    public Color ButtonForeColor
    {
      get { return _buttonForeColor; }
      set { _buttonForeColor = value; }
    }
	
    public Color ButtonBackColor
    {
      get { return _buttonBackColor; }
      set { _buttonBackColor = value; }
    }
	
    public Color ChangedForeColor
    {
      get { return _changedForeColor; }
      set { _changedForeColor = value; }
    }
	
    public Color ChangedBackColor
    {
      get { return _changedBackColor; }
      set { _changedBackColor = value; }
    }

    public Color DefaultBackColor
    {
      get { return _defaultBackColor; }
      set { _defaultBackColor = value; }
    }

    public Color SelectionBackColor
    {
      get { return _selectionBackColor; }
      set { _selectionBackColor = value; }
    }

    public Color AlternatingRowForeColor
    {
      get { return _alternatingRowForeColor; }
      set { _alternatingRowForeColor = value; }
    }
	
    public Color AlternatingRowBackColor
    {
      get { return _alternatingRowBackColor; }
      set { _alternatingRowBackColor = value; }
    }
	

    public Color PanelHeadingDirectionCtrlColor
    {
      get { return _panelHeadingDirectionCtrlColor; }
      set { _panelHeadingDirectionCtrlColor = value; }
    }
	
    public Font PanelHeadingFont
    {
      get { return _panelHeadingFont; }
      set { _panelHeadingFont = value; }
    }
	
    public Color PanelHeadingBackColor
    {
      get { return _panelHeadingBackColor; }
      set { _panelHeadingBackColor = value; }
    }
	
    public string ThemeName
    {
      get { return _themeName; }
      set { _themeName = value; }
    }
	
    public Font LabelFont
    {
      get { return _labelFont; }
      set { _labelFont = value; }
    }
	
    public Color LabelForeColor
    {
      get { return _labelForeColor; }
      set { _labelForeColor = value; }
    }

    public Font FormHeaderFont
    {
      get { return _formHeaderFont; }
      set { _formHeaderFont = value; }
    }

    public Color FormHeaderForeColor
    {
      get { return _formHeaderForeColor; }
      set { _formHeaderForeColor = value; }
    }

    public Color BackColor
    {
      get { return _backColor; }
      set { _backColor = value; }
    }
	

    #region ctor
    public Theme()
    {
    }
    #endregion
  }
}
