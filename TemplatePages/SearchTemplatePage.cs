using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace Camelonta.Boilerplate.TemplatePages
{
    public class SearchTemplatePage : CamelontaUmbracoTemplatePage
    {
        protected string SearchTerm
        {
            get
            {
                return Request.QueryString["q"] ?? "";
            }
        }

        protected int CurrentPageNumber
        {
            get
            {
                int pageNumber;
                int.TryParse(Request.QueryString["page"], out pageNumber);
                if (pageNumber == 0)
                {
                    pageNumber = 1;
                }
                return pageNumber;
            }
        }

        protected int NextPage
        {
            get
            {
                return CurrentPageNumber + 1;
            }
        }

        public override void Execute()
        {
            
        }
    }

    public class Search
    {
        private List<IPublishedContent> _allSearchResults;
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }

        public int MaxResults
        {
            get
            {
                return 10;
            }
        }

        public int ShowResults
        {
            get
            {
                return MaxResults * PageNumber;
            }
        }

        public int TotalResults
        {
            get
            {
                return _allSearchResults.Count;
            }
        }

        public List<IPublishedContent> SearchResults
        {
            get
            {
                return _allSearchResults.Take(ShowResults).ToList();
            }
        }

        public bool HasHiddenResults
        {
            get
            {
                return (SearchResults.Count < TotalResults) && (TotalResults > MaxResults);
            }
        }

        public Search(IEnumerable<IPublishedContent> allSearchResults, string searchTerm, int pageNumber)
        {
            _allSearchResults = allSearchResults.Where(searchResult => searchResult.DocumentTypeAlias.ToLower() != "site").ToList();
            SearchTerm = searchTerm;
            PageNumber = pageNumber;
        }
    }
}