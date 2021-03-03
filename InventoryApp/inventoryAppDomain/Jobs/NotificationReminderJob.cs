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
            switch (timeFrame)
            {
                case TimeFrame.WEEKLY:
                {
                    var drugs = DrugService.GetAllExpiringDrugs(timeFrame);
                    drugs.ForEach(drug =>
                    {
                        NotificationService.CreateNotification("Drug Expiration",$"{drug.DrugName} is Expiring this Week.",
                            NotificationType.REOCCURRING);
                    });
                    break;
                }
                case TimeFrame.MONTHLY:
                {
                    var drugs = DrugService.GetAllExpiringDrugs(timeFrame);
                    drugs.ForEach(drug =>
                    {
                        NotificationService.CreateNotification("Drug Expiration", $"{drug.DrugName} is Expiring this Month.",
                            NotificationType.REOCCURRING);
                    });
                    break;
                }
            }
        }

        public static async Task ExpireDrugs()
        {
            var drugs = DrugService.GetAllExpiredDrugs();
            
            drugs.ForEach(drug =>
            {
                drug.CurrentDrugStatus = DrugStatus.EXPIRED;
                _dbContext.Entry(drug).State = EntityState.Modified;
            });

            await _dbContext.SaveChangesAsync();
        }
        
    }
}