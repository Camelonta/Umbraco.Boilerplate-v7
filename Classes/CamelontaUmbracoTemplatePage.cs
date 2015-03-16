using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.Classes
{
    public abstract class CamelontaUmbracoTemplatePage : UmbracoTemplatePage
    {
        public dynamic CurrentSite
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
        private dynamic _currentSite;
    }
}