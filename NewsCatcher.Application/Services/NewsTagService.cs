using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class NewsTagService : INewsTagService
    {
        private const string StoredProcedureCreate = "sp_NewsTag_Create";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<NewsTagService> _logger;

        public NewsTagService(IDatabaseContext dbContext, ILogger<NewsTagService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<NewsTagModel.CreateModel.Return> AddNewsTag(NewsTagModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            sqlCommand.Parameters.AddWithValue("@TagId", request.TagId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new NewsTagModel.CreateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add news tag for news {NewsId}", request.NewsId);
                throw;
            }
        }
    }
}
