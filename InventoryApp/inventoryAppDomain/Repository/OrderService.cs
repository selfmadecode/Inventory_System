using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class OrderService : IOrderService
    {
        public IDrugCartService DrugCartService { get; }
        private readonly ApplicationDbContext _ctx;
        
        public OrderService(IDrugCartService drugCartService)
        {
            DrugCartService = drugCartService;
        }

        public OrderService()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public void CreateOrder(Order order, string userId)
        {
            var cart = DrugCartService.GetCart(userId);
            order.OrderItems = cart.DrugCartItems;
            _ctx.Order.Add(order);
            _ctx.SaveChanges();
        }
    }
}
