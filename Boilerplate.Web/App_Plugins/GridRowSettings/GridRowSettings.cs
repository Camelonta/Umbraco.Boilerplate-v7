using System.Web.Optimization;
using Umbraco.Core;

namespace Boilerplate.Web.App_Plugins.GridRowSettings
{
    public class GridRowSettings: IApplicationEventHandler
    {
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            // We want to add our css FIRST in the order so they can be overwritten.
            RegisterStyles(BundleTable.Bundles);
        }

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }
        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext){}


        private static void RegisterStyles(BundleCollection bundles)
        {
            var mainCssBundle = bundles.GetBundleFor("~/bundles/styles") ?? new StyleBundle("~/bundles/styles");

            // Add the plugins style to the main bundle
            mainCssBundle.Include("~/App_Plugins/GridRowSettings/css/Bundles.min.css");
            bundles.Add(mainCssBundle);
        }
    }
}