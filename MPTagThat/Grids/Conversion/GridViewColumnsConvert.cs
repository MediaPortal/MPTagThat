using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.GridView
{
  public class GridViewColumnsConvert
  {
    #region Variables
    private GridViewSettings _settings;
    private GridViewColumn _filename;
    private GridViewColumn _newfilename;
    private GridViewColumn _status;
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
