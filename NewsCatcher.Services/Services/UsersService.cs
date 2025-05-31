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
    public class UsersService : IUsersService
    {
        private readonly IDatabaseContext _dbContext;
        public UsersService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<UsersModel.CreateModel.Return> AddUserAsync(UsersModel.CreateModel.Request request)
        {
            throw new NotImplementedException();
        }

        public async Task<UsersModel.BrowseModel.Return> BrowseUsersAsync(UsersModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Users_BrowseAll", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            var userData = new List<UsersModel.BrowseModel.ReturnData>();
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userData.Add(new UsersModel.BrowseModel.ReturnData
                        {
                            UserId = reader.GetInt32("UserId"),
                            UserName = reader.GetString("UserName"),
                            Email = reader.GetString("Email"),
                            RoleId = reader.GetInt32("RoleId"),
                            CreatedDate = reader.GetDateTime("CreatedDate"),
                            UpdatedDate = reader.GetDateTime("UpdatedDate")
                        });
                    }
                } 
                return new UsersModel.BrowseModel.Return
                {
                    Status = true,
                    Message = "Kullanıcı Başarıyla Getirildi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = userData

                };
            }
            catch (SqlException ex) 
            {
                return new UsersModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Kullanıcı Getirilirken Hata Oluştu",
                    ErrorCode = ex.Number.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = null
                };
            }
        }
    
        public async Task<UsersModel.BrowseByIdModel.Return> BrowseUsersByIdAsync(UsersModel.BrowseByIdModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Users_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            var userData = new List<UsersModel.BrowseByIdModel.ReturnData>();
            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userData.Add(new UsersModel.BrowseByIdModel.ReturnData
                        {
                            UserId = reader.GetInt32("UserId"),
                            UserName = reader.GetString("UserName"),
                            Email = reader.GetString("Email"),
                            RoleId = reader.GetInt32("RoleId"),
                            CreatedDate = reader.GetDateTime("CreatedDate"),
                            UpdatedDate = reader.GetDateTime("UpdatedDate")
                        });
                    }
                }
                return new UsersModel.BrowseByIdModel.Return
                {
                    Status = true,
                    Message = "Kullanıcı Başarıyla Getirildi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = userData
                };
            }
            catch
            {
                return new UsersModel.BrowseByIdModel.Return
                {
                    Status = false,
                    Message = "Kullanıcı Getirilirken Hata Oluştu",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = null
                };
            }
        }

        public async Task<UsersModel.DeleteModel.Return> DeleteUserAsync(UsersModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Users_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new UsersModel.DeleteModel.Return
                {
                    Status = true,
                    Message = "Kullanıcı Başarıyla Silindi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now
                };
            }
            catch (SqlException ex)
            {
                return new UsersModel.DeleteModel.Return
                {
                    Status = false,
                    Message = "Kullanıcı Silinirken Hata Oluştu",
                    ErrorCode = ex.Number.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now
                };
            }
        }

        public async Task<UsersModel.UpdateModel.Return> UpdateUserAsync(UsersModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Users_Update", sqlConnection)
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
                    Status = true,
                    Message = "Kullanıcı Başarıyla Güncellendi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = userData

               };
            }
            catch (SqlException ex)
            {
                return new UsersModel.UpdateModel.Return
                {
                    Status = false,
                    Message = "Kullanıcı Güncellenirken Hata Oluştu",
                    ErrorCode = ex.Number.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.Now,
                    ResponseTime = DateTime.Now,
                    Data = null
                };
            }
        }
    }
}
