using Task.Api.Entities;

namespace Task.Api.Infrastructure
{
    public interface ITaskEventPublisher
    {
        public System.Threading.Tasks.Task PublishTaskItemCreatedAsync(TaskItems taskItem, string projName, Users userAssigned);

        public System.Threading.Tasks.Task PublishTaskItemUpdatedAsync(DateTime dtOperateTime,TaskItems taskItem, string projName, Users userAssigned, Users userUpdated,
            string strRoutingKey, int oldAssignedUserNumber = -1);
    }
}
