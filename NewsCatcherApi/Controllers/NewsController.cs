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
        /// <summary>
        /// Haber listesini döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetNews")]
        public async Task<IActionResult> GetNewsAsync([FromQuery] NewsModel.BrowseModel.Request request)
        {
            var result = await _newsService.GetNewsAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Belli bir haber hakkında bilgi döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetNewsById")]
        public async Task<IActionResult> GetNewsByIdAsync([FromQuery] NewsModel.BrowseModel.Request request)
        {
            var result = await _newsService.GetNewsByIdAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Yeni bir haber ekler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddNews")]
        public async Task<IActionResult> AddNewsAsync(NewsModel.CreateModel.Request request)
        {
            var result = await _newsService.AddNewsAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Var olan bir haberi günceller.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateNews")]
        public async Task<IActionResult> UpdateNewsAsync(NewsModel.UpdateModel.Request request)
        {
            var result = await _newsService.UpdateNewsAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Belirlenen haberi siler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNews")]
        public async Task<IActionResult> DeleteNewsAsync(NewsModel.DeleteModel.Request request)
        {
            var result = await _newsService.DeleteNewsAsync(request);
            return Ok(result);
        }
    }
}
