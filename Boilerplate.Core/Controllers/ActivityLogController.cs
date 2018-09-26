using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Boilerplate.Core.Classes.ActivityLog;
using Boilerplate.Core.Models.ActivityLog;
using Boilerplate.Core.Resources;
using umbraco.BusinessLogic;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using File = System.IO.File;

namespace Boilerplate.Core.Controllers
{
    [PluginController("ActivityLog")]
    public class ActivityLogController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<ActivityViewModel> GetLog(int take = 10, int skip = 0)
        {
            string currentUserLanguage = string.Empty;
            var currentUser = Services.UserService.GetByUsername(User.Identity.Name);
            if (currentUser == null)
            {
                LogHelper.Warn(GetType(), string.Format("No user found by User.Identity.Name '{0}'", User.Identity.Name));
            }
            else
            {
                currentUserLanguage = currentUser.Language;
            }

            var repo = new UmbracoRepository();
            var logItems = repo.GetLatestLogItems(take, skip);
            var nodesInRecyleBin = repo.GetRecycleBinNodes().Select(x => x.Id).ToArray();
            
            var activities = new List<ActivityViewModel>();
            foreach (var logItem in logItems)
            {
                var user = GetUser(logItem.UserId);
                var contentNode = GetContent(logItem.NodeId);

                var vm = new ActivityViewModel
                {
                    UserDisplayName = user.Name,
                    UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user),
                    NodeId = logItem.NodeId,
                    Message = logItem.Comment,
                    LogItemType = logItem.LogType.ToString(),
                    Timestamp = logItem.Timestamp,
                    SectionHeader = GetHeader(logItem.Timestamp, UserHelper.GetCultureInfo(currentUserLanguage))// This is the date-header ("today", "2015-06-16" etc.)
                };

                if (contentNode != null)
                {
                    vm.DisplayName = contentNode.Name;
                    vm.ContentTypeIcon = contentNode.ContentType.Icon;
                }

                var customAction = GetCustomAction(logItem);

                if (customAction == CustomActions.RecycleBinEmptied)
                    vm.LogItemType = "RecycleBinEmptied";


                if (customAction == CustomActions.SaveMedia || customAction == CustomActions.MoveMediaToRecycleBin)
                {
                    var media = Umbraco.TypedMedia(logItem.NodeId);
                    if (media != null)
                    {
                        if (media.DocumentTypeAlias != "Folder" && !String.IsNullOrEmpty(media.Url))
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath(media.Url)))
                                vm.ContentTypeIcon = media.Url;
                        }
                        vm.LogItemType = media.DocumentTypeAlias;
                        vm.DisplayName = media.Name;
                    }
                    else
                    {
                        // If the item is deleted from recycle bin, fallback to show just a file-icon
                        vm.LogItemType = "File";
                    }

                    vm.CustomAction = customAction.ToString();
                }

                if (logItem.Comment.StartsWith("Move Content to Recycle") && nodesInRecyleBin.Contains(logItem.NodeId))
                {
                    vm.LogItemType = "MovePageToRecycleBin";
                }

                // Set the final name for display (node id) if its still empty
                if (string.IsNullOrEmpty(vm.DisplayName))
                    vm.DisplayName = string.Format("[{0}]", logItem.NodeId);

                activities.Add(vm);
            }

            return activities;
        }

        /// <summary>
        /// Get resources and misc info. 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetTotalActivitiesAndResources()
        {
            var userService = ApplicationContext.Services.UserService;
            var currentUser = userService.GetByUsername(Security.CurrentUser.Username);
            var cultureInfo = UserHelper.GetCultureInfo(currentUser.Language);
            var resources = new Dictionary<string, string>
            {
                { "ActivityLog", Resource.ResourceManager.GetString("ActivityLog", cultureInfo) },
                { "Saved", Resource.ResourceManager.GetString("Saved", cultureInfo) },
                { "Moved", Resource.ResourceManager.GetString("Moved", cultureInfo) },
                { "Deleted", Resource.ResourceManager.GetString("Deleted", cultureInfo) },
                { "Published", Resource.ResourceManager.GetString("Published", cultureInfo) },
                { "And", Resource.ResourceManager.GetString("And", cultureInfo) },
                { "RecycleBin", Resource.ResourceManager.GetString("RecycleBin", cultureInfo) },
                { "EmptiedRecycleBin", Resource.ResourceManager.GetString("EmptiedRecycleBin", cultureInfo) },
                { "Unpublished", Resource.ResourceManager.GetString("Unpublished", cultureInfo) },
                { "To", Resource.ResourceManager.GetString("To", cultureInfo) },
                { "Created", Resource.ResourceManager.GetString("Created", cultureInfo) },
                { "Uploaded", Resource.ResourceManager.GetString("Uploaded", cultureInfo) },
                { "Copied", Resource.ResourceManager.GetString("Copied", cultureInfo) },
                { "RolledBack", Resource.ResourceManager.GetString("RolledBack", cultureInfo) }
            };

            var repo = new UmbracoRepository();
            var response = new Dictionary<string, object>();
            response["NumberOfActivities"] = repo.CountLogItems();
            response["Resources"] = resources;
            return response;
        }

        #region Private
        private IUser GetUser(int id)
        {
            return Services.UserService.GetByProviderKey(id);
        }

        private IContent GetContent(int id)
        {
            var doc = Services.ContentService.GetById(id);
            return doc;
        }

        private static CustomActions GetCustomAction(LogItem logItem)
        {
            if (logItem.Comment.StartsWith("Empty Content Recycle Bin") || logItem.Comment.StartsWith("Empty Media Recycle Bin"))
                return CustomActions.RecycleBinEmptied;
            if (logItem.Comment.StartsWith("Save Media"))
                return CustomActions.SaveMedia;
            if (logItem.Comment.StartsWith("Move Media to Recycle Bin"))
                return CustomActions.MoveMediaToRecycleBin;
            return CustomActions.None;
        }

        DateTime _lastDate = DateTime.MinValue;
        private string GetHeader(DateTime dateTime, CultureInfo cultureInfo)
        {
            string ret = null;
            if (dateTime.Day != _lastDate.Day)
                ret = dateTime.ToString("yyyy-MM-dd");

            if (ret == DateTime.Today.ToString("yyyy-MM-dd"))
                ret = Resource.ResourceManager.GetString("Today", cultureInfo);

            if (ret == DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"))
                ret = Resource.ResourceManager.GetString("Yesterday", cultureInfo);

            _lastDate = dateTime;
            return ret;
        }
        #endregion
    }
}