using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;
using NewsCatcher.Services.Services;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateOtpController : ControllerBase
    {
        private readonly IGenerateOtpService _generateOtpService;
        private readonly IVerifyOtpService _verifyOtpService;
        public GenerateOtpController(IGenerateOtpService generateOtpService, IVerifyOtpService verifyOtpService)
        {
            _generateOtpService = generateOtpService;
            _verifyOtpService = verifyOtpService;
        }
        [HttpPost("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp(OtpModel.GenerateOtp.Request request)
        {
            var result = await _generateOtpService.GenerateOtpAsync(request);
            return Ok(result);
        }
        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] OtpModel.VerifyOtp.Request request)
        {
            var result = await _verifyOtpService.VerifyOtpAsync(request);
            return Ok(result);
        }
    }
}
