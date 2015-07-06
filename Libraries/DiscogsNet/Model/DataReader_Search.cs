using System;
using System.Linq;
using System.Linq;
using System.Xml.Linq;
using DiscogsNet.Model.Obsolete;

namespace DiscogsNet.Model
{
    public partial class DataReader
    {
        private static SearchResultType_Obsolete ParseSearchResultType(string type)
        {
            SearchResultType_Obsolete searchResultType;
            if (Enum.TryParse<SearchResultType_Obsolete>(type, true, out searchResultType))
            {
                return searchResultType;
            }
            else
            {
                throw new Exception("Unknown search result type: " + type);
            }
        }

        private static SearchExactResult_Obsolete ReadSearchExactResult(XElement el)
        {
            el.AssertName("result");

            SearchExactResult_Obsolete result = new SearchExactResult_Obsolete();

            foreach (XAttribute attr in el.Attributes())
            {
                if (attr.Name == "num")
                {
                    result.Number = int.Parse(attr.Value);
                }
                else if (attr.Name == "type")
                {
                    result.Type = ParseSearchResultType(attr.Value);
                }
                else
                {
                    throw new Exception("Unknown exact search result attribute: " + attr.Name);
                }
            }

            foreach (XElement e in el.Elements())
            {
                if (e.Name == "title")
                {
                    e.AssertOnlyText();
                    result.Title = e.Value.Trim();
                }
                else if (e.Name == "uri")
                {
                    e.AssertOnlyText();
                    result.Uri = e.Value.Trim();
                }
                else if (e.Name == "anv")
                {
                    e.AssertOnlyText();
                    result.ArtistNameVariation = e.Value.Trim();
                }
                else
                {
                    throw new Exception("Unknown exact search result element: " + e.Name);
                }
            }

            return result;
        }

        private static SearchExactResult_Obsolete[] ReadSearchExactResults(XElement el)
        {
            el.AssertName("exactresults");
            el.AssertNoAttributes();
            return el.Elements().Select(e => ReadSearchExactResult(e)).ToArray();
        }

        private static SearchResult_Obsolete ReadSearchResult(XElement el)
        {
            el.AssertName("result");

            SearchResult_Obsolete result = new SearchResult_Obsolete();

            foreach (XAttribute attr in el.Attributes())
            {
                if (attr.Name == "num")
                {
                    result.Number = int.Parse(attr.Value);
                }
                else if (attr.Name == "type")
                {
                    result.Type = ParseSearchResultType(attr.Value);
                }
                else
                {
                    throw new Exception("Unknown exact search result attribute: " + attr.Name);
                }
            }

            foreach (XElement e in el.Elements())
            {
                if (e.Name == "title")
                {
                    e.AssertOnlyText();
                    result.Title = e.Value.Trim();
                }
                else if (e.Name == "uri")
                {
                    e.AssertOnlyText();
                    result.Uri = e.Value.Trim();
                }
                else if (e.Name == "summary")
                {
                    e.AssertOnlyText();
                    result.Summary = e.Value.Trim();
                }
                else
                {
                    throw new Exception("Unknown exact search result element: " + e.Name);
                }
            }

            return result;
        }

        private static SearchResult_Obsolete[] ReadSearchResultsInternal(XElement el, ref int numberOfResults, ref int start, ref int end)
        {
            el.AssertName("searchresults");

            foreach (XAttribute attr in el.Attributes())
            {
                if (attr.Name == "numResults")
                {
                    numberOfResults = int.Parse(attr.Value);
                }
                else if (attr.Name == "start")
                {
                    start = int.Parse(attr.Value);
                }
                else if (attr.Name == "end")
                {
                    end = int.Parse(attr.Value);
                }
            }

            return el.Elements().Select(e => ReadSearchResult(e)).ToArray();
        }

        public static SearchResults_Obsolete ReadSearchResults(XElement searchResults)
        {
            SearchResults_Obsolete result = new SearchResults_Obsolete();

            foreach (XElement e in searchResults.Elements())
            {
                if (e.Name == "exactresults")
                {
                    result.ExactResults = ReadSearchExactResults(e);
                }
                else if (e.Name == "searchresults")
                {
                    int numberOfResults = result.NumberOfResults;
                    int start = result.Start;
                    int end = result.End;

                    result.Results = ReadSearchResultsInternal(e, ref numberOfResults, ref start, ref end);

                    result.NumberOfResults = numberOfResults;
                    result.Start = start;
                    result.End = end;
                }
                else
                {
                    throw new Exception("Unknown search element: " + e.Name);
                }
            }

            return result;
        }
    }
}
