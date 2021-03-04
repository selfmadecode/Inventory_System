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

            _drugService.AddDrug(Mapper.Map<DrugViewModel, Drug>(newDrug));

            return View("AddDrugForm");
        }
    }
}