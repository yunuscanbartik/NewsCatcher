using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsStatisticsController : ControllerBase
    {
        private INewsStatisticsService _newsStatisticsService;
        public NewsStatisticsController(INewsStatisticsService newsService)
        {
            _newsStatisticsService = newsService;
        }
        /// <summary>
        /// Belli bir habere ait istatistikleri döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetNewsStatistics")]
        public async Task<IActionResult> GetNewsStatisticsAsync([FromQuery] NewsStatisticsModel.BrowseModel.Request request)
        {
            var result = await _newsStatisticsService.GetNewsStatisticsByIdAsync(request);
            return Ok(result);
        }
    }
}
