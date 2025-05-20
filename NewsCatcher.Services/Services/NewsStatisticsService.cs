using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;

namespace NewsCatcher.Services.Services
{
    public class NewsStatisticsService : INewsStatisticsService
    {
        private readonly IDatabaseContext _dbContext;
        public NewsStatisticsService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Haber İstatistiklerini ID'ye göre alır.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NewsStatisticsModel.BrowseModel.Return> GetNewsStatisticsByIdAsync(NewsStatisticsModel.BrowseModel.Request request)
        {
            var newsStatistic = new List<NewsStatisticsModel.BrowseModel.ReturnData>(); ;
            var SqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_NewsStatistics_BrowseById", SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        newsStatistic.Add(new NewsStatisticsModel.BrowseModel.ReturnData
                        {
                            NewsStatisticId = reader.GetInt32("NewsStatisticId"),
                            NewsId = reader.GetInt32("NewsId"),
                            ViewCount = reader.GetInt32("ViewCount"),
                            ReadCount = reader.GetInt32("ReadCount"),
                            CreatedDate = reader.GetDateTime("CreatedDate"),
                            UpdatedDate = reader.GetDateTime("UpdatedDate")
                        });
                }
                return new NewsStatisticsModel.BrowseModel.Return
                {
                    Status = true,
                    Message = "Haber İstatistikleri Başarıyla Alındı",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = newsStatistic
                };
            }
            catch (Exception ex)
            {
                return new NewsStatisticsModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Haber İstatistikleri Alınamadı",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
    }
}
