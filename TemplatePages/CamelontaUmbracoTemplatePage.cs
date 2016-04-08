using Camelonta.Boilerplate.Classes;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.TemplatePages
{
    public abstract class CamelontaUmbracoTemplatePage : UmbracoTemplatePage
    {
        private List<IPublishedContent> _leftNav;
        private IPublishedContent _currentSite;

        public IPublishedContent CurrentSite
        {
            get
            {
                if (_currentSite == null)
                {
                    _currentSite = Model.Content.AncestorOrSelf(1);
                }
                return _currentSite;
            }
        }


        /// <summary>
        /// The current sites search page
        /// </summary>
        public IPublishedContent SearchPage
        {
            get { return CurrentSite.DescendantOrSelf("Search"); }
        }

        public List<IPublishedContent> LeftNavigation => _leftNav ?? (_leftNav = Model.Content.AncestorOrSelf(2).Children.FilterInvalidPages());

        public bool HideLeftNav => !LeftNavigation.Any() || Model.Content.GetPropertyValue<bool>("hideLeftNav");
    }
}