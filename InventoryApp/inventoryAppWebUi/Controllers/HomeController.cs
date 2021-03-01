using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    
    public class HomeController : Controller
    {
        public ISupplierService _supplierService { get; }

        public HomeController( ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public ActionResult Index()
        {
            var totalNumberOfSupplier = new IndexPageViewModel
            {
                TotalNumberOfSupplier = _supplierService.TotalNumberOfSupplier()
            };
            return View("Index", totalNumberOfSupplier);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}