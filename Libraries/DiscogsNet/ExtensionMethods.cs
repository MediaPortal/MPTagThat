using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiscogsNet.Model;
using Newtonsoft.Json.Linq;

namespace DiscogsNet
{
    public static class ExtensionMethods
    {
        public static string Join(this IEnumerable<object> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string Join(this IEnumerable<ReleaseArtist> source)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var releaseArtist in source)
            {
                if (string.IsNullOrEmpty(releaseArtist.NameVariation))
                {
                    sb.Append(releaseArtist.Name);
                }
                else
                {
                    sb.Append(releaseArtist.NameVariation);
                }

                if (!string.IsNullOrEmpty(releaseArtist.Join))
                {
                    if (releaseArtist.Join == ",")
                    {
                        sb.Append(", ");
                    }
                    else
                    {
                        sb.Append(" " + releaseArtist.Join + " ");
                    }
                }
            }
            return sb.ToString();
        }

        public static string JoinFixed(this IEnumerable<ReleaseArtist> source)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var releaseArtist in source)
            {
                sb.Append(releaseArtist.Aggregate.NameVariationFixed);
                if (!string.IsNullOrEmpty(releaseArtist.Join))
                {
                    if (releaseArtist.Join == ",")
                    {
                        sb.Append(", ");
                    }
                    else
                    {
                        sb.Append(" " + releaseArtist.Join + " ");
                    }
                }
            }
            return sb.ToString();
        }

        public static string TrimAndNormalizeLineEndings(this string source)
        {
            return source.Trim().Replace("\r", "").Replace("\n", "\r\n");
        }

        public static void AddIfNotNullOrWhiteSpace(this List<string> list, string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                list.Add(item);
            }
        }

        public static string[] ValueAsStringArray(this JToken token)
        {
            return token.Value<JArray>().Cast<JValue>().Select(value => value.Value<string>()).ToArray();
        }

        public static StringBuilder AddQueryParam(this StringBuilder source, string key, string value)
        {
            if (value == null)
            {
                return source;
            }

            bool hasQuery = false;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '?')
                {
                    hasQuery = true;
                    break;
                }
            }

            string delim;
            if (!hasQuery)
            {
                delim = "?";
            }
            else if ((source[source.Length - 1] == '?')
                || (source[source.Length - 1] == '&'))
            {
                delim = string.Empty;
            }
            else
            {
                delim = "&";
            }

            return source.Append(delim).Append(Uri.EscapeDataString(key))
                .Append("=").Append(Uri.EscapeDataString(value));
        }
    }
}
