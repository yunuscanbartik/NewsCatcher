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
        /// <summary>
        /// Tüm tag'leri listeler. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TagsModel.BrowseModel.Return> GetTagsAsync(TagsModel.BrowseModel.Request request)
        {
            var tags = new List<TagsModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_BrowseAll", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tags.Add(new TagsModel.BrowseModel.ReturnData
                        {
                            TagId = reader.GetInt32("TagsId"),
                            TagName = reader.GetString("TagName"),
                            CreatedAt = reader.GetDateTime("CreatedAt"),
                        });
                    }
                }                   
                return new TagsModel.BrowseModel.Return
                {
                    Status = true,
                    Message = "Etiketler Başarıyla Alındı",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = tags
                };
            }
            catch (Exception ex)
            {
                return new TagsModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Etiketler Alınamadı",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = null
                };
            }
        }
        /// <summary>
        /// Tagları istenilen Id ye göre getiri
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TagsModel.BrowseModel.Return> GetTagsByIdAsync(TagsModel.BrowseModel.Request request)
        {
            var tags = new List<TagsModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagsId", request.TagId);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tags.Add(new TagsModel.BrowseModel.ReturnData
                            {
                                TagId = reader.GetInt32("TagsId"),
                                TagName = reader.GetString("TagName"),
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            });
                        }
                    }                
                    return new TagsModel.BrowseModel.Return
                    {
                        Status = true,
                        Message = "Etiket Başarıyla Alındı",
                        ErrorCode = null,
                        ErrorMessage = null,
                        RequestId = Guid.NewGuid().ToString(),
                        StatusCode = 200,
                        RequestTime = DateTime.UtcNow,
                        ResponseTime = DateTime.UtcNow,
                        Data = tags
                    };
                }
            }
            catch (Exception ex) {
                return new TagsModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Etiket Alınamadı",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = null
                };
            }
            
        }
        /// <summary>
        /// Yeni bir tag ekler. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TagsModel.CreateModel.Return> AddTagAsync(TagsModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagName", request.TagName);
            try
            {
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
             catch (Exception ex)
            {
                return new TagsModel.CreateModel.Return
                {
                    Status = false,
                    Message = "Etiket Eklenemedi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
        /// <summary>
        /// Tagi günceller
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TagsModel.UpdateModel.Return> UpdateTagAsync(TagsModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagsId", request.TagId);
            sqlCommand.Parameters.AddWithValue("@TagName", request.TagName);
            try
            {
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
            catch (Exception ex)
            {
                return new TagsModel.UpdateModel.Return
                {
                    Status = false,
                    Message = "Etiket Güncellenemedi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
        /// <summary>
        /// Var olan Tag'i siler
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TagsModel.DeleteModel.Return> DeleteTagAsync(TagsModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Tags_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagsId", request.TagId);
            try
            {
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
            catch (Exception ex)
            {
                return new TagsModel.DeleteModel.Return
                {
                    Status = false,
                    Message = "Etiket Silinemedi",
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
}
