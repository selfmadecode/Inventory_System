using AutoMapper;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppWebUi.Models;

namespace inventoryAppWebUi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DrugController : Controller
    {
        private readonly IDrugService _drugService;
        private readonly ISupplierService _supplierService;
        
        public DrugController(IDrugService drugService, ISupplierService supplierService)
        {
            _drugService = drugService;
            _supplierService = supplierService;

        }
        // GET: Drug
        public ActionResult AllDrugs()
        {
            return View(_drugService.GetAllDrugs());
        }
        public ActionResult AvailableDrugs()
        {
            return View(_drugService.GetAvailableDrugs());
        }



        public ActionResult AddDrugForm()
        {
            var drugCategory = new DrugViewModel()
            {
                DrugCategory = _drugService.AllCategories()

            };
            return View(drugCategory);
        }

        public ActionResult UpdateDrug(DrugViewModel drug)
        {

            var drugInDb = Mapper.Map<DrugViewModel>(_drugService.EditDrug(drug.Id));

            if (drugInDb == null) return HttpNotFound("No drug found");

            drugInDb.DrugCategory = _drugService.AllCategories();

            return View("AddDrugForm", drugInDb);
        }

        public ActionResult SaveDrug(DrugViewModel drug)
        {
            if (!ModelState.IsValid)
            {
                drug.DrugCategory = _drugService.AllCategories();
                return View("AddDrugForm", drug);
            }


            try
            {
                var supplierInDb = _supplierService.GetSupplierWithTag(drug.SupplierTag);

                if (supplierInDb == null)
                {
                    //If the supplier tag is not in the Db
                    ModelState.AddModelError("SupplierTag", "Supplier Tag isn't registered yet");
                    drug.DrugCategory = _drugService.AllCategories();
                    return View("AddDrugForm", drug);
                }
                else
                {
                    //Add a new drug
                    if (drug.Id == 0)
                    {
                        var expiryDate = _drugService.DateComparison(DateTime.Today, drug.ExpiryDate);

                        if (expiryDate >= 0)
                        {
                            ModelState.AddModelError("ExpiryDate", "Must be later than today");
                            drug.DrugCategory = _drugService.AllCategories();
                            return View("AddDrugForm", drug);
                        }
                         _drugService.AddDrug(Mapper.Map<DrugViewModel, Drug>(drug));
                    }
                    else
                    {
                        // update existing drug
                        //NOTE
                        // check expiry date for drugs
                        var getDrugInDb = _drugService.EditDrug(drug.Id);
                        _drugService.UpdateDrug(Mapper.Map(drug, getDrugInDb));
                    }
                }
            }
            catch (Exception)
            {

                throw new HttpException("Something went wrong");
            }
            
            return RedirectToAction("AddDrugForm");
        }

        //Get
        [HttpGet]
        public ActionResult AddDrugCategory()
        {
            return View();
        }

        //Post
        [HttpPost]
        public ActionResult SaveDrugCategory(DrugCategory category)
        {
            if (ModelState.IsValid)
            {
                _drugService.AddDrugCategory(category);
                TempData["Category"] = "Category successfully added";
                return View("AddDrugCategory");
            }
            return View("AddDrugCategory");
        }

        public ActionResult RemoveDrug(int id)
        {
            _drugService.RemoveDrug(id);

            return RedirectToAction("AllDrugs");
        }

        public ActionResult ListDrugCategories()
        {
            return View(_drugService.AllCategories());
        }

        public ActionResult RemoveDrugCategory(int id)
        {
            _drugService.RemoveDrugCategory(id);
            return RedirectToAction("ListDrugCategories");
        }
    }
}