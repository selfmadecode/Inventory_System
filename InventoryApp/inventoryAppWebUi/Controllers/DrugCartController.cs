using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    public class DrugCartController : Controller
    {
        private readonly IDrugCart _drugCart;
        private readonly IDrugService _drugService;

        public DrugCartController()
        {

        }

        public DrugCartController(IDrugCart drugCart, IDrugService drugService)
        {
            _drugCart = drugCart;
            _drugService = drugService;
        }

        public ActionResult Index()
        {
             _drugCart.GetDrugCartItems();
            var drugCartCountTotal = _drugCart.GetDrugCartTotalCount();
            var drugCartViewModel = new DrugCartViewModel
            {
                DrugCart = (inventoryAppDomain.Entities.DrugCart)_drugCart,
                DrugCartItemsTotal = drugCartCountTotal,
                DrugCartTotal = _drugCart.GetDrugCartTotal(),
            };
            return View(drugCartViewModel);
        }

        public ActionResult AddToShoppingCart(int drugId, int amount)
        {
            var selectedDrug = _drugCart.GetDrugById(drugId);
            if (selectedDrug == null)
            {
                return HttpNotFound();
            }

            _drugCart.AddToCart(selectedDrug, amount);
            return RedirectToAction("Index");
        }


        public ActionResult RemoveFromShoppingCart(int DrugId)
        {
            var selectedItem = _drugCart.GetDrugById(DrugId);

            if (selectedItem != null)
            {
                _drugCart.RemoveFromCart(selectedItem);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveAllCart()
        {
            _drugCart.ClearCart();
            return RedirectToAction("Index");
        }

    }
}
