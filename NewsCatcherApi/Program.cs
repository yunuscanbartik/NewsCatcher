using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using NewsCatcher.Services.Services;
using NewsCatcherApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
builder.Services.AddSingleton<INewsService, NewsService>();
builder.Services.AddSingleton<INewsStatisticsService, NewsStatisticsService>();
builder.Services.AddSingleton<INewsTagService, NewsTagService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<ITagsService, TagsService>();
builder.Services.AddSingleton<IUserFavoritiesService, UserFavoritiesService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSwaggerGen(swagger =>
{
    var schemaHelper = new SwashbuckleSchemaHelper();
    swagger.CustomSchemaIds(type => schemaHelper.GetSchemaId(type));
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
