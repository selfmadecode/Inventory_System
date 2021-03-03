using System.Collections.Generic;
using System.Threading.Tasks;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotification(string title, string details, NotificationType notificationType);
        List<Notification> GetAllNotifications();
        List<Notification> GetAllReOccurringNotifications();
    }
}