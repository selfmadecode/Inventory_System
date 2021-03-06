using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Services
{
    public interface IReportService
    {
        Report CreateReport(TimeFrame timeFrame);
    }   
}