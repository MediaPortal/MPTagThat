using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class GridViewColumn
  {
    private string _columnName;
    private bool _display = true;
    private bool _readonly = false;
    private int _width = 100;
    private bool _bound = true;
    private bool _frozen = false;
    private string _type = "text";
    private int _displayIndex = 0;


    public GridViewColumn(string name, string type, int width, bool display, bool readOnly, bool bound, bool frozen)
    {
      _columnName = name;
      _type = type;
      _width = width;
      _display = display;
      _readonly = readOnly;
      _bound = bound;
      _frozen = frozen;
    }

    public GridViewColumn()
    {

    }

    public string Name
    {
      get { return _columnName; }
      set { _columnName = value; }
    }

    public string Title
    {
      get { return ServiceScope.Get<ILocalisation>().ToString("column_header", _columnName); }
    }

    public bool Display
    {
      get { return _display; }
      set { _display = value; }
    }

    public bool Bound
    {
      get { return _bound; }
      set { _bound = value; }
    }

    public bool Readonly
    {
      get { return _readonly; }
      set { _readonly = value; }
    }

    public bool Frozen
    {
      get { return _frozen; }
      set { _frozen = value; }
    }

    public int Width
    {
      get { return _width; }
      set { _width = value; }
    }

    public string Type
    {
      get { return _type.ToLower(); }
      set { _type = value; }
    }

    public int DisplayIndex
    {
      get { return _displayIndex; }
      set { _displayIndex = value; }
    }
  }
}
