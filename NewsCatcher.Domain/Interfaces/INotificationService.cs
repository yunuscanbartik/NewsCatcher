using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationModel.CreateModel.Return> AddNotification(NotificationModel.CreateModel.Request request);
        Task<NotificationModel.BrowseModel.Return> GetNotifications(NotificationModel.BrowseModel.Request request);
        Task<NotificationModel.NotificationReadModel.Return> NotificationIsRead(NotificationModel.NotificationReadModel.Request request);
        Task<NotificationModel.DeleteModel.Return> DeleteNotification(NotificationModel.DeleteModel.Request request);
    }
}
