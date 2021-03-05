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


        public void AddToCart(Drug drug, string userId, int amount)
        {
            var drugCart = GetCart(userId);
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
        public int RemoveFromCart(Drug drug, string userId)
        {
            var drugCart = GetCart(userId);
            var cartItem = _dbContext.DrugCartItems.FirstOrDefault(item => item.DrugId == drug.Id);
            if (cartItem != null)
            {
                if (cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                }
                else
                {
                    drugCart.DrugCartItems.Remove(cartItem);
                    _dbContext.Entry(drugCart).State = EntityState.Modified;
                    _dbContext.DrugCartItems.Remove(cartItem);
                }
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
            var drug = _dbContext.DrugCartItems.Include(item => item.Drug).FirstOrDefault(x => x.Id == id);
            return drug;
        }
        public List<DrugCartItem> GetDrugCartItems(string userId)
        {
            var cart = GetCart(userId);
            return _dbContext.DrugCartItems.Include(item => item.Drug).Where(item => item.DrugCartId == cart.Id).ToList();
        }
        public void ClearCart(string userId)
        {
            var cart = GetCart(userId);
            _dbContext.DrugCartItems.RemoveRange(cart.DrugCartItems);
            cart.DrugCartItems = new List<DrugCartItem>();
            _dbContext.Entry(cart).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public decimal GetDrugCartTotal(string userId)
        {
            var cart = GetCart(userId);
            if (cart.DrugCartItems.Count == 0)
            {
                return 0;
            }
            var sum = _dbContext.DrugCartItems.Where(c => c.DrugCartId == cart.Id)
                .Select(c => c.Amount == 0 ? 0 : c.Drug.Price * c.Amount).Sum();
            return sum;
        }
        public int GetDrugCartTotalCount(string userId)
        {
            var cart = GetCart(userId);
            var total = _dbContext.DrugCartItems.Count(c => c.DrugCartId == cart.Id);
            return total;
        }
    }
}