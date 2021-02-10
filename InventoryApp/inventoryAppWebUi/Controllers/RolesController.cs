using inventoryAppDomain.IdentityEntities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    public class RolesController : Controller
    {
        //private readonly ApplicationRoleManager roleManager;
        private ApplicationDbContext _ctx;

        public RolesController()
        {
            _ctx = new ApplicationDbContext();
        }
        // GET: Roles
        public ActionResult Index()
        {
            var roles = _ctx.Roles.ToList();
            return View(roles);
        }

        // GET: Roles/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Roles/Create
        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View(role);
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                // TODO: Add insert logic here
                _ctx.Roles.Add(role);
                _ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

  

        // GET: Roles/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    var role = _ctx.Roles.FirstOrDefault(x => x.Id == id);
            
        //    return View();
        //}

        //// POST: Roles/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
