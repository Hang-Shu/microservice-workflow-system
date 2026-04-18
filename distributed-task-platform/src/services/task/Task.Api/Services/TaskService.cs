using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Contracts.Events;
using Task.Api.Data;
using Task.Api.Dtos;
using Task.Api.Entities;
using Task.Api.Infrastructure;

namespace Task.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _dbContext;
        private readonly IMapper _mapster;
        private readonly ITaskEventPublisher _eventPublisher;

        public TaskService(IMapper mapper ,TaskDbContext dbContext, ITaskEventPublisher eventPublisher)
        {
            _mapster = mapper;
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<TaskInfoReturnDto>> CreateTask(TaskCreateInDto dto)
        {
            try
            {
                TaskItems taskItem = _mapster.Map<TaskItems>(dto);
                taskItem.Id = Guid.NewGuid();
                taskItem.CreatedTime = DateTime.UtcNow;
                _dbContext.Tasks.Add(taskItem);

                //Get poject name
                string projName = "";
                if(taskItem.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(taskItem.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }

                //Get assigned user info
                Users users = null;
                if (taskItem.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == taskItem.AssignedUserNumber).FirstOrDefault();
                
                

                if (await _dbContext.SaveChangesAsync()>0)
                {
                    await _eventPublisher.PublishTaskItemCreatedAsync(taskItem, projName, users);
                    TaskInfoReturnDto retuenDto = _mapster.Map<TaskInfoReturnDto>(taskItem);
                    return Result<TaskInfoReturnDto>.Success(retuenDto);
                }
                return Result<TaskInfoReturnDto>.Failure("Insert Failed");
            }
            catch (Exception ex)
            {
                return Result<TaskInfoReturnDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<TaskInfoReturnDto>>> GetAllTasks()
        {
            try
            {
                List<TaskItems> lstResult = await _dbContext.Tasks.ToListAsync();
                List<TaskInfoReturnDto> lstReturn = _mapster.Map<List<TaskInfoReturnDto>>(lstResult);
                return Result<List<TaskInfoReturnDto>>.Success(lstReturn); 
            }
            catch(Exception ex)
            {
                return Result<List<TaskInfoReturnDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<TaskInfoReturnDto>> GetTask(int taskNumber)
        {
            try
            {
                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result<TaskInfoReturnDto>.Failure("Can't find any data");
                TaskInfoReturnDto returnDto = _mapster.Map<TaskInfoReturnDto>(result);
                return Result<TaskInfoReturnDto>.Success(returnDto);
            }
            catch(Exception ex)
            {
                return Result<TaskInfoReturnDto>.Failure(ex.Message);
            }    
        }

        public async Task<Result> UpdateTask(int taskNo, TaskUpdateInDto dto)
        {
            try
            {
                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNo).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                _mapster.Map(dto, result);
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
