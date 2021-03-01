using inventoryAppDomain.Services;

namespace inventoryAppDomain.Jobs
{
    public class NotificationReminderJob
    {
        public INotificationService NotificationService { get; }

        public NotificationReminderJob(INotificationService notificationService)
        {
            NotificationService = notificationService;
        }
        
        
    }
}