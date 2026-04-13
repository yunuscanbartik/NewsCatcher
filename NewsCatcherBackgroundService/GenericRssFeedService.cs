using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;

namespace NewsCatcherBackgroundService
{
    public class GenericRssFeedService : IGenericRssFeedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GenericRssFeedService> _logger;

        public GenericRssFeedService(IHttpClientFactory httpClientFactory, ILogger<GenericRssFeedService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<NewsModel.CreateModel.ReturnData>> FetchAsync(string feedUrl, string sourceName, CancellationToken cancellationToken = default)
        {
            var list = new List<NewsModel.CreateModel.ReturnData>();
            if (string.IsNullOrWhiteSpace(feedUrl))
                return list;

            try
            {
                var client = _httpClientFactory.CreateClient(nameof(GenericRssFeedService));
                await using var stream = await client.GetStreamAsync(new Uri(feedUrl), cancellationToken);
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    Async = true
                };
                using var xmlReader = XmlReader.Create(stream, settings);
                var feed = SyndicationFeed.Load(xmlReader);

                foreach (var item in feed.Items ?? Array.Empty<SyndicationItem>())
                {
                    var link = GetItemLink(item);
                    var pub = item.PublishDate.LocalDateTime;
                    if (pub == default)
                        pub = DateTime.Now;

                    list.Add(new NewsModel.CreateModel.ReturnData
                    {
                        NewsId = 0,
                        Title = item.Title?.Text?.Trim(),
                        Content = item.Summary?.Text?.Trim(),
                        Summary = item.Summary?.Text?.Trim(),
                        CategoryId = 0,
                        ShareDate = pub,
                        SourceName = sourceName,
                        CreatedDate = pub,
                        UpdatedDate = DateTime.Now,
                        ThumbnailUrl = TryGetMediaThumbnail(item),
                        Link = link,
                        GuId = !string.IsNullOrWhiteSpace(item.Id) ? item.Id : link
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RSS okunamadı: {Source} {Url}", sourceName, feedUrl);
            }

            return list;
        }

        private static string? GetItemLink(SyndicationItem item)
        {
            var alternate = item.Links?.FirstOrDefault(l =>
                string.Equals(l.RelationshipType, "alternate", StringComparison.OrdinalIgnoreCase));
            if (alternate?.Uri != null)
                return alternate.Uri.ToString();
            return item.Links?.FirstOrDefault()?.Uri?.ToString();
        }

        private string? TryGetMediaThumbnail(SyndicationItem item)
        {
            if (item.ElementExtensions == null)
                return null;

            foreach (var ext in item.ElementExtensions)
            {
                if (!string.Equals(ext.OuterName, "thumbnail", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (ext.OuterNamespace?.IndexOf("search.yahoo.com", StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                try
                {
                    using var reader = ext.GetReader();
                    if (reader.MoveToElement() && reader.HasAttributes)
                    {
                        var url = reader.GetAttribute("url");
                        if (!string.IsNullOrWhiteSpace(url))
                            return url;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Skipping malformed media thumbnail extension");
                }
            }

            return null;
        }
    }
}
