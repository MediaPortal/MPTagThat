using System;
using System.IO;
using System.IO.Compression;
using DiscogsNet.Model;

namespace DiscogsNet.FileReading
{
    public class GZipAsyncReleaseReader : IAsyncDiscogsReader<Release>
    {
        private Stream inputStream;
        private AsyncReleaseReader releaseReader;

        public bool IsConcurrent
        {
            get
            {
                return this.releaseReader.IsConcurrent;
            }
            set
            {
                this.releaseReader.IsConcurrent = value;
            }
        }

        public double EstimatedProgress
        {
            get
            {
                if (this.inputStream == null)
                {
                    throw new InvalidOperationException();
                }
                return (double)this.inputStream.Position / (double)this.inputStream.Length;
            }
        }

        public GZipAsyncReleaseReader(Stream inputStream)
        {
            this.inputStream = inputStream;
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.releaseReader = new AsyncReleaseReader(new StreamReader(gzipStream));
        }

        public GZipAsyncReleaseReader(string filename)
            : this(File.OpenRead(filename))
        {
        }

        public void ReadAll(Func<int, Release, bool> processor)
        {
            this.releaseReader.ReadAll(processor);
        }

        public void Dispose()
        {
            this.releaseReader.Dispose();
        }
    }
}
