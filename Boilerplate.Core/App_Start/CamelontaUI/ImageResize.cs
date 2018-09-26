using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageProcessor;
using ImageProcessor.Imaging;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Events;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Boilerplate.Core.CamelontaUI
{
    public class ImageResize : Umbraco.Core.ApplicationEventHandler
    {
        int DefaultMaxWidth = 2700; // TODO: Set this from appSettings

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //MediaService.Saving += MediaService_Saving;
        }

        void MediaService_Saving(IMediaService mediaService, SaveEventArgs<IMedia> args)
        {
            // Don't allow upload in root
            if (args.SavedEntities.Any() && args.SavedEntities.First().ParentId == -1 && args.SavedEntities.First().HasProperty("umbracoFile"))
            {
                LogHelper.Warn(this.GetType(), "Files are not allowed to be uploaded in the root folder");
                args.Cancel = true;
                return;
            }


            MediaFileSystem mediaFileSystem = FileSystemProviderManager.Current.GetFileSystemProvider<MediaFileSystem>();
            IContentSection contentSection = UmbracoConfig.For.UmbracoSettings().Content;
            IEnumerable<string> supportedTypes = contentSection.ImageFileTypes.ToList();

            foreach (IMedia media in args.SavedEntities)
            {
                if (media.HasProperty("umbracoFile"))
                {
                    // Make sure it's an image.
                    string path = media.GetValue<string>("umbracoFile");
                    var fullExtension = Path.GetExtension(path);
                    if (fullExtension != null)
                    {
                        string extension = fullExtension.Substring(1);
                        if (supportedTypes.InvariantContains(extension))
                        {
                            // Get maxwidth from parent folder
                            var maxWidth = GetMaxWidthFromParent(DefaultMaxWidth, media);

                            if (maxWidth < media.GetValue<int>("umbracoWidth"))
                            {
                                // Resize the image to maxWidth:px wide, height is driven by the aspect ratio of the image.
                                string fullPath = mediaFileSystem.GetFullPath(path);
                                using (ImageFactory imageFactory = new ImageFactory(true))
                                {
                                    ResizeLayer layer = new ResizeLayer(new Size(maxWidth, 0), resizeMode: ResizeMode.Max, anchorPosition: AnchorPosition.Center, upscale: false);
                                    imageFactory.Load(fullPath).Resize(layer).Save(fullPath);
                                }
                            }
                        }
                    }
                }
            }
        }

        int GetMaxWidthFromParent(int maxWidth, IMedia media)
        {
            if (media.ParentId > 0)
            {
                var parent = media.Parent();
                if (parent.HasProperty("maxwidth") && parent.GetValue<int>("maxwidth") > 0)
                    maxWidth = parent.GetValue<int>("maxwidth");
                else
                    maxWidth = GetMaxWidthFromParent(maxWidth, parent);

            }
            return maxWidth;
        }
    }
}