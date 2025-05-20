using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsTagController : ControllerBase
    {
        private INewsTagService _newsTagService;
        public NewsTagController(INewsTagService newsTagService)
        {
            _newsTagService = newsTagService;
        }
        [HttpPost("AddNewsTag")]
        public async Task<IActionResult> AddNewsTagAsync(NewsTagModel.CreateModel.Request request)
        {
            var result = await _newsTagService.AddNewsTagAsync(request);
            return Ok(result);
        }
    }
}
