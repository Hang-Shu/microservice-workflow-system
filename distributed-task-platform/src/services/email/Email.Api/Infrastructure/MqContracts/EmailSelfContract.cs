namespace Email.Api.Infrastructure
{
    public static class EmailSelfContract
    {
        public const string DeadExchange = "email.dlx";
        public static class RoutingKeys
        {
            public const string TaskUpdatedDead = "task.updated.dead";
            public const string TaskCreatedDead = "task.created.dead";

        }
        public static class Queues
        {
            public const string Email_TaskUpdated = "email.task.updated.queue";
            public const string Email_TaskCreated = "email.task.created.queue";
            public const string Email_TaskUpdated_Dead = "email.task.update.dead.queue";
            public const string Email_TaskCreated_Dead = "email.task.created.dead.queue";
        }
    }
}
