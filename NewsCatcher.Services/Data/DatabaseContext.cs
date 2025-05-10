using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace NewsCatcher.Services.Data
{
    /// <summary>
    /// Database bağlaantısı için app settings.json dosyasındaki DefaultConnection bağlantı dizesini kullanır.
    /// </summary>
    public class DatabaseContext
    {
        private readonly string _connectionString;
        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        /// Veritabanı bağlantısını açar ve döner.
        /// </summary>
        /// <returns></returns>
        public SqlConnection DatabaseConnection()
        {
            var sqlConnection = CreateConnection();
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
            return sqlConnection;
        }
    }
}
