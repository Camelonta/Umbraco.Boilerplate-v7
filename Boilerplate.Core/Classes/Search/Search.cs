using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.SearchCriteria;
using Examine.LuceneEngine.SearchCriteria;

namespace Boilerplate.Core.Classes.Search
{
    public class Search
    {
        public string SearchTerm { get; private set; }

        /// <summary>
        /// Value of the maximum amount of results on current "page"
        /// </summary>
        public int AmountOfTakenResult
        {
            get
            {
                // Calculate the total amount of results taken (not always correct since skip + take is hardcoded variables)
                var amountOfTakenResult = _skip + _take;
                // If total amount of results "taken" is more than TotalResults - display TotalResults instead
                return amountOfTakenResult > TotalResults ? TotalResults : amountOfTakenResult;
            }
        }

        /// <summary>
        /// To be able to show/hide the "load more"-button (helpful variable so we don't have to do this logic in javascript AND razor)
        /// </summary>
        public bool MoreResultsAvailable
        {
            get { return AmountOfTakenResult < TotalResults; }
        }

        int _take { get; }
        int _skip { get; }
        public int TotalResults { get; private set; }

        /// <summary>
        /// Contains all search results
        /// </summary>
        public List<SearchResult> SearchResults { get; private set; }

        public List<string> IndexedFields { get; private set; }

        ISearcher _searcher;
        IIndexer _indexer;

        /// <summary>
        /// Search using ExternalSearcher and ExternalIndexer
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        public Search(string searchTerm, int skip, int take)
        {
            SearchTerm = searchTerm;
            _take = take;
            _skip = skip;

            _searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
            _indexer = ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"];
            Initialize();
        }

        /// <summary>
        /// Search using a custom Searcher/Indexer
        /// </summary>
        /// <param name="indexer"></param>
        /// <param name="searcher"></param>
        /// <param name="searchTerm"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        public Search(IIndexer indexer, ISearcher searcher, string searchTerm, int skip, int take)
        {
            SearchTerm = searchTerm;
            _take = take;
            _skip = skip;

            _searcher = searcher;
            _indexer = indexer;
           Initialize();
        }

        void Initialize()
        {
            // Set indexed fields
            IndexedFields = _indexer.IndexerData.UserFields.Select(f => f.Name).ToList();

            var searchCriteria = _searcher.CreateSearchCriteria(BooleanOperation.And).Field("robotsIndex", "0").Compile();
            ISearchCriteria query = searchCriteria.RawQuery(CreateRawQuery());
            var searchResults = _searcher.Search(query);

            // Set total result-count
            TotalResults = searchResults.TotalItemCount;

            // Skip, take and order
            var resultCollection = searchResults.OrderByDescending(x => x.Score).Skip(_skip).Take(_take);

            SearchResults = resultCollection.ToList();
        }

        string CreateRawQuery()
        {
            string term = SearchTerm.MultipleCharacterWildcard().Value;

            string query = string.Empty;

            // Search 
            foreach (string indexedField in IndexedFields)
            {
                query += indexedField + ": ";
                query += "(+" + term.Replace(" ", " +") + ")^5 ";
            }

            return query;
        }
    }
}