using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("BrowseUsers")]
        public async Task<IActionResult> BrowseUsers([FromQuery] UsersModel.BrowseModel.Request request)
        {
            var result = await _usersService.BrowseUsers(request);
            return Ok(result.Data);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UsersModel.CreateModel.Request request)
        {
            var result = await _usersService.AddUser(request);
            return Ok(result.Data);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UsersModel.UpdateModel.Request request)
        {
            var result = await _usersService.UpdateUser(request);
            return Ok(result.Data);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] UsersModel.DeleteModel.Request request)
        {
            var result = await _usersService.DeleteUser(request);
            return Ok(result.Data);
        }
    }
}
