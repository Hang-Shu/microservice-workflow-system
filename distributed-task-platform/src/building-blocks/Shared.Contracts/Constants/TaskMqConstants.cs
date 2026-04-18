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
            public const string TaskUpdated = "task.updated";
        }
    }
}
