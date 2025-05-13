using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Services
{
    public class NewsService : INewsService      
    {
        private readonly DatabaseContext _dbContext;
        public NewsService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<NewsModel.CreateModel.Return> AddNewsAsync(NewsModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Title", request.Title);
            sqlCommand.Parameters.AddWithValue("@Content", request.Content);
            sqlCommand.Parameters.AddWithValue("@Summary", request.Summary);
            sqlCommand.Parameters.AddWithValue("@CategoryId", request.CategoryId);
            sqlCommand.Parameters.AddWithValue("@SourceName", request.SourceName);

            await sqlCommand.ExecuteNonQueryAsync();
            return new NewsModel.CreateModel.Return
            {
                Status = true,
                Message = "Haber Başarıyla Eklendi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(), 
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<NewsModel.DeleteModel.Return> DeleteGetNewsAsync(NewsModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            await sqlCommand.ExecuteNonQueryAsync();
            return new NewsModel.DeleteModel.Return
            {
                Status = true,
                Message = "Haber Başarıyla Silindi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(), 
                StatusCode = 200, 
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<NewsModel.BrowseModel.Return> GetNewsAsync(NewsModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_BrowseAll", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await sqlCommand.ExecuteReaderAsync();

            return new NewsModel.BrowseModel.Return
            {
                Status = true,
                Message = "Haber Başarıyla Alındı",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(), 
                StatusCode = 200, 
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
                       
        }

        public async Task<NewsModel.BrowseModel.Return> GetNewsByIdAsync(NewsModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);

            await sqlCommand.ExecuteReaderAsync();

            return new NewsModel.BrowseModel.Return
            {
                Status = true,
                Message = "Haber Başarıyla Alındı",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };

        }

        public async Task<NewsModel.UpdateModel.Return> UpdateNewsAsync(NewsModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);

            await sqlCommand.ExecuteNonQueryAsync();
            return new NewsModel.UpdateModel.Return
            {
                Status = true,
                Message = "Haber Başarıyla Güncellendi",
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
