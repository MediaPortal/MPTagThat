using System;
using System.Collections;
using System.Text;

namespace LyricsEngineConfig
{
    public struct LyricConfigInfo
    {
        internal string licenseKey;
        internal string artist;
        internal string track;
        internal string extra;

        public LyricConfigInfo(string licenseKey, string artist, string track, string extraInfo)
        {
            this.licenseKey = licenseKey;
            this.artist = artist;
            this.track = track;
            this.extra = extraInfo;;
        }
    }
}
