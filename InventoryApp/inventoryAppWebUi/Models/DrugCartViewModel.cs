using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inventoryAppWebUi.Models
{
    public class DrugCartViewModel
    {
        public DrugCart DrugCart { get; set; }
        public decimal DrugCartTotal { get; set; }
        public int DrugCartItemsTotal { get; set; }
        public int DrugId { get; set; }
    }
}