using Query.Api.Dtos;
using Shared.Common;

namespace Query.Api.Services
{
    public interface IQueryTaskDatasService
    {
        public Task<Result<TaskDetailQueryOutTimeLineDto>> QueryTaskInfoByTimeLine(int tasknum);
    }
}
