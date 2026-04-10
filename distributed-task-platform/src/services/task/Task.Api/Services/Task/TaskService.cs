using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Task.Api.Data;
using Task.Api.Dtos;
using Task.Api.Entities;

namespace Task.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _dbContext;
        private readonly IMapper _mapster;

        public TaskService(IMapper mapper ,TaskDbContext dbContext)
        {
            _mapster = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result<TaskItem>> CreateTaskSingle(CreateTaskDto createTaskDto)
        {
            try
            {
                TaskItem taskItem = _mapster.Map<TaskItem>(createTaskDto);
                taskItem.Id = Guid.NewGuid();
                taskItem.CreatedTime = DateTime.UtcNow;
                _dbContext.Tasks.Add(taskItem);
                if(await _dbContext.SaveChangesAsync()>0)
                    return Result<TaskItem>.Success(taskItem);
                return Result<TaskItem>.Failure("Insert Failed");
            }
            catch (Exception ex)
            {
                return Result<TaskItem>.Failure(ex.Message);
            }
        }
    }
}
