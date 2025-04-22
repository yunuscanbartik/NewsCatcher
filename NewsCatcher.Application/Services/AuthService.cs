using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NewsCatcher.Application.Exceptions;
using NewsCatcher.Domain.Models.Config;
using NewsCatcher.Models.Models;
using NewsCatcher.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace NewsCatcher.Application.Services
{
    public class AuthService : IAuthService
    {
        private const int OtpValiditySeconds = 180;
        /// <summary>
        /// Minimum seconds between sending a new OTP email for the same address.
        /// </summary>
        private const int OtpResendCooldownSeconds = 60;
        private const int MaxOtpEmailsPer24Hours = 3;
        private static readonly TimeSpan OtpMailCountWindow = TimeSpan.FromHours(24);
        private static readonly TimeSpan OtpFlowCacheSliding = TimeSpan.FromHours(24);
        private const string OtpFlowCachePrefix = "otp_flow:";

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> EmailLocks =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly IDatabaseContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly AppSettingsOptions _appSettings;
        private readonly IMemoryCache _memoryCache;

        public AuthService(
            IDatabaseContext dbContext,
            IEmailService emailService,
            IOptions<AppSettingsOptions> appSettingsOptions,
            IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _appSettings = appSettingsOptions.Value;
            _memoryCache = memoryCache;
        }

        public async Task<AuthModel.GenerateOtp.Return> GenerateOtpAsync(AuthModel.GenerateOtp.Request request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            var email = request.Email.Trim();
            var emailKey = email.ToLowerInvariant();
            var gate = EmailLocks.GetOrAdd(emailKey, _ => new SemaphoreSlim(1, 1));
            await gate.WaitAsync();
            try
            {
                var cacheKey = OtpFlowCachePrefix + emailKey;
                if (!_memoryCache.TryGetValue(cacheKey, out OtpFlowState? flow) || flow is null)
                {
                    flow = new OtpFlowState();
                }

                TrimMailHistory(flow);
                if (flow.MailSentAtUtc.Count >= MaxOtpEmailsPer24Hours)
                {
                    throw new OtpEmailLimitExceededException();
                }

                if (flow.LastOtpCreationUtc.HasValue)
                {
                    var sinceLast = (DateTime.UtcNow - flow.LastOtpCreationUtc.Value).TotalSeconds;
                    if (sinceLast < OtpResendCooldownSeconds)
                    {
                        var remainingOnly = RemainingSecondsFromUtc(flow.LastOtpCreationUtc.Value);
                        _memoryCache.Set(cacheKey, flow, CreateFlowCacheOptions());
                        return new AuthModel.GenerateOtp.Return
                        {
                            Status = true,
                            StatusCode = 200,
                            RemainingTime = remainingOnly,
                            MailSent = false,
                        };
                    }
                }

                var sqlConnection = _dbContext.DatabaseConnection();
                var sqlCommand = new SqlCommand("sp_GenerateOtpCode", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Email", email);

                await using var reader = await sqlCommand.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                {
                    throw new InvalidOperationException("OTP could not be generated.");
                }

                var verificationOrdinal = reader.GetOrdinal("VerificationCode");
                var verificationCode = reader.GetString(verificationOrdinal);
                var otpCreated = GetOptionalDateTime(reader, "CreatedDate", "CreateDate", "GeneratedDate", "GeneratedAt", "OtpCreatedDate");
                var anchorUtc = ToUtcAnchor(otpCreated);

                var emailSent = await _emailService.SendEmailAsync(
                    email,
                    "NewsCatcher OTP Verification",
                    verificationCode
                );
                if (emailSent != true)
                {
                    throw new InvalidOperationException("OTP generated but email could not be sent.");
                }

                flow.MailSentAtUtc.Add(DateTime.UtcNow);
                TrimMailHistory(flow);
                flow.LastOtpCreationUtc = anchorUtc;
                _memoryCache.Set(cacheKey, flow, CreateFlowCacheOptions());

                var remainingTime = RemainingSecondsFromUtc(anchorUtc);
                return new AuthModel.GenerateOtp.Return
                {
                    Status = true,
                    StatusCode = 200,
                    RemainingTime = remainingTime,
                    MailSent = true,
                };
            }
            finally
            {
                gate.Release();
            }
        }

        private static MemoryCacheEntryOptions CreateFlowCacheOptions() =>
            new() { SlidingExpiration = OtpFlowCacheSliding };

        private static void TrimMailHistory(OtpFlowState flow)
        {
            var cutoff = DateTime.UtcNow - OtpMailCountWindow;
            flow.MailSentAtUtc.RemoveAll(t => t < cutoff);
        }

        private static DateTime ToUtcAnchor(DateTime? otpCreated)
        {
            if (!otpCreated.HasValue)
            {
                return DateTime.UtcNow;
            }

            var dt = otpCreated.Value;
            if (dt.Kind == DateTimeKind.Utc)
            {
                return dt;
            }

            if (dt.Kind == DateTimeKind.Local)
            {
                return dt.ToUniversalTime();
            }

            return DateTime.SpecifyKind(dt, DateTimeKind.Local).ToUniversalTime();
        }

        private static int RemainingSecondsFromUtc(DateTime createdUtc)
        {
            var elapsed = (int)Math.Floor((DateTime.UtcNow - createdUtc).TotalSeconds);
            var remaining = OtpValiditySeconds - elapsed;
            return Math.Clamp(remaining, 0, OtpValiditySeconds);
        }

        private sealed class OtpFlowState
        {
            public DateTime? LastOtpCreationUtc { get; set; }
            public List<DateTime> MailSentAtUtc { get; set; } = new();
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
