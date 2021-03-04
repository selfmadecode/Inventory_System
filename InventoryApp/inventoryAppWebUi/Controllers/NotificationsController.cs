using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index()
        {
            return Json( NotificationService.GetAllNotifications().Skip(Math.Max(0, NotificationService.GetAllNotifications().Count - 5)).Take(5).ToList(), JsonRequestBehavior.AllowGet);
        }

        // [HttpPost]
        // public async Task<ActionResult> CreateNotification()
        // {
        //     return Json(await NotificationService.CreateNotification("Test Notification", "This is a test", NotificationType.NONREOCCURRING));
        // }

    }
}