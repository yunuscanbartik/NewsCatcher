namespace NewsCatcher.Domain.Models.Config
{
    public class RssOptions
    {
        public Dictionary<string, string> FeedUrl { get; set; } = new();
        public Dictionary<string, string> FeedUrls { get; set; } = new();
    }
}



