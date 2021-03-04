using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace inventoryAppWebUi.Controllers
{
    public class DrugCartController : Controller
    {
        public IDrugCartService DrugCartService { get; }
        private readonly IDrugService _drugService;
        
        public DrugCartController(IDrugCartService drugCartService, IDrugService drugService)
        {
            DrugCartService = drugCartService;
            _drugService = drugService;
        }

        public ActionResult Index()
        {
            var cart = DrugCartService.GetCart(User.Identity.GetUserId());
            
            DrugCartService.GetDrugCartItems(cart.Id);
            var drugCartCountTotal = DrugCartService.GetDrugCartTotalCount(cart.Id);
            var drugCartViewModel = new DrugCartViewModel
            {
                DrugCart = cart,
                DrugCartItemsTotal = drugCartCountTotal,
                DrugCartTotal = DrugCartService.GetDrugCartTotal(cart.Id),
            };
            return View(drugCartViewModel);
        }

        public ActionResult AddToShoppingCart(int drugId, int amount)
        {
            var cart = DrugCartService.GetCart(User.Identity.GetUserId());
            var selectedDrug = DrugCartService.GetDrugById(drugId);
            if (selectedDrug == null)
            {
                return HttpNotFound();
            }

            DrugCartService.AddToCart(selectedDrug, cart.Id, amount);
            return RedirectToAction("Index");
        }


        public ActionResult RemoveFromShoppingCart(int DrugId)
        {
            var cart = DrugCartService.GetCart(User.Identity.GetUserId());
            var selectedItem = DrugCartService.GetDrugById(DrugId);

            if (selectedItem != null)
            {
                DrugCartService.RemoveFromCart(selectedItem, cart.Id);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveAllCart()
        {
            var cart = DrugCartService.GetCart(User.Identity.GetUserId());
            DrugCartService.ClearCart(cart.Id);
            return RedirectToAction("Index");
        }

    }
}
