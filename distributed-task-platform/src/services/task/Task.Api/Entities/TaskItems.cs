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
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public Guid? ProjectId { get; set; }

        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Created;

        public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Middle;

        public int? AssignedUserNumber { get; set; }

        public string? Remark { get; set; }

        public int CreatedUserNumber { get; set; }

        [Required]
        public DateOnly DueDate { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }
    }
}
