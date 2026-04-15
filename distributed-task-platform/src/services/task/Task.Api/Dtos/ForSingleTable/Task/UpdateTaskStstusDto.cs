using Task.Api.Enum;

namespace Task.Api.Dtos
{
    public class UpdateTaskStstusDto
    {
        public Guid Id { get; set; }

        public TaskStatusEnum Status { get; set; }

    }
}
