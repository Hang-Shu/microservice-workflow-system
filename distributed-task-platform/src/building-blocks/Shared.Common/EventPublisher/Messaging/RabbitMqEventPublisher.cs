using RabbitMQ.Client;
using Shared.Common;
using System.Text;
using System.Text.Json;

namespace Shared.Common
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public RabbitMqEventPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync<T>(T @event) where T : IExchangeInfo
        {
            string exchangeName = @event.ExchangeName;
            string routingKey = @event.RoutingKey;

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                body: body
            );

            return Task.CompletedTask;
        }
    }
}
