using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Email.Api.Services.EmailSender
{
    public class EmailSenderService: IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Task System", _configuration["EmailSettings:FromEmail"]));

            message.To.Add(new MailboxAddress("", toEmail));

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(_configuration["EmailSettings:SmtpHost"], Convert.ToInt16(_configuration["EmailSettings:Port"]), true);

            await client.AuthenticateAsync(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:Password"]);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
