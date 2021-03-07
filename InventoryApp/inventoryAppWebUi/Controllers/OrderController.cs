using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace inventoryAppWebUi.Controllers
{
    public class OrderController : Controller
    {
        public IDrugCartService DrugCartService { get; }
        // GET: Order

        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService, IDrugCartService drugCartService)
        {
            DrugCartService = drugCartService;
            _orderService = orderService;
        }
        

        //[Authorize]
        public ActionResult Invoice()
        {
            return View();
        }
        [HttpPost]
        //[Authorize]
        public ActionResult Checkout(Order order)
        {
            var userId = User.Identity.GetUserId();
            var items = DrugCartService.GetDrugCartItems(userId);


            if (!items.Any())
            {
                //ModelState.AddModelError("", "");
                ModelState.AddModelError("", @"Your cart is empty");
            }

            if (ModelState.IsValid)
            {
                _orderService.CreateOrder(order, userId);
                DrugCartService.ClearCart(userId);
                return RedirectToAction("CheckoutComplete");
            }
            return View("Invoice", order);

        }

        public ActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = HttpContext.User.Identity.Name +
                                      " thanks for your order!";
            return View("CheckoutComplete");
        }
    }
}