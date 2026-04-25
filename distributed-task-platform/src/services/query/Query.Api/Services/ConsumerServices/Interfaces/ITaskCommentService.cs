using Shared.Common;
using Shared.Contracts.Events;

namespace Query.Api.Services
{
    public interface ITaskCommentService
    {
        public Task<Result> HandleTaskCommentCreatedAsync(CommentEvent dto);
        public Task<Result> HandleTaskCommentUpdatedAsync(CommentEvent dto);
        public Task<Result> HandleTaskCommentDeletedAsync(CommentEvent dto);
    }
}
