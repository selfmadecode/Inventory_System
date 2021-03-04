using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Jobs
{
    public class NotificationReminderJob
    {
        private static ApplicationDbContext _dbContext;
        public static INotificationService NotificationService { get; set; }
        public static IDrugService DrugService { get; set; }

        public NotificationReminderJob(INotificationService notificationService, IDrugService drugService)
        {
            NotificationService = notificationService;
            DrugService = drugService;
        }

        public NotificationReminderJob()
        {
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }
        
        //Weekly Notification
        //Monthly Notifications
        //Purge Notifications When they have expired
        

        public static void RunReminder(TimeFrame timeFrame)
        {
            
        }

        public static async Task ExpireDrugs()
        {
            
        }
        
    }
}