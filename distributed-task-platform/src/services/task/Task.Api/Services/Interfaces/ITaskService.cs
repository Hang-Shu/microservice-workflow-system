using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Task.Api.Dtos;

namespace Task.Api.Services
{
    public interface ITaskService
    {
        Task<Result<TaskInfoReturnDto>> CreateTask(TaskCreateInDto dto);

        Task<Result<List<TaskInfoReturnDto>>> GetAllTasks();

        Task<Result<TaskInfoReturnDto>> GetTask(int taskNo);

        Task<Result> UpdateTask(int TaskNo, TaskUpdateInDto dto);
    }
}
