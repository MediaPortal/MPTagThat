using System;
namespace DiscogsNet.Model.Obsolete
{
    [Obsolete]
    public class SearchResult_Obsolete
    {
        public int Number { get; set; }
        public SearchResultType_Obsolete Type { get; set; }

        public string Title { get; set; }
        public string Uri { get; set; }
        public string Summary { get; set; }

        public SearchResultAggregate_Obsolete Aggregate { get; set; }

        public SearchResult_Obsolete()
        {
            this.Aggregate = new SearchResultAggregate_Obsolete(this);
        }
    }
}
