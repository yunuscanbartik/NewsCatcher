using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Application.Services;
using NewsCatcher.Domain.Models.Config;
using NewsCatcher.Infrastructure.Data;
using NewsCatcherBackgroundService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RssOptions>(builder.Configuration.GetSection("Rss"));
builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddHttpClient(nameof(GenericRssFeedService), client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("NewsCatcher-RssWorker/1.0");
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddSingleton<IGenericRssFeedService, GenericRssFeedService>();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<INewsService, NewsService>();
builder.Services.AddLogging(logging => logging.AddConsole());

builder.Services.AddHostedService<RssBackgroundService>();

var host = builder.Build();
host.Run();
