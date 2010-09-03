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

namespace MPTagThat.Core
{
  public class GridViewColumn
  {
    private bool _bound = true;
    private string _columnName;
    private bool _display = true;
    private string _type = "text";
    private int _width = 100;


    public GridViewColumn(string name, string type, int width, bool display, bool readOnly, bool bound, bool frozen)
    {
      _columnName = name;
      _type = type;
      _width = width;
      _display = display;
      Readonly = readOnly;
      _bound = bound;
      Frozen = frozen;
    }

    public GridViewColumn() {}

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

    public bool Readonly { get; set; }

    public bool Frozen { get; set; }

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

    public int DisplayIndex { get; set; }
  }
}