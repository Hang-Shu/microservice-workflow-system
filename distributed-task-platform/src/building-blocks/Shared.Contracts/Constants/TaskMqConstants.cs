using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public static class TaskMqConstants
    {
        public const string Exchange = "task-events";
        public static class RoutingKeys
        {
            public const string TaskCreated = "task.created";
            public const string TaskUpdated_Title = "task.updated.title";
            public const string TaskUpdated_Description = "task.updated.description";
            public const string TaskUpdated_Project = "task.updated.project";
            public const string TaskUpdated_Status = "task.updated.status";
            public const string TaskUpdated_Priority = "task.updated.priority";
            public const string TaskUpdated_Assigned = "task.updated.assigned";
            public const string TaskUpdated_Remark = "task.updated.remark";
            public const string TaskUpdated_DueDate = "task.updated.dueDate";
        }
    }
}
