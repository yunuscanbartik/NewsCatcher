using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NewsCatcherBackgroundService
{
    public class RssFeedService : IRssFeedService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public async Task<List<NewsModel.BBCModel.Item>> FetchRssItemsAsync(string feedUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var response = await httpClient.GetAsync(feedUrl);

                    var xmlContent = await response.Content.ReadAsStringAsync();

                    var serializer = new XmlSerializer(typeof(NewsModel.BBCModel.Rss));
                    using (var stringReader = new StringReader(xmlContent))
                    {
                        var rss = (NewsModel.BBCModel.Rss)serializer.Deserialize(stringReader);
                        var items = rss.Channel.Item;
                        return items;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new List<NewsModel.BBCModel.Item>();
            }
        }
    }
}
