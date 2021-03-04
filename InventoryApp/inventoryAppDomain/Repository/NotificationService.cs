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
    public class NotificationService : INotificationService
    {
        private ApplicationDbContext _dbContext;

        public NotificationService()
        {
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public async Task<Notification> CreateNotification(string title, string details, NotificationType notificationType)
        {
            Notification notification = null;
            switch (notificationType)
            {
                case NotificationType.NONREOCCURRING:
                {
                    notification = new Notification()
                    {
                        Title = title,
                        NotificationDetails = details,
                        NotificationType = notificationType
                    };
                    _dbContext.Notifications.Add(notification);
                    break;
                }
                case NotificationType.REOCCURRING:
                {
                    notification = await _dbContext.Notifications.FirstOrDefaultAsync(notification1 =>
                        notification1.NotificationDetails.Equals(details));

                    if (notification == null)
                    {
                        notification = new Notification()
                        {
                            Title = title,
                            NotificationDetails = details,
                            NotificationType = notificationType
                        };
                        _dbContext.Notifications.Add(notification);
                    }
                    break;
                }
            }

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