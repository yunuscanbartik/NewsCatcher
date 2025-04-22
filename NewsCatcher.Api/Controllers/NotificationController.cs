using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications([FromQuery] NotificationModel.BrowseModel.Request request)
        {
            var result = await _notificationService.GetNotifications(request);
            return Ok(result.Data);
        }

        [HttpPost("AddNotification")]
        public async Task<IActionResult> AddNotification(NotificationModel.CreateModel.Request request)
        {
            var result = await _notificationService.AddNotification(request);
            return Ok(result.Data);
        }

        [HttpPut("NotificationIsRead")]
        public async Task<IActionResult> NotificationIsRead(NotificationModel.NotificationReadModel.Request request)
        {
            var result = await _notificationService.NotificationIsRead(request);
            return Ok(result.Data);
        }

        [HttpDelete("DeleteNotification")]
        public async Task<IActionResult> DeleteNotification(NotificationModel.DeleteModel.Request request)
        {
            var result = await _notificationService.DeleteNotification(request);
            return Ok(result.Data);
        }
    }
}
