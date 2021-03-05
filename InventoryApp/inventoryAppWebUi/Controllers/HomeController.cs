using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace inventoryAppWebUi.Controllers
{
    
    public class HomeController : Controller
    {
        public ISupplierService _supplierService { get; }
        public IDrugService DrugService { get; }
        public IDrugCartService DrugCartService { get; }

        public HomeController( ISupplierService supplierService, IDrugService drugService, IDrugCartService drugCartService)
        {
            _supplierService = supplierService;
            DrugService = drugService;
            DrugCartService = drugCartService;
        }

        public ActionResult Index()
        {
            //check if user already has as cart
            if (Request.IsAuthenticated)
            {
                var cart = DrugCartService.GetCart(User.Identity.GetUserId());
                if (cart == null)
                {
                    cart = DrugCartService.CreateCart(User.Identity.GetUserId());   
                }
            }
            var totalNumberOfSupplier = new IndexPageViewModel
            {
                TotalNumberOfSupplier = _supplierService.TotalNumberOfSupplier(),
                TotalNumberOfDrugs = DrugService.GetAllDrugs().Count
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