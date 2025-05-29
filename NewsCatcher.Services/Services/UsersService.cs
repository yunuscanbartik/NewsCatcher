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
                    if (await reader.ReadAsync())
                    {
                        userData.Add(new UsersModel.BrowseModel.ReturnData
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate"))
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

        public Task<UsersModel.BrowseModel.Return> BrowseUsersByIdAsync(UsersModel.BrowseModel.Request request)
        {
            throw new NotImplementedException();
        }

        public Task<UsersModel.DeleteModel.Return> DeleteUserAsync(UsersModel.DeleteModel.Request request)
        {
            throw new NotImplementedException();
        }

        public Task<UsersModel.UpdateModel.Return> UpdateUserAsync(UsersModel.UpdateModel.Request request)
        {
            throw new NotImplementedException();
        }
    }
}
