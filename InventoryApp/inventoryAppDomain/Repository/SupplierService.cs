using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace inventoryAppDomain.Repository
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _dbContext;

        public SupplierService() => _dbContext = HttpContext.Current.GetOwinContext()
            .Get<ApplicationDbContext>();

        public void AddSupplier(Supplier supplier)
        {
            var newSupplier = _dbContext.Suppliers.Add(supplier);
            _dbContext.Entry(newSupplier).State = EntityState.Added;
            _dbContext.SaveChanges();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            var updateSupplier = _dbContext.Suppliers.Add(supplier);
            _dbContext.Entry(updateSupplier).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }

        public IEnumerable<Supplier> GetAllSuppliers() => 
            _dbContext.Suppliers
            .OrderBy(s =>s.Status)
            .ToList();

        public bool ProcessSupplier(int id, SupplierStatus condition)
        {
            var supplier = FindSupplier(id);

            if (supplier == null)
                return false;
            else
            {
                supplier.Status = condition;
                _dbContext.SaveChanges();
            }
            return true;
        }


        public Supplier FindSupplier(int id) => _dbContext.Suppliers.SingleOrDefault(s => s.Id == id);

        private static Random random = new Random();
        public string GenerateTagNumber()
        {
            int length = 7;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return "TAG-" + new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int TotalNumberOfSupplier() => _dbContext.Suppliers.Count();

        public Supplier GetSupplierWithTag(string tag) => _dbContext.Suppliers.SingleOrDefault(s => s.TagNumber == tag);

    }
}
