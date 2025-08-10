using Microsoft.Extensions.Logging;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcherBackgroundService
{
    public class RssBackgroundService : BackgroundService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger(); // Log mesajları için Nlog kullanılıyor.
        private readonly IDatabaseContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IBbcJobService _bbcJobService; 
        private readonly ICnnJobService _cnnJobService;

        public RssBackgroundService(IDatabaseContext dbContext, IConfiguration configuration, IBbcJobService bbcJobService, ICnnJobService cnnJobService)
        {
            _dbContext = dbContext; 
            _configuration = configuration;
            _bbcJobService = bbcJobService;
            _cnnJobService = cnnJobService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var feedUrlList = _configuration
                .GetSection("Rss:FeedUrls")
                .Get<Dictionary<string, string>>(); // 2 tane string alan olma sebebi ismi ve karşısına linki olacak şekilde.
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var feed in feedUrlList) //TODO CONFIGDEN LISTEYI AL
                    {
                        var sourceName = feed.Key;
                        var feedUrl = feed.Value;

                        switch (sourceName.ToUpper())
                        {
                            case "BBC":
                                await _bbcJobService.GetRssItemsAsync(feedUrl);
                                break;

                            case "CNN":
                                await _cnnJobService.GetRssItemsAsync(feedUrl);
                                break;

                            default:
                                _logger.Error($"kaynak bulunamadı");
                                break;
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "RssBackgroundService çalışırken hata oluştu");
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Background Service başlıyor");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Background Service duruyor");
            await base.StopAsync(cancellationToken);
        }
    }
}
