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
        private readonly ApplicationDbContext _dbContext;
        
        public DrugController(IDrugService drugService, ISupplierService supplierService, ApplicationDbContext dbContext)
        {
            _drugService = drugService;
            _supplierService = supplierService;
            _dbContext = dbContext;

        }
        // GET: Drug
        public ActionResult AllDrugs()
        {
            return View(_drugService.GetAllDrugs());
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
                        _drugService.AddDrug(Mapper.Map<DrugViewModel, Drug>(drug));

                    else
                    {
                        // update existing drug
                        var getDrugInDb = _drugService.EditDrug(drug.Id);
                        var updateDrugInDb = Mapper.Map(drug, getDrugInDb);
                        _dbContext.Entry(updateDrugInDb).State = EntityState.Modified;
                    }
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw new HttpException("Something went wrong");
            }
            
            return RedirectToAction("AddDrugForm");
        }

    }
}