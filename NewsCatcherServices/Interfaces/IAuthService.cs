namespace NewsCatcherApi.Interfaces
{
    public class IAuthService
    {
        Task SendVerificationCodeAsync(string Email);
        Task GenerateTokenAsync(string Email, string VerificationCode);
    }
}
