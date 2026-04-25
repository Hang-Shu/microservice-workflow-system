using RabbitMQ.Client;
using Shared.Common;
using Shared.Contracts;

namespace Email.Api.Infrastructure
{
    public class EmailMqTopologyInitializer : IMqTopologyInitializer
    {
        private readonly IConfiguration _configuration;

        public EmailMqTopologyInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //init exchange that from Email.Api
            channel.ExchangeDeclare(
                exchange: TaskMqConstants.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
                );

            //init dead letter exchange
            channel.ExchangeDeclare(
                exchange: EmailSelfContract.DeadExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false
            );

            //init dead letter queue that task update used
            channel.QueueDeclare(
              queue: EmailSelfContract.Queues.Email_TaskUpdated_Dead,
              durable: true,
              exclusive: false,
              autoDelete: false
            );
            //init dead letter queue that task create used
            channel.QueueDeclare(
              queue: EmailSelfContract.Queues.Email_TaskCreated_Dead,
              durable: true,
              exclusive: false,
              autoDelete: false
            );
            //Init queue that subscribe task updated
            channel.QueueDeclare(
                queue: EmailSelfContract.Queues.Email_TaskUpdated,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange",EmailSelfContract.DeadExchange},
                    {"x-dead-letter-routing-key",EmailSelfContract.RoutingKeys.TaskUpdatedDead }
                }
            );
            //Init queue that subscribe task created
            channel.QueueDeclare(
                queue: EmailSelfContract.Queues.Email_TaskCreated,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange",EmailSelfContract.DeadExchange},
                    {"x-dead-letter-routing-key",EmailSelfContract.RoutingKeys.TaskCreatedDead }
                }
            );

            channel.QueueBind(
                queue: EmailSelfContract.Queues.Email_TaskUpdated_Dead,
                exchange: EmailSelfContract.DeadExchange,
                routingKey: EmailSelfContract.RoutingKeys.TaskUpdatedDead
            );
            channel.QueueBind(
                queue: EmailSelfContract.Queues.Email_TaskCreated_Dead,
                exchange: EmailSelfContract.DeadExchange,
                routingKey: EmailSelfContract.RoutingKeys.TaskCreatedDead
            );
            channel.QueueBind(
                queue: EmailSelfContract.Queues.Email_TaskCreated,
                exchange: TaskMqConstants.Exchange,
                routingKey: TaskMqConstants.RoutingKeys.TaskCreated
            );
            channel.QueueBind(
                queue: EmailSelfContract.Queues.Email_TaskUpdated,
                exchange: TaskMqConstants.Exchange,
                routingKey: TaskMqConstants.RoutingKeys.TaskUpdated_Assigned
            );
            channel.QueueBind(
                queue: EmailSelfContract.Queues.Email_TaskUpdated,
                exchange: TaskMqConstants.Exchange,
                routingKey: TaskMqConstants.RoutingKeys.TaskUpdated_Priority
            );
            //Init exchange that Email.Api to Query.Api
            channel.ExchangeDeclare(
                exchange: EmailMqConstants.Exchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false
            );
        }
    }
}
