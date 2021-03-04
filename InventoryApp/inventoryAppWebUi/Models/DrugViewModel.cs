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
        [Required]
        public string DrugName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public string SupplierTag { get; set; }
        [Required]
        public List<DrugCategory> DrugCategory { get; set; }
        [Required]
        public int DrugCategoryId { get; set; }
    }
}