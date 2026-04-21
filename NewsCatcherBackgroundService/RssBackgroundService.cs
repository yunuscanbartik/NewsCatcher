using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsCatcher.Domain.Models.Config;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using NLog;

namespace NewsCatcherBackgroundService
{
    public class RssBackgroundService : BackgroundService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly RssOptions _rssOptions;
        private readonly IGenericRssFeedService _rssFeedService;
        private readonly INewsService _newsService;

        public RssBackgroundService(
            IOptions<RssOptions> rssOptions,
            IGenericRssFeedService rssFeedService,
            INewsService newsService)
        {
            _rssOptions = rssOptions.Value;
            _rssFeedService = rssFeedService;
            _newsService = newsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var feedUrlList = _rssOptions.FeedUrls.Any() ? _rssOptions.FeedUrls : _rssOptions.FeedUrl;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (feedUrlList is null || !feedUrlList.Any())
                {
                    _logger.Error("RSS feed source list is empty.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    continue;
                }

                foreach (var feed in feedUrlList)
                {
                    var sourceName = feed.Key?.Trim();
                    var feedUrl = feed.Value?.Trim();
                    if (string.IsNullOrEmpty(sourceName) || string.IsNullOrEmpty(feedUrl))
                        continue;

                    try
                    {
                        var mapped = await _rssFeedService.FetchAsync(feedUrl, sourceName, stoppingToken);
                        await SaveOnlyNewNewsAsync(mapped, sourceName);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error while processing RSS source: {Source}", sourceName);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task SaveOnlyNewNewsAsync(List<NewsModel.CreateModel.ReturnData> feedNews, string sourceName)
        {
            if (feedNews is null || feedNews.Count == 0)
            {
                _logger.Info("{0} feed returned no news.", sourceName);
                return;
            }

            var allExistingNews = await _newsService.GetNews(new NewsModel.BrowseModel.Request());

            var existingLinks = (allExistingNews.Data ?? new List<NewsModel.BrowseModel.ReturnData>())
                .Where(x => !string.IsNullOrWhiteSpace(x.Link))
                .Select(x => x.Link!.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var existingGuids = (allExistingNews.Data ?? new List<NewsModel.BrowseModel.ReturnData>())
                .Where(x => !string.IsNullOrWhiteSpace(x.GuId))
                .Select(x => x.GuId!.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newNews = feedNews
                .Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Link) && !existingLinks.Contains(x.Link.Trim())) ||
                    (!string.IsNullOrWhiteSpace(x.GuId) && !existingGuids.Contains(x.GuId.Trim())))
                .GroupBy(x => $"{x.Link?.Trim()?.ToLowerInvariant()}|{x.GuId?.Trim()?.ToLowerInvariant()}")
                .Select(g => g.First())
                .ToList();

            if (newNews.Count == 0)
            {
                _logger.Info("{0} feed has no new news.", sourceName);
                return;
            }

            await _newsService.SaveToDatabase(newNews);
            _logger.Info("{0} new news saved for {1}.", newNews.Count, sourceName);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Background service is starting.");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Background service is stopping.");
            await base.StopAsync(cancellationToken);
        }
    }
}
