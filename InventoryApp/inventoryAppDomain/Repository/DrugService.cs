using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public void RemoveDrug(int id)
        {
            _dbContext.Drugs.Remove(_dbContext.Drugs.Single(d => d.Id == id));
            _dbContext.SaveChanges();
        }

        public Drug EditDrug(int id) => _dbContext.Drugs.SingleOrDefault(d => d.Id == id);

        public IEnumerable<Drug> DispensedDrugs(string time)
        {
            var today = DateTime.Today.ToShortDateString();

            var thisWeek = DateTime.Now.AddDays(7).AddSeconds(-1);

            var thisMonth = DateTime.Now.AddMonths(1);

            var drug = GetAllDrugs();

            switch (time)
            {
                case "weekly":
                    return drug.Where(d => d.ExpiryDate <= thisWeek && );

                case "monthly":\
                    return drug.Where(d => d.ExpiryDate == thisMonth);

                default:
                    return drug.Where(d => d.ExpiryDate == thisWeek);
            }

        }


       

    }
}