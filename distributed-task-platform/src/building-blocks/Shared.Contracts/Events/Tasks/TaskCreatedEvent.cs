using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public class TaskCreatedEvent:IExchangeInfo
    {
        public string ExchangeName => TaskMqConstants.Exchange;
        public string RoutingKey => TaskMqConstants.RoutingKeys.TaskCreated;
        public int TaskNumber { get; set; }
        //Task title
        public string Title { get; set; }
        //Task description
        public string Description { get; set; }
        public string? ProjectName { get; set; }

        public int? AssignedUserNumber { get; set; }

        public string? AssignedUserDisplayName { get; set; }

        public string? AssignedUserEmailAddress { get; set; }

        public int Priority { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
