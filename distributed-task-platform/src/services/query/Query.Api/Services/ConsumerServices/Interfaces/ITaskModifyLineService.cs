using Shared.Common;
using Shared.Contracts.Events;

namespace Query.Api.Services
{
    public interface ITaskModifyLineService
    {
        public Task<Result> HandleOperateTaskUpdateAsync(TaskUpdatedEvent dto, string strRoutringKey);
    }
}
