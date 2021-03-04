using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Entities
{
    public class DrugCartItem
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public int DrugId { get; set; }

        public Drug Drug { get; set; }


        [Required]
        [StringLength(255)]
        public string DrugCartId { get; set; }
    }
}
