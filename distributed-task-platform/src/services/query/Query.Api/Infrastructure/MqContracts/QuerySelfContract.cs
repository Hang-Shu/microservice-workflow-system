namespace Query.Api.Infrastructure
{
    public static class QuerySelfContract
    {
        public const string DeadExchange = "query.dlx";
        

        public static class RoutingKeys
        {
            public const string TaskUpdate = "task.updated.*";
            public const string Comment = "task.comment.*";
            public const string TaskUpdatedDead = "task.updated.dead";
            public const string EmailSendDead = "email.send.dead";
            public const string TaskCommentDead = "comment.dead";
        }
        public static class Queues
        {
            public const string Query_TaskUpdated = "query.task.updated.queue";
            public const string Query_TaskUpdated_Dead = "query.task.update.dead.queue";
            public const string Query_EmailSend = "query.email.send.queue";
            public const string Query_EmailSend_Dead = "query.email.send.dead.queue";
            public const string Query_Comment = "query.task.comment.queue";
            public const string Query_Comment_Dead = "query.task.comment.dead.queue";
        }
    }
}
