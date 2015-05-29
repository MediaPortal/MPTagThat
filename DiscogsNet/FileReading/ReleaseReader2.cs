using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using DiscogsNet.Model;
using System.Text;

namespace DiscogsNet.FileReading
{
    public class ReleaseReader2 : IDiscogsReader<Release>
    {
        private StreamReader streamReader;
        private XmlReader xmlReader;
        private DataReader2 dataReader;
        private bool preparedReader;

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

        public ReleaseReader2(XmlReader xmlReader)
        {
            this.xmlReader = xmlReader;
            this.dataReader = new DataReader2(this.xmlReader);
        }

        public ReleaseReader2(TextReader textReader)
        {
            this.xmlReader = XmlReader.Create(textReader, this.GetXmlReaderSettings());
            this.dataReader = new DataReader2(this.xmlReader);
        }

        public ReleaseReader2(string filename)
        {
            this.streamReader = new StreamReader(filename, Encoding.UTF8);
            this.xmlReader = XmlReader.Create(this.streamReader, this.GetXmlReaderSettings());
            this.dataReader = new DataReader2(this.xmlReader);
        }

        private XmlReaderSettings GetXmlReaderSettings()
        {
            return new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document,
                IgnoreComments = true,
                IgnoreWhitespace = true,
                CheckCharacters = false
            };
        }

        public Release Read()
        {
            if (!this.preparedReader)
            {
                this.PrepareReader();
            }

            this.xmlReader.AssertRead();

            if (this.xmlReader.IsElementEnd("releases"))
            {
                return null;
            }

            this.xmlReader.AssertElementStart("release");

            return this.dataReader.ReadRelease();
        }

        private void PrepareReader()
        {
            if (!this.xmlReader.Read())
            {
                throw new EndOfStreamException();
            }

            this.xmlReader.AssertElementStart("releases");

            this.preparedReader = true;
        }

        public void Dispose()
        {
            this.xmlReader.Close();
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
