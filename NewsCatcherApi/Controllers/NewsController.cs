using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        [HttpGet("GetNews")]
        public async Task<IActionResult> GetNewsAsync(NewsModel.BrowseModel.Request request)
        {
            var result = await _newsService.GetNewsAsync(request);
            return Ok(result);
        }
        [HttpPost("AddNews")]
        public async Task<IActionResult> AddNewsAsync(NewsModel.CreateModel.Request request)
        {
            var result = await _newsService.AddNewsAsync(request);
            return Ok(result);
        }
        [HttpPut("UpdateNews")]
        public async Task<IActionResult> UpdateNewsAsync(NewsModel.UpdateModel.Request request)
        {
            var result = await _newsService.UpdateNewsAsync(request);
            return Ok(result);
        }
        [HttpDelete("DeleteNews")]
        public async Task<IActionResult> DeleteGetNewsAsync(NewsModel.DeleteModel.Request request)
        {
            var result = await _newsService.DeleteGetNewsAsync(request);
            return Ok(result);
        }
        [HttpGet("GetNewsById")]
        public async Task<IActionResult> GetNewsByIdAsync(NewsModel.BrowseModel.Request request)
        {
            var result = await _newsService.GetNewsByIdAsync(request);
            return Ok(result);
        }

    }
}
