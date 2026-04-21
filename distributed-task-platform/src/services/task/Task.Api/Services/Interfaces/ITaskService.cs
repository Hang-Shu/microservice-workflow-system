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

        Task<Result> UpdateTitle(int taskNumber, string newValue, int updateUserNumber);

        Task<Result> UpdateDescription(int taskNumber, string newValue, int updateUserNumber);

        Task<Result> UpdateProject(int taskNumber, Guid newValue, int updateUserNumber);

        Task<Result> UpdateStatus(int taskNumber, int newValue, int updateUserNumber);

        Task<Result> UpdatePriority(int taskNumber, int newValue, int updateUserNumber);

        Task<Result> UpdateAssigned(int taskNumber, int newValue, int updateUserNumber);

        Task<Result> UpdateRemark(int taskNumber, string newValue, int updateUserNumber);

        Task<Result> UpdateDueDate(int taskNumber, DateOnly newValue, int updateUserNumber);
    }
}
