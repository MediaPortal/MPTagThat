using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class TagToFileNameFormatSettings : ParameterFormat
  {
    #region Public Methods
    public void Save()
    {
      ServiceScope.Get<ISettingsManager>().Save(this);
    }
    #endregion
  }
}
