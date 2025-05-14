using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;

namespace NewsCatcher.Services.Services
{
    /// <summary>
    /// Kategorilerle ilgili işlemleri gerçekleştiren servis sınıfı.Önce veritabanı bağlantısını alır, ardından SQL komutlarını çalıştırır.
    /// Insert Update Delete işlemleri için ExecuteNonQueryAsync, Browse işlemleri için ExecuteReader kullanır.
    /// </summary>
    public class CategoriesService : ICategoriesService
    {
        private readonly IDatabaseContext _dbContext;
        public CategoriesService(IDatabaseContext dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<CategoriesModel.CreateModel.Return> AddCategoryAsync(CategoriesModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieName", request.CategorieName);
            sqlCommand.Parameters.AddWithValue("@CategorieDescription", request.CategorieDescription);

            await sqlCommand.ExecuteNonQueryAsync();

            return new CategoriesModel.CreateModel.Return
            {
                Status = true,
                Message = "Kategori Başarıyla Eklendi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(), // Her işlem için benzersiz bir ID oluştur   
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<CategoriesModel.DeleteModel.Return> DeleteGetCategoryAsync(CategoriesModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieId", request.CategorieId);

            await sqlCommand.ExecuteNonQueryAsync();
            return new CategoriesModel.DeleteModel.Return
            {
                Status = true,
                Message = "Kategori Başarıyla Silindi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(), // Her işlem için benzersiz bir ID oluştur
                StatusCode = 200, // Başarılı işlem için 200 gerekli
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<CategoriesModel.BrowseModel.Return> GetCategoriesAsync(CategoriesModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_BrowseAll", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await sqlCommand.ExecuteReaderAsync();

            return new CategoriesModel.BrowseModel.Return
            {
                Status = true,
                Message = "Kategoriler Başarıyla Listelendi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(), // Her işlem için benzersiz bir ID oluştur
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };

        }

        public async Task<CategoriesModel.BrowseModel.Return> GetCategoryByIdAsync(CategoriesModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@CategorieId", request.CategorieId);

            await sqlCommand.ExecuteReaderAsync();
            return new CategoriesModel.BrowseModel.Return
            {
                Status = true,
                Message = "Kategori Başarıyla Listelendi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };

        }

        public async Task<CategoriesModel.UpdateModel.Return> UpdateCategoryAsync(CategoriesModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieId", request.CategorieId);
            sqlCommand.Parameters.AddWithValue("@CategorieName", request.CategorieName);

            await sqlCommand.ExecuteNonQueryAsync();
            return new CategoriesModel.UpdateModel.Return
            {
                Status = true,
                Message = "Kategori Başarıyla Güncellendi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(), // Her işlem için benzersiz bir ID oluştur
                StatusCode = 200, // Başarılı işlem için 200 gerekli
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }
    };
}
