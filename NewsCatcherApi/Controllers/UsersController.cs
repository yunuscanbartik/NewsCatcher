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
    }
}
