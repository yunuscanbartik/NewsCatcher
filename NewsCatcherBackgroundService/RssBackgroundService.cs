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
        private readonly IRssFeedService _rssFeedService; // Bu servisi kullanarak RSS beslemelerini alacağız.
        public RssBackgroundService(IDatabaseContext dbContext, IConfiguration configuration, IRssFeedService rssFeedService)
        {
            _dbContext = dbContext; 
            _configuration = configuration;
            _rssFeedService = rssFeedService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var feedUrl = _configuration.GetValue<string>("Rss:FeedUrl:BBC");
            try
            {
                _logger.Info("RssBackgroundService RSS verilerini çekiyor...");

                var rssItems = await _rssFeedService.GetRssItemsAsync(feedUrl);
                var mappedData = await _rssFeedService.MapToReturnDataAsync(rssItems);

                var savedData = await _rssFeedService.SaveToDatabaseAsync(mappedData);
                _logger.Info($"{savedData.Count} haber başarıyla veritabanına kaydedildi.");

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "RssBackgroundService çalışırken hata oluştu");
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                
            }
            throw new NotImplementedException("RssBackgroundService henüz uygulanmadı. Lütfen uygulamayı tamamlayın.");
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
