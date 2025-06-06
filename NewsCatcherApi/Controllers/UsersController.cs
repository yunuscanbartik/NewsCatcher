using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet("BrowseUsers")]
        public async Task<IActionResult> BrowseUsersAsync([FromQuery] UsersModel.BrowseModel.Request request)
        {
            var result = await _usersService.BrowseUsersAsync(request);
            return Ok(result);
        }
        [HttpGet("BrowseUserById")]
        public async Task<IActionResult> BrowseUsersByIdAsync([FromQuery] UsersModel.BrowseByIdModel.Request request)
        {
            var result = await _usersService.BrowseUsersByIdAsync(request);
            return Ok(result);
        }
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] UsersModel.CreateModel.Request request)
        {
            var result = await _usersService.AddUserAsync(request);
            return Ok(result);
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UsersModel.UpdateModel.Request request)
        {
            var result = await _usersService.UpdateUserAsync(request);
            return Ok(result);
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] UsersModel.DeleteModel.Request request)
        {
            var result = await _usersService.DeleteUserAsync(request);
            return Ok(result);
        }
    }
}
