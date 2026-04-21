using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Application.Services;
using NewsCatcher.Infrastructure.Data;
using NewsCatcher.Models.Models;
using NewsCatcher.Domain.Models.Config;
using NewsCatcherApi;
using NewsCatcherApi.Middleware;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<SmtpSettingsOptions>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<AppSettingsOptions>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IEmailService, SendEmailService>();
builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
builder.Services.AddSingleton<ITagsService, TagsService>();
builder.Services.AddSingleton<INewsTagService, NewsTagService>();
builder.Services.AddSingleton<INewsService, NewsService>();
builder.Services.AddSingleton<INewsStatisticsService, NewsStatisticsService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IUserFavoritiesService, UserFavoritiesService>();
builder.Services.AddSingleton<IUsersService, UsersService>();

var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettingsOptions>() ?? new AppSettingsOptions();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("auth_generate_otp", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 12,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            }));

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        var response = new ReturnModel
        {
            Status = false,
            Message = "Too many requests. Please try again later.",
            ErrorCode = "RATE_LIMIT_EXCEEDED",
            ErrorMessage = "Rate limit exceeded",
            RequestId = context.HttpContext.TraceIdentifier,
            StatusCode = StatusCodes.Status429TooManyRequests,
            RequestTime = DateTime.Now,
            ResponseTime = DateTime.Now
        };

        context.HttpContext.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(response);
        await context.HttpContext.Response.WriteAsync(json, token);
    };

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));
});

builder.Services.AddSwaggerGen(swagger =>
{
    var schemaHelper = new SwashbuckleSchemaHelper();
    swagger.CustomSchemaIds(type => schemaHelper.GetSchemaId(type));
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "News Catcher API", Version = "v1" });
    swagger.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Enter your API key (JWT) in the Authorization header."
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "apiKey"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
