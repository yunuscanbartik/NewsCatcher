using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;

namespace NewsCatcher.Services.Services
{
    public class TagsService : ITagsService
    {
        private readonly IDatabaseContext _dbContext;
        public TagsService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TagsModel.CreateModel.Return> AddTagAsync(TagsModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagName", request.TagName);

            await sqlCommand.ExecuteNonQueryAsync();
            return new TagsModel.CreateModel.Return
            {
                Status = true,
                Message = "Etiket Başarıyla Eklendi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<TagsModel.DeleteModel.Return> DeleteTagAsync(TagsModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagId", request.TagId);

            await sqlCommand.ExecuteNonQueryAsync();
            return new TagsModel.DeleteModel.Return
            {
                Status = true,
                Message = "Etiket Başarıyla Silindi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };

        }

        public async Task<TagsModel.BrowseModel.Return> GetTagsAsync(TagsModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_BrowseAll", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await sqlCommand.ExecuteReaderAsync();
            return new TagsModel.BrowseModel.Return
            {
                Status = true,
                Message = "Etiketler Başarıyla Alındı",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<TagsModel.DeleteModel.Return> GetTagsByIdAsync(TagsModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagId", request.TagId);
            await sqlCommand.ExecuteReaderAsync();
            return new TagsModel.DeleteModel.Return
            {
                Status = true,
                Message = "Etiket Başarıyla Alındı",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<TagsModel.UpdateModel.Return> UpdateTagAsync(TagsModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagId", request.TagId);
            sqlCommand.Parameters.AddWithValue("@TagName", request.TagName);

            await sqlCommand.ExecuteNonQueryAsync();
            return new TagsModel.UpdateModel.Return
            {
                Status = true,
                Message = "Etiket Başarıyla Güncellendi",
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
