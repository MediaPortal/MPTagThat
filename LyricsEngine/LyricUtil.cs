using System;
using System.Collections.Generic;
using System.Text;

namespace LyricsEngine
{
    public class LyricUtil
    {
        static string[] parenthesesAndAlike = new string[6] { "(album", "(acoustic", "(live", "(radio", "[", "{" };
        static string[] charsToDelete = new string[11] { ".", ",", "&", "'", "!", "\"", "&", "?", "(", ")", "+" /*, "ä", "ö", "ü", "Ä", "Ö", "Ü", "ß" */ };

        // capatalize string and make ready for XML
        public static string CapatalizeString(string s)
        {
            s = s.Replace("\"", "");

            char[] space = new char[1] { ' ' };
            string[] words = s.Split(space, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                result.Append(words[i].Substring(0, 1).ToUpper() + (words[i].Length > 1 ? words[i].Substring(1, words[i].Length - 1).ToLower() : "") + " ");
            }
            return result.ToString().Trim();
        }

        public static string RemoveFeatComment(string str)
        {
            int index = str.IndexOf("(Feat");
            if (index != -1)
                str = str.Substring(0, index).Trim();
            return str;
        }

        public static string TrimForParenthesis(string str)
        {
            for (int i = 0; i < parenthesesAndAlike.Length; i++)
            {
                int index = str.IndexOf(parenthesesAndAlike[i], StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    str = str.Substring(0, index).Trim();
                }
            }
            return str;
        }

        public static string DeleteSpecificChars(string str)
        {
            for (int i = 0; i < charsToDelete.Length; i++)
            {
                str = str.Replace(charsToDelete[i], "");
            }
            return str;
        }

        public static string ChangeAnds(string str)
        {
            string strTemp = str;
            if (str.Contains("&"))
            {
                strTemp = str.Replace("&", "And");
            }
            return strTemp;
        }

        public static string ReturnEnvironmentNewLine(string str)
        {
            string justNewLine = "\n";
            string environmentNewLine = Environment.NewLine;

            if (str.Split(justNewLine.ToCharArray()).Length == str.Split(Environment.NewLine.ToCharArray()).Length)
            {
                str = str.Replace(justNewLine, Environment.NewLine);
            }

            return str;
        }
    }
}
