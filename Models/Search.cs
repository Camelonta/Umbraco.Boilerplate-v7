using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Camelonta.Boilerplate.Models
{
    public class Search
    {
        public string SearchTerm { get; private set; }
        public int PageNumber { get; private set; }

        public int MaxResults
        {
            get
            {
                return 10;
            }
        }

        public int ShowingResults
        {
            get
            {
                if (Take > TotalResults)
                {
                    return TotalResults;
                }
                return Take;
            }
        }

        private int Take
        {
            get
            {
                return MaxResults * PageNumber;
            }
        }

        public int TotalResults
        {
            get; private set;
        }

        public List<IPublishedContent> SearchResults
        {
            get; private set;
        }

        public bool HasHiddenResults
        {
            get
            {
                return SearchResults.Any() && (SearchResults.Count < TotalResults) && (TotalResults > MaxResults);
            }
        }

        public Search(UmbracoHelper umbracoHelper, string searchTerm, int pageNumber)
        {
            SearchTerm = searchTerm;
            PageNumber = pageNumber;

            // Search
            var search = FilterSearchResults(umbracoHelper.TypedSearch(searchTerm));

            // Set properties depending on the result
            TotalResults = search.Count();
            SearchResults = search.Take(Take).ToList();
        }

        private IEnumerable<IPublishedContent> FilterSearchResults(IEnumerable<IPublishedContent> pages)
        {
            // Remove invalid document types
            pages = pages.Where(page => page.DocumentTypeAlias.ToLower() != "site");

            // Remove pages that doesn't have "Tillåt EJ sökmotorindexering" set
            pages = pages.Where(page => !page.GetPropertyValue<bool>("robotsIndex"));

            return pages;
        }
    }
}