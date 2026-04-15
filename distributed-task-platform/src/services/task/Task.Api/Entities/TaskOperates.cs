using System.ComponentModel.DataAnnotations;

namespace Task.Api.Entities
{
    public class TaskOperates
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        public Guid UserID { get; set; }

        public string? Remark { get; set; }

        public DateTime OperateTime { get; set; }



    }
}
