using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyOtpController : ControllerBase
    {
        private readonly IVerifyOtpService _verifyOtpService;

        public VerifyOtpController(IVerifyOtpService verifyOtpService)
        {
            _verifyOtpService = verifyOtpService;
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] OtpModel.VerifyOtp.Request request)
        {
            var result = await _verifyOtpService.VerifyOtpAsync(request);
            return Ok(result);
        }
    }
}
