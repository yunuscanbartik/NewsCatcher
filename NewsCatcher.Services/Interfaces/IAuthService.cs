using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel.VerifyOtp.Return> VerifyOtp(AuthModel.VerifyOtp.Request request);
        Task<AuthModel.VerifyOtp.Return> GenerateJwt(AuthModel.VerifyOtp.Request request);
    }
}
