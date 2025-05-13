using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;

namespace NewsCatcher.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _dbContext;
        public NotificationService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<NotificationModel.CreateModel.Return> AddNotificationAsync(NotificationModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            sqlCommand.Parameters.AddWithValue("@Message", request.Message);
            await sqlCommand.ExecuteNonQueryAsync();
            return new NotificationModel.CreateModel.Return
            {
                Status = true,
                Message = "Bildirim Başarıyla Eklendi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<NotificationModel.DeleteModel.Return> DeleteNotificationAsync(NotificationModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NotificationId", request.NotificationId);

            await sqlCommand.ExecuteNonQueryAsync();
            return new NotificationModel.DeleteModel.Return
            {
                Status = true,
                Message = "Bildirim Başarıyla Silindi",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<NotificationModel.BrowseModel.Return> GetNotificationByIdAsync(NotificationModel.BrowseModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NotificationId", request.NotificationId);

            await sqlCommand.ExecuteReaderAsync();
            return new NotificationModel.BrowseModel.Return
            {
                Status = true,
                Message = "Bildirim Başarıyla Alındı",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }

        public async Task<NotificationModel.NotificationReadModel.Return> NotificationIsReadAsync(NotificationModel.NotificationReadModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_MarkAsRead", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NotificationId", request.NotificationId);
            sqlCommand.Parameters.AddWithValue("@IsRead", request.IsRead);

            await sqlCommand.ExecuteNonQueryAsync();
            return new NotificationModel.NotificationReadModel.Return
            {
                Status = true,
                Message = "Bildirim Başarıyla Okundu",
                ErrorCode = null,
                ErrorMessage = null,
                RequestId = Guid.NewGuid().ToString(),
                StatusCode = 200,
                RequestTime = DateTime.UtcNow,
                ResponseTime = DateTime.UtcNow
            };
        }
    }
}
