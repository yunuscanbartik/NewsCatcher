using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Domain.Models.Config;
using System.Data;

namespace NewsCatcher.Infrastructure.Data
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;
        public DatabaseContext(IOptions<ConnectionStringsOptions> connectionOptions)
        {
            _connectionString = connectionOptions.Value.DefaultConnection;
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public SqlConnection DatabaseConnection()
        {
            var sqlConnection = CreateConnection();
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            return sqlConnection;
        }
    }
}



