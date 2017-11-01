using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Boilerplate.Core
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
            //var contentService = ApplicationContext.Current.Services.ContentService;
            //var pageOrganizer = new Camelonta.Utilities.PageOrganizer();
            //pageOrganizer.MoveToDatefolder(e, contentService, "News", "NewsList");
        }
    }
}