using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using System.Linq;

namespace Boilerplate.Core.Controllers
{
    public class PartialSurfaceController : SurfaceController
    {
        [HttpPost]
        public PartialViewResult NavigationMenu(string nodeId)
        {
            return PartialView("~/Views/Partials/_NavigationMenu.cshtml", Umbraco.TypedContent(nodeId));
        }

        [HttpPost]
        public PartialViewResult SearchMenu(string nodeId)
        {
            return PartialView("~/Views/Partials/_SearchMenu.cshtml", Umbraco.TypedContent(nodeId));
        }

        [HttpPost]
        public PartialViewResult CookieWarning(string nodeId)
        {
            var currentSite = Umbraco.TypedContent(nodeId).AncestorOrSelf(1);
            return PartialView("~/Views/Partials/_CookieWarning.cshtml", currentSite);
        }
    }
}