using System;
using System.Linq;
using System.Text;

namespace DiscogsNet.Model
{
    public class PaginationRequest
    {
        public static int DefaultPerPage = 50;

        public int Page { get; set; }
        public int PerPage { get; set; }

        public PaginationRequest()
            : this(1)
        {
        }

        public PaginationRequest(int page)
            : this(page, DefaultPerPage)
        {
        }

        public PaginationRequest(int page, int perPage)
        {
            this.Page = page;
            this.PerPage = perPage;
        }

        public void AddQueryParams(StringBuilder query)
        {
            query.AddQueryParam("page", this.Page.ToString());
            query.AddQueryParam("per_page", this.PerPage.ToString());
        }

        public override bool Equals(object obj)
        {
            PaginationRequest other = obj as PaginationRequest;
            if (other == null)
            {
                return false;
            }

            return
                this.Page == other.Page &&
                this.PerPage == other.PerPage;
        }

        public override int GetHashCode()
        {
            return Utility.GetCombinedHashCode(this.Page, this.PerPage);
        }
    }
}
