using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DiscogsNet.Model;

namespace DiscogsNet.FileReading
{
    public class AsyncReleaseReader : IAsyncDiscogsReader<Release>
    {
        struct CharBuffer
        {
            public char[] Data;
            public int Position, Length;

            public CharBuffer(char[] data, int length)
            {
                Data = data;
                Position = 0;
                Length = length;
            }
        }

        const int BufferBacklog = 4;
        const int BatchSize = 16;
        const int StringBacklog = 4;
        const string Separator = "</release>";
        const int BufferSize = 1024 * 1024;
        const int CollectionCount = 2;

        private int consumerCount = 0;
        private StreamReader streamReader;
        private TextReader reader;
        private BlockingCollection<CharBuffer> bufferCollection;
        private CharBuffer currentBuffer;
        private int[] kmpData;

        private BlockingCollection<string[]>[] stringCollection;

        private CancellationTokenSource cancellationSource;
        private CancellationToken cancellationToken;

        public bool IsConcurrent { get; set; }

        public double EstimatedProgress
        {
            get
            {
                if (this.streamReader == null)
                {
                    throw new InvalidOperationException();
                }
                return (double)this.streamReader.BaseStream.Position / (double)this.streamReader.BaseStream.Length;
            }
        }

        public AsyncReleaseReader(TextReader textReader)
        {
            this.Init(textReader);
        }

        public AsyncReleaseReader(string filename)
        {
            this.streamReader = new StreamReader(filename);
            this.Init(this.streamReader);
        }

        private void Init(TextReader textReader)
        {
            this.IsConcurrent = true;

            consumerCount = Debugger.IsAttached ? 1 : Environment.ProcessorCount * 2;

            reader = textReader;
            kmpData = Preprocess(Separator);

            bufferCollection = new BlockingCollection<CharBuffer>(BufferBacklog);
            stringCollection = Enumerable.Range(0, CollectionCount).Select(c => new BlockingCollection<string[]>(StringBacklog)).ToArray();

            cancellationSource = new CancellationTokenSource();
            cancellationToken = cancellationSource.Token;

        }

        public void Dispose()
        {
            if (this.reader != null)
            {
                this.reader.Close();
                this.reader.Dispose();
                reader = null;
                this.bufferCollection = null;
                this.currentBuffer = default(CharBuffer);
                this.kmpData = null;
                this.stringCollection = null;
            }
        }

        public int ThreadCount
        {
            get
            {
                return this.IsConcurrent ? consumerCount : 1;
            }
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

        private int previousMatch = 0;
        private string Read()
        {
            StringBuilder result = new StringBuilder(previousMatch);

            int state = 0;
            while (true)
            {
                if (currentBuffer.Position == currentBuffer.Length)
                {
                    try
                    {
                        currentBuffer = bufferCollection.Take(cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return null;
                    }
                    catch (ArgumentException)
                    {
                        return null;
                    }
                    if (currentBuffer.Position == currentBuffer.Length)
                    {
                        return null;
                    }
                }
                char c = currentBuffer.Data[currentBuffer.Position++];
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
                        state = kmpData[state];
                    } // try shorter partial match
                }
            }
        }

        private bool ReadBatch(int max, out string[] result)
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

        public void ReadAll(Func<int, Release, bool> processor)
        {
            TaskCreationOptions taskOptions = TaskCreationOptions.LongRunning;
            Task[] consumers = new Task[this.ThreadCount];
            for (int i = 0; i < consumers.Length; ++i)
            {
                consumers[i] = Task.Factory.StartNew(delegate(object state)
                {
                    int threadNumber = (int)state;
                    int count = 0;
                    try
                    {
                        string[] stringBatch;
                        while (true)
                        {
                            BlockingCollection<string[]>.TakeFromAny(stringCollection, out stringBatch, cancellationToken);
                            foreach (string releaseStringOriginal in stringBatch)
                            {
                                // Remove <releases>...</releases>
                                string releaseString = Utility.TrimString(releaseStringOriginal, "<releases>");
                                releaseString = Utility.TrimString(releaseString, "</releases>");

                                var fixedText = Utility.FixXmlText(releaseString);
                                XDocument doc = XDocument.Parse(fixedText);
                                var release = DataReader.ReadRelease(doc.Root);
                                ++count;
                                if (!processor(threadNumber, release))
                                {
                                    cancellationSource.Cancel();
                                    throw new OperationCanceledException();
                                }
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    catch (ArgumentException)
                    {
                    }
                }, i, taskOptions);
            }
            Task stringProducer = Task.Factory.StartNew(delegate
            {
                try
                {
                    string[] stringBatch;
                    while (ReadBatch(BatchSize, out stringBatch))
                    {
                        BlockingCollection<string[]>.AddToAny(stringCollection, stringBatch, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                finally
                {
                    foreach (var coll in stringCollection)
                        coll.CompleteAdding();
                }
            }, taskOptions);
            Task dataReader = Task.Factory.StartNew(delegate
            {
                try
                {
                    int read = 0;
                    char[] NewBuffer = new char[BufferSize];
                    while ((read = reader.ReadBlock(NewBuffer, 0, BufferSize)) > 0)
                    {
                        bufferCollection.Add(new CharBuffer(NewBuffer, read), cancellationToken);
                        NewBuffer = new char[BufferSize];
                    }
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    bufferCollection.CompleteAdding();
                }
            }, taskOptions);

            Task.WaitAll(consumers);
            stringProducer.Wait();
            dataReader.Wait();
        }
    }
}