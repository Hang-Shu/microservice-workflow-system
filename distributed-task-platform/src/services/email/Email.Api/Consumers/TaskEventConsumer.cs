using Email.Api.Infrastructure;
using Email.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts;
using Shared.Contracts.Events;
using System.Text;
using System.Text.Json;

namespace Email.Api.Consumers
{
    public class TaskEventConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        public TaskEventConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
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

            InitCreateQueen(channel);
            InitUpdateQueen(channel);

            await Task.Delay(Timeout.Infinite, stoppingToken);

        }

        void InitUpdateQueen(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var routingKey = ea.RoutingKey;
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var taskEvent = JsonSerializer.Deserialize<TaskUpdatedEvent>(message);

                if (taskEvent == null || string.IsNullOrWhiteSpace(taskEvent.AssignedUserEmail) || taskEvent.AssignedUserNumber <= 0) 
                {
                    // TODO:Log
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }
                using var scope = _scopeFactory.CreateScope();
                var emailsPendingService = scope.ServiceProvider.GetRequiredService<IEmailsPendingService>();
                if(routingKey==TaskMqConstants.RoutingKeys.TaskUpdated_Assigned)
                {
                    var result = await emailsPendingService.HandleTaskUpdateAssignedAsync(taskEvent);
                    if (!result.IsSuccess)
                        channel.BasicReject(ea.DeliveryTag, false);
                    else
                        channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }
                else if(routingKey == TaskMqConstants.RoutingKeys.TaskUpdated_Priority)
                {
                    var result = await emailsPendingService.HandleTaskUpdatePriorityAsync(taskEvent);
                    if (!result.IsSuccess)
                        channel.BasicReject(ea.DeliveryTag, false);
                    else
                        channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                channel.BasicReject(ea.DeliveryTag, false);
                return;
            };
            channel.BasicConsume(
                queue: EmailSelfContract.Queues.Email_TaskUpdated,
                autoAck: false,
                consumer: consumer
            );
        }

        void InitCreateQueen(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var taskEvent = JsonSerializer.Deserialize<TaskCreatedEvent>(message);

                if (taskEvent == null || string.IsNullOrWhiteSpace(taskEvent.AssignedUserEmailAddress) || taskEvent.AssignedUserNumber <= 0
                || taskEvent.IsImportant == false) 
                {
                    // TODO:Log
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }
                using var scope = _scopeFactory.CreateScope();
                var emailsPendingService = scope.ServiceProvider.GetRequiredService<IEmailsPendingService>();

                var result = await emailsPendingService.HandleTaskCreatedAsync(taskEvent);
                if (!result.IsSuccess)
                    channel.BasicReject(ea.DeliveryTag, false);
                else
                    channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(
                queue: EmailSelfContract.Queues.Email_TaskCreated,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
