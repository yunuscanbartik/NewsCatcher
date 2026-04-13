using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("GetNews")]
        public async Task<IActionResult> GetNews([FromQuery] NewsModel.BrowseModel.Request request)
        {
            var result = await _newsService.GetNews(request);
            return Ok(result.Data);
        }

        [HttpPost("AddNews")]
        public async Task<IActionResult> AddNews(NewsModel.CreateModel.Request request)
        {
            var result = await _newsService.AddNews(request);
            return Ok(result.Data);
        }

        [HttpPut("UpdateNews")]
        public async Task<IActionResult> UpdateNews(NewsModel.UpdateModel.Request request)
        {
            var result = await _newsService.UpdateNews(request);
            return Ok(result.Data);
        }

        [HttpDelete("DeleteNews")]
        public async Task<IActionResult> DeleteNews(NewsModel.DeleteModel.Request request)
        {
            var result = await _newsService.DeleteNews(request);
            return Ok(result.Data);
        }
    }
}
