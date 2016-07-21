using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.SearchCriteria;

namespace Camelonta.Boilerplate.Models
{
    public class Search
    {
        public string SearchTerm { get; private set; }

        // Value of the maximum amount of results on current "page"
        public int AmountOfTakenResult
        {
            get
            {
                // Calculate the total amount of results taken (not always correct since skip + take is hardcoded variables)
                var amountOfTakenResult = Skip + Take;
                // If total amount of results "taken" is more than TotalResults - display TotalResults instead
                return amountOfTakenResult > TotalResults ? TotalResults : amountOfTakenResult;
            }
        }

        // To be able to show/hide the "load more"-button (helpful variable so we don't have to do this logic in javascript AND razor)
        public bool MoreResultsAvailable
        {
            get { return AmountOfTakenResult < TotalResults; }
        }

        private int Take { get; }
        private int Skip { get; }
        public int TotalResults { get; }

        // Contains all search results
        public List<SearchResult> SearchResults
        {
            get;
        }

        public Search(string searchTerm, int skip, int take)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            
            ISearchCriteria query = null;

            query = searchCriteria.RawQuery(searchTerm);

            var searchResults = searcher.Search(query);

            // Set total result-count
            TotalResults = searchResults.TotalItemCount;

            SearchTerm = searchTerm;
            Take = take;
            Skip = skip;

            // Skip, take and order
            var resultCollection = searchResults.OrderByDescending(x => x.Score).Skip(skip).Take(take);

            SearchResults = resultCollection.ToList();
        }
    }
}