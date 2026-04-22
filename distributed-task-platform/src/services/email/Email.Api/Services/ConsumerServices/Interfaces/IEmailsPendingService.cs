using Shared.Common;
using Shared.Contracts.Events;

namespace Email.Api.Services
{
    public interface IEmailsPendingService
    {
        public Task<Result> HandleTaskUpdateAssignedAsync(TaskUpdatedEvent dto);
        public Task<Result> HandleTaskUpdatePriorityAsync(TaskUpdatedEvent dto);
        public Task<Result> HandleTaskCreatedAsync(TaskCreatedEvent dto);
    }
}
