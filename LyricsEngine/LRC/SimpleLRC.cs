using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace LyricsEngine.LRC
{
    public class SimpleLRC
    {
        private string _album;
        private string _artist;
        private readonly bool _isValid;
        private string _lyricAsLRC;
        private readonly string _lyricAsPlainLyric;

        private readonly ArrayList _lyricLines;
        private string _offset;
        private readonly ArrayList _simpleLRCTimeAndLineArray;
        private readonly SimpleLRCTimeAndLineCollection _simpleLRCTimeAndLineCollection;
        private readonly SimpleLRCTimeAndLineCollection _simpleLRCTimeAndLineCollectionWithOffset;
        private string _title;


        /// <summary>
        /// Constructor used in the MyLyrics configuration
        /// </summary>
        /// <param name="file"></param>
        public SimpleLRC(string file)
        {
            var textReader = new StreamReader(file);

            string line;
            _lyricLines = new ArrayList();
            _simpleLRCTimeAndLineArray = new ArrayList();

            while ((line = textReader.ReadLine()) != null)
            {
                var done = false;
                var originalLine = true;
                while (done == false)
                {
                    done = GetLRCinfoFromFile(ref line, originalLine);
                    originalLine = false;
                }
            }

            if (_simpleLRCTimeAndLineArray.Count > 0)
            {
                _simpleLRCTimeAndLineCollection =
                    new SimpleLRCTimeAndLineCollection(
                        (SimpleLRCTimeAndLine[]) _simpleLRCTimeAndLineArray.ToArray(typeof (SimpleLRCTimeAndLine)));
                _isValid = true;
            }

            if (!string.IsNullOrEmpty(_lyricAsLRC))
            {
                _lyricAsPlainLyric = SimpleLRCFormat.LineLineRegex.Replace(_lyricAsLRC, string.Empty);
            }

            textReader.Close();
        }


        public SimpleLRC(string artist, string title, string lyric)
        {
            _artist = artist;
            _title = title;

            var separators = new[] {"\n"};
            var lines = lyric.Split(separators, StringSplitOptions.None);
            _lyricLines = new ArrayList();
            _simpleLRCTimeAndLineArray = new ArrayList();

            for (var i = 0; i < lines.Length; i++)
            {
                var done = false;
                var originalLine = true;
                var line = lines[i];
                while (done == false)
                {
                    done = GetLRCinfoFromFile(ref line, originalLine);
                    originalLine = false;
                }
            }

            if (_simpleLRCTimeAndLineArray.Count > 0)
            {
                _simpleLRCTimeAndLineCollection =
                    new SimpleLRCTimeAndLineCollection(
                        (SimpleLRCTimeAndLine[]) _simpleLRCTimeAndLineArray.ToArray(typeof (SimpleLRCTimeAndLine)));
                _isValid = true;
            }

            var simpleLRCTimeAndLineArrayWithOffset = new ArrayList();

            int offsetInt;

            if (int.TryParse(_offset, out offsetInt))
            {
                for (var i = 0; i < _simpleLRCTimeAndLineArray.Count; i++)
                {
                    simpleLRCTimeAndLineArrayWithOffset.Add(
                        ((SimpleLRCTimeAndLine) _simpleLRCTimeAndLineArray[i]).IncludeOffset(offsetInt));
                }

                _simpleLRCTimeAndLineCollectionWithOffset =
                    new SimpleLRCTimeAndLineCollection(
                        (SimpleLRCTimeAndLine[])
                            simpleLRCTimeAndLineArrayWithOffset.ToArray(typeof (SimpleLRCTimeAndLine)));
            }
            else
            {
                _simpleLRCTimeAndLineCollectionWithOffset = _simpleLRCTimeAndLineCollection;
            }

            if (!string.IsNullOrEmpty(_lyricAsLRC))
            {
                _lyricAsPlainLyric = SimpleLRCFormat.LineLineRegex.Replace(_lyricAsLRC, string.Empty);
            }
        }

        private bool GetLRCinfoFromFile(ref string line, bool originalLine)
        {
            Match m;

            if ((m = SimpleLRCFormat.LineLineRegex.Match(line)).Success)
            {
                line = line.Trim();
                int index;
                if ((index = m.Value.IndexOf("[", StringComparison.Ordinal)) > 0)
                {
                    line = line.Substring(index);
                }

                var lineWithTimeAndNewLine = line + Environment.NewLine;

                // if a line with multiple timetags, only add the first which is the complete with all tags.
                if (originalLine)
                {
                    _lyricAsLRC += lineWithTimeAndNewLine;
                }

                // we update the line for potential further time-tags. This will natural not be regarded as an original line
                // and will therefore not be added to the lyric. It will however be added to the LRC-object just as every other line
                line = line.Replace(m.Value, "");

                var lineWithNewLine = line + Environment.NewLine;
                _lyricLines.Add(lineWithNewLine);

                const int next = 1;
                int secLength;
                int sec;
                var msec = 0;

                const int minStart = next;
                var minLength = m.Value.IndexOf(":", StringComparison.Ordinal) - minStart;
                var min = int.Parse(m.Value.Substring(minStart, minLength));

                var secStart = minStart + minLength + next;

                if (m.Value.IndexOf(".", StringComparison.Ordinal) != -1 &&
                    m.Value.IndexOf(".", StringComparison.Ordinal) < m.Value.IndexOf("]", StringComparison.Ordinal))
                {
                    secLength = m.Value.IndexOf(".", StringComparison.Ordinal) - secStart;
                    sec = int.Parse(m.Value.Substring(secStart, secLength));

                    var msecStart = secStart + secLength + next;
                    var msecLength = m.Value.IndexOf("]", StringComparison.Ordinal) - msecStart;
                    msec = int.Parse(m.Value.Substring(msecStart, msecLength));
                }
                else
                {
                    secLength = m.Value.IndexOf("]", StringComparison.Ordinal) - secStart;
                    sec = int.Parse(m.Value.Substring(secStart, secLength));
                }

                var lineTemp = lineWithNewLine;
                var done = true;

                while ((m = SimpleLRCFormat.LineLineRegex.Match(lineTemp)).Success)
                {
                    lineTemp = lineTemp.Replace(m.Value, "");
                    done = false;
                }

                _simpleLRCTimeAndLineArray.Add(new SimpleLRCTimeAndLine(min, sec, msec, lineTemp));

                return done;
            }

            if ((m = SimpleLRCFormat.ArtistLineStartRegex.Match(line)).Success)
            {
                _artist = line.Substring(m.Index + m.Length);
                _artist =
                    LyricUtil.CapatalizeString(_artist.Substring(0, _artist.LastIndexOf("]", StringComparison.Ordinal)));
                return true;
            }

            if ((m = SimpleLRCFormat.TitleLineStartRegex.Match(line)).Success)
            {
                _title = line.Substring(m.Index + m.Length);
                _title =
                    LyricUtil.CapatalizeString(_title.Substring(0, _title.LastIndexOf("]", StringComparison.Ordinal)));
                return true;
            }

            if ((m = SimpleLRCFormat.AlbumLineStartRegex.Match(line)).Success)
            {
                _album = line.Substring(m.Index + m.Length);
                _album =
                    LyricUtil.CapatalizeString((_album.Substring(0, _album.LastIndexOf("]", StringComparison.Ordinal))));
                return true;
            }

            if ((m = SimpleLRCFormat.OffsetLineStartRegex.Match(line)).Success)
            {
                _offset = line.Substring(m.Index + m.Length);
                _offset =
                    LyricUtil.CapatalizeString(
                        (_offset.Substring(0, _offset.LastIndexOf("]", StringComparison.Ordinal))));
                return true;
            }
            return true;
        }

        #region properties

        public string Artist
        {
            get { return _artist; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Album
        {
            get { return _album; }
        }

        public string Offset
        {
            get { return _offset; }
        }

        public string LyricAsLRC
        {
            get { return _lyricAsLRC; }
        }

        public string LyricAsPlainLyric
        {
            get { return _lyricAsPlainLyric; }
        }

        public bool IsValid
        {
            get { return _isValid; }
        }

        public SimpleLRCTimeAndLineCollection SimpleLRCTimeAndLineCollectionWithOffset
        {
            get { return _simpleLRCTimeAndLineCollectionWithOffset; }
        }

        #endregion
    }
}