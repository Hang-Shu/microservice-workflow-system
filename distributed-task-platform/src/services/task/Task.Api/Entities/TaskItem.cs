using System.ComponentModel.DataAnnotations;

namespace Task.Api.Entities
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Created";

        public string Priority { get; set; } = "Medium";

        public Guid? AssignedUserId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
