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
        public int ReciveUserNumber { get; set; }

        public bool IsImportant { get; set; } = false;

        public bool IsRead { get; set; } = false;

        public DateTime? ReadTime { get; set; }

        public DateTime SendTime { get; set; }
    }
}
