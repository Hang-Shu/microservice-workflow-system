namespace Email.Api.Services.EmailSender
{
    public interface IEmailsPendingService
    {
        Task ProcessPendingEmailsAsync(CancellationToken cancellationToken);
    }
}
