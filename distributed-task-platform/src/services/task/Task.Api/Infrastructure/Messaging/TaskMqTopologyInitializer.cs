using RabbitMQ.Client;
using Shared.Common;
using Shared.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Task.Api.Infrastructure
{
    public class TaskMqTopologyInitializer:IMqTopologyInitializer
    {
        private readonly IConfiguration _configuration;

        public TaskMqTopologyInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"]!,
                UserName = _configuration["RabbitMQ:UserName"]!,
                Password= _configuration["RabbitMQ:Password"]!
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
            //init exchange that Task.Api used
            channel.ExchangeDeclare(
            exchange: TaskMqConstants.Exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false
            );
        }
    }
}
