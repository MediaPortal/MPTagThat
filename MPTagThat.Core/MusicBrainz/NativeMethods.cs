using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MPTagThat.Core.MusicBrainz
{
	internal static class NativeMethods
	{
		/// <summary>
		/// Get a PUID based on the raw audio data from a given track
		/// </summary>
    /// <param name="fingerprint">A Stringbuilder, which will receive the fingerprint</param>
		/// <param name="samples">At least the first 135 seconds worth of samples of the audio file as an array of bytes</param>
		/// <param name="byteOrder">0 for little-endian or 1 for big-endian</param>
		/// <param name="numberOfSamples">Number of samples (half the number of bytes if stereo = 1)</param>
		/// <param name="sampleRate">Sample rate (44100 is typical)</param>
		/// <param name="stereo">1 if stereo, 0 otherwise</param>
		/// <returns>A bool indicating, if the call was sucessfull or not</returns>
    [DllImport("libofa.dll", EntryPoint = "ofa_create_print", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern bool ofa_create_print(StringBuilder fingerprint, byte[] samples, int byteOrder, int size, int sRate, bool stereo);
	}
}
