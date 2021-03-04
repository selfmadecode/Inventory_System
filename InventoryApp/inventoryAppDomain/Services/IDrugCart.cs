using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Services
{
    public interface IDrugCart
    {
        string Id { get; set; }

        void AddToCart(Drug drug, int amount);
        void ClearCart();
        List<DrugCartItem> GetDrugCartItems();
        int RemoveFromCart(Drug drug);
        //Task<(int ItemCount, decimal TotalAmmount)> GetCartCountAndTotalAmmountAsync();
        decimal GetDrugCartTotal();
        int GetDrugCartTotalCount();
        Drug GetDrugById(int Id);

    }
}
