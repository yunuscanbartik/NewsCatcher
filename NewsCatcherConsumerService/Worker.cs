using Microsoft.Extensions.Logging;
using NewsCatcher.Models.Models;
using NewsCatcher.RabbitMQ.Interfaces;
using NewsCatcher.Services.Interfaces;
using Newtonsoft.Json;

namespace NewsCatcherConsumerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly INewsService _newsService;
        private readonly string _queueName;

        public Worker(ILogger<Worker> logger, IRabbitMqService rabbitMqService, INewsService newsService, IConfiguration configuration)
        {
            _rabbitMqService = rabbitMqService;
            _newsService = newsService;
            _logger = logger;
            _queueName = "yunusqueue";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _rabbitMqService.Consume(_queueName, async (message) =>
                {
                    try
                    {
                        var newsList = JsonConvert.DeserializeObject<List<NewsModel.CreateModel.ReturnData>>(message);

                        if (newsList != null && newsList.Count > 0)
                        {
                            await _newsService.SaveToDatabaseAsync(newsList);
                            _logger.LogInformation("RabbitMQ'dan gelen {Count} haber veritabanýna kaydedildi.", newsList.Count);
                        }
                        else
                        {
                            _logger.LogWarning("RabbitMQ'dan veri alýnýrken sorun olustu");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "RabbitMQ mesajý iþlenirken hata oluþtu.");
                    }
                });
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
