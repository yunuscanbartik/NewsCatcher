using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcherBackgroundService
{
    public class ConsumerService
    {
        private readonly IDatabaseContext _dbContext;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public ConsumerService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabaseAsync(List<NewsModel.CreateModel.ReturnData> returnDataList)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            try
            {
                foreach (var item in returnDataList)
                {

                    using (var sqlCommand = new SqlCommand("sp_News_Create", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Title", (object)item.Title?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Content", (object)item.Content?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Summary", (object)item.Summary?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@CategoryId", item.CategoryId.HasValue && item.CategoryId != 0 ? (object)item.CategoryId : DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@SourceName", (object)item.SourceName?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@ThumbnailUrl", (object)item.ThumbnailUrl?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@GuId", (object)item.GuId?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Link", (object)item.Link?.Trim() ?? DBNull.Value);
                        var newsIdParam = new SqlParameter("@NewsId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        sqlCommand.Parameters.Add(newsIdParam);
                        await sqlCommand.ExecuteNonQueryAsync();
                        item.NewsId = (int)newsIdParam.Value;
                    }

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
