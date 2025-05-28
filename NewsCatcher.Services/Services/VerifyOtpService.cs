using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace NewsCatcher.Services.Services
{
    public class VerifyOtpService : IVerifyOtpService
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IConfiguration _configuration;
        public VerifyOtpService(IDatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public Task<string> GenerateJwtTokenAsync(DateTime expireDateTime, OtpModel.VerifyOtp.Request request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, request.Email ?? string.Empty)
                }),
                Expires = expireDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return Task.FromResult(tokenHandler.WriteToken(token));
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
                    if(await reader.ReadAsync())
                    {
                       verificationResult.Add(new OtpModel.VerifyOtp.ReturnData
                        {
                            Email = reader.GetString("Email"),
                            JwtToken = await GenerateJwtTokenAsync(DateTime.Now.AddMinutes(180), request),
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
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = verificationResult
                };
            }
            catch(SqlException ex)
            { 
                return new OtpModel.VerifyOtp.Return
                {
                    Status = false,
                    Message = ex.Message,
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = verificationResult
                };
            }
        }
    }
}
