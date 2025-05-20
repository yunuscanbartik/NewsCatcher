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
            _dbContext = dbContext;
        }
        /// <summary>
        /// Kategorileri listelemek için kullanılan metot. SQL bağlantısını açar, komutu hazırlar ve çalıştırır. parametre eşlemesi yapar ve onu bir listeye atar. Atacağı nesne ise modelin içerisinden referans alır.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CategoriesModel.BrowseModel.Return> GetCategoriesAsync(CategoriesModel.BrowseModel.Request request) //Hata alıyorum object null
        {
            var categories = new List<CategoriesModel.BrowseModel.ReturnData>(); //kategorileri tutacak liste
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_BrowseAll", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync()) //using'in amacı kullanıldıktan sonra otomatik kapatılması
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new CategoriesModel.BrowseModel.ReturnData
                        { //bak burada categories modelinin returnData sınıfını kullanıyorduk ya onun içine dolduruypruz
                            CategorieId = reader.GetInt32("CategorieId"),
                            CategorieName = reader.GetString("CategorieName"),
                            CategorieDescription = reader.GetString("CategorieDescription"),
                            CreatedDate = reader.GetDateTime("CreatedDate"),
                            UpdatedDate = reader.GetDateTime("CreatedDate")
                        });
                    }
                }
                return new CategoriesModel.BrowseModel.Return
                {
                    Status = true,
                    Message = "Kategoriler Başarıyla Listelendi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(), // Her işlem için benzersiz bir ID oluştur
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = categories
                };
            }
            catch (Exception ex)
            {
                return new CategoriesModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Kategoriler Listelenirken Hata Oluştu",
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
        /// Kategorileri ID'ye göre listelemek için kullanılan metot. SQL bağlantısını açar, komutu hazırlar ve çalıştırır. parametre eşlemesi yapar ve onu bir listeye atar. Atacağı nesne ise modelin içerisinden referans alır.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CategoriesModel.BrowseModel.Return> GetCategoryByIdAsync(CategoriesModel.BrowseModel.Request request)
        {
            var categories = new List<CategoriesModel.BrowseModel.ReturnData>(); //kategorileri tutacak liste
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@CategorieId", request.CategorieId); //parametre eşlemesi yapıldı
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync()) //using'in amacı kullanıldıktan sonra otomatik kapatılması
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new CategoriesModel.BrowseModel.ReturnData
                        {
                            CategorieId = reader.GetInt32("CategorieId"),
                            CategorieName = reader.GetString("CategorieName"),
                            CategorieDescription = reader.GetString("CategorieDescription"),
                            CreatedDate = reader.GetDateTime("CreatedDate"),
                            UpdatedDate = reader.GetDateTime("CreatedDate")

                        });
                    }
                }

                return new CategoriesModel.BrowseModel.Return
                {
                    Status = true,
                    Message = "Kategori Başarıyla Listelendi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = categories
                };
            }
            catch (Exception ex)
            {
                return new CategoriesModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Kategori Listelenirken Hata Oluştu",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
        /// <summary>
        /// Data dönmeyeceğimiz sadece işlem sonucu dönecek. ExecuteNonQueryAsync kullanıyoruz.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CategoriesModel.CreateModel.Return> AddCategoryAsync(CategoriesModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_Create", sqlConnection)
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
            catch (Exception ex)
            {
                return new CategoriesModel.CreateModel.Return
                {
                    Status = false,
                    Message = "Kategori Eklenirken Hata Oluştu",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
        /// <summary>
        /// Kategorileri güncellemek için kullanılan metot. SQL bağlantısını açar, komutu hazırlar ve çalıştırır.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CategoriesModel.UpdateModel.Return> UpdateCategoryAsync(CategoriesModel.UpdateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_Update", sqlConnection)
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
            catch (Exception ex)
            {
                return new CategoriesModel.UpdateModel.Return
                {
                    Status = false,
                    Message = "Kategori Güncellenirken Hata Oluştu",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
        /// <summary>
        /// Kategorileri silmek için kullanılan metot. SQL bağlantısını açar, komutu hazırlar ve çalıştırır.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CategoriesModel.DeleteModel.Return> DeleteCategoryAsync(CategoriesModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Categories_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@CategorieId", request.CategorieId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new CategoriesModel.DeleteModel.Return
                {
                    Status = true,
                    Message = "Kategori Başarıyla Silindi",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(), // Her işlem için benzersiz bir ID oluştur
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new CategoriesModel.DeleteModel.Return
                {
                    Status = false,
                    Message = "Kategori Silinirken Hata Oluştu",
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = ex.Message,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
    };
}
