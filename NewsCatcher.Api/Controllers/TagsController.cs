using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpGet("GetTags")]
        public async Task<IActionResult> GetTags([FromQuery] TagsModel.BrowseModel.Request request)
        {
            var result = await _tagsService.GetTags(request);
            return Ok(result.Data);
        }

        [HttpPost("AddTag")]
        public async Task<IActionResult> AddTag(TagsModel.CreateModel.Request request)
        {
            var result = await _tagsService.AddTag(request);
            return Ok(result.Data);
        }

        [HttpPut("UpdateTag")]
        public async Task<IActionResult> UpdateTag(TagsModel.UpdateModel.Request request)
        {
            var result = await _tagsService.UpdateTag(request);
            return Ok(result.Data);
        }

        [HttpDelete("DeleteTag")]
        public async Task<IActionResult> DeleteTag(TagsModel.DeleteModel.Request request)
        {
            var result = await _tagsService.DeleteTag(request);
            return Ok(result.Data);
        }
    }
}
