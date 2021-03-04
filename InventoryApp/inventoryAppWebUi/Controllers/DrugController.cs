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
        private readonly ApplicationDbContext _dbContext;
        
        public DrugController(IDrugService drugService, ApplicationDbContext dbContext)
        {
            _drugService = drugService;
            _dbContext = dbContext;

        }
        // GET: Drug
        public ActionResult AllDrugs()
        {
            return View(_drugService.GetAllDrugs());
        }

        //public ActionResult AddDrug()
        //{


        //    return View(new DrugViewModel());
        //}

        

        public ActionResult AddDrugForm()
        {
            var drugCategory = new DrugViewModel()
            {
                DrugCategory = _drugService.AllCategories()

            };
            return View(drugCategory);
        }

        public ActionResult SaveDrug(DrugViewModel newDrug)
        {
            if (!ModelState.IsValid)
            {
                newDrug.DrugCategory = _drugService.AllCategories();
                return View("AddDrugForm", newDrug);
            }

            //var today = DateTime.Today;
            //var expiryDate = DateTime.Compare(today, newDrug.ExpiryDate);
            var expiryDate = _drugService.DateComparison(DateTime.Today, newDrug.ExpiryDate);

            if (expiryDate >= 0)
            {
                ModelState.AddModelError("ExpiryDate", "Must be later than today");
                newDrug.DrugCategory = _drugService.AllCategories();
                return View("AddDrugForm", newDrug);
            }
                _drugService.AddDrug(Mapper.Map<DrugViewModel, Drug>(newDrug));

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
    }
}