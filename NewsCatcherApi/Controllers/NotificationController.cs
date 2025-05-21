using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        /// <summary>
        /// Belli bir haberin bildirimini döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetNotificationsById")]
        public async Task<IActionResult> GetNotificationsAsync([FromQuery] NotificationModel.BrowseModel.Request request)
        {
            var result = await _notificationService.GetNotificationByIdAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Var olan habere yeni bir bildirim ekler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddNotification")]
        public async Task<IActionResult> AddNotificationAsync(NotificationModel.CreateModel.Request request)
        {
            var result = await _notificationService.AddNotificationAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Bildirimin okunup okunmadığını belirler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("NotificationIsRead")]
        public async Task<IActionResult> NotificationIsReadAsync(NotificationModel.NotificationReadModel.Request request)
        {
            var result = await _notificationService.NotificationIsReadAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Bildirimi silmek için kullanılır.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNotification")]
        public async Task<IActionResult> DeleteNotificationAsync(NotificationModel.DeleteModel.Request request)
        {
            var result = await _notificationService.DeleteNotificationAsync(request);
            return Ok(result);
        }
    }
}
