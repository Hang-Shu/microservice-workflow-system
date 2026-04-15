using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Task.Api.Dtos;
using Task.Api.Entities;

namespace Task.Api.Services
{
    public interface ITaskService
    {
        Task<Result<Entities.TaskItems>> CreateTaskSingle(CreateTaskDto createTaskDto);

        Task<Result<List<Entities.TaskItems>>> GetAllTasks();

        Task<Result<Entities.TaskItems>> GetTaskById(Guid id);

        Task<Result> UpdateTaskStatusById(UpdateTaskStstusDto dto);
    }
}
