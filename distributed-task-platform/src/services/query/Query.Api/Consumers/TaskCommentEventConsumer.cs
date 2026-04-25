using Query.Api.Infrastructure;
using Query.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts;
using Shared.Contracts.Events;
using System.Text;
using System.Text.Json;

namespace Query.Api.Consumers
{
    public class TaskCommentEventConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        public TaskCommentEventConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
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

            InitCommentQueen(channel);

            await Task.Delay(Timeout.Infinite, stoppingToken);

        }
        void InitCommentQueen(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var routingKey = ea.RoutingKey;
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var taskEvent = JsonSerializer.Deserialize<CommentEvent>(message);

                if (taskEvent == null)
                {
                    // TODO:Log
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }
                using var scope = _scopeFactory.CreateScope();
                var commentService = scope.ServiceProvider.GetRequiredService<ITaskCommentService>();
                switch(ea.RoutingKey)
                {
                    case TaskMqConstants.RoutingKeys.TaskCommentCreated:
                        var result = await commentService.HandleTaskCommentCreatedAsync(taskEvent);
                        if (!result.IsSuccess)
                            channel.BasicReject(ea.DeliveryTag, false);
                        else
                            channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    case TaskMqConstants.RoutingKeys.TaskCommentUpdated:
                        var results = await commentService.HandleTaskCommentUpdatedAsync(taskEvent);
                        if (!results.IsSuccess)
                            channel.BasicReject(ea.DeliveryTag, false);
                        else
                            channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    case TaskMqConstants.RoutingKeys.TaskCommentDeleted:
                        var resultss = await commentService.HandleTaskCommentDeletedAsync(taskEvent);
                        if (!resultss.IsSuccess)
                            channel.BasicReject(ea.DeliveryTag, false);
                        else
                            channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    default:
                        channel.BasicReject(ea.DeliveryTag, false);
                        return;
                }
            };
            channel.BasicConsume(
                queue: QuerySelfContract.Queues.Query_Comment,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
