namespace Email.Api.Services.EmailSender
{
    public interface IEmailSenderService
    {
        public Task SendAsync(string toEmail, string subject, string body);
    }
}
