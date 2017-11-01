using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Boilerplate.Core.Classes;
using Newtonsoft.Json;
using Umbraco.Web.Mvc;
using Boilerplate.Core.Classes.Search;

namespace Boilerplate.Core.Controllers
{
    public class SearchSurfaceController : SurfaceController
    {
        [HttpPost]
        public ActionResult GetSearchResults(string searchTerm, int skip, int take)
        {
            var search = new CamelontaSearch(searchTerm, skip, take);

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
            var suggestions = UmbracoSpellChecker.Instance.SuggestSimilar(searchTerm, 20).ToList();
            return Json(suggestions);
        }
    }
}