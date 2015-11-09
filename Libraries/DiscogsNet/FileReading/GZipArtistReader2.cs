using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using DiscogsNet.Model;

namespace DiscogsNet.FileReading
{
    public class GZipArtistReader2 : IDiscogsReader<Artist>
    {
        private Stream inputStream;
        private ArtistReader2 artistReader;

        public double EstimatedProgress
        {
            get
            {
                return (double)inputStream.Position / (double)inputStream.Length;
            }
        }

        public GZipArtistReader2(Stream inputStream)
        {
            this.inputStream = inputStream;
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.artistReader = new ArtistReader2(new StreamReader(gzipStream, Encoding.UTF8));
        }

        public GZipArtistReader2(string filename)
            : this(File.OpenRead(filename))
        {
        }

        public Artist Read()
        {
            return this.artistReader.Read();
        }

        public void Dispose()
        {
            this.artistReader.Dispose();
        }

        public IEnumerable<Artist> Enumerate()
        {
            return this.artistReader.Enumerate();
        }
    }
}
