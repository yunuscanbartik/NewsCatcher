using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;
using System.Security.AccessControl;

namespace NewsCatcher.Services.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly DatabaseContext _dbContext;
        public CategoriesService(DatabaseContext dbContext)
        {
            dbContext = _dbContext;
        }
        public Task<CategoriesModel.CreateModel.Return> AddCategoryAsync(CategoriesModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieName", request.CategorieName);
            sqlCommand.Parameters.AddWithValue("@CategorieDescription", request.CategorieDescription);      
 
            var result = sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
            return result;
        }

        public Task<CategoriesModel.DeleteModel.Return> DeleteGetCategoryAsync(CategoriesModel.DeleteModel.Request request)
        {
            throw new NotImplementedException();
        }

        public Task<CategoriesModel.BrowseModel.Return> GetCategoriesAsync(CategoriesModel.BrowseModel.Request request)
        {
            throw new NotImplementedException();
        }

        public Task<CategoriesModel.BrowseModel.Return> GetCategoryByIdAsync(CategoriesModel.BrowseModel.Request request)
        {
            throw new NotImplementedException();
        }

        public Task<CategoriesModel.UpdateModel.Return> UpdateCategoryAsync(CategoriesModel.UpdateModel.Request request)
        {
            throw new NotImplementedException();
        }
    }
}
