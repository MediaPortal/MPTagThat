using System.Collections.Generic;
using System.Text;

namespace DiscogsNet
{
    class Utility
    {
        private static void ProcessUtf8Data(StringBuilder res, List<byte> bytes)
        {
            res.Append(Encoding.UTF8.GetString(bytes.ToArray()));
            bytes.Clear();
        }

        public static string TrimString(string text, string trim)
        {
            bool startsWith = text.StartsWith(trim);
            bool endsWidth = text.EndsWith(trim);
            if (startsWith)
            {
                text = text.Substring(trim.Length, text.Length - trim.Length);
            }
            if (endsWidth)
            {
                text = text.Substring(0, text.Length - trim.Length);
            }
            return text;
        }

        public static string FixXmlText(string text)
        {
            StringBuilder res = new StringBuilder(text.Length);
            List<byte> utf8bytes = new List<byte>();
            int state = 0;
            StringBuilder entity = new StringBuilder();
            char c;
            for (int i = 0, l = text.Length; i < l; ++i)
            {
                c = text[i];
                if (c <= '\x1F' && c != '\x9' && c != '\xA' && c != '\xD')
                {
                    continue;
                }
                switch (state)
                {
                    case 0:
                        if (c == '&')
                        {
                            state = 1;
                        }
                        else
                        {
                            if (utf8bytes.Count > 0)
                                ProcessUtf8Data(res, utf8bytes);
                            res.Append(c);
                            state = 0;
                        }
                        break;
                    case 1:
                        if (c == '#')
                        {
                            state = 2;
                        }
                        else
                        {
                            if (utf8bytes.Count > 0)
                                ProcessUtf8Data(res, utf8bytes);
                            res.Append("&" + c);
                            state = 0;
                        }
                        break;
                    case 2:
                        if (c >= '0' && c <= '9')
                        {
                            entity.Append(c);
                        }
                        else if (c == ';')
                        {
                            utf8bytes.Add(byte.Parse(entity.ToString()));
                            entity.Clear();
                            state = 0;
                        }
                        else
                        {
                            if (utf8bytes.Count > 0)
                                ProcessUtf8Data(res, utf8bytes);
                            res.Append("&#" + entity + c);
                            entity.Clear();
                            state = 0;
                        }
                        break;
                }
            }
            switch (state)
            {
                case 1:
                    res.Append('&');
                    break;
                case 2:
                    res.Append("&#" + entity);
                    break;
            }
            return res.ToString();
        }

        public static int GetCombinedHashCode(params object[] objects)
        {
            int combinedHash = 271;
            for (int i = 0; i < objects.Length; ++i)
            {
                combinedHash *= 31;
                if (objects[i] != null)
                {
                    combinedHash += objects[i].GetHashCode();
                }
                else
                {
                    combinedHash += 271;
                }
            }
            return combinedHash;
        }
    }
}
