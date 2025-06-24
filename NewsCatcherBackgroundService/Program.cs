using NewsCatcher.Services.Interfaces;
using NewsCatcher.Services.Services;
using NewsCatcherBackgroundService;

var builder = Host.CreateApplicationBuilder(args);

var host = builder.Build();
host.Run();
