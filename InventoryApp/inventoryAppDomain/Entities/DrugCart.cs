using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Entities
{
    public class DrugCart : IDrugCart
    {
        
        private ApplicationDbContext _appDbContext;
        public const string CartSessionKey = "CartId";
        private DrugCart(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string DrugCartId { get; set; }
        public List<DrugCartItem> DrugCartItems { get; set; }
        public string Id { get; set; }
        //public IEnumerable<DrugCartItem> DrugCartItems { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static DrugCart GetCart(IServiceProvider services, ApplicationDbContext appDbContext, string cartId)
        {
            //var httpContext = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
            //var context = services.GetRequiredService<ApplicationDbContext>();

            //var request = httpContext.Request;
            //var response = httpContext.Response;

            //var cardId = request.Cookies["CartId-cookie"] ?? Guid.NewGuid().ToString();

            //response.Cookies.Append("CartId-cookie", cardId, new CookieOptions
            //{
            //    Expires = DateTime.Now.AddMonths(2)
            //});

            var cart = appDbContext.DrugCartItems.FirstOrDefault(x => x.DrugCartId == cartId);
        
            return new DrugCart(appDbContext)
            {
                Id = cart.DrugCartId
            };
        }

        public void AddToCart(Drug drug, int amount)
        {
            var DrugCartItem =
                 _appDbContext.DrugCartItems.SingleOrDefault(
                     s => s.Drug.Id == drug.Id && s.DrugCartId == DrugCartId);

            if (DrugCartItem == null)
            {
                DrugCartItem = new DrugCartItem
                {
                    DrugCartId = DrugCartId,
                    Drug = drug,
                    Amount = 1
                };
                _appDbContext.DrugCartItems.Add(DrugCartItem);
            }
            else
            {
                DrugCartItem.Amount++;
            }
            _appDbContext.SaveChanges();
        }
        public int RemoveFromCart(Drug drug)
        {
            var DrugCartItem =
                _appDbContext.DrugCartItems.SingleOrDefault(
                    s => s.Drug.Id == drug.Id && s.DrugCartId == DrugCartId);
            var localAmount = 0;
            if (DrugCartItem != null)
            {
                if(DrugCartItem.Amount > 1)
                {
                    DrugCartItem.Amount--;
                    localAmount = DrugCartItem.Amount;
                }
                else
                {
                    _appDbContext.DrugCartItems.Remove(DrugCartItem);
                }
            }

            _appDbContext.SaveChanges();
            return localAmount;
        }

        public Drug GetDrugById(int Id)
        {
            var drugCartItem = _appDbContext.Drugs.FirstOrDefault(x => x.Id == Id);
            return drugCartItem;
        }

        public List<DrugCartItem> GetDrugCartItems()
        {
            return DrugCartItems ??
                (DrugCartItems =
                _appDbContext.DrugCartItems.Where(c => c.DrugCartId == DrugCartId)
                .Include(s => s.Drug)
                .ToList());
        }
        public void ClearCart()
        {
            var cartItems = _appDbContext
                 .DrugCartItems
                 .Where(cart => cart.DrugCartId == DrugCartId);

            _appDbContext.DrugCartItems.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }
        public decimal GetDrugCartTotal()
        {
            var total = _appDbContext.DrugCartItems.Where(c => c.DrugCartId == DrugCartId)
                .Select(c => c.Drug.Price * c.Amount).Sum();
            return total;
        }
        public int GetDrugCartTotalCount()
        {
            var total = _appDbContext.DrugCartItems.Where(c => c.DrugCartId == DrugCartId)
                .Count();
            return total;
        }
    }
}
