using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Web.Mvc;
using inventoryAppDomain.Entities.Enums;
using Microsoft.AspNet.Identity;

namespace inventoryAppWebUi.Controllers
{
    
    public class HomeController : Controller
    {
        public ISupplierService SupplierService { get; }
        public IDrugService DrugService { get; }
        public IDrugCartService DrugCartService { get; }
        public IOrderService OrderService { get; }

        public HomeController( ISupplierService supplierService, IDrugService drugService, IDrugCartService drugCartService, IOrderService orderService)
        {
            SupplierService = supplierService;
            DrugService = drugService;
            DrugCartService = drugCartService;
            OrderService = orderService;
        }

        public ActionResult Index()
        {
            //check if user already has as cart
            if (Request.IsAuthenticated && User.Identity.IsAuthenticated)
            {
                var cart = DrugCartService.GetCart(User.Identity.GetUserId(),CartStatus.ACTIVE);
                if (cart == null)
                {
                    cart = DrugCartService.CreateCart(User.Identity.GetUserId());   
                }
            }
            var totalNumberOfSupplier = new IndexPageViewModel
            {
                TotalNumberOfSupplier = SupplierService.TotalNumberOfSupplier(),
                TotalNumberOfDrugs = DrugService.GetAllDrugs().Count,
                TotalRevenue = OrderService.GetTotalRevenue(),
                TotalSales = OrderService.GetTotalSales()

            };
            return View(totalNumberOfSupplier);
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