using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.ExtensionMethods;
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
            _ctx = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public void CreateOrder(Order order, string userId)
        {
            var cart = DrugCartService.GetCart(userId,CartStatus.ACTIVE);
            order.OrderItems = cart.DrugCartItems;
            order.Price = DrugCartService.GetDrugCartTotal(userId);
            _ctx.Orders.Add(order);
            cart.CartStatus = CartStatus.MOST_RECENT;
            _ctx.Entry(cart).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        public List<Order> GetOrdersForTheDay()
        {
            return _ctx.Orders.Include(order => order.OrderItems).Where(order => DbFunctions.TruncateTime(order.CreatedAt) == DbFunctions.TruncateTime(DateTime.Now)).ToList();
        }

        public List<Order> GetOrdersForTheWeek()
        {
            var beginningOfWeek = DateTime.Now.FirstDayOfWeek();
            var lastDayOfTheWeek = DateTime.Now.LastDayOfWeek();

            return _ctx.Orders.Include(order => order.OrderItems).Where(order => DateTime.Now.Month == order.CreatedAt.Month
                    && DateTime.Now.Year == order.CreatedAt.Year)
                .Where(order => order.CreatedAt >= beginningOfWeek && order.CreatedAt < lastDayOfTheWeek)
                .ToList();
        }

        public List<Order> GetOrdersForTheMonth()
        {
            return _ctx.Orders.Include(order => order.OrderItems).Where(order => order.CreatedAt.Month.Equals(DateTime.Now.Month) && 
                                              order.CreatedAt.Year.Equals(DateTime.Now.Year))
                .ToList();
        }
    }
}
