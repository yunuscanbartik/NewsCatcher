using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class NewsStatisticsService : INewsStatisticsService
    {
        private const string StoredProcedureBrowse = "sp_NewsStatistics_Browse";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<NewsStatisticsService> _logger;

        public NewsStatisticsService(IDatabaseContext dbContext, ILogger<NewsStatisticsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<NewsStatisticsModel.BrowseModel.Return> GetNewsStatistics(NewsStatisticsModel.BrowseModel.Request request)
        {
            var newsStatistic = new List<NewsStatisticsModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureBrowse, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NewsId", (object?)request.NewsId ?? DBNull.Value);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
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
                }

                return new NewsStatisticsModel.BrowseModel.Return
                {
                    Data = newsStatistic
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to browse news statistics");
                throw;
            }
        }
    }
}
