using System;
using System.Collections.Generic;
using System.Text;

namespace LyricsEngine.LRC
{
    public class SimpleLRCTimeAndLine : IComparable
    {
        long min, sec, msec;
        string line;

        public SimpleLRCTimeAndLine(long min, long sec, long msec, string line)
        {
            this.min = min;
            this.sec = sec;
            this.msec = msec;
            this.line = line;
        }

        public long Time
        {
            get { return min * 60 * 1000 + sec * 1000 + msec; }
        }

        public string TimeString
        {
            get { return min.ToString() + ":" + sec.ToString() + "." + msec.ToString(); }
        }

        public string Line
        {
            get { return line; }
        }

        public int CompareTo(object obj)
        {
            SimpleLRCTimeAndLine objSLRC = (SimpleLRCTimeAndLine)obj;
            long thisTime = this.min * 60 * 1000 + this.sec * 1000 + this.msec;
            long objTime = objSLRC.min * 60 * 1000 + objSLRC.sec * 1000 + objSLRC.msec;

            if (thisTime > objTime)
                return -1;
            else if (thisTime < objTime)
                return 1;
            else
                return 0;
        }
    }
}
