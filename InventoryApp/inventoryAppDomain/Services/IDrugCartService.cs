using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Services
{
    public interface IDrugCartService
    {
        DrugCart CreateCart(string userId);
        DrugCart GetCart(string userId);
        void AddToCart(Drug drug, string cartId, int amount);
        void ClearCart(string cartId);
        List<DrugCartItem> GetDrugCartItems(string cartId);
        int RemoveFromCart(Drug drug, string cartId);
        //Task<(int ItemCount, decimal TotalAmmount)> GetCartCountAndTotalAmmountAsync();
        decimal GetDrugCartTotal(string cartId);
        int GetDrugCartTotalCount(string cartId);
        Drug GetDrugById(int id);
        DrugCartItem GetDrugCartItemById(int id);

    }
}
