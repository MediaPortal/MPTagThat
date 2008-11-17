using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class FileNameToTagFormatSettings : ParameterFormat
  {
    #region Public Methods
    public void Save()
    {
      ServiceScope.Get<ISettingsManager>().Save(this);
    }
    #endregion
  }
}
