using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DiscogsNet.Model;
using System;

namespace DiscogsNet.FileReading
{
    [Obsolete]
    public class GZipReleaseReader : IDiscogsReader<Release>
    {
        private Stream inputStream;
        private ReleaseReader releaseReader;

        public double EstimatedProgress
        {
            get
            {
                return (double)inputStream.Position / (double)inputStream.Length;
            }
        }

        public GZipReleaseReader(Stream inputStream)
        {
            this.inputStream = inputStream;
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.releaseReader = new ReleaseReader(new StreamReader(gzipStream));
        }

        public GZipReleaseReader(string filename)
            : this(File.OpenRead(filename))
        {
        }

        public Release Read()
        {
            return this.releaseReader.Read();
        }

        public void Dispose()
        {
            this.releaseReader.Dispose();
        }

        public IEnumerable<Release> Enumerate()
        {
            return this.releaseReader.Enumerate();
        }
    }
}
