using System.Web.Mvc;
using Camelonta.Boilerplate.Models;
using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.Controllers
{
    public class MenuSurfaceController : SurfaceController
    {
        [HttpPost]
        public PartialViewResult GetSubmenus(string id, string currentNode)
        {
            var menuItems = new MenuItems
            {
                CurrentPage = Umbraco.TypedContent(currentNode),
                Pages = Umbraco.TypedContent(id).Children
            };
            return PartialView("~/Views/Partials/_MenuItems.cshtml", menuItems);
        }
    }
}