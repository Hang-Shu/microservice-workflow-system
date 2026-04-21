using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Shared.Common;
using Shared.Contracts;
using Shared.Contracts.Events;
using Task.Api.Data;
using Task.Api.Dtos;
using Task.Api.Entities;
using Task.Api.Enum;
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
        public async Task<Result> UpdateTitle(int taskNumber, string newValue, int updateUserNumber)
        {
            try
            {
                //New value empty check
                if (newValue == "")
                    return Result.Failure("Title's new value is not vaild");

                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "Title",
                    OldValue = result.Title,
                    NewValue = newValue
                };

                result.Title = newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);


                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate= _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users,userUpdate,TaskMqConstants.RoutingKeys.TaskUpdated_Title);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateDescription(int taskNumber, string newValue, int updateUserNumber)
        {
            try
            {
                //New value empty check
                if (newValue == "")
                    return Result.Failure("Description's new value is not vaild");

                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "Description",
                    OldValue = result.Description,
                    NewValue = newValue
                };

                result.Description = newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);

                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate = _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users, userUpdate, TaskMqConstants.RoutingKeys.TaskUpdated_Description);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateProject(int taskNumber, Guid newValue, int updateUserNumber)
        {
            try
            {
                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                string strOldName = "";
                if(result.ProjectId.HasValue)
                {//Get old project Name
                    Projects oldProjects = _dbContext.Projects.Find(result.ProjectId.Value);
                    if (oldProjects != null)
                        strOldName = oldProjects.ProjectName;
                }

                string strNewName = _dbContext.Projects.Find(newValue).ProjectName;

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "Project",
                    OldValue = strOldName,
                    NewValue = strNewName
                };

                result.ProjectId = newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);

                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate = _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users, userUpdate, TaskMqConstants.RoutingKeys.TaskUpdated_Project);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateStatus(int taskNumber, int newValue, int updateUserNumber)
        {
            try
            {
                //New value vaild check
                if (!System.Enum.IsDefined(typeof(TaskStatusEnum), newValue))
                    return Result.Failure("Status's new value is not vaild");
                

                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "Status",
                    OldValue = result.Status.ToString(),
                    NewValue = ((TaskStatusEnum)newValue).ToString()
                };

                result.Status = (TaskStatusEnum)newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);

                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate = _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users, userUpdate, TaskMqConstants.RoutingKeys.TaskUpdated_Status);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdatePriority(int taskNumber, int newValue, int updateUserNumber)
        {
            try
            {
                //New value vaild check
                if (!System.Enum.IsDefined(typeof(TaskPriorityEnum), newValue))
                    return Result.Failure("Priority's new value is not vaild");

                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "Priority",
                    OldValue = result.Priority.ToString(),
                    NewValue = ((TaskPriorityEnum)newValue).ToString()
                };

                result.Priority = (TaskPriorityEnum)newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);

                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate = _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users, userUpdate, TaskMqConstants.RoutingKeys.TaskUpdated_Priority);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateAssigned(int taskNumber, int newValue, int updateUserNumber)
        {
            try
            {
                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                int oldAssignedUserNumber;
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "AssignedUser",
                    OldValue = result.AssignedUserNumber.HasValue ? "$" + result.AssignedUserNumber.Value.ToString("D4") + " " : "",
                    NewValue = "$" + newValue.ToString("D4") + " "
                };

                oldAssignedUserNumber = result.AssignedUserNumber.HasValue ? result.AssignedUserNumber.Value:-1;
                result.AssignedUserNumber = newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);

                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate = _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users, userUpdate, TaskMqConstants.RoutingKeys.TaskUpdated_Assigned,oldAssignedUserNumber);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateRemark(int taskNumber, string newValue, int updateUserNumber)
        {
            try
            {
                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "Remark",
                    OldValue = result.Remark,
                    NewValue = newValue
                };

                result.Remark = newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);

                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate = _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users, userUpdate, TaskMqConstants.RoutingKeys.TaskUpdated_Remark);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateDueDate(int taskNumber, DateOnly newValue, int updateUserNumber)
        {
            try
            {
                var result = _dbContext.Tasks.Where(x => x.TaskNumber == taskNumber).FirstOrDefault();
                if (result == null)
                    return Result.Failure("Can't find this task");
                //Create new TaskOperates and it's dtl
                TaskOperates itemOperate = new TaskOperates()
                {
                    Id = Guid.NewGuid(),
                    TaskNumber = taskNumber,
                    UserNumber = updateUserNumber,
                    OperateTime = DateTime.UtcNow
                };

                TaskOperates_Dtl itemOperateDtl = new TaskOperates_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = itemOperate.Id,
                    OpreateType = "DueDate",
                    OldValue = result.DueDate.ToString("dd MMM yyyy"),
                    NewValue = newValue.ToString("dd MMM yyyy")
                };

                result.DueDate = newValue;

                //Db add datas
                _dbContext.TaskOperates.Add(itemOperate);
                _dbContext.TaskOperates_Dtl.Add(itemOperateDtl);

                //Get poject name
                string projName = "";
                if (result.ProjectId.HasValue)
                {
                    Projects projects = _dbContext.Projects.Find(result.ProjectId);
                    if (projects != null)
                        projName = projects.ProjectName;
                }
                //Get user info
                Users users = null;
                if (result.AssignedUserNumber.HasValue)
                    users = _dbContext.Users.Where(x => x.UserNumber == result.AssignedUserNumber).FirstOrDefault();
                Users userUpdate = _dbContext.Users.Where(x => x.UserNumber == updateUserNumber).FirstOrDefault();

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    await _eventPublisher.PublishTaskItemUpdatedAsync(itemOperate.OperateTime, result, projName, users, userUpdate, TaskMqConstants.RoutingKeys.TaskUpdated_DueDate);
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
