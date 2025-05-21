using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateOtpController : ControllerBase
    {
        private readonly IGenerateOtpService _generateOtpService;
        public GenerateOtpController(IGenerateOtpService generateOtpService)
        {
            _generateOtpService = generateOtpService;
        }
        [HttpPost("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp(GenerateOtpModel.GenerateOtp.Request request)
        {
            var result = await _generateOtpService.GenerateOtpAsync(request);
            return Ok(result);
        }
    }
}
