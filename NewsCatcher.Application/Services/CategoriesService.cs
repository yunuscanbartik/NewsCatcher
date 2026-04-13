using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private const string StoredProcedureBrowse = "sp_Categories_Browse";
        private const string StoredProcedureCreate = "sp_Categories_Create";
        private const string StoredProcedureUpdate = "sp_Categories_Update";
        private const string StoredProcedureDelete = "sp_Categories_Delete";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<CategoriesService> _logger;

        public CategoriesService(IDatabaseContext dbContext, ILogger<CategoriesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CategoriesModel.BrowseModel.Return> GetCategories(CategoriesModel.BrowseModel.Request request)
        {
            var categories = new List<CategoriesModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureBrowse, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieId", (object?)request.CategorieId ?? DBNull.Value);

            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(MapCategoryBrowseRow(reader));
                    }
                }

                return new CategoriesModel.BrowseModel.Return
                {
                    Data = categories
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to browse categories");
                throw;
            }
        }

        public async Task<CategoriesModel.CreateModel.Return> AddCategory(CategoriesModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieName", request.CategorieName);
            sqlCommand.Parameters.AddWithValue("@CategorieDescription", request.CategorieDescription);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();

                return new CategoriesModel.CreateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create category");
                throw;
            }
        }

        public async Task<CategoriesModel.UpdateModel.Return> UpdateCategory(CategoriesModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureUpdate, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieId", request.CategorieId);
            sqlCommand.Parameters.AddWithValue("@CategorieName", request.CategorieName);
            sqlCommand.Parameters.AddWithValue("@CategorieDescription", request.CategorieDescription);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new CategoriesModel.UpdateModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update category {CategoryId}", request.CategorieId);
                throw;
            }
        }

        public async Task<CategoriesModel.DeleteModel.Return> DeleteCategory(CategoriesModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureDelete, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@CategorieId", request.CategorieId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new CategoriesModel.DeleteModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete category {CategoryId}", request.CategorieId);
                throw;
            }
        }

        private static CategoriesModel.BrowseModel.ReturnData MapCategoryBrowseRow(SqlDataReader reader)
        {
            var categoryIdOrdinal = reader.GetOrdinal("CategoryId");
            var categoryId = reader["CategoryId"] == DBNull.Value ? (int?)null : reader.GetInt32(categoryIdOrdinal);
            var createdDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
            var updatedDate = reader["UpdatedDate"] == DBNull.Value
                ? createdDate
                : reader.GetDateTime(reader.GetOrdinal("UpdatedDate"));

            return new CategoriesModel.BrowseModel.ReturnData
            {
                CategorieId = categoryId,
                CategorieName = reader.GetString(reader.GetOrdinal("CategorieName")),
                CategorieDescription = reader.GetString(reader.GetOrdinal("CategorieDescription")),
                CreatedDate = createdDate,
                UpdatedDate = updatedDate
            };
        }
    }
}
