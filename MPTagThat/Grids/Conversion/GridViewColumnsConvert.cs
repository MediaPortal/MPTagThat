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

using System.Collections.Generic;
using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat.GridView
{
  public class GridViewColumnsConvert
  {
    #region Variables

    private readonly GridViewColumn _filename;
    private readonly GridViewColumn _newfilename;
    private readonly GridViewColumn _status;
    private GridViewSettings _settings;

    #endregion

    #region Constructor

    public GridViewColumnsConvert()
    {
      _status = new GridViewColumn("Status", "process", 100, true, true, false, true);
      _filename = new GridViewColumn("FileName", "text", 350, true, true, true, true);
      _newfilename = new GridViewColumn("NewFileName", "text", 350, true, true, true, true);
      LoadSettings();
    }

    #endregion

    #region Properties

    public GridViewSettings Settings
    {
      get { return _settings; }
    }

    #endregion

    #region Private Methods

    private void LoadSettings()
    {
      _settings = new GridViewSettings();
      _settings.Name = "Convert";
      ServiceScope.Get<ISettingsManager>().Load(_settings);
      if (_settings.Columns.Count == 0)
      {
        // Setup the Default Columns to display on first use of the program
        List<GridViewColumn> columnList = new List<GridViewColumn>();
        columnList = SetDefaultColumns();
        _settings.Columns.Clear();
        foreach (GridViewColumn column in columnList)
        {
          _settings.Columns.Add(column);
        }
        _settings.Name = "Convert";
        ServiceScope.Get<ISettingsManager>().Save(_settings);
      }
    }

    public void SaveSettings()
    {
      _settings.Name = "Convert";
      ServiceScope.Get<ISettingsManager>().Save(_settings);
    }

    public void SaveColumnSettings(DataGridViewColumn column, int colIndex)
    {
      _settings.Columns[colIndex].Width = column.Width;
      _settings.Columns[colIndex].DisplayIndex = column.DisplayIndex;
      _settings.Columns[colIndex].Display = column.Visible;
    }


    private List<GridViewColumn> SetDefaultColumns()
    {
      List<GridViewColumn> columnList = new List<GridViewColumn>();
      columnList.Add(_status);
      columnList.Add(_filename);
      columnList.Add(_newfilename);
      return columnList;
    }

    #endregion
  }
}