using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using NewsCatcher.Domain.Models.Config;
using NewsCatcher.Models.Models;
using NewsCatcher.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace NewsCatcher.Application.Services
{
    public class AuthService : IAuthService
    {
        private const int OtpValiditySeconds = 180;
        private readonly IDatabaseContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly AppSettingsOptions _appSettings;
        public AuthService(IDatabaseContext dbContext, IEmailService emailService, IOptions<AppSettingsOptions> appSettingsOptions)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _appSettings = appSettingsOptions.Value;
        }

        public async Task<AuthModel.GenerateOtp.Return> GenerateOtpAsync(AuthModel.GenerateOtp.Request request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_GenerateOtpCode", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);

            using (var reader = sqlCommand.ExecuteReader())
                if (reader.Read())
                {
                    var verificationCode = reader.GetString("VerificationCode");
                    bool emailSent = await _emailService.SendEmailAsync(
                        request.Email,
                            "NewsCatcher OTP Verification",
                        verificationCode
                    );
                    if (emailSent != true)
                    {
                        throw new InvalidOperationException("OTP generated but email could not be sent.");
                    }
                    return new AuthModel.GenerateOtp.Return
                    {
                        Status = true,
                        StatusCode = 200
                    };
                }
                else
                {
                    throw new InvalidOperationException("OTP could not be generated.");
                }
        }

        private Task<string> GenerateJwtTokenAsync(DateTime expireDateTime, AuthModel.GenerateToken.Request request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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

        public async Task<AuthModel.GenerateToken.Return> GenerateTokenAsync(AuthModel.GenerateToken.Request request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required.");
            }
            if (string.IsNullOrWhiteSpace(request.VerificationCode))
            {
                throw new ArgumentException("VerificationCode is required.");
            }

            var verificationResult = new List<AuthModel.GenerateToken.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_ValidateLoginCode", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);
            sqlCommand.Parameters.AddWithValue("@VerificationCode", request.VerificationCode);
            using (var reader = await sqlCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    var otpCreatedDate = GetOptionalDateTime(reader, "CreatedDate", "CreateDate", "GeneratedDate", "GeneratedAt");
                    if (otpCreatedDate.HasValue && DateTime.Now > otpCreatedDate.Value.AddSeconds(OtpValiditySeconds))
                    {
                        throw new UnauthorizedAccessException("OTP has expired.");
                    }

                    if (!reader.GetBoolean("IsUsed"))
                    {
                        throw new UnauthorizedAccessException("OTP validation failed.");
                    }

                    verificationResult.Add(new AuthModel.GenerateToken.ReturnData
                    {
                        JwtCode = await GenerateJwtTokenAsync(DateTime.Now.AddHours(3), request)
                    });
                }
            }
            return new AuthModel.GenerateToken.Return
            {
                Data = verificationResult
            };
        }

        private static DateTime? GetOptionalDateTime(SqlDataReader reader, params string[] columnNames)
        {
            var availableColumns = Enumerable.Range(0, reader.FieldCount)
                .Select(reader.GetName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var matchedColumn = columnNames.FirstOrDefault(availableColumns.Contains);
            if (string.IsNullOrWhiteSpace(matchedColumn))
            {
                return null;
            }

            var ordinal = reader.GetOrdinal(matchedColumn);
            return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
        }
    }
}
