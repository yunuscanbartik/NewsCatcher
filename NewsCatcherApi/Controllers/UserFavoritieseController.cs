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
        [HttpGet("GetUserFavoritiesById")]
        public async Task<IActionResult> GetUserFavoritiesByIdAsync(UserFavoritiesModel.BrowseModel.Request request)
        {
            var result = await _userFavoritiesService.GetUserFavoritiesByIdAsync(request);
            return Ok(result);
        }
        [HttpPost("AddUserFavorities")]
        public async Task<IActionResult> AddUserFavoritiesAsync(UserFavoritiesModel.CreateModel.Request request)
        {
            var result = await _userFavoritiesService.AddUserFavoritiesAsync(request);
            return Ok(result);
        }
        [HttpPut("UpdateUserFavorities")]
        public async Task<IActionResult> UpdateUserFavoritiesAsync(UserFavoritiesModel.UpdateModel.Request request)
        {
            var result = await _userFavoritiesService.UpdateUserFavoritiesAsync(request);
            return Ok(result);
        }
        [HttpDelete("DeleteUserFavorities")]
        public async Task<IActionResult> DeleteUserFavoritiesAsync(UserFavoritiesModel.DeleteModel.Request request)
        {
            var result = await _userFavoritiesService.DeleteUserFavoritiesAsync(request);
            return Ok(result);
        }
    }
}
