using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Camelonta.Utilities;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Camelonta.Boilerplate.Classes
{
    public class Meta
    {
        public static string PageTeaser(IPublishedContent page, int truncate = 250)
        {
            var teaser = page.GetPropertyValue<string>("metadescription");
            if (page.HasValue("metadescription")) return teaser;

            var pageContent = page.GetPropertyValue<string>("contentMiddle");
            if (string.IsNullOrEmpty(pageContent))
                return "";

            teaser = Regex.Replace(pageContent.StripHtml(), @"\s+", " "); // Remove long whitespaces and truncate

            return teaser.Truncate(truncate);
        }

        // Every page should have an image that represets that specific page.
        /* 1. Take image from the property "facebookShareImage"
         * 2. Else: take from the header/slider (if there is any)
         * 3. else: take the first image from the content
         * 4. else: take default share-image from our template
         * */
        public static string PageMainImage(IPublishedContent page, string content = null)
        {
            var umbraco = new UmbracoHelper(UmbracoContext.Current);
            var previewImage = "";

            if (page.HasValue("facebookShareImage"))
                previewImage = umbraco.Media(page.GetPropertyValue<string>("facebookShareImage")).Url;

            if (string.IsNullOrEmpty(previewImage))
            {
                var pageContent = page.GetPropertyValue<string>("contentMiddle") ?? content;

                if (pageContent != null)
                {
                    // Get the first image from the content
                    previewImage = Regex.Match(pageContent, "<img.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                }
            }

            if (string.IsNullOrEmpty(previewImage))
            {
                previewImage = "/img/default-social-share.jpg";
            }

            return previewImage.Split('?').First();
        }


        public static string PageTitle(IPublishedContent page)
        {
            var windowTitle = page.GetPropertyValue<string>("windowTitle");

            // Default-titeln är "Sidans namn - FÖRETAGET". En egen fönstertitel öveskriver alltid default helt. 
            return string.IsNullOrEmpty(windowTitle) ? string.Format("{0} - Boilerplate", page.Name) : windowTitle;

        }

        public static string RobotsContent(IPublishedContent page)
        {
            var sb = new StringBuilder();

            sb.Append(page.AllowRobotsIndex() ? "index" : "noindex");

            sb.Append(",");

            sb.Append(page.AllowRobotsFollow() ? "follow" : "nofollow");

            return sb.ToString();
        }
    }
}