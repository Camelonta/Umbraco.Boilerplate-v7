using System.Collections.Generic;
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
                "vendor/swiper.css",
                "vendor/bootstrap.css",
                "base.css",
                "layout.css",
                "form.css",
                "modules/_faq.css",
                "modules/_video.css",
                "modules/_search-form.css",
                "nav.css",
                "animations.css",
                "state.css",
                "utillity.css",
                "typo.css",
                "faq.css",
                "media-queries.css",
                "styles-print.css"
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
                "vendor/modernizr.js",
                "main.js",
                "nav.js",
                "helper.js",
                "youtube.js",
                "slider.js",
                "faq.js"
            }.Select(jsFile => scriptsPath + jsFile).ToArray(); // Add scripts-path;
            var scriptBundle = new ScriptBundle("~/bundles/scripts").Include(jsFiles);
            scriptBundle.Orderer = Bundles.AsIsBundleOrderer;
            bundles.Add(scriptBundle);

            var ltIe9Files = new List<string>
            {
                 "vendor/html5shiv.js",
                 "vendor/respond.min.js" //Polyfill for media-queries. Needed for the bootstrap grid to function correctly.
            }.Select(jsFile => scriptsPath + jsFile).ToArray();
            bundles.Add(new ScriptBundle("~/bundles/ltIe9Scripts").Include(ltIe9Files));

            Bundles.DisableBundles();
        }
    }
}