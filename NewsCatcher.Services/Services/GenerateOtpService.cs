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
        public GenerateOtpService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GenerateOtpModel.GenerateOtp.Return> GenerateOtpAsync(GenerateOtpModel.GenerateOtp.Request request)
        {
            var otp = new List<GenerateOtpModel.GenerateOtp.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_GenerateOtpCode", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);

            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        otp.Add(new GenerateOtpModel.GenerateOtp.ReturnData
                        {
                            Email = reader.GetString("Email"),
                            VerificationCode = reader.GetString("VerificationCode")
                        });
                    }
                return new GenerateOtpModel.GenerateOtp.Return
                {
                    Status = true,
                    Message = "OTP başarıyla oluşturuldu",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = otp
                };
            }
            catch (Exception ex)
            {
                return new GenerateOtpModel.GenerateOtp.Return
                {
                    Status = false,
                    Message = "Otp Kodu oluşturulamadı",
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
