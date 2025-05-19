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
        [HttpGet("GetTags")]
        public async Task<IActionResult> GetTagsAsync([FromQuery] TagsModel.BrowseModel.Request request)
        {
            var result = await _tagsService.GetTagsAsync(request);
            return Ok(result);
        }
        [HttpGet("GetTagsById")]
        public async Task<IActionResult> GetTagsByIdAsync([FromQuery] TagsModel.DeleteModel.Request request)
        {
            var result = await _tagsService.GetTagsByIdAsync(request);
            return Ok(result);
        }
        [HttpPost("AddTag")]
        public async Task<IActionResult> AddTagAsync(TagsModel.CreateModel.Request request)
        {
            var result = await _tagsService.AddTagAsync(request);
            return Ok(result);
        }
        [HttpPut("UpdateTag")]
        public async Task<IActionResult> UpdateTagAsync(TagsModel.UpdateModel.Request request)
        {
            var result = await _tagsService.UpdateTagAsync(request);
            return Ok(result);
        }
        [HttpDelete("DeleteTag")]
        public async Task<IActionResult> DeleteTagAsync(TagsModel.DeleteModel.Request request)
        {
            var result = await _tagsService.DeleteTagAsync(request);
            return Ok(result);
        }
    }
}
