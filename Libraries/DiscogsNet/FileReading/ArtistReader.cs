using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DiscogsNet.Model;
using System;

namespace DiscogsNet.FileReading
{
    [Obsolete]
    public class ArtistReader : IDiscogsReader<Artist>
    {
        private StreamReader streamReader;
        private FindTextReader textReader;

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

        public ArtistReader(TextReader reader)
        {
            textReader = new FindTextReader(reader);
            textReader.Prepare("</artist>");
        }

        public ArtistReader(string filename)
        {
            this.streamReader = new StreamReader(filename);
            this.textReader = new FindTextReader(this.streamReader);
            this.textReader.Prepare("</artist>");
        }

        public Artist Read()
        {
            string text = textReader.Read();
            if (text == null)
                return null;
            text = Utility.TrimString(text, "<artists>");
            text = Utility.TrimString(text, "</artists>");
            text = Utility.FixXmlText(text);
            XDocument doc = XDocument.Parse(text);
            return DataReader.ReadArtist(doc.Root);
        }

        public void Dispose()
        {
            this.textReader.Dispose();
        }

        public IEnumerable<Artist> Enumerate()
        {
            Artist artist;
            while ((artist = this.Read()) != null)
            {
                yield return artist;
            }
        }
    }
}
