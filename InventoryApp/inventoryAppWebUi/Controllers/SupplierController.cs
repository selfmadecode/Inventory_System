using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly ApplicationDbContext _dbContext;

        public SupplierController(ISupplierService supplierService, ApplicationDbContext dbContext)
        {
            _supplierService = supplierService;
            _dbContext = dbContext;
        }
        public ActionResult AllSuppliers()
        {
            var suppliers = Mapper.Map<IEnumerable<SupplierViewModel>>(_supplierService.GetAllSuppliers());
            
            return View(suppliers);
        }
        // GET
        public ActionResult AddSupplier()
        {
            ViewBag.TagNumber = _supplierService.GenerateTagNumber();

            return View(new SupplierViewModel());
        }

        public ActionResult Save(SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
                return View("AddSupplier", supplier);

            //Add new supplier
            if (supplier.Id == 0)
            {
                var newSupplier = Mapper.Map<SupplierViewModel, Supplier>(supplier);
                _dbContext.Entry(newSupplier).State = EntityState.Added;

            }
            else
            {
                //Update the existing supplier in DB
                var supplierInDb = _supplierService.FindSupplier(supplier.Id);

                var update = Mapper.Map(supplier, supplierInDb);

                _dbContext.Entry(update).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();

            return RedirectToAction("AllSuppliers");
        }

        public ActionResult ProcessSupplier(int id)
        {
            var supplier = _supplierService.FindSupplier(id);
            

            if(supplier.Status == SupplierStatus.Active)
                supplier.Status = SupplierStatus.InActive;
            else
                supplier.Status = SupplierStatus.Active;

            bool processSupplier = _supplierService.ProcessSupplier(id, supplier.Status);

            if (processSupplier == true)
                return RedirectToAction("AllSuppliers");

            return HttpNotFound("No Supplier found!");
        }

        public ActionResult EditSupplier(int id)
        {
            var supplier = Mapper.Map<SupplierViewModel>(_supplierService.FindSupplier(id));

            if (supplier == null)
                return HttpNotFound("Supplier not found");

            return View("AddSupplier", supplier);
        }

        public ActionResult SupplierDetails(int id)
        {
            var supplier = Mapper.Map<SupplierViewModel>(_supplierService.FindSupplier(id));

            if (supplier == null)
                return HttpNotFound("Supplier not found");
            
            return View(supplier);
        }

    }
}