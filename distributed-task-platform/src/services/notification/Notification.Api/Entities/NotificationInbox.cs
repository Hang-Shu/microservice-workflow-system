using System.ComponentModel.DataAnnotations;

namespace Notification.Api.Entities
{
    public class NotificationInbox
    {
        [Key]
        [MaxLength(300)]
        public string EventKey { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
