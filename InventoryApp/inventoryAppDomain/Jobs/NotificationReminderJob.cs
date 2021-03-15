using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.ExtensionMethods;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Repository;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Jobs
{
    public class NotificationReminderJob
    {
        private static ApplicationDbContext _dbContext;
        

        public NotificationReminderJob()
        {
            _dbContext = new ApplicationDbContext();
        }
        
        //Weekly Notification
        //Monthly Notifications
        //Purge Notifications When they have expired
        

        public void RunReminder(TimeFrame timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrame.WEEKLY:
                {
                    var beginningOfWeek = DateTime.Now.FirstDayOfWeek();
                    var endOfWeek = DateTime.Now.LastDayOfWeek();

                    var drugs = _dbContext.Drugs.Where(drug => DateTime.Now.Month == drug.ExpiryDate.Month
                                                               && DateTime.Now.Year == drug.ExpiryDate.Year)
                        .Where(drug => drug.ExpiryDate >= beginningOfWeek && drug.ExpiryDate <= endOfWeek).ToList();
                    drugs.ForEach(drug =>
                    {
                        var notification = new Notification()
                        {
                            Title = "Drug Expiration",
                            NotificationDetails = $"{drug.DrugName} is Expiring this Week.",
                            NotificationType = NotificationType.REOCCURRING,
                            NotificationCategory = NotificationCategory.EXPIRATION
                        };
                        _dbContext.Notifications.Add(notification);
                    }); 
                    _dbContext.SaveChanges();
                    break;
                }
                case TimeFrame.MONTHLY:
                {
                    var drugs = _dbContext.Drugs.Where(drug => DateTime.Now.Month.Equals(drug.ExpiryDate.Month) 
                                                               && DateTime.Now.Year.Equals(drug.ExpiryDate.Year)).ToList();
                    drugs.ForEach(drug =>
                    {
                        var notification = new Notification()
                        {
                            Title = "Drug Expiration",
                            NotificationDetails = $"{drug.DrugName} is Expiring this Month.",
                            NotificationType = NotificationType.REOCCURRING,
                            NotificationCategory = NotificationCategory.EXPIRATION
                        };
                        _dbContext.Notifications.Add(notification);
                    });
                    _dbContext.SaveChanges();
                    break;
                }
            }
        }

        public void OutOfStockReminders()
        {
            var drugs = _dbContext.Drugs.Where(drug => drug.Quantity <= 20).ToList();
            
            drugs.ForEach(drug =>
            {
                var notification = new Notification()
                {
                    Title = "Running Out Stock",
                    NotificationDetails = $"{drug.DrugName} is Running Out.",
                    NotificationType = NotificationType.REOCCURRING,
                    NotificationCategory = NotificationCategory.RUNNING_OUT_OF_STOCK
                };
                _dbContext.Notifications.Add(notification);
            });
            _dbContext.SaveChanges();
        }

        public void ExpireDrugs()
        {
            var drugs = _dbContext.Drugs.Where(drug => drug.ExpiryDate.Equals(DateTime.Today)).ToList();
            
            drugs.ForEach(drug =>
            {
                drug.CurrentDrugStatus = DrugStatus.EXPIRED;
                _dbContext.Entry(drug).State = EntityState.Modified;
            });

            _dbContext.SaveChanges();
        }
        
    }
}