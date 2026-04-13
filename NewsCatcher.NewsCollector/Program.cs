using NewsCatcher.NewsCollector;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient(nameof(Job));
builder.Services.AddSingleton<ICustomJob, Job>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
