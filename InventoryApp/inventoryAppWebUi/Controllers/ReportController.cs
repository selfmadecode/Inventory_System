using System.Web.Mvc;
using inventoryAppDomain.Services;

namespace inventoryAppWebUi.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public IReportService ReportService { get; }

        public ReportController(IReportService reportService)
        {
            ReportService = reportService;
        }
        
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}