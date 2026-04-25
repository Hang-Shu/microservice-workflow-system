using Email.Api.Entities;
using Shared.Common;
using Shared.Contracts;
using Shared.Contracts.Events;

namespace Email.Api.Infrastructure
{
    public class EmailEventPublisher: IEmailEventPublisher
    {
        private readonly IEventPublisher _eventPublisher;
        public EmailEventPublisher(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        public async Task PublishEmailSendAsync(Emails emails,TasksEmailsPending pending)
        {
            var evt = new EmailSendEvent
            {
                EmailId = emails.Id,
                TaskNumbers = pending.PendingTaskNumbers,
                ReciveUserNumber = pending.ReciveUserNumber,
                DisplayNameSnapshot = emails.EmailTitle,
                StatusName = emails.EmailStatus.ToString(),
                Reason = "New high priority task",
                CreateTime = emails.CreateTime
            };
            
            await _eventPublisher.PublishAsync(evt,EmailMqConstants.Exchange,EmailMqConstants.RoutingKeys.EmailSend);
        }
    }
}
