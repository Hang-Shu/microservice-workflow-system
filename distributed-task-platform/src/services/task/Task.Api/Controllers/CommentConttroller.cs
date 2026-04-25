using Microsoft.AspNetCore.Mvc;
using Task.Api.Dtos;
using Task.Api.Services;

namespace Task.Api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentConttroller : ControllerBase
    {
        ICommentService _commentService;

        public CommentConttroller(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("{taskNumber}")]
        public async Task<IActionResult> CreateComment(int taskNumber,CommentCreateDto dto)
        {
            var result = await _commentService.CreateComment(taskNumber, dto);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        [HttpPost("{commentId}/Update")]
        public async Task<IActionResult> CreateComment(Guid commentId, string strNewValue)
        {
            var result = await _commentService.UpdateComment(commentId, strNewValue);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpDelete("{commentId}/Delete")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var result = await _commentService.DeleteComment(commentId);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }
    }
}
