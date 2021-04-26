using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.ExtensionMethods;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class ReportService : IReportService
    {
        private ApplicationDbContext _dbContext;
        private readonly IOrderService _orderService;

        public ReportService(IOrderService orderService)
        {
            _orderService = orderService;
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public string GenerateSalesTable(List<DrugCartItem> cartItems)
        {
            var sb = new StringBuilder();
            var table = @"
                                <table class= "" table table-hover table-bordered text-left "">
                                <thead>
                                    <tr class= ""table-success "">
                                    <th>Drug Name</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    </tr>
                                </thead>";
            sb.Append(table);
            foreach (var item in cartItems)
            {
                string row = $@"<tbody>
                                <tr class=""info"" style="" cursor: pointer"">
                                <td class=""font-weight-bold"">{item.Drug.DrugName}</td>
                                <td class=""font-weight-bold"">{item.Amount}</td>
                                <td class=""font-weight-bold"">{item.Drug.Price}</td>
                         </tr>
                         </tbody>";
                sb.Append(row);
            }

            sb.Append("</Table>");
            return sb.ToString();
        }

        public Report CreateReport(TimeFrame timeFrame)
        {
            Report report;
            switch (timeFrame)
            {
                case TimeFrame.DAILY:
                {
                    Func<Report, bool> dailyFunc = report1 => report1.CreatedAt.Date == DateTime.Now.Date && report1.TimeFrame == timeFrame;
                    report = _dbContext.Reports.FirstOrDefault(dailyFunc) ?? new Report();

                    report.Orders = _orderService.GetOrdersForTheDay();
                    report.TimeFrame = timeFrame;
                    report.TotalRevenueForReport = _orderService.GetOrdersForTheDay().Select(order => order.Price).Sum();

                    var drugItem = new List<DrugCartItem>();
                    var drugs = new List<Drug>();
                    var orders = _orderService.GetOrdersForTheDay();
                    foreach (var order in orders)
                    {
                        foreach (var drugCartItem in order.OrderItems)
                        {
                            drugItem.Add(_dbContext.DrugCartItems.Include(item => item.Drug).Include(item => item.DrugCart).FirstOrDefault(item => item.Id == drugCartItem.Id));
                            drugs.Add(_dbContext.Drugs.FirstOrDefault(drug => drug.Id == drugCartItem.DrugId));
                        }
                    }

                    report.DrugSales = GenerateSalesTable(drugItem);
                    report.ReportDrugs = drugs;

                    if (_dbContext.Reports.Any(dailyFunc))
                    {
                        _dbContext.Entry(report).State = EntityState.Modified;
                    }
                    else
                    {
                        _dbContext.Reports.Add(report);
                    }

                    _dbContext.SaveChanges();
                    return report;
                }
                case TimeFrame.WEEKLY:
                {
                    var beginningOfWeek = DateTime.Now.FirstDayOfWeek();
                    var lastDayOfWeek = DateTime.Now.LastDayOfWeek();
                    Func<Report, bool> weeklyFunc = report1 =>
                        report1.CreatedAt.Month.Equals(beginningOfWeek.Month) &&
                        report1.CreatedAt.Year.Equals(beginningOfWeek.Year) && (report1.CreatedAt >= beginningOfWeek &&
                        report1.CreatedAt <= lastDayOfWeek && report1.TimeFrame == timeFrame);

                    
                    report = _dbContext.Reports.FirstOrDefault(weeklyFunc) ?? new Report();

                    report.Orders = _orderService.GetOrdersForTheWeek();
                    report.TimeFrame = timeFrame;
                    report.TotalRevenueForReport =
                        _orderService.GetOrdersForTheWeek().Select(order => order.Price).Sum();

                    var drugItem = new List<DrugCartItem>();
                    var drugs = new List<Drug>();
                    var orders = _orderService.GetOrdersForTheWeek();
                    foreach (var order in orders)
                    {
                        foreach (var drugCartItem in order.OrderItems)
                        {
                            drugItem.Add(_dbContext.DrugCartItems.Include(item => item.Drug).Include(item => item.DrugCart).FirstOrDefault(item => item.Id == drugCartItem.Id));
                            drugs.Add(_dbContext.Drugs.FirstOrDefault(drug => drug.Id == drugCartItem.DrugId));
                        }
                    }

                    report.ReportDrugs = drugs;
                    report.DrugSales = GenerateSalesTable(drugItem);

                    if (_dbContext.Reports.Any(weeklyFunc))
                    {
                        _dbContext.Entry(report).State = EntityState.Modified;
                    }
                    else
                    {
                        _dbContext.Reports.Add(report);
                    }

                    _dbContext.SaveChanges();
                    return report;
                }
                case TimeFrame.MONTHLY:
                {
                    Func<Report, bool> monthlyFunc = report1 =>
                        report1.CreatedAt.Month.Equals(DateTime.Now.Month) &&
                        report1.CreatedAt.Year.Equals(DateTime.Now.Year) && report1.TimeFrame == timeFrame;
                    
                    report = _dbContext.Reports.FirstOrDefault(monthlyFunc);
                    if (report == null)
                    {
                        report = new Report();
                    }

                    report.Orders = _orderService.GetOrdersForTheMonth();
                    report.TimeFrame = timeFrame;
                    report.TotalRevenueForReport =
                        _orderService.GetOrdersForTheMonth().Select(order => order.Price).Sum();

                    var drugItem = new List<DrugCartItem>();
                    var drugs = new List<Drug>();
                    var orders = _orderService.GetOrdersForTheMonth();
                    foreach (var order in orders)
                    {
                        foreach (var drugCartItem in order.OrderItems)
                        {
                            drugItem.Add(_dbContext.DrugCartItems.Include(item => item.Drug).Include(item => item.DrugCart).FirstOrDefault(item => item.Id == drugCartItem.Id));
                            drugs.Add(_dbContext.Drugs.FirstOrDefault(drug => drug.Id == drugCartItem.DrugId));
                        }
                    }

                    report.DrugSales = GenerateSalesTable(drugItem);
                    report.ReportDrugs = drugs;
                    if (_dbContext.Reports.Any(monthlyFunc))
                    {
                        _dbContext.Entry(report).State = EntityState.Modified;
                    }
                    else
                    {
                        _dbContext.Reports.Add(report);
                    }

                    _dbContext.SaveChanges();
                    return report;
                }
                default: return null;
            }
        }
    }
}