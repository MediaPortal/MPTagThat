using System;
using System.Collections.Generic;
using System.Text;

namespace LyricsEngine.LRC
{
    public class SimpleLRCTimeAndLine : IComparable
    {
        int min, sec, msec;
        string line;

        public SimpleLRCTimeAndLine(int min, int sec, int msec, string line)
        {
            this.min = min;
            this.sec = sec;
            this.msec = msec;
            this.line = line;
        }

        public SimpleLRCTimeAndLine IncludeOffset(int offset)
        {
            if ((this.min * 60 * 1000 + this.sec * 1000 + this.msec) < offset)
            {
                this.min = 0;
                this.sec = 0;
                this.msec = 0;
                return this;
            }

            DateTime time = new DateTime(1111, 11, 11, 0, this.min, this.sec, this.msec);
            time = time.AddMilliseconds(-offset);

            this.min = time.Minute;
            this.sec = time.Second;
            this.msec = time.Millisecond;

            return this;
        }


        public long Time
        {
            get { return min * 60 * 1000 + sec * 1000 + msec; }
        }

        public string TimeString
        {
            get { return "[" + min.ToString() + ":" + (sec.ToString().Length == 2 ? sec.ToString() : "0" + sec.ToString()) + "." + (msec.ToString().Length >= 2 ? msec.ToString().Substring(0, 2) : msec.ToString() + "0") + "]"; }

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
