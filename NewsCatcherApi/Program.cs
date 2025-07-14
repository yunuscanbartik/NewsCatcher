using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using NewsCatcher.Services.Services;
using NewsCatcherApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<IVerifyOtpService, VerifyOtpService>();
builder.Services.AddSingleton<IGenerateOtpService, GenerateOtpService>();
builder.Services.AddSingleton<IEmailService, SendEmailService>();
builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
builder.Services.AddSingleton<ITagsService, TagsService>();
builder.Services.AddSingleton<INewsTagService, NewsTagService>();
builder.Services.AddSingleton<INewsService, NewsService>();
builder.Services.AddSingleton<INewsStatisticsService, NewsStatisticsService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IUserFavoritiesService, UserFavoritiesService>();
builder.Services.AddSingleton<IUsersService, UsersService>();


builder.Services.AddSwaggerGen(swagger =>
{
    var schemaHelper = new SwashbuckleSchemaHelper();
    swagger.CustomSchemaIds(type => schemaHelper.GetSchemaId(type));
});
var key = Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:Secret"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.Events = new JwtBearerEvents
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
    x.TokenValidationParameters = new TokenValidationParameters
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "News Catcher API", Version = "v1" });
    c.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "API anahtarýnýzý buraya girin."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
