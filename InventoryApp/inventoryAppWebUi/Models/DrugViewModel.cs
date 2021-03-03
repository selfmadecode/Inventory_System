using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;


namespace inventoryAppWebUi.Models
{
    public class DrugViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DrugStatus CurrentDrugStatus { get; set; } = DrugStatus.NOT_EXPIRED;
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public DrugCategory DrugCategoryName { get; set; }
        public List<DrugCategory> DrugCategory { get; set; }
    }
}