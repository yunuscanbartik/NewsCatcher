using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class UserFavoritiesService : IUserFavoritiesService
    {
        private const string StoredProcedureBrowse = "sp_UserFavorities_Browse";
        private const string StoredProcedureCreate = "sp_UserFavorities_Create";
        private const string StoredProcedureUpdate = "sp_UserFavorities_Update";
        private const string StoredProcedureDelete = "sp_UserFavorities_Delete";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<UserFavoritiesService> _logger;

        public UserFavoritiesService(IDatabaseContext dbContext, ILogger<UserFavoritiesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<UserFavoritiesModel.BrowseModel.Return> GetUserFavorities(UserFavoritiesModel.BrowseModel.Request request)
        {
            var favorities = new List<UserFavoritiesModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureBrowse, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", (object?)request.UserId ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@UserFavoritesId", (object?)request.UserFavoritiesId ?? DBNull.Value);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        favorities.Add(new UserFavoritiesModel.BrowseModel.ReturnData
                        {
                            UserFavoritiesId = reader.GetInt32("UserFavoritesId"),
                            UserId = reader.GetInt32("UserId"),
                            NewsId = reader.GetInt32("NewsId"),
                            CreatedAt = reader.GetDateTime("CreatedAt")
                        });
                    }
                }

                return new UserFavoritiesModel.BrowseModel.Return
                {
                    Data = favorities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to browse user favorites");
                throw;
            }
        }

        public async Task<UserFavoritiesModel.CreateModel.Return> AddUserFavorities(UserFavoritiesModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new UserFavoritiesModel.CreateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add user favorite for user {UserId}", request.UserId);
                throw;
            }
        }

        public async Task<UserFavoritiesModel.UpdateModel.Return> UpdateUserFavorities(UserFavoritiesModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureUpdate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new UserFavoritiesModel.UpdateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user favorite for user {UserId}", request.UserId);
                throw;
            }
        }

        public async Task<UserFavoritiesModel.DeleteModel.Return> DeleteUserFavorities(UserFavoritiesModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureDelete, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserFavoritesId", request.UserFavoritiesId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new UserFavoritiesModel.DeleteModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete user favorite {FavoriteId}", request.UserFavoritiesId);
                throw;
            }
        }
    }
}
