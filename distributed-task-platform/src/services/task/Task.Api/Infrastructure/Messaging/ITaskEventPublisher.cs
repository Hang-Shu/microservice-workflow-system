using Task.Api.Entities;

namespace Task.Api.Infrastructure
{
    public interface ITaskEventPublisher
    {
        public System.Threading.Tasks.Task PublishTaskItemCreatedAsync(TaskItems taskItem, string projName, Users userAssigned);
    }
}
