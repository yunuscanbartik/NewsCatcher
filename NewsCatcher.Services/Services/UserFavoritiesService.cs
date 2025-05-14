using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;

namespace NewsCatcher.Services.Services
{
    public class UserFavoritiesService : IUserFavoritiesService
    {
        private readonly IDatabaseContext _dbContext;
        public UserFavoritiesService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserFavoritiesModel.CreateModel.Return> AddUserFavoritiesAsync(UserFavoritiesModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_UserFavorities_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);

            await sqlCommand.ExecuteNonQueryAsync();
            return new UserFavoritiesModel.CreateModel.Return
            {
                Status = true,
                Message = "Favori Başarıyla Oluşturuldu.",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<UserFavoritiesModel.DeleteModel.Return> DeleteUserFavoritiesAsync(UserFavoritiesModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_UserFavorities_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserFavoritiesId", request.UserFavoritiesId);

            await sqlCommand.ExecuteNonQueryAsync();
            return new UserFavoritiesModel.DeleteModel.Return
            {
                Status = true,
                Message = "Favori Başarıyla Silindi.",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<UserFavoritiesModel.BrowseModel.Return> GetUserFavoritiesByIdAsync(UserFavoritiesModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_UserFavorities_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserFavoritiesId", request.UserFavoritiesId);

            await sqlCommand.ExecuteReaderAsync();
            return new UserFavoritiesModel.BrowseModel.Return
            {
                Status = true,
                Message = "Başarıyla Favoriye Alındı.",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<UserFavoritiesModel.UpdateModel.Return> UpdateUserFavoritiesAsync(UserFavoritiesModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_UserFavorities_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserFavoritiesId", request.UserFavoritiesId);
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);

            await sqlCommand.ExecuteNonQueryAsync();
            return new UserFavoritiesModel.UpdateModel.Return
            {
                Status = true,
                Message = "Favori Başarıyla Güncellendi.",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };

            //todo: update işlemi yapılacak db kısmında 
        }
    }
}
