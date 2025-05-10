using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsCatcherApi.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationModel.CreateModel.Return> AddNotificationAsync(NotificationModel.CreateModel.Request request);
        Task<NotificationModel.BrowseModel.Return> GetNotificationAsync(NotificationModel.BrowseModel.Request request);
        Task<NotificationModel.NotificationReadModel.Return> NotificationIsReadAsync(NotificationModel.NotificationReadModel.Request request);
        Task<NotificationModel.DeleteModel.Return> DeleteNotificationAsync(NotificationModel.DeleteModel.Request request);
    }
}
