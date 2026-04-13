using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class NewsService : INewsService
    {
        private const string StoredProcedureBrowse = "sp_News_Browse";
        private const string StoredProcedureCreate = "sp_News_Create";
        private const string StoredProcedureUpdate = "sp_News_Update";
        private const string StoredProcedureDelete = "sp_News_Delete";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<NewsService> _logger;

        public NewsService(IDatabaseContext dbContext, ILogger<NewsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<NewsModel.BrowseModel.Return> GetNews(NewsModel.BrowseModel.Request request)
        {
            var news = new List<NewsModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureBrowse, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NewsId", (object?)request.NewsId ?? DBNull.Value);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        news.Add(MapNewsBrowseRow(reader));
                    }
                }

                return new NewsModel.BrowseModel.Return
                {
                    Data = news
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to browse news");
                throw;
            }
        }

        public async Task<NewsModel.CreateModel.Return> AddNews(NewsModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Title", request.Title);
            sqlCommand.Parameters.AddWithValue("@Content", request.Content);
            sqlCommand.Parameters.AddWithValue("@Summary", request.Summary);
            sqlCommand.Parameters.AddWithValue("@CategoryId", request.CategoryId);
            sqlCommand.Parameters.AddWithValue("@SourceName", request.SourceName);
            sqlCommand.Parameters.AddWithValue("@ThumbnailUrl", request.ThumbnailUrl ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Link", request.Link ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@GuId", request.GuId ?? (object)DBNull.Value);
            sqlCommand.Parameters.Add(new SqlParameter("@NewsId", SqlDbType.Int) { Direction = ParameterDirection.Output });
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new NewsModel.CreateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create news");
                throw;
            }
        }

        public async Task<NewsModel.UpdateModel.Return> UpdateNews(NewsModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureUpdate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            sqlCommand.Parameters.AddWithValue("@Title", request.Title);
            sqlCommand.Parameters.AddWithValue("@Content", request.Content);
            sqlCommand.Parameters.AddWithValue("@Summary", request.Summary);
            sqlCommand.Parameters.AddWithValue("@CategoryId", request.CategoryId);
            sqlCommand.Parameters.AddWithValue("@SourceName", request.SourceName);
            sqlCommand.Parameters.AddWithValue("@ThumbnailUrl", request.ThumbnailUrl ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Link", request.Link ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@GuId", request.GuId ?? (object)DBNull.Value);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new NewsModel.UpdateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update news {NewsId}", request.NewsId);
                throw;
            }
        }

        public async Task<NewsModel.DeleteModel.Return> DeleteNews(NewsModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureDelete, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NewsId", request.NewsId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new NewsModel.DeleteModel.Return
                {
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete news {NewsId}", request.NewsId);
                throw;
            }
        }

        public async Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabase(List<NewsModel.CreateModel.ReturnData> returnDataList)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            try
            {
                foreach (var item in returnDataList)
                {
                    using (var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Title", (object)item.Title?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Content", (object)item.Content?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Summary", (object)item.Summary?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@CategoryId", item.CategoryId.HasValue && item.CategoryId != 0 ? (object)item.CategoryId : DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@SourceName", (object)item.SourceName?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@ThumbnailUrl", (object)item.ThumbnailUrl?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@GuId", (object)item.GuId?.Trim() ?? DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Link", (object)item.Link?.Trim() ?? DBNull.Value);
                        var newsIdParam = new SqlParameter("@NewsId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        sqlCommand.Parameters.Add(newsIdParam);
                        await sqlCommand.ExecuteNonQueryAsync();
                        item.NewsId = (int)newsIdParam.Value;
                    }
                }

                return returnDataList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save news batch to database");
                throw;
            }
        }

        private static NewsModel.BrowseModel.ReturnData MapNewsBrowseRow(SqlDataReader reader)
        {
            return new NewsModel.BrowseModel.ReturnData
            {
                NewsId = reader.GetInt32(reader.GetOrdinal("NewsId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Content = reader.GetString(reader.GetOrdinal("Content")),
                Summary = reader.GetString(reader.GetOrdinal("Summary")),
                CategoryId = reader["CategoryId"] == DBNull.Value ? (int?)null : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                ShareDate = reader.GetDateTime(reader.GetOrdinal("ShareDate")),
                SourceName = reader.GetString(reader.GetOrdinal("SourceName")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                ThumbnailUrl = reader["ThumbnailUrl"] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal("ThumbnailUrl")),
                Link = reader["Link"] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal("Link")),
                GuId = reader["GuId"] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal("GuId"))
            };
        }
    }
}
