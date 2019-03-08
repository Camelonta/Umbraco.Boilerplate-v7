using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Boilerplate.Core.Classes.CamelontaUI
{
    /// <summary>
    /// Organizes pages
    /// </summary>
    public class PageOrganizer
    {
        public void MoveToDatefolder(SaveEventArgs<IContent> e, IContentService contentService,
            string contentTypeToMove = "News", string contentTypeOfContainer = "NewsList", bool moveToMonth = false, bool englishMonthNames = false)
        {

            MoveToDatefolder(e, contentService, new List<string> { contentTypeToMove }, contentTypeOfContainer, moveToMonth, englishMonthNames);
        }

        /// <summary>
        /// Moves pages into year/month folders
        /// </summary>
        public void MoveToDatefolder(SaveEventArgs<IContent> e, IContentService contentService, List<string> contentTypeToMove, string contentTypeOfContainer = "NewsList", bool moveToMonth = false, bool englishMonthNames = false)
        {
            foreach (var page in e.SavedEntities)
            {
                // Not interested in anything but "create" events.
                if (!page.IsNewEntity()) return;

                // Not interested if the item being added is not a news-page.
                if (!contentTypeToMove.Contains(page.ContentType.Alias)) return;

                var now = page.ReleaseDate.HasValue ? page.ReleaseDate.Value : DateTime.Now;
                var year = now.ToString("yyyy");
                var month = now.ToString("MM").GetMonthFromNumber(englishMonthNames);

                IContent yearDocument = null;

                // Get year-document by container (if it is a 4 digit number)
                int n;
                if (int.TryParse(page.Parent().Name, out n))
                {
                    if (n.ToString().Length == 4)
                        yearDocument = page.Parent();
                }
                // Get year-document by parent-siblings
                if (yearDocument == null)
                {
                    foreach (var child in page.Parent().Children())
                    {
                        if (child.Name == year)
                        {
                            yearDocument = child;
                            break;
                        }
                    }
                }

                // If the year folder doesn't exist, create it.
                if (yearDocument == null)
                {
                    yearDocument = contentService.CreateContentWithIdentity(year, page.ParentId, contentTypeOfContainer);
                    contentService.Publish(yearDocument);
                }

                if (moveToMonth)
                {
                    var monthDocument = yearDocument.Children().FirstOrDefault(x => x.Name == month);

                    // If the month folder doesn't exist, create it.
                    if (monthDocument == null)
                    {
                        monthDocument = contentService.CreateContentWithIdentity(month, yearDocument.Id, contentTypeOfContainer);
                        contentService.Publish(monthDocument);
                    }

                    // Move the document into the month folder
                    contentService.Move(page, monthDocument.Id);
                }
                else
                {
                    // Move the document into the year folder
                    contentService.Move(page, yearDocument.Id);
                }

                #region Sort year pages
                try
                {
                    var mainNewsPage = yearDocument.Parent();

                    // SORT year-folders by year (newest first)
                    var sortedYearPages = mainNewsPage.Children().OrderByDescending(p => p.Name).ToArray();

                    for (var i = 0; i < sortedYearPages.Count(); i++)
                    {
                        sortedYearPages[i].SortOrder = i;
                        contentService.SaveAndPublishWithStatus(sortedYearPages[i]);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(typeof(PageOrganizer), "Could not sort year-documents for News", ex);
                }
                #endregion
            }
        }
    }
}