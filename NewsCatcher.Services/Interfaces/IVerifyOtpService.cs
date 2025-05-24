using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface IVerifyOtpService
    {
        Task<OtpModel.VerifyOtp.Return> VerifyOtpAsync(OtpModel.VerifyOtp.Request request);
    }
}
