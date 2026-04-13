using Microsoft.Data.SqlClient;

namespace NewsCatcher.Domain.Interfaces
{
    public interface IDatabaseContext
    {
        SqlConnection CreateConnection();
        SqlConnection DatabaseConnection();
    }
}


