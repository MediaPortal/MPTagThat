using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DiscogsNet
{
    public static class DiscogsAssert
    {
        public static void AssertName(this XElement e, string name)
        {
            if (e.Name != name)
            {
                throw new FormatException("XElement has name " + e.Name + " while asserted " + name);
            }
        }
        public static IEnumerable<XElement> AssertNames(this IEnumerable<XElement> source, string name)
        {
            foreach (var e in source)
            {
                AssertName(e, name);
            }
            return source;
        }
        public static void AssertNoAttributes(this XElement e)
        {
            if (e.HasAttributes)
            {
                throw new FormatException("XElement has attributes");
            }
        }
        public static void AssertNoElements(this XElement e)
        {
            if (e.HasElements)
            {
                throw new FormatException("XElement has elements");
            }
        }
        public static void AssertOnlyText(this XElement e)
        {
            e.AssertNoAttributes();
            e.AssertNoElements();
        }
        public static IEnumerable<XElement> AssertOnlyText(this IEnumerable<XElement> source)
        {
            foreach (var e in source)
            {
                e.AssertOnlyText();
            }
            return source;
        }

        public static void AssertNull(this string src)
        {
            if (src != null)
            {
                throw new FormatException("String is not null");
            }
        }
    }
}
