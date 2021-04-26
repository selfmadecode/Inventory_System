﻿using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
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

            return PartialView("_SupplierPartial", new SupplierViewModel());
        }

        [HttpPost]
        public ActionResult Save(SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
            {
                TempData["failed"] = "failed";
                Response.StatusCode = 201;
                return PartialView("_SupplierPartial", supplier);
            }

            //Add new supplier
            if (supplier.Id == 0)
            {
                var allSuppliers = _supplierService.GetAllSuppliers();
                var tagAlreadyExists = allSuppliers.Any(s => s.TagNumber == supplier.TagNumber);
                if (tagAlreadyExists)
                {
                    ModelState.AddModelError("Supplier Tag", "Supplier with this tag already exists");
                    return View("AddSupplier", supplier);
                }
                else
                {
                    var newSupplier = Mapper.Map<SupplierViewModel, Supplier>(supplier);
                    _supplierService.AddSupplier(Mapper.Map<SupplierViewModel, Supplier>(supplier));
                    TempData["supplierAdded"] = "added";
                }

                
            }
            else
            {
                //Update the existing supplier in DB
                var supplierInDb = _supplierService.FindSupplier(supplier.Id);
               _supplierService.UpdateSupplier(Mapper.Map(supplier, supplierInDb));
                TempData["supplierAdded"] = "added";
            }
            // return RedirectToAction("AllSuppliers");
            return Json(new { response = "success" }, JsonRequestBehavior.AllowGet);
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

            return PartialView("_SupplierPartial", supplier);
        }

        public ActionResult SupplierAndDrugDetails(int id)
        {
            var supplier = Mapper.Map<SupplierViewModel>(_supplierService.FindSupplier(id));

            if (supplier == null)
                return HttpNotFound("Supplier not found");
            var drugsBySupplier = Mapper.Map<IEnumerable<DrugViewModel>>(_supplierService.GetAllDrugsBySupplier(supplier.TagNumber));
            
            var supplierAndDrugs = new SupplierAndDrugsViewModel
            {
                SupplierViewModel = supplier,
                DrugViewModel = drugsBySupplier
            };

            return View("SupplierDetails", supplierAndDrugs);
        }

        public ActionResult GetDrugsBySupplier(string supplierTag)
        {
            var drugsBySupplier = Mapper.Map<IEnumerable<DrugViewModel>>(_supplierService.GetAllDrugsBySupplier(supplierTag));

            if (drugsBySupplier == null)
                return HttpNotFound("Not Found");

            return View(drugsBySupplier);
        }

    }
}