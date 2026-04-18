using System.ComponentModel.DataAnnotations;

namespace Task.Api.Entities
{
    public class TaskOperates
    {
        [Key]
        public Guid Id { get; set; }

        public int TaskNumber { get; set; }

        public int UserNumber { get; set; }

        public string? Remark { get; set; }

        public DateTime OperateTime { get; set; }



    }
}
