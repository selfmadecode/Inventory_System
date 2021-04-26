﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class DrugService : IDrugService
    {
        private ApplicationDbContext _dbContext;
        
        public DrugService()
        {
           _dbContext =  HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public List<Drug> GetAllDrugs() => _dbContext.Drugs.Include(d => d.DrugCategory).ToList();

        public List<Drug> GetAvailableDrugs()
        {
            var drugs = GetAllDrugs();
            var availableDrugs = new List<Drug>();
            drugs.ForEach(drug =>
           {
               if (drug.Quantity > 0)
               {
                   if(drug.ExpiryDate.CompareTo(DateTime.Now) == 1)
                   {
                       availableDrugs.Add(drug);
                   }
               }
           }
            );
            return availableDrugs;
        }
        public List<Drug> GetAvailableDrugFilter(string searchQuery)
        {
            var queries = string.IsNullOrEmpty(searchQuery) ? null : Regex.Replace(searchQuery, @"\s+", " ").Trim().ToLower();
            if (queries == null)
            {
                return GetAvailableDrugs();
            }
            return GetAvailableDrugs().Where(item => queries.Any(query => (item.DrugName.ToLower().Contains(query)))).ToList();
        }

        public List<Drug> GetAllExpiringDrugs(TimeFrame timeFrame)
        {
            var drugs = GetAllDrugs();
            var expiringDrugs = new List<Drug>();
            var today = DateTime.Now;
            switch (timeFrame)
            {
                case TimeFrame.MONTHLY:
                {
                    drugs.ForEach(drug =>
                    {
                        if (today.AddMonths(1).Month.Equals(drug.ExpiryDate.Month))
                        {
                            expiringDrugs.Add(drug);
                        }
                    });
                    break;
                }
                case TimeFrame.WEEKLY:
                {
                    drugs.ForEach(drug =>
                    {
                        if (today.AddDays(7).Day.Equals(drug.ExpiryDate.Day))
                        {
                            expiringDrugs.Add(drug);
                        }
                    });
                    break;
                }
                default:
                {
                    throw new Exception("An Error Occurred");
                }
            }

            return expiringDrugs;
        }

        public List<Drug> GetAllExpiredDrugs()
        {
            var drugs = GetAllDrugs();
            var expiredDrugs = new List<Drug>();
            drugs.ForEach(drug =>
            {
                if (drug.ExpiryDate.CompareTo(DateTime.Today) == 1)
                {
                    expiredDrugs.Add(drug);
                }
            });
            return expiredDrugs;
        }

        public List<Drug> GetDrugsOutOfStock()
        {
            var drugs = GetAllDrugs();
            var drugsRunningOutOfStock = new List<Drug>();
            drugs.ForEach(drug =>
            {
                if (drug.Quantity <= 20)
                {
                    drugsRunningOutOfStock.Add(drug);
                }
            });
            return drugsRunningOutOfStock;
        }

        public List<DrugCategory> AllCategories() => _dbContext.DrugCategories.ToList();


        public void AddDrug(Drug drug)
        {
            _dbContext.Drugs.Add(drug);
            _dbContext.SaveChanges();
        }
        public void UpdateDrug(Drug drug)
        {
            var update = _dbContext.Drugs.Add(drug);
            _dbContext.Entry(update).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }

        public void RemoveDrug(int id)
        {
            _dbContext.Drugs.Remove(_dbContext.Drugs.Single(d => d.Id == id));
            _dbContext.SaveChanges();
        }
        public Drug EditDrug(int id) => _dbContext.Drugs.SingleOrDefault(d => d.Id == id);
      
        public int DateComparison(DateTime FirstDate, DateTime SecondDate) => 
            DateTime.Compare(FirstDate, SecondDate);


        public void AddDrugCategory(DrugCategory category)
        {
            _dbContext.DrugCategories.Add(category);
            _dbContext.SaveChanges();
        }

        public void RemoveDrugCategory(int id)
        {
            _dbContext.DrugCategories.Remove(_dbContext.DrugCategories.Single(c => c.Id == id));
            _dbContext.SaveChanges();
        }

        public DrugCategory EditDrugCategory(int id) => _dbContext.DrugCategories.SingleOrDefault(d => d.Id == id);

        public void UpdateDrugCategory(DrugCategory category)
        {
           // var update = _dbContext.DrugCategories.SingleOrDefault(c => c.Id == category.Id);
            var update = _dbContext.DrugCategories.Add(category);
            _dbContext.Entry(update).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }

    }
}