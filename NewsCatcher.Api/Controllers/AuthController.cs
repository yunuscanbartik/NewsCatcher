using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NewsCatcher.Application.Exceptions;
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
        [EnableRateLimiting("auth_generate_otp")]
        public async Task<IActionResult> GenerateOtp(AuthModel.GenerateOtp.Request request)
        {
            try
            {
                var result = await _authService.GenerateOtpAsync(request);
                return Ok(new { remainingTime = result.RemainingTime, mailSent = result.MailSent });
            }
            catch (OtpEmailLimitExceededException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, new
                {
                    errorCode = "OTP_EMAIL_LIMIT",
                    message = ex.Message,
                });
            }
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateTokenAsync([FromBody] AuthModel.GenerateToken.Request request)
        {
            var result = await _authService.GenerateTokenAsync(request);
            var jwt = result.Data?.FirstOrDefault()?.JwtCode;
            // Same lifetime as JWT issued in AuthService (AddHours(3)).
            const int tokenLifetimeSeconds = 3 * 60 * 60;
            return Ok(new { jwtCode = jwt, expiresInSeconds = tokenLifetimeSeconds });
        }
    }
}
