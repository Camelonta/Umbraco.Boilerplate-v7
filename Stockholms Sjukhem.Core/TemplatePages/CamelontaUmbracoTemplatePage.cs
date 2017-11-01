using Boilerplate.Core.Classes;
using System.Collections.Generic;
using System.Linq;
using Boilerplate.Core.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Boilerplate.Core.TemplatePages
{
    public abstract class CamelontaUmbracoTemplatePage : UmbracoTemplatePage
    {
        private List<IPublishedContent> _leftNav;
        private IEnumerable<IPublishedContent> _sideContent;
        private IPublishedContent _currentSite;
        private Hero _hero;

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

        public Hero Hero => _hero ?? (_hero = Model.Content.GetHero());

        /// <summary>
        /// The current sites search page
        /// </summary>
        public IPublishedContent SearchPage
        {
            get { return CurrentSite.DescendantOrSelf("Search"); }
        }

        public List<IPublishedContent> LeftNavigation => _leftNav ?? (_leftNav = Model.Content.AncestorOrSelf(2).Children.FilterInvalidPages());

        public bool HideLeftNav => !LeftNavigation.Any() || Model.Content.GetPropertyValue<bool>("hideLeftNav");

        public IEnumerable<IPublishedContent> SideContent => _sideContent ?? (_sideContent = Model.Content.GetPropertyValue<IEnumerable<IPublishedContent>>("rightColumnContent", false) ?? new List<IPublishedContent>());
    }
}