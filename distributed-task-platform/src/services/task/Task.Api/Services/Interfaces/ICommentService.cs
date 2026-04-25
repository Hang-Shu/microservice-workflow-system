using Shared.Common;
using Task.Api.Dtos;
using Task.Api.Entities;

namespace Task.Api.Services
{
    public interface ICommentService
    {
        public Task<Result<TaskComment>> CreateComment(int taskNumber, CommentCreateDto dto);
        public Task<Result> UpdateComment(Guid commentId, string newText);
        public Task<Result> DeleteComment(Guid commentId);
    }
}
