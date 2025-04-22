using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel.GenerateOtp.Return> GenerateOtpAsync(AuthModel.GenerateOtp.Request request);
        Task<AuthModel.GenerateToken.Return> GenerateTokenAsync(AuthModel.GenerateToken.Request request);
    }
}
