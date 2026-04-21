using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public class TaskUpdatedEvent
    {
        public int TaskNumber { get; set; }
        public string Title { get; set; }
        public string ProjectName { get; set; }
        public bool IsImportant { get; set; } = false;
        public bool IsClosed { get; set; } = false;
        public int AssignedUserNumber { get; set; } = -1;
        public string AssignedUserEmail { get; set; }
        public string AssignedUserDisplayName { get; set; }
        public int UpdateUserNumber { get; set; }
        public string UpdateUserDisplayName { get; set; }
        public DateTime UpdateTime { get; set; }
        //Just for cancle Email
        public int OldAssignedUserNumber { get; set; }
    }
}
