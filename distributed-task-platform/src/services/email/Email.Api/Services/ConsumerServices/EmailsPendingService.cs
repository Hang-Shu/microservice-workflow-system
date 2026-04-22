using Email.Api.Data;
using Email.Api.Dtos;
using Email.Api.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common;
using Shared.Contracts.Events;
using System.Text.Json;

namespace Email.Api.Services
{
    public class EmailsPendingService:IEmailsPendingService
    {
        private readonly EmailDbContext _dbContext;

        public EmailsPendingService(EmailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> HandleTaskUpdateAssignedAsync(TaskUpdatedEvent dto)
        {
            try
            {
                DateTime dtNow = DateTime.UtcNow;
                //Check if old assigned user number vaild
                if (dto.OldAssignedUserNumber > 0)
                {
                    TasksEmailsPending pendingOld = _dbContext.TasksEmailsPending.Where(x => x.ReciveUserNumber == dto.OldAssignedUserNumber
                                                && x.PendingTaskNumbers.Contains(dto.TaskNumber) && x.PlanSentTime >= dto.UpdateTime).FirstOrDefault();
                    if (pendingOld != null)
                    {//Delete this task from old recieve user pending list
                        pendingOld.PendingTaskNumbers.Remove(dto.TaskNumber);
                        pendingOld.LastUpdateTime = dtNow;
                    }
                }

                //Check if pending email is exist for new assigned user
                TasksEmailsPending pendingNew = _dbContext.TasksEmailsPending.Where(x => x.ReciveUserNumber == dto.AssignedUserNumber
                                                && x.PlanSentTime >= dto.UpdateTime).FirstOrDefault();

                if (pendingNew == null)
                {//Create new pending list for new recieve user
                    List<TaskTitleInfoDto> lstTitle = new();
                    lstTitle.Add(new() { TaskNumber = dto.TaskNumber, TaskTitle = dto.Title });
                    pendingNew = new()
                    {
                        Id = Guid.NewGuid(),
                        ReciveUserNumber = dto.AssignedUserNumber,
                        EmailAccount = dto.AssignedUserEmail,
                        PendingTaskNumbers = new() { dto.TaskNumber },
                        TaskTitles = JsonSerializer.Serialize(lstTitle),
                        FirstEventTime = dto.UpdateTime,
                        LastUpdateTime = dtNow,
                        PlanSentTime = dtNow.AddMinutes(1.8)
                    };
                    _dbContext.TasksEmailsPending.Add(pendingNew);
                }
                else
                {//Update pending list info(TaskNumber/Title) for new recieve user
                    if (!pendingNew.PendingTaskNumbers.Contains(dto.TaskNumber))
                        pendingNew.PendingTaskNumbers.Add(dto.TaskNumber);
                    List<TaskTitleInfoDto> lstTitle = JsonSerializer.Deserialize<List<TaskTitleInfoDto>>(pendingNew.TaskTitles);
                    TaskTitleInfoDto titleDto = lstTitle.Where(x => x.TaskNumber == dto.TaskNumber).FirstOrDefault();
                    if (titleDto != null)
                        titleDto.TaskTitle = dto.Title;
                    else
                    {
                        titleDto = new()
                        {
                            TaskNumber = dto.TaskNumber,
                            TaskTitle = dto.Title
                        };
                        lstTitle.Add(titleDto);
                    }
                    pendingNew.TaskTitles = JsonSerializer.Serialize(lstTitle);
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                else
                    return Result.Failure("Save filed in database");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }


        public async Task<Result> HandleTaskUpdatePriorityAsync(TaskUpdatedEvent dto)
        {
            try
            {
                DateTime dtNow = DateTime.UtcNow;
                if (!dto.IsImportant)
                {
                    //Just need cancel info that pending sent
                    TasksEmailsPending cancelPending = _dbContext.TasksEmailsPending.Where(x => x.ReciveUserNumber == dto.AssignedUserNumber
                                                     && x.PendingTaskNumbers.Contains(dto.TaskNumber) && x.PlanSentTime >= dto.UpdateTime).FirstOrDefault();
                    if(cancelPending!=null)
                    {
                        cancelPending.PendingTaskNumbers.Remove(dto.TaskNumber);
                        cancelPending.LastUpdateTime = dtNow;
                        if (await _dbContext.SaveChangesAsync() > 0)
                            return Result.Success();
                        else
                            return Result.Failure("Save filed in database");
                    }
                    else
                        return Result.Success();
                }

                //Check if pending email is exist for new assigned user
                TasksEmailsPending pendingNew = _dbContext.TasksEmailsPending.Where(x => x.ReciveUserNumber == dto.AssignedUserNumber
                                                && x.PlanSentTime >= dto.UpdateTime).FirstOrDefault();

                if (pendingNew == null)
                {//Create new pending list for new recieve user
                    List<TaskTitleInfoDto> lstTitle = new();
                    lstTitle.Add(new() { TaskNumber = dto.TaskNumber, TaskTitle = dto.Title });
                    pendingNew = new()
                    {
                        Id = Guid.NewGuid(),
                        ReciveUserNumber = dto.AssignedUserNumber,
                        EmailAccount = dto.AssignedUserEmail,
                        PendingTaskNumbers = new() { dto.TaskNumber },
                        TaskTitles = JsonSerializer.Serialize(lstTitle),
                        FirstEventTime = dto.UpdateTime,
                        LastUpdateTime = dtNow,
                        PlanSentTime = dtNow.AddMinutes(1.8)
                    };
                    _dbContext.TasksEmailsPending.Add(pendingNew);
                }
                else
                {//Update pending list info(TaskNumber/Title) for new recieve user
                    if (!pendingNew.PendingTaskNumbers.Contains(dto.TaskNumber))
                        pendingNew.PendingTaskNumbers.Add(dto.TaskNumber);
                    List<TaskTitleInfoDto> lstTitle = JsonSerializer.Deserialize<List<TaskTitleInfoDto>>(pendingNew.TaskTitles);
                    TaskTitleInfoDto titleDto = lstTitle.Where(x => x.TaskNumber == dto.TaskNumber).FirstOrDefault();
                    if (titleDto != null)
                        titleDto.TaskTitle = dto.Title;
                    else
                    {
                        titleDto = new()
                        {
                            TaskNumber = dto.TaskNumber,
                            TaskTitle = dto.Title
                        };
                        lstTitle.Add(titleDto);
                    }
                    pendingNew.TaskTitles = JsonSerializer.Serialize(lstTitle);
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                else
                    return Result.Failure("Save filed in database");

            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }


        public async Task<Result> HandleTaskCreatedAsync(TaskCreatedEvent dto)
        {
            try
            {
                //Only high priority task need this
                DateTime dtNow = DateTime.UtcNow;


                TasksEmailsPending pendingNew = _dbContext.TasksEmailsPending.Where(x => x.ReciveUserNumber == dto.AssignedUserNumber
                                                && x.PlanSentTime >= dto.CreatedTime).FirstOrDefault();

                if (pendingNew == null)
                {//Create new pending list for new recieve user
                    List<TaskTitleInfoDto> lstTitle = new();
                    lstTitle.Add(new() { TaskNumber = dto.TaskNumber, TaskTitle = dto.Title });
                    pendingNew = new()
                    {
                        Id = Guid.NewGuid(),
                        ReciveUserNumber = dto.AssignedUserNumber.Value,
                        EmailAccount = dto.AssignedUserEmailAddress,
                        PendingTaskNumbers = new() { dto.TaskNumber },
                        TaskTitles = JsonSerializer.Serialize(lstTitle),
                        FirstEventTime = dto.CreatedTime,
                        LastUpdateTime = dtNow,
                        PlanSentTime = dtNow.AddMinutes(1.8)
                    };
                    _dbContext.TasksEmailsPending.Add(pendingNew);
                }
                else
                {//Update pending list info(TaskNumber/Title) for new recieve user
                    if (!pendingNew.PendingTaskNumbers.Contains(dto.TaskNumber))
                        pendingNew.PendingTaskNumbers.Add(dto.TaskNumber);
                    List<TaskTitleInfoDto> lstTitle = JsonSerializer.Deserialize<List<TaskTitleInfoDto>>(pendingNew.TaskTitles);
                    TaskTitleInfoDto titleDto = lstTitle.Where(x => x.TaskNumber == dto.TaskNumber).FirstOrDefault();
                    if (titleDto != null)
                        titleDto.TaskTitle = dto.Title;
                    else
                    {
                        titleDto = new()
                        {
                            TaskNumber = dto.TaskNumber,
                            TaskTitle = dto.Title
                        };
                        lstTitle.Add(titleDto);
                    }
                    pendingNew.TaskTitles = JsonSerializer.Serialize(lstTitle);
                }


                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                else
                    return Result.Failure("Save filed in database");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
