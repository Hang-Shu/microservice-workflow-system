using Query.Api.Enum;
using Shared.Contracts;

namespace Query.Api.Infrastructure
{
    public static class TaskRoutingKeyMapper
    {
        public static TaskChangeFieldEnum ToChangeField(string routingKey)
        {
            return routingKey switch
            {
                TaskMqConstants.RoutingKeys.TaskUpdated_Title => TaskChangeFieldEnum.Title,
                TaskMqConstants.RoutingKeys.TaskUpdated_Description => TaskChangeFieldEnum.Description,
                TaskMqConstants.RoutingKeys.TaskUpdated_Project => TaskChangeFieldEnum.Project,
                TaskMqConstants.RoutingKeys.TaskUpdated_Status => TaskChangeFieldEnum.Status,
                TaskMqConstants.RoutingKeys.TaskUpdated_Priority => TaskChangeFieldEnum.Priority,
                TaskMqConstants.RoutingKeys.TaskUpdated_Assigned => TaskChangeFieldEnum.Assigned,
                TaskMqConstants.RoutingKeys.TaskUpdated_Remark => TaskChangeFieldEnum.Remark,
                TaskMqConstants.RoutingKeys.TaskUpdated_DueDate => TaskChangeFieldEnum.DueDate
            };
        }
    }
}
