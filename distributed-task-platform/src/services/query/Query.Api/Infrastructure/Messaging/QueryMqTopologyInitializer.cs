using RabbitMQ.Client;
using Shared.Common;
using Shared.Contracts;

namespace Query.Api.Infrastructure
{
    public class QueryMqTopologyInitializer : IMqTopologyInitializer
    {
        private readonly IConfiguration _configuration;

        public QueryMqTopologyInitializer(IConfiguration configuration)
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

            //init exchange that from Query.Api
            channel.ExchangeDeclare(
                exchange: TaskMqConstants.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
                );

            //init dead letter exchange
            channel.ExchangeDeclare(
                exchange: QuerySelfContract.DeadExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false
            );

            //init dead letter queue that task update used
            channel.QueueDeclare(
              queue: QuerySelfContract.Queues.Query_TaskUpdated_Dead,
              durable: true,
              exclusive: false,
              autoDelete: false
            );

            //Init queue that subscribe task updated
            channel.QueueDeclare(
                queue: QuerySelfContract.Queues.Query_TaskUpdated,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange",QuerySelfContract.DeadExchange},
                    {"x-dead-letter-routing-key",QuerySelfContract.RoutingKeys.TaskUpdatedDead }
                }
            );

            channel.QueueBind(
                queue: QuerySelfContract.Queues.Query_TaskUpdated_Dead,
                exchange: QuerySelfContract.DeadExchange,
                routingKey: QuerySelfContract.RoutingKeys.TaskUpdatedDead
            );
            channel.QueueBind(
                queue: QuerySelfContract.Queues.Query_TaskUpdated,
                exchange: TaskMqConstants.Exchange,
                routingKey: QuerySelfContract.RoutingKeys.TaskUpdate
            );
        }
    }
}
