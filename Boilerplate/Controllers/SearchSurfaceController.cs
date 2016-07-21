using System;
using Camelonta.Boilerplate.Models;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Camelonta.Boilerplate.Classes;
using Examine;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.Controllers
{
    public class SearchSurfaceController : SurfaceController
    {
        [HttpPost]
        public ActionResult GetSearchResults(string searchTerm, int skip, int take)
        {
            var search = new Search(searchTerm, skip, take);

            dynamic result = new ExpandoObject();
            result.html = CoreHelpers.RenderPartialToString("~/Views/Partials/_SearchResults.cshtml", search, ControllerContext);
            result.amountOfTakenResult = search.AmountOfTakenResult;
            result.moreResultsAvailable = search.MoreResultsAvailable;
            result.totalResultCount = search.TotalResults;

            var json = JsonConvert.SerializeObject(result);

            return Content(json, "application/json");
        }

        [HttpPost]
        public JsonResult GetSearchSuggestions(string searchTerm)
        {
            return Json(GetSuggestedWords(searchTerm));
        }



        // TODO - This can have more accurate results: http://blog.aabech.no/archive/building-a-spell-checker-for-search-in-umbraco/
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
            foreach (var page in pages)
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
    }
}