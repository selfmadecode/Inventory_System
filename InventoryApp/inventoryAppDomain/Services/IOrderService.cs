using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Services
{
    public interface IOrderService
    {
        void CreateOrder(Order order, string cartId);
    }
}
