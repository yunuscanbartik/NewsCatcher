using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class UsersService : IUsersService
    {
        private const string StoredProcedureBrowse = "sp_Users_Browse";
        private const string StoredProcedureCreate = "sp_Users_Create";
        private const string StoredProcedureUpdate = "sp_Users_Update";
        private const string StoredProcedureDelete = "sp_Users_Delete";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IDatabaseContext dbContext, ILogger<UsersService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<UsersModel.BrowseModel.Return> BrowseUsers(UsersModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureBrowse, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@UserId", (object?)request.UserId ?? DBNull.Value);
            var userData = new List<UsersModel.BrowseModel.ReturnData>();
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userData.Add(MapUserBrowseRow(reader));
                    }
                }

                return new UsersModel.BrowseModel.Return
                {
                    Data = userData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to browse users");
                throw;
            }
        }

        public async Task<UsersModel.CreateModel.Return> AddUser(UsersModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            var userData = new List<UsersModel.CreateModel.ReturnData>();
            sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);
            sqlCommand.Parameters.AddWithValue("@RoleId", request.RoleId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new UsersModel.CreateModel.Return
                {
                    Data = userData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user");
                throw;
            }
        }

        public async Task<UsersModel.UpdateModel.Return> UpdateUser(UsersModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureUpdate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            var userData = new List<UsersModel.UpdateModel.ReturnData>();
            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
            sqlCommand.Parameters.AddWithValue("@Email", request.Email);
            sqlCommand.Parameters.AddWithValue("@RoleId", request.RoleId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new UsersModel.UpdateModel.Return
                {
                    Data = userData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user {UserId}", request.UserId);
                throw;
            }
        }

        public async Task<UsersModel.DeleteModel.Return> DeleteUser(UsersModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureDelete, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new UsersModel.DeleteModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete user {UserId}", request.UserId);
                throw;
            }
        }

        private static UsersModel.BrowseModel.ReturnData MapUserBrowseRow(SqlDataReader reader)
        {
            var userIdOrdinal = reader.GetOrdinal("UserId");
            var userNameOrdinal = reader.GetOrdinal("UserName");
            var emailOrdinal = reader.GetOrdinal("Email");
            var roleIdOrdinal = reader.GetOrdinal("RoleId");
            var createdDateOrdinal = reader.GetOrdinal("CreatedDate");
            var updatedDateOrdinal = reader.GetOrdinal("UpdatedDate");

            return new UsersModel.BrowseModel.ReturnData
            {
                UserId = reader.IsDBNull(userIdOrdinal) ? null : reader.GetInt32(userIdOrdinal),
                UserName = reader.IsDBNull(userNameOrdinal) ? null : reader.GetString(userNameOrdinal),
                Email = reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal),
                RoleId = reader.IsDBNull(roleIdOrdinal) ? null : reader.GetInt32(roleIdOrdinal),
                CreatedDate = reader.IsDBNull(createdDateOrdinal) ? null : reader.GetDateTime(createdDateOrdinal),
                UpdatedDate = reader.IsDBNull(updatedDateOrdinal) ? null : reader.GetDateTime(updatedDateOrdinal)
            };
        }
    }
}
