using System;
using Camelonta.Boilerplate.Classes;
using Camelonta.Boilerplate.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Examine;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.Controllers
{
    public class SearchSurfaceController : SurfaceController
    {
        [HttpPost]
        public PartialViewResult GetSearchResults(string searchTerm, int nextPage)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var search = new Search(umbracoHelper, searchTerm, nextPage);
            return PartialView("~/Views/Partials/_SearchResults.cshtml", search);
        }

        [HttpPost]
        public JsonResult GetSearchSuggestions(string searchTerm)
        {
            return Json(GetSuggestedWords(searchTerm));
        }

        private List<string> GetSuggestedWords(string searchTerm)
        {
            var searchProvider = ExamineManager.Instance.DefaultSearchProvider;
            var pages = searchProvider.Search(searchTerm, true).ToList();
            var searchFields = new List<string>
            {
                "nodeName",
                "contentMiddle",
                "contentRight",
                "metadescription",
            };

            var words = new List<string>();
            foreach (var page in FilterSearchResults(pages)) // TODO: Konvertera till IPublishedContent och använd .FilterSearchResults();
            {
                var pageProperties = page.Fields.Where(field => searchFields.Any(f => f == field.Key));
                foreach (var pageProperty in pageProperties)
                {
                    var wordsInField = pageProperty.Value.StripHtml().Split(new[] { " ", @"\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var wordInField in wordsInField)
                    {
                        if (wordInField.ToLower().Contains(searchTerm.ToLower()))
                        {
                            if (!words.Contains(wordInField))
                            {
                                words.Add(wordInField);
                            }
                        }
                    }

                }
            }
            return words;
        }

        private IEnumerable<SearchResult> FilterSearchResults(IEnumerable<SearchResult> pages)
        {
            // Remove invalid document types
            pages = pages.Where(page => page.Fields.Single(field => field.Key == "nodeTypeAlias").Value.ToLower() != "site");

            // Remove pages that doesn't have search engine indexing allowed ("Tillåt EJ sökmotorindexering"-property)
            pages = pages.Where(page => AllowSearchEngineIndexing(page.Fields)); // 1 = true, 0 = false

            return pages;
        }

        private bool AllowSearchEngineIndexing(IDictionary<string, string> fields)
        {
            string fieldName = "robotsIndex";
            var field = fields.SingleOrDefault(f => f.Key == fieldName);
            return field.Equals(default(KeyValuePair<string, string>)) || field.Value == "0"; // 1 = true, 0 = false
        }
    }
}