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
    public class VerifyOtpService : IVerifyOtpService
    {
        private readonly IDatabaseContext _dbContext;
        public VerifyOtpService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OtpModel.VerifyOtp.Return> VerifyOtpAsync(OtpModel.VerifyOtp.Request request)
        {
            var verificationResult = new List<OtpModel.VerifyOtp.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_ValidateLoginCode", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);
            sqlCommand.Parameters.AddWithValue("@VerificationCode", request.VerificationCode);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                       verificationResult.Add(new OtpModel.VerifyOtp.ReturnData
                        {
                            Email = reader.GetString("Email"),
                            IsUsed = reader.GetBoolean("IsUsed")
                        });
                    }
                }
                return new OtpModel.VerifyOtp.Return
                {
                    Status = true,
                    Message = "OTP Doğrulandı",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = verificationResult
                };
            }
            catch
            { 
                return new OtpModel.VerifyOtp.Return
                {
                    Status = false,
                    Message = "OTP Doğrulama Başarısız",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = verificationResult
                };
            }
        }
    }
}
