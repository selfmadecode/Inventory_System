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

        List<DrugCategory> AllCategories();
        void AddDrug(Drug drug);
        void RemoveDrug(int id);
        Drug EditDrug(int id);
        int DateComparison(DateTime FirstDate, DateTime SecondDate);

        string FormatDate(Drug drug);

        void AddDrugCategory(DrugCategory category);
    }
}