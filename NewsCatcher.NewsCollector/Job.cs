using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using NewsCatcher.Models.Models;

namespace NewsCatcher.NewsCollector
{
    public class Job : ICustomJob
    {
        private readonly ILogger<Job> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Job(ILogger<Job> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> Execute(JobObject jobObject)
        {
            if (string.IsNullOrWhiteSpace(jobObject.FeedUrl))
            {
                _logger.LogWarning("RSS job skipped: feed URL is empty");
                return false;
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient(nameof(Job));
                using var response = await httpClient.GetAsync(jobObject.FeedUrl);
                response.EnsureSuccessStatusCode();
                var xmlContent = await response.Content.ReadAsStringAsync();
                var serializer = new XmlSerializer(typeof(NewsModel.BBCModel.Rss));
                using var stringReader = new StringReader(xmlContent);
                var rss = (NewsModel.BBCModel.Rss?)serializer.Deserialize(stringReader);
                var itemCount = rss?.Channel?.Item?.Count ?? 0;
                _logger.LogInformation("Fetched RSS feed; item count: {ItemCount}", itemCount);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute RSS job for feed");
                return false;
            }
        }
    }
}
