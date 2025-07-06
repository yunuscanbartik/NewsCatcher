using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static NewsCatcher.Models.Models.NewsModel.BBCModel;

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
                        var rss = (NewsModel.BBCModel.Rss)serializer.Deserialize(stringReader);
                        return rss.Channel?.Item;
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
        public async Task<List<NewsModel.CreateModel.ReturnData>> MapToReturnDataAsync(List<NewsModel.BBCModel.Item> bbcItems)
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

        public async Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabaseAsync(List<NewsModel.CreateModel.ReturnData> returnDataList)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                foreach (var item in returnDataList)
                {
                    sqlCommand.Parameters.AddWithValue("@Title", (object)item.Title?.Trim() ?? DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@Content", (object)item.Content?.Trim() ?? DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@Summary", (object)item.Summary?.Trim() ?? DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@CategoryId", item.CategoryId ?? 1); 
                    sqlCommand.Parameters.AddWithValue("@SourceName", (object)item.SourceName?.Trim() ?? DBNull.Value);
                    var newsIdParam = new SqlParameter("@NewsId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    sqlCommand.Parameters.Add(newsIdParam);
                    await sqlCommand.ExecuteNonQueryAsync();

                    item.NewsId = (int)newsIdParam.Value;
                }
                _logger.Info("Veritabanına kaydedilen haber sayısı: {Count}", returnDataList.Count);
                return returnDataList;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Veritabanına kaydetme işlemi sırasında hata oluştu.");
                return returnDataList;
            }
        }
    }
}
