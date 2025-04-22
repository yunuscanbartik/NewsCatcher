using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NewsStatisticsController : ControllerBase
    {
        private readonly INewsStatisticsService _newsStatisticsService;

        public NewsStatisticsController(INewsStatisticsService newsStatisticsService)
        {
            _newsStatisticsService = newsStatisticsService;
        }

        [HttpGet("GetNewsStatistics")]
        public async Task<IActionResult> GetNewsStatistics([FromQuery] NewsStatisticsModel.BrowseModel.Request request)
        {
            var result = await _newsStatisticsService.GetNewsStatistics(request);
            return Ok(result.Data);
        }
    }
}
