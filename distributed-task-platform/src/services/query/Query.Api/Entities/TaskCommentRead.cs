using System.ComponentModel.DataAnnotations;

namespace Query.Api.Entities
{
    public class TaskCommentRead
    {
        [Key]
        public Guid Id { get; set; }

        public int TaskNumber { get; set; }

        //Connect "TaskComment"-"Id"
        public Guid TaskCommentId { get; set; }

        [Required]
        public int UserNumber { get; set; }

        [Required]
        public string DisplayNameSnapshot { get; set; }

        [Required]
        public string PreviewText { get; set; }

        public bool IsPreview { get; set; } = false;

        public DateTime CreateTime  { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
