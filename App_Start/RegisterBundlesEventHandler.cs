using System.Web.Optimization;
using Umbraco.Core;

namespace Camelonta.Boilerplate.App_Start
{
    public class RegisterBundlesEventHandler : IApplicationEventHandler
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
                cssPath + "faq.css",
                cssPath + "styles-print.css",
                cssPath + "vendor/swiper.css"
            ));

            const string scriptsPath = "~/scripts/";
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                scriptsPath + "vendor/jquery-1.11.2.min.js",
                scriptsPath + "main.js",
                scriptsPath + "menu.js",
                scriptsPath + "youtube.js",
                scriptsPath + "vendor/swiper.js",
                scriptsPath + "slider.js",
                scriptsPath + "faq.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/html5shiv").Include(
                scriptsPath + "vendor/html5shiv.js"
            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}