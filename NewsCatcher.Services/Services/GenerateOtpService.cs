using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_GenerateOtp", sqlConnection);   
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new GenerateOtpModel.GenerateOtp.Return
                {
                    Status = true,
                    Message = "Otp Kodu oluşturuldu",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
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
        public Task<GenerateOtpModel.GenerateOtp.Return> SendMailAsync(GenerateOtpModel.GenerateOtp.Request request)
        {
            throw new NotImplementedException();
        }
    }  
}
