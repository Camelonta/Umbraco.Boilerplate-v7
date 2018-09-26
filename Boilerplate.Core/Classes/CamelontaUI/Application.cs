using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;

namespace Boilerplate.Core.Classes.CamelontaUI
{
    public class Application : Umbraco.Core.ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RegisterRoutes(RouteTable.Routes);
            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "sitemap.axd", // Route name
                "sitemap.axd", // URL
                new { controller = "Sitemap", action = "Index" }  // Parameter defaults
                );
        }
    }
}