using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoritieseController : ControllerBase
    {
        private readonly IUserFavoritiesService _userFavoritiesService;
        public UserFavoritieseController(IUserFavoritiesService userFavoritiesService)
        {
            _userFavoritiesService = userFavoritiesService;
        }
        /// <summary>
        /// Kullanıcının Favori haber listesini döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetUserFavoritiesById")]
        public async Task<IActionResult> GetUserFavoritiesByIdAsync([FromQuery] UserFavoritiesModel.BrowseModel.Request request)
        {
            var result = await _userFavoritiesService.GetUserFavoritiesByIdAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Kullanıcı haber favori listesine ekler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddUserFavorities")]
        public async Task<IActionResult> AddUserFavoritiesAsync(UserFavoritiesModel.CreateModel.Request request)
        {
            var result = await _userFavoritiesService.AddUserFavoritiesAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Favorisindeki haberleri günceller.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateUserFavorities")]
        public async Task<IActionResult> UpdateUserFavoritiesAsync(UserFavoritiesModel.UpdateModel.Request request)
        {
            var result = await _userFavoritiesService.UpdateUserFavoritiesAsync(request);
            return Ok(result);
        }
        /// <summary>
        /// Haberi favori listesinden siler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUserFavorities")]
        public async Task<IActionResult> DeleteUserFavoritiesAsync(UserFavoritiesModel.DeleteModel.Request request)
        {
            var result = await _userFavoritiesService.DeleteUserFavoritiesAsync(request);
            return Ok(result);
        }
    }
}
