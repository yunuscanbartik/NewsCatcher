using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NewsCatcherApi.Data
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
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
