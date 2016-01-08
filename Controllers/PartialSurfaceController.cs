using Camelonta.Boilerplate.TemplatePages;
using System.Web.Mvc;
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
    }
}