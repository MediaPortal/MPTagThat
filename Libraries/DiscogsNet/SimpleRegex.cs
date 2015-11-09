using System.Text.RegularExpressions;

namespace DiscogsNet
{
    class SimpleRegex
    {
        Regex Regex;

        public SimpleRegex(string pattern, bool compiled = false)
        {
            RegexOptions opts = RegexOptions.IgnorePatternWhitespace;
            if (compiled)
                opts |= RegexOptions.Compiled;
            Regex = new Regex(pattern, opts);
        }

        public GroupCollection Match(string input)
        {
            Match match = Regex.Match(input);
            if (match.Success)
            {
                return match.Groups;
            }
            else
            {
                return null;
            }
        }
    }
}
