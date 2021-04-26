﻿using System;
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

        void AddDrugCategory(DrugCategory category);
        void RemoveDrugCategory(int id);

        List<Drug> GetAvailableDrugs();

        List<Drug> GetAvailableDrugFilter(string searchQuery);

        void UpdateDrug(Drug drug);

        DrugCategory EditDrugCategory(int id);
        void UpdateDrugCategory(DrugCategory category);

    }
}