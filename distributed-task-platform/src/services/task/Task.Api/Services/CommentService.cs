using Shared.Common;
using Task.Api.Data;
using Task.Api.Dtos;
using Task.Api.Entities;
using Task.Api.Infrastructure;

namespace Task.Api.Services
{
    public class CommentService: ICommentService
    {
        private readonly TaskDbContext _dbContext;
        private readonly ITaskEventPublisher _eventPublisher;
        public CommentService(TaskDbContext dbContext, ITaskEventPublisher eventPublisher)
        {
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<TaskComment>> CreateComment(int taskNumber,CommentCreateDto dto)
        {
            try
            {
                TaskComment comment = new()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    CommentText = dto.CommentText,
                    UserNumber = dto.UserNumber,
                    CommentTime = DateTime.UtcNow
                };
                Users user = _dbContext.Users.Where(x => x.UserNumber == dto.UserNumber).FirstOrDefault();
                if (user == default)
                    return Result<TaskComment>.Failure("Not find user");
                _dbContext.TaskComment.Add(comment);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskCommentCreatedAsync(taskNumber, comment, user.UserName);
                    return Result<TaskComment>.Success(comment);
                }
                else
                {
                    return Result<TaskComment>.Failure("Faild insert database");
                }

            }
            catch (Exception ex)
            {
                return Result<TaskComment>.Failure(ex.Message);
            }
        }
        public async Task<Result> UpdateComment(Guid commentId, string newText)
        {
            try
            {
                TaskComment comment = _dbContext.TaskComment.Find(commentId);
                if (comment == null)
                    return Result.Failure("Can't find this comment");
                comment.CommentText = newText;
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskCommentUpdatedAsync(comment);
                    return Result.Success();
                }
                else
                {
                    return Result.Failure("Faild update database");
                }

            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
        public async Task<Result> DeleteComment(Guid commentId)
        {
            try
            {
                TaskComment comment = _dbContext.TaskComment.Find(commentId);
                if (comment == null)
                    return Result.Failure("Can't find this comment");
                _dbContext.TaskComment.Remove(comment);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskCommentDeletedAsync(comment);
                    return Result.Success();
                }
                else
                {
                    return Result.Failure("Faild delete database");
                }

            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
