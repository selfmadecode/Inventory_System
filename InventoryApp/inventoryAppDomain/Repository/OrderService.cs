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
        private readonly IDrugCart _drugCart;
        private readonly ApplicationDbContext _ctx;
        public OrderService(IDrugCart drugCart, ApplicationDbContext ctx)
        {
            _ctx = ctx;
            _drugCart = drugCart;
        }

        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            _ctx.Order.Add(order);

            var drugCartItems = _drugCart.GetDrugCartItems();

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
