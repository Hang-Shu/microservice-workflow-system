using Email.Api.Entities;

namespace Email.Api.Infrastructure
{
    public interface IEmailEventPublisher
    {
        public Task PublishEmailSendAsync(Emails emails, TasksEmailsPending pending);
    }
}
