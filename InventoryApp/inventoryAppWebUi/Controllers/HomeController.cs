using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Web.Mvc;
using inventoryAppDomain.Entities.Enums;
using Microsoft.AspNet.Identity;

namespace inventoryAppWebUi.Controllers
{
    
    public class HomeController : Controller
    {
        public ISupplierService _supplierService { get; }
        public IDrugService DrugService { get; }
        public IDrugCartService DrugCartService { get; }
        public IOrderService _orderService { get; }

        public HomeController( ISupplierService supplierService, IDrugService drugService, IDrugCartService drugCartService, IOrderService orderService)
        {
            _supplierService = supplierService;
            DrugService = drugService;
            DrugCartService = drugCartService;
            _orderService = orderService;
        }

        public ActionResult Index()
        {
            //check if user already has as cart
            if (Request.IsAuthenticated)
            {
                var cart = DrugCartService.GetCart(User.Identity.GetUserId(),CartStatus.ACTIVE);
                if (cart == null)
                {
                    cart = DrugCartService.CreateCart(User.Identity.GetUserId());   
                }
            }
            var totalNumberOfSupplier = new IndexPageViewModel
            {
                TotalNumberOfSupplier = _supplierService.TotalNumberOfSupplier(),
                TotalNumberOfDrugs = DrugService.GetAllDrugs().Count,
                TotalRevenue = _orderService.GetTotalRevenue(),
                TotalSales = _orderService.GetTotalSales()

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