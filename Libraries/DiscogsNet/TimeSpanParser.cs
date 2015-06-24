using System;
using System.Text.RegularExpressions;

namespace DiscogsNet
{
    class TimeSpanParser
    {
        private static SimpleRegex Format1 = new SimpleRegex(@"
^(?<Minutes>[0-9]+) (:|.|') (?<Seconds>[0-9]+) ""?$
", true);
        private static SimpleRegex Format2 = new SimpleRegex(@"
^(?<Hours>[0-9]+) (:|.) (?<Minutes>[0-9]+) (:|.|') (?<Seconds>[0-9]+) ""?$
", true);
        private static SimpleRegex Format3 = new SimpleRegex(@"
^:?(?<Seconds>[0-9]+)$
", true);
        private static SimpleRegex Format4 = new SimpleRegex(@"
^(?<Minutes>[0-9]+)\ min(ute)?s?$
", true);

        public static bool TryParse(string timeSpan, out TimeSpan result)
        {
            try
            {
                GroupCollection g;
                if ((g = Format1.Match(timeSpan)) != null)
                {
                    result = TimeSpan.FromSeconds(int.Parse(g["Minutes"].Value) * 60 + int.Parse(g["Seconds"].Value));
                    return true;
                }
                else if ((g = Format2.Match(timeSpan)) != null)
                {
                    result = TimeSpan.FromSeconds(int.Parse(g["Hours"].Value) * 60 * 60 + int.Parse(g["Minutes"].Value) * 60 + int.Parse(g["Seconds"].Value));
                    return true;
                }
                else if ((g = Format3.Match(timeSpan)) != null)
                {
                    result = TimeSpan.FromSeconds(int.Parse(g["Seconds"].Value));
                    return true;
                }
                else if ((g = Format4.Match(timeSpan)) != null)
                {
                    result = TimeSpan.FromSeconds(int.Parse(g["Minutes"].Value) * 60);
                    return true;
                }
            }
            catch
            {
            }

            result = TimeSpan.Zero;
            return false;
        }
    }
}
