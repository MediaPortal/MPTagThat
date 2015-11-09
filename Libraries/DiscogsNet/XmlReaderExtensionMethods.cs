using System;
using System.Linq;
using System.Xml;
using System.IO;

namespace DiscogsNet
{
    static class XmlReaderExtensionMethods
    {
        public static void AssertElementStart(this XmlReader reader, string name)
        {
            if (reader.NodeType != XmlNodeType.Element || reader.Name != name)
            {
                throw new XmlException("Invalid element.");
            }
        }

        public static void AssertElementEnd(this XmlReader reader, string name)
        {
            if (reader.NodeType != XmlNodeType.EndElement || reader.Name != name)
            {
                throw new XmlException("Invalid element.");
            }
        }

        public static void AssertEmptyElement(this XmlReader reader)
        {
            if (!reader.IsEmptyElement)
            {
                throw new XmlException("Invalid element.");
            }
        }

        public static void AssertNonEmptyElement(this XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                throw new XmlException("Invalid element.");
            }
        }

        public static void AssertRead(this XmlReader reader)
        {
            if (!reader.Read())
            {
                throw new EndOfStreamException("Unexpected end of stream.");
            }
        }

        public static bool IsElementStart(this XmlReader reader, string name)
        {
            return reader.NodeType == XmlNodeType.Element && reader.Name == name;
        }

        public static bool IsElementEnd(this XmlReader reader, string name)
        {
            return reader.NodeType == XmlNodeType.EndElement && reader.Name == name;
        }
    }
}
