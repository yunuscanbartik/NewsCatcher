using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NewsTagController : ControllerBase
    {
        private readonly INewsTagService _newsTagService;

        public NewsTagController(INewsTagService newsTagService)
        {
            _newsTagService = newsTagService;
        }

        [HttpPost("AddNewsTag")]
        public async Task<IActionResult> AddNewsTag(NewsTagModel.CreateModel.Request request)
        {
            var result = await _newsTagService.AddNewsTag(request);
            return Ok(result.Data);
        }
    }
}
