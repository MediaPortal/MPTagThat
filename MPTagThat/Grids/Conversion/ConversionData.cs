using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TagLib;

namespace MPTagThat.GridView
{
  public class ConversionData
  {
    private string _fileNameNew;
    private string _fileNameOld;

    public ConversionData()
    {
    }

    public string FileName
    {
      get { return _fileNameOld; }
      set { _fileNameOld = value; }
    }

    public string NewFileName
    {
      get { return _fileNameNew; }
      set { _fileNameNew = value; }
    }
  }
}
