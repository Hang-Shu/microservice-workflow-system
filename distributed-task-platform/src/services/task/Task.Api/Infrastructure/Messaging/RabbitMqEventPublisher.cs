using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using Shared.Common;
using System.Text;
using System.Text.Json;

namespace Task.Api
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;


        public RabbitMqEventPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public System.Threading.Tasks.Task PublishAsync<T>(T @event)
        {
            var queueName = typeof(T).Name;
            var exchangeName = "task-events";

            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            _channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: false,
                autoDelete: false
                );

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: "task.created",
                basicProperties: null,
                body: body
            );

            System.Diagnostics.Debug.WriteLine($"[RabbitMQ] Published event {queueName}");

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
