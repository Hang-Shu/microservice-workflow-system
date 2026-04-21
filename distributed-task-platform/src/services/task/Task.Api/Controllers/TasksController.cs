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

        [HttpPatch("Update/{taskNumber}/Title")]
        public async Task<IActionResult> UpdateTitle(int taskNumber, string newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdateTitle(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }

        [HttpPatch("Update/{taskNumber}/Description")]
        public async Task<IActionResult> UpdateDescription(int taskNumber, string newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdateDescription(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }

        [HttpPatch("Update/{taskNumber}/Project")]
        public async Task<IActionResult> UpdateProject(int taskNumber, Guid newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdateProject(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }

        [HttpPatch("Update/{taskNumber}/Status")]
        public async Task<IActionResult> UpdateProject(int taskNumber, int newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdateStatus(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }

        [HttpPatch("Update/{taskNumber}/Priority")]
        public async Task<IActionResult> UpdateProjectSingle(int taskNumber, int newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdatePriority(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }

        [HttpPatch("Update/{taskNumber}/Assigned")]
        public async Task<IActionResult> UpdateAssigned(int taskNumber, int newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdateAssigned(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }

        [HttpPatch("Update/{taskNumber}/Remark")]
        public async Task<IActionResult> UpdateRemark(int taskNumber, string newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdateRemark(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }

        [HttpPatch("Update/{taskNumber}/DueDate")]
        public async Task<IActionResult> UpdateDueDate(int taskNumber, DateOnly newValue, int updateUserNumber)
        {
            var result = await _taskService.UpdateDueDate(taskNumber, newValue, updateUserNumber);
            if (result.IsSuccess)
                return Ok("Update success");
            return NotFound(result.Error);
        }
    }
}
