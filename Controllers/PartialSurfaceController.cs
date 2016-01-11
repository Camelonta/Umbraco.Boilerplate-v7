using Camelonta.Boilerplate.Classes;
using Camelonta.Boilerplate.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.Controllers
{
    public class PartialSurfaceController : SurfaceController
    {
        [HttpPost]
        public PartialViewResult CookieWarning(string nodeId)
        {
            var currentSite = Umbraco.TypedContent(nodeId).AncestorOrSelf(1);
            return PartialView("~/Views/Partials/_CookieWarning.cshtml", currentSite);
        }

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
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var words = new List<string>();
           
            var pages = umbracoHelper.TypedSearch(searchTerm).FilterSearchResults();
            foreach(var page in pages)
            {
                var matchingWords = GetMatchingWords(page, searchTerm);
                if(matchingWords.Any())
                {
                    words.AddRange(matchingWords);
                }
            }
            return Json(words.Distinct());
        }

        private IEnumerable<string> GetMatchingWords(IPublishedContent page, string s)
        {
            var contentMiddle = page.GetProperty("contentMiddle");
            if (contentMiddle == null || !contentMiddle.HasValue)
            {
                return new List<string>();
            }

            return contentMiddle.DataValue.ToString().StripHtml().Split(' ').Where(word => word.ToLower().Contains(s.ToLower()));
        }
    }
}