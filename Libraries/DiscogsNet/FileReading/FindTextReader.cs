using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscogsNet.FileReading
{
    internal class FindTextReader : IDisposable
    {
        TextReader Reader;
        char[] buffer;
        int bufferPosition, bufferLength;

        public FindTextReader(TextReader reader)
        {
            Reader = reader;
            buffer = new char[4 * 1024 * 1024];
            bufferPosition = bufferLength = 0;
        }

        private void _Dispose()
        {
            if (Reader != null)
            {
                Reader.Close();
                Reader.Dispose();
                Reader = null;
            }
        }
        ~FindTextReader()
        {
            _Dispose();
        }
        public void Dispose()
        {
            _Dispose();
            GC.SuppressFinalize(this);
        }

        private void Refill()
        {
            bufferLength = Reader.Read(buffer, 0, buffer.Length);
            bufferPosition = 0;
        }

        private int[] Preprocess(string pattern)
        {
            int[] overlap = new int[pattern.Length + 1];
            overlap[0] = -1;
            for (int i = 0; i < pattern.Length; ++i)
            {
                overlap[i + 1] = overlap[i] + 1;
                while (overlap[i + 1] > 0 && pattern[i] != pattern[overlap[i + 1] - 1])
                    overlap[i + 1] = overlap[overlap[i + 1] - 1] + 1;
            }
            return overlap;
        }

        private string Separator;
        private int[] PreprocessData;
        public void Prepare(string separator)
        {
            Separator = separator;
            PreprocessData = Preprocess(separator);
        }

        private int previousMatch = 0;
        public string Read()
        {
            if (PreprocessData == null)
                throw new InvalidOperationException("You should call Prepare before reading!");

            StringBuilder result = new StringBuilder(previousMatch);

            int state = 0;
            while (true)
            {
                if (bufferPosition == bufferLength)
                {
                    Refill();
                    if (bufferPosition == bufferLength)
                    {
                        return null;
                    }
                }
                char c = buffer[bufferPosition++];
                result.Append(c);
                for (; ; )
                {      // loop until break
                    if (c == Separator[state])
                    { // matches?
                        state++;        // yes, move on to next state
                        if (state == Separator.Length)
                        {   // maybe that was the last state
                            previousMatch = Math.Max(previousMatch, result.Length);
                            return result.ToString();
                        }
                        break;
                    }
                    else if (state == 0)
                    {
                        break;   // no match in state j=0, give up
                    }
                    else
                    {
                        state = PreprocessData[state];
                    } // try shorter partial match
                }
            }
        }

        public bool ReadBatch(int max, out string[] result)
        {
            List<string> batch = new List<string>(max);
            for (int i = 0; i < max; ++i)
            {
                string cur = Read();
                if (cur == null)
                {
                    result = batch.ToArray();
                    return result.Length != 0;
                }
                batch.Add(cur);
            }
            result = batch.ToArray();
            return true;
        }
    }
}
