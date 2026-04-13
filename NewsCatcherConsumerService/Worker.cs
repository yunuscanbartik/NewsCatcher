using Microsoft.Extensions.Logging;
using NewsCatcher.Models.Models;
using NewsCatcher.Infrastructure.Interfaces;
using NewsCatcher.Infrastructure.Models;
using NewsCatcher.Domain.Interfaces;
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
            await _rabbitMqService.Consume(new RabbitMqModel.Consume.Request
            {
                QueueName = _queueName,
                MessageHandler = async (message) =>
                {
                    try
                    {
                        var newsList = JsonConvert.DeserializeObject<List<NewsModel.CreateModel.ReturnData>>(message);

                        if (newsList != null && newsList.Count > 0)
                        {
                            await _newsService.SaveToDatabase(newsList);
                            _logger.LogInformation("RabbitMQ'dan gelen {Count} haber veritaban�na kaydedildi.", newsList.Count);
                        }
                        else
                        {
                            _logger.LogWarning("RabbitMQ'dan veri al�n�rken sorun olustu");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "RabbitMQ mesaj� i�lenirken hata olu�tu.");
                    }
                }
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}



