using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class DrugCartService : IDrugCartService
    {

        private ApplicationDbContext _dbContext;
        private ApplicationUserManager userManager;

        public DrugCartService()
        {
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
        }

        public DrugCart CreateCart(string userId)
        {
            var user = userManager.Users.FirstOrDefault(applicationUser => applicationUser.Id == userId);
            var cart = new DrugCart()
            {
                ApplicationUser = user,
                ApplicationUserId = userId,
                DrugCartItems = new List<DrugCartItem>()
            };

            _dbContext.DrugCarts.Add(cart);
            _dbContext.SaveChanges();

            return cart;
        }

        public DrugCart GetCart(string userId)
        {
            var user = userManager.Users.FirstOrDefault(applicationUser => applicationUser.Id == userId);

            if (user != null)
                return _dbContext.DrugCarts.Include(cart => cart.DrugCartItems)
                    .FirstOrDefault(cart => cart.ApplicationUserId == userId);
            
            return null;
        }


        public void AddToCart(Drug drug, string cartId, int amount)
        {
            var drugCart = GetCart(cartId);
            var cartItem = _dbContext.DrugCartItems.FirstOrDefault(item => item.DrugId == drug.Id);
            if (cartItem == null)
            {
                cartItem = new DrugCartItem
                {
                    DrugCartId = drugCart.Id,
                    DrugCart = drugCart,
                    Drug = drug,
                    Amount = 1
                };
                _dbContext.DrugCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Amount++;
            }
            _dbContext.SaveChanges();
        }
        public int RemoveFromCart(Drug drug, string cartId)
        {
            var drugCart = GetCart(cartId);
            var cartItem = _dbContext.DrugCartItems.FirstOrDefault(item => item.DrugId == drug.Id);
            if (cartItem != null)
            {
                cartItem.Amount--;
            }
            _dbContext.SaveChanges();
            if (cartItem != null) return cartItem.Amount;
            return 0;
        }

        public Drug GetDrugById(int Id)
        {
            var drug = _dbContext.Drugs.FirstOrDefault(x => x.Id == Id);
            return drug;
        }

        public DrugCartItem GetDrugCartItemById(int id)
        {
            var drug = _dbContext.DrugCartItems.FirstOrDefault(x => x.Id == id);
            return drug;
        }
        public List<DrugCartItem> GetDrugCartItems(string cartId)
        {
            var cart = GetCart(cartId);
            return _dbContext.DrugCartItems.Where(item => item.DrugCartId == cartId).ToList();
        }
        public void ClearCart(string cartId)
        {
            var cart = GetCart(cartId);
            cart.DrugCartItems = new List<DrugCartItem>();
            _dbContext.Entry(cart).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public decimal GetDrugCartTotal(string cartId)
        {
            var sum = _dbContext.DrugCartItems.Where(c => c.DrugCartId == cartId)
                .Select(c => c.Drug.Price * c.Amount).Sum();
            return sum;
        }
        public int GetDrugCartTotalCount(string cartId)
        {
            var total = _dbContext.DrugCartItems.Count(c => c.DrugCartId == cartId);
            return total;
        }
    }
}