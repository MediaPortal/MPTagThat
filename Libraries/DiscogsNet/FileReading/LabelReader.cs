using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DiscogsNet.Model;
using System;

namespace DiscogsNet.FileReading
{
    [Obsolete]
    public class LabelReader : IDiscogsReader<Label>
    {
        private Label[] labels;
        private int position = 0;

        public double EstimatedProgress
        {
            get
            {
                return (double)position / (double)labels.Length;
            }
        }

        public LabelReader(string filename)
            : this(new StreamReader(filename))
        {
        }

        public LabelReader(TextReader reader)
        {
            string text = Utility.FixXmlText(reader.ReadToEnd());
            XDocument doc = XDocument.Parse(text);
            labels = doc.Root.Elements().Select(l => DataReader.ReadLabel(l)).ToArray();
            text = null;
            doc = null;
        }

        public Label Read()
        {
            if (position >= labels.Length)
            {
                return null;
            }
            return labels[position++];
        }

        public void Dispose()
        {
        }

        public IEnumerable<Label> Enumerate()
        {
            foreach (Label label in this.labels)
            {
                yield return label;
            }
        }
    }
}
