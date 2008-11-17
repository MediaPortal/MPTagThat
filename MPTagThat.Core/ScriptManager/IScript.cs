using System;
using System.Collections.Generic;
using System.Text;
using TagLib;

namespace MPTagThat.Core
{
  public interface IScript
  {
    TrackData Invoke(TrackData track);
  }
}
