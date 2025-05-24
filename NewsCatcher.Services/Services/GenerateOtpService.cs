using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Services
{
    public class GenerateOtpService : IGenerateOtpService
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IEmailService _emailService;
        public GenerateOtpService(IDatabaseContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }
        public async Task<OtpModel.GenerateOtp.Return> GenerateOtpAsync(OtpModel.GenerateOtp.Request request)
        {
            var otp = new List<OtpModel.GenerateOtp.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_GenerateOtpCode", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);

            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                    if (await reader.ReadAsync())
                    {
                        otp.Add(new OtpModel.GenerateOtp.ReturnData
                        {
                            Email = reader.GetString("Email"),
                            VerificationCode = reader.GetString("VerificationCode")
                        });
                        var VerificationCode = reader.GetString("VerificationCode");
                        bool emailSent = await _emailService.SendEmailAsync(
                            request.Email,
                            "Tek Kullanımlık Şifre",
                            $"Mehaba Tek kullanımlık Şifreniz: {VerificationCode}"
                        );
                        if (emailSent != true)
                        {
                            return new OtpModel.GenerateOtp.Return
                            {
                                Status = false,
                                Message = "OTP oluşturuldu ama e-posta gönderilemedi",
                                ErrorCode = null,
                                ErrorMessage = null,
                                RequestId = Guid.NewGuid().ToString(),
                                StatusCode = 200,
                                RequestTime = DateTime.UtcNow,
                                ResponseTime = DateTime.UtcNow,
                                Data = null
                            };
                        }
                        return new OtpModel.GenerateOtp.Return
                        {
                            Status = true,
                            Message = "OTP başarıyla oluşturuldu ve e-postayla gönderildi",
                            ErrorCode = null,
                            ErrorMessage = null,
                            RequestId = Guid.NewGuid().ToString(),
                            StatusCode = 200,
                            RequestTime = DateTime.UtcNow,
                            ResponseTime = DateTime.UtcNow,
                            Data = otp
                        };
                    }
                    else
                    {
                        return new OtpModel.GenerateOtp.Return
                        {
                            Status = false,
                            Message = "OTP kodu oluşturulamadı",
                            ErrorCode = null,
                            ErrorMessage = null,
                            RequestId = Guid.NewGuid().ToString(),
                            StatusCode = 200,
                            RequestTime = DateTime.UtcNow,
                            ResponseTime = DateTime.UtcNow,
                            Data = null
                        };
                    }
            }
            catch (Exception ex)
            {
                return new OtpModel.GenerateOtp.Return
                {
                    Status = false,
                    Message = "OTP oluşturulamadı veya e-posta gönderilemedi",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now
                };
            }
        }
    }
}
