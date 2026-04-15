using System.ComponentModel.DataAnnotations;

namespace Task.Api.Entities
{
    public class TaskOperates_Dtl
    {
        [Key]
        public Guid Id { get; set; }

        //Connect "TaskOperates"-"Id"
        public Guid MainId { get; set; }

        public string OpreateType { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

    }
}
