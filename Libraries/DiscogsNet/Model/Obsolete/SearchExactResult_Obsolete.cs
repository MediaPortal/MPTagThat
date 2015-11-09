namespace DiscogsNet.Model.Obsolete
{
    public class SearchExactResult_Obsolete
    {
        public int Number { get; set; }
        public SearchResultType_Obsolete Type { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// This property is set only on artist matches, which matched an ANV.
        /// </summary>
        public string ArtistNameVariation { get; set; }

        public string Uri { get; set; }

        public SearchExactResultAggregate_Obsolete Aggregate { get; set; }

        public SearchExactResult_Obsolete()
        {
            this.Aggregate = new SearchExactResultAggregate_Obsolete(this);
        }
    }
}
