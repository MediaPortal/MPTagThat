using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DiscogsNet.Model;

namespace DiscogsNet.FileReading
{
    [Obsolete]
    public class ReleaseReader : IDiscogsReader<Release>
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

        public ReleaseReader(TextReader reader)
        {
            textReader = new FindTextReader(reader);
            textReader.Prepare("</release>");
        }

        public ReleaseReader(string filename)
        {
            this.streamReader = new StreamReader(filename);
            textReader = new FindTextReader(this.streamReader);
            textReader.Prepare("</release>");
        }

        public Release Read()
        {
            string text = textReader.Read();
            if (text == null)
                return null;
            text = Utility.TrimString(text, "<releases>");
            text = Utility.TrimString(text, "</releases>");
            text = Utility.FixXmlText(text);
            XDocument doc = XDocument.Parse(text);
            return DataReader.ReadRelease(doc.Root);
        }

        public void Dispose()
        {
            this.textReader.Dispose();
        }

        public IEnumerable<Release> Enumerate()
        {
            Release release;
            while ((release = this.Read()) != null)
            {
                yield return release;
            }
        }
    }
}
