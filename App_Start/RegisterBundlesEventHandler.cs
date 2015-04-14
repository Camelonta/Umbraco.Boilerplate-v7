using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Optimization;
using Camelonta.Utilities;
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
            // CSS
            const string cssPath = "~/css/";
            var cssFiles = new List<string>
            {
                "vendor/normalize.css", // Should be before base.css
                "base.css",
                "layout.css",
                "form.css",
                "nav.css",
                "modules.css",
                "state.css",
                "utillity.css",
                "vendor/bootstrap.css",
                "media-queries.css",
                "typo.css",
                "faq.css",
                "styles-print.css",
                "vendor/swiper.css"
            }.Select(cssFile => cssPath + cssFile).ToArray(); // Add CSS-path
            var styleBundle = new StyleBundle("~/bundles/styles").Include(cssFiles);
            styleBundle.Orderer = Bundles.AsIsBundleOrderer;
            bundles.Add(styleBundle);
            
            // Scripts
            const string scriptsPath = "~/scripts/";
            var jsFiles = new List<string>
            {
                "vendor/jquery-1.11.2.min.js",
                "vendor/swiper.jquery.min.js",
                "main.js",
                "menu.js",
                "youtube.js",
                "slider.js",
                "faq.js"
            }.Select(jsFile => scriptsPath + jsFile).ToArray(); // Add scripts-path;
            var scriptBundle = new ScriptBundle("~/bundles/scripts").Include(jsFiles);
            scriptBundle.Orderer = Bundles.AsIsBundleOrderer;
            bundles.Add(scriptBundle);

            bundles.Add(new ScriptBundle("~/bundles/html5shiv").Include(
                scriptsPath + "vendor/html5shiv.js"
            ));

            Bundles.DisableBundles();
        }
    }
}