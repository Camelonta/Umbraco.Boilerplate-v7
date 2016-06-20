using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.SearchCriteria;
using Umbraco.Core.Models;

namespace Camelonta.Boilerplate.Models
{
    public class Search
    {
        public string SearchTerm { get; private set; }

        public int AmountOfTakenResult
        {
            get
            {
                var amountOfTakenResult = Skip + Take;
                // If we are displaying less results than Take - show total amount of results
                return amountOfTakenResult > TotalResults ? TotalResults : amountOfTakenResult;
            }
        }

        public bool MoreResultsAvailable
        {
            get { return AmountOfTakenResult < TotalResults; }
        }

        private int Take { get; }
        private int Skip { get; }
        public int TotalResults { get; }

        public List<IPublishedContent> SearchResultsAsPage
        {
            get; private set;
        }

        public List<SearchResult> SearchResults
        {
            get;
        }

        //public bool HasMoreResults
        //{
        //    get
        //    {
        //        return SearchResults.Count <= Take;
        //    }
        //}

        /// <summary>
        /// Search... TODO
        /// </summary>
        public Search(string searchTerm, int skip, int take)
        {
            SearchTerm = searchTerm;
            Take = take;
            Skip = skip;



            var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            ISearchCriteria query = null;

            query = searchCriteria.RawQuery(searchTerm);

            var searchResults = searcher.Search(query);

            // Set total result-count
            TotalResults = searchResults.TotalItemCount;

            // Skip, take and order
            var resultCollection = searchResults.Skip(skip).Take(take).OrderByDescending(x => x.Score);

            SearchResults = resultCollection.ToList();



            //PageNumber = pageNumber;

            // Search
            //var search = umbracoHelper.TypedSearch(searchTerm).Skip(skip).Take(take).FilterSearchResults();

            ////var searchProvider = ExamineManager.Instance.DefaultSearchProvider;
            ////var searchResults = searchProvider.Search(searchTerm, true);
            ////TotalResults = searchResults.TotalItemCount;
            ////SearchResults = searchResults.Take(Take).ToList(); // TODO: Konvertera till IPublishedContent

            //// Set properties depending on the result
            //TotalResults = search.Count();
            //SearchResults = search.Skip(skip).Take(take).ToList();
        }
    }
}