using System.Text.RegularExpressions;

namespace DiscogsNet.Model
{
    public static class ReleaseYearGuesser
    {
        public static int Guess(string releaseDate)
        {
            if (releaseDate == null)
            {
                return 0;
            }
            Regex regex = new Regex("(?<year>[0-9]{4})");
            Match match = regex.Match(releaseDate);
            if (match.Success)
            {
                return int.Parse(match.Value);
            }
            return 0;
        }
    }
}
