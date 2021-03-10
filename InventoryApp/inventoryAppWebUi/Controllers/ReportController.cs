using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;

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
        public ActionResult Index(TimeFrame timeFrame)
        {
            ViewBag.CurrentPage = timeFrame + " Report";
            var report = Mapper.Map<ReportViewModel>(ReportService.CreateReport(timeFrame));
            return View(report);
        }
    }
}