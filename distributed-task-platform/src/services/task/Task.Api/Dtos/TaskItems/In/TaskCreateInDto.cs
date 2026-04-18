using Task.Api.Enum;

namespace Task.Api.Dtos
{
    public class TaskCreateInDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid? ProjectId { get; set; }

        public TaskStatusEnum Status { get; set; }

        public TaskPriorityEnum Priority { get; set; }

        public int? AssignedUserNumber { get; set; }

        public string? Remark { get; set; }

        public int CreatedUserNumber { get; set; }

        public DateTime DueDate { get; set; }
    }
}
