using System.IO;
using System.IO.Compression;
using DiscogsNet.Model;
using System.Collections.Generic;
using System;

namespace DiscogsNet.FileReading
{
    [Obsolete]
    public class GZipLabelReader : IDiscogsReader<Label>
    {
        private LabelReader labelReader;

        public double EstimatedProgress
        {
            get
            {
                return this.labelReader.EstimatedProgress;
            }
        }

        public GZipLabelReader(Stream inputStream)
        {
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.labelReader = new LabelReader(new StreamReader(gzipStream));
        }

        public GZipLabelReader(string filename)
            : this(File.OpenRead(filename))
        {
        }

        public Label Read()
        {
            return this.labelReader.Read();
        }

        public void Dispose()
        {
            this.labelReader.Dispose();
        }


        public IEnumerable<Label> Enumerate()
        {
            return this.labelReader.Enumerate();
        }
    }
}
