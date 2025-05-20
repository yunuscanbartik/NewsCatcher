using Microsoft.Data.SqlClient;
using NewsCatcher.Models.Models;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System.Data;

namespace NewsCatcher.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IDatabaseContext _dbContext;
        public NotificationService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Bildirimleri almak için kullanılır. Kullanıcıya ait bildirimleri getirir.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NotificationModel.BrowseModel.Return> GetNotificationByIdAsync(NotificationModel.BrowseModel.Request request)
        {
            var notifications = new List<NotificationModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_BrowseById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            try
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                        while (await reader.ReadAsync())
                        {
                            notifications.Add(new NotificationModel.BrowseModel.ReturnData
                            {
                                NotificationId = reader.GetInt32("NotificationId"),
                                UserId = reader.GetInt32("UserId"),
                                Message = reader.GetString("Message"),
                                IsRead = reader.GetBoolean("IsRead"),
                                SendDate = reader.GetDateTime("SendDate")
                            });
                        }
                    return new NotificationModel.BrowseModel.Return
                    {
                        Status = true,
                        Message = "Bildirim Başarıyla Alındı",
                        ErrorCode = null,
                        ErrorMessage = null,
                        RequestId = Guid.NewGuid().ToString(),
                        StatusCode = 200,
                        RequestTime = DateTime.UtcNow,
                        ResponseTime = DateTime.UtcNow,
                        Data = notifications
                    };
                }
            }
            catch (Exception ex)
            {
                return new NotificationModel.BrowseModel.Return
                {
                    Status = false,
                    Message = "Bildirim Düşmedi.",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow,
                    Data = null
                };
            }
        }
        /// <summary>
        /// Bildirimin okunduğunu belirtmek için kullanılır. Kullanıcıya ait bildirimleri okundu olarak işaretler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NotificationModel.NotificationReadModel.Return> NotificationIsReadAsync(NotificationModel.NotificationReadModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_MarkAsRead", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NotificationId", request.NotificationId);
            try
            {
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
            catch (Exception ex)
            {
                return new NotificationModel.NotificationReadModel.Return
                {
                    Status = false,
                    Message = "Bildirim Okunamadı.",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
        /// <summary>
        /// Yeni bir bildirim eklemek için kullanılır. Kullanıcıya ait yeni bir bildirim oluşturur.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NotificationModel.CreateModel.Return> AddNotificationAsync(NotificationModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_Create", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
            sqlCommand.Parameters.AddWithValue("@Message", request.Message);
            try
            {
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
            catch (Exception ex)
            {
                return new NotificationModel.CreateModel.Return
                {
                    Status = false,
                    Message = "Bildirim Eklenemdi.",
                    ErrorCode = null,
                    ErrorMessage = null,
                    RequestId = Guid.NewGuid().ToString(),
                    StatusCode = 200,
                    RequestTime = DateTime.UtcNow,
                    ResponseTime = DateTime.UtcNow
                };
            }
        }
        /// <summary>
        /// Bildirimi Silmek için kullanılır. Kullanıcıya ait bildirimleri siler.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<NotificationModel.DeleteModel.Return> DeleteNotificationAsync(NotificationModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand("sp_Notification_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NotificationId", request.NotificationId);
            try
            {
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
            catch (Exception ex)
            {
                return new NotificationModel.DeleteModel.Return
                {
                    Status = false,
                    Message = "Bildirim Silinemedi.",
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
}
