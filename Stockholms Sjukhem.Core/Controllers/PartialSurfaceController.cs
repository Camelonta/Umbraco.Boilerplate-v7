using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Boilerplate.Core.Controllers
{
    public class PartialSurfaceController : SurfaceController
    {
        [HttpPost]
        public PartialViewResult CookieWarning(string nodeId)
        {
            var currentSite = Umbraco.TypedContent(nodeId).AncestorOrSelf(1);
            return PartialView("~/Views/Partials/_CookieWarning.cshtml", currentSite);
        }
    }
}