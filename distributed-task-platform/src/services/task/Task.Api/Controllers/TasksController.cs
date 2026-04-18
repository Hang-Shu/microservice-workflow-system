using Microsoft.AspNetCore.Mvc;
using Task.Api.Dtos;
using Task.Api.Services;


namespace Task.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController:ControllerBase
    {

        ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTask(TaskCreateInDto dto)
        {
            var result = await _taskService.CreateTask(dto);
            if(result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _taskService.GetAllTasks();
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        [HttpGet("{taskNumber}")]
        public async Task<IActionResult> GetTask(int taskNumber)
        {
            var result = await _taskService.GetTask(taskNumber);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(result.Error);
        }

        [HttpPatch("{taskNumber}/Update")]
        public async Task<IActionResult> UpdateTask(int taskNumber, TaskUpdateInDto dto)
        {
            var result = await _taskService.UpdateTask(taskNumber,dto);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }
    }
}
