using System;

namespace Boilerplate.Core.Models.ActivityLog
{
    public class ActivityViewModel
    {  
        public string UserAvatarUrl { get; set; }
        public string UserDisplayName { get; set; }
        public string Message { get; set; }
        public string NodeName { get; set; }
        public string ContentTypeIcon { get; set; }
        public int NodeId { get; set; }
        public string LogItemType { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? ScheduledPublishDate { get; set; }
        public string SectionHeader { get; set; }
        public string CustomAction { get; set; }
        public string DisplayName { get; set; }
    }
}