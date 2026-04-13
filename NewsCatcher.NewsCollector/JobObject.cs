using System.Text.Json.Serialization;

namespace NewsCatcher.NewsCollector
{
    public class JobObject
    {
        [JsonPropertyName("URL")]
        public string FeedUrl { get; set; } = string.Empty;
    }
}
