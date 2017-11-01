using System.IO;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Boilerplate.Core.Classes
{
    public class CoreHelpers
    {
        public static string RenderPartialToString(string viewName, object model, ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controllerContext.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary();
            var tempData = new TempDataDictionary();
            viewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public static IPublishedContent CurrentSite()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            return umbracoHelper.TypedContentAtRoot().FirstOrDefault();
        }
    }
}