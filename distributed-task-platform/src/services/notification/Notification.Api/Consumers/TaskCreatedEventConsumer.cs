using Notification.Api.Infrastructure;
using Notification.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Common;
using Shared.Contracts;
using Shared.Contracts.Events;
using System.Text;
using System.Text.Json;

namespace notification.Api.Consumers
{
    public class TaskCreatedEventConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public TaskCreatedEventConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
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

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var taskEvent = JsonSerializer.Deserialize<TaskCreatedEvent>(message);
                if(taskEvent==null)
                {
                    // TODO:Log
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }


                if (!taskEvent.AssignedUserNumber.HasValue) 
                {// Not assigned any person
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }
                    

                using var scope = _scopeFactory.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                var result = await notificationService.HandleTaskCreatedAsync(taskEvent);

                if (!result.IsSuccess)
                    channel.BasicReject(ea.DeliveryTag,false);
                else
                    channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(
                queue: NotificationSelfContract.Queues.Notification_TaskCreated,
                autoAck: false,
                consumer: consumer
            );
            await Task.Delay(Timeout.Infinite, stoppingToken);

        }
    }
}
