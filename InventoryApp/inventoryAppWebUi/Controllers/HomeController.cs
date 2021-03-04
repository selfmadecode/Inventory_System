using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    
    public class HomeController : Controller
    {
        public ISupplierService _supplierService { get; }
        public IDrugService DrugService { get; }

        public HomeController( ISupplierService supplierService, IDrugService drugService)
        {
            _supplierService = supplierService;
            DrugService = drugService;
        }

        public ActionResult Index()
        {
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