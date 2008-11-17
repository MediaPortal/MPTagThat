using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core.AudioEncoder
{
  public interface IAudioEncoder
  {
    /// <summary>
    /// Sets the Encoder and the Outfile Name
    /// </summary>
    /// <param name="encoder"></param>
    /// <param name="outFile"></param>
    /// <returns>Formatted Outfile with Extension</returns>
    string SetEncoder(string encoder, string outFile);

    /// <summary>
    /// Starts encoding using the given Parameters
    /// </summary>
    /// <param name="stream"></param>
    /// 
    Un4seen.Bass.BASSError StartEncoding(int stream);
  }
}
