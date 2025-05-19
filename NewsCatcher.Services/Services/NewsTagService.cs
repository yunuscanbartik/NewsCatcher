using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;

namespace NewsCatcher.Services.Services
{
    public class NewsTagService : INewsTagService
    {
        private readonly IDatabaseContext _dbContext;
        public NewsTagService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<NewsTagModel.CreateModel.Return> AddNewsTagAsync(NewsTagModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_NewsTag_Create", sqlConnection)
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
                    Status = true,
                    Message = "Haber Etiketi Başarıyla Eklendi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new NewsTagModel.CreateModel.Return
                {
                    Status = false,
                    Message = "Haber Etiketi Eklenemdi.",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 404,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };

            }
        }
    }
}
