using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class TagsService : ITagsService
    {
        private const string StoredProcedureBrowse = "sp_Tags_Browse";
        private const string StoredProcedureCreate = "sp_Tags_Create";
        private const string StoredProcedureUpdate = "sp_Tags_Update";
        private const string StoredProcedureDelete = "sp_Tags_Delete";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<TagsService> _logger;

        public TagsService(IDatabaseContext dbContext, ILogger<TagsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<TagsModel.BrowseModel.Return> GetTags(TagsModel.BrowseModel.Request request)
        {
            var tags = new List<TagsModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureBrowse, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@TagsId", (object?)request.TagId ?? DBNull.Value);
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
                    Data = tags
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to browse tags");
                throw;
            }
        }

        public async Task<TagsModel.CreateModel.Return> AddTag(TagsModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagName", request.TagName);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new TagsModel.CreateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create tag");
                throw;
            }
        }

        public async Task<TagsModel.UpdateModel.Return> UpdateTag(TagsModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureUpdate, sqlConnection)
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
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update tag {TagId}", request.TagId);
                throw;
            }
        }

        public async Task<TagsModel.DeleteModel.Return> DeleteTag(TagsModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureDelete, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@TagsId", request.TagId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new TagsModel.DeleteModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete tag {TagId}", request.TagId);
                throw;
            }
        }
    }
}
