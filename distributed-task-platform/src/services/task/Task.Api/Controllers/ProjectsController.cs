using Microsoft.AspNetCore.Mvc;
using Task.Api.Dtos;
using Task.Api.Dtos.Project.In;
using Task.Api.Services;

namespace Task.Api.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {


        IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectCreateInDto dto)
        {
            var result = await _projectService.CreateProject(dto);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpPost("{id}/Update")]
        public async Task<IActionResult> UpdateProject(Guid id, ProjectUpdateInDto dto)
        {
            var result = await _projectService.UpdateProject(id, dto);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> DelUser(Guid id)
        {
            var result = await _projectService.DelProject(id);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }
    }
}
