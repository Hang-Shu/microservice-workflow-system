using System.ComponentModel.DataAnnotations;

namespace Notification.Api.Entities
{
    public class Notifications_Dtl
    {
        [Key]
        public Guid Id { get; set; }

        //Connect "Notifications"-"Id"
        [Required]
        public Guid MainId { get; set; }

        [Required]
        public Guid ReciveUserId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime readTime { get; set; }
    }
}
