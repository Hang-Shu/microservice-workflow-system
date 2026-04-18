namespace Notification.Api.Infrastructure
{
    public static class NotificationSelfContract
    {
        public const string DeadExchange = "notification.dlx";
        public static class RoutingKeys
        {
            public const string TaskCreatedDead = "task.created.dead";
        }
        public static class Queues
        {
            public const string Notification_TaskCreated = "notification.task.created.queue";
            public const string Notification_TaskCreated_Dead = "notification.task.created.dead.queue";
        }
        
    }
}
