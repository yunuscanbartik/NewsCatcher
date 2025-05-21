using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    /// <summary>
    /// Kategorilerle ilgili işlemleri gerçekleştiren API denetleyici sınıfı.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoriesService _categoriesService;
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        /// <summary>
        /// Kategori listesini döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategories([FromQuery] CategoriesModel.BrowseModel.Request request)
        {
            var result = await _categoriesService.GetCategoriesAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Belirli bir kategoriye ait bilgileri döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoriesById([FromQuery] CategoriesModel.BrowseModel.Request request)
        {
            var result = await _categoriesService.GetCategoryByIdAsync(request);
            return Ok(result);
        }/// <summary>
        /// Yeni bir kategori ekler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategoryAsync(CategoriesModel.CreateModel.Request request)
        {
            var result = await _categoriesService.AddCategoryAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Var olan bir kategoriyi günceller.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategoryAsync(CategoriesModel.UpdateModel.Request request)
        {
            var result = await _categoriesService.UpdateCategoryAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Belirli bir kategoriyi siler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategoryAsync(CategoriesModel.DeleteModel.Request request)
        {
            var result = await _categoriesService.DeleteCategoryAsync(request);
            return Ok(result);
        }
    }
}
