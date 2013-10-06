using System;
using System.Globalization;

namespace LyricsEngine.LRC
{
    public class SimpleLRCTimeAndLine : IComparable
    {
        private int _min;
        private int _sec;
        private int _msec;
        private readonly string _line;

        public SimpleLRCTimeAndLine(int min, int sec, int msec, string line)
        {
            _min = min;
            _sec = sec;
            _msec = msec;
            _line = line;
        }

        public SimpleLRCTimeAndLine IncludeOffset(int offset)
        {
            if ((_min*60*1000 + _sec*1000 + _msec) < offset)
            {
                _min = 0;
                _sec = 0;
                _msec = 0;
                return this;
            }

            var time = new DateTime(1111, 11, 11, 0, _min, _sec, _msec);
            time = time.AddMilliseconds(-offset);

            _min = time.Minute;
            _sec = time.Second;
            _msec = time.Millisecond;

            return this;
        }


        public long Time
        {
            get { return _min*60*1000 + _sec*1000 + _msec; }
        }

        public string TimeString
        {
            get
            {
                return "[" + _min + ":" +
                       (_sec.ToString(CultureInfo.InvariantCulture).Length == 2
                           ? _sec.ToString(CultureInfo.InvariantCulture)
                           : "0" + _sec) + "." +
                       (_msec.ToString(CultureInfo.InvariantCulture).Length >= 2
                           ? _msec.ToString(CultureInfo.InvariantCulture).Substring(0, 2)
                           : _msec + "0") + "]";
            }
        }

        public string Line
        {
            get { return _line; }
        }

        public int CompareTo(object obj)
        {
            var objSLRC = (SimpleLRCTimeAndLine) obj;
            long thisTime = _min*60*1000 + _sec*1000 + _msec;
            long objTime = objSLRC._min*60*1000 + objSLRC._sec*1000 + objSLRC._msec;

            if (thisTime > objTime)
            {
                return -1;
            }
            
            if (thisTime < objTime)
            {
                return 1;
            }
            return 0;
        }
    }
}
