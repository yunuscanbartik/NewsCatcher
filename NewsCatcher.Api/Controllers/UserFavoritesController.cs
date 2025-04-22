using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserFavoritesController : ControllerBase
    {
        private readonly IUserFavoritiesService _userFavoritiesService;

        public UserFavoritesController(IUserFavoritiesService userFavoritiesService)
        {
            _userFavoritiesService = userFavoritiesService;
        }

        [HttpGet("GetUserFavorities")]
        public async Task<IActionResult> GetUserFavorities([FromQuery] UserFavoritiesModel.BrowseModel.Request request)
        {
            var result = await _userFavoritiesService.GetUserFavorities(request);
            return Ok(result.Data);
        }

        [HttpPost("AddUserFavorities")]
        public async Task<IActionResult> AddUserFavorities(UserFavoritiesModel.CreateModel.Request request)
        {
            var result = await _userFavoritiesService.AddUserFavorities(request);
            return Ok(result.Data);
        }

        [HttpPut("UpdateUserFavorities")]
        public async Task<IActionResult> UpdateUserFavorities(UserFavoritiesModel.UpdateModel.Request request)
        {
            var result = await _userFavoritiesService.UpdateUserFavorities(request);
            return Ok(result.Data);
        }

        [HttpDelete("DeleteUserFavorities")]
        public async Task<IActionResult> DeleteUserFavorities(UserFavoritiesModel.DeleteModel.Request request)
        {
            var result = await _userFavoritiesService.DeleteUserFavorities(request);
            return Ok(result.Data);
        }
    }
}
