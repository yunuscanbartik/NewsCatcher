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
        Task<OtpModel.GenerateOtp.Return> GenerateOtpAsync(OtpModel.GenerateOtp.Request request);
    }
}
