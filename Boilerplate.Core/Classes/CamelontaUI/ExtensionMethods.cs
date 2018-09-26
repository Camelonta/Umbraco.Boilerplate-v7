using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Boilerplate.Core.Classes.CamelontaUI
{
    public static class ExtensionMethods
    {
        #region IPublishedContent

        public static string NavName(this IPublishedContent page)
        {
            if (page.HasProperty("navName") && page.GetProperty("navName").HasValue)
                return page.GetProperty("navName").Value.ToString();
            return page.Name;
        }

        public static string IfPageIsCurrent(this IPublishedContent page, RenderModel model, string cssClass)
        {
            if (model.Content.Id == page.Id)
                return cssClass;
            return null;
        }

        public static string IfPageIsCurrent(this IPublishedContent page, IPublishedContent currentPage, string cssClass)
        {
            if (currentPage.Id == page.Id)
                return cssClass;
            return null;
        }

        public static string IfPageIsActive(this IPublishedContent page, RenderModel model, string cssClass)
        {
            if (page.IsAncestorOrSelf(model.Content))
                return cssClass;
            return null;
        }

        public static string IfPageIsActive(this IPublishedContent page, IPublishedContent currentPage, string cssClass)
        {
            if (page.IsAncestorOrSelf(currentPage))
                return cssClass;
            return null;
        }

        public static string GetUrl(this IPublishedContent page)
        {
            var url = string.Empty;
            if (page != null)
            {
                if (page.DocumentTypeAlias == "home" && page.Parent.DocumentTypeAlias == "Site")
                {
                    // Get current site
                    var site = page.Parent;

                    // Use URL from site to get "/" instead of "/home" for example since we're using umbracoInternalRedirectId
                    url = site.Url;
                }
                else if (page.HasProperty("externalLink"))
                {
                    url = page.GetPropertyValue<string>("externalLink");
                }
                else
                {
                    url = page.Url;
                }
            }

            return url;
        }

        public static bool AllowRobotsFollow(this IPublishedContent p)
        {
            return p.GetPropertyValue<bool>("robotsFollow") == false;
        }

        public static bool AllowRobotsIndex(this IPublishedContent p)
        {
            return p.GetPropertyValue<bool>("robotsIndex") == false;
        }

        public static IEnumerable<IPublishedContent> WhereAllowRobotsIndex(this IEnumerable<IPublishedContent> pages)
        {
            return pages.Where(AllowRobotsIndex);
        }

        #endregion

        #region String

        /// <summary>
        /// Truncate string at first word after set length
        /// </summary>
        [Obsolete("Use umbracos own truncate method")]
        public static string TruncateAtWord(this string value, int length, string endWith = "...")
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", length)) + endWith;
        }

        /// <summary>
        /// Languages are available in swedish and english
        /// </summary>
        public static string GetMonthFromNumber(this string number, bool englishMonthNames)
        {
            var fullMonthsEnglish = new string[] { "January", "February", "Mars", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var fullMonthsSwedish = new string[] { "Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December" };

            int n;
            if (!int.TryParse(number, out n))
                return number;

            if (!englishMonthNames)
                return fullMonthsSwedish[n - 1];

            return fullMonthsEnglish[n - 1];
        }

        #endregion
    }
}