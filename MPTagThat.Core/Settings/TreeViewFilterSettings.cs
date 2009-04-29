using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class TreeViewFilterSettings
  {
    #region Variable
    private string _lastUsedFormat = "";
    private List<TreeViewFilter> _filters = new List<TreeViewFilter>();
    #endregion

    #region Properties
    [Setting(SettingScope.User, "")]
    public string LastUsedFormat
    {
      get { return _lastUsedFormat; }
      set { _lastUsedFormat = value; }
    }

    [Setting(SettingScope.User, "")]
    public List<TreeViewFilter> Filter
    {
      get { return _filters; }
      set { _filters = value; }
    }
    #endregion

    #region Public Methods
    public void Save()
    {
      ServiceScope.Get<ISettingsManager>().Save(this);
    }
    #endregion
  }

  public class TreeViewFilter
  {
    #region Variables

    private string _name;
    private string _fileFilter;
    private string _fileMask;
    private bool _useTagFilter;
    private List<TreeViewTagFilter> _tagFilter = new List<TreeViewTagFilter>();
    #endregion

    #region Properties
    public  string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public string FileFilter
    {
      get { return _fileFilter; }
      set { _fileFilter = value; }
    }

    public  string FileMask
    {
      get { return _fileMask; }
      set { _fileMask = value; }
    }

    public bool UseTagFilter
    {
      get { return _useTagFilter; }
      set { _useTagFilter = value; }
    }

    public List<TreeViewTagFilter> TagFilter
    {
      get { return _tagFilter; }
    }
    #endregion
  }

  public class TreeViewTagFilter
  {
    #region Variables

    private string _filterField;
    private string _filterValue = "";
    private string _filterOperator = "";
    #endregion

    #region Properties
    public string Field
    {
      get { return _filterField; }
      set { _filterField = value; }
    }

    public string FilterValue
    {
      get { return _filterValue; }
      set { _filterValue = value; }
    }

    public string FilterOperator
    {
      get { return _filterOperator; }
      set { _filterOperator = value; }
    }
    #endregion
  }
}
