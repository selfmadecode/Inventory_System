using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.Entities.Enums;
using inventoryAppWebUi.Models;
using Microsoft.AspNet.Identity;

namespace inventoryAppWebUi.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IDrugCartService DrugCartService { get; }
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService, IDrugCartService drugCartService)
        {
            DrugCartService = drugCartService;
            _orderService = orderService;
        }
        
        public ActionResult Invoice()
        {
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Checkout(OrderViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();
            var items = DrugCartService.GetDrugCartItems(userId,CartStatus.ACTIVE);


            if (!items.Any())
            {
                //ModelState.AddModelError("", "");
                ModelState.AddModelError("", @"Your cart is empty");
            }

            if (ModelState.IsValid)
            {
                _orderService.CreateOrder(Mapper.Map<OrderViewModel, Order>(viewModel), userId);
                DrugCartService.RefreshCart(userId);
                return RedirectToAction("CheckoutComplete");
            }
            return View("Invoice",viewModel);

        }

        public ActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Drug Dispensed";
            return View("CheckoutComplete");
        }
    }
}