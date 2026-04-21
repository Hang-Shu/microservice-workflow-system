using Query.Api.Infrastructure;
using Query.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using System.Text;
using System.Text.Json;

namespace Query.Api.Consumers
{
    public class TaskUpdatedEventConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        public TaskUpdatedEventConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"]!,
                UserName = _configuration["RabbitMQ:UserName"]!,
                Password = _configuration["RabbitMQ:Password"]!
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            InitUpdateQueen(channel);

            await Task.Delay(Timeout.Infinite, stoppingToken);

        }

        void InitUpdateQueen(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var exchangeKey = ea.RoutingKey;
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var taskEvent = JsonSerializer.Deserialize<TaskUpdatedEvent>(message);

                if (taskEvent == null)
                {
                    // TODO:Log
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }
                using var scope = _scopeFactory.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<ITaskModifyLineService>();

                var result = await notificationService.HandleOperateTaskUpdateAsync(taskEvent, exchangeKey);
                if (!result.IsSuccess)
                    channel.BasicReject(ea.DeliveryTag, false);
                else
                    channel.BasicAck(ea.DeliveryTag, false);
                return;
            };
            channel.BasicConsume(
                queue: QuerySelfContract.Queues.Query_TaskUpdated,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
