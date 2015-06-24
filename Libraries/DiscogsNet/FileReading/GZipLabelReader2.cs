using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using DiscogsNet.Model;

namespace DiscogsNet.FileReading
{
    public class GZipLabelReader2 : IDiscogsReader<Label>
    {
        private Stream inputStream;
        private LabelReader2 labelReader;

        public double EstimatedProgress
        {
            get
            {
                return (double)inputStream.Position / (double)inputStream.Length;
            }
        }

        public GZipLabelReader2(Stream inputStream)
        {
            this.inputStream = inputStream;
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.labelReader = new LabelReader2(new StreamReader(gzipStream, Encoding.UTF8));
        }

        public GZipLabelReader2(string filename)
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
