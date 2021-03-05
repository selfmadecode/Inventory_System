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
            var userId = User.Identity.GetUserId();
            
            DrugCartService.GetDrugCartItems(userId);
            var drugCartCountTotal = DrugCartService.GetDrugCartTotalCount(userId);
            var drugCartViewModel = new DrugCartViewModel
            {
                CartItems = DrugCartService.GetDrugCartItems(userId),
                DrugCartItemsTotal = drugCartCountTotal,
                DrugCartTotal = DrugCartService.GetDrugCartTotal(userId),
            };
            return View(drugCartViewModel);
        }

        public ActionResult AddToShoppingCart(int id)
        {
            var userId = User.Identity.GetUserId();
            var selectedDrug = DrugCartService.GetDrugById(id);
            if (selectedDrug == null)
            {
                return HttpNotFound();
            }

            DrugCartService.AddToCart(selectedDrug, userId, selectedDrug.Quantity);
            return RedirectToAction("Index");
        }


        public ActionResult RemoveFromShoppingCart(int id)
        {
            var userId = User.Identity.GetUserId();
            var cartItem = DrugCartService.GetDrugCartItemById(id);
            var selectedItem = DrugCartService.GetDrugById(cartItem.Drug.Id);

            if (selectedItem != null)
            {
                DrugCartService.RemoveFromCart(selectedItem, userId);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveAllCart()
        {
            var userId = User.Identity.GetUserId();
            DrugCartService.ClearCart(userId);
            return RedirectToAction("Index");
        }

    }
}
