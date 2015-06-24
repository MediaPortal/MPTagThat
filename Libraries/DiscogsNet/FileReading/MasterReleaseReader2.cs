using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DiscogsNet.Model;

namespace DiscogsNet.FileReading
{
    public class MasterReleaseReader2 : IDiscogsReader<MasterRelease>
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

        public MasterReleaseReader2(XmlReader xmlReader)
        {
            this.xmlReader = xmlReader;
            this.dataReader = new DataReader2(this.xmlReader);
        }

        public MasterReleaseReader2(TextReader textReader)
        {
            this.xmlReader = XmlReader.Create(textReader, this.GetXmlReaderSettings());
            this.dataReader = new DataReader2(this.xmlReader);
        }

        public MasterReleaseReader2(string filename)
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

        public MasterRelease Read()
        {
            if (!this.preparedReader)
            {
                this.PrepareReader();
            }

            this.xmlReader.AssertRead();

            if (this.xmlReader.IsElementEnd("masters"))
            {
                return null;
            }

            this.xmlReader.AssertElementStart("master");

            return this.dataReader.ReadMasterRelease();
        }

        private void PrepareReader()
        {
            if (!this.xmlReader.Read())
            {
                throw new EndOfStreamException();
            }

            this.xmlReader.AssertElementStart("masters");

            this.preparedReader = true;
        }

        public void Dispose()
        {
            this.xmlReader.Close();
        }

        public IEnumerable<MasterRelease> Enumerate()
        {
            MasterRelease master;
            while ((master = this.Read()) != null)
            {
                yield return master;
            }
        }
    }
}
