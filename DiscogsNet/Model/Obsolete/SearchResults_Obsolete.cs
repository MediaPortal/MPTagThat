namespace DiscogsNet.Model.Obsolete
{
    public class SearchResults_Obsolete
    {
        public SearchExactResult_Obsolete[] ExactResults { get; set; }

        /// <summary>
        /// This is regarding the Results property only.
        /// </summary>
        public int NumberOfResults { get; set; }

        /// <summary>
        /// This is regarding the Results property only.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// This is regarding the Results property only.
        /// </summary>
        public int End { get; set; }

        public SearchResult_Obsolete[] Results { get; set; }
    }
}
