using Notification.Api.Enum;
using System.ComponentModel.DataAnnotations;

namespace Notification.Api.Entities
{
    public class Notifications
    {
        [Key]
        public Guid Id { get; set; }

        public int? FromUserNumber { get; set; }

        public string MsgTitle { get; set; }

        public string MsgText { get; set; }

        public bool IsSystemMsg { get; set; } = false;

        public bool IsValid { get; set; } = true;

        public NotificationCategoryEnum NotificationCategory { get; set; }

        public DateTime CreateTime { get; set; }

        
    }
}
