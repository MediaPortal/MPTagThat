using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DiscogsNet.Model;
using System.Text;

namespace DiscogsNet.FileReading
{
    public class GZipReleaseReader2 : IDiscogsReader<Release>
    {
        private Stream inputStream;
        private ReleaseReader2 releaseReader;

        public double EstimatedProgress
        {
            get
            {
                return (double)inputStream.Position / (double)inputStream.Length;
            }
        }

        public GZipReleaseReader2(Stream inputStream)
        {
            this.inputStream = inputStream;
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.releaseReader = new ReleaseReader2(new StreamReader(gzipStream, Encoding.UTF8));
        }

        public GZipReleaseReader2(string filename)
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
