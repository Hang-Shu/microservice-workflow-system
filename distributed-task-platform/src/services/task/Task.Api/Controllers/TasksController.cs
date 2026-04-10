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
            return BadRequest(result.Message);
        }
    }
}
