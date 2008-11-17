using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MPTagThat.Core;

namespace MPTagThat.GridView
{
  public class GridViewSettings : INamedSettings
  {
    private Collection<GridViewColumn> _columns = new Collection<GridViewColumn>();
    private string _name;

    [Setting(SettingScope.User, "")]
    public Collection<GridViewColumn> Columns
    {
      get { return _columns; }
      set { _columns = value; }
    }

    #region INamedSettings Members

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        _name = value;
      }
    }

    #endregion
  }
}
