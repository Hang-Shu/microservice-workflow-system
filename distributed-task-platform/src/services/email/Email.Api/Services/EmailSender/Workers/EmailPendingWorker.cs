using Email.Api.Services.EmailSender;

namespace Email.Api.Workers.EmailSender
{
    public class EmailPendingWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailPendingWorker> _logger;

        public EmailPendingWorker(IServiceScopeFactory scopeFactory,ILogger<EmailPendingWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailPendingWorker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var pendingService = scope.ServiceProvider.GetRequiredService<IEmailsPendingService>();

                    await pendingService.ProcessPendingEmailsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing pending emails.");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }

            _logger.LogInformation("EmailPendingWorker stopped.");
        }
    }
}
