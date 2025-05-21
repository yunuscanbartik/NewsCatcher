using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private ITagsService _tagsService;
        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }
        /// <summary>
        /// Tüm etiketleri döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetTags")]
        public async Task<IActionResult> GetTagsAsync([FromQuery] TagsModel.BrowseModel.Request request)
        {
            var result = await _tagsService.GetTagsAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Id ye göre etiket döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetTagsById")]
        public async Task<IActionResult> GetTagsByIdAsync([FromQuery] TagsModel.BrowseModel.Request request)
        {
            var result = await _tagsService.GetTagsByIdAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Yeni bir etiket ekler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddTag")]
        public async Task<IActionResult> AddTagAsync(TagsModel.CreateModel.Request request)
        {
            var result = await _tagsService.AddTagAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Var olan Etiketleri günceller.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateTag")]
        public async Task<IActionResult> UpdateTagAsync(TagsModel.UpdateModel.Request request)
        {
            var result = await _tagsService.UpdateTagAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Etiketi silmek için kullanılır.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTag")]
        public async Task<IActionResult> DeleteTagAsync(TagsModel.DeleteModel.Request request)
        {
            var result = await _tagsService.DeleteTagAsync(request);
            return Ok(result);
        }
    }
}
