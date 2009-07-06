using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace LyricsEngine.LRC
{
    public class SimpleLRC
    {
        string artist;
        string title;
        string offset;
        string album;
        string lyric;
        bool isValid;

        ArrayList lyricLines;
        private ArrayList simpleLRCTimeAndLineArray;
        private ArrayList simpleLRCTimeAndLineArrayWithOffset;
        SimpleLRCTimeAndLineCollection simpleLRCTimeAndLineCollection;
        SimpleLRCTimeAndLineCollection simpleLRCTimeAndLineCollectionWithOffset;


        /// <summary>
        /// Constructor used in the MyLyrics configuration
        /// </summary>
        /// <param name="file"></param>
        public SimpleLRC(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            TextReader textReader = new StreamReader(file);

            string line = "";
            lyricLines = new System.Collections.ArrayList();
            simpleLRCTimeAndLineArray = new System.Collections.ArrayList();

            while ((line = textReader.ReadLine()) != null)
            {
                bool done = false;
                bool originalLine = true;
                while (done == false)
                {
                    done = getLRCinfoFromFile(ref line, originalLine);
                    originalLine = false;
                }
            }

            if (simpleLRCTimeAndLineArray.Count > 0)
            {
                simpleLRCTimeAndLineCollection = new SimpleLRCTimeAndLineCollection((SimpleLRCTimeAndLine[])simpleLRCTimeAndLineArray.ToArray(typeof(SimpleLRCTimeAndLine)));
                isValid = true;
            }

            textReader.Close();
        }


        public SimpleLRC(string artist, string title, string lyric)
        {
            this.artist = artist;
            this.title = title;

            string[] separators = new string[1] { "\n" };
            string[] lines = lyric.Split(separators, StringSplitOptions.None);
            string line = "";
            lyricLines = new System.Collections.ArrayList();
            simpleLRCTimeAndLineArray = new System.Collections.ArrayList();

            for (int i = 0; i < lines.Length; i++)
            {
                bool done = false;
                bool originalLine = true;
                line = lines[i];
                while (done == false)
                {
                    done = getLRCinfoFromFile(ref line, originalLine);
                    originalLine = false;
                }
            }

            if (simpleLRCTimeAndLineArray.Count > 0)
            {
                simpleLRCTimeAndLineCollection = new SimpleLRCTimeAndLineCollection((SimpleLRCTimeAndLine[])simpleLRCTimeAndLineArray.ToArray(typeof(SimpleLRCTimeAndLine)));
                isValid = true;
            }

            simpleLRCTimeAndLineArrayWithOffset = new System.Collections.ArrayList();

            int offsetInt = 0;

            if (int.TryParse(offset, out offsetInt))
            {
                for (int i = 0; i < simpleLRCTimeAndLineArray.Count; i++)
                {
                    simpleLRCTimeAndLineArrayWithOffset.Add(((SimpleLRCTimeAndLine)simpleLRCTimeAndLineArray[i]).IncludeOffset(offsetInt));
                }

                simpleLRCTimeAndLineCollectionWithOffset = new SimpleLRCTimeAndLineCollection((SimpleLRCTimeAndLine[])simpleLRCTimeAndLineArrayWithOffset.ToArray(typeof(SimpleLRCTimeAndLine)));
            }
            else
            {
                simpleLRCTimeAndLineCollectionWithOffset = simpleLRCTimeAndLineCollection;
            }
        }

        private bool getLRCinfoFromFile(ref string line, bool originalLine)
        {
            System.Text.RegularExpressions.Match m;

            if ((m = LRC.SimpleLRCFormat.LineLineRegex.Match(line)).Success)
            {
                line = line.Trim();
                int index;
                if ((index = m.Value.IndexOf("[")) > 0)
                {
                    line = line.Substring(index);
                }

                string lineWithTimeAndNewLine = line + Environment.NewLine;

                // if a line with multiple timetags, only add the first which is the complete with all tags.
                if (originalLine)
                {
                    lyric += lineWithTimeAndNewLine;
                }

                // we update the line for potential further time-tags. This will natural not be regarded as an original line
                // and will therefore not be added to the lyric. It will however be added to the LRC-object just as every other line
                line = line.Replace(m.Value, "");

                string lineWithNewLine = line + Environment.NewLine;
                lyricLines.Add(lineWithNewLine);

                int NEXT = 1;
                int minStart, minLength, secStart, secLength, msecStart, msecLength = 0;
                int min, sec, msec = 0;
                sec = 0;

                minStart = NEXT;
                minLength = m.Value.IndexOf(":") - minStart;
                min = int.Parse(m.Value.Substring(minStart, minLength));

                secStart = minStart + minLength + NEXT;

                if (m.Value.IndexOf(".") != -1 && m.Value.IndexOf(".") < m.Value.IndexOf("]"))
                {
                    secLength = m.Value.IndexOf(".") - secStart;
                    sec = int.Parse(m.Value.Substring(secStart, secLength));

                    msecStart = secStart + secLength + NEXT;
                    msecLength = m.Value.IndexOf("]") - msecStart;
                    msec = int.Parse(m.Value.Substring(msecStart, msecLength));
                }
                else
                {
                    secLength = m.Value.IndexOf("]") - secStart;
                    sec = int.Parse(m.Value.Substring(secStart, secLength));
                }

                string lineTemp = lineWithNewLine;
                bool done = true;

                while ((m = LRC.SimpleLRCFormat.LineLineRegex.Match(lineTemp)).Success)
                {
                    lineTemp = lineTemp.Replace(m.Value, "");
                    done = false;
                }

                simpleLRCTimeAndLineArray.Add(new SimpleLRCTimeAndLine(min, sec, msec, lineTemp));

                return done;
            }

            else if ((m = LRC.SimpleLRCFormat.ArtistLineStartRegex.Match(line)).Success)
            {
                artist = line.Substring(m.Index + m.Length);
                artist = LyricsEngine.LyricUtil.CapatalizeString(artist.Substring(0, artist.LastIndexOf("]")));
                return true;
            }

            else if ((m = LRC.SimpleLRCFormat.TitleLineStartRegex.Match(line)).Success)
            {
                title = line.Substring(m.Index + m.Length);
                title = LyricsEngine.LyricUtil.CapatalizeString(title.Substring(0, title.LastIndexOf("]")));
                return true;
            }

            else if ((m = LRC.SimpleLRCFormat.AlbumLineStartRegex.Match(line)).Success)
            {
                album = line.Substring(m.Index + m.Length);
                album = LyricsEngine.LyricUtil.CapatalizeString((album.Substring(0, album.LastIndexOf("]"))));
                return true;
            }

            else if ((m = LRC.SimpleLRCFormat.OffsetLineStartRegex.Match(line)).Success)
            {
                offset = line.Substring(m.Index + m.Length);
                offset = LyricsEngine.LyricUtil.CapatalizeString((offset.Substring(0, offset.LastIndexOf("]"))));
                return true;
            }
            else
            {
                return true;
            }
        }

        #region properties
        public string Artist
        {
            get { return artist; }
        }

        public string Title
        {
            get { return title; }
        }

        public string Album
        {
            get { return album; }
        }

        public string Offset
        {
            get { return offset; }
        }

        public string Lyric
        {
            get { return lyric; }
        }

        public bool IsValid
        {
            get { return isValid; }
        }

        public SimpleLRCTimeAndLineCollection SimpleLRCTimeAndLineCollectionWithOffset
        {
            get { return simpleLRCTimeAndLineCollectionWithOffset; }
        }
        #endregion
    }
}
