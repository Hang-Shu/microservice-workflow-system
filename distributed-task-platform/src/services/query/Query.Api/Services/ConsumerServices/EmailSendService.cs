using Query.Api.Data;
using Query.Api.Entities;
using Query.Api.Infrastructure;
using Shared.Common;
using Shared.Contracts.Events;

namespace Query.Api.Services
{
    public class EmailSendService : IEmailSendService
    {
        private readonly QueryDbContext _dbContext;

        public EmailSendService(QueryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result> HandleEmailSendAsync(EmailSendEvent dto)
        {
            try
            {
                foreach(int taskNum in dto.TaskNumbers)
                {
                    //Make idempotent key and check it in table
                    string strIdempotent = "Email:" + dto.ReciveUserNumber + ":" + taskNum + ":" + dto.CreateTime.ToString("O");
                    if (_dbContext.TaskEmailSummaryRead.Where(x => x.IdempotentId == strIdempotent).FirstOrDefault() != default)
                        continue;
                    TaskEmailSummaryRead summaryReadItem = new()
                    {
                        Id = Guid.NewGuid(),
                        TaskNumber = taskNum,
                        EmailId = dto.EmailId,
                        IdempotentId = strIdempotent,
                        ReciveUserNumber = dto.ReciveUserNumber,
                        DisplayNameSnapshot = dto.DisplayNameSnapshot,
                        StatusName = dto.StatusName,
                        Reason = dto.Reason,
                        CreateTime = dto.CreateTime
                    };
                    _dbContext.TaskEmailSummaryRead.Add(summaryReadItem);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                return Result.Failure("Update database filed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
