using Email.Api.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Email.Api.Entities
{
    public class TasksEmailsPending
    {
        [Key]
        public Guid Id { get; set; }
        public int ReciveUserNumber { get; set; }
        public string EmailAccount { get; set; }
        public List<int> PendingTaskNumbers { get; set; } = new();
        public string TaskTitles { get; set; }
        public DateTime FirstEventTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public DateTime PlanSentTime { get; set; }
    }
}
