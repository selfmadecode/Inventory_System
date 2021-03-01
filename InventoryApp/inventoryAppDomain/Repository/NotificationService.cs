using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class NotificationService: INotificationService
    {
        private ApplicationDbContext _dbContext;

        public NotificationService()
        {
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }
        
        public async Task<Notification> CreateNotification(string details, NotificationType notificationType)
        {
            var notification = new Notification()
            {
                NotificationDetails = details,
                NotificationType = notificationType
            };

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        public List<Notification> GetAllNotifications()
        {
            return _dbContext.Notifications.ToList();
        }

        public List<Notification> GetAllReOccurringNotifications()
        {
            return _dbContext.Notifications
                .Where(notification => notification.NotificationType == NotificationType.REOCCURRING).ToList();
        }
    }
}