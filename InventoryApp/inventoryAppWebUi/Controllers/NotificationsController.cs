using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Repository;

namespace inventoryAppWebUi.Controllers
{
    public class NotificationsController : Controller
    {
        public NotificationService NotificationService { get; }

        public NotificationsController(NotificationService notificationService)
        {
            NotificationService = notificationService;
        }
        
        // GET
        public List<Notification> Index()
        {
            return NotificationService.GetAllNotifications();
        }
        
        [HttpPost]
        public async Task<Notification> CreateNotification()
        {
            var notification = await NotificationService.CreateNotification("Test Notification", NotificationType.NONREOCCURRING);
            return notification;    
        }
        
    }
}