using System;
using System.Collections.Generic;
using System.Text;

namespace LyricsEngine
{
    #region LyricSiteInfo Class
    internal struct LyricSiteInfo
    {
        internal string siteName;
        internal string siteNameToSearch;
        internal string siteWWW;
        internal int noOfTries;

        internal string artist;
        internal string track;
        internal string extra;
        internal string artistWithDoubleQuotes;
        internal string trackWithDoubleQuotes;
        internal string titleContent;

        internal string[] parseStringStart;
        internal string parseStringEnd;
        internal int maxNoOfLinesBeforeLyric;
        internal int linesToSkip;
        internal int linesToSkipInTheEnd;
        internal string[] trimPhrases;
        internal string phraseThatEqualsLineshift;

        internal int noOfHitsUsed;
        internal string notALyric;

        internal string removePhraseInLineStart;
        internal string removePhraseInLineEnd;

        internal LyricSiteInfo(string siteName, string siteNameToSearch, string siteWWW, int noOfTries, string artist, string track, string extra,
                                string titleContent, string[] parseStringStart, string parseStringEnd, int maxNoOfLinesBeforeLyric,
                                int linesToSkip, int linesToSkipInTheEnd, string[] trimPhrases, string phraseThatEqualsLineshift, int noOfHitsUsed, string notALyric,
                                string removePhraseInLineStart, string removePhraseInLineEnd)
        {
            this.siteName = siteName;
            this.siteNameToSearch = siteNameToSearch;
            this.siteWWW = siteWWW;
            this.noOfTries = noOfTries;
            this.artist = artist;
            this.track = track;
            this.extra = extra;
            this.artistWithDoubleQuotes = @"""" + artist + @"""";
            this.trackWithDoubleQuotes = @"""" + track + @"""";
            this.titleContent = titleContent;
            this.parseStringStart = parseStringStart;
            this.parseStringEnd = parseStringEnd;
            this.maxNoOfLinesBeforeLyric = maxNoOfLinesBeforeLyric;
            this.linesToSkip = linesToSkip;
            this.linesToSkipInTheEnd = linesToSkipInTheEnd;
            this.trimPhrases = trimPhrases;
            this.phraseThatEqualsLineshift = phraseThatEqualsLineshift;
            this.noOfHitsUsed = noOfHitsUsed;
            this.notALyric = notALyric;
            this.removePhraseInLineStart = removePhraseInLineStart;
            this.removePhraseInLineEnd = removePhraseInLineEnd;
        }
    }
    #endregion
}
