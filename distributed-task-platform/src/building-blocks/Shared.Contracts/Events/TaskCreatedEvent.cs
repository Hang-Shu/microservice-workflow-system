using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public class TaskCreatedEvent
    {
        public Guid TaskId { get; set; }

        public string Title { get; set; }

        public Guid? AssignedUserId { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
