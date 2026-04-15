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
        public async Task<IActionResult> CreateNewTask(CreateTaskDto createTask)
        {
            var result = await _taskService.CreateTaskSingle(createTask);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var result = await _taskService.GetTaskById(id);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(result.Error);
        }

        [HttpPatch("ChangeStatus")]
        public async Task<IActionResult> UpdateTaskStatusById(UpdateTaskStstusDto updateDto)
        {
            var result = await _taskService.UpdateTaskStatusById(updateDto);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }
    }
}
