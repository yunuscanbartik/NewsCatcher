using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface IGenerateOtpService
    {
        Task<GenerateOtpModel.GenerateOtp.Return> GenerateOtpAsync(GenerateOtpModel.GenerateOtp.Request request);
        Task<GenerateOtpModel.GenerateOtp.Return> SendMailAsync(GenerateOtpModel.GenerateOtp.Request request);
    }
}
