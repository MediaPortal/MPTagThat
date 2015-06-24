using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using DiscogsNet.Model;

namespace DiscogsNet.FileReading
{
    public class GZipMasterReleaseReader2 : IDiscogsReader<MasterRelease>
    {
        private Stream inputStream;
        private MasterReleaseReader2 masterReader;

        public double EstimatedProgress
        {
            get
            {
                return (double)inputStream.Position / (double)inputStream.Length;
            }
        }

        public GZipMasterReleaseReader2(Stream inputStream)
        {
            this.inputStream = inputStream;
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.masterReader = new MasterReleaseReader2(new StreamReader(gzipStream, Encoding.UTF8));
        }

        public GZipMasterReleaseReader2(string filename)
            : this(File.OpenRead(filename))
        {
        }

        public MasterRelease Read()
        {
            return this.masterReader.Read();
        }

        public void Dispose()
        {
            this.masterReader.Dispose();
        }

        public IEnumerable<MasterRelease> Enumerate()
        {
            return this.masterReader.Enumerate();
        }
    }
}
