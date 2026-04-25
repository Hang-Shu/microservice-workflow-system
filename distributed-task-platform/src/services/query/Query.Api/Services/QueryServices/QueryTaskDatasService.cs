using Query.Api.Data;
using Query.Api.Dtos;
using Query.Api.Entities;
using Shared.Common;
using Shared.Contracts.Events;

namespace Query.Api.Services.QueryServices
{
    public class QueryTaskDatasService: IQueryTaskDatasService
    {
        private readonly QueryDbContext _dbContext;

        public QueryTaskDatasService(QueryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<TaskDetailQueryOutTimeLineDto>> QueryTaskInfoByTimeLine(int tasknum)
        {
            try
            {
                TaskDetailQueryOutTimeLineDto dto = new();
                List<TaskEmailSummaryRead> lstEmail = _dbContext.TaskEmailSummaryRead.Where(x => x.TaskNumber == tasknum).ToList();
                List<TaskCommentRead> lstComment = _dbContext.TaskCommentRead.Where(x => x.TaskNumber == tasknum).ToList();
                List<TaskModifyLineRead> lstModify = _dbContext.TaskModifyLineRead.Where(x => x.TaskNumber == tasknum).ToList();
                if (lstEmail.Count > 0)
                {
                    dto.TotalEmails = lstEmail.Count;
                    dto.lstActives.AddRange(lstEmail.Select(x => new ActiveDto
                    {
                        Type = "Email",
                        Time = x.CreateTime,
                        Data = x
                    }));
                }
                else
                    dto.TotalEmails = 0;

                if(lstComment.Count>0)
                {
                    dto.TotalComments = lstComment.Count;
                    dto.lstActives.AddRange(lstComment.Select(x => new ActiveDto
                    {
                        Type = "Comment",
                        Time = x.CreateTime,
                        Data = x
                    }));
                }
                else
                    dto.TotalComments = 0;

                if(lstModify.Count>0)
                {
                    dto.lstActives.AddRange(lstModify.Select(x => new ActiveDto
                    {
                        Type = "Modify",
                        Time = x.GroupStartTime,
                        Data = x
                    }));
                }
                dto.lstActives = dto.lstActives.OrderByDescending(x => x.Time).ToList();

                return Result<TaskDetailQueryOutTimeLineDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<TaskDetailQueryOutTimeLineDto>.Failure(ex.Message);
            }
        }
    }
}
