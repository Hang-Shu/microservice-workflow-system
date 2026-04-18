using System.ComponentModel.DataAnnotations;

namespace Task.Api.Entities
{
    public class TaskComment
    {
        [Key]
        public Guid Id {  get; set; }

        [Required]
        public int TaskNumber { get; set; }

        [Required]
        [MaxLength(2000)]
        public string CommentText { get; set; }

        [Required]
        public int UserNumber { get; set; }

        public DateTime CommentTime { get; set; }
    }
}
