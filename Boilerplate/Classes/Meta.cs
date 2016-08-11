using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Camelonta.Utilities;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Camelonta.Boilerplate.Classes.Search;

namespace Camelonta.Boilerplate.Classes
{
    public static class Meta
    {
        public static string PageTeaser(IPublishedContent page, int truncate = 250)
        {
            var teaser = page.GetPropertyValue<string>("metadescription");
            if (page.HasValue("metadescription"))
                return teaser.Truncate(truncate);

            // Get whole property, otherwise we get error "Cannot render a macro when there is no current PublishedContentRequest." if it contains Macro
            var contentMiddle = page.GetProperty("contentMiddle");
            if (contentMiddle != null && contentMiddle.HasValue)
                teaser = contentMiddle.DataValue.ToString();

            if (page.HasValue("grid"))
            {
                teaser = ExamineIndexer.GetGridText(page.GetProperty("grid").DataValue.ToString());
            }

            if (string.IsNullOrEmpty(teaser))
                return string.Empty; 

            // Clean text
            teaser = Regex.Replace(teaser.StripHtml(), @"\s+", " "); // Strip html and remove long whitespaces 

            return teaser.Truncate(truncate); // return truncated string
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
            return string.IsNullOrEmpty(windowTitle) ? page.Name : windowTitle;

        }

        // Check if the header is in the content
        public static bool AutoHeader(this IPublishedContent model)
        {
            if (model.GetPropertyValue<bool>("hideAutoHeader") || model.DocumentTypeAlias == "Newslist")
                return false;

            var content = model.GetPropertyValue<string>("contentMiddle");
            if (content.Contains("<h1"))
            {
                // If index of the header is small, it is in the beginning of the text. Else it might be further down (which is not recommended)
                return content.IndexOf("<h1") > 10;
            }

            return true;
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