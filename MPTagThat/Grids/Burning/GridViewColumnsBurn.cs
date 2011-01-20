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

using System.Collections.Generic;
using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat.GridView
{
  public class GridViewColumnsBurn
  {
    #region Variables

    private readonly GridViewColumn _artist;
    private readonly GridViewColumn _duration;
    private readonly GridViewColumn _filename;
    private readonly GridViewColumn _status;
    private readonly GridViewColumn _title;
    private readonly GridViewColumn _track;
    private GridViewSettings _settings;

    #endregion

    #region Constructor

    public GridViewColumnsBurn()
    {
      _status = new GridViewColumn("Status", "process", 100, true, true, false, true);
      _track = new GridViewColumn("Track", "text", 40, true, false, true, false);
      _artist = new GridViewColumn("Artist", "text", 150, true, false, true, false);
      _title = new GridViewColumn("Title", "text", 250, true, false, true, false);
      _duration = new GridViewColumn("Duration", "text", 100, true, true, true, false);
      _filename = new GridViewColumn("FullFileName", "text", 350, true, false, true, false);
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
      _settings.Name = "Burning";
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
        _settings.Name = "Burning";
        ServiceScope.Get<ISettingsManager>().Save(_settings);
      }
    }

    public void SaveSettings()
    {
      _settings.Name = "Burning";
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
      columnList.Add(_track);
      columnList.Add(_artist);
      columnList.Add(_title);
      columnList.Add(_duration);
      columnList.Add(_filename);
      return columnList;
    }

    #endregion
  }
}