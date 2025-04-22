using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NewsCatcher.Domain.Interfaces;
using NewsCatcher.Models.Models;
using System.Data;

namespace NewsCatcher.Application.Services
{
    public class NotificationService : INotificationService
    {
        private const string StoredProcedureBrowse = "sp_Notification_Browse";
        private const string StoredProcedureMarkAsRead = "sp_Notification_MarkAsRead";
        private const string StoredProcedureCreate = "sp_Notification_Create";
        private const string StoredProcedureDelete = "sp_Notification_Delete";

        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IDatabaseContext dbContext, ILogger<NotificationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<NotificationModel.BrowseModel.Return> GetNotifications(NotificationModel.BrowseModel.Request request)
        {
            var notifications = new List<NotificationModel.BrowseModel.ReturnData>();
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureBrowse, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserId", (object?)request.UserId ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@NotificationId", (object?)request.NotificationId ?? DBNull.Value);
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
                            IsRead = reader.GetBoolean("IsRead"),
                            SendDate = reader.GetDateTime("SendDate")
                        });
                    }
                }

                return new NotificationModel.BrowseModel.Return
                {
                    Data = notifications
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to browse notifications");
                throw;
            }
        }

        public async Task<NotificationModel.NotificationReadModel.Return> NotificationIsRead(NotificationModel.NotificationReadModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureMarkAsRead, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@NotificationId", request.NotificationId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new NotificationModel.NotificationReadModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark notification {NotificationId} as read", request.NotificationId);
                throw;
            }
        }

        public async Task<NotificationModel.CreateModel.Return> AddNotification(NotificationModel.CreateModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureCreate, sqlConnection)
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
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create notification for user {UserId}", request.UserId);
                throw;
            }
        }

        public async Task<NotificationModel.DeleteModel.Return> DeleteNotification(NotificationModel.DeleteModel.Request request)
        {
            var sqlConnection = _dbContext.DatabaseConnection();
            var sqlCommand = new SqlCommand(StoredProcedureDelete, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@NotificationId", request.NotificationId);
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                return new NotificationModel.DeleteModel.Return
                {
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete notification {NotificationId}", request.NotificationId);
                throw;
            }
        }
    }
}
