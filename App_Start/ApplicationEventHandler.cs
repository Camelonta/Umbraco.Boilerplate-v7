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
            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                "~/css/normalize.css",
                "~/css/base.css",
                "~/css/layout.css",
                "~/css/module-menu.css",
                "~/css/modules.css",
                "~/css/state.css",
                "~/css/utillity.css",
                "~/css/media-queries.css"
            ));
            bundles.Add(new StyleBundle("~/bundles/styles-print").Include(
                "~/css/print.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/scripts/main.js"
            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}