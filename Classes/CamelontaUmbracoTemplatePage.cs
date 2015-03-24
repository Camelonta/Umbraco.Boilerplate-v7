using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.Classes
{
    public abstract class CamelontaUmbracoTemplatePage : UmbracoTemplatePage
    {
        public IPublishedContent CurrentSite
        {
            get
            {
                if (_currentSite == null)
                {
                    _currentSite = CurrentPage.AncestorOrSelf(1);
                }
                return _currentSite;
            }
        }
        private IPublishedContent _currentSite;

        /// <summary>
        /// Get UmbracoTemplatePage.CurrentPage typed. IMPORTANT: It might contain dynamic properties that this property won´t display
        /// </summary>
        public IPublishedContent CurrentPageTyped
        {
            get { return CurrentPage; }
        }
    }
}