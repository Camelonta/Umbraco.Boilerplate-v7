using System.Web.Mvc;
using Boilerplate.Core.Models;
using Umbraco.Web.Mvc;

namespace Boilerplate.Core.Controllers
{
    public class NavigationSurfaceController : SurfaceController
    {
        [HttpPost]
        public PartialViewResult GetSubmenus(string id, string currentNode, int level, string type)
        {
            var menuItems = new MenuItems
            {
                CurrentPage = Umbraco.TypedContent(currentNode),
                Pages = Umbraco.TypedContent(id).Children,
                Level = level + 1,
                Type = type
            };
            return PartialView("~/Views/Partials/_MenuItems.cshtml", menuItems);
        }
    }
}