#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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

using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace MPTagThat.Core.MusicBrainz
{
  internal static class NativeMethods
  {
    /// <summary>
    ///   Get a PUID based on the raw audio data from a given track
    /// </summary>
    /// <param name = "fingerprint">A Stringbuilder, which will receive the fingerprint</param>
    /// <param name = "samples">At least the first 135 seconds worth of samples of the audio file as an array of bytes</param>
    /// <param name = "byteOrder">0 for little-endian or 1 for big-endian</param>
    /// <param name = "numberOfSamples">Number of samples (half the number of bytes if stereo = 1)</param>
    /// <param name = "sampleRate">Sample rate (44100 is typical)</param>
    /// <param name = "stereo">1 if stereo, 0 otherwise</param>
    /// <returns>A bool indicating, if the call was sucessfull or not</returns>
    [DllImport("libofa.dll", EntryPoint = "ofa_create_print", ExactSpelling = true,
      CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern bool ofa_create_print(StringBuilder fingerprint, byte[] samples, int byteOrder, int size,
                                               int sRate, bool stereo);
  }
}