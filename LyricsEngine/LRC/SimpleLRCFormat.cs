using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LyricsEngine.LRC
{
    public static class SimpleLRCFormat
    {
        public static Regex ArtistLineStartRegex = new Regex(@"\[ar\w*\:", RegexOptions.IgnoreCase);
        public static Regex TitleLineStartRegex = new Regex(@"\[ti\w*\:", RegexOptions.IgnoreCase);
        public static Regex OffsetLineStartRegex = new Regex(@"\[offset\w*\:", RegexOptions.IgnoreCase);
        public static Regex AlbumLineStartRegex = new Regex(@"\[al\w*\:", RegexOptions.IgnoreCase);
        public static Regex LineLineRegex = new Regex(@"\[\d+:\d+\.*\d*\]");
    }
}