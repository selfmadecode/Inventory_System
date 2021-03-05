using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Repository
{
    public class OrderService : IOrderService
    {
        public IDrugCartService DrugCartService { get; }
        private readonly ApplicationDbContext _ctx;
        public OrderService(IDrugCartService drugCartService, ApplicationDbContext ctx)
        {
            DrugCartService = drugCartService;
            _ctx = ctx;
        }

        public void CreateOrder(Order order, string userId)
        {
            order.OrderPlaced = DateTime.Now;

            _ctx.Order.Add(order);
            var drugCartItems = DrugCartService.GetDrugCartItems(userId);

            foreach (var drugCartItem in drugCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = drugCartItem.Amount,
                    DrugId = drugCartItem.Drug.Id,
                    OrderId = order.OrderId,
                    Price = drugCartItem.Drug.Price
                };

                _ctx.OrderDetails.Add(orderDetail);
            }

            _ctx.SaveChanges();
        }
    }
}
