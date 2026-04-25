using Microsoft.AspNetCore.Mvc;
using Query.Api.Services;

namespace Query.Api.Controllers
{
    [Route("api/query")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        IQueryTaskDatasService _queryTaskDatasService;
        public QueryController(IQueryTaskDatasService queryTaskDatasService)
        {
            _queryTaskDatasService = queryTaskDatasService;
        }

        [HttpGet("{taskNumber}")]
        public async Task<IActionResult> GetTaskDetail(int taskNumber)
        {
            var result = await _queryTaskDatasService.QueryTaskInfoByTimeLine(taskNumber);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }
    }
}
