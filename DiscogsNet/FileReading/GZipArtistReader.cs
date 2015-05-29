using System.IO;
using System.IO.Compression;
using DiscogsNet.Model;
using System.Collections.Generic;
using System;

namespace DiscogsNet.FileReading
{
    [Obsolete]
    public class GZipArtistReader : IDiscogsReader<Artist>
    {
        private Stream inputStream;
        private ArtistReader artistReader;

        public double EstimatedProgress
        {
            get
            {
                return (double)inputStream.Position / (double)inputStream.Length;
            }
        }

        public GZipArtistReader(Stream inputStream)
        {
            this.inputStream = inputStream;
            GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            this.artistReader = new ArtistReader(new StreamReader(gzipStream));
        }

        public GZipArtistReader(string filename)
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
