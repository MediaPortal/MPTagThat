using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.GridView
{
  public class GridViewColumnsBurn
  {
    #region Variables
    private GridViewSettings _settings;
    private GridViewColumn _filename;
    private GridViewColumn _status;
    private GridViewColumn _track;
    private GridViewColumn _artist;
    private GridViewColumn _title;
    private GridViewColumn _duration;
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
