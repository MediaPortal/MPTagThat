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

using Un4seen.Bass;

#endregion

namespace MPTagThat.Core.AudioEncoder
{
  public interface IAudioEncoder
  {
    /// <summary>
    ///   Sets the Encoder and the Outfile Name
    /// </summary>
    /// <param name = "encoder"></param>
    /// <param name = "outFile"></param>
    /// <returns>Formatted Outfile with Extension</returns>
    string SetEncoder(string encoder, string outFile);

    /// <summary>
    ///   Starts encoding using the given Parameters
    /// </summary>
    /// <param name = "stream"></param>
    /// <param name = "rowIndex"></param>
    BASSError StartEncoding(int stream, int rowIndex);

    /// <summary>
    /// Aborts the current encoding
    /// </summary>
    void AbortEncoding();
  }
}