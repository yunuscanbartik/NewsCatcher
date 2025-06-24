using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
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
    /// <summary>
    /// RssFeedService sınıfı, RSS beslemelerinden haberleri HTTP isteği ile alıyorum.
    /// </summary>
    public class RssFeedService : IRssFeedService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabaseContext _dbContext; 
        public RssFeedService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public async Task<List<NewsModel.BBCModel.Item>> GetRssItemsAsync(string feedUrl) //bir url alır ve içindeki haberleri list olarak döner.
        {
            try
            {
                using (var httpClient = new HttpClient()) // HTTP isteklerini yapabilmek için değişkene atıyorum.
                {
                    var response = await httpClient.GetAsync(feedUrl); //URL e get isteği göndererek haberleri alıyorum ve response değişkenine atıyorum.

                    var xmlContent = await response.Content.ReadAsStringAsync(); //response içeriğini string olarak okuyorum. 

                    var serializer = new XmlSerializer(typeof(NewsModel.BBCModel.Rss)); // xml i parçalayarak nesneye dönüştürmek için XmlSerializer kullanıyorum.
                    using (var stringReader = new StringReader(xmlContent)) // xml içeriğini string olarak okuyabilmek için StringReader kullanıyorum.
                    {
                        var rss = (NewsModel.BBCModel.Rss)serializer.Deserialize(stringReader); // xml içeriğini NewsModel.BBCModel.Rss tipine deserialize ediyorum.
                        var items = rss.Channel.Item; // rss içindeki channel ın item kısmını alıyorum. Bu kısım haberlerin bulunduğu kısım.
                        return items; // haberlerin listesini döndürüyorum.
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new List<NewsModel.BBCModel.Item>();
            }
        }

        /// <summary>
        /// Burada BBC RSS feedinden alınan haberleri, uygulamanın ihtiyaç duyduğu modele dönüştürüyorum.
        /// </summary>
        /// <param name="bbcItems"></param>
        /// <returns></returns>
        public async Task<List<NewsModel.BrowseModel.ReturnData>> MapToReturnDataAsync(List<NewsModel.BBCModel.Item> bbcItems)
        {
            var returnDataList = new List<NewsModel.BrowseModel.ReturnData>();
            try
            {
                foreach (var item in bbcItems)
                {
                    var returnData = new NewsModel.BrowseModel.ReturnData
                    {
                        NewsId = 0,
                        Title = item.Title,
                        Content = item.Description,
                        Summary = item.Description,
                        CategoryId = 0,
                        ShareDate = item.PubDate,
                        SourceName = "BBC",
                        CreatedDate = item.PubDate,
                        UpdatedDate = DateTime.Now
                    };
                    returnDataList.Add(returnData);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return returnDataList;
        }
    }
}
