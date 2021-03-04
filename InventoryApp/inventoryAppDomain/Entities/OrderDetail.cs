using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int DrugId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public virtual Drug Drug { get; set; }
        public virtual Order Order { get; set; }
    }
}
