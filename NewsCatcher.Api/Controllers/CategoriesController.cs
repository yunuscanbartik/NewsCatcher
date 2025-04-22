using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategories([FromQuery] CategoriesModel.BrowseModel.Request request)
        {
            var result = await _categoriesService.GetCategories(request);
            return Ok(result.Data);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoriesModel.CreateModel.Request request)
        {
            var result = await _categoriesService.AddCategory(request);
            return Ok(result.Data);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoriesModel.UpdateModel.Request request)
        {
            var result = await _categoriesService.UpdateCategory(request);
            return Ok(result.Data);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(CategoriesModel.DeleteModel.Request request)
        {
            var result = await _categoriesService.DeleteCategory(request);
            return Ok(result.Data);
        }
    }
}
