using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace notification.Api.Consumers
{
    public class TaskCreatedEventConsumer : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var queueName = "notification-queue";
            var exchangeName = "task-events";

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: false,
                autoDelete: false
                );
            channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: "task.created"
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var taskEvent = JsonSerializer.Deserialize<TaskCreatedEvent>(message);

                System.Diagnostics.Debug.WriteLine($"[Notification] Received task: {taskEvent.TaskId}");
            };

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            return Task.CompletedTask;
        }
    }
}
