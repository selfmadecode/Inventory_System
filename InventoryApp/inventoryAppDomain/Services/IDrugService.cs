using System;
using System.Collections.Generic;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Services
{
    public interface IDrugService
    {
        List<Drug> GetAllDrugs();
        List<Drug> GetAllExpiringDrugs(TimeFrame timeFrame);
        List<Drug> GetAllExpiredDrugs();
        List<Drug> GetDrugsOutOfStock();
    }
}