using Email.Api.Data;
using Email.Api.Dtos;
using Email.Api.Entities;
using Email.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Email.Api.Services.EmailSender
{
    public class EmailsPendingService:IEmailsPendingService
    {
        private readonly EmailDbContext _dbContext;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailEventPublisher _emailEventPublisher;
        public EmailsPendingService(EmailDbContext dbContext,IEmailSenderService emailSenderService, IEmailEventPublisher emailEventPublisher)
        {
            _dbContext = dbContext;
            _emailSenderService = emailSenderService;
            _emailEventPublisher = emailEventPublisher;
        }
        public async Task ProcessPendingEmailsAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var pendingList = await _dbContext.TasksEmailsPending
                .Where(x => x.PlanSentTime <= now)
                .ToListAsync(cancellationToken);

            foreach (var pending in pendingList)
            {
                //Check if have't pending task
                if (pending.PendingTaskNumbers.Count <= 0)
                {
                    _dbContext.TasksEmailsPending.Remove(pending);
                    continue;
                }
                    

                List<TaskTitleInfoDto> taskTitles = JsonSerializer.Deserialize<List<TaskTitleInfoDto>>(pending.TaskTitles);
                string subject = string.Format("You received {0} high priority tasks", pending.PendingTaskNumbers.Count);
                string body = BuildBody(pending, taskTitles);
                try
                {
                    await _emailSenderService.SendAsync(pending.EmailAccount, subject, body);

                    // Write tables Emails
                    Emails emails = new()
                    {
                        Id = Guid.NewGuid(),
                        ReciveUserNumber=pending.ReciveUserNumber,
                        EmailAccount=pending.EmailAccount,
                        EmailTitle=subject,
                        EmailText=body,
                        EmailStatus=Enum.EmailStatusEnum.Submitted,
                        CreateTime=now
                    };
                    _dbContext.Emails.Add(emails);

                    await _emailEventPublisher.PublishEmailSendAsync(emails, pending);
                    _dbContext.TasksEmailsPending.Remove(pending);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private string BuildBody(TasksEmailsPending pending, List<TaskTitleInfoDto> taskTitles)
        {
            string strBody = "Hi! You received high priority tasks.\n\n";
            foreach(int iTaskNumber in pending.PendingTaskNumbers)
            {
                strBody += "#" + iTaskNumber.ToString("D4") + "  " + taskTitles.Where(x => x.TaskNumber == iTaskNumber).First().TaskTitle + "\n";
            }
            return strBody;
        }
    }
}
