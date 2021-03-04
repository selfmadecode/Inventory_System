using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order

        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _ctx;
        private readonly IDrugCart _drugCart;
        public OrderController(IOrderService orderService, IDrugCart drugCart, ApplicationDbContext ctx)
        {
            _orderService = orderService;
            _drugCart = drugCart;
            _ctx = ctx;
        }

        public OrderController()
        {

        }

        //[Authorize]
        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        //[Authorize]
        public ActionResult Checkout(Order order)
        {
            var items = _drugCart.GetDrugCartItems();


            if (_ctx.DrugCartItems.Count() == 0)
            {
                //ModelState.AddModelError("", "");
                ModelState.AddModelError("", "Your cart is empty, add some food first");
            }

            if (ModelState.IsValid)
            {
                _orderService.CreateOrder(order);
                _drugCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);

        }

        public ActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = HttpContext.User.Identity.Name +
                                      " thanks for your order. You'll soon enjoy our delicious burgers!";
            return View();
        }
    }
}