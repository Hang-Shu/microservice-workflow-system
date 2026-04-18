using Microsoft.AspNetCore.Mvc;
using Notification.Api.Dtos;
using Notification.Api.Services;

namespace Notification.Api.Controllers
{
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(NotificationCreateInDto dto)
        {
            var result = await _notificationService.CreateNewMessage(dto);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }
    }
}
