using Query.Api.Data;
using Query.Api.Entities;
using Query.Api.Infrastructure;
using Shared.Common;
using Shared.Contracts.Events;

namespace Query.Api.Services
{
    public class TaskModifyLineService:ITaskModifyLineService
    {
        private readonly QueryDbContext _dbContext;

        public TaskModifyLineService(QueryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> HandleOperateTaskUpdateAsync(TaskUpdatedEvent dto,string strRoutringKey)
        {
            //Look for if update group exist 
            TaskModifyLineRead taskModify =
                _dbContext.TaskModifyLineRead.Where(x => x.TaskNumber == dto.TaskNumber && x.ModifyUserNumber == dto.UpdateUserNumber
                                                    && x.GroupStartTime <= dto.UpdateTime && x.GroupEndTime >= dto.UpdateTime).FirstOrDefault();
            string strField = TaskRoutingKeyMapper.ToChangeField(strRoutringKey).ToString();
            if (taskModify != null)
            {//Exist, update info
                if(!taskModify.ListUpdateItems.Contains(strField))
                {
                    taskModify.ListUpdateItems.Add(strField);
                    if (await _dbContext.SaveChangesAsync() <= 0)
                        return Result.Failure("Update database filed");
                }
                return Result.Success();
            }
            //Not exist, insert new info
            taskModify = new()
            {
                Id = Guid.NewGuid(),
                TaskNumber = dto.TaskNumber,
                ModifyUserNumber = dto.UpdateUserNumber,
                DisplayNameSnapshot = dto.UpdateUserDisplayName,
                ListUpdateItems = new List<string> { strField },
                GroupStartTime = dto.UpdateTime,
                GroupEndTime = dto.UpdateTime.AddMinutes(2)
            };
            _dbContext.TaskModifyLineRead.Add(taskModify);
            if (await _dbContext.SaveChangesAsync() > 0)
                return Result.Success();
            return Result.Failure("Update database filed");

        }
    }
}
