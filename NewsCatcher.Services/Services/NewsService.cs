using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;
using System.Reflection.PortableExecutable;

namespace NewsCatcher.Services.Services
{
    public class NewsService : INewsService
    {
        private readonly IDatabaseContext _dbContext;
        public NewsService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Haberleri veritabanından alır. Client' ın isteği doğrultusunda haberleri listeler.  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NewsModel.BrowseModel.Return> GetNewsAsync(NewsModel.BrowseModel.Request request)
        {
            var news = new List<NewsModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_BrowseAll", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        news.Add(new NewsModel.BrowseModel.ReturnData
                        {
                            NewsId = reader.GetInt32("NewsId"),
                            Title = reader.GetString("Title"),
                            Content = reader.GetString("Content"),
                            Summary = reader.GetString("Summary"),
                            CategoryId = reader.GetInt32("CategoryId"),
                            ShareDate = reader.GetDateTime("ShareDate"),
                            SourceName = reader.GetString("SourceName"),
                            CreatedDate = reader.GetDateTime("CreatedDate"),
                            UpdatedDate = reader.GetDateTime("UpdatedDate")
                        });
                }
                return new NewsModel.BrowseModel.Return
                {
                    Status = true,
                    Message = "Haber Başarıyla Alındı",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = news
                };
            }
            catch (Exception ex)
            {
                return new NewsModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Haber Alınırken Hata Oluştu",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = null
                };
            }
        }
        /// <summary>
        /// Haberleri veritabanından alır. Client' ın istediği haberleri alır ve liste ile döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NewsModel.BrowseModel.Return> GetNewsByIdAsync(NewsModel.BrowseModel.Request request)
        {
            var news = new List<NewsModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        news.Add(new NewsModel.BrowseModel.ReturnData
                        {
                            NewsId = reader.GetInt32("NewsId"),
                            Title = reader.GetString("Title"),
                            Content = reader.GetString("Content"),
                            Summary = reader.GetString("Summary"),
                            CategoryId = reader.GetInt32("CategoryId"),
                            ShareDate = reader.GetDateTime("ShareDate"),
                            SourceName = reader.GetString("SourceName"),
                            CreatedDate = reader.GetDateTime("CreatedDate"),
                            UpdatedDate = reader.GetDateTime("UpdatedDate")
                        });
                    }
                return new NewsModel.BrowseModel.Return
                {
                    Status = true,
                    Message = "Haber Başarıyla Alındı",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = news
                };
            }
            catch (Exception ex)
            {
                return new NewsModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Haber Alınırken Hata Oluştu",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = null
                };
            }
        }
        /// <summary>
        /// Haberleri veritabanına ekler.Client'ın gönderdiği verileri sokar.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
            try
            {
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
            catch (Exception ex)
            {
                return new NewsModel.CreateModel.Return
                {
                    Status = false,
                    Message = "Haber Eklenirken Hata Oluştu",
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
        /// Haber güncelleme işlemi için Db deki prosedürü çağırıyoruz. herhangi bir data döndürmüyoruz.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NewsModel.UpdateModel.Return> UpdateNewsAsync(NewsModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            sqlCommand.Parameters.AddWithValue("@Title", request.Title);
            sqlCommand.Parameters.AddWithValue("@Content", request.Content);
            sqlCommand.Parameters.AddWithValue("@Summary", request.Summary);
            sqlCommand.Parameters.AddWithValue("@CategoryId", request.CategoryId);
            sqlCommand.Parameters.AddWithValue("@SourceName", request.SourceName);
            try
            {
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
            catch (SqlException ex)
            {
                return new NewsModel.UpdateModel.Return
                {
                    Status = false,
                    Message = ex.Message,
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
        ///  Haberi silmek için Db deki prosedürü çağırıyoruz. herhangi bir data döndürmüyoruz.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NewsModel.DeleteModel.Return> DeleteNewsAsync(NewsModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_News_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            try
            {
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
                    ResponseTime = DateTime.UtcNow,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new NewsModel.DeleteModel.Return
                {
                    Status = false,
                    Message = "Haber Silinirken Hata Oluştu",
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
    }
}
