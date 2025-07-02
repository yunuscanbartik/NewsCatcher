using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using NewsCatcher.Services.Services;
using NewsCatcherBackgroundService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddScoped<IRssFeedService, RssFeedService>();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();

builder.Services.AddHostedService<RssBackgroundService>();

var host = builder.Build();
host.Run();
