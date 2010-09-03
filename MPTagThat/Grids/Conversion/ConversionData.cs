using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MPTagThat.Core;

namespace MPTagThat.GridView
{
  public class ConversionData
  {
    private string _fileNameNew;
    private TrackData _track; 

    public ConversionData()
    {
    }

    public string FileName
    {
      get { return Track.FullFileName; }
    }

    public string NewFileName
    {
      get { return _fileNameNew; }
      set { _fileNameNew = value; }
    }

    public TrackData Track
    {
      get { return _track; }
      set { _track = value; }
    }
  }
}
