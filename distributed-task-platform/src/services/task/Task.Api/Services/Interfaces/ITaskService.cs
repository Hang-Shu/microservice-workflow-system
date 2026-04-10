using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Task.Api.Dtos;
using Task.Api.Entities;

namespace Task.Api.Services
{
    public interface ITaskService
    {
        Task<Result<TaskItem>> CreateTaskSingle(CreateTaskDto createTaskDto);
    }
}
