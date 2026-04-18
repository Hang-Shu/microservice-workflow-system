using Microsoft.AspNetCore.Mvc;
using Task.Api.Dtos;
using Task.Api.Services;

namespace Task.Api.Controllers
{

   
    [Route("api/users")]
    [ApiController]
    public class UserController:ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateInDto dto)
        {
            var result = await _userService.CreateUser(dto);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        [HttpPost("{userNumber}/Update")]
        public async Task<IActionResult> UpdateUser(int userNumber, UserUpdateInDto dto)
        {
            var result = await _userService.UpdateUser(userNumber, dto);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpPost("{userNumber}/Delete")]
        public async Task<IActionResult> DelUser(int userNumber)
        {
            var result = await _userService.DelUser(userNumber);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

    }
}
