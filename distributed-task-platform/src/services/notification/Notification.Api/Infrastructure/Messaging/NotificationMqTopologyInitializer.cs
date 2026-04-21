using RabbitMQ.Client;
using Shared.Common;
using Shared.Contracts;

namespace Notification.Api.Infrastructure
{
    public class NotificationMqTopologyInitializer: IMqTopologyInitializer
    {
        private readonly IConfiguration _configuration;

        public NotificationMqTopologyInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"]!,
                UserName = _configuration["RabbitMQ:UserName"]!,
                Password = _configuration["RabbitMQ:Password"]!
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //init exchange that from Task.Api
            channel.ExchangeDeclare(
                exchange: TaskMqConstants.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
                );

            //init dead letter exchange
            channel.ExchangeDeclare(
                exchange: NotificationSelfContract.DeadExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false
            );

            //Init dead letter queue 
            channel.QueueDeclare(
              queue: NotificationSelfContract.Queues.Notification_TaskCreated_Dead,
              durable: true,
              exclusive: false,
              autoDelete: false
          );

            //Init queue that subscribe task created
            channel.QueueDeclare(
                queue: NotificationSelfContract.Queues.Notification_TaskCreated,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments:new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange",NotificationSelfContract .DeadExchange},
                    {"x-dead-letter-routing-key",NotificationSelfContract.RoutingKeys.TaskCreatedDead }
                }
            );

            channel.QueueBind(
                queue: NotificationSelfContract.Queues.Notification_TaskCreated_Dead,
                exchange: NotificationSelfContract.DeadExchange,
                routingKey: NotificationSelfContract.RoutingKeys.TaskCreatedDead
            );

            channel.QueueBind(
                queue: NotificationSelfContract.Queues.Notification_TaskCreated,
                exchange: TaskMqConstants.Exchange,
                routingKey: TaskMqConstants.RoutingKeys.TaskCreated
            );

            //Init queue that subscribe task updated
            channel.QueueDeclare(
                queue: NotificationSelfContract.Queues.Notification_TaskUpdated,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange",NotificationSelfContract .DeadExchange},
                    {"x-dead-letter-routing-key",NotificationSelfContract.RoutingKeys.TaskCreatedDead }
                }
            );
            channel.QueueBind(
                queue: NotificationSelfContract.Queues.Notification_TaskUpdated,
                exchange: TaskMqConstants.Exchange,
                routingKey: TaskMqConstants.RoutingKeys.TaskUpdated_Assigned
            );
            channel.QueueBind(
                queue: NotificationSelfContract.Queues.Notification_TaskUpdated,
                exchange: TaskMqConstants.Exchange,
                routingKey: TaskMqConstants.RoutingKeys.TaskUpdated_Priority
            );



        }
    }
}
