using System;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Camelonta.Boilerplate.App_Start
{
    public class DateFolderEventHandler : IApplicationEventHandler
    {
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }
        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saved += ContentService_Saved;
        }

        private void ContentService_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            var contentService = ApplicationContext.Current.Services.ContentService;
            foreach (var page in e.SavedEntities)
            {
                // Not interested in anything but "create" events.
                if (!page.IsNewEntity()) return;

                // Not interested if the item being added is not a news-page.
                if (page.ContentType.Alias != "News") return;

                var now = page.ReleaseDate.HasValue ? page.ReleaseDate.Value: DateTime.Now;
                var year = now.ToString("yyyy");
                var month = now.ToString("MM");

                IContent yearDocument = null;
                foreach (var child in page.Parent().Children())
                {
                    if (child.Name == year)
                    {
                        yearDocument = child;
                        break;
                    }
                }

                // If the year folder doesn't exist, create it.
                if (yearDocument == null)
                {
                    yearDocument = contentService.CreateContentWithIdentity(year, page.ParentId, "NewsList");
                    contentService.Publish(yearDocument);
                }

                // Move the document into the year folder
                contentService.Move(page, yearDocument.Id);
            }
        }
    }
}