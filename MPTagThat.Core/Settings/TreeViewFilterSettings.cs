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

#endregion

namespace MPTagThat.Core
{
  public class TreeViewFilterSettings
  {
    #region Variable

    private List<TreeViewFilter> _filters = new List<TreeViewFilter>();
    private string _lastUsedFormat = "";

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

    private readonly List<TreeViewTagFilter> _tagFilter = new List<TreeViewTagFilter>();

    #endregion

    #region Properties

    public string Name { get; set; }

    public string FileFilter { get; set; }

    public string FileMask { get; set; }

    public bool UseTagFilter { get; set; }

    public List<TreeViewTagFilter> TagFilter
    {
      get { return _tagFilter; }
    }

    #endregion
  }

  public class TreeViewTagFilter
  {
    #region Variables

    private string _filterOperator = "";
    private string _filterValue = "";

    #endregion

    #region Properties

    public string Field { get; set; }

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