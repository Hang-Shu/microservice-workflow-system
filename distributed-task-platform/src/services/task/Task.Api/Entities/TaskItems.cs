using System.ComponentModel.DataAnnotations;
using Task.Api.Enum;

namespace Task.Api.Entities
{
    public class TaskItems
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// User could use it easily
        /// </summary>
        public int TaskNumber { get; private set; }


        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public Guid? ProjectId { get; set; }

        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Created;

        public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Middle;

        public Guid? AssignedUserId { get; set; }

        public string? Remark { get; set; }

        public Guid CreatedUserId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
