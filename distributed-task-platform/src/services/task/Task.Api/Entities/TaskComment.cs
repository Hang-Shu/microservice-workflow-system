using System.ComponentModel.DataAnnotations;

namespace Task.Api.Entities
{
    public class TaskComment
    {
        [Key]
        public Guid Id {  get; set; }

        [Required]
        public Guid TaskId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string CommentText { get; set; }

        [Required]
        public Guid UderId { get; set; }

        public DateTime CommentTime { get; set; }
    }
}
