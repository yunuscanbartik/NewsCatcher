using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> GetCategories(CategoriesModel.BrowseModel.Request request)
        {
            var result = await _categoriesService.GetCategoriesAsync(request);
            return Ok(result);
        }
    }
}
