using Camelonta.Boilerplate.Classes;
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

        /// <summary>
        /// Search... TODO
        /// </summary>
        /// <param name="umbracoHelper"></param>
        /// <param name="searchTerm"></param>
        /// <param name="pageNumber"></param>
        public Search(UmbracoHelper umbracoHelper, string searchTerm, int pageNumber)
        {
            SearchTerm = searchTerm;
            PageNumber = pageNumber;

            // Search
            var search = umbracoHelper.TypedSearch(searchTerm).FilterSearchResults();

            // Set properties depending on the result
            TotalResults = search.Count();
            SearchResults = search.Take(Take).ToList();
        }
    }
}