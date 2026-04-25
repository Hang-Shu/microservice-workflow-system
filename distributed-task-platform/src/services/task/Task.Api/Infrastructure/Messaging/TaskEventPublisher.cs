using Shared.Common;
using Shared.Contracts;
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
            if(taskItem.Priority==Enum.TaskPriorityEnum.High)
                evt.IsImportant = true;
            if (userAssigned!=null)
            {
                evt.AssignedUserNumber = userAssigned.UserNumber;
                evt.AssignedUserDisplayName = userAssigned.DisplayName;
                evt.AssignedUserEmailAddress = userAssigned.UserEmail;
            }
            await _eventPublisher.PublishAsync(evt);
        }

        public async System.Threading.Tasks.Task PublishTaskItemUpdatedAsync(DateTime dtOperateTime,TaskItems taskItem, string projName, Users userAssigned, Users userUpdated,
            string strRoutingKey, int oldAssignedUserNumber = -1)
        {
            var evt = new TaskUpdatedEvent
            {
                TaskNumber = taskItem.TaskNumber,
                Title = taskItem.Title,
                ProjectName = projName,
                UpdateTime = dtOperateTime,
                DueDate = taskItem.DueDate
            };
            if (taskItem.Priority == Enum.TaskPriorityEnum.High)
                evt.IsImportant = true;
            if(taskItem.Status==Enum.TaskStatusEnum.Completed)
                evt.IsClosed = true;
            evt.OldAssignedUserNumber = oldAssignedUserNumber;
            if (userAssigned != null)
            {
                evt.AssignedUserNumber = userAssigned.UserNumber;
                evt.AssignedUserDisplayName = userAssigned.DisplayName;
                evt.AssignedUserEmail = userAssigned.UserEmail;
            }

            if(userUpdated!=null)
            {
                evt.UpdateUserNumber = userUpdated.UserNumber;
                evt.UpdateUserDisplayName = userUpdated.DisplayName;
            }

            await _eventPublisher.PublishAsync(evt, TaskMqConstants.Exchange, strRoutingKey);
        }

        public async System.Threading.Tasks.Task PublishTaskCommentCreatedAsync(int taskNumber, TaskComment taskComment,string strUserName)
        {
            CommentEvent commentEvent = new()
            {
                CommentId = taskComment.Id,
                TaskNumber = taskComment.TaskNumber,
                UserNumber = taskComment.UserNumber,
                DisplayNameSnapshot = strUserName,
                CommentText = taskComment.CommentText,
                CreateTime = taskComment.CommentTime
            };
            await _eventPublisher.PublishAsync(commentEvent, TaskMqConstants.Exchange, TaskMqConstants.RoutingKeys.TaskCommentCreated);
        }
        public async System.Threading.Tasks.Task PublishTaskCommentUpdatedAsync(TaskComment taskComment)
        {
            CommentEvent commentEvent = new()
            {
                CommentId = taskComment.Id,
                CommentText = taskComment.CommentText,
                CreateTime = DateTime.UtcNow
            };
            await _eventPublisher.PublishAsync(commentEvent, TaskMqConstants.Exchange, TaskMqConstants.RoutingKeys.TaskCommentUpdated);
        }

        public async System.Threading.Tasks.Task PublishTaskCommentDeletedAsync(TaskComment taskComment)
        {
            CommentEvent commentEvent = new()
            {
                CommentId = taskComment.Id
            };
            await _eventPublisher.PublishAsync(commentEvent, TaskMqConstants.Exchange, TaskMqConstants.RoutingKeys.TaskCommentDeleted);
        }
    }
}
