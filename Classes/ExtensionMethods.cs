using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Camelonta.Boilerplate.Classes
{
    public static class ExtensionMethods
    {
        public static List<IPublishedContent> FilterInvalidPages(this IEnumerable<IPublishedContent> pages)
        {
            return pages.Where(p => p.IsVisible() && p.DocumentTypeAlias != "Faqquestion" && p.DocumentTypeAlias != "folder").ToList();
        }

        public static IEnumerable<IPublishedContent> FilterSearchResults(this IEnumerable<IPublishedContent> pages)
        {
            // Remove invalid document types
            pages = pages.Where(page => page.DocumentTypeAlias.ToLower() != "site");

            // Remove pages that doesn't have search engine indexing allowed ("Tillåt EJ sökmotorindexering"-property)
            pages = pages.Where(page => !page.GetPropertyValue<bool>("robotsIndex"));

            return pages;
        }
    }
}