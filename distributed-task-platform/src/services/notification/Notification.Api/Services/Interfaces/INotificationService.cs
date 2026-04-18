using Notification.Api.Dtos;
using Shared.Common;
using Shared.Contracts.Events;

namespace Notification.Api.Services
{
    public interface INotificationService
    {
        public Task<Result> CreateNewMessage(NotificationCreateInDto dto);

        public Task<Result> HandleTaskCreatedAsync(TaskCreatedEvent taskEvent);
    }
}
