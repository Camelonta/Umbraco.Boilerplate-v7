using System.Collections.Generic;
using System.Linq;
using Boilerplate.Core.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Boilerplate.Core.Classes
{
    public static class ExtensionMethods
    {
        public static List<IPublishedContent> FilterInvalidPages(this IEnumerable<IPublishedContent> pages)
        {
            return pages.Where(p => p.IsVisible() && p.DocumentTypeAlias != "Faqquestion" && p.DocumentTypeAlias != "folder").ToList();
        }

        /// <summary>
        /// Get page from Content picker ID
        /// </summary>
        /// <param name="helper">UmbracoHelper</param>
        /// <param name="page">Page to get property from</param>
        /// <param name="propertyAlias">Alias of Content picker property</param>
        /// <returns></returns>
        public static IPublishedContent GetIPublishedContent(this UmbracoHelper helper, IPublishedContent page, string propertyAlias)
        {
            int id = page.GetPropertyValue<int>(propertyAlias);
            return helper.TypedContent(id);
        }

        public static Hero GetHero(this IPublishedContent page)
        {
            var imageId = page.GetPropertyValue<string>("heroImage");

            if (string.IsNullOrEmpty(imageId)) return null;

            var umbraco = new UmbracoHelper(UmbracoContext.Current);
            var image = umbraco.TypedMedia(imageId);
            
            if (image != null)
            {
                return new Hero
                {
                    Image = image.Url,
                    Text = page.GetPropertyValue<string>("heroText")
                };
            }

            return null;
        }
    }
}