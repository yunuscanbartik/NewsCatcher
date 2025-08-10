using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.RabbitMQ.Interfaces;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NewsCatcherBackgroundService
{
    public class CnnJobService : ICnnJobService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabaseContext _dbContext;
        private readonly IRabbitMqService _rabbitMqService;
        public CnnJobService(IDatabaseContext dbContext, IRabbitMqService rabbitMqService)
        {
            _dbContext = dbContext;
            _rabbitMqService = rabbitMqService;
        }
        public async Task<List<NewsModel.CNNModel.Item>> GetRssItemsAsync(string feedUrl)
        {
            try
            {
                using (var httpClient = new HttpClient()) // HTTP isteklerini yapabilmek için değişkene atıyorum.
                {
                    var response = await httpClient.GetAsync(feedUrl); //URL e get isteği göndererek haberleri alıyorum ve response değişkenine atıyorum.

                    var xmlContent = await response.Content.ReadAsStringAsync(); //response içeriğini string olarak okuyorum. 

                    var serializer = new XmlSerializer(typeof(NewsModel.CNNModel.Item)); // xml i parçalayarak nesneye dönüştürmek için XmlSerializer kullanıyorum.
                    using (var stringReader = new StringReader(xmlContent)) // xml içeriğini string olarak okuyabilmek için StringReader kullanıyorum.
                    {
                        var rss = (NewsModel.CNNModel.Rss)serializer.Deserialize(stringReader); 
                        return rss.Channel?.Item ?? new List<NewsModel.CNNModel.Item>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new List<NewsModel.CNNModel.Item>();
            }
        }

        public async Task<List<NewsModel.CreateModel.ReturnData>> MapToReturnDataAsync(List<NewsModel.CNNModel.Item> bbcItems)
        {
            var returnDataList = new List<NewsModel.CreateModel.ReturnData>();
            try
            {
                foreach (var item in bbcItems)
                {
                    var returnData = new NewsModel.CreateModel.ReturnData
                    {
                        NewsId = 0,
                        Title = item.Title,
                        Content = item.Description,
                        Summary = item.Description,
                        CategoryId = 0,
                        ShareDate = item.PubDate,
                        SourceName = "BBC",
                        CreatedDate = item.PubDate,
                        UpdatedDate = DateTime.Now,
                        Link = item.Link,
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

        public async Task SendToQueueAsync(List<NewsModel.CreateModel.ReturnData> returnDataList, string queueName)
        {
            try
            {
                foreach (var item in returnDataList)
                {
                    var message = JsonSerializer.Serialize(item);

                    _rabbitMqService.PublishMessage(message, queueName);
                }

                _logger.Info("Kuyruğa gönderilen haberler: ", returnDataList.Count);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Kuyruğa gönderirken hata oluştu.");
            }
        }
    }
}
