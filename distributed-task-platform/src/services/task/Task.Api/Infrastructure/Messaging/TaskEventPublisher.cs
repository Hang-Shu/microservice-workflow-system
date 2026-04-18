using Shared.Common;
using Shared.Contracts.Events;
using Task.Api.Entities;

namespace Task.Api.Infrastructure
{
    public class TaskEventPublisher:ITaskEventPublisher
    {
        private readonly IEventPublisher _eventPublisher;
        public TaskEventPublisher(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async System.Threading.Tasks.Task PublishTaskItemCreatedAsync(TaskItems taskItem,string projName,Users userAssigned)
        {
            var evt = new TaskCreatedEvent
            {
                TaskNumber = taskItem.TaskNumber,
                Title = taskItem.Title,
                Description = taskItem.Description,
                ProjectName = projName,
                CreatedTime = taskItem.CreatedTime,
                Priority = (int)taskItem.Priority
            };
            if(userAssigned!=null)
            {
                evt.AssignedUserNumber = userAssigned.UserNumber;
                evt.AssignedUserDisplayName = userAssigned.DisplayName;
                evt.AssignedUserEmailAddress = userAssigned.UserEmail;
            }
            await _eventPublisher.PublishAsync(evt);
        }
    }
}
