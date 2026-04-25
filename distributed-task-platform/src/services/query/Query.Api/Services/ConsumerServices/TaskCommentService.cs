using Query.Api.Data;
using Query.Api.Entities;
using Query.Api.Infrastructure;
using Shared.Common;
using Shared.Contracts.Events;

namespace Query.Api.Services
{
    public class TaskCommentService: ITaskCommentService
    {
        private readonly QueryDbContext _dbContext;

        public TaskCommentService(QueryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result> HandleTaskCommentCreatedAsync(CommentEvent dto)
        {
            try
            {
                //Check if this comment is exist equals idempotent check
                TaskCommentRead taskComment =
                    _dbContext.TaskCommentRead.Where(x => x.TaskCommentId == dto.CommentId).FirstOrDefault();
               
                if (taskComment != null)
                {//Exist, return
                    return Result.Success();
                }
                //Not exist, insert new comment
                string strPreview;
                bool isPreview = false;
                if(dto.CommentText.Length>30)
                {
                    strPreview = dto.CommentText.Substring(0, 30);
                    isPreview = true;
                }
                else
                    strPreview=dto.CommentText;
                taskComment = new()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = dto.TaskNumber,
                    TaskCommentId = dto.CommentId,
                    UserNumber = dto.UserNumber,
                    DisplayNameSnapshot = dto.DisplayNameSnapshot,
                    PreviewText = strPreview,
                    IsPreview = isPreview,
                    CreateTime = dto.CreateTime
                };
                _dbContext.TaskCommentRead.Add(taskComment);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                return Result.Failure("Insert database filed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> HandleTaskCommentUpdatedAsync(CommentEvent dto)
        {
            try
            {
                //Check if this comment is exist 
                TaskCommentRead taskComment =
                    _dbContext.TaskCommentRead.Where(x => x.TaskCommentId == dto.CommentId).FirstOrDefault();

                if (taskComment == null)
                {//Exist, return
                    return Result.Failure("This data isn't exist");
                }

                if (taskComment.UpdateTime.HasValue && taskComment.UpdateTime >= dto.CreateTime)
                    return Result.Success();

                if (dto.CommentText.Length > 30)
                {
                    taskComment.PreviewText = dto.CommentText.Substring(0, 30);
                    taskComment.IsPreview = true;
                }
                else
                {
                    taskComment.PreviewText = dto.CommentText;
                    taskComment.IsPreview = false;
                }
                taskComment.UpdateTime= dto.CreateTime;

                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                return Result.Failure("Update database filed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> HandleTaskCommentDeletedAsync(CommentEvent dto)
        {
            try
            {
                //Check if this comment is exist 
                TaskCommentRead taskComment =
                    _dbContext.TaskCommentRead.Where(x => x.TaskCommentId == dto.CommentId).FirstOrDefault();

                if (taskComment == null)
                {//Exist, return
                    return Result.Success();
                }
                _dbContext.TaskCommentRead.Remove(taskComment);

                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                return Result.Failure("Delete database filed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

    }
}
