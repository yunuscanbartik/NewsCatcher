using Microsoft.Data.SqlClient;

namespace NewsCatcher.Services.Data
{
    public interface IDatabaseContext
    {
        SqlConnection CreateConnection();
        SqlConnection DatabaseConnection();
    }
}