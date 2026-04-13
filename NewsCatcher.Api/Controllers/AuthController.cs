using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp(AuthModel.GenerateOtp.Request request)
        {
            await _authService.GenerateOtpAsync(request);
            return Ok();
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateTokenAsync([FromBody] AuthModel.GenerateToken.Request request)
        {
            var result = await _authService.GenerateTokenAsync(request);
            var jwt = result.Data?.FirstOrDefault()?.JwtCode;
            return Ok(new { jwtCode = jwt });
        }
    }
}
