using System.Web.Optimization;
using Umbraco.Core;

namespace Camelonta.Boilerplate.App_Start
{
    public class ApplicationEventHandler : IApplicationEventHandler
    {
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }
        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RegisterBundles(BundleTable.Bundles);
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            const string cssPath = "~/css/";
            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                cssPath + "vendor/normalize.css",
                cssPath + "base.css",
                cssPath + "layout.css",
                cssPath + "form.css",
                cssPath + "nav.css",
                cssPath + "modules.css",
                cssPath + "state.css",
                cssPath + "utillity.css",
                cssPath + "vendor/bootstrap.css",
                cssPath + "media-queries.css",
                cssPath + "typo.css",
                cssPath + "styles-print.css"
            ));

            const string scriptsPath = "~/scripts/";
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                scriptsPath + "main.js",
                scriptsPath + "menu.js",
                scriptsPath + "youtube.js"
            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}