using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using Camelonta.Utilities;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Boilerplate.Core.Classes.Search;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Values;
using System.Web;

namespace Boilerplate.Core.Classes
{
    public static class Meta
    {
        public static string PageTeaser(IPublishedContent page, int truncate = 250)
        {
            var teaser = page.GetPropertyValue<string>("metadescription");
            if (page.HasValue("metadescription"))
                return teaser.Truncate(truncate);

            teaser = PageMainContent(page);

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
        public static string PageMainImage(IPublishedContent page)
        {
            var umbraco = new UmbracoHelper(UmbracoContext.Current);
            var previewImage = "";

            if (page.HasValue("facebookShareImage"))
                previewImage = umbraco.Media(page.GetPropertyValue<string>("facebookShareImage")).Url;

            if (string.IsNullOrEmpty(previewImage))
            {
                var pageContent = PageMainContent(page);

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

        private static string PageMainContent(IPublishedContent page)
        {
            var content = "";
            // Get whole property, otherwise we get error "Cannot render a macro when there is no current PublishedContentRequest." if it contains Macro
            var contentMiddle = page.GetProperty("contentMiddle");
            if (contentMiddle != null && contentMiddle.HasValue)
                content = contentMiddle.DataValue.ToString();

            if (page.HasValue("grid"))
                content = GetGridText(page.GetProperty("grid").DataValue.ToString());

            return content;
        }

        public static string GetGridText(string content)
        {
            GridDataModel grid = GridDataModel.Deserialize(content);

            StringBuilder combined = new StringBuilder();

            foreach (GridControl ctrl in grid.GetAllControls())
            {

                switch (ctrl.Editor.Alias)
                {

                    case "rte":
                        {

                            // Get the HTML value
                            string html = ctrl.GetValue<GridControlRichTextValue>().Value;

                            // Strip any HTML tags so we only have text
                            string text = Regex.Replace(html, "<.*?>", "");

                            // Extra decoding may be necessary
                            text = HttpUtility.HtmlDecode(text);

                            // Now append the text
                            combined.AppendLine(text);

                            break;

                        }

                    case "media":
                        {
                            GridControlMediaValue media = ctrl.GetValue<GridControlMediaValue>();
                            combined.AppendLine(media.Caption);
                            break;
                        }

                    case "headline":
                    case "quote":
                        {
                            combined.AppendLine(ctrl.GetValue<GridControlTextValue>().Value);
                            break;
                        }

                }

            }

            return combined.ToString();
        }

        public static string PageTitle(IPublishedContent page)
        {
            var windowTitle = page.GetPropertyValue<string>("windowTitle");

            // Default-titeln är "Sidans namn - FÖRETAGET". En egen fönstertitel öveskriver alltid default helt. 
            return string.IsNullOrEmpty(windowTitle) ? page.Name : windowTitle;

        }

        // Check if the header is in the content
        public static bool AutoHeader(this IPublishedContent page)
        {
            if (page.GetPropertyValue<bool>("hideAutoHeader") || page.DocumentTypeAlias == "Newslist")
                return false;

            var content = PageMainContent(page);

            // If content does NOT contain <h1> = set autoheader (return true)
            // If content DOES contain <h1> - DON'T set autoheder (return false).  h1 should only occur once at the top of the page. All other headers should be h2,h3 etc.
            return !content.Contains("<h1");
        }
        

        public static bool GridHasContent(IPublishedContent page, string gridAlias)
        {
            var grid = page.GetProperty(gridAlias);
            if (grid != null && grid.HasValue)
            {
                var content = grid.DataValue.ToString();
                GridDataModel model = GridDataModel.Deserialize(content);
                var controls = model.GetAllControls();

                return controls != null && controls.Any();
            }

            return false;
        }


        public static string RobotsContent(IPublishedContent page)
        {
            var sb = new StringBuilder();

            //sb.Append(page.AllowRobotsIndex() ? "index" : "noindex");

            //sb.Append(",");

            //sb.Append(page.AllowRobotsFollow() ? "follow" : "nofollow");

            return sb.ToString();
        }
    }
}