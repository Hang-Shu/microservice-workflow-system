using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Contracts.Events;
using Task.Api.Data;
using Task.Api.Dtos;
using Task.Api.Entities;

namespace Task.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _dbContext;
        private readonly IMapper _mapster;
        private readonly IEventPublisher _eventPublisher;

        public TaskService(IMapper mapper ,TaskDbContext dbContext, IEventPublisher eventPublisher)
        {
            _mapster = mapper;
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<TaskItems>> CreateTaskSingle(CreateTaskDto createTaskDto)
        {
            try
            {
                TaskItems taskItem = _mapster.Map<TaskItems>(createTaskDto);
                taskItem.Id = Guid.NewGuid();
                taskItem.CreatedTime = DateTime.UtcNow;
                _dbContext.Tasks.Add(taskItem);
                if(await _dbContext.SaveChangesAsync()>0)
                {
                    var taskCreatedEvent = new TaskCreatedEvent
                    {
                        TaskId = taskItem.Id,
                        Title = taskItem.Title,
                        AssignedUserId = taskItem.AssignedUserId,
                        CreatedTime = taskItem.CreatedTime
                    };
                    await _eventPublisher.PublishAsync(taskCreatedEvent);
                    return Result<TaskItems>.Success(taskItem);
                }
                return Result<TaskItems>.Failure("Insert Failed");
            }
            catch (Exception ex)
            {
                return Result<TaskItems>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<TaskItems>>> GetAllTasks()
        {
            try
            {
                List<TaskItems> lstResult = await _dbContext.Tasks.ToListAsync();
                return Result<List<TaskItems>>.Success(lstResult); 
            }
            catch(Exception ex)
            {
                return Result<List<TaskItems>>.Failure(ex.Message);
            }
        }

        public async Task<Result<TaskItems>> GetTaskById(Guid id)
        {
            try
            {
                var result = await _dbContext.Tasks.FindAsync(id);
                if (result == null)
                    return Result<TaskItems>.Failure("Can't find any data");
                return Result<TaskItems>.Success((TaskItems)result);
            }
            catch(Exception ex)
            {
                return Result<TaskItems>.Failure(ex.Message);
            }    
        }

        public async Task<Result> UpdateTaskStatusById(UpdateTaskStstusDto dto)
        {
            try
            {
                var result = await _dbContext.Tasks.FindAsync(dto.Id);
                if (result == null)
                    return Result.Failure("Can't find any data");
                result.Status = dto.Status;
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
