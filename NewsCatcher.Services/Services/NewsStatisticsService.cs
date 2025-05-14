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
        public async Task<NewsStatisticsModel.BrowseModel.Return> GetNewsStatisticsByIdAsync(NewsStatisticsModel.BrowseModel.Request request)
        {
            var SqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_NewsStatistics_BrowseById", SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsStatisticsId);

            await sqlCommand.ExecuteReaderAsync();

            return new NewsStatisticsModel.BrowseModel.Return
            {
                Status = true,
                Message = "Haber İstatistikleri Başarıyla Alındı",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }
    }
}
