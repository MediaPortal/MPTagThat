using System;
using System.Linq;
using System.Text;

namespace DiscogsNet.Model.Search
{
    public class SearchQuery
    {
        public string Query { get; set; }
        public SearchItemType? Type { get; set; }
        public string Artist { get; set; }
        public string ReleaseTitle { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public string CatalogNumber { get; set; }
        public string Barcode { get; set; }
        public string Year { get; set; }

        public SearchQuery()
        {
        }

        public string AddQueryParams(StringBuilder query)
        {
            query.AddQueryParam("q", this.Query);
            if (this.Type != null)
            {
                query.AddQueryParam("type", this.Type.ToString().ToLower());
            }
            query.AddQueryParam("artist", this.Artist);
            query.AddQueryParam("release_title", this.ReleaseTitle);
            query.AddQueryParam("label", this.Label);
            query.AddQueryParam("title", this.Title);
            query.AddQueryParam("catno", this.CatalogNumber);
            query.AddQueryParam("barcode", this.Barcode);
            query.AddQueryParam("year", this.Year);
            if (query.Length == 0)
            {
                throw new ArgumentException("No search parts specified. Try setting Query or another property.");
            }

            return query.ToString();
        }

        public override bool Equals(object obj)
        {
            SearchQuery other = obj as SearchQuery;
            if (other == null)
            {
                return false;
            }

            return this.Query == other.Query &&
                this.Type == other.Type &&
                this.Artist == other.Artist &&
                this.ReleaseTitle == other.ReleaseTitle &&
                this.Label == other.Label &&
                this.Title == other.Title &&
                this.CatalogNumber == other.CatalogNumber &&
                this.Barcode == other.Barcode &&
                this.Year == other.Year;
        }

        public override int GetHashCode()
        {
            return Utility.GetCombinedHashCode(this.Query, this.Type, this.Artist, this.ReleaseTitle, this.Label, this.Title, this.CatalogNumber, this.Barcode, this.Year);
        }
    }
}
